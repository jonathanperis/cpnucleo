import { diag, DiagConsoleLogger, DiagLogLevel, metrics, trace } from '@opentelemetry/api';
import { logs, SeverityNumber } from '@opentelemetry/api-logs';
import { getNodeAutoInstrumentations } from '@opentelemetry/auto-instrumentations-node';
import { OTLPLogExporter } from '@opentelemetry/exporter-logs-otlp-http';
import { OTLPMetricExporter } from '@opentelemetry/exporter-metrics-otlp-http';
import { OTLPTraceExporter } from '@opentelemetry/exporter-trace-otlp-http';
import { resourceFromAttributes } from '@opentelemetry/resources';
import { BatchLogRecordProcessor } from '@opentelemetry/sdk-logs';
import { PeriodicExportingMetricReader } from '@opentelemetry/sdk-metrics';
import { NodeSDK } from '@opentelemetry/sdk-node';
import { hostname } from 'node:os';
import { version as processVersion } from 'node:process';

const serviceName = process.env.OTEL_SERVICE_NAME ?? 'WebClient-Cpnucleo';
const serviceVersion = process.env.npm_package_version ?? '0.1.0';
const deploymentEnvironment = process.env.NODE_ENV ?? process.env.ASPNETCORE_ENVIRONMENT ?? 'Production';
const exportIntervalMillis = Number(process.env.OTEL_METRIC_EXPORT_INTERVAL ?? '30000');

const normalizeOtlpHttpEndpoint = () => {
  const explicitHttpEndpoint = process.env.OTEL_EXPORTER_OTLP_HTTP_ENDPOINT;
  if (explicitHttpEndpoint) return explicitHttpEndpoint.replace(/\/$/, '');

  const endpoint = process.env.OTEL_EXPORTER_OTLP_ENDPOINT ?? 'http://localhost:4318';
  return endpoint.replace(':4317', ':4318').replace(/\/$/, '');
};

const otlpHttpEndpoint = normalizeOtlpHttpEndpoint();
const makeSignalUrl = (signal) => `${otlpHttpEndpoint}/v1/${signal}`;

if (process.env.OTEL_DIAGNOSTICS === 'true') {
  diag.setLogger(new DiagConsoleLogger(), DiagLogLevel.INFO);
}

const resource = resourceFromAttributes({
  'service.name': serviceName,
  'service.namespace': 'cpnucleo',
  'service.version': serviceVersion,
  'service.instance.id': hostname(),
  'deployment.environment': deploymentEnvironment,
  'host.name': hostname(),
  'process.pid': process.pid,
  'process.runtime.name': `node ${processVersion}`,
  'cpnucleo.project': 'webclient',
});

const sdk = new NodeSDK({
  resource,
  traceExporter: new OTLPTraceExporter({ url: makeSignalUrl('traces') }),
  metricReaders: [
    new PeriodicExportingMetricReader({
      exporter: new OTLPMetricExporter({ url: makeSignalUrl('metrics') }),
      exportIntervalMillis,
    }),
  ],
  logRecordProcessors: [
    new BatchLogRecordProcessor(new OTLPLogExporter({ url: makeSignalUrl('logs') })),
  ],
  instrumentations: [
    getNodeAutoInstrumentations({
      '@opentelemetry/instrumentation-fs': { enabled: false },
    }),
  ],
});

sdk.start();

const meter = metrics.getMeter('cpnucleo.webclient.http');
const requestCounter = meter.createCounter('cpnucleo.webclient.http.server.requests', {
  description: 'Total WebClient HTTP requests served by the Node static server.',
});
const responseDuration = meter.createHistogram('cpnucleo.webclient.http.server.duration', {
  description: 'WebClient HTTP server response duration in milliseconds.',
  unit: 'ms',
});
const errorCounter = meter.createCounter('cpnucleo.webclient.http.server.errors', {
  description: 'Total WebClient HTTP server errors.',
});

const tracer = trace.getTracer('cpnucleo.webclient.preview');
const logger = logs.getLogger('cpnucleo.webclient.preview');

export const startHttpRequestSpan = (request) => tracer.startSpan(`HTTP ${request.method ?? 'GET'}`, {
  attributes: {
    'http.request.method': request.method,
    'url.path': request.url?.split('?')[0] ?? '/',
    'url.query.length': request.url?.split('?')[1]?.length ?? 0,
    'user_agent.original': request.headers['user-agent'] ?? '',
    'network.protocol.name': 'http',
  },
});

export const recordHttpRequest = (request, response, startTime, span) => {
  const duration = Number(process.hrtime.bigint() - startTime) / 1_000_000;
  const attributes = {
    'http.request.method': request.method,
    'http.response.status_code': response.statusCode,
    'url.path': request.url?.split('?')[0] ?? '/',
    'user_agent.original': request.headers['user-agent'] ?? '',
  };

  requestCounter.add(1, attributes);
  responseDuration.record(duration, attributes);
  span?.setAttributes(attributes);
  span?.end();

  logger.emit({
    severityNumber: SeverityNumber.INFO,
    severityText: 'INFO',
    body: `${request.method} ${request.url} ${response.statusCode}`,
    attributes: { ...attributes, 'http.server.duration_ms': duration },
  });
};

export const recordHttpError = (request, error, span) => {
  const attributes = {
    'http.request.method': request.method,
    'url.path': request.url?.split('?')[0] ?? '/',
    'exception.type': error?.name ?? 'Error',
    'exception.message': error?.message ?? String(error),
  };

  errorCounter.add(1, attributes);
  span?.recordException(error);
  span?.setAttributes(attributes);
  span?.end();

  logger.emit({
    severityNumber: SeverityNumber.ERROR,
    severityText: 'ERROR',
    body: error?.message ?? String(error),
    attributes,
  });
};

const shutdown = async () => {
  try {
    await sdk.shutdown();
  } finally {
    process.exit(0);
  }
};

process.once('SIGTERM', shutdown);
process.once('SIGINT', shutdown);

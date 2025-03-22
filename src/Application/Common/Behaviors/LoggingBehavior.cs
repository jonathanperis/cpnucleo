namespace Application.Common.Behaviors;

internal sealed class LoggingBehavior<TMessage, TResponse>(ILogger<LoggingBehavior<TMessage, TResponse>> logger) : IPipelineBehavior<TMessage, TResponse>
    where TMessage : IMessage
{
    public async ValueTask<TResponse> Handle(TMessage message, CancellationToken cancellationToken, MessageHandlerDelegate<TMessage, TResponse> next)
    {
        try
        {
            logger.LogInformation("Starting {messageType}", message.GetType().Name);
            
            return await next(message, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error handling message of type {messageType}", message.GetType().Name);
            throw;
        }
    }
}
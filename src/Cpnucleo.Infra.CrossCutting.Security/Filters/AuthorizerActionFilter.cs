using Cpnucleo.Infra.CrossCutting.Util.Interfaces;
using JWT;
using JWT.Serializers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Cpnucleo.Infra.CrossCutting.Security.Filters
{
    public class AuthorizerActionFilter : IActionFilter
    {
        private readonly ISystemConfiguration _systemConfiguration;

        private static string Usuario;

        public AuthorizerActionFilter(ISystemConfiguration systemConfiguration)
        {
            _systemConfiguration = systemConfiguration;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            bool hasHeaderAuthorization = context.HttpContext.Request.Headers.TryGetValue("Authorization", out StringValues authorizationToken);

            if (!hasHeaderAuthorization)
            {
                throw new UnauthorizedAccessException("Acesso não autorizado, token não encontrado");
            }

            try
            {
                JsonNetSerializer serializer = new JsonNetSerializer();
                UtcDateTimeProvider provider = new UtcDateTimeProvider();
                JwtValidator validator = new JwtValidator(serializer, provider);

                JwtBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                JwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder);

                string json = decoder.Decode(authorizationToken.ToString(), _systemConfiguration.JwtKey, verify: false);

                JObject Json = JsonConvert.DeserializeObject<JObject>(json);

                Usuario = Json["unique_name"].ToString();

                if (string.IsNullOrEmpty(Usuario))
                {
                    throw new UnauthorizedAccessException("Usuário e/ou Id do dispositivo não informados");
                }
            }
            catch (TokenExpiredException)
            {
                throw new UnauthorizedAccessException("Token expirado, acesso não autorizado");
            }
            catch (SignatureVerificationException)
            {
                throw new UnauthorizedAccessException("Token inválido, acesso não autorizado");
            }
            catch (Exception)
            {
                throw new UnauthorizedAccessException("Token inválido");
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {

        }
    }
}

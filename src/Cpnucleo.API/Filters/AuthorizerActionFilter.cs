using Cpnucleo.Infra.CrossCutting.Configuration.Interfaces;
using JWT;
using JWT.Serializers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Cpnucleo.API.Filters
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
                throw new UnauthorizedAccessException("Acesso não Autorizado, Token não encontrado");
            }

            try
            {
                IJsonSerializer serializer = new JsonNetSerializer();
                IDateTimeProvider provider = new UtcDateTimeProvider();
                IJwtValidator validator = new JwtValidator(serializer, provider);

                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder);

                var json = decoder.Decode(authorizationToken.ToString(), _systemConfiguration.JwtKey, verify: false);

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

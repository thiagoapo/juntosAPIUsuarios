using System;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using JuntosBusiness.Interface;
using JuntosBusiness;

namespace APIJuntosUsuarios
{
    public class TokenConfigurations
    {
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public int Seconds { get; set; }
    }

    public class SigningConfigurations
    {
        public SecurityKey Key { get; }
        public SigningCredentials SigningCredentials { get; }

        public SigningConfigurations()
        {
            using (var provider = new RSACryptoServiceProvider(2048))
            {
                Key = new RsaSecurityKey(provider.ExportParameters(true));
            }

            SigningCredentials = new SigningCredentials(
                Key, SecurityAlgorithms.RsaSha256Signature);
        }
    }

    public class Authorization : AuthorizeAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            try
            {

                var usuarioService = (IUsuarioService)filterContext.HttpContext.RequestServices.GetService(typeof(IUsuarioService));
                var headerAuth = filterContext.HttpContext.Request.Headers["Authorization"];
                if (!headerAuth.Any())
                {
                    filterContext.Result = new UnauthorizedResult();
                    return;
                }

                var auth = headerAuth.First();
                auth = auth.Replace("Bearer", string.Empty);
                auth = auth.Replace(" ", string.Empty);
                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadJwtToken(auth);
                if (token.ValidTo < DateTime.Now)
                {
                    filterContext.Result = new UnauthorizedResult();
                    return;
                }

                Claim unique_name = token.Claims.FirstOrDefault(p => p.Type == "unique_name");
                var usuario = usuarioService.Obter(new Guid(unique_name.Value));
                if(usuario == null)
                {
                    filterContext.Result = new UnauthorizedResult();
                }
            }
            catch(Exception ex)
            {
                filterContext.Result = new UnauthorizedResult();
            }
        }
    }
}
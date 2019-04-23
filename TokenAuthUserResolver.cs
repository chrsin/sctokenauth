using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using Sitecore;
using Sitecore.Pipelines.HttpRequest;
using Sitecore.Security.Authentication;
using Sitecore.Services.Infrastructure.Web.Http.Security;

namespace MyWebsite
{
    [ExcludeFromCodeCoverage]
    public class TokenAuthUserResolver : HttpRequestProcessor
    {
        private readonly ITokenProvider _tokenProvider;

        public TokenAuthUserResolver(ITokenProvider tokenProvider)
        {
            _tokenProvider = tokenProvider;
        }
        
        public override void Process(HttpRequestArgs args)
        {
            if (Context.IsLoggedIn)
                return;

            string authorize = args.HttpContext.Request.Headers["Authorization"];

            if (string.IsNullOrEmpty(authorize))
                return;

            Match match = Regex.Match(authorize, @"^Bearer ([^\.]+\.[^\.]+\.[^\.]+)$");
            if (!match.Success)
            {
                //Maybe log this? Someone might be trying to fuck around with auth
                return;
            }

            try
            {
                Group smt = match.Groups[1];
                string token = smt.Value;

                ITokenValidationResult tokenResult = _tokenProvider.ValidateToken(token);

                if (!tokenResult.IsValid)
                    return;

                Claim usernameClaim = tokenResult.Claims.FirstOrDefault(x => x.Type == "MyWebsite.Username");
                if (usernameClaim == null)
                    return;

                AuthenticationManager.Login(usernameClaim.Value);
            }
            catch (Exception)
            {
                //If something threw it was not a valid token and we ignore it
            }
        }
    }
}
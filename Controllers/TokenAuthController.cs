using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Web.Http;
using Sitecore.Services.Core.Security;
using Sitecore.Services.Infrastructure.Web.Http.Security;

namespace MyWebsite
{
    [RoutePrefix("_api/TAuth")]
    public class TokenAuthController : ApiController
    {
        private readonly IUserService _userService;
        private readonly ITokenProvider _tokenProvider;

        public TokenAuthController(IUserService userService, ITokenProvider tokenProvider)
        {
            _userService = userService;
            _tokenProvider = tokenProvider;
        }

        [HttpPost]
        [Route("SimpleLogin")]
        public IHttpActionResult SimpleLogin(LoginParameter login)
        {
            //You should do input validation right? ;)
            _userService.Login(login.Domain, login.Username,
                login.Password); //Will throw an AuthenticationException if credentials are wrong which will be handled by the auth exception filter

            string username = $@"{login.Domain}\{login.Username}";

            List<Claim> claims = new List<Claim>
            {
                new Claim("MyWebsite.Username", username)
                /*
                normally you should always include the issuer in your claims aswell. 
                However The Sitecore TokenProvider doesn't seem to support this.
                You can add more claims to the list here which will be available for you when you get your token back.
                Just dont put anything confidential in here as it will be possible to parse it from the JWT token.
                Remember the way token auth works is that anyone can inspect the token (Anyone who can access the wire that is)
                But you can only validate the token if you have the secret. */
            };

            string token = _tokenProvider.GenerateToken(claims);
            return Ok(token);
        }

        [HttpGet]
        [Route("GetTime")]
        public IHttpActionResult GetTime()
        {
            //Here we just validate that the user isn't the anonymous user for testing purposes.
            if (_userService.IsAnonymousUser)
                throw new AuthorizationException("User isn't authenticated");

            return Ok(DateTime.Now.ToLongTimeString());
        }
    }

    public class LoginParameter
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Domain { get; set; }
    }
}
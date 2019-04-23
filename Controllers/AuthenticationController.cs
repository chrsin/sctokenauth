using System;
using System.Collections.Generic;
using System.Security.Authentication;
using System.Security.Claims;
using System.Web.Http;
using Sitecore.Services.Core.Security;
using Sitecore.Services.Infrastructure.Web.Http.Security;

namespace MyWebsite
{
    public class AuthenticationController: ApiController
    {
        private readonly IUserService _userService;
        private readonly ITokenProvider _tokenProvider;

        public AuthenticationController(IUserService userService,
            ITokenProvider tokenProvider)
        {
            _userService = userService;
            _tokenProvider = tokenProvider;
        }

        [HttpPost]
        public IHttpActionResult Login(LoginParameters login)
        {
            try
            {
                _userService.Login(login.Domain, login.Username, login.Password);
                List<Claim> claims = new List<Claim>
                {
                    new Claim("username", login.Username, ClaimValueTypes.String, "MyWebsite")
                };

                string token = _tokenProvider.GenerateToken(claims);
                return Ok(new { Token = token});

            }
            catch (AuthenticationException e)
            {
                //Rethrow and let the authentication exception handler handle this 
                throw e;
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        public class LoginParameters
        {
            public string Domain { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
        }
    }
}
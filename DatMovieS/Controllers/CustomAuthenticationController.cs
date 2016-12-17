using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.Azure.Mobile.Server.Login;
using DatMovieS.DataObjects;
using Microsoft.Azure.Mobile.Server.Config;
using System.Security.Principal;
using System.Threading.Tasks;
using DatMovieS.Models;

namespace DatMovieS.Controllers
{
    
   [AllowAnonymous]
   [MobileAppController]
    public class CustomAuthenticationController : ApiController
    {
        public PublishProfile CurrentPublishProfile = Startup.CurrentPublishProfile;

       
        [Route("Login")]
        [HttpGet]
        public async Task<HttpResponseMessage> Get([FromUri]string email, [FromUri]string password)
        {
            var credentials = new SimpleCredentials { email = email, password = password };
            return await Post(credentials);
        }

      
        [HttpPost]
        [Route("Login")]
       
        public async Task<HttpResponseMessage> Post(SimpleCredentials credentials)
        {
            var user = getUser(credentials.email, credentials.password);
            // return error if password is not correct
            if (user == null)
            {
                return this.Request.CreateUnauthorizedResponse();
            }

            var token = this.GetAuthenticationTokenForUser(user);

            return this.Request.CreateResponse(HttpStatusCode.OK, new
            {
                Token = token.RawData,
                User = user
            });
        }
        [AllowAnonymous]
        private User getUser(string userName, string password)
        {
            var users = (new MobileServiceContext()).Users;
            User myUser = users.FirstOrDefault(user => (user.Alias == userName || user.Email == userName) && user.Password == password );

            if (myUser == null) {
                return null;
            }

            return myUser;
        }
        [AllowAnonymous]
        public JwtSecurityToken GetAuthenticationTokenForUser(User u)
        {
            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, u.Id),
                //new Claim(JwtRegisteredClaimNames.GivenName, u.Alias),
                new Claim(JwtRegisteredClaimNames.Email, u.Email),

            };
            
            var signingKey = Environment.GetEnvironmentVariable("WEBSITE_AUTH_SIGNING_KEY");
            var audience = "";  // audience must match the url of the site
            var issuer = "";  // audience must match the url of the site

            switch (CurrentPublishProfile)
            {
                case PublishProfile.Fabio:
                    audience = "https://datmoviethevelopers.azurewebsites.net/";
                    issuer = "https://datmoviethevelopers.azurewebsites.net/";
                    break;
                case PublishProfile.Vitaly:
                    audience = "https://datmovie.azurewebsites.net/";
                    issuer = "https://datmovie.azurewebsites.net/";
                    break;
                case PublishProfile.Local:
                    signingKey = "GfYVqdtZUJQfghRiaonAeRQRDjytRi47";
                    audience = "http://localhost:4287/";
                    issuer = "http://localhost:4287/";
                    break;
                default:
                    break;
            }
            
            var token = AppServiceLoginHandler.CreateToken(
               claims,
               signingKey,
               audience,
               issuer,
               TimeSpan.FromHours(24)
            );
            var identity = new GenericIdentity(u.Email);
            identity.AddClaims(claims);
            
            var principal = new ClaimsPrincipal(identity);
            this.User = principal;
            return token;
        }
    }
}

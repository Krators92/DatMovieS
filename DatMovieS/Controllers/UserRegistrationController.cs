using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Azure.Mobile.Server.Config;
using DatMovieS.DataObjects;
using System.Security.Claims;
using DatMovieS.Models;
using System.IdentityModel.Tokens;
using System.Web;

namespace DatMovieS.Controllers
{
    // GET api/UserRegistration
    [MobileAppController]
    [AllowAnonymous]
    public class UserRegistrationController : ApiController
    {
        [Route("Register")]
        public HttpResponseMessage Get(string  email, string password)
        { 
            var context = new MobileServiceContext();
            var users = context.Users;

            User myUser;

            if (!email.Contains("@")) {
                return CreateResponce(HttpStatusCode.BadRequest, false, errorMsg: "Insert a valid email");
            }

            myUser = users.FirstOrDefault(user => user.Email == email);
           
            if (myUser != null)
            {
                return CreateResponce(HttpStatusCode.BadRequest, false, errorMsg: "User already present");
            }
                
            myUser = new User
            {
                Alias = email,
                Id = Guid.NewGuid().ToString(),
                Password = password,
                Email = email,
            };
              
            var token = new CustomAuthenticationController().GetAuthenticationTokenForUser(myUser);

            context.Set<User>().Add(myUser);
            context.SaveChanges();

            return CreateResponce(HttpStatusCode.OK, true, myUser, token: token.RawData);
         
        }
        private HttpResponseMessage CreateResponce(HttpStatusCode code, bool isOK, User usr = null, 
            string errorMsg = null, string token = null) {

            return this.Request.CreateResponse(HttpStatusCode.OK, new 
            {
                currentUser = usr,
                isOK = isOK,
                ErrorMsg = errorMsg,
                TokenString = token
            });
        }

    }
}

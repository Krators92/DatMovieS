using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Web.Http;
using Microsoft.Azure.Mobile.Server;
using Microsoft.Azure.Mobile.Server.Authentication;
using Microsoft.Azure.Mobile.Server.Config;
using DatMovieS.DataObjects;
using DatMovieS.Models;
using Owin;
using System.Web.Http.OData.Builder;
using System.Web.Http.OData.Extensions;
using System.Web.Http.Cors;

namespace DatMovieS
{
    public partial class Startup
    {
        public static PublishProfile CurrentPublishProfile = PublishProfile.Vitaly;

        public static void ConfigureMobileApp(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();
            
            new MobileAppConfiguration()
                .UseDefaultConfiguration()
                .ApplyTo(config);

            // Use Entity Framework Code First to create database tables based on your DbContext
            Database.SetInitializer(new MobileServiceInitializer());
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            builder.EntitySet<Follow>("Follows");
            builder.EntitySet<Clip>("Clips");
            builder.EntitySet<User>("Users");
            builder.EntitySet<Likes>("Likes");
            builder.EntitySet<StoryLike>("StoryLikes");
            builder.EntitySet<Story>("Stories");
          
            config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
          

            var signingKey = "GfYVqdtZUJQfghRiaonAeRQRDjytRi47";
            var audience = "http://localhost:4287/";
            var issuer = "http://localhost:4287/";

            switch (CurrentPublishProfile)
            {
                case PublishProfile.Fabio:
                    audience = "https://datmoviethevelopers.azurewebsites.net/";
                    issuer = "https://datmoviethevelopers.azurewebsites.net/";
                    break;
                case PublishProfile.Vitaly:
                    signingKey = Environment.GetEnvironmentVariable("WEBSITE_AUTH_SIGNING_KEY");
                    audience = "https://datmovies.azurewebsites.net/";
                    issuer = "https://datmovies.azurewebsites.net/";
                    break;
                case PublishProfile.Local:
                    signingKey = "GfYVqdtZUJQfghRiaonAeRQRDjytRi47";
                    audience = "http://localhost:4287/";
                    issuer = "http://localhost:4287/";
                    break;
                default:
                    break;
            }
           
            
            app.UseAppServiceAuthentication(new AppServiceAuthenticationOptions
            {
                SigningKey = signingKey,//ConfigurationManager.AppSettings["SigningKey"],
                ValidAudiences = new[] { audience },//new[] { ConfigurationManager.AppSettings["ValidAudience"], "192.168.2.72:4287" },
                ValidIssuers = new[] { issuer },//new[] { ConfigurationManager.AppSettings["ValidIssuer"], "192.168.2.72:4287" },
                TokenHandler = config.GetAppServiceTokenHandler()
            });
           
            app.UseWebApi(config);
        }
    }

    public class MobileServiceInitializer : CreateDatabaseIfNotExists<MobileServiceContext>
    {
        protected override void Seed(MobileServiceContext context)
        {
            List<TodoItem> todoItems = new List<TodoItem>
            {
                new TodoItem { Id = Guid.NewGuid().ToString(), Text = "First item", Complete = false },
                new TodoItem { Id = Guid.NewGuid().ToString(), Text = "Second item", Complete = false }
            };

            foreach (TodoItem todoItem in todoItems)
            {
                context.Set<TodoItem>().Add(todoItem);
            }

            base.Seed(context);
        }
    }
    public enum PublishProfile {
         Vitaly,
         Fabio,
         Local
    }
}


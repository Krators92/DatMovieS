using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(DatMovieS.Startup))]

namespace DatMovieS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureMobileApp(app);
            
        }
    }
}
using System.Web.Http;
using Microsoft.Azure.Mobile.Server.Config;
using System.Web.Http.Cors;

namespace DatMovieS.Controllers
{
    // Use the MobileAppController attribute for each ApiController you want to use  
    // from your mobile clients 
    [MobileAppController]
    [EnableCors(origins: "http://192.168.2.72", headers: "*", methods: "*")]
    public class ValuesController : ApiController
    {
        // GET api/values
        public string Get()
        {
            return "Ciao Marion";
        }

        // POST api/values
        public string Post()
        {
            return "Hello World!";
        }
    }
}

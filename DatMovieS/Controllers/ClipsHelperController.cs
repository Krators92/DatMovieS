using DatMovieS.DataObjects;
using DatMovieS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DatMovieS.Controllers
{
    public class ClipsHelperController : ApiController
    {
        MobileServiceContext db = new MobileServiceContext();
        [Route("odata/ClipsLikedBy")]
        public IQueryable<Clip> GetClips(string id)
        {
            var clips = db.Likes.Where(c => c.userId == id);
            List<string> clipids = new List<string>();
            foreach (var c in clips)
            {
                clipids.Add(c.clipId);
            }
            return db.Clips.Where(c => clipids.Contains(c.Id));
        }

    }
}

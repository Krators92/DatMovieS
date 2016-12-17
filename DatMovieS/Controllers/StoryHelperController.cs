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
    public class StoryHelperController : ApiController
    {
        MobileServiceContext db = new MobileServiceContext();
        [Route("odata/StoriesLikedBy")]
        public IQueryable<Story> GetClips(string id)
        {
            var stories = db.StoryLikes.Where(c => c.userId == id);
            List<string> storiesids = new List<string>();
            foreach (var c in stories)
            {
                storiesids.Add(c.storyId);
            }
            return db.Stories.Where(c => storiesids.Contains(c.Id));
        }
       
    }
}


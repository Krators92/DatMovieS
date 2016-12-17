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
    public class LikesHelperController : ApiController
    {
        MobileServiceContext db = new MobileServiceContext();

        [Route("odata/GetStoryLikes")]
        public HttpResponseMessage GetOptStoryLikes(string userid, string storyid)
        {

            var _doILike = db.StoryLikes.Count(e => e.userId == userid) > 0;
            var _totalLikes = db.StoryLikes.Count(e => e.storyId == storyid);
            var resp = new LikeResponce
            {
                doIlike = _doILike,
                totalLikes = _totalLikes
            };
            return this.Request.CreateResponse(HttpStatusCode.OK, resp);

        }

        [Route("odata/GetClipLikes")]
        public HttpResponseMessage GetOptClipLikes(string userid, string clipid)
        {

            var _doILike = db.Likes.Count(e => e.userId == userid) > 0;
            var _totalLikes = db.Likes.Count(e => e.clipId == clipid);
            var resp = new LikeResponce
            {
                doIlike = _doILike,
                totalLikes = _totalLikes
            };
            return this.Request.CreateResponse(HttpStatusCode.OK, resp);

        }

    }
}
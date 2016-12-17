using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.OData;
using System.Web.Http.OData.Routing;
using DatMovieS.DataObjects;
using DatMovieS.Models;

namespace DatMovieS.Controllers
{
    /*
    The WebApiConfig class may require additional changes to add a route for this controller. Merge these statements into the Register method of the WebApiConfig class as applicable. Note that OData URLs are case sensitive.

    using System.Web.Http.OData.Builder;
    using System.Web.Http.OData.Extensions;
    using DatMovieS.DataObjects;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<StoryLike>("StoryLikes");
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class StoryLikesController : ODataController
    {
        private MobileServiceContext db = new MobileServiceContext();

        // GET: odata/StoryLikes
        [EnableQuery]
        public IQueryable<StoryLike> GetStoryLikes()
        {

            return db.StoryLikes;
        }

      
        


        // GET: odata/StoryLikes(5)
        [EnableQuery]
        public SingleResult<StoryLike> GetStoryLike([FromODataUri] string key)
        {
            return SingleResult.Create(db.StoryLikes.Where(storyLike => storyLike.Id == key));
        }

        // PUT: odata/StoryLikes(5)
        public async Task<IHttpActionResult> Put([FromODataUri] string key, Delta<StoryLike> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            StoryLike storyLike = await db.StoryLikes.FindAsync(key);
            if (storyLike == null)
            {
                return NotFound();
            }

            patch.Put(storyLike);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StoryLikeExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(storyLike);
        }

        // POST: odata/StoryLikes
        public async Task<IHttpActionResult> Post(StoryLike storyLike)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.StoryLikes.Add(storyLike);
            var stories = await db.Stories.Where(s => s.Id == storyLike.storyId).ToListAsync();
            if (stories.Count > 0)
            {
                stories[0].likes += 1;
                db.Entry(stories[0]).State = EntityState.Modified;
            }
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (StoryLikeExists(storyLike.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(storyLike);
        }

        // PATCH: odata/StoryLikes(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] string key, Delta<StoryLike> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            StoryLike storyLike = await db.StoryLikes.FindAsync(key);
            if (storyLike == null)
            {
                return NotFound();
            }

            patch.Patch(storyLike);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StoryLikeExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(storyLike);
        }

        // DELETE: odata/StoryLikes(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] string key)
        {
            StoryLike storyLike = await db.StoryLikes.FindAsync(key);
            if (storyLike == null)
            {
                return NotFound();
            }

            db.StoryLikes.Remove(storyLike);
            await db.SaveChangesAsync();

            return StatusCode(HttpStatusCode.NoContent);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool StoryLikeExists(string key)
        {
            return db.StoryLikes.Count(e => e.Id == key) > 0;
        }
    }
}

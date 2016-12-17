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
    builder.EntitySet<Story>("Stories");
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class StoriesController : ODataController
    {
        private MobileServiceContext db = new MobileServiceContext();

        // GET: odata/Stories
        [EnableQuery]
        public IQueryable<Story> GetStories()
        {
            return db.Stories;
        }

        // GET: odata/Stories(5)
        [EnableQuery]
        public SingleResult<Story> GetStory([FromODataUri] string key)
        {
            return SingleResult.Create(db.Stories.Where(story => story.Id == key));
        }

        // PUT: odata/Stories(5)
        public async Task<IHttpActionResult> Put([FromODataUri] string key, Delta<Story> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Story story = await db.Stories.FindAsync(key);
            if (story == null)
            {
                return NotFound();
            }

            patch.Put(story);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StoryExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(story);
        }

        // POST: odata/Stories
        public async Task<IHttpActionResult> Post(Story story)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Stories.Add(story);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (StoryExists(story.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(story);
        }

        // PATCH: odata/Stories(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] string key, Delta<Story> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Story story = await db.Stories.FindAsync(key);
            if (story == null)
            {
                return NotFound();
            }

            patch.Patch(story);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StoryExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(story);
        }

        // DELETE: odata/Stories(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] string key)
        {
            Story story = await db.Stories.FindAsync(key);
            if (story == null)
            {
                return NotFound();
            }

            db.Stories.Remove(story);
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

        private bool StoryExists(string key)
        {
            return db.Stories.Count(e => e.Id == key) > 0;
        }
    }
}

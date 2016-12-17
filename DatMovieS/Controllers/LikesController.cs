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
    builder.EntitySet<Likes>("Likes");
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class LikesController : ODataController
    {
        private MobileServiceContext db = new MobileServiceContext();

        // GET: odata/Likes
        [EnableQuery]
        public IQueryable<Likes> GetLikes()
        {
            return db.Likes;
        }

        // GET: odata/Likes(5)
        [EnableQuery]
        public SingleResult<Likes> GetLikes([FromODataUri] string key)
        {
            return SingleResult.Create(db.Likes.Where(likes => likes.Id == key));
        }

        // PUT: odata/Likes(5)
        public async Task<IHttpActionResult> Put([FromODataUri] string key, Delta<Likes> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Likes likes = await db.Likes.FindAsync(key);
            if (likes == null)
            {
                return NotFound();
            }

            patch.Put(likes);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LikesExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(likes);
        }

        // POST: odata/Likes
        public async Task<IHttpActionResult> Post(Likes likes)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Likes.Add(likes);
            var clips =await db.Clips.Where(c => c.Id == likes.clipId).ToArrayAsync();
            if (clips.Length > 0) {
                clips[0].likes += 1;
                db.Entry(clips[0]).State = EntityState.Modified; 
            }
            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (LikesExists(likes.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(likes);
        }

        // PATCH: odata/Likes(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] string key, Delta<Likes> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Likes likes = await db.Likes.FindAsync(key);
            if (likes == null)
            {
                return NotFound();
            }

            patch.Patch(likes);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LikesExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(likes);
        }

        // DELETE: odata/Likes(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] string key)
        {
            Likes likes = await db.Likes.FindAsync(key);
            if (likes == null)
            {
                return NotFound();
            }

            db.Likes.Remove(likes);
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

        private bool LikesExists(string key)
        {
            return db.Likes.Count(e => e.Id == key) > 0;
        }
    }
}

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
using DatMovieS.Models;
using DatMovieS.DataObjects;
using System.Web.Http.OData.Routing.Conventions;

namespace DatMovieS.Controllers
{
    /*
    The WebApiConfig class may require additional changes to add a route for this controller. Merge these statements into the Register method of the WebApiConfig class as applicable. Note that OData URLs are case sensitive.

    using System.Web.Http.OData.Builder;
    using System.Web.Http.OData.Extensions;
    using DatMovieTheVelopersService.DataObjects;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<Clip>("Clips");
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    
    public class ClipsController : ODataController
    {
        private MobileServiceContext db = new MobileServiceContext();

        // GET: odata/Clips
        [EnableQuery]
        public IQueryable<Clip> GetClips()
        {
            return db.Clips;
        }

        // GET: odata/Clips(5)
        [EnableQuery]
        public SingleResult<Clip> GetClip([FromODataUri] string key)
        {
            return SingleResult.Create(db.Clips.Where(clip => clip.Id == key));
        }

        [EnableQuery]
        public SingleResult<Clip> GetClip2(string id)
        {
            return SingleResult.Create(db.Clips.Where(clip => clip.Id == id));
        }

        // PUT: odata/Clips(5)
        public async Task<IHttpActionResult> Put([FromODataUri] string key, Delta<Clip> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Clip clip = await db.Clips.FindAsync(key);
            if (clip == null)
            {
                return NotFound();
            }

            patch.Put(clip);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClipExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(clip);
        }

        // POST: odata/Clips
        public async Task<IHttpActionResult> Post(Clip clip)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Clips.Add(clip);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ClipExists(clip.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(clip);
        }

        // PATCH: odata/Clips(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] string key, Delta<Clip> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Clip clip = await db.Clips.FindAsync(key);
            if (clip == null)
            {
                return NotFound();
            }

            patch.Patch(clip);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClipExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(clip);
        }

        // DELETE: odata/Clips(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] string key)
        {
            Clip clip = await db.Clips.FindAsync(key);
            if (clip == null)
            {
                return NotFound();
            }

            db.Clips.Remove(clip);
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

        private bool ClipExists(string key)
        {
            return db.Clips.Count(e => e.Id == key) > 0;
        }

    }
}

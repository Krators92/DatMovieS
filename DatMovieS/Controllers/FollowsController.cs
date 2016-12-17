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

namespace DatMovieS.DataObjects
{
    /*
    The WebApiConfig class may require additional changes to add a route for this controller. Merge these statements into the Register method of the WebApiConfig class as applicable. Note that OData URLs are case sensitive.

    using System.Web.Http.OData.Builder;
    using System.Web.Http.OData.Extensions;
    using DatMovieS.DataObjects;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<Follow>("Follows");
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class FollowsController : ODataController
    {
        private MobileServiceContext db = new MobileServiceContext();

        // GET: odata/Follows
        [EnableQuery]
        public IQueryable<Follow> GetFollows()
        {
            return db.Follows;
        }


        [AcceptVerbs("GET")]
        [EnableQuery]
        public IQueryable<Follow> Followers(string foruser)
        {
            return db.Follows.Where(follow => follow.userId == foruser);
        }

        [EnableQuery]
        [Route("odata/Followings")]
        public IQueryable<Follow> GetFollowins(string follower)
        {
            return db.Follows.Where(follow => follow.followerId == follower);
        }

        // GET: odata/Follows(5)
        [EnableQuery]
        public SingleResult<Follow> GetFollow([FromODataUri] string key)
        {
            return SingleResult.Create(db.Follows.Where(follow => follow.Id == key));
        }

        // PUT: odata/Follows(5)
        public async Task<IHttpActionResult> Put([FromODataUri] string key, Delta<Follow> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Follow follow = await db.Follows.FindAsync(key);
            if (follow == null)
            {
                return NotFound();
            }

            patch.Put(follow);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FollowExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(follow);
        }

        // POST: odata/Follows
        public async Task<IHttpActionResult> Post(Follow follow)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Follows.Add(follow);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (FollowExists(follow.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Created(follow);
        }

        // PATCH: odata/Follows(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] string key, Delta<Follow> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Follow follow = await db.Follows.FindAsync(key);
            if (follow == null)
            {
                return NotFound();
            }

            patch.Patch(follow);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FollowExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(follow);
        }

        // DELETE: odata/Follows(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] string key)
        {
            Follow follow = await db.Follows.FindAsync(key);
            if (follow == null)
            {
                return NotFound();
            }

            db.Follows.Remove(follow);
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

        private bool FollowExists(string key)
        {
            return db.Follows.Count(e => e.Id == key) > 0;
        }
    }
}

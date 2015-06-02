using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.WindowsAzure.Mobile.Service;
using frameService.DataObjects;
using frameService.Models;

namespace frameService.Controllers
{
    public class MediaContentController : TableController<MediaContent>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            frameContext context = new frameContext();
            DomainManager = new EntityDomainManager<MediaContent>(context, Request, Services);
        }

        // GET tables/MediaContent
        public IQueryable<MediaContent> GetAllTodoItems()
        {
            return Query();
        }

        // GET tables/MediaContent/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<MediaContent> GetTodoItem(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/MediaContent/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<MediaContent> PatchTodoItem(string id, Delta<MediaContent> patch)
        {
            return UpdateAsync(id, patch);
        }

        // POST tables/MediaContent
        public async Task<IHttpActionResult> PostTodoItem(MediaContent item)
        {
            MediaContent current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/MediaContent/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteTodoItem(string id)
        {
            return DeleteAsync(id);
        }
    }
}
using Inshapardaz.Domain.Ports.Library;
using Inshapardaz.Functions.Authentication;
using Inshapardaz.Functions.Converters;
using Inshapardaz.Functions.Views;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Paramore.Darker;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Inshapardaz.Functions.Library.Categories
{
    public class GetCategoryById : QueryBase
    {
        public GetCategoryById(IQueryProcessor queryProcessor)
        : base(queryProcessor)
        {
        }

        [FunctionName("GetCategoryById")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "library/{libraryId}/categories/{categoryById:int}")] HttpRequest req,
            int libraryId, int categoryById,
            [AccessToken] ClaimsPrincipal principal,
            CancellationToken token)
        {
            var query = new GetCategoryByIdQuery(libraryId, categoryById);
            var category = await QueryProcessor.ExecuteAsync(query, token);

            if (category == null)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(category.Render(principal));
        }

        public static LinkView Link(int categoryById, string relType = RelTypes.Self) => SelfLink($"categories/{categoryById}", relType);
    }
}

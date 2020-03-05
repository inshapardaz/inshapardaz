using Inshapardaz.Domain.Ports.Library;
using Inshapardaz.Functions.Authentication;
using Inshapardaz.Functions.Extensions;
using Inshapardaz.Functions.Views;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Paramore.Brighter;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Inshapardaz.Functions.Library.Categories
{
    public class DeleteCategory : CommandBase
    {
        public DeleteCategory(IAmACommandProcessor commandProcessor)
        : base(commandProcessor)
        {
        }

        [FunctionName("DeleteCategory")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "library/{libraryId}/categories/{categoryId:int}")] HttpRequest req,
            int libraryId,
            int categoryId,
            [AccessToken] ClaimsPrincipal claims,
            CancellationToken token)
        {
            return await Executor.Execute(async () =>
            {
                var request = new DeleteCategoryRequest(claims, libraryId, categoryId);
                await CommandProcessor.SendAsync(request, cancellationToken: token);
                return new NoContentResult();
            });
        }

        public static LinkView Link(int categoryId, string relType = RelTypes.Self) => SelfLink($"categories/{categoryId}", relType, "DELETE");
    }
}

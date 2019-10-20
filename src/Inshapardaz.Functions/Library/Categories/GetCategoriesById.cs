using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Inshapardaz.Domain.Ports.Library;
using Inshapardaz.Functions.Authentication;
using Inshapardaz.Functions.Converters;
using Inshapardaz.Functions.Views;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Paramore.Brighter;

namespace Inshapardaz.Functions.Library.Categories
{
    public class GetCategoryById : FunctionBase
    {
        public GetCategoryById(IAmACommandProcessor commandProcessor)
        : base(commandProcessor)
        {
        }

        [FunctionName("GetCategoryById")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "categories/{categoryById:int}")] HttpRequest req,
            ILogger log, int categoryById, [AccessToken] ClaimsPrincipal principal, CancellationToken token)
        {
            var request = new GetCategoryByIdRequest(categoryById);
            await CommandProcessor.SendAsync(request, cancellationToken: token);

            if (request.Result == null)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(request.Result.Render(principal));
        }

        public static LinkView Link(int categoryById, string relType = RelTypes.Self) => SelfLink($"categories/{categoryById}", relType);
    }
}
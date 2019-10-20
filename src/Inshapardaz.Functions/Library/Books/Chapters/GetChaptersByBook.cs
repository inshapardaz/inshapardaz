using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Inshapardaz.Domain.Ports.Library;
using Inshapardaz.Functions.Converters;
using Inshapardaz.Functions.Views;
using Inshapardaz.Functions.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Paramore.Brighter;

namespace Inshapardaz.Functions.Library.Books.Chapters
{
    public class GetChaptersByBook : FunctionBase
    {
        public GetChaptersByBook(IAmACommandProcessor commandProcessor) 
        : base(commandProcessor)
        {
        }

        [FunctionName("GetChaptersByBook")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "books/{bookId:int}/chapters")] HttpRequest req,
            int bookId, ClaimsPrincipal principal, CancellationToken token)
        {
            var request = new GetChaptersByBookRequest(bookId, principal.GetUserId());
            await CommandProcessor.SendAsync(request, cancellationToken: token);

            if (request.Result != null)
            {
                return new OkObjectResult(request.Result.Render(bookId, principal));
            }

            return new NotFoundResult();
        }

        public static LinkView Link(int bookId, string relType = RelTypes.Self) => SelfLink($"books/{bookId}/chapters", relType);
    }
}
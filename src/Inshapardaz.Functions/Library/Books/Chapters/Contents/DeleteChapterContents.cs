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

namespace Inshapardaz.Functions.Library.Books.Chapters.Contents
{
    public class DeleteChapterContents : CommandBase
    {
        public DeleteChapterContents(IAmACommandProcessor commandProcessor)
            : base(commandProcessor)
        {
        }

        [FunctionName("DeleteChapterContents")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "library/{libraryId}/books/{bookId:int}/chapter/{chapterId:int}/contents")] HttpRequest req,
            int libraryId,
            int bookId,
            int chapterId,
            [AccessToken] ClaimsPrincipal claims = null,
            CancellationToken token = default)
        {
            return await Executor.Execute(async () =>
            {
                var contentType = GetHeader(req, "Content-Type", "text/markdown");
                var request = new DeleteChapterContentRequest(claims, libraryId, bookId, chapterId, contentType, claims.GetUserId());
                await CommandProcessor.SendAsync(request, cancellationToken: token);
                return new NoContentResult();
            });
        }

        public static LinkView Link(int libraryId, int bookId, int chapterId, string mimetype, string relType = RelTypes.Self, string language = null)
            => SelfLink($"library/{libraryId}/books/{bookId}/chapters/{chapterId}/contents", relType, "DELETE", type: mimetype, language: language);
    }
}

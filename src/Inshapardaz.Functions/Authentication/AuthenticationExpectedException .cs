using Inshapardaz.Functions.Views;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Inshapardaz.Functions.Authentication
{
    public sealed class AuthenticationExpectedException : ExpectedException
    {
        public AuthenticationExpectedException(string message = "")
            : base(HttpStatusCode.Forbidden, message)
        {
        }

        protected override void ApplyResponseDetails(HttpResponseMessage response)
        {
            response.Headers.WwwAuthenticate.Add(new AuthenticationHeaderValue("Bearer", "token_type=\"JWT\""));
            response.StatusCode = HttpStatusCode.Unauthorized;
        }
    }
}

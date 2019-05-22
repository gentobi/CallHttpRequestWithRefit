
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace HttpRequestWithRefit.RefitServices
{
    public class CatLoggingHandler : DelegatingHandler
    {
        public CatLoggingHandler(HttpMessageHandler innerHandler = null) : base(innerHandler ?? new HttpClientHandler())
        {
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            //
            // Prepare request logging information
            await Task.Delay(1, cancellationToken).ConfigureAwait(false);
            var startTime = DateTime.Now;
            var requestId = Guid.NewGuid().ToString();
            //
            // Log request info
            var requestMessage = $"[Request {requestId}] {request.Method} - {request.RequestUri}";
            //
            // TODO: Log the request message
            //
            // Get response
            var response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
            //
            // Log response info
            var responseMessage = $"[Request {requestId}] - Status: {response.StatusCode} \nResponse: ";
            //
            // If response is text based content type
            if (response.Content != null
                && (response.Content is StringContent
                    || IsTextBasedContentType(response.Headers)
                    || IsTextBasedContentType(response.Content.Headers)))
                responseMessage += await response.Content.ReadAsStringAsync();
            //
            // Log request time
            responseMessage += $"\nDuration: {DateTime.Now - startTime}";

            //
            // TODO: Log response message

            //
            // Return raw http response
            return response;
        }

        readonly string[] types = { "html", "text", "xml", "json", "txt", "x-www-form-urlencoded" };

        private bool IsTextBasedContentType(HttpHeaders headers)
        {
            IEnumerable<string> values;
            if (!headers.TryGetValues("Content-Type", out values))
                return false;
            var header = string.Join(" ", values).ToLowerInvariant();

            return types.Any(t => header.Contains(t));
        }
    }
}

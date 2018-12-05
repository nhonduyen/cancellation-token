using System;
using System.Collections;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Threading;


namespace cancel.Controllers
{
    public class ServerSentEventController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ServerSentEventController(IHttpContextAccessor _httpContextAccessor)
        {
            this._httpContextAccessor = _httpContextAccessor;
        }

       
        [HttpGet]
        public async Task GetTime(CancellationToken cancellationToken)
        {
            var response = _httpContextAccessor.HttpContext.Response;
            response.Headers.Add("Content-Type", "text/event-stream");
            response.StatusCode = 200;

            await response.WriteAsync($"data: {DateTime.Now}\r\r", cancellationToken);
            response.Body.Flush();
        }
        [HttpGet]
        public async Task GetFile(CancellationToken cancellationToken)
        {
            var response = _httpContextAccessor.HttpContext.Response;
            response.Headers.Add("Content-Type", "text/event-stream");
            response.StatusCode = 200;
            var fileContent =  System.IO.File.ReadAllTextAsync(@"data.txt");
           
            await response.WriteAsync($"data: {fileContent.Result}\r\r", cancellationToken);
            response.Body.Flush();
        }
    }
}
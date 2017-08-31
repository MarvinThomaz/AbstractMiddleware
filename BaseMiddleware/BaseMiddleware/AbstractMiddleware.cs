using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace BaseMiddleware
{
    public abstract class AbstractMiddleware<TException>
        where TException : BaseException
    {
        private readonly RequestDelegate _next;
        private const string _contentType = "application/json";

        protected AbstractMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await ConfigRequest(context.Request);
                await _next(context);
                await ConfigResponse(context.Response);
            }
            catch (TException ex)
            {
                await ConfigException(ex, context);
            }
        }

        protected virtual async Task ConfigRequest(HttpRequest request)
        {
            if (request.StatusCode != 404)
                return;

            var message = new ResponseMessage()
            {
                Success = false,
                Code = request.StatusCode
            };

            var response = request.CreateResponse(HttpStatusCode.NotFound);

            response.ContentType = _contentType;

            await response.Body.WriteAsync(message);
        }

        protected virtual async Task ConfigResponse(HttpResponse response)
        {
            response.ContentType = _contentType;

            var message = new ResponseMessage()
            {
                Success = IsSuccessStatusCode(response.StatusCode),
                Code = response.StatusCode,
                Data = response.Body
            };

            await response.Body.WriteAsync(message);
        }

        protected virtual async Task ConfigException(TException exception, HttpContext context)
        {
            var message = new ResponseMessage()
            {
                Success = IsSuccessStatusCode(response.StatusCode),
                Code = context.Response.StatusCode,
                Errors = exception.Errors
            };

            context.Response.ContentType = _contentType;

            await context.Response.Body.WriteAsync(message);
        }

        protected bool IsSuccessStatusCode(int code)
        {
            return (code >= 200) && (code <= 299);
        }
    }
}
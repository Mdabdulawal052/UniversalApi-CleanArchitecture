
using CloudPosCleanArc.Application.Exceptions;
using Newtonsoft.Json;
using Serilog;
using System.Diagnostics;
using System.Net;

namespace Web.Api.Helper
{
    public class AuditLogMiddleware
    {
        private readonly RequestDelegate _next;

        public AuditLogMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                await _next(context);

                stopwatch.Stop();
                var responseTime = stopwatch.ElapsedMilliseconds;
                var requestPath = context.Request.Path;
                var user = context.User.Identity.Name;
                var timestamp = DateTime.Now;
                Log.Information("Audit: Path={Path} User={User} Timestamp={Timestamp}| Response Time : {Time}", requestPath, user, timestamp, responseTime);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }

        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var requestPath = context.Request.Path;
            var user = context.User.Identity.Name;
            var timestamp = DateTime.Now;

            var code = HttpStatusCode.InternalServerError;

            var result = string.Empty;

            switch (exception)
            {
                case BadRequestException badRequestException:
                    code = HttpStatusCode.BadRequest;
                    result = badRequestException.Message;
                    Log.Error("Audit: Path={Path} User={User} Timestamp={Timestamp} |Error Code : {} | Error : {Error}", requestPath, user, timestamp, code, exception.Message);
                    break;
                case LoginFailedException loginFailedException:
                    code = HttpStatusCode.Forbidden;
                    result = loginFailedException.Message;
                    Log.Error("Audit: Path={Path} User={User} Timestamp={Timestamp} |Error Code : {} | Error : {Error}", requestPath, user, timestamp, code, exception.Message);
                    break;
                case UnauthorizedAccessException unauthorizedAccessException:
                    code = HttpStatusCode.Unauthorized;
                    result = unauthorizedAccessException.Message;
                    Log.Error("Audit: Path={Path} User={User} Timestamp={Timestamp} |Error Code : {} | Error : {Error}", requestPath, user, timestamp, code, exception.Message);
                    break;
                case DataValidationException validationException:
                    code = HttpStatusCode.NotAcceptable;

                    foreach (var f in validationException.Failures)
                    {
                        foreach (var s in f.Value)
                        {
                            result += s + ";";
                        }

                    }
                    result = JsonConvert.SerializeObject(validationException.Failures);
                    Log.Error("Audit: Path={Path} User={User} Timestamp={Timestamp} |Error Code : {} | Error : {Error}", requestPath, user, timestamp, code, exception.Message);
                    break;
                case NotFoundException _:
                    code = HttpStatusCode.NotFound;
                    Log.Error("Audit: Path={Path} User={User} Timestamp={Timestamp} |Error Code : {} | Error : {Error}", requestPath, user, timestamp, code, exception.Message);
                    break;
            }

            context.Response.ContentType = "application/problem+json";
            context.Response.StatusCode = (int)code;

            if (result == string.Empty)

            {
                result = exception.Message;
                BuildException(exception.InnerException, result);
                result = JsonConvert.SerializeObject(new
                {
                    error = exception.Message,
                    exception = JsonConvert.SerializeObject(exception)
                });
            }

            return context.Response.WriteAsync(result);
        }
        void BuildException(Exception ex, string message)
        {
            if (ex.InnerException != null)
            {
                message += ex.Message;
                BuildException(ex.InnerException, message);
            }
        }

    }
}
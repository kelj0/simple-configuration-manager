using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleConfigurationManager.Infrastructure.Middleware
{
    /// <summary>
    /// ASP.NET Core Middleware for logging errors including some specific ones (SQL).
    /// </summary>
    public class ErrorLoggingMiddleware
    {
        private readonly string[] _excludeHeaders = new string[] { "Cookie", "Authorization" };
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorLoggingMiddleware> _logger;

        /// <summary>
        /// Base constructor for <see cref="ErrorLoggingMiddleware"/> class.
        /// </summary>
        /// <param name="next">Required parameter.</param>
        /// <param name="loggerFactory">LoggerFactory used for creating logger.</param>
        public ErrorLoggingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<ErrorLoggingMiddleware>();
        }

        /// <summary>
        /// Method which gets called for every request.
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext httpContext)
        {
            var request = await ReadRequest(httpContext);
            var headers = httpContext.Request.Headers.Where(x => !_excludeHeaders.Contains(x.Key)).ToDictionary(x => x.Key, y => y.Value);
            var requestPath = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}{httpContext.Request.Path}{httpContext.Request.QueryString}";
            try
            {
                await _next(httpContext);
                // Check if httpContext resolves for success when conditions are met.  
                if (!IsSuccessStatusCode(httpContext))
                {
                    // If it's not a success, log it nicely.
                    _logger.LogWarning("Error logging middleware logged a request on path {path} with a status code: {statusCode}, headers {headers} with request {request}", requestPath, httpContext.Response.StatusCode, headers, request);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error logging middleware reported an exception on path {path} with a status code: 500, headers {headers} with request {request}", requestPath, httpContext.Response.StatusCode, headers, request);
                httpContext.Response.ContentType = "application/json";
                httpContext.Response.StatusCode = 500;
                // Exception detailing.
                string errorMessage = e.Message;
                if (e is SqlException sqlEx)
                {
                    if (new int[] { -2, 53 }.Contains(sqlEx.ErrorCode))
                    {
                        errorMessage = "There is an issue connecting to the database. This could be a network or a configuration issue. If the problem persists, please contact developers.";
                    }
                    else if (sqlEx.ErrorCode == 515)
                    {
                        errorMessage = "Required value is missing and database cannot be updated.";
                    }
                    else
                    {
                        errorMessage = "There is an issue with the database. If the problem persist, contact developers.";
                    }
                }
                else if (e is NullReferenceException || e is ArgumentException || e is ArgumentNullException || e is ArgumentOutOfRangeException)
                {
                    errorMessage = "Developers have made an error. Please contact them and let them know.";
                }
                else if (e is TimeoutException)
                {
                    errorMessage = "A process timed out. This could be temporary. If the problem persists, please contact developers.";
                }

                await httpContext.Response.WriteAsync(
                    new
                    {
                        Error = errorMessage
                    }.ToJson());
            }
        }

        private bool IsSuccessStatusCode(HttpContext httpContext)
        {
            var statusCode = (int)httpContext.Response.StatusCode;
            return statusCode >= 200 && statusCode <= 304;
        }

        private async Task<string> ReadRequest(HttpContext httpContext)
        {
            var buffer = new byte[Convert.ToInt32(httpContext.Request.ContentLength)];
            await httpContext.Request.Body.ReadAsync(buffer, 0, buffer.Length);
            //Reset to beginning (or the rest of middlewares will not be able to read it)
            httpContext.Request.Body.Seek(0, System.IO.SeekOrigin.Begin);
            return Encoding.UTF8.GetString(buffer);
        }
    }
}

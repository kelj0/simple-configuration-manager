using Microsoft.AspNetCore.Builder;
using SimpleConfigurationManager.Infrastructure.Middleware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleConfigurationManager.Infrastructure.Extensions
{
    /// <summary>
    /// Collection of extension methods for <see cref="ErrorLoggingMiddleware"/> class.
    /// </summary>
    public static class ErrorLoggingMiddlewareExtensions
    {
        /// <summary>
        /// Extension method used to add the middleware to the HTTP request pipeline.
        /// </summary>
        /// <param name="builder">ApplicationBuilder instance for an application.</param>
        /// <returns>Application builder with added error logging middleware.</returns>
        public static IApplicationBuilder UseErrorLoggingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorLoggingMiddleware>();
        }
    }
}

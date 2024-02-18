using Microsoft.AspNetCore.Diagnostics;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using JayRide.Test.Api.Core.Exceptions;
using System.Globalization;
using Serilog;

namespace JayRide.Test.Api.Core.Logging
{
    public static class LoggingApplicationBuilderExtensions
    {
        /// <summary>
        /// Use Configure Exception Handling
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public static void UseConfigureExceptionHandling(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseExceptionHandler(a => a.Run(async context =>
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;

                var feature = context.Features.Get<IExceptionHandlerPathFeature>();
                var exception = feature.Error;
                string jsonProperties = string.Empty;

                var exceptionTypeName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(nameof(exception));

                var errorDetails = new
                {
                    Type = exceptionTypeName,
                    Message = exception.Message?.Trim(),
                    StackTrace = exception.StackTrace?.Trim(),
                    exception.InnerException,
                    exception.Source
                };

                if (exception is BaseException baseException)
                {
                    context.Response.StatusCode = baseException.StatusCode;

                    foreach (var item in baseException.Properties)
                    {
                        Log.ForContext(item.Key, item.Value);
                    }
                }

                Log.Error(exception, $"UseConfigureExceptionHandling > {exceptionTypeName}", errorDetails);

                if (env.IsDevelopment())
                {
                    var errorModel = new
                    {
                        Errors = new[] { $"{errorDetails.Type} - {errorDetails.Message}" },
                        Detail = errorDetails
                    };

                    await context.Response.WriteAsync(JsonConvert.SerializeObject(errorModel, new StringEnumConverter()));
                }
                else
                {
                    //Open for modification to support returning multiple errors
                    var errorModel = new
                    {
                        Errors = new[] { $"{errorDetails.Type} - {errorDetails.Message}" }
                    };

                    await context.Response.WriteAsync(JsonConvert.SerializeObject(errorModel, new StringEnumConverter()));
                }
            }));
        }
    }
}
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using FluentValidation;
using GPMS.Backend.Services.Exceptions;
using Newtonsoft.Json;

namespace GPMS.Backend.Middlewares
{
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;
        public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (APIException ex)
            {

                _logger.LogError($"Error: {ex.Message}");

                //Set up the response status code 
                context.Response.StatusCode = ex.StatusCode;
                //Set up the response type to Json
                context.Response.ContentType = "application/json";
                //Create API Exception and serialize to Json 
                var errorResponse = new { statuscode = ex.StatusCode, message = ex.Message, data = ex.Data };
                var result = JsonConvert.SerializeObject(errorResponse);
                //Write error json to response body 
                await context.Response.WriteAsync(result);

            }
            catch (FluentValidation.ValidationException ex)
            {
                _logger.LogError($"Error: {ex.Message}");

                //Set up the response status code 
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                //Set up the response type to Json
                context.Response.ContentType = "application/json";
                //Create API Exception and serialize to Json 
                List<FormError> errorList = new List<FormError>();
                foreach (var error in ex.Errors)
                {
                    errorList.Add(new FormError
                    {
                        ErrorMessage = error.ErrorMessage,
                        Property = error.PropertyName
                    });
                }
                var errorResponse = new { statuscode = context.Response.StatusCode, message = ex.Message, data = errorList };
                var result = JsonConvert.SerializeObject(errorResponse);
                //Write error json to response body 
                await context.Response.WriteAsync(result);
            }
            catch (Exception ex)
            {

                _logger.LogError($"An Unhadled exception : {ex}");
                //Set up the response status code 
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                //Set up the response type to Json
                context.Response.ContentType = "application/json";
                //Create Exception and serialize to Json 
                var errorResponse = new { statuscode = context.Response.StatusCode, message = ex.Message, data = ex.Data };
                var exception = JsonConvert.SerializeObject(errorResponse);
                //Write error json to response body
                await context.Response.WriteAsync(exception);
            }
        }
    }
}
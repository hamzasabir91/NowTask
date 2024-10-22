using Authentication.Application.Common.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace Now.Infra.MiddleWare
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); // Call the next middleware in the pipeline
            }
            catch (NotFoundException ex)
            {
                await HandleNotFoundExceptionAsync(context, ex);
            }
            catch (ForbiddenAccessException ex)
            {
                await HandleForbiddenAccessExceptionAsync(context, ex);
            }
            catch (BadRequestException ex)
            {
                await HandleBadRequestExceptionAsync(context, ex);
            }
            catch (Exception ex)
            {
                await HandleGeneralExceptionAsync(context, ex);
            }
        }

        private Task HandleNotFoundExceptionAsync(HttpContext context, NotFoundException ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;

            var result = JsonSerializer.Serialize(new
            {
                StatusCode = context.Response.StatusCode,
                Message = ex.Message
            });

            return context.Response.WriteAsync(result);
        }

        private Task HandleForbiddenAccessExceptionAsync(HttpContext context, ForbiddenAccessException ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.Forbidden;

            var result = JsonSerializer.Serialize(new
            {
                StatusCode = context.Response.StatusCode,
                Message = ex.Message
            });

            return context.Response.WriteAsync(result);
        }

        private Task HandleBadRequestExceptionAsync(HttpContext context, BadRequestException ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            var result = JsonSerializer.Serialize(new
            {
                StatusCode = context.Response.StatusCode,
                Message = ex.Message
            });

            return context.Response.WriteAsync(result);
        }

        private Task HandleGeneralExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var result = JsonSerializer.Serialize(new
            {
                StatusCode = context.Response.StatusCode,
                Message = "An unexpected error occurred. Please try again later."
            });

            return context.Response.WriteAsync(result);
        }
    }
}

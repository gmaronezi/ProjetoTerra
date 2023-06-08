using System.Net.Mime;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using ProjetoTerra.Shared.Abstractions;
using ProjetoTerra.Shared.Exceptions;
using ProjetoTerra.Shared.Extensions;

namespace ProjetoTerra.Shared.HttpServices.Configurations;

public static class ExceptionHandler
{
    public static IApplicationBuilder UseCustomExceptionHandler(
        this IApplicationBuilder app
    )
    {
        return app.UseExceptionHandler(errorApp =>
        {
            errorApp.Run(async context =>
            {
                context.Response.ContentType = MediaTypeNames.Application.Json;

                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                if (contextFeature != null)
                {
                    context.Response.StatusCode = contextFeature.Error is InvalidActionException ? StatusCodes.Status400BadRequest : StatusCodes.Status500InternalServerError;
                    
                    if (contextFeature.Error is InvalidActionException parsedException)
                    {
                        await context.Response.WriteAsync(
                            JsonSnakeSerializer.Serialize(parsedException.AsFailedResult()));
                    }
                    else
                    {
                        await context.Response.WriteAsync(JsonSnakeSerializer.Serialize(
                            new FailedResult(
                                contextFeature.Error.Message,
                                new StringBuilder(contextFeature.Error.Source)
                                    .Append(Environment.NewLine)
                                    .Append(contextFeature.Error.Source)
                                    .Append(Environment.NewLine)
                                    .Append(contextFeature.Error.StackTrace)
                                    .Append(Environment.NewLine)
                                    .Append(contextFeature.Error.HelpLink).ToString()
                            ))
                        );
                    }
                }
            });
        });
    }
}
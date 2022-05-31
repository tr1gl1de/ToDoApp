using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ToDoApp.WebApi.Filters;

public class OpenApiFilter : IOperationFilter
{
    private readonly OpenApiSecurityRequirement _apiSecurityRequirement;

    public OpenApiFilter() =>
        _apiSecurityRequirement = new OpenApiSecurityRequirement()
        {
            {
                new OpenApiSecurityScheme()
                {
                    Reference = new OpenApiReference()
                    {
                        Id = "Bearer",
                        Type = ReferenceType.SecurityScheme
                    }
                },
                Array.Empty<string>()
            }
        };
    
    
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var actionMetadata = context.ApiDescription.ActionDescriptor.EndpointMetadata;
        var markedWithAuthorize = actionMetadata
            .Any(metadataItem => metadataItem is AuthorizeAttribute);

        if (markedWithAuthorize)
        {
            operation.Security.Add(_apiSecurityRequirement);
        }
    }
}
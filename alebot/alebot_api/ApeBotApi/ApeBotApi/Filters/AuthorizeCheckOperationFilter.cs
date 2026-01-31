using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Authorization;

namespace AleBotApi.Filters
{
    public class AuthorizeCheckOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (HasAttribute<AllowAnonymousAttribute>(context))
                return;
            if (!HasAttribute<AuthorizeAttribute>(context))
                return;
            operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized" });
            operation.Responses.Add("403", new OpenApiResponse { Description = "Forbidden" });
            operation.Security = new List<OpenApiSecurityRequirement>
            {
                new OpenApiSecurityRequirement
                {
                    [
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference {Type = ReferenceType.SecurityScheme, Id = "Bearer"},
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        }
                    ] = new List<string>()
                }
            };
        }

        private bool HasAttribute<T>(OperationFilterContext context) where T : Attribute
        {
            var hasAuthorize =
                context.MethodInfo.DeclaringType.GetCustomAttributes(true).OfType<T>().Any()
                || context.MethodInfo.GetCustomAttributes(true).OfType<T>().Any();
            return hasAuthorize;
        }
    }
}

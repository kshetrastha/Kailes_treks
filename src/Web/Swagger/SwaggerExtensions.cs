using Microsoft.OpenApi.Models;

namespace TravelCleanArch.Web.Swagger;

public static class SwaggerExtensions
{
    public static IServiceCollection AddSwaggerWithJwtAndGroups(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("admin", new OpenApiInfo { Title = "Admin API", Version = "v1" });
            c.SwaggerDoc("customer", new OpenApiInfo { Title = "Customer API", Version = "v1" });

            c.DocInclusionPredicate((docName, apiDesc) =>
            {
                var group = apiDesc.GroupName ?? string.Empty;
                return docName.Equals(group, StringComparison.OrdinalIgnoreCase);
            });

            var scheme = new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Enter: Bearer {your JWT token}"
            };

            c.AddSecurityDefinition("Bearer", scheme);
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                { scheme, Array.Empty<string>() }
            });
        });

        return services;
    }

    public static WebApplication UseGroupedSwaggerUI(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/admin/swagger.json", "Admin API v1");
            c.SwaggerEndpoint("/swagger/customer/swagger.json", "Customer API v1");
            c.RoutePrefix = "swagger";
        });

        return app;
    }
}

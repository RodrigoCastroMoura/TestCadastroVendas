using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using TestCadastroVendas.Api.Infra.Swagger;


namespace TestCadastroVendas.Api.Infra.Configurations;

public static class SwaggerConfiguration
{
    public static void AddCustomSwagger(this IServiceCollection services)
    {
        services.AddApiVersioning(options =>
        {
            options.ReportApiVersions = true;
            options.ApiVersionReader = new HeaderApiVersionReader("api-version");
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.DefaultApiVersion = new ApiVersion(1, 0);
        });

        services.AddVersionedApiExplorer(
            options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;

            });

        services.AddSwaggerGen(options =>
        {
            options.CustomOperationIds(e => $"{e.ActionDescriptor.RouteValues["controller"]}_{e.RelativePath}_{e.HttpMethod}");
            options.DescribeAllParametersInCamelCase();
          
            options.IncludeXmlComments(XmlCommentsFilePath);
        
        });

        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
    }

    public static IApplicationBuilder UseCustomSwagger(this IApplicationBuilder app)
    {
        var host = Environment.GetEnvironmentVariable("HOST");
        var hostBasePath = Environment.GetEnvironmentVariable("HOST_BASE_PATH");
        var hostPort = Environment.GetEnvironmentVariable("HOST_PORT");

        app.UseSwagger(options =>
        {

            if (!string.IsNullOrEmpty(host))
                options.PreSerializeFilters.Add((swagger, httpReq) =>
                {

                    var serverUrl = $"{httpReq.Scheme}://{host}/{hostBasePath}";

                    if (!string.IsNullOrEmpty(hostPort))
                        serverUrl = $"{httpReq.Scheme}://{host}:{hostPort}/{hostBasePath}";

                    swagger.Servers = new List<OpenApiServer> { new OpenApiServer { Url = serverUrl } };
                });
        });
        app.UseSwaggerUI(c =>
        {
            if (!string.IsNullOrEmpty(hostBasePath))
                c.SwaggerEndpoint($"/{hostBasePath}/swagger/v1/swagger.json", "v1");
            else
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        });

        return app;
    }

    private static string XmlCommentsFilePath
    {
        get
        {
            var programAssembly = typeof(Program).Assembly;
            var basePath = Path.GetDirectoryName(programAssembly.Location);
            var fileName = $"{programAssembly.GetName().Name}.xml";
            return Path.Combine(basePath, fileName);
        }
    }
}

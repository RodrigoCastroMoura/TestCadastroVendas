using TestCadastroVendas.Api.Infra.Configurations;
using System.Text.Json.Serialization;


namespace TestCadastroVendas.Api;

public static class HostingExtensions
{
    public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddCustomSwagger();

        builder.Services.Configure<CookiePolicyOptions>(options =>
        {
            options.CheckConsentNeeded = context => true;
            options.MinimumSameSitePolicy = SameSiteMode.None;
        });
        builder.Services
            .AddControllers(options =>
            {
                //options.Filters.Add<ErrorOrMvcResultFilter>();
            })
            .ConfigureApiBehaviorOptions(options =>
            {
                options.SuppressMapClientErrors = false;
            })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            });

       

        return builder;
    }
}

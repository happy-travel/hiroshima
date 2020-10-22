using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Options;

namespace HappyTravel.Hiroshima.WebApi.Filters
{
    public class LocalizationPipelineFilter
    {
        public void Configure(IApplicationBuilder app, IOptions<RequestLocalizationOptions> options)
        {
            app.UseRequestLocalization(options.Value);
        }
    }
}
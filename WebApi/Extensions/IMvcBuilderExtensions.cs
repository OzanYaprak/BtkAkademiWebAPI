using WebApi.Utilities.Formatters;

namespace WebApi.Extensions
{
    public static class IMvcBuilderExtensions
    {
        public static IMvcBuilder AddCustomCsvFormatter(this IMvcBuilder mvcBuilder)
        {
            return mvcBuilder.AddMvcOptions(config => config.OutputFormatters.Add(new CsvOutputFormatter()));
        }
    }
}
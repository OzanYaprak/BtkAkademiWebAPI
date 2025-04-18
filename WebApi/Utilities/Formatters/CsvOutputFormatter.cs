using Entities.DataTransferObjects;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using System.Text;

namespace WebApi.Utilities.Formatters
{
    public class CsvOutputFormatter : TextOutputFormatter
    {
        public CsvOutputFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/csv"));

            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
        }

        protected override bool CanWriteType(Type type)
        {
            if (typeof(BookDTO).IsAssignableFrom(type) || typeof(IEnumerable<BookDTO>).IsAssignableFrom(type))
            {
                return base.CanWriteType(type);
            }
            return false;
        }

        private static void FormatCsv(StringBuilder stringBuilder, BookDTO bookDTO)
        {
            stringBuilder.AppendLine($"{bookDTO.Id},{bookDTO.Title},{bookDTO.Price}");
        }

        public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            var response = context.HttpContext.Response;
            var stringBuilder = new StringBuilder();

            if (context.Object is IEnumerable<BookDTO>)
            {
                foreach (var item in (IEnumerable<BookDTO>)context.Object)
                {
                    FormatCsv(stringBuilder, item);
                }
            }
            else
            {
                FormatCsv(stringBuilder, (BookDTO)context.Object);
            }
            await response.WriteAsync(stringBuilder.ToString());
        }
    }
}
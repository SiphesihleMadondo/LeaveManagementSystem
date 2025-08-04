using Newtonsoft.Json.Linq;
using System.Text;

namespace LeaveManagementSystem.Application.Services.Export
{
 
    public class ExportService : IExportService
    {
        public byte[] ConvertToCsv(JArray data, IEnumerable<string> fieldsToInclude, out string contentType, out string fileName)
        {
            contentType = "text/csv";
            fileName = $"export_{DateTime.Now:yyyyMMdd_HHmmss}.csv";

            if (data == null || !data.Any())
                throw new ArgumentException("No data provided for export.");

            if (fieldsToInclude == null || !fieldsToInclude.Any())
                throw new ArgumentException("No fields specified for export.");

            var csv = new StringBuilder();

            // Header row
            csv.AppendLine(string.Join(",", fieldsToInclude));

            // Data rows
            foreach (JObject item in data)
            {
                var row = fieldsToInclude.Select(field =>
                {
                    var value = item[field]?.ToString() ?? "";
                    value = value.Replace("\"", "\"\"");
                    return $"\"{value}\"";
                });

                csv.AppendLine(string.Join(",", row));
            }

            return Encoding.UTF8.GetBytes(csv.ToString());
        }

    }
}

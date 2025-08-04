
using Newtonsoft.Json.Linq;

namespace LeaveManagementSystem.Application.Services.Export
{
    public interface IExportService
    {
        byte[] ConvertToCsv(JArray data, IEnumerable<string> fieldsToInclude, out string contentType, out string fileName);
    }
}
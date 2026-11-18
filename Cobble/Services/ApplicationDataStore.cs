using Cobble.Abstractions;
using Cobble.Models;
using System.IO;
using System.Text.Json;
namespace Cobble.Services
{
    public class ApplicationDataStore : IFileStore<ApplicationData>
    {
        private readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions()
        {
            WriteIndented = true
        };
        public async Task<ApplicationData?> LoadAsync(string path)
        {
            if (!File.Exists(path)) return null;
            string content = await File.ReadAllTextAsync(path);
            ApplicationData? applicationData = JsonSerializer.Deserialize<ApplicationData>(content, _jsonSerializerOptions);
            if (applicationData is null) return null;
            return applicationData;
        }
        public async Task<bool> SaveAsync(string path, ApplicationData content)
        {
            if (!Path.Exists(Path.GetDirectoryName(Path.GetDirectoryName(path)))) return false;
            Directory.CreateDirectory((Path.GetDirectoryName(path) ?? "Null"));
            string json = JsonSerializer.Serialize<ApplicationData>(content, _jsonSerializerOptions);
            await File.WriteAllTextAsync(path, json);
            return true;
        }
    }
}

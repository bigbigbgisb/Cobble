using Cobble.Abstractions;
using Cobble.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.Json;

namespace Cobble.Services
{
    public class ApplicationCacheStore : IFileStore<ApplicationCache>
    {
        private readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions()
        {
            WriteIndented = true
        };
        public async Task<ApplicationCache?> LoadAsync(string path)
        {
            if (!File.Exists(path)) return null;
            string content = await File.ReadAllTextAsync(path);
            ApplicationCache? applicationCache = JsonSerializer.Deserialize<ApplicationCache>(content, _jsonSerializerOptions);
            if (applicationCache is null) return null;
            return applicationCache;
        }

        public async Task<bool> SaveAsync(string path, ApplicationCache content)
        {
            if (!Path.Exists(Path.GetDirectoryName(Path.GetDirectoryName(path)))) return false;
            Directory.CreateDirectory((Path.GetDirectoryName(path) ?? "Null"));
            string json = JsonSerializer.Serialize<ApplicationCache>(content, _jsonSerializerOptions);
            await File.WriteAllTextAsync(path, json);
            return true;
        }
    }
}

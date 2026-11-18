using Cobble.Abstractions;
using System.IO;
namespace Cobble.Services
{
    public class MemoFileStore : IFileStore<string>
    {
        public async Task<string?> LoadAsync(string path)
        {
            if (File.Exists(path))
            {
                string content = await File.ReadAllTextAsync(path);
                return content;
            }
            return null;
        }
        public async Task<bool> SaveAsync(string path, string content)
        {
            if (Directory.Exists(Path.GetDirectoryName(path)))
            {
                await File.WriteAllTextAsync(path, content);
                return true;
            }
            else
                return false;
        }
    }
}

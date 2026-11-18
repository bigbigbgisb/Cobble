using Cobble.Abstractions;
using System.IO;
namespace Cobble.Services
{
    public class CodeFileStore : IFileStore<string>
    {
        public async Task<string?> LoadAsync(string path)
        {
            var files = Directory.GetFiles(path, searchPattern: "*.cs", SearchOption.AllDirectories);
            string code = "";
            foreach(var currentPath in files)
            {
                string currentCode = await File.ReadAllTextAsync(currentPath);
                code += $"====={Path.GetFileName(currentPath)}====={Environment.NewLine}";
                code += currentCode;
            }
            return code;
        }
        public Task<bool> SaveAsync(string path, string content)
        {
            throw new NotSupportedException("Save you **,what are you doin'");
        }
    }
}

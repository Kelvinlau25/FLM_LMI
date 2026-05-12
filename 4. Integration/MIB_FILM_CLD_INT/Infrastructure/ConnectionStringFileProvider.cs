using System.Collections.Concurrent;

namespace MIB_FILM_CLD_INT.Infrastructure
{
    public sealed class ConnectionStringFileProvider(IWebHostEnvironment environment)
    {
        private readonly IWebHostEnvironment _environment = environment;
        private readonly ConcurrentDictionary<string, string> _cache = new(StringComparer.OrdinalIgnoreCase);

        public string GetRequired(string fileName)
        {
            return _cache.GetOrAdd(fileName, LoadConnectionString);
        }

        private string LoadConnectionString(string fileName)
        {
            string path = Path.Combine(_environment.ContentRootPath, fileName);
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"Connection string file '{fileName}' was not found.", path);
            }

            string? lastLine = null;
            foreach (string line in File.ReadLines(path))
            {
                lastLine = line;
            }

            if (string.IsNullOrWhiteSpace(lastLine))
            {
                throw new InvalidOperationException($"Connection string file '{fileName}' is empty.");
            }

            return lastLine.Trim();
        }
    }
}

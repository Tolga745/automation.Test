namespace automation.Tests.Support;

public static class TestConfiguration
{
    private static readonly IReadOnlyDictionary<string, string> Values =
        LoadEnvironmentFile();

    public static string Resolve(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return value;
        }

        var environmentValue = Environment.GetEnvironmentVariable(value);
        if (!string.IsNullOrEmpty(environmentValue))
        {
            return environmentValue;
        }

        return Values.TryGetValue(value, out var fileValue)
            ? fileValue
            : value;
    }

    private static IReadOnlyDictionary<string, string> LoadEnvironmentFile()
    {
        var values = new Dictionary<string, string>(
            StringComparer.OrdinalIgnoreCase);

        foreach (var candidate in GetCandidatePaths())
        {
            if (!File.Exists(candidate))
            {
                continue;
            }

            foreach (var line in File.ReadAllLines(candidate))
            {
                var trimmed = line.Trim();
                if (trimmed.Length == 0 ||
                    trimmed.StartsWith('#'))
                {
                    continue;
                }

                var separator = trimmed.IndexOf('=');
                if (separator <= 0)
                {
                    continue;
                }

                var key = trimmed[..separator].Trim();
                var value = trimmed[(separator + 1)..].Trim();

                if (value.Length >= 2 &&
                    ((value.StartsWith('"') && value.EndsWith('"')) ||
                     (value.StartsWith('\'') && value.EndsWith('\''))))
                {
                    value = value[1..^1];
                }

                values[key] = value;
            }

            break;
        }

        return values;
    }

    private static IEnumerable<string> GetCandidatePaths()
    {
        var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        var paths = new[]
        {
            Path.Combine(AppContext.BaseDirectory, ".env"),
            "/src/.env",
            Path.Combine(Directory.GetCurrentDirectory(), ".env")
        };

        foreach (var path in paths)
        {
            if (seen.Add(path))
            {
                yield return path;
            }
        }

        var directory = new DirectoryInfo(Directory.GetCurrentDirectory());
        while (directory != null)
        {
            var path = Path.Combine(directory.FullName, ".env");
            if (seen.Add(path))
            {
                yield return path;
            }

            directory = directory.Parent;
        }
    }
}

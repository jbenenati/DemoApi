using System.Text.RegularExpressions;

namespace DemoApi.Tests;

public class UnsafeSqlUsageTests
{
    [Fact]
    public void No_Interpolated_Or_Concatenated_Sql_Commands()
    {
        var root = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../.."));
        var csFiles = Directory.GetFiles(root, "*.cs", SearchOption.AllDirectories);

        // Look for new SqlCommand($"...{") or SqlCommand("..."+ var ...)
        var interp = new Regex(@"new\s+SqlCommand\s*\(\s*\$@?""[^""]*{\s*[^}]+}\s*[^""]*""", RegexOptions.Compiled);
        var concat = new Regex(@"new\s+SqlCommand\s*\(\s*""[^""]*""\s*\+\s*\w+", RegexOptions.Compiled);

        var offenders = new List<string>();
        foreach (var file in csFiles)
        {
            var text = File.ReadAllText(file);
            if (interp.IsMatch(text) || concat.IsMatch(text))
                offenders.Add(file);
        }

        Assert.True(offenders.Count == 0,
            "Potential unsafe SQL construction in: " + string.Join(", ", offenders));
    }
}

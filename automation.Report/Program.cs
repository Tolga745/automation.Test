using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

if (args.Length != 2)
{
    Console.Error.WriteLine("Usage: automation.Report <input.trx> <output.html>");
    return 2;
}

var trxPath = args[0];
var htmlPath = args[1];

if (!File.Exists(trxPath))
{
    Console.Error.WriteLine($"TRX report not found: {trxPath}");
    return 2;
}

var document = XDocument.Load(trxPath);
XNamespace ns = document.Root?.Name.Namespace ?? XNamespace.None;

var results = document
    .Descendants(ns + "UnitTestResult")
    .Select(element =>
    {
        var name = element.Attribute("testName")?.Value
            ?? element.Attribute("name")?.Value
            ?? "Unknown scenario";
        var outcome = element.Attribute("outcome")?.Value ?? "Unknown";
        var output = element.Element(ns + "Output");
        var stdout = output?.Element(ns + "StdOut")?.Value ?? "";
        var errorMessage = output?.Element(ns + "ErrorInfo")?.Element(ns + "Message")?.Value;

        return new TestResult(name, outcome, ParseSteps(stdout), stdout, errorMessage);
    })
    .ToList();

var passed = results.Count(result =>
    result.Outcome.Equals("Passed", StringComparison.OrdinalIgnoreCase));
var failed = results.Count(result =>
    result.Outcome.Equals("Failed", StringComparison.OrdinalIgnoreCase));

var html = BuildHtml(results, passed, failed);
Directory.CreateDirectory(Path.GetDirectoryName(htmlPath)!);
var temporaryHtmlPath = htmlPath + ".tmp";
File.WriteAllText(temporaryHtmlPath, html, Encoding.UTF8);
File.Move(temporaryHtmlPath, htmlPath, overwrite: true);

Console.WriteLine($"Test report written to: {htmlPath}");
return 0;

static List<StepResult> ParseSteps(string stdout)
{
    var steps = new List<StepResult>();
    string? pendingStep = null;

    foreach (var line in stdout.Replace("\r\n", "\n").Split('\n'))
    {
        var trimmed = line.Trim();

        var stepMatch = Regex.Match(
            trimmed,
            @"^(Given|When|Then|And|But)\b\s*(.*)$",
            RegexOptions.IgnoreCase);
        if (stepMatch.Success)
        {
            if (pendingStep != null)
            {
                steps.Add(new StepResult(pendingStep, "Skipped", ""));
            }

            pendingStep = $"{stepMatch.Groups[1].Value} {stepMatch.Groups[2].Value}".Trim();
            continue;
        }

        var statusMatch = Regex.Match(
            trimmed,
            @"^->\s*(done|error|undefined|skipped|pending):?\s*(.*)$",
            RegexOptions.IgnoreCase);
        if (statusMatch.Success && pendingStep != null)
        {
            var status = statusMatch.Groups[1].Value.ToLowerInvariant() switch
            {
                "done" => "Passed",
                "error" => "Failed",
                "undefined" => "Failed",
                "skipped" => "Skipped",
                "pending" => "Skipped",
                _ => "Skipped"
            };

            steps.Add(new StepResult(
                pendingStep,
                status,
                statusMatch.Groups[2].Value.Trim()));
            pendingStep = null;
        }
    }

    if (pendingStep != null)
    {
        steps.Add(new StepResult(pendingStep, "Skipped", ""));
    }

    return steps;
}

static string BuildHtml(
    List<TestResult> results,
    int passed,
    int failed)
{
    var body = new StringBuilder();

    body.AppendLine("<main>");
    body.AppendLine($"<h1>Automation Test Report</h1>");
    body.AppendLine($"<p>Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}</p>");
    body.AppendLine("<div class=\"summary\">");
    body.AppendLine($"<div><strong>{results.Count}</strong><span>Total</span></div>");
    body.AppendLine($"<div class=\"passed\"><strong>{passed}</strong><span>Passed</span></div>");
    body.AppendLine($"<div class=\"failed\"><strong>{failed}</strong><span>Failed</span></div>");
    body.AppendLine("</div>");

    foreach (var result in results
                 .OrderBy(result => result.Outcome.Equals("Failed", StringComparison.OrdinalIgnoreCase) ? 0 : 1)
                 .ThenBy(result => result.Name))
    {
        var resultClass = result.Outcome.Equals("Passed", StringComparison.OrdinalIgnoreCase)
            ? "passed"
            : result.Outcome.Equals("Failed", StringComparison.OrdinalIgnoreCase)
                ? "failed"
                : "unknown";

        body.AppendLine($"<section class=\"scenario {resultClass}\">");
        body.AppendLine("<div class=\"scenario-heading\">");
        body.AppendLine($"<h2>{Encode(result.Name)}</h2>");
        body.AppendLine($"<span class=\"status {resultClass}\">{Encode(result.Outcome)}</span>");
        body.AppendLine("</div>");

        if (result.Steps.Count > 0)
        {
            body.AppendLine("<ol class=\"steps\">");
            foreach (var step in result.Steps)
            {
                var stepClass = step.Status.ToLowerInvariant();
                body.AppendLine($"<li class=\"step {stepClass}\">");
                body.AppendLine($"<span class=\"step-status\">{Encode(step.Status)}</span>");
                body.AppendLine($"<span class=\"step-text\">{Encode(step.Text)}</span>");
                if (!string.IsNullOrWhiteSpace(step.Detail))
                {
                    body.AppendLine($"<span class=\"step-detail\">{Encode(step.Detail)}</span>");
                }
                body.AppendLine("</li>");
            }
            body.AppendLine("</ol>");
        }
        else
        {
            body.AppendLine("<p class=\"no-steps\">No Reqnroll step output was available for this result.</p>");
        }

        if (!string.IsNullOrWhiteSpace(result.ErrorMessage))
        {
            body.AppendLine("<details>");
            body.AppendLine("<summary>Error details</summary>");
            body.AppendLine($"<pre>{Encode(result.ErrorMessage)}</pre>");
            body.AppendLine("</details>");
        }

        body.AppendLine("</section>");
    }

    body.AppendLine("</main>");

    return $$"""
<!doctype html>
<html lang="en">
<head>
  <meta charset="utf-8">
  <meta name="viewport" content="width=device-width, initial-scale=1">
  <title>Automation Test Report</title>
  <style>
    body { margin: 0; background: #f5f7fa; font-family: system-ui, sans-serif; color: #1f2937; }
    main { max-width: 1100px; margin: 0 auto; padding: 24px; }
    h1 { margin-bottom: 4px; }
    .summary { display: flex; gap: 12px; margin: 20px 0; }
    .summary div { min-width: 120px; padding: 16px; background: white; border-radius: 8px; box-shadow: 0 1px 4px #d8dee8; text-align: center; }
    .summary strong, .summary span { display: block; }
    .summary strong { font-size: 28px; }
    .summary .passed strong { color: #16833a; }
    .summary .failed strong { color: #b42318; }
    .scenario { margin-bottom: 16px; background: white; border-radius: 8px; border-left: 6px solid #94a3b8; box-shadow: 0 1px 4px #d8dee8; overflow: hidden; }
    .scenario.passed { border-left-color: #22a447; }
    .scenario.failed { border-left-color: #dc2626; }
    .scenario-heading { display: flex; justify-content: space-between; align-items: center; gap: 16px; padding: 16px 20px; border-bottom: 1px solid #e5e7eb; }
    .scenario-heading h2 { margin: 0; font-size: 18px; }
    .status { padding: 5px 10px; border-radius: 999px; font-weight: 700; font-size: 13px; }
    .status.passed { background: #dcfce7; color: #166534; }
    .status.failed { background: #fee2e2; color: #991b1b; }
    .status.unknown { background: #e5e7eb; color: #374151; }
    .steps { list-style: none; margin: 0; padding: 12px 20px 16px; }
    .step { display: grid; grid-template-columns: 82px 1fr auto; gap: 12px; align-items: start; padding: 8px 0; border-bottom: 1px solid #f1f5f9; }
    .step-status { font-weight: 700; font-size: 12px; text-transform: uppercase; }
    .step.passed .step-status { color: #16833a; }
    .step.failed .step-status { color: #b42318; }
    .step.skipped .step-status { color: #6b7280; }
    .step-detail { color: #6b7280; font-size: 13px; }
    .no-steps { padding: 0 20px 16px; color: #6b7280; }
    details { margin: 0 20px 16px; }
    pre { white-space: pre-wrap; background: #111827; color: #f9fafb; padding: 12px; border-radius: 6px; overflow-x: auto; }
  </style>
</head>
<body>
{{body}}
</body>
</html>
""";
}

static string Encode(string value) =>
    System.Net.WebUtility.HtmlEncode(value);

record StepResult(string Text, string Status, string Detail);
record TestResult(
    string Name,
    string Outcome,
    List<StepResult> Steps,
    string StandardOutput,
    string? ErrorMessage);

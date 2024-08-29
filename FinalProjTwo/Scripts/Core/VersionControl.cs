using DrawingProgram;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO.Compression;

public static class VersionControl
{
    public const string CurrentVersion = "v2.417";
    private const string GitHubToken = "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";
    private static string latestVersion;
    private static PopupWindow errorWindow = new(null, 600, 200, ["An error has occured!"]);

    public static async Task<bool> IsLatestVersion(ProgramManager program)
    {
        latestVersion = await GetLatestReleaseTag(program);
        return latestVersion == CurrentVersion;
    }

    public static async Task UpdateProgram(ProgramManager program)
    {
        await DownloadUpdate(program);
        ExtractUpdate(program);
    }

    private static async Task<string> GetLatestReleaseTag(ProgramManager program)
    {
        using (HttpClient client = new HttpClient())
        {
            string url = "https://api.github.com/repos/rivade/Paint.TO/releases/latest";
            client.DefaultRequestHeaders.UserAgent.ParseAdd("request");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("token", GitHubToken);

            HttpResponseMessage response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();

                using (JsonDocument doc = JsonDocument.Parse(json))
                {
                    JsonElement root = doc.RootElement;
                    if (root.TryGetProperty("tag_name", out JsonElement tagNameElement))
                    {
                        return tagNameElement.GetString();
                    }
                }
            }
            else
            {
                program.popupWindow = errorWindow;
            }
            return null;
        }
    }

    private static async Task DownloadUpdate(ProgramManager program)
    {
        try
        {
            string downloadUrl = await GetZipDownloadUrl(latestVersion, program);
            if (downloadUrl == null)
            {
                program.popupWindow = errorWindow;
                return;
            }

            string executableFolder = AppContext.BaseDirectory;
            string zipFilePath = Path.Combine(executableFolder, "updatedver.zip");

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.UserAgent.ParseAdd("request");
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("token", GitHubToken);

                HttpResponseMessage response = await client.GetAsync(downloadUrl);
                if (response.IsSuccessStatusCode)
                {
                    using (FileStream fs = new FileStream(zipFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        await response.Content.CopyToAsync(fs);
                    }
                }
                else
                {
                    program.popupWindow = errorWindow;
                }
            }
        }
        catch
        {
            program.popupWindow = errorWindow;
        }

    }

    private static void ExtractUpdate(ProgramManager program)
    {
        try
        {
            string executableFolder = AppContext.BaseDirectory;
            string zipFilePath = Path.Combine(executableFolder, "updatedver.zip");
            string extractPath = Path.Combine(executableFolder, "update");

            if (File.Exists(zipFilePath))
            {
                Directory.CreateDirectory(extractPath);

                ZipFile.ExtractToDirectory(zipFilePath, extractPath);

                File.Delete(zipFilePath);
                Environment.Exit(0);
            }
            else
            {
                program.popupWindow = errorWindow;
            }
        }
        catch
        {
            program.popupWindow = errorWindow;
        }
    }

    private static async Task<string> GetZipDownloadUrl(string tag, ProgramManager program)
    {
        using (HttpClient client = new HttpClient())
        {
            string url = $"https://api.github.com/repos/rivade/Paint.TO/releases/tags/{tag}";
            client.DefaultRequestHeaders.UserAgent.ParseAdd("request");
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("token", GitHubToken);

            HttpResponseMessage response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();

                using (JsonDocument doc = JsonDocument.Parse(json))
                {
                    JsonElement root = doc.RootElement;
                    if (root.TryGetProperty("assets", out JsonElement assetsElement))
                    {
                        foreach (JsonElement asset in assetsElement.EnumerateArray())
                        {
                            if (asset.TryGetProperty("name", out JsonElement nameElement) &&
                                asset.TryGetProperty("browser_download_url", out JsonElement downloadUrlElement))
                            {
                                string name = nameElement.GetString();
                                string downloadUrl = downloadUrlElement.GetString();

                                // Filter by file extension or name if needed
                                if (name.EndsWith($"{latestVersion.ToUpper()}.zip", StringComparison.OrdinalIgnoreCase))
                                {
                                    return downloadUrl;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                program.popupWindow = errorWindow;
            }
            return null;
        }
    }
}
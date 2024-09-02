using DrawingProgram;
using System.Text.Json;

public static class UserPrefs
{
    private static readonly string appFilePath = AppContext.BaseDirectory;
    private static readonly string jsonFilePath = Path.Combine(appFilePath, "Assets/Settings.json");

    public static void SaveSettings()
    {
        ColorData guiColorData = new()
        {
            R = GUIarea.GUIColor.R,
            G = GUIarea.GUIColor.G,
            B = GUIarea.GUIColor.B
        };

        ColorData buttonColorData = new()
        {
            R = ToolButton.toolButtonColor.R,
            G = ToolButton.toolButtonColor.G,
            B = ToolButton.toolButtonColor.B
        };

        ColorData[] colorSettings = { guiColorData, buttonColorData };
        JsonSerializerOptions options = new() { WriteIndented = true };

        string content = JsonSerializer.Serialize(colorSettings, options);
        File.WriteAllText(jsonFilePath, content);
    }

    public static void LoadSettings()
    {
        string content = File.ReadAllText(jsonFilePath);
        ColorData[] colorSettings = JsonSerializer.Deserialize<ColorData[]>(content);

        GUIarea.GUIColor = new Color(colorSettings[0].R, colorSettings[0].G, colorSettings[0].B, (byte)255);
        ToolButton.toolButtonColor = new Color(colorSettings[1].R, colorSettings[1].G, colorSettings[1].B, (byte)255);
    }
}

public class ColorData
{
    public byte R { get; set; }
    public byte G { get; set; }
    public byte B { get; set; }
}
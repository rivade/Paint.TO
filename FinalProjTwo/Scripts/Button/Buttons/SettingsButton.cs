namespace DrawingProgram;

public sealed class SettingsButton : Button
{
    private SettingsWindow settingsWindow;

    public SettingsButton(ProgramManager programInstance, Rectangle buttonRect) : base(programInstance, buttonRect)
    {
        settingsWindow = new(programInstance, 1300, 525, ["Settings"]);
        infoWindow = new("Settings", (int)buttonRect.X - Raylib.MeasureText("Settings", InfoText.FontSize) - 20, (int)buttonRect.Y);
        icon = Raylib.LoadTexture("Textures/Icons/settings.png");
    }

    public override void OnClick()
    {
        program.popupWindow = settingsWindow;
    }

    public override void Draw()
    {
        Raylib.DrawRectangleRec(buttonRect, GetButtonColor(Color.Gray, Color.LightGray, Color.White, false));
        base.Draw();
    }
}
namespace DrawingProgram;

public sealed class CloseButton : Button
{
    UserPrefs userPrefs;

    public CloseButton(ProgramManager programInstance, Rectangle buttonRect, UserPrefs userPrefsExtern) : base(programInstance, buttonRect)
    {
        infoWindow = new("Close program", (int)buttonRect.X - Raylib.MeasureText("Close program", InfoText.FontSize) - 20, (int)buttonRect.Y);
        userPrefs = userPrefsExtern;
    }

    public override void OnClick()
    {
        userPrefs.SaveSettings();
        Environment.Exit(0);
    }

    public override void Draw()
    {
        Raylib.DrawRectangleRec(buttonRect, GetButtonColor(Color.Red, Color.Pink, Color.White, false));
        base.Draw();
    }
}
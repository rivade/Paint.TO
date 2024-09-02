namespace DrawingProgram;

public sealed class CloseButton : Button
{
    public CloseButton(ProgramManager programInstance, Rectangle buttonRect) : base(programInstance, buttonRect)
    {
        infoWindow = new("Close program", (int)buttonRect.X - Raylib.MeasureText("Close program", InfoText.FontSize) - 20, (int)buttonRect.Y);
    }

    public override void OnClick()
    {
        UserPrefs.SaveSettings();
        Environment.Exit(0);
    }

    public override void Draw()
    {
        Raylib.DrawRectangleRec(buttonRect, GetButtonColor(Color.Red, Color.Pink, Color.White, false));
        base.Draw();
    }
}
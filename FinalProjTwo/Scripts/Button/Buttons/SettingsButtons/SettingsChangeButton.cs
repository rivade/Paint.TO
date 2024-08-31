using DrawingProgram;

public sealed unsafe class SettingsChangeButton : Button
{
    private string buttonString;
    private string windowString;
    private Color* colorToChange;
    public SettingsChangeButton(ProgramManager programInstance, Rectangle button, string buttonText, string windowText, Color* colorToChangePtr) : base(programInstance, button)
    {
        buttonString = buttonText;
        windowString = windowText;
        colorToChange = colorToChangePtr;
    }

    public override void OnClick()
    {
        program.popupWindow = new ColorSelector(program, 1150, 750, [windowString], colorToChange);
    }

    public override void Draw()
    {
        Raylib.DrawRectangleRec(buttonRect, GetButtonColor(Color.Lime, Color.Green, Color.White, false));
        TextHandling.DrawCenteredTextPro([buttonString], (int)buttonRect.X, (int)buttonRect.X + (int)buttonRect.Width, (int)buttonRect.Y + 25, 40, 0, Color.Black);
    }
}
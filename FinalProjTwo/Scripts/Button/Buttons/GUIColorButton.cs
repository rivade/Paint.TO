namespace DrawingProgram;

public sealed class GUIColorButton : Button
{
    private Action ChangeColor;
    private string text;

    public GUIColorButton(ProgramManager programInstance, Rectangle buttonRect, Action ColorToChange, string buttonText) : base(programInstance, buttonRect)
    {
        ChangeColor = ColorToChange;
        text = buttonText;
    }

    public override void OnClick()
    {
        ChangeColor.Invoke();
    }

    public override void Draw()
    {
        Raylib.DrawRectangleRec(buttonRect, GetButtonColor(Color.Lime, Color.Green, Color.White, false));
        TextHandling.DrawCenteredTextPro([text], (int)buttonRect.X, (int)buttonRect.X + (int)buttonRect.Width, (int)buttonRect.Y + 25, 40, 0, Color.Black);
    }
}
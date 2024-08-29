using DrawingProgram;

public sealed class ChangeBackgroundButton : Button
{
    public ChangeBackgroundButton(ProgramManager programInstance, Rectangle button) : base(programInstance, button)
    {

    }

    public unsafe override void OnClick()
    {
        fixed(Color* colorPtr = &program.canvas.backgroundColor)
        program.popupWindow = new ColorSelector(program, 1150, 750, ["Set BG color"], colorPtr);
    }

    public override void Draw()
    {
        Raylib.DrawRectangleRec(buttonRect, GetButtonColor(Color.Lime, Color.Green, Color.White, false));
        TextHandling.DrawCenteredTextPro(["Change background color"], (int)buttonRect.X, (int)buttonRect.X + (int)buttonRect.Width, (int)buttonRect.Y + 25, 40, 0, Color.Black);
    }
}
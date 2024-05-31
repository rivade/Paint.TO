namespace DrawingProgram;

public sealed class CheckerSizeButton : Button
{
    private ValueSetterWindow valueSetterWindow;

    public CheckerSizeButton(ProgramManager programInstance, Rectangle buttonRect) : base(programInstance, buttonRect)
    {
        valueSetterWindow =
        new(programInstance, 800, 500, ["Set checker size"]) { minValue = 5, maxValue = 20, thisChanges = ValueSetterWindow.Changes.CheckerSize };
    }

    public override void OnClick()
    {
        if (program.currentTool.GetType().Name == "Checker")
            program.popupWindow = valueSetterWindow;
    }

    public override void Draw()
    {
        if (program.currentTool.GetType().Name == "Checker")
        {
            TextHandling.DrawCenteredTextPro(["Checker", "size"], Canvas.CanvasWidth, ProgramManager.ScreenWidth, (int)buttonRect.Y - 65, 30, 30, Color.Black);
            Raylib.DrawRectangleRec(buttonRect, Color.Black);
            Raylib.DrawRectangle((int)buttonRect.X + 5, (int)buttonRect.Y + 5, ButtonSize - 10, ButtonSize - 10, Color.White);
            TextHandling.DrawCenteredTextPro([$"{Checker.checkerSize}"], (int)buttonRect.X, (int)buttonRect.X + (int)buttonRect.Width, (int)buttonRect.Y + 20, 50, 0, Color.Black);
        }
    }
}
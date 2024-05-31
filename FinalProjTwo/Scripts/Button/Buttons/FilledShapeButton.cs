namespace DrawingProgram;

public sealed class FilledShapeButton : Button
{
    public FilledShapeButton(ProgramManager programInstance, Rectangle buttonRect) : base(programInstance, buttonRect) { }

    public override void OnClick()
    {
        if (program.currentTool is ShapeTool && program.currentTool is not LineTool)
            ShapeTool.drawFilled = !ShapeTool.drawFilled;
    }

    public override void Draw()
    {
        if (program.currentTool is ShapeTool && program.currentTool is not LineTool)
        {
            TextHandling.DrawCenteredTextPro(["Filled", "shape"], Canvas.CanvasWidth, ProgramManager.ScreenWidth, (int)buttonRect.Y - 65, 30, 30, Color.Black);
            Raylib.DrawRectangleRec(buttonRect, Color.Black);

            if (ShapeTool.drawFilled)
                Raylib.DrawRectangle((int)buttonRect.X + 5, (int)buttonRect.Y + 5, ButtonSize - 10, ButtonSize - 10, Color.Green);
            else
                Raylib.DrawRectangle((int)buttonRect.X + 5, (int)buttonRect.Y + 5, ButtonSize - 10, ButtonSize - 10, Color.Red);
        }
    }
}
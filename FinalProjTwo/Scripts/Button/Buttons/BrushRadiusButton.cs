namespace DrawingProgram;

public sealed class BrushRadiusButton : Button
{
    private ValueSetterWindow valueSetterWindow;

    public BrushRadiusButton(ProgramManager programInstance, Rectangle buttonRect) : base(programInstance, buttonRect)
    {
        valueSetterWindow =
        new(programInstance, 800, 500, ["Set brush radius"]) { minValue = 1, maxValue = 100, thisChanges = ValueSetterWindow.Changes.BrushRadius };
    }

    public override void OnClick()
    {
        if (ViewConditions())
            program.popupWindow = valueSetterWindow;
    }

    public override void Draw()
    {
        if (ViewConditions())
        {
            TextHandling.DrawCenteredTextPro(["Brush", "radius"], Canvas.CanvasWidth, ProgramManager.ScreenWidth, (int)buttonRect.Y - 65, 30, 30, Color.Black);
            Raylib.DrawRectangleRec(buttonRect, Color.Black);
            Raylib.DrawRectangle((int)buttonRect.X + 5, (int)buttonRect.Y + 5, ButtonSize - 10, ButtonSize - 10, Color.White);
            TextHandling.DrawCenteredTextPro([$"{DrawTool.brushRadius}"], (int)buttonRect.X, (int)buttonRect.X + (int)buttonRect.Width, (int)buttonRect.Y + 20, 50, 0, Color.Black);
        }
    }

    private bool ViewConditions()
    {
        return program.currentTool is not Pencil &&
        program.currentTool is not Bucket &&
        program.currentTool is not EyeDropper &&
        program.currentTool is not RectangleSelect &&
        program.currentTool is not ShapeTool ||
        program.currentTool is LineTool;
    }
}
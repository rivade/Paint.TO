namespace DrawingProgram;

public sealed class ColorSelectorButton : Button
{
    private ColorSelector colorSelectorWindow;

    public ColorSelectorButton(ProgramManager programInstance, Rectangle buttonRect) : base(programInstance, buttonRect)
    {
        colorSelectorWindow = new(programInstance, 1150, 750, ["Select a color"]);
    }

    public override void OnClick()
    {
        program.popupWindow = colorSelectorWindow;
    }

    public override void Draw()
    {
        Color colorPreview = DrawTool.drawingColor;
        colorPreview.A = 255;
        Raylib.DrawText("Color", Canvas.CanvasWidth + 62, (int)buttonRect.Y - 35, 30, Color.Black);
        Raylib.DrawRectangleRec(buttonRect, Color.Black);
        Raylib.DrawRectangle((int)buttonRect.X + 5, (int)buttonRect.Y + 5, ButtonSize - 10, ButtonSize - 10, colorPreview);
    }
}
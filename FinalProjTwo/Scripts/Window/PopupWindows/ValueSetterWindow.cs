namespace DrawingProgram;

public sealed class ValueSetterWindow : PopupWindow
{
    public enum Changes
    {
        BrushRadius,
        CheckerSize
    }

    public Changes thisChanges { get; set; }

    private Slider slider;

    public int minValue { get; set; }
    public int maxValue { get; set; }
    private int value;

    public ValueSetterWindow(ProgramManager programInstance, int width, int height, string[] messagesExtern) : base(programInstance, width, height, messagesExtern)
    {
        int sliderWidth = 500;
        int sliderHeight = 15;

        int sliderX = (int)(windowRect.X + windowRect.Width / 2 - sliderWidth / 2);

        slider = new(20, new(sliderX, 475, sliderWidth, sliderHeight));
    }

    public override void Logic(Canvas canvas, Vector2 mousePos)
    {
        base.Logic(canvas, mousePos);
        value = slider.GetValue(mousePos, minValue, maxValue);

        switch (thisChanges)
        {
            case Changes.BrushRadius:
                DrawTool.brushRadius = value;
                break;
            case Changes.CheckerSize:
                Checker.checkerSize = value;
                break;
        }
    }

    public override void Draw()
    {
        base.Draw();
        slider.Draw();

        switch (thisChanges)
        {
            case Changes.BrushRadius:
                Color colorPreview = DrawTool.drawingColor;
                colorPreview.A = 255;
                Raylib.DrawCircle(ProgramManager.ScreenWidth / 2, 650, value, colorPreview);
                break;
            case Changes.CheckerSize:
                CheckerPreview.DrawCheckerPreview();
                break;
        }
    }

    public void SetSlider(int value)
    {
        float relativePosition = (value - minValue) / (float)(maxValue - minValue);
        float sliderX = slider.SliderBar.X + relativePosition * slider.SliderBar.Width;

        slider.SliderCircle.Middle = new Vector2(sliderX, slider.SliderCircle.Middle.Y);
    }
}

public static class CheckerPreview
{
    public static void DrawCheckerPreview()
    {
        int centerX = ProgramManager.ScreenWidth / 2;
        int centerY = 650;

        int rows = (int)Math.Ceiling(200d / Checker.checkerSize);
        int cols = (int)Math.Ceiling(200d / Checker.checkerSize);

        Color colorPreview = DrawTool.drawingColor;
        colorPreview.A = 255;

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                int xPos = col * Checker.checkerSize;
                int yPos = row * Checker.checkerSize;

                int xOffset = (200 - (cols * Checker.checkerSize)) / 2;
                int yOffset = (200 - (rows * Checker.checkerSize)) / 2;

                xPos += xOffset + centerX - 100;
                yPos += yOffset + centerY - 100;

                if ((row + col) % 2 == 0)
                    Raylib.DrawRectangle(xPos, yPos, Checker.checkerSize, Checker.checkerSize, DrawTool.drawingColor);
            }
        }
    }
}
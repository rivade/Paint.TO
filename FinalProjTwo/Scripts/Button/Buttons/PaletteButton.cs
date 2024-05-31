namespace DrawingProgram;

public sealed class PaletteButton : Button
{
    public Color paletteColor;
    public ColorSelector window;
    public static Queue<Color> paletteColors = new([Color.Black, Color.Black, Color.Black, Color.Black]);

    public PaletteButton(ProgramManager programInstance, ColorSelector windowInstance, Rectangle button) : base(programInstance, button)
    {
        window = windowInstance;
    }

    private static void LimitQueueSize()
    {
        if (paletteColors.Count > 4) paletteColors.Dequeue();
    }

    public static void UpdatePalette()
    {
        if (!paletteColors.Contains(DrawTool.drawingColor))
        {
            paletteColors.Enqueue(DrawTool.drawingColor);
            LimitQueueSize();
        }
    }

    public override void OnClick()
    {
        DrawTool.drawingColor = paletteColor;
        window.SetSliders();
    }

    public override void Draw()
    {
        Raylib.DrawRectangle((int)buttonRect.X - 5, (int)buttonRect.Y - 5, (int)buttonRect.Width + 10, (int)buttonRect.Height + 10, Color.White);
        Raylib.DrawRectangleRec(buttonRect, paletteColor);
    }
}
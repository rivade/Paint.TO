namespace DrawingProgram;

public abstract class DrawTool : ITool
{
    public static Color drawingColor = Color.Black;
    public static int brushRadius = 1;

    protected static readonly object lockObj = new();

    protected static Vector2 lastMousePos;
    public static void UpdateLastMousePos(Vector2 mousePos) => lastMousePos = mousePos + Vector2.One * Canvas.CanvasOffset;

    public virtual void Update(Image canvas, Vector2 mousePos)
    {
        if (Raylib.IsMouseButtonPressed(MouseButton.Left))
        {
            PaletteButton.UpdatePalette();
        }
    }

    // Bresenhams line algorithm
    public static void DrawThickLine(Image canvas, Vector2 startPos, Vector2 endPos, Color color, bool drawOnCanvas)
    {
        int x = (int)startPos.X;
        int y = (int)startPos.Y;

        int dx = Math.Abs((int)endPos.X - x);
        int dy = Math.Abs((int)endPos.Y - y);

        int sx = x < (int)endPos.X ? 1 : -1;
        int sy = y < (int)endPos.Y ? 1 : -1;

        int error = dx - dy;

        while (true)
        {
            if (drawOnCanvas)
                Raylib.ImageDrawCircleV(ref canvas, new Vector2(x, y), brushRadius, color);
            else
                Raylib.DrawCircleV(new Vector2(x, y), brushRadius, color);

            if (x == (int)endPos.X && y == (int)endPos.Y)
                break;

            int doubleError = 2 * error;
            if (doubleError > -dy)
            {
                error -= dy;
                x += sx;
            }
            if (doubleError < dx)
            {
                error += dx;
                y += sy;
            }
        }
    }
}
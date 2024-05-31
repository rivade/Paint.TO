namespace DrawingProgram;

public sealed class RectangleTool : ShapeTool
{
    public static Rectangle rectToDraw;

    protected override void DrawShape(Image canvas, Vector2 mousePos, Vector2 lastMousePos)
    {
        base.DrawShape(canvas, mousePos, lastMousePos);

        if (Raylib.IsMouseButtonDown(MouseButton.Left))
            UpdateRect(mousePos);

        if (Raylib.IsMouseButtonReleased(MouseButton.Left))
        {
            if (drawFilled)
                Raylib.ImageDrawRectangle(ref canvas, (int)rectToDraw.X, (int)rectToDraw.Y,
                (int)rectToDraw.Width, (int)rectToDraw.Height, drawingColor);
            else
                Raylib.ImageDrawRectangleLines(ref canvas, rectToDraw, 1, drawingColor);

            rectToDraw = new(Vector2.Zero, Vector2.Zero);
        }
    }

    private void UpdateRect(Vector2 mousePos)
    {
        int x = Math.Min((int)startPos.X, (int)mousePos.X);
        int y = Math.Min((int)startPos.Y, (int)mousePos.Y);
        int width = Math.Abs((int)mousePos.X - (int)startPos.X);
        int height = Math.Abs((int)mousePos.Y - (int)startPos.Y);
        rectToDraw = new Rectangle(x, y, width, height);
    }
}
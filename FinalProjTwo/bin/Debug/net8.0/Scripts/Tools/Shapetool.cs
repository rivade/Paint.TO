namespace DrawingProgram;

public abstract class ShapeTool : DrawTool
{
    protected Vector2 startPos;
    public static bool drawFilled = true;

    public override void Stroke(Image canvas, Vector2 mousePos, Vector2 lastMousePos)
    {
        DrawShape(canvas, mousePos, lastMousePos);
    }

    protected virtual void DrawShape(Image canvas, Vector2 mousePos, Vector2 lastMousePos)
    {
        if (Raylib.IsMouseButtonPressed(MouseButton.Left))
            startPos = mousePos;
    }
}

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

public sealed class LineTool : ShapeTool
{
    public static Line lineToDraw = new(new Vector2(-10000, -10000), new Vector2(-10000, -10000));

    protected override void DrawShape(Image canvas, Vector2 mousePos, Vector2 lastMousePos)
    {
        base.DrawShape(canvas, mousePos, lastMousePos);
        if (Raylib.IsMouseButtonDown(MouseButton.Left))
            lineToDraw = new(startPos, mousePos);

        if (Raylib.IsMouseButtonReleased(MouseButton.Left))
        {
            DrawThickLine(canvas, lineToDraw.startPos, lineToDraw.endPos, drawingColor, true);
            lineToDraw = new(new Vector2(-10000, -10000), new Vector2(-10000, -10000));
        }
    }
}

public sealed class CircleTool : ShapeTool
{
    public static Circle circleToDraw;

    protected override void DrawShape(Image canvas, Vector2 mousePos, Vector2 lastMousePos)
    {
        base.DrawShape(canvas, mousePos, lastMousePos);
        if (Raylib.IsMouseButtonDown(MouseButton.Left))
            circleToDraw = Circle.CreateCircle(startPos, mousePos);

        if (Raylib.IsMouseButtonReleased(MouseButton.Left))
        {
            if (drawFilled)
                Raylib.ImageDrawCircleV(ref canvas, circleToDraw.Middle, circleToDraw.Radius, drawingColor);
            else
            {
                unsafe //av någon anledning är imagedrawcirclelines jättedåligt konstruerad så detta behövs
                {
                    Raylib.ImageDrawCircleLinesV(&canvas, circleToDraw.Middle, circleToDraw.Radius, drawingColor);
                }
                //i imagedrawcirclelines går det ej att passera imagen med ref, så canvas behövs passeras med en pointer som är 'unsafe'
            }
            circleToDraw = new();
        }
    }
}
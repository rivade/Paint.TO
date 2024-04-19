namespace DrawingProgram;

public abstract class ShapeTool : DrawTool
{
    protected enum Shapes
    {
        Rectangle,
        Line,
        Circle
    }
    protected Shapes shape;

    public static Vector2 startPos;
    public static Rectangle tempRectangle;
    public static Line tempLine = new(new Vector2(-10000, -10000), new Vector2(-10000, -10000));
    public static Circle tempCircle;
    public static bool drawFilled = true;

    public override void Stroke(Image canvas, Vector2 mousePos, Vector2 lastMousePos)
    {
        if (Raylib.IsMouseButtonPressed(MouseButton.Left))
            startPos = mousePos;

        if (Raylib.IsMouseButtonDown(MouseButton.Left))
        {
            switch (shape)
            {
                case Shapes.Rectangle:
                    UpdateTempRectangle(mousePos);
                    break;

                case Shapes.Line:
                    tempLine = new(startPos, mousePos);
                    break;

                case Shapes.Circle:
                    tempCircle = Circle.CreateCircle(startPos, mousePos);
                    break;

            }
        }

        if (Raylib.IsMouseButtonReleased(MouseButton.Left))
        {
            switch (shape)
            {
                case Shapes.Rectangle:
                    if (drawFilled)
                        Raylib.ImageDrawRectangle(ref canvas, (int)tempRectangle.X, (int)tempRectangle.Y,
                        (int)tempRectangle.Width, (int)tempRectangle.Height, drawingColor);
                    else
                        Raylib.ImageDrawRectangleLines(ref canvas, tempRectangle, 1, drawingColor);

                    tempRectangle = new(Vector2.Zero, Vector2.Zero);
                    break;

                case Shapes.Line:
                    DrawThickLine(canvas, tempLine.startPos, tempLine.endPos, drawingColor, true);

                    tempLine = new(new Vector2(-10000, -10000), new Vector2(-10000, -10000));
                    break;

                case Shapes.Circle:
                    if (drawFilled)
                        Raylib.ImageDrawCircleV(ref canvas, tempCircle.Middle, tempCircle.Radius, drawingColor);
                    else
                    {
                        unsafe //av någon anledning är imagedrawcirclelines jättedåligt konstruerad så detta behövs
                        {
                            Raylib.ImageDrawCircleLinesV(&canvas, tempCircle.Middle, tempCircle.Radius, drawingColor);
                        }
                        //i imagedrawcirclelines går det ej att passera imagen med ref, så canvas behövs passeras med en pointer som är 'unsafe'
                    }

                    tempCircle = new();
                    break;
            }
        }
    }

    private void UpdateTempRectangle(Vector2 mousePos)
    {
        int x = Math.Min((int)startPos.X, (int)mousePos.X);
        int y = Math.Min((int)startPos.Y, (int)mousePos.Y);
        int width = Math.Abs((int)mousePos.X - (int)startPos.X);
        int height = Math.Abs((int)mousePos.Y - (int)startPos.Y);
        tempRectangle = new Rectangle(x, y, width, height);
    }
}

public sealed class RectangleTool : ShapeTool
{
    public RectangleTool()
    {
        shape = Shapes.Rectangle;
    }
}

public sealed class LineTool : ShapeTool
{
    public LineTool()
    {
        shape = Shapes.Line;
    }
}

public sealed class CircleTool : ShapeTool
{
    public CircleTool()
    {
        shape = Shapes.Circle;
    }
}
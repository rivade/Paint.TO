namespace DrawingProgram;

public sealed class CircleTool : ShapeTool
{
    public static Circle circleToDraw;

    protected override void DrawShape(Image canvas, Vector2 mousePos, Vector2 lastMousePos)
    {
        base.DrawShape(canvas, mousePos, lastMousePos);
        if (Raylib.IsMouseButtonDown(MouseButton.Left))
            circleToDraw = new(startPos, mousePos);

        if (Raylib.IsMouseButtonReleased(MouseButton.Left))
        {
            if (drawFilled)
                Raylib.ImageDrawCircleV(ref canvas, circleToDraw.Middle + Vector2.One * Canvas.CanvasOffset, circleToDraw.Radius, drawingColor);

            else
            {
                unsafe //For some reason, you can't pass the target image with the ref keyword in ImageDrawCircleLines
                //Therefore it needs to be passed with a pointer (guessing this is due to a bad port of raylib from C++ lol)
                {
                    Raylib.ImageDrawCircleLinesV(&canvas, circleToDraw.Middle + Vector2.One * Canvas.CanvasOffset, circleToDraw.Radius, drawingColor);
                }
            }
            circleToDraw = new();
        }
    }
}
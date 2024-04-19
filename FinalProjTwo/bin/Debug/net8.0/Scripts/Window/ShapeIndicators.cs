namespace DrawingProgram;

public class ShapeIndicators : IDrawable
{
    public void Draw()
    {
        DrawTool.DrawThickLine(new(), ShapeTool.tempLine.startPos, ShapeTool.tempLine.endPos, DrawTool.drawingColor, false);

        if (ShapeTool.drawFilled)
        {
            Raylib.DrawRectangleRec(ShapeTool.tempRectangle, DrawTool.drawingColor);
            Raylib.DrawCircle((int)ShapeTool.tempCircle.Middle.X, (int)ShapeTool.tempCircle.Middle.Y, ShapeTool.tempCircle.Radius, DrawTool.drawingColor);
        }
        else
        {
            Raylib.DrawRectangleLines((int)ShapeTool.tempRectangle.X, (int)ShapeTool.tempRectangle.Y, (int)ShapeTool.tempRectangle.Width, (int)ShapeTool.tempRectangle.Height, DrawTool.drawingColor);
            Raylib.DrawCircleLines((int)ShapeTool.tempCircle.Middle.X, (int)ShapeTool.tempCircle.Middle.Y, ShapeTool.tempCircle.Radius, DrawTool.drawingColor);
        }
    }
}
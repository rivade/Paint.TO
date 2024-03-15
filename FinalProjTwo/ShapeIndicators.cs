namespace DrawingProgram;

public class ShapeIndicators : IDrawable
{
    public void Draw()
    {
        Raylib.DrawRectangleRec(ShapeTool.tempRectangle, DrawTool.drawingColor);
        DrawTool.DrawThickLine(new(), ShapeTool.tempLine.startPos, ShapeTool.tempLine.endPos, DrawTool.drawingColor, false);
        Raylib.DrawCircle((int)ShapeTool.tempCircle.Middle.X, (int)ShapeTool.tempCircle.Middle.Y, ShapeTool.tempCircle.Radius, DrawTool.drawingColor);
    }
}
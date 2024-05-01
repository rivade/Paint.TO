namespace DrawingProgram;

public class ShapeToolPreviews : IDrawable
{
    private ProgramManager program;

    private Rectangle previewRect;
    private Circle previewCircle;
    private Line previewLine;

    public ShapeToolPreviews(ProgramManager programInstance)
    {
        program = programInstance;
    }

    private void SetPreviewShapes()
    {
        previewRect = new(RectangleTool.rectToDraw.X - Canvas.CanvasOffset,
                        RectangleTool.rectToDraw.Y - Canvas.CanvasOffset,
                        RectangleTool.rectToDraw.Width,
                        RectangleTool.rectToDraw.Height);

        previewCircle = new(CircleTool.circleToDraw.Middle -= Vector2.One * Canvas.CanvasOffset,
                            CircleTool.circleToDraw.Middle + Vector2.UnitX * CircleTool.circleToDraw.Radius);

        previewLine = new(LineTool.lineToDraw.StartPos - Vector2.One * Canvas.CanvasOffset, LineTool.lineToDraw.EndPos - Vector2.One * Canvas.CanvasOffset);
    }

    public void Draw()
    {
        SetPreviewShapes();
        switch (program.currentTool)
        {
            case RectangleTool:
                if (ShapeTool.drawFilled)
                    Raylib.DrawRectangleRec(previewRect, DrawTool.drawingColor);
                else Raylib.DrawRectangleLines((int)previewRect.X, (int)previewRect.Y, (int)previewRect.Width, (int)previewRect.Height, DrawTool.drawingColor);
                break;

            case CircleTool:
                if (ShapeTool.drawFilled)
                    Raylib.DrawCircle((int)previewCircle.Middle.X, (int)previewCircle.Middle.Y, previewCircle.Radius, DrawTool.drawingColor);
                else Raylib.DrawCircleLines((int)previewCircle.Middle.X, (int)previewCircle.Middle.Y, previewCircle.Radius, DrawTool.drawingColor);
                break;

            case LineTool:
                DrawTool.DrawThickLine(new Image(), previewLine.StartPos, previewLine.EndPos, DrawTool.drawingColor, false);
                break;

            default: return;
        }
    }
}
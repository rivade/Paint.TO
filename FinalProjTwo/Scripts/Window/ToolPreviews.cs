namespace DrawingProgram;

public class ToolPreviews : IDrawable
{
    private ProgramManager program;
    private RectangleSelect selectionTool;

    private Rectangle previewRect;
    private Circle previewCircle;
    private Line previewLine;
    private PerspectiveCamera camera;

    public ToolPreviews(ProgramManager programInstance, RectangleSelect selectionToolInstance, PerspectiveCamera cameraExtern)
    {
        program = programInstance;
        selectionTool = selectionToolInstance;
        camera = cameraExtern;
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

            case RectangleSelect:
                Raylib.DrawTexture(selectionTool.selectionPreview, (int)selectionTool.selectionRec.X, (int)selectionTool.selectionRec.Y, Color.White);
                Raylib.DrawRectangleRec(selectionTool.selectionRec, RectangleSelect.selectionColor);
                selectionTool.corners?.ForEach(c => c.Draw());
                break;

            case PaintBrush: case Eraser: case Checker:
                Vector2 mousePos = camera.projectCameraPointToCanvas(Raylib.GetMousePosition());
                Raylib.DrawCircleLines((int)mousePos.X, (int)mousePos.Y, DrawTool.brushRadius, Color.Black);
                break;


            default: return;
        }
    }
}
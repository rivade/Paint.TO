namespace DrawingProgram;

public class ShapeToolPreviews : IDrawable
{
    private ProgramManager program;

    public ShapeToolPreviews(ProgramManager programInstance)
    {
        program = programInstance;
    }

    public void Draw()
    {
        switch(program.currentTool)
        {
            case RectangleTool:
                if (ShapeTool.drawFilled)
                    Raylib.DrawRectangleRec(RectangleTool.rectToDraw, DrawTool.drawingColor);   
                else Raylib.DrawRectangleLines((int)RectangleTool.rectToDraw.X, (int)RectangleTool.rectToDraw.Y, (int)RectangleTool.rectToDraw.Width, (int)RectangleTool.rectToDraw.Height, DrawTool.drawingColor); 
                break;

            case CircleTool:
                if (ShapeTool.drawFilled)
                    Raylib.DrawCircle((int)CircleTool.circleToDraw.Middle.X, (int)CircleTool.circleToDraw.Middle.Y, CircleTool.circleToDraw.Radius, DrawTool.drawingColor);
                else Raylib.DrawCircleLines((int)CircleTool.circleToDraw.Middle.X, (int)CircleTool.circleToDraw.Middle.Y, CircleTool.circleToDraw.Radius, DrawTool.drawingColor);
                break;

            case LineTool:
                DrawTool.DrawThickLine(new Image(), LineTool.lineToDraw.startPos, LineTool.lineToDraw.endPos, DrawTool.drawingColor, false);
                break;
        
            default: return;       
        }
    }
}
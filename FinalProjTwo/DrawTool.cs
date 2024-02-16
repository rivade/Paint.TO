namespace MyGame;

public abstract class DrawTool
{
    protected Vector2 mousePos;
    protected Vector2 lastMousePos;
    //protected Button
}

public class Pencil : DrawTool, ICanDraw
{
    public void Draw(Color drawingColor)
    {  
        if (Raylib.IsMouseButtonDown(MouseButton.Left))
        {
            mousePos = Raylib.GetMousePosition();
            Raylib.ImageDrawRectangleV(ref ProgramManager.canvas, mousePos, new Vector2(2, 2), drawingColor);
            Raylib.ImageDrawLine(ref ProgramManager.canvas, (int)lastMousePos.X, (int)lastMousePos.Y, (int)mousePos.X, (int)mousePos.Y, drawingColor);
            
            lastMousePos = mousePos;
        }
    }
}

public class Eraser : DrawTool, ICanDraw
{
    public void Draw(Color drawingColor)
    {

    }
}
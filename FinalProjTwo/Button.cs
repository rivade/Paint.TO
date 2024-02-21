namespace DrawingProgram;

public abstract class Button
{
    public Rectangle buttonRect { get; set; }
    public Color buttonColor = Color.Red;
}

public class ToolButton : Button, IMouseInteractable, IDrawable
{
    public DrawTool DrawTool { get; set; }

    public void OnHover(Vector2 mousePos)
    {
        if (Raylib.CheckCollisionPointRec(mousePos, buttonRect) && Raylib.IsMouseButtonPressed(MouseButton.Left))
        {
            OnClick();
        }
    }

    public void OnClick()
    {
        ProgramManager.currentTool = (DrawTool != ProgramManager.currentTool) ? DrawTool : ProgramManager.currentTool;
    }

    public void Draw()
    {
        Raylib.DrawRectangleRec(buttonRect, buttonColor);
    }
}
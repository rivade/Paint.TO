namespace DrawingProgram;

public abstract class Button
{
    public Rectangle buttonRect { get; set; }
    public Color buttonColor { get; set; }
    protected bool isHoveredOn;
}

public class ToolButton : Button, IMouseInteractable, IDrawable
{
    public DrawTool DrawTool { get; set; }

    public void OnHover(Vector2 mousePos)
    {
        isHoveredOn = false;
        if (Raylib.CheckCollisionPointRec(mousePos, buttonRect))
        {
            isHoveredOn = true;

            if (Raylib.IsMouseButtonPressed(MouseButton.Left))
                OnClick();
        }
    }

    public void OnClick()
    {
        ProgramManager.currentTool = (DrawTool != ProgramManager.currentTool) ? DrawTool : ProgramManager.currentTool;
    }

    public void Draw()
    {
        GetButtonColor();
        Raylib.DrawRectangleRec(buttonRect, buttonColor);
    }

    private void GetButtonColor()
    {
        buttonColor = Color.Red;

        if (isHoveredOn)
            buttonColor = Color.Brown;
    }
}
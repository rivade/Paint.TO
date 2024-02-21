namespace DrawingProgram;

public abstract class Button
{
    public Rectangle buttonRect;
    protected Color buttonColor;
    protected bool isHoveredOn;
}

public class ToolButton : Button, IMouseInteractable, IDrawable
{
    public const int buttonSize = 80;

    public DrawTool DrawTool { get; set; }

    private bool IsActiveTool() => ProgramManager.currentTool == DrawTool;

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
        if (!IsActiveTool())
            ProgramManager.currentTool = DrawTool;
    }

    public void Draw()
    {
        GetButtonColor();
        Raylib.DrawRectangleRec(buttonRect, buttonColor);
    }


    private void GetButtonColor()
    {
        buttonColor = Color.Lime;

        if (isHoveredOn && !IsActiveTool())
            buttonColor = Color.Green;

        else if (IsActiveTool())
            buttonColor = Color.DarkGreen;
    }
}
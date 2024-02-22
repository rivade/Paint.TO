namespace DrawingProgram;

public abstract class Button : IHoverable, IClickable
{
    public Rectangle buttonRect;
    protected Color buttonColor;
    protected bool isHoveredOn;

    public virtual void OnHover(Vector2 mousePos)
    {
        isHoveredOn = false;
        if (Raylib.CheckCollisionPointRec(mousePos, buttonRect))
        {
            isHoveredOn = true;

            if (Raylib.IsMouseButtonPressed(MouseButton.Left))
                OnClick();
        }
    }

    public virtual void OnClick()
    {

    }

    protected void GetButtonColor(Color defaultColor, Color hoverColor, Color activeColor, bool condition) //Condition lämnas false om knappen ej kan vara aktiv
    {
        buttonColor = defaultColor;

        if (isHoveredOn && !condition)
            buttonColor = hoverColor;

        else if (condition)
            buttonColor = activeColor;
    }
}

public class ToolButton : Button, IHoverable, IClickable, IDrawable
{
    public const int buttonSize = 80;

    public DrawTool DrawTool { get; set; }

    private bool IsActiveTool() => ProgramManager.currentTool == DrawTool;

    public override void OnClick()
    {
        if (!IsActiveTool())
            ProgramManager.currentTool = DrawTool;
    }

    public void Draw()
    {
        GetButtonColor(Color.Lime, Color.Green, Color.DarkGreen, IsActiveTool());
        Raylib.DrawRectangleRec(buttonRect, buttonColor);
    }
}

/* public class ColorSelectorButton : Button, IMouseInteractable, IDrawable
{
    public const int buttonSize = 80;
    public Color[] colors = { Color.Black, Color.Red, Color.Orange, Color.Yellow, Color.Green, Color.Blue, Color.DarkBlue, Color.Violet, Color.Beige, Color.Brown};

    public void OnClick => Progra
} */

public class SaveCanvasButton : Button, IHoverable, IClickable, IDrawable
{
    public const int buttonSize = 80;

    

    public override void OnClick()
    {
        
    }
    public void Draw()
    {
        GetButtonColor(Color.Orange, Color.Yellow, Color.White, false);
    }
}
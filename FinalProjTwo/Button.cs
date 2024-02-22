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

public class ColorSelectorButton : Button, IHoverable, IClickable, IDrawable
{
    public const int buttonSize = 80;

    

    public override void OnClick()
    {
        DrawTool.colorInt++;
    }

    public void Draw()
    {
        Raylib.DrawText("Color", ProgramManager.CanvasWidth + 12, (int)buttonRect.Y - 35, 30, Color.Black);
        Raylib.DrawRectangleRec(buttonRect, Color.Black);
        Raylib.DrawRectangle((int)buttonRect.X + 5, (int)buttonRect.Y + 5, buttonSize - 10, buttonSize - 10, DrawTool.DrawingColor);
    }

}

public class BrushRadiusButton : Button, IHoverable, IClickable, IDrawable
{
    public const int buttonSize = 80;

    

    public override void OnClick()
    {
        DrawTool.brushRadiusSelectorInt++;
    }

    public void Draw()
    {
        Raylib.DrawText("Size", ProgramManager.CanvasWidth + 17, (int)buttonRect.Y - 35, 30, Color.Black);
        Raylib.DrawRectangleRec(buttonRect, Color.Black);
        Raylib.DrawRectangle((int)buttonRect.X + 5, (int)buttonRect.Y + 5, buttonSize - 10, buttonSize - 10, Color.White);
        Raylib.DrawText($"{DrawTool.brushRadius}", (int)buttonRect.X + 10, (int)buttonRect.Y + 20, 50, Color.Black);
    }

}

/* public class SaveCanvasButton : Button, IHoverable, IClickable, IDrawable
{
    public const int buttonSize = 80;

    

    public override void OnClick()
    {
        
    }
    public void Draw()
    {
        GetButtonColor(Color.Orange, Color.Yellow, Color.White, false);
    }
} */
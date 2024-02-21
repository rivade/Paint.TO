namespace DrawingProgram;

public abstract class Button : IMouseInteractable
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
}

public class ToolButton : Button, IMouseInteractable, IDrawable
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

/* public class ColorSelectorButton : Button, IMouseInteractable, IDrawable
{
    public const int buttonSize = 80;
    public Color[] colors = { Color.Black, Color.Red, Color.Orange, Color.Yellow, Color.Green, Color.Blue, Color.DarkBlue, Color.Violet, Color.Beige, Color.Brown};

    public void OnClick => Progra
} */

public class SaveCanvasButton : Button, IMouseInteractable, IDrawable
{
    public const int buttonSize = 80;

    public void SaveProject(Image canvas)
    {
        string fileName = "drawing.png";
        Raylib.ExportImage(canvas, fileName); // Save canvas as PNG file
        Console.WriteLine($"Canvas saved as {fileName}");
    }

    public override void OnClick()
    {
        
    }
    public void Draw()
    {
        
    }
}
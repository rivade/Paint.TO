using System.Collections.ObjectModel;

namespace DrawingProgram;

public abstract class Button : IHoverable
{
    public const int buttonSize = 80;
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

public class ToolButton : Button, IHoverable, IDrawable
{
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

public class ColorSelectorButton : Button, IHoverable, IDrawable
{
    public override void OnClick()
    {
        DrawTool.colorInt++;
    }

    public void Draw()
    {
        Raylib.DrawText("Color", Canvas.CanvasWidth + 62, (int)buttonRect.Y - 35, 30, Color.Black);
        Raylib.DrawRectangleRec(buttonRect, Color.Black);
        Raylib.DrawRectangle((int)buttonRect.X + 5, (int)buttonRect.Y + 5, buttonSize - 10, buttonSize - 10, DrawTool.DrawingColor);
    }
}


public class BrushRadiusButton : Button, IHoverable, IDrawable
{
    public override void OnClick()
    {
        DrawTool.brushRadiusSelectorInt++;
    }

    public void Draw()
    {
        Raylib.DrawText("Size", Canvas.CanvasWidth + 67, (int)buttonRect.Y - 35, 30, Color.Black);
        Raylib.DrawRectangleRec(buttonRect, Color.Black);
        Raylib.DrawRectangle((int)buttonRect.X + 5, (int)buttonRect.Y + 5, buttonSize - 10, buttonSize - 10, Color.White);
        Raylib.DrawText($"{DrawTool.brushRadius}", (int)buttonRect.X + 10, (int)buttonRect.Y + 20, 50, Color.Black);
    }

}

public class SaveCanvasButton : Button, IDrawable, IHoverable
{
    public SavePopup CreatePopup(Vector2 mousePos, Canvas canvas)
    {
        if (Raylib.CheckCollisionPointRec(mousePos, buttonRect) && Raylib.IsMouseButtonPressed(MouseButton.Left))
        {
            SavePopup popup = new(500, 300, ["Select file name ", "Press enter to save", "Press ESC to close"], canvas);

            return popup;
        }

        return null;
    }

    public void Draw()
    {
        GetButtonColor(Color.Orange, Color.Yellow, Color.White, false);
        Raylib.DrawRectangleRec(buttonRect, buttonColor);
    }
}

public class CloseButton : Button, IDrawable, IHoverable
{
    public override void OnClick()
    {
        Environment.Exit(0);
    }

    public void Draw()
    {
        Raylib.DrawRectangleRec(buttonRect, Color.Red);
    }
}
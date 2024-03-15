using System.Collections.ObjectModel;
using System.IO.Compression;

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
    private ColorSelector colorSelectorWindow = new(660, 750, ["Select a color", "Press ESC/Enter to close"], null);

    public override void OnClick()
    {
        ProgramManager.popupWindow = colorSelectorWindow;
    }

    public void Draw()
    {
        Raylib.DrawText("Color", Canvas.CanvasWidth + 62, (int)buttonRect.Y - 35, 30, Color.Black);
        Raylib.DrawRectangleRec(buttonRect, Color.Black);
        Raylib.DrawRectangle((int)buttonRect.X + 5, (int)buttonRect.Y + 5, buttonSize - 10, buttonSize - 10, DrawTool.drawingColor);
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
        Raylib.DrawText("Brush", Canvas.CanvasWidth + 56, (int)buttonRect.Y - 65, 30, Color.Black);
        Raylib.DrawText("size", Canvas.CanvasWidth + 70, (int)buttonRect.Y - 35, 30, Color.Black);
        Raylib.DrawRectangleRec(buttonRect, Color.Black);
        Raylib.DrawRectangle((int)buttonRect.X + 5, (int)buttonRect.Y + 5, buttonSize - 10, buttonSize - 10, Color.White);
        Raylib.DrawText($"{DrawTool.brushRadius}", (int)buttonRect.X + 10, (int)buttonRect.Y + 20, 50, Color.Black);
    }

}

public class CheckerSizeButton : Button, IDrawable, IHoverable
{
    public override void OnClick()
    {
        if (ProgramManager.currentTool.GetType().Name == "Checker")
            Checker.checkerSizeInt++;
    }

    public void Draw()
    {
        if (ProgramManager.currentTool.GetType().Name == "Checker")
        {
            Raylib.DrawText("Checker", Canvas.CanvasWidth + 45, (int)buttonRect.Y - 65, 30, Color.Black);
            Raylib.DrawText("size", Canvas.CanvasWidth + 70, (int)buttonRect.Y - 35, 30, Color.Black);
            Raylib.DrawRectangleRec(buttonRect, Color.Black);
            Raylib.DrawRectangle((int)buttonRect.X + 5, (int)buttonRect.Y + 5, buttonSize - 10, buttonSize - 10, Color.White);
            Raylib.DrawText($"{Checker.checkerSize}", (int)buttonRect.X + 10, (int)buttonRect.Y + 20, 50, Color.Black);
        }
    }
}

public class SaveCanvasButton : Button, IDrawable, IHoverable
{
    private SavePopup saveWindow = new(500, 300, ["Select file name ", "Press enter to save", "Press ESC to close"]);

    public override void OnClick()
    {
        ProgramManager.popupWindow = saveWindow;
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
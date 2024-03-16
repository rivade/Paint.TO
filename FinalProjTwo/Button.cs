using System.Collections.ObjectModel;
using System.IO.Compression;
using System.Security.Principal;

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
    private ColorSelector colorSelectorWindow = new(660, 750, ["Select a color", "Press ESC/Enter to close"]);

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
        if (ProgramManager.currentTool.GetType().Name != "Pencil" && 
        ProgramManager.currentTool.GetType().Name != "Bucket" && 
        ProgramManager.currentTool.GetType().Name != "EyeDropper" && 
        ProgramManager.currentTool is not ShapeTool ||
        ProgramManager.currentTool is LineTool)
        {
            TextHandling.DrawCenteredTextPro(["Brush", "radius"], Canvas.CanvasWidth, ProgramManager.ScreenWidth, (int)buttonRect.Y - 65, 30, 30, Color.Black);
            Raylib.DrawRectangleRec(buttonRect, Color.Black);
            Raylib.DrawRectangle((int)buttonRect.X + 5, (int)buttonRect.Y + 5, buttonSize - 10, buttonSize - 10, Color.White);
            TextHandling.DrawCenteredTextPro([$"{DrawTool.brushRadius}"], (int)buttonRect.X, (int)buttonRect.X + (int)buttonRect.Width, (int)buttonRect.Y + 20, 50, 0, Color.Black);
        }
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
            TextHandling.DrawCenteredTextPro(["Checker", "size"], Canvas.CanvasWidth, ProgramManager.ScreenWidth, (int)buttonRect.Y - 65, 30, 30, Color.Black);
            Raylib.DrawRectangleRec(buttonRect, Color.Black);
            Raylib.DrawRectangle((int)buttonRect.X + 5, (int)buttonRect.Y + 5, buttonSize - 10, buttonSize - 10, Color.White);
            TextHandling.DrawCenteredTextPro([$"{Checker.checkerSize}"], (int)buttonRect.X, (int)buttonRect.X + (int)buttonRect.Width, (int)buttonRect.Y + 20, 50, 0, Color.Black);
        }
    }
}

public class FilledShapeButton : Button, IDrawable, IHoverable
{
    public override void OnClick()
    {
        if (ProgramManager.currentTool is ShapeTool && ProgramManager.currentTool is not LineTool)
            ShapeTool.drawFilled = !ShapeTool.drawFilled;
    }

    public void Draw()
    {
        if (ProgramManager.currentTool is ShapeTool && ProgramManager.currentTool is not LineTool)
        {
            TextHandling.DrawCenteredTextPro(["Filled", "shape"], Canvas.CanvasWidth, ProgramManager.ScreenWidth, (int)buttonRect.Y - 65, 30, 30, Color.Black);
            Raylib.DrawRectangleRec(buttonRect, Color.Black);

            if (ShapeTool.drawFilled)
                Raylib.DrawRectangle((int)buttonRect.X + 5, (int)buttonRect.Y + 5, buttonSize - 10, buttonSize - 10, Color.Green);
            else
                Raylib.DrawRectangle((int)buttonRect.X + 5, (int)buttonRect.Y + 5, buttonSize - 10, buttonSize - 10, Color.Red);
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

public class LoadButton : Button, IDrawable, IHoverable
{
    private DropFileWindow dropFileWindow = new(1820, 880, ["DROP FILE", "HERE"]);
    public override void OnClick()
    {
        ProgramManager.popupWindow = dropFileWindow;
        if (Raylib.IsWindowFullscreen())
            Raylib.ToggleFullscreen();
        Process.Start("explorer.exe");
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
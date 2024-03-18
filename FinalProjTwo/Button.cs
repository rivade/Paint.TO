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
    Canvas canv;

    public LoadButton(Canvas canvasInstance)
    {
        canv = canvasInstance;
    }
    public override void OnClick()
    {
        if (Raylib.IsWindowFullscreen())
            Raylib.ToggleFullscreen();
        
        string fileDirectory = OpenDialog.GetFileDirectory();

        if (!string.IsNullOrEmpty(fileDirectory))
        {
            Image loadedImage = Raylib.LoadImage(fileDirectory);
            canv.LoadProject(loadedImage);
        }
        
        Raylib.ToggleFullscreen();
    }

    public void Draw()
    {
        GetButtonColor(Color.Orange, Color.Yellow, Color.White, false);
        Raylib.DrawRectangleRec(buttonRect, buttonColor);
    }
}

public class OpenLayersButton : Button, IDrawable, IHoverable
{
    private LayerWindow layerWindow = new(1300, 500, ["Layers:", "Press ESC/Enter to close"]);
    public override void OnClick()
    {
        ProgramManager.popupWindow = layerWindow;
    }

    public void Draw()
    {
        GetButtonColor(Color.White, Color.LightGray, Color.White, false);
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







public class LayerButton : Button, IHoverable, IDrawable
{
    public int ThisLayerNumber {get; set;}

    public void Draw()
    {
        GetButtonColor(Color.Lime, Color.Green, Color.White, false);

        if (Canvas.currentLayer == ThisLayerNumber - 1)
            Raylib.DrawRectangle((int)buttonRect.X - 5, (int)buttonRect.Y - 5, (int)buttonRect.Width + 10, (int)buttonRect.Height + 10, Color.Red);

        Raylib.DrawRectangleRec(buttonRect, buttonColor);

        TextHandling.DrawCenteredTextPro([$"Layer {ThisLayerNumber}"],
        (int)buttonRect.X, (int)buttonRect.X + (int)buttonRect.Width,
        (int)buttonRect.Y + 20, 30, 0, Color.Black);
    }


    public override void OnClick()
    {
        Canvas.currentLayer = ThisLayerNumber - 1;
    }
}

public class AddLayerButton : Button, IDrawable
{
    private Texture2D icon;

    public AddLayerButton()
    {
        icon = Raylib.LoadTexture("Icons/plus.png");
    }

    public void Draw()
    {
        GetButtonColor(Color.Lime, Color.Green, Color.White, false);
        Raylib.DrawRectangleRec(buttonRect, buttonColor);
        Raylib.DrawTexture(icon, (int)buttonRect.X, (int)buttonRect.Y, Color.White);
    }

    public void Hover(Vector2 mousePos, Canvas canvas)
    {
        isHoveredOn = false;
        if (Raylib.CheckCollisionPointRec(mousePos, buttonRect))
        {
            isHoveredOn = true;

            if (Raylib.IsMouseButtonPressed(MouseButton.Left))
                Click(canvas);
        }
    }
    public void Click(Canvas canvas)
    {
        if (canvas.layers.Count < 5)
            canvas.layers.Add( new() );
            
        Canvas.currentLayer = canvas.layers.Count - 1;
    }
}

public class RemoveLayerButton : Button, IDrawable
{
    private Texture2D icon;

    public RemoveLayerButton()
    {
        icon = Raylib.LoadTexture("Icons/x.png");
    }

    public void Draw()
    {
        GetButtonColor(Color.Red, Color.Pink, Color.White, false);
        Raylib.DrawRectangleRec(buttonRect, buttonColor);
        Raylib.DrawTexture(icon, (int)buttonRect.X, (int)buttonRect.Y, Color.White);
    }

    public void Hover(Vector2 mousePos, Canvas canvas)
    {
        isHoveredOn = false;
        if (Raylib.CheckCollisionPointRec(mousePos, buttonRect))
        {
            isHoveredOn = true;

            if (Raylib.IsMouseButtonPressed(MouseButton.Left))
                Click(canvas);
        }
    }
    public void Click(Canvas canvas)
    {
        if (canvas.layers.Count != 1)
        {
            canvas.layers.Remove(canvas.layers[Canvas.currentLayer]);

            if (Canvas.currentLayer != 0) Canvas.currentLayer--;
            else Canvas.currentLayer = 0;
        }
    }
}
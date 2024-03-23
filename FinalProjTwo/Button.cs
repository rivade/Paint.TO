using System.Collections.ObjectModel;
using System.IO.Compression;
using System.Security.Principal;
using System.Text.Json.Serialization.Metadata;

namespace DrawingProgram;

public abstract class Button : IHoverable, IDrawable
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

    public virtual void Draw()
    {

    }

    protected void GetButtonColor(Color defaultColor, Color hoverColor, Color activeColor, bool isActive) //Condition lämnas false om knappen ej kan vara aktiv
    {
        buttonColor = defaultColor;

        if (isHoveredOn && !isActive)
            buttonColor = hoverColor;

        else if (isActive)
            buttonColor = activeColor;
    }

    public static bool PaintBrushTypeConditions()
    {
        return ProgramManager.currentTool.GetType().Name != "Pencil" &&
        ProgramManager.currentTool.GetType().Name != "Bucket" &&
        ProgramManager.currentTool.GetType().Name != "EyeDropper" &&
        ProgramManager.currentTool is not ShapeTool ||
        ProgramManager.currentTool is LineTool;
    }
}

public class ToolButton : Button, IHoverable, IDrawable
{
    public DrawTool DrawTool { get; set; }

    private static List<Color>[] colorSets =
    [
        new List<Color> {Color.Lime, Color.Green, Color.DarkGreen},
        new List<Color> {Color.Purple, Color.Pink, Color.DarkPurple},
        new List<Color> {Color.Blue, Color.SkyBlue, Color.DarkBlue}
    ];

    private static List<Color> activeColorSet
    {
        get
        {
            if (colorSetInt >= 3)
                colorSetInt = 0;
            return colorSets[colorSetInt];
        }
    }
    public static int colorSetInt = 0;

    private bool IsActiveTool() => ProgramManager.currentTool == DrawTool;

    public override void OnClick()
    {
        if (!IsActiveTool())
            ProgramManager.currentTool = DrawTool;
    }

    public override void Draw()
    {
        GetButtonColor(activeColorSet[0], activeColorSet[1], activeColorSet[2], IsActiveTool());
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

    public override void Draw()
    {
        Color colorPreview = DrawTool.drawingColor;
        colorPreview.A = 255;
        Raylib.DrawText("Color", Canvas.CanvasWidth + 62, (int)buttonRect.Y - 35, 30, Color.Black);
        Raylib.DrawRectangleRec(buttonRect, Color.Black);
        Raylib.DrawRectangle((int)buttonRect.X + 5, (int)buttonRect.Y + 5, buttonSize - 10, buttonSize - 10, colorPreview);
    }
}


public class BrushRadiusButton : Button, IHoverable, IDrawable
{
    private ValueSetterWindow valueSetterWindow =
    new(800, 600, ["Set brush radius", "Press ESC/Enter to close"]) { minValue = 1, maxValue = 100, thisChanges = ValueSetterWindow.Changes.BrushRadius };

    public override void OnClick()
    {
        if (PaintBrushTypeConditions())
            ProgramManager.popupWindow = valueSetterWindow;
    }

    public override void Draw()
    {
        if (PaintBrushTypeConditions())
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
    private ValueSetterWindow valueSetterWindow =
    new(800, 600, ["Set checker size", "Press ESC/Enter to close"]) { minValue = 5, maxValue = 20, thisChanges = ValueSetterWindow.Changes.CheckerSize };

    public override void OnClick()
    {
        if (ProgramManager.currentTool.GetType().Name == "Checker")
            ProgramManager.popupWindow = valueSetterWindow;
    }

    public override void Draw()
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

public class OpacityButton : Button, IDrawable, IHoverable
{
    private ValueSetterWindow valueSetterWindow =
    new(800, 600, ["Set opacity", "Press ESC/Enter to close"]) { minValue = 0, maxValue = 255, thisChanges = ValueSetterWindow.Changes.Opacity };

    public override void OnHover(Vector2 mousePos)
    {
        base.OnHover(mousePos);
        buttonRect.Y = IsBottomButton() ? Canvas.CanvasHeight - 150 : Canvas.CanvasHeight - 320;

    }
    public override void OnClick()
    {
        if (Conditions())
            valueSetterWindow.SetSlider(DrawTool.drawingColor.A);
        ProgramManager.popupWindow = valueSetterWindow;
    }

    public override void Draw()
    {
        if (Conditions())
        {
            TextHandling.DrawCenteredTextPro(["Opacity"], Canvas.CanvasWidth, ProgramManager.ScreenWidth, (int)buttonRect.Y - 35, 30, 30, Color.Black);
            Raylib.DrawRectangleRec(buttonRect, Color.Black);
            Raylib.DrawRectangle((int)buttonRect.X + 5, (int)buttonRect.Y + 5, buttonSize - 10, buttonSize - 10, Color.White);
            TextHandling.DrawCenteredTextPro([$"{DrawTool.drawingColor.A}"], (int)buttonRect.X, (int)buttonRect.X + (int)buttonRect.Width, (int)buttonRect.Y + 25, 40, 0, Color.Black);
        }
    }

    private bool Conditions()
    {
        return (PaintBrushTypeConditions() ||
                ProgramManager.currentTool.GetType().Name == "Bucket" ||
                ProgramManager.currentTool.GetType().Name == "Pencil"
                || ProgramManager.currentTool is ShapeTool) && ProgramManager.currentTool.GetType().Name != "Eraser";
    }

    private bool IsBottomButton()
    {
        return ProgramManager.currentTool.GetType().Name == "Bucket" || ProgramManager.currentTool.GetType().Name == "Pencil";
    }
}

public class FilledShapeButton : Button, IDrawable, IHoverable
{
    public override void OnClick()
    {
        if (ProgramManager.currentTool is ShapeTool && ProgramManager.currentTool is not LineTool)
            ShapeTool.drawFilled = !ShapeTool.drawFilled;
    }

    public override void Draw()
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

    public override void Draw()
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

        string fileDirectory = OpenDialog.GetFile();

        if (!string.IsNullOrEmpty(fileDirectory))
        {
            Image loadedImage = Raylib.LoadImage(fileDirectory);
            canv.LoadProject(loadedImage);
        }

        Raylib.ToggleFullscreen();
    }

    public override void Draw()
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

    public override void Draw()
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

    public override void Draw()
    {
        Raylib.DrawRectangleRec(buttonRect, Color.Red);
    }
}







public class LayerButton : Button, IHoverable, IDrawable
{
    public int ThisLayerNumber { get; set; }
    public bool isVisible { get; set; }

    public override void Draw()
    {
        if (isVisible)
            GetButtonColor(Color.Lime, Color.Green, Color.White, false);

        else
            GetButtonColor(Color.Gray, Color.LightGray, Color.White, false);

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

    public override void Draw()
    {
        GetButtonColor(Color.Lime, Color.Green, Color.White, false);
        Raylib.DrawRectangleRec(buttonRect, buttonColor);
        Raylib.DrawTexture(icon, (int)buttonRect.X, (int)buttonRect.Y, Color.White);
    }

    public void Update(Vector2 mousePos, Canvas canvas)
    {
        isHoveredOn = false;
        if (Raylib.CheckCollisionPointRec(mousePos, buttonRect))
        {
            isHoveredOn = true;

            if (Raylib.IsMouseButtonPressed(MouseButton.Left))
                Click(canvas);
        }
    }
    private void Click(Canvas canvas)
    {
        if (canvas.layers.Count < 5)
            canvas.layers.Add(new());

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

    public override void Draw()
    {
        GetButtonColor(Color.Red, Color.Pink, Color.White, false);
        Raylib.DrawRectangleRec(buttonRect, buttonColor);
        Raylib.DrawTexture(icon, (int)buttonRect.X, (int)buttonRect.Y, Color.White);
    }

    public void Update(Vector2 mousePos, Canvas canvas)
    {
        isHoveredOn = false;
        if (Raylib.CheckCollisionPointRec(mousePos, buttonRect))
        {
            isHoveredOn = true;

            if (Raylib.IsMouseButtonPressed(MouseButton.Left))
                Click(canvas);
        }
    }
    private void Click(Canvas canvas)
    {
        if (canvas.layers.Count != 1)
        {
            canvas.layers.Remove(canvas.layers[Canvas.currentLayer]);

            if (Canvas.currentLayer != 0) Canvas.currentLayer--;
            else Canvas.currentLayer = 0;
        }
    }
}

public class LayerVisibilityButton : Button, IDrawable
{
    private List<Texture2D> icons = new();
    public int currentIcon = 0;

    public LayerVisibilityButton()
    {
        icons.Add(Raylib.LoadTexture("Icons/visible.png"));
        icons.Add(Raylib.LoadTexture("Icons/invisible.png"));
    }

    public override void Draw()
    {
        GetButtonColor(Color.LightGray, Color.White, Color.White, false);
        Raylib.DrawRectangleRec(buttonRect, buttonColor);
        Raylib.DrawTexture(icons[currentIcon], (int)buttonRect.X, (int)buttonRect.Y, Color.White);
    }

    public void Update(Vector2 mousePos, Canvas canvas)
    {
        currentIcon = canvas.layers[Canvas.currentLayer].isVisible ? 0 : 1;

        isHoveredOn = false;
        if (Raylib.CheckCollisionPointRec(mousePos, buttonRect))
        {
            isHoveredOn = true;

            if (Raylib.IsMouseButtonPressed(MouseButton.Left))
                Click(canvas);
        }
    }
    private void Click(Canvas canvas)
    {
        canvas.layers[Canvas.currentLayer].isVisible = !canvas.layers[Canvas.currentLayer].isVisible;
    }
}

public class SettingsButton : Button, IHoverable, IDrawable
{
    private SettingsWindow settingsWindow = new(1300, 500, ["Settings", "Press ESC/Enter to close"]);
    public override void OnClick()
    {
        ProgramManager.popupWindow = settingsWindow;
    }

    public override void Draw()
    {
        GetButtonColor(Color.Gray, Color.LightGray, Color.White, false);
        Raylib.DrawRectangleRec(buttonRect, buttonColor);
    }
}

public class GUIColorButton : Button, IHoverable, IDrawable
{
    private Action ChangeColor;
    private string text;

    public GUIColorButton(Action ColorToChange, string buttonText)
    {
        ChangeColor = ColorToChange;
        text = buttonText;
    }

    public override void OnClick()
    {
        ChangeColor.Invoke();
    }

    public override void Draw()
    {
        GetButtonColor(Color.Lime, Color.Green, Color.White, false);
        Raylib.DrawRectangleRec(buttonRect, buttonColor);
        TextHandling.DrawCenteredTextPro([text], (int)buttonRect.X, (int)buttonRect.X + (int)buttonRect.Width, (int)buttonRect.Y + 25, 40, 0, Color.Black);
    }
}
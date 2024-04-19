using System.Collections.ObjectModel;
using System.IO.Compression;
using System.Security.Principal;
using System.Text.Json.Serialization.Metadata;

namespace DrawingProgram;

public abstract class Button : IHoverable, IDrawable
{
    protected ProgramManager program;
    public Button(ProgramManager programInstance)
    {
        program = programInstance;
    }

    public const int buttonSize = 80;
    public Rectangle buttonRect;
    protected Color buttonColor;
    protected bool isHoveredOn;
    protected InfoWindow infoWindow;

    public virtual void OnHover(Vector2 mousePos)
    {
        isHoveredOn = false;
        infoWindow = null;
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
        infoWindow?.Draw();
    }

    protected void GetButtonColor(Color defaultColor, Color hoverColor, Color activeColor, bool isActive)
    {
        buttonColor = defaultColor;

        if (isHoveredOn && !isActive)
            buttonColor = hoverColor;

        else if (isActive)
            buttonColor = activeColor;
    }

    public bool PaintBrushTypeConditions()
    {
        return program.currentTool.GetType().Name != "Pencil" &&
        program.currentTool.GetType().Name != "Bucket" &&
        program.currentTool.GetType().Name != "EyeDropper" &&
        program.currentTool is not ShapeTool ||
        program.currentTool is LineTool;
    }
}

public sealed class ToolButton : Button, IHoverable, IDrawable
{
    public DrawTool DrawTool { get; set; }

    private static List<Color>[] colorSets =
    [
        new List<Color> {Color.Blue, Color.SkyBlue, Color.DarkBlue},
        new List<Color> {Color.Lime, Color.Green, Color.DarkGreen},
        new List<Color> {Color.Purple, Color.Pink, Color.DarkPurple}
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

    private string hoverText;

    public ToolButton(ProgramManager programInstance, string hovText) : base(programInstance)
    {
        hoverText = hovText;
    }

    private bool IsActiveTool() => program.currentTool == DrawTool;

    public override void OnHover(Vector2 mousePos)
    {
        base.OnHover(mousePos);

        if (isHoveredOn)
            infoWindow = new(hoverText, (int)buttonRect.X, (int)buttonRect.Y - 40);
    }

    public override void OnClick()
    {
        if (!IsActiveTool())
            program.currentTool = DrawTool;
    }

    public override void Draw()
    {
        GetButtonColor(activeColorSet[0], activeColorSet[1], activeColorSet[2], IsActiveTool());
        Raylib.DrawRectangleRec(buttonRect, buttonColor);
        base.Draw();
    }
}

public sealed class ColorSelectorButton : Button, IHoverable, IDrawable
{
    private ColorSelector colorSelectorWindow;

    public ColorSelectorButton(ProgramManager programInstance) : base(programInstance)
    {
        colorSelectorWindow = new(programInstance, 660, 750, ["Select a color"]);
    }

    public override void OnClick()
    {
        program.popupWindow = colorSelectorWindow;
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


public sealed class BrushRadiusButton : Button, IHoverable, IDrawable
{
    private ValueSetterWindow valueSetterWindow;

    public BrushRadiusButton(ProgramManager programInstance) : base(programInstance)
    {
        valueSetterWindow =
        new(programInstance, 800, 500, ["Set brush radius"]) { minValue = 1, maxValue = 100, thisChanges = ValueSetterWindow.Changes.BrushRadius };
    }

    public override void OnClick()
    {
        if (PaintBrushTypeConditions())
            program.popupWindow = valueSetterWindow;
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

public sealed class CheckerSizeButton : Button, IDrawable, IHoverable
{
    private ValueSetterWindow valueSetterWindow;

    public CheckerSizeButton(ProgramManager programInstance) : base(programInstance)
    {
        valueSetterWindow =
        new(programInstance, 800, 500, ["Set checker size"]) { minValue = 5, maxValue = 20, thisChanges = ValueSetterWindow.Changes.CheckerSize };
    }

    public override void OnClick()
    {
        if (program.currentTool.GetType().Name == "Checker")
            program.popupWindow = valueSetterWindow;
    }

    public override void Draw()
    {
        if (program.currentTool.GetType().Name == "Checker")
        {
            TextHandling.DrawCenteredTextPro(["Checker", "size"], Canvas.CanvasWidth, ProgramManager.ScreenWidth, (int)buttonRect.Y - 65, 30, 30, Color.Black);
            Raylib.DrawRectangleRec(buttonRect, Color.Black);
            Raylib.DrawRectangle((int)buttonRect.X + 5, (int)buttonRect.Y + 5, buttonSize - 10, buttonSize - 10, Color.White);
            TextHandling.DrawCenteredTextPro([$"{Checker.checkerSize}"], (int)buttonRect.X, (int)buttonRect.X + (int)buttonRect.Width, (int)buttonRect.Y + 20, 50, 0, Color.Black);
        }
    }
}

public sealed class OpacityButton : Button, IDrawable, IHoverable
{
    private ValueSetterWindow valueSetterWindow;

    public OpacityButton(ProgramManager programInstance) : base(programInstance)
    {
        valueSetterWindow =
        new(programInstance, 800, 500, ["Set opacity"]) { minValue = 0, maxValue = 255, thisChanges = ValueSetterWindow.Changes.Opacity };
    }

    public override void OnHover(Vector2 mousePos)
    {
        base.OnHover(mousePos);
        buttonRect.Y = IsBottomButton() ? Canvas.CanvasHeight - 150 : Canvas.CanvasHeight - 320;

    }
    public override void OnClick()
    {
        if (Conditions())
            valueSetterWindow.SetSlider(DrawTool.drawingColor.A);
        program.popupWindow = valueSetterWindow;
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
                program.currentTool.GetType().Name == "Bucket" ||
                program.currentTool.GetType().Name == "Pencil"
                || program.currentTool is ShapeTool) && program.currentTool.GetType().Name != "Eraser";
    }

    private bool IsBottomButton()
    {
        return program.currentTool.GetType().Name == "Bucket" || program.currentTool.GetType().Name == "Pencil";
    }
}

public sealed class FilledShapeButton : Button, IDrawable, IHoverable
{
    public FilledShapeButton(ProgramManager programInstance) : base(programInstance) {}

    public override void OnClick()
    {
        if (program.currentTool is ShapeTool && program.currentTool is not LineTool)
            ShapeTool.drawFilled = !ShapeTool.drawFilled;
    }

    public override void Draw()
    {
        if (program.currentTool is ShapeTool && program.currentTool is not LineTool)
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

public sealed class SaveCanvasButton : Button, IDrawable, IHoverable
{
    public SaveCanvasButton(ProgramManager programInstance) : base(programInstance) {}

    public override void OnHover(Vector2 mousePos)
    {
        base.OnHover(mousePos);

        if (isHoveredOn)
            infoWindow = new("Save image", (int)buttonRect.X - Raylib.MeasureText("Save image", InfoWindow.FontSize) - 20, (int)buttonRect.Y);
    }

    public override void OnClick()
    {
        program.popupWindow = new SavePopup(program, 800, 300, ["Select file name", "Press enter to save"]);
    }

    public override void Draw()
    {
        GetButtonColor(Color.Orange, Color.Yellow, Color.White, false);
        Raylib.DrawRectangleRec(buttonRect, buttonColor);
        base.Draw();
    }
}

public sealed class LoadButton : Button, IDrawable, IHoverable
{
    Canvas canvas;

    public LoadButton(ProgramManager programInstance, Canvas canvasInstance) : base(programInstance)
    {
        canvas = canvasInstance;
    }
    public override void OnHover(Vector2 mousePos)
    {
        base.OnHover(mousePos);

        if (isHoveredOn)
            infoWindow = new("Open image", (int)buttonRect.X - Raylib.MeasureText("Open image", InfoWindow.FontSize) - 20, (int)buttonRect.Y);
    }

    public override void OnClick()
    {
        if (Raylib.IsWindowFullscreen())
            Raylib.ToggleFullscreen();

        string fileDirectory = OpenDialog.GetFile();

        if (!string.IsNullOrEmpty(fileDirectory))
        {
            Image loadedImage = Raylib.LoadImage(fileDirectory);
            canvas.LoadProject(loadedImage);
        }

        Raylib.ToggleFullscreen();
    }

    public override void Draw()
    {
        GetButtonColor(Color.Orange, Color.Yellow, Color.White, false);
        Raylib.DrawRectangleRec(buttonRect, buttonColor);
        base.Draw();
    }
}

public sealed class OpenLayersButton : Button, IDrawable, IHoverable
{
    private LayerWindow layerWindow;

    public OpenLayersButton(ProgramManager programInstance) : base(programInstance)
    {
        layerWindow = new(programInstance, 1300, 500, ["Layers:"]);
    }

    public override void OnClick()
    {
        program.popupWindow = layerWindow;
    }

    public override void OnHover(Vector2 mousePos)
    {
        base.OnHover(mousePos);

        if (isHoveredOn)
            infoWindow = new("Layers", (int)buttonRect.X, (int)buttonRect.Y - 40);
    }

    public override void Draw()
    {
        GetButtonColor(Color.White, Color.LightGray, Color.White, false);
        Raylib.DrawRectangleRec(buttonRect, buttonColor);
        base.Draw();
    }
}

public sealed class CloseButton : Button, IDrawable, IHoverable
{
    public CloseButton(ProgramManager programInstance) : base(programInstance) {}

    public override void OnHover(Vector2 mousePos)
    {
        base.OnHover(mousePos);

        if (isHoveredOn)
            infoWindow = new("Close program", (int)buttonRect.X - Raylib.MeasureText("Close program", InfoWindow.FontSize) - 20, (int)buttonRect.Y);
    }

    public override void OnClick()
    {
        Environment.Exit(0);
    }

    public override void Draw()
    {
        GetButtonColor(Color.Red, Color.Pink, Color.White, false);
        Raylib.DrawRectangleRec(buttonRect, buttonColor);
        base.Draw();
    }
}

public sealed class SettingsButton : Button, IHoverable, IDrawable
{
    private SettingsWindow settingsWindow;

    public SettingsButton(ProgramManager programInstance) : base(programInstance)
    {
        settingsWindow = new(programInstance, 1300, 400, ["Settings"]);
    }

    public override void OnHover(Vector2 mousePos)
    {
        base.OnHover(mousePos);

        if (isHoveredOn)
            infoWindow = new("Settings", (int)buttonRect.X - Raylib.MeasureText("Settings", InfoWindow.FontSize) - 20, (int)buttonRect.Y);
    }
    public override void OnClick()
    {
        program.popupWindow = settingsWindow;
    }

    public override void Draw()
    {
        GetButtonColor(Color.Gray, Color.LightGray, Color.White, false);
        Raylib.DrawRectangleRec(buttonRect, buttonColor);
        base.Draw();
    }
}

public sealed class GUIColorButton : Button, IHoverable, IDrawable
{
    private Action ChangeColor;
    private string text;

    public GUIColorButton(ProgramManager programInstance, Action ColorToChange, string buttonText) : base(programInstance)
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

public sealed class ClosePopupButton : Button, IHoverable, IDrawable
{
    private const int buttonWidth = 50;
    private const int buttonHeight = 30;
    private Texture2D icon = Raylib.LoadTexture("Textures/Icons/x2.png");

    public ClosePopupButton(ProgramManager programInstance, Rectangle popupRect) : base(programInstance)
    {
        buttonRect = new((popupRect.X + popupRect.Width) - buttonWidth, popupRect.Y, buttonWidth, buttonHeight);
    }

    public override void OnHover(Vector2 mousePos)
    {
        base.OnHover(mousePos);

        if (isHoveredOn)
            infoWindow = new("Close window", (int)buttonRect.X - Raylib.MeasureText("Close window", InfoWindow.FontSize) - 20, (int)buttonRect.Y);
    }

    public override void OnClick()
    {
        program.isMouseInputEnabled = false;
        program.popupWindow = null;
    }

    public override void Draw()
    {
        GetButtonColor(Color.Red, Color.Pink, Color.White, false);
        Raylib.DrawRectangleRec(buttonRect, buttonColor);
        Raylib.DrawTexture(icon, (int)buttonRect.X, (int)buttonRect.Y, Color.White);
        base.Draw();
    }
}
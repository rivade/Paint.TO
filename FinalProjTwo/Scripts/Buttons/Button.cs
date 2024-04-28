using System.Collections.ObjectModel;
using System.IO.Compression;
using System.Security.Principal;
using System.Text.Json.Serialization.Metadata;

namespace DrawingProgram;

public abstract class Button : IHoverable, IDrawable
{
    protected ProgramManager program;
    public Rectangle buttonRect;

    public Button(ProgramManager programInstance, Rectangle button)
    {
        program = programInstance;
        buttonRect = button;
    }

    public const int ButtonSize = 80;
    protected bool isHoveredOn;
    protected InfoWindow infoWindow;

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

    public virtual void OnClick() { }

    public virtual void Draw()
    {
        if (isHoveredOn)
            infoWindow.Draw();
    }

    protected Color GetButtonColor(Color defaultColor, Color hoverColor, Color activeColor, bool isActive)
    {
        Color color = defaultColor;

        if (isHoveredOn && !isActive)
            color = hoverColor;

        else if (isActive)
            color = activeColor;

        return color;
    }
}

public sealed class ToolButton : Button
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

    public ToolButton(ProgramManager programInstance, Rectangle buttonRect, string hovText) : base(programInstance, buttonRect)
    {
        infoWindow = new(hovText, (int)buttonRect.X, (int)buttonRect.Y - 40);
    }

    private bool IsActiveTool() => program.currentTool == DrawTool;

    public override void OnClick()
    {
        if (!IsActiveTool())
            program.currentTool = DrawTool;
    }

    public override void Draw()
    {
        Raylib.DrawRectangleRec(buttonRect, GetButtonColor(activeColorSet[0], activeColorSet[1], activeColorSet[2], IsActiveTool()));
        base.Draw();
    }
}

public sealed class ColorSelectorButton : Button
{
    private ColorSelector colorSelectorWindow;

    public ColorSelectorButton(ProgramManager programInstance, Rectangle buttonRect) : base(programInstance, buttonRect)
    {
        colorSelectorWindow = new(programInstance, 1150, 750, ["Select a color"]);
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
        Raylib.DrawRectangle((int)buttonRect.X + 5, (int)buttonRect.Y + 5, ButtonSize - 10, ButtonSize - 10, colorPreview);
    }
}


public sealed class BrushRadiusButton : Button
{
    private ValueSetterWindow valueSetterWindow;

    public BrushRadiusButton(ProgramManager programInstance, Rectangle buttonRect) : base(programInstance, buttonRect)
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
            Raylib.DrawRectangle((int)buttonRect.X + 5, (int)buttonRect.Y + 5, ButtonSize - 10, ButtonSize - 10, Color.White);
            TextHandling.DrawCenteredTextPro([$"{DrawTool.brushRadius}"], (int)buttonRect.X, (int)buttonRect.X + (int)buttonRect.Width, (int)buttonRect.Y + 20, 50, 0, Color.Black);
        }
    }

    private bool PaintBrushTypeConditions()
    {
        return program.currentTool.GetType().Name != "Pencil" &&
        program.currentTool.GetType().Name != "Bucket" &&
        program.currentTool.GetType().Name != "EyeDropper" &&
        program.currentTool is not ShapeTool ||
        program.currentTool is LineTool;
    }
}

public sealed class CheckerSizeButton : Button
{
    private ValueSetterWindow valueSetterWindow;

    public CheckerSizeButton(ProgramManager programInstance, Rectangle buttonRect) : base(programInstance, buttonRect)
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
            Raylib.DrawRectangle((int)buttonRect.X + 5, (int)buttonRect.Y + 5, ButtonSize - 10, ButtonSize - 10, Color.White);
            TextHandling.DrawCenteredTextPro([$"{Checker.checkerSize}"], (int)buttonRect.X, (int)buttonRect.X + (int)buttonRect.Width, (int)buttonRect.Y + 20, 50, 0, Color.Black);
        }
    }
}

public sealed class FilledShapeButton : Button
{
    public FilledShapeButton(ProgramManager programInstance, Rectangle buttonRect) : base(programInstance, buttonRect) { }

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
                Raylib.DrawRectangle((int)buttonRect.X + 5, (int)buttonRect.Y + 5, ButtonSize - 10, ButtonSize - 10, Color.Green);
            else
                Raylib.DrawRectangle((int)buttonRect.X + 5, (int)buttonRect.Y + 5, ButtonSize - 10, ButtonSize - 10, Color.Red);
        }
    }
}

public sealed class SaveCanvasButton : Button
{
    public SaveCanvasButton(ProgramManager programInstance, Rectangle buttonRect) : base(programInstance, buttonRect)
    {
        infoWindow = new("Save image", (int)buttonRect.X - Raylib.MeasureText("Save image", InfoWindow.FontSize) - 20, (int)buttonRect.Y);
    }

    public override void OnClick()
    {
        program.popupWindow = new SavePopup(program, 800, 300, ["Select file name", "Press enter to save"]);
    }

    public override void Draw()
    {
        Raylib.DrawRectangleRec(buttonRect, GetButtonColor(Color.Orange, Color.Yellow, Color.White, false));
        base.Draw();
    }
}

public sealed class LoadButton : Button
{
    private Canvas canvas;

    public LoadButton(ProgramManager programInstance, Rectangle buttonRect, Canvas canvasInstance) : base(programInstance, buttonRect)
    {
        canvas = canvasInstance;
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
        Raylib.DrawRectangleRec(buttonRect, GetButtonColor(Color.Orange, Color.Yellow, Color.White, false));
        base.Draw();
    }
}

public sealed class OpenLayersButton : Button
{
    private LayerWindow layerWindow;

    public OpenLayersButton(ProgramManager programInstance, Rectangle buttonRect, Canvas canvasInstance) : base(programInstance, buttonRect)
    {
        layerWindow = new(programInstance, canvasInstance, 1300, 500, ["Layers:"]);
        infoWindow = new("Layers", (int)buttonRect.X, (int)buttonRect.Y - 40);
    }

    public override void OnClick()
    {
        program.popupWindow = layerWindow;
    }

    public override void Draw()
    {
        Raylib.DrawRectangleRec(buttonRect, GetButtonColor(Color.White, Color.LightGray, Color.White, false));
        base.Draw();
    }
}

public sealed class CloseButton : Button
{
    public CloseButton(ProgramManager programInstance, Rectangle buttonRect) : base(programInstance, buttonRect)
    {
        infoWindow = new("Close program", (int)buttonRect.X - Raylib.MeasureText("Close program", InfoWindow.FontSize) - 20, (int)buttonRect.Y);
    }

    public override void OnClick()
    {
        Environment.Exit(0);
    }

    public override void Draw()
    {
        Raylib.DrawRectangleRec(buttonRect, GetButtonColor(Color.Red, Color.Pink, Color.White, false));
        base.Draw();
    }
}

public sealed class SettingsButton : Button
{
    private SettingsWindow settingsWindow;

    public SettingsButton(ProgramManager programInstance, Rectangle buttonRect) : base(programInstance, buttonRect)
    {
        settingsWindow = new(programInstance, 1300, 400, ["Settings"]);
        infoWindow = new("Settings", (int)buttonRect.X - Raylib.MeasureText("Settings", InfoWindow.FontSize) - 20, (int)buttonRect.Y);
    }

    public override void OnClick()
    {
        program.popupWindow = settingsWindow;
    }

    public override void Draw()
    {
        Raylib.DrawRectangleRec(buttonRect, GetButtonColor(Color.Gray, Color.LightGray, Color.White, false));
        base.Draw();
    }
}

public sealed class GUIColorButton : Button
{
    private Action ChangeColor;
    private string text;

    public GUIColorButton(ProgramManager programInstance, Rectangle buttonRect, Action ColorToChange, string buttonText) : base(programInstance, buttonRect)
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
        Raylib.DrawRectangleRec(buttonRect, GetButtonColor(Color.Lime, Color.Green, Color.White, false));
        TextHandling.DrawCenteredTextPro([text], (int)buttonRect.X, (int)buttonRect.X + (int)buttonRect.Width, (int)buttonRect.Y + 25, 40, 0, Color.Black);
    }
}

public sealed class ClosePopupButton : Button
{
    private const int buttonWidth = 50;
    private const int buttonHeight = 30;
    private Texture2D icon = Raylib.LoadTexture("Textures/Icons/x2.png");

    public ClosePopupButton(ProgramManager programInstance, Rectangle popupRect) : base(programInstance, new())
    {
        buttonRect = new((popupRect.X + popupRect.Width) - buttonWidth, popupRect.Y, buttonWidth, buttonHeight);
        infoWindow = new("Close window", (int)buttonRect.X - Raylib.MeasureText("Close window", InfoWindow.FontSize) - 20, (int)buttonRect.Y);
    }

    public override void OnClick()
    {
        program.isMouseInputEnabled = false;
        program.popupWindow = null;
    }

    public override void Draw()
    {
        Raylib.DrawRectangleRec(buttonRect, GetButtonColor(Color.Red, Color.Pink, Color.White, false));
        Raylib.DrawTexture(icon, (int)buttonRect.X, (int)buttonRect.Y, Color.White);
        base.Draw();
    }
}

public sealed class PaletteButton : Button
{
    public Color paletteColor;
    public ColorSelector window;
    public static Queue<Color> paletteColors = new([Color.Black, Color.Black, Color.Black, Color.Black]);

    public PaletteButton(ProgramManager programInstance, ColorSelector windowInstance, Rectangle button) : base(programInstance, button)
    {
        window = windowInstance;
    }

    public static void LimitQueueSize()
    {
        if (paletteColors.Count > 4) paletteColors.Dequeue();
    }

    public override void OnClick()
    {
        DrawTool.drawingColor = paletteColor;
        window.SetSliders();
    }

    public override void Draw()
    {
        Raylib.DrawRectangle((int)buttonRect.X - 5, (int)buttonRect.Y - 5, (int)buttonRect.Width + 10, (int)buttonRect.Height + 10, Color.White);
        Raylib.DrawRectangleRec(buttonRect, paletteColor);
    }
}
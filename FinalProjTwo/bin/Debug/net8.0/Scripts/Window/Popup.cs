namespace DrawingProgram;

public abstract class PopupWindow : IDrawable
{
    protected Rectangle windowRect;
    public string[] messages;
    protected ClosePopupButton closeButton;
    protected ProgramManager program;

    public virtual void Draw()
    {
        Raylib.DrawRectangleRec(windowRect, GUIarea.guiColor);
        TextHandling.DrawScreenCenteredText(messages, (int)windowRect.Y + 20, 60, 70, Color.Black);
        closeButton.Draw();
    }

    public virtual void Logic(Canvas canvas, Vector2 mousePos)
    {
        closeButton.OnHover(mousePos);
        if (!Raylib.CheckCollisionPointRec(mousePos, windowRect) && Raylib.IsMouseButtonPressed(MouseButton.Left))
            program.popupWindow = null;
    }

    public PopupWindow(ProgramManager programInstance, int width, int height, string[] messagesExtern)
    {
        program = programInstance;
        windowRect = new Rectangle(ProgramManager.ScreenWidth / 2 - width / 2, ProgramManager.ScreenHeight / 2 - height / 2, width, height);
        messages = messagesExtern;
        closeButton = new(program, windowRect);
    }
}

public sealed class StartPopup : PopupWindow
{
    public StartPopup(ProgramManager programInstance, int width, int height, string[] messagesExtern) : base(programInstance, width, height, messagesExtern) { }

    public override void Draw()
    {
        base.Draw();
        TextHandling.DrawScreenCenteredText(["Paint.TO v2.02"], (int)windowRect.Y + 30, 80, 0, Color.Black);
        TextHandling.DrawScreenCenteredText(["Changenotes:", "-Selection in rectangleselect can be resized", "Small tweaks and fixes"],
                                            (int)windowRect.Y + 175, 20, 30, Color.Black);
    }
}

public sealed class SavePopup : PopupWindow
{
    public Dictionary<KeyboardKey, string> alphabet;
    public string fileName = "";

    public SavePopup(ProgramManager programInstance, int width, int height, string[] messagesExtern) : base(programInstance, width, height, messagesExtern)
    {
        alphabet = new();
        for (int i = (int)KeyboardKey.A; i <= (int)KeyboardKey.Z; i++)
        {
            alphabet.Add((KeyboardKey)i, ((char)(i + 32)).ToString());
        }

        for (int i = 48; i <= 57; i++)
        {
            alphabet.Add((KeyboardKey)i, ((char)i).ToString());
        }
    }

    public void SaveCanvas(Canvas canvas)
    {
        if (Raylib.IsKeyPressed(KeyboardKey.Enter) && fileName != "")
        {
            Raylib.ToggleFullscreen();
            string directory = OpenDialog.GetDirectory();
            canvas.SaveProject(fileName + ".png", directory);
            Raylib.ToggleFullscreen();
        }
    }

    public string UpdateFileName()
    {
        KeyboardKey keyPressed = (KeyboardKey)Raylib.GetKeyPressed();
        if (keyPressed != KeyboardKey.Null && alphabet.ContainsKey(keyPressed) && Raylib.MeasureText(fileName, 30) < windowRect.Width - 50)
        {
            if (Raylib.IsKeyDown(KeyboardKey.LeftShift) || Raylib.IsKeyDown(KeyboardKey.RightShift))
                fileName += alphabet[keyPressed].ToUpper();

            else
                fileName += alphabet[keyPressed];
        }

        if (keyPressed == KeyboardKey.Backspace && fileName.Length != 0)
            fileName = fileName[..^1]; //Tar bort sista bokstaven/siffran i stringen

        return fileName;
    }

    public override void Draw()
    {
        base.Draw();
        Raylib.DrawRectangle((int)windowRect.X + 10, (int)(windowRect.Y + windowRect.Height) - 80, (int)windowRect.Width - 20, 50, Color.LightGray);
        Raylib.DrawText(fileName, (int)windowRect.X + 20, (int)(windowRect.Y + windowRect.Height) - 70, 30, Color.Black);
    }

    public override void Logic(Canvas canvas, Vector2 mousePos)
    {
        base.Logic(canvas, mousePos);
        fileName = UpdateFileName();
        SaveCanvas(canvas);
    }
}

public sealed class LayerWindow : PopupWindow
{
    private List<LayerButton> layerButtons = new();
    private List<LayerWindowButton> buttons;

    public LayerWindow(ProgramManager programInstance, Canvas canvasInstance, int width, int height, string[] messagesExtern) : base(programInstance, width, height, messagesExtern)
    {
        buttons =
        [
            new AddLayerButton(programInstance, new(670, 650, Button.ButtonSize, Button.ButtonSize), canvasInstance),
            new MoveLayerButton(programInstance, new(770, 650, Button.ButtonSize, Button.ButtonSize), canvasInstance) { direction = MoveLayerButton.Direction.Down },
            new LayerVisibilityButton(programInstance, new(870, 650, Button.ButtonSize, Button.ButtonSize), canvasInstance),
            new MergeLayersButton(programInstance, new(970, 650, Button.ButtonSize, Button.ButtonSize), canvasInstance),
            new MoveLayerButton(programInstance, new(1070, 650, Button.ButtonSize, Button.ButtonSize), canvasInstance) { direction = MoveLayerButton.Direction.Up },
            new RemoveLayerButton(programInstance, new(1170, 650, Button.ButtonSize, Button.ButtonSize), canvasInstance)
        ];
    }

    public override void Logic(Canvas canvas, Vector2 mousePos)
    {
        base.Logic(canvas, mousePos);
        layerButtons = new();

        for (int i = 0; i < canvas.layers.Count; i++)
        {
            layerButtons.Add(new(program, new(i * 250 + 360, 475, 200, 100), canvas) { ThisLayerNumber = i + 1, IsVisible = canvas.layers[i].isVisible });
        }

        layerButtons.ForEach(l => l.OnHover(mousePos));
        buttons.ForEach(b => b.OnHover(mousePos));

    }

    public override void Draw()
    {
        base.Draw();

        layerButtons.ForEach(l => l.Draw());
        buttons.ForEach(b => b.Draw());
    }
}

public sealed class ValueSetterWindow : PopupWindow
{
    public enum Changes
    {
        BrushRadius,
        CheckerSize
    }

    public Changes thisChanges { get; set; }

    private Slider slider;

    public int minValue { get; set; }
    public int maxValue { get; set; }
    private int value;

    public ValueSetterWindow(ProgramManager programInstance, int width, int height, string[] messagesExtern) : base(programInstance, width, height, messagesExtern)
    {
        int sliderWidth = 500;
        int sliderHeight = 15;

        int sliderX = (int)(windowRect.X + windowRect.Width / 2 - sliderWidth / 2);

        slider = new(20, new(sliderX, 475, sliderWidth, sliderHeight));
    }

    public override void Logic(Canvas canvas, Vector2 mousePos)
    {
        base.Logic(canvas, mousePos);
        value = slider.GetValue(mousePos, minValue, maxValue);

        switch (thisChanges)
        {
            case Changes.BrushRadius:
                DrawTool.brushRadius = value;
                break;
            case Changes.CheckerSize:
                Checker.checkerSize = value;
                break;
        }
    }

    public override void Draw()
    {
        base.Draw();
        slider.Draw();

        switch (thisChanges)
        {
            case Changes.BrushRadius:
                Color colorPreview = DrawTool.drawingColor;
                colorPreview.A = 255;
                Raylib.DrawCircle(ProgramManager.ScreenWidth / 2, 650, value, colorPreview);
                break;
            case Changes.CheckerSize:
                CheckerPreview.DrawCheckerPreview();
                break;
        }
    }

    public void SetSlider(int value)
    {
        float relativePosition = (value - minValue) / (float)(maxValue - minValue);
        float sliderX = slider.SliderBar.X + relativePosition * slider.SliderBar.Width;

        slider.SliderCircle.Middle = new Vector2(sliderX, slider.SliderCircle.Middle.Y);
    }
}

public sealed class SettingsWindow : PopupWindow
{
    private List<Button> buttons;

    public SettingsWindow(ProgramManager programInstance, int width, int height, string[] messagesExtern) : base(programInstance, width, height, messagesExtern)
    {
        buttons =
        [
            new GUIColorButton(programInstance, new(340, 475, 1230, 100), () => GUIarea.colorInt++, "Change GUI color"),
            new GUIColorButton(programInstance, new(340, 600, 1230, 100), () => ToolButton.colorSetInt++, "Change toolbutton color")
        ];
    }

    public override void Logic(Canvas c, Vector2 mousePos)
    {
        base.Logic(c, mousePos);
        buttons.ForEach(b => b.OnHover(mousePos));
    }

    public override void Draw()
    {
        base.Draw();
        buttons.ForEach(b => b.Draw());
    }
}

public static class CheckerPreview
{
    public static void DrawCheckerPreview()
    {
        int centerX = ProgramManager.ScreenWidth / 2;
        int centerY = 650;

        int rows = (int)Math.Ceiling(200d / Checker.checkerSize);
        int cols = (int)Math.Ceiling(200d / Checker.checkerSize);

        Color colorPreview = DrawTool.drawingColor;
        colorPreview.A = 255;

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                int xPos = col * Checker.checkerSize;
                int yPos = row * Checker.checkerSize;

                int xOffset = (200 - (cols * Checker.checkerSize)) / 2;
                int yOffset = (200 - (rows * Checker.checkerSize)) / 2;

                xPos += xOffset + centerX - 100;
                yPos += yOffset + centerY - 100;

                if ((row + col) % 2 == 0)
                    Raylib.DrawRectangle(xPos, yPos, Checker.checkerSize, Checker.checkerSize, DrawTool.drawingColor);
            }
        }
    }
}

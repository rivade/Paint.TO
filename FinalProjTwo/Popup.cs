using Microsoft.VisualBasic.FileIO;

namespace DrawingProgram;

public abstract class PopupWindow : IDrawable
{
    protected Rectangle windowRect;
    public string[] messages;
    protected ClosePopupButton closeButton;

    public virtual void Draw()
    {
        Raylib.DrawRectangleRec(windowRect, GUIarea.guiColor);
        TextHandling.DrawScreenCenteredText(messages, (int)windowRect.Y + 20, 30, 60, Color.Black);
        closeButton.Draw();
    }

    public virtual void Logic(Canvas canvas, Vector2 mousePos)
    {
        closeButton.OnHover(mousePos);
        if (!Raylib.CheckCollisionPointRec(mousePos, windowRect) && Raylib.IsMouseButtonPressed(MouseButton.Left))
            ProgramManager.popupWindow = null;
    }

    public PopupWindow(int width, int height, string[] messagesExtern)
    {
        windowRect = new Rectangle(ProgramManager.ScreenWidth / 2 - width / 2, ProgramManager.ScreenHeight / 2 - height / 2, width, height);
        messages = messagesExtern;
        closeButton = new(windowRect);
    }
}

public class StartPopup : PopupWindow
{
    public StartPopup(int width, int height, string[] messagesExtern) : base(width, height, messagesExtern) {}

    public override void Draw()
    {
        base.Draw();
        TextHandling.DrawScreenCenteredText(["TheoShop v1.69"], (int)windowRect.Y + 30, 80, 0, Color.Black);
        TextHandling.DrawScreenCenteredText(["Changenotes:", "blablablablablablablablablabla", "jag orkar inte uppdatera detta varje gång"],
                                            (int)windowRect.Y + 175, 20, 30, Color.Black);
    }
}

public class SavePopup : PopupWindow
{
    public Dictionary<KeyboardKey, string> alphabet;
    public string fileName = "";

    public SavePopup(int width, int height, string[] messagesExtern) : base(width, height, messagesExtern)
    {
        alphabet = new();
        for (int i = (int)KeyboardKey.A; i <= (int)KeyboardKey.Z; i++)
        {
            alphabet.Add((KeyboardKey)i, ((char)(i + 32)).ToString());
        }

        // Add numbers 0-9
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
        if (keyPressed != KeyboardKey.Null && alphabet.ContainsKey(keyPressed) && fileName.Length <= 26)
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

public class ColorSelector : PopupWindow
{
    private Texture2D colors;
    private Image colorsImg;
    private Rectangle colorsRect;
    public ColorSelector(int width, int height, string[] messagesExtern) : base(width, height, messagesExtern)
    {
        colors = Raylib.LoadTexture("Icons/colors.png");
        colorsImg = Raylib.LoadImageFromTexture(colors);
        colorsRect = new(ProgramManager.ScreenWidth / 2 - colors.Width / 2, 300, colors.Width, colors.Height);
    }

    public override void Draw()
    {
        base.Draw();
        Raylib.DrawTexture(colors, (int)colorsRect.X, (int)colorsRect.Y, Color.White);
    }

    public override void Logic(Canvas canvas, Vector2 mousePos)
    {
        base.Logic(canvas, mousePos);
        if (Raylib.CheckCollisionPointRec(mousePos, colorsRect) && Raylib.IsMouseButtonDown(MouseButton.Left))
        {
            Color newColor = Raylib.GetImageColor(colorsImg, (int)mousePos.X - (int)colorsRect.X, (int)mousePos.Y - (int)colorsRect.Y);
            newColor.A = DrawTool.drawingColor.A;
            DrawTool.drawingColor = newColor;
        }
    }
}

public class LayerWindow : PopupWindow
{
    public List<LayerButton> layerButtons = new();

    private List<LayerWindowButton> buttons = 
    [
        new AddLayerButton() { buttonRect = new(720, 650, Button.buttonSize, Button.buttonSize) },
        new MoveLayerButton() { buttonRect = new(820, 650, Button.buttonSize, Button.buttonSize), direction = MoveLayerButton.Direction.Down },
        new LayerVisibilityButton() { buttonRect = new(920, 650, Button.buttonSize, Button.buttonSize) },
        new MoveLayerButton() { buttonRect = new(1020, 650, Button.buttonSize, Button.buttonSize), direction = MoveLayerButton.Direction.Up },
        new RemoveLayerButton() { buttonRect = new(1120, 650, Button.buttonSize, Button.buttonSize) }
    ];

    public LayerWindow(int width, int height, string[] messagesExtern) : base(width, height, messagesExtern) { }

    public override void Logic(Canvas canvas, Vector2 mousePos)
    {
        base.Logic(canvas, mousePos);
        layerButtons = new();

        for (int i = 0; i < canvas.layers.Count; i++)
        {
            layerButtons.Add(new() { buttonRect = new(i * 250 + 360, 475, 200, 100), ThisLayerNumber = i + 1, isVisible = canvas.layers[i].isVisible });
        }

        layerButtons.ForEach(l => l.OnHover(mousePos));
        buttons.ForEach(b => b.Update(mousePos, canvas));
    }

    public override void Draw()
    {
        base.Draw();

        layerButtons.ForEach(l => l.Draw());
        buttons.ForEach(b => b.Draw());
    }
}

public class ValueSetterWindow : PopupWindow
{
    public enum Changes
    {
        BrushRadius,
        Opacity,
        CheckerSize
    }

    public Changes thisChanges { get; set; }

    private Slider slider;

    public int minValue { get; set; }
    public int maxValue { get; set; }
    public int value;

    public ValueSetterWindow(int width, int height, string[] messagesExtern) : base(width, height, messagesExtern)
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
            case Changes.Opacity:
                DrawTool.drawingColor.A = (byte)value;
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
                Raylib.DrawCircle(ProgramManager.ScreenWidth / 2, 675, value, colorPreview);
                break;
            case Changes.Opacity:
                Raylib.DrawCircle(ProgramManager.ScreenWidth / 2, 675, 100, DrawTool.drawingColor);
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

public class SettingsWindow : PopupWindow
{
    private List<Button> buttons = 
    [ 
        new GUIColorButton(() => GUIarea.colorInt++, "Change GUI color") { buttonRect = new(340, 475, 1230, 100) },
        new GUIColorButton(() => ToolButton.colorSetInt++, "Change toolbutton color") { buttonRect = new(340, 600, 1230, 100) }
    ];

    public SettingsWindow(int width, int height, string[] messagesExtern) : base(width, height, messagesExtern)
    {
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
        int centerY = 675;

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

                // Offset to center the square within the 200x200 area
                int xOffset = (200 - (cols * Checker.checkerSize)) / 2;
                int yOffset = (200 - (rows * Checker.checkerSize)) / 2;

                // Adjusting position based on offset and center of previous shape
                xPos += xOffset + centerX - 100; // 100 is half of the side length of the square
                yPos += yOffset + centerY - 100;

                if ((row + col) % 2 == 0)
                    Raylib.DrawRectangle(xPos, yPos, Checker.checkerSize, Checker.checkerSize, DrawTool.drawingColor);
            }
        }
    }
}

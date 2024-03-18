using Microsoft.VisualBasic.FileIO;

namespace DrawingProgram;

public abstract class PopupWindow : IDrawable
{
    protected Rectangle windowRect;
    public string[] messages;

    public virtual void Draw()
    {
        Raylib.DrawRectangleRec(windowRect, GUIarea.guiColor);
        TextHandling.DrawScreenCenteredText(messages, (int)windowRect.Y + 20, 30, 60, Color.Black);
    }

    public virtual void Logic(Canvas canvas, Vector2 mousePos)
    {

    }

    public PopupWindow(int width, int height, string[] messagesExtern)
    {
        windowRect = new Rectangle(ProgramManager.ScreenWidth / 2 - width / 2, ProgramManager.ScreenHeight / 2 - height / 2, width, height);
        messages = messagesExtern;
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
            canvas.SaveProject(fileName + ".png");
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
        if (Raylib.CheckCollisionPointRec(mousePos, colorsRect) && Raylib.IsMouseButtonDown(MouseButton.Left))
        {
            DrawTool.drawingColor = Raylib.GetImageColor(colorsImg, (int)mousePos.X - (int)colorsRect.X, (int)mousePos.Y - (int)colorsRect.Y);
        }
    }
}

public class LayerWindow : PopupWindow
{
    public List<LayerButton> layerButtons = new();
    private AddLayerButton addLayer = new() { buttonRect = new(820, 650, Button.buttonSize, Button.buttonSize) };
    private RemoveLayerButton removeLayer = new() { buttonRect = new(920, 650, Button.buttonSize, Button.buttonSize) };
    private LayerVisibilityButton layerVisibility = new() { buttonRect = new(1020, 650, Button.buttonSize, Button.buttonSize) };

    public LayerWindow(int width, int height, string[] messagesExtern) : base(width, height, messagesExtern) { }

    public override void Logic(Canvas canvas, Vector2 mousePos)
    {
        layerButtons = new();

        for (int i = 0; i < canvas.layers.Count; i++)
        {
            layerButtons.Add(new() { buttonRect = new(i * 250 + 360, 475, 200, 100), ThisLayerNumber = i + 1, isVisible = canvas.layers[i].isVisible });
        }

        layerButtons.ForEach(l => l.OnHover(mousePos));

        addLayer.Update(mousePos, canvas);
        removeLayer.Update(mousePos, canvas);
        layerVisibility.Update(mousePos, canvas);
    }

    public override void Draw()
    {
        base.Draw();

        layerButtons.ForEach(l => l.Draw());

        addLayer.Draw();
        removeLayer.Draw();
        layerVisibility.Draw();
    }
}

public class ValueSetterWindow : PopupWindow
{
    private Slider slider;

    public int minValue { get; set; }
    public int maxValue { get; set; }

    int value;

    private Action<int> UpdateValue;

    public ValueSetterWindow(int width, int height, string[] messagesExtern, Action<int> valueSetter) : base(width, height, messagesExtern)
    {
        int sliderWidth = 500;
        int sliderHeight = 15;

        int sliderX = (int)(windowRect.X + windowRect.Width/2 - sliderWidth/2);

        UpdateValue = valueSetter;
        slider = new(20, new(sliderX, 475, sliderWidth, sliderHeight));
    }

    public override void Logic(Canvas canvas, Vector2 mousePos)
    {
        value = slider.GetValue(mousePos, minValue, maxValue);
        UpdateValue(value);
    }

    public override void Draw()
    {
        base.Draw();
        slider.Draw();

        bool isRadiusWindow = false;
        foreach (string str in messages)
        {
            if (str.Contains("radius"))
            {
                isRadiusWindow = true;
                break;
            }
        }

        if (isRadiusWindow)
            Raylib.DrawCircle(ProgramManager.ScreenWidth/2, 675, value, DrawTool.drawingColor);

        else
            CheckerPreview.DrawCheckerPreview();
            
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

                Vector2 squareCenter = new Vector2(xPos + Checker.checkerSize / 2, yPos + Checker.checkerSize / 2);

                if ((row + col) % 2 == 0)
                    Raylib.DrawRectangle(xPos, yPos, Checker.checkerSize, Checker.checkerSize, DrawTool.drawingColor);
            }
        }
    }
}

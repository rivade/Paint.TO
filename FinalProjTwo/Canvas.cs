namespace DrawingProgram;

public class Canvas : IDrawable
{
    public const int CanvasWidth = ProgramManager.ScreenWidth - 200;
    public const int CanvasHeight = ProgramManager.ScreenHeight - 100;

    public static List<Image> layers = new();
    public List<Texture2D> layerTextures = new();
    public static int currentLayer = 0;
    public Texture2D canvasTexture;
    private Texture2D transparencyBG = Raylib.LoadTexture("Icons/transparent.png");

    public Canvas()
    {
        Image temp = Raylib.GenImageColor(2500, 2500, Color.Blank);
        Raylib.ImageDrawRectangle(ref temp, 0, 0, CanvasWidth, CanvasHeight, Color.White);
        layers.Add(temp);
    }

    public void Update(Vector2 mousePos, DrawTool tool)
    {
        if (IsCursorOnCanvas(mousePos))
        {
            PreStrokeSaveCanvas(layers[currentLayer]);
            tool.Stroke(layers[currentLayer], mousePos);
        }

        layers[currentLayer] = UndoStroke(layers[currentLayer]);
    }

    private bool IsCursorOnCanvas(Vector2 cursor)
    {
        return cursor.X < CanvasWidth && cursor.Y < CanvasHeight;
    }

    public void SaveProject(string fileName)
    {
        Raylib.ExportImage(CropCanvas(CompressLayers(layers), Raylib.GenImageColor(CanvasWidth, CanvasHeight, Color.Blank)), fileName);
    }

    public void LoadProject(ref Image newImage)
    {
        currentLayer = 0;
        Raylib.ImageResize(ref newImage, CanvasWidth, CanvasHeight);
        layers = [ CropCanvas(newImage, Raylib.GenImageColor(2500, 2500, Color.Blank)) ];
    }

    private Image CropCanvas(Image canvas, Image newImage)
    {
        for (int x = 0; x < CanvasWidth; x++)
        {
            for (int y = 0; y < CanvasHeight; y++)
            {
                Color pixelColor = Raylib.GetImageColor(canvas, x, y);
                Raylib.ImageDrawPixel(ref newImage, x, y, pixelColor);
            }
        }
        return newImage;
    }

    private Image CompressLayers(List<Image> layers)
    {
        Image result = Raylib.GenImageColor(CanvasWidth, CanvasHeight, Color.Blank);
        foreach (Image layer in layers)
        {
            Raylib.ImageDraw(ref result, layer, new(0, 0, CanvasWidth, CanvasHeight), new(0, 0, CanvasWidth, CanvasHeight), Color.White);
        }
        return result;
    }

    public void Draw()
    {
        Raylib.DrawTexture(transparencyBG, 0, 0, Color.White);
        foreach (Image layer in layers)
        {
            layerTextures.Add(Raylib.LoadTextureFromImage(layer));
        }
        foreach (Texture2D layer in layerTextures)
        {
            Raylib.DrawTexture(layer, 0, 0, Color.White);
        }
    }






    Stack<Image> strokes = new();
    Stack<Image> CleanupStrokeStack(Stack<Image> strokes)
    {
        Stack<Image> tempReverse = new();

        while (strokes.Count > 0) tempReverse.Push(strokes.Pop());

        tempReverse.Pop();

        while (tempReverse.Count > 0) strokes.Push(tempReverse.Pop());

        return strokes;
    }
    void PreStrokeSaveCanvas(Image canvas)
    {
        if (Raylib.IsMouseButtonPressed(MouseButton.Left))
            strokes.Push(Raylib.ImageCopy(canvas));

        if (strokes.Count > 20)
            strokes = CleanupStrokeStack(strokes);
    }
    Image UndoStroke(Image canvas)
    {
        try
        {
            return (Raylib.IsKeyPressed(KeyboardKey.Z) && ProgramManager.popupWindow == null) ? strokes.Pop() : canvas;
        }
        catch (InvalidOperationException)
        {
            return canvas;
        }
    }
}
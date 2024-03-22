namespace DrawingProgram;
using System.IO;

public class Canvas : IDrawable
{
    public const int CanvasWidth = ProgramManager.ScreenWidth - 200;
    public const int CanvasHeight = ProgramManager.ScreenHeight - 100;

    public List<Layer> layers = new();
    public static int currentLayer = 0;

    public List<Texture2D> layerTextures = new();
    private Texture2D transparencyBG = Raylib.LoadTexture("Icons/transparent.png");

    public Canvas()
    {
        layers.Add(new());
        Raylib.ImageDrawRectangle(ref layers[0].canvasImg, 0, 0, CanvasWidth, CanvasHeight, Color.White);
    }

    public void Update(Vector2 mousePos, DrawTool tool)
    {
        layers[currentLayer].Logic(mousePos, tool);
    }

    public void SaveProject(string fileName, string directory)
    {
        string path = directory + @"\" + fileName;
        Raylib.ExportImage(CropCanvas(CompressLayers(layers), Raylib.GenImageColor(CanvasWidth, CanvasHeight, Color.Blank)), path);
        ProgramManager.popupWindow = null;
    }

    public void LoadProject(Image newImage)
    {
        currentLayer = 0;
        Raylib.ImageResize(ref newImage, CanvasWidth, CanvasHeight);
        layers = [new()];
        layers[currentLayer].canvasImg = CropCanvas(newImage, Raylib.GenImageColor(2500, 1600, Color.Blank));
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

    private Image CompressLayers(List<Layer> layers)
    {
        Image result = Raylib.GenImageColor(CanvasWidth, CanvasHeight, Color.Blank);
        foreach (Layer layer in layers)
        {
            Raylib.ImageDraw(ref result, layer.canvasImg, new(0, 0, CanvasWidth, CanvasHeight), new(0, 0, CanvasWidth, CanvasHeight), Color.White);
        }
        return result;
    }

    public void Draw()
    {
        Raylib.DrawTexture(transparencyBG, 0, 0, Color.White);
        layers.ForEach(l => l.Draw());
    }
}

public class Layer
{
    public Image canvasImg;
    public Texture2D canvasTexture;
    public Stack<Image> strokes;

    public bool isVisible = true;

    public Layer()
    {
        canvasImg = Raylib.GenImageColor(2500, 1600, Color.Blank);
        strokes = new();
    }

    public void Draw()
    {
        if (isVisible)
        {
            canvasTexture = Raylib.LoadTextureFromImage(canvasImg);
            Raylib.DrawTexture(canvasTexture, 0, 0, Color.White);
        }
    }

    public void Logic(Vector2 mousePos, DrawTool tool)
    {
        if (IsCursorOnCanvas(mousePos))
        {
            PreStrokeSaveCanvas(canvasImg);
            tool.Stroke(canvasImg, mousePos);
        }

        canvasImg = UndoStroke(canvasImg);
    }

    private bool IsCursorOnCanvas(Vector2 cursor)
    {
        return cursor.X < Canvas.CanvasWidth && cursor.Y < Canvas.CanvasHeight;
    }

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
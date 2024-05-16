namespace DrawingProgram;

public class Canvas : IDrawable
{
    public const int CanvasWidth = ProgramManager.ScreenWidth - 200;
    public const int CanvasHeight = ProgramManager.ScreenHeight - 100;

    // Makes it so that the top left corner of the canvas on screen isn't (0,0)
    // That caused issues when for example drawing a circle there as it would reach out of bounds (negative x and y coordinates)
    public const int CanvasOffset = 500;

    public const int CanvasImgSize = 2500;

    public List<Layer> layers = new();
    public int currentLayer = 0;

    public List<Texture2D> layerTextures = new();
    private Texture2D transparencyBG = Raylib.LoadTexture("Textures/transparent.png");

    private ProgramManager program;

    public static Vector2 relativeMousePos;
    public static Vector2 relativeLastMousePos;

    public Canvas(ProgramManager programInstance)
    {
        program = programInstance;
        layers.Add(new(program));
        Raylib.ImageDrawRectangle(ref layers[0].canvasImg, CanvasOffset, CanvasOffset, CanvasWidth, CanvasHeight, Color.White);
    }

    private static void UpdateRelativeMousePos(Vector2 mousePos, Vector2 lastMousePos)
    {
        relativeMousePos = mousePos += Vector2.One * CanvasOffset;
        relativeLastMousePos = lastMousePos += Vector2.One * CanvasOffset;
    }

    public void Update(Vector2 mousePos, Vector2 lastMousePos, DrawTool tool)
    {
        UpdateRelativeMousePos(mousePos, lastMousePos);
        layers[currentLayer].Logic(mousePos, tool);
    }

    public void SaveProject(string fileName, string directory)
    {
        string path = directory + @"\" + fileName;
        Raylib.ExportImage(CropCanvas(FuseLayers(layers), Raylib.GenImageColor(CanvasWidth, CanvasHeight, Color.Blank)), path);
        program.popupWindow = null;
    }

    public void LoadProject(Image newImage)
    {
        currentLayer = 0;
        Raylib.ImageResize(ref newImage, CanvasWidth, CanvasHeight);
        layers = [new(program)];
        layers[currentLayer].canvasImg = Raylib.GenImageColor(CanvasImgSize, CanvasImgSize, Color.Blank);
        Raylib.ImageDraw(ref layers[currentLayer].canvasImg, newImage, new(0, 0, CanvasWidth, CanvasHeight), new(CanvasOffset, CanvasOffset, CanvasWidth, CanvasHeight), Color.White);
    }

    public static Image CropCanvas(Image canvas, Image newImage)
    {
        for (int x = CanvasOffset; x < CanvasWidth + CanvasOffset; x++)
        {
            for (int y = CanvasOffset; y < CanvasHeight + CanvasOffset; y++)
            {
                Color pixelColor = Raylib.GetImageColor(canvas, x, y);
                Raylib.ImageDrawPixel(ref newImage, x - CanvasOffset, y - CanvasOffset, pixelColor);
            }
        }
        return newImage;
    }

    private static Image FuseLayers(List<Layer> layers)
    {
        Image result = Raylib.GenImageColor(CanvasImgSize, CanvasImgSize, Color.Blank);
        foreach (Layer layer in layers)
        {
            Raylib.ImageDraw(ref result, layer.canvasImg, new(0, 0, CanvasImgSize, CanvasImgSize), new(0, 0, CanvasImgSize, CanvasImgSize), Color.White);
        }
        return result;
    }

    public void CompressLayersInProject()
    {
        currentLayer = 0;
        layers = [new(program) { canvasImg = FuseLayers(layers) }];
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

    private ProgramManager program;

    public Layer(ProgramManager programInstance)
    {
        program = programInstance;
        canvasImg = Raylib.GenImageColor(Canvas.CanvasImgSize, Canvas.CanvasImgSize, Color.Blank);
        strokes = new();
    }

    public void Draw()
    {
        if (isVisible)
        {
            Raylib.UnloadTexture(canvasTexture);
            canvasTexture = Raylib.LoadTextureFromImage(canvasImg);
            Raylib.DrawTexture(canvasTexture, -Canvas.CanvasOffset, -Canvas.CanvasOffset, Color.White);
        }
    }

    public void Logic(Vector2 mousePos, DrawTool tool)
    {
        if (IsCursorOnCanvas(mousePos))
        {
            PreStrokeSaveCanvas(canvasImg);
            tool.Stroke(canvasImg, Canvas.relativeMousePos, Canvas.relativeLastMousePos);
        }

        canvasImg = UndoStroke(canvasImg);
    }

    private bool IsCursorOnCanvas(Vector2 mousePos)
    {
        return mousePos.X < Canvas.CanvasWidth && mousePos.Y < Canvas.CanvasHeight;
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
        if (Raylib.IsMouseButtonPressed(MouseButton.Left) || Raylib.IsMouseButtonPressed(MouseButton.Right))
            strokes.Push(Raylib.ImageCopy(canvas));

        if (strokes.Count > 20)
            strokes = CleanupStrokeStack(strokes);
    }
    Image UndoStroke(Image canvas)
    {
        try
        {
            return (Raylib.IsKeyPressed(KeyboardKey.Z) && program.popupWindow == null) ? strokes.Pop() : canvas;
        }
        catch (InvalidOperationException)
        {
            return canvas;
        }
    }
}
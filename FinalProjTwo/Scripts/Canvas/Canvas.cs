namespace DrawingProgram;

public class Canvas : IDrawable
{
    public static readonly int CanvasWidth = ProgramManager.ScreenWidth - 200;
    public static readonly int CanvasHeight = ProgramManager.ScreenHeight - 100;

    // Makes it so that the top left corner of the canvas on screen isn't (0,0)
    // That caused issues when for example drawing a circle there as it would reach out of bounds (negative x and y coordinates)
    public const int CanvasOffset = 500;
    public const int CanvasImgSize = 2500;

    public List<Layer> layers = new();
    public int currentLayer = 0;

    private Texture2D transparencyBG = Raylib.LoadTexture("Textures/transparent.png");

    private Image backgroundImg;
    private Texture2D backgroundTxt;

    private ProgramManager program;

    public Canvas(ProgramManager programInstance)
    {
        program = programInstance;
        layers.Add(new(program));
        backgroundImg = Raylib.GenImageColor(CanvasWidth, CanvasHeight, Color.White);
        backgroundTxt = Raylib.LoadTextureFromImage(backgroundImg);
    }


    public void Update(Vector2 mousePos, ITool tool) => layers[currentLayer].Logic(mousePos, tool);

    public void SaveProject(string fileName, string directory)
    {
        string path = directory + @"\" + fileName;
        Image temp = Raylib.ImageCopy(backgroundImg);
        Raylib.ImageDraw(ref temp, CropCanvas(FuseLayers(layers), Raylib.GenImageColor(CanvasWidth, CanvasHeight, Color.Blank)), new(0, 0, CanvasWidth, CanvasHeight), new(0, 0, CanvasWidth, CanvasHeight), Color.White);
        Raylib.ExportImage(temp, path);
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

    public void ChangeBackgroundColor(Color newColor)
    {
        Raylib.UnloadImage(backgroundImg);
        Raylib.UnloadTexture(backgroundTxt);
        backgroundImg = Raylib.GenImageColor(CanvasWidth, CanvasHeight, newColor);
        backgroundTxt = Raylib.LoadTextureFromImage(backgroundImg);

    }

    public void Draw()
    {
        Raylib.DrawTexture(transparencyBG, 0, 0, Color.White);
        Raylib.DrawTexture(backgroundTxt, 0, 0, Color.White);
        layers.ForEach(l => l.Draw());
    }

}
namespace DrawingProgram;

public class Canvas : IDrawable
{
    public const int CanvasWidth = ProgramManager.ScreenWidth - 200;
    public const int CanvasHeight = ProgramManager.ScreenHeight - 100;

    public Image canvasImg;
    public Texture2D canvasTexture;

    public Canvas()
    {
        canvasImg = Raylib.GenImageColor(2500, 2500, Color.LightGray);
        Raylib.ImageDrawRectangle(ref canvasImg, 0, 0, CanvasWidth, CanvasHeight, Color.White);
    }

    public void Update(Vector2 mousePos, DrawTool tool)
    {
        if (IsCursorOnCanvas(mousePos))
        {
            DrawTool.PreStrokeSaveCanvas(canvasImg);
            tool.Stroke(canvasImg, mousePos);
        }

        canvasImg = DrawTool.UndoStroke(canvasImg);
    }

    private bool IsCursorOnCanvas(Vector2 cursor)
    {
        return cursor.X < CanvasWidth && cursor.Y < CanvasHeight;
    }

    public void SaveProject(string fileName)
    {
        Raylib.ExportImage(CropCanvas(canvasImg, Raylib.GenImageColor(CanvasWidth, CanvasHeight, Color.White)), fileName);
    }

    public void LoadProject(ref Image newImage)
    {
        Raylib.ImageResize(ref newImage, CanvasWidth, CanvasHeight);
        canvasImg = CropCanvas(newImage, Raylib.GenImageColor(2500, 2500, Color.LightGray));
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

    public void Draw()
    {
        canvasTexture = Raylib.LoadTextureFromImage(canvasImg);
        Raylib.DrawTexture(canvasTexture, 0, 0, Color.White);
    }
}
namespace DrawingProgram;

public class Canvas : IDrawable
{
    public const int CanvasWidth = ProgramManager.ScreenWidth - 200;
    public const int CanvasHeight = ProgramManager.ScreenHeight - 100;

    public Image canvasImg;
    public Texture2D canvasTexture;

    public Canvas()
    {
        canvasImg = Raylib.GenImageColor(ProgramManager.ScreenWidth, ProgramManager.ScreenHeight, Color.White);
    }

    public void Update(Vector2 mousePos, DrawTool tool)
    {
        if (IsCursorOnCanvas(mousePos))
        {
            DrawTool.PreStrokeSaveCanvas(canvasImg);
            tool.Draw(canvasImg, mousePos);
        }

        canvasImg = DrawTool.UndoStroke(canvasImg);
    }

    private bool IsCursorOnCanvas(Vector2 cursor)
    {
        return cursor.X < CanvasWidth && cursor.Y < CanvasHeight;
    }

    public void SaveProject()
    {
        string fileName = "drawing.png";
        Raylib.ExportImage(canvasImg, fileName);
    }

    public void Draw()
    {
        canvasTexture = Raylib.LoadTextureFromImage(canvasImg);
        Raylib.DrawTexture(canvasTexture, 0, 0, Color.White);
        Raylib.DrawRectangle(CanvasWidth, 0, 200, ProgramManager.ScreenHeight, Color.Gray);
        Raylib.DrawRectangle(0, CanvasHeight, ProgramManager.ScreenWidth, 100, Color.Gray);
    }
}
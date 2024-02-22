namespace DrawingProgram;

public class Canvas : IDrawable
{
    public Image canvasImg;
    public Texture2D canvasTexture;

    public Canvas()
    {
        canvasImg = Raylib.GenImageColor(ProgramManager.CanvasWidth, ProgramManager.CanvasHeight, Color.White);
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
        return cursor.X < ProgramManager.CanvasWidth && cursor.Y < ProgramManager.CanvasHeight;
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
    }
}
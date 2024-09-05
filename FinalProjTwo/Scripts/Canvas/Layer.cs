namespace DrawingProgram;

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

    public void Logic(Vector2 mousePos, ITool tool)
    {
        if (IsCursorOnCanvas(mousePos))
        {
            PreStrokeSaveCanvas(canvasImg);
            tool.Update(canvasImg, mousePos + Vector2.One * Canvas.CanvasOffset);
        }

        canvasImg = UndoStroke();
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
    public Image UndoStroke()
    {
        try
        {
            return (Raylib.IsKeyPressed(KeyboardKey.Z) && program.popupWindow == null) ? strokes.Pop() : canvasImg;
        }
        catch (InvalidOperationException)
        {
            return canvasImg;
        }
    }
}
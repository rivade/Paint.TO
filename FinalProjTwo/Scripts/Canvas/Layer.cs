namespace DrawingProgram;

public class Layer
{
    public Image canvasImg;
    public Texture2D canvasTexture;
    public Stack<Image> strokes;
    public Stack<Image> undos;

    public bool isVisible = true;

    private ProgramManager program;

    public Layer(ProgramManager programInstance)
    {
        program = programInstance;
        canvasImg = Raylib.GenImageColor(Canvas.CanvasImgSize, Canvas.CanvasImgSize, Color.Blank);
        strokes = new();
        undos = new();
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
            if (Raylib.IsMouseButtonPressed(MouseButton.Left))
            {
                undos.Clear();
            }
        }

        if(!Raylib.IsKeyDown(KeyboardKey.LeftShift) && Raylib.IsKeyPressed(KeyboardKey.Z)) UndoStroke();
        if(Raylib.IsKeyDown(KeyboardKey.LeftShift) && Raylib.IsKeyPressed(KeyboardKey.Z)) RedoStroke();
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
    public void UndoStroke()
    {
        try
        {
            if (program.popupWindow == null && strokes.Count > 0)
            {
                undos.Push(Raylib.ImageCopy(canvasImg));
                canvasImg = strokes.Pop();
            }
        }
        catch (InvalidOperationException) {}
    }

    public void RedoStroke()
    {
        try
        {
            if (program.popupWindow == null && undos.Count > 0)
            {
                strokes.Push(Raylib.ImageCopy(canvasImg));
                canvasImg = undos.Pop();
            }
        }
        catch (InvalidOperationException) {}
    }
}
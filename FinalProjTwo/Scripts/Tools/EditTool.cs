namespace DrawingProgram;

public abstract class EditTool : DrawTool
{

}

public sealed class RectangleSelect : EditTool
{
    public Rectangle selectionRec = new(0, 0, 0, 0);
    private Rectangle sourceRec = new(0, 0, 0, 0);
    private Image selection;
    public Texture2D selectionPreview;
    private Vector2 startPos;
    private bool hasMadeSelection;
    private bool isResizing;
    private bool isMoving;
    public static readonly Color selectionColor = new(0, 78, 129, 125);

    public List<SelectionCorner> corners;

    public RectangleSelect()
    {
        hasMadeSelection = false;
        isResizing = false;
    }

    public void CreateSelection(Vector2 mousePos, Image canvas)
    {
        corners = [];
        if (Raylib.IsMouseButtonPressed(MouseButton.Left))
            startPos = mousePos;

        if (Raylib.IsMouseButtonDown(MouseButton.Left))
            UpdateRect(mousePos);

        if (Raylib.IsMouseButtonReleased(MouseButton.Left))
        {
            if (selectionRec.Width < 5 && selectionRec.Height < 5) 
            {
                selectionPreview = new();
                return;
            }
            hasMadeSelection = true;

            corners =
            [
                new TopLeftSelectionCorner(new Vector2(selectionRec.X, selectionRec.Y)),
                new TopRightSelectionCorner(new Vector2(selectionRec.X + selectionRec.Width, selectionRec.Y)),
                new BottomLeftSelectionCorner(new Vector2(selectionRec.X, selectionRec.Y + selectionRec.Height)),
                new BottomRightSelectionCorner(new Vector2(selectionRec.X + selectionRec.Width, selectionRec.Y + selectionRec.Height)),
            ];

            Rectangle relativeSelectionRec = new(new Vector2(selectionRec.X, selectionRec.Y) + Vector2.One * Canvas.CanvasOffset, new(selectionRec.Width, selectionRec.Height));
            selection = Raylib.GenImageColor((int)selectionRec.Width, (int)selectionRec.Height, Color.Blank);
            Raylib.ImageDraw(ref selection, canvas, relativeSelectionRec, new(0, 0, selectionRec.Width, selectionRec.Height), Color.White);
            Raylib.UnloadTexture(selectionPreview);
            selectionPreview = Raylib.LoadTextureFromImage(selection);
            ClearSelectionOnCanvas(canvas);
        }
    }

    public override void Stroke(Image canvas, Vector2 mousePos, Vector2 _)
    {
        mousePos -= Vector2.One * Canvas.CanvasOffset;

        if (!hasMadeSelection)
            CreateSelection(mousePos, canvas);

        else
        {
            if (isResizing)
            {
                Image temp = Raylib.ImageCopy(selection);
                Raylib.ImageResize(ref temp, (int)selectionRec.Width, (int)selectionRec.Height);
                Raylib.UnloadTexture(selectionPreview);
                selectionPreview = Raylib.LoadTextureFromImage(temp);
            }

            if (Raylib.IsMouseButtonPressed(MouseButton.Left))
            {
                if (Raylib.CheckCollisionPointRec(mousePos, selectionRec) && corners.All(c => !Raylib.CheckCollisionPointCircle(mousePos, c.cornerCircle.Middle, c.cornerCircle.Radius)))
                {
                    isMoving = true;
                }
            }

            if (Raylib.CheckCollisionPointRec(mousePos, selectionRec) && Raylib.IsMouseButtonReleased(MouseButton.Left))
            {
                isMoving = false;
            }

            if (isMoving)
            {
                Vector2 rectSize = new(selectionRec.Width, selectionRec.Height);
                Vector2 mouseDelta = Raylib.GetMouseDelta();
                Vector2 newPos = new(selectionRec.X + mouseDelta.X, selectionRec.Y + mouseDelta.Y);
                selectionRec = new(newPos, rectSize);
            }

            if (!Raylib.CheckCollisionPointRec(mousePos, selectionRec) && corners.All(c => !Raylib.CheckCollisionPointCircle(mousePos, c.cornerCircle.Middle, c.cornerCircle.Radius)) && Raylib.IsMouseButtonPressed(MouseButton.Left))
            {
                Rectangle relativeSelectionRec = new(new Vector2(selectionRec.X, selectionRec.Y) + Vector2.One * Canvas.CanvasOffset, new(selectionRec.Width, selectionRec.Height));
                Raylib.ImageDraw(ref canvas, selection, new(0, 0, sourceRec.Width, sourceRec.Height), relativeSelectionRec, Color.White);
                selectionPreview = new();
                hasMadeSelection = false;
                startPos = mousePos;
            }

            if (Raylib.IsKeyPressed(KeyboardKey.Delete) || Raylib.IsKeyPressed(KeyboardKey.Backspace))
            {
                hasMadeSelection = false;
                selectionRec = new();
                selectionPreview = new();
                ClearSelectionOnCanvas(canvas);
            }

            corners.ForEach(c => c.Update(ref selectionRec, () => sourceRec = selectionRec, mousePos));
            ResetCorners();
            ResizeSelection(mousePos, canvas);
        }
    }

    private void UpdateRect(Vector2 mousePos)
    {
        int x = Math.Min((int)startPos.X, (int)mousePos.X);
        int y = Math.Min((int)startPos.Y, (int)mousePos.Y);
        int width = Math.Abs((int)mousePos.X - (int)startPos.X);
        int height = Math.Abs((int)mousePos.Y - (int)startPos.Y);
        selectionRec = new Rectangle(x, y, width, height);
        sourceRec = new Rectangle(x, y, width, height);
    }
    

    private void ResetCorners()
    {
        corners[0].cornerCircle.Middle = new(selectionRec.X, selectionRec.Y);
        corners[1].cornerCircle.Middle = new(selectionRec.X + selectionRec.Width, selectionRec.Y);
        corners[2].cornerCircle.Middle = new(selectionRec.X, selectionRec.Y + selectionRec.Height);
        corners[3].cornerCircle.Middle = new(selectionRec.X + selectionRec.Width, selectionRec.Y + selectionRec.Height);
    }

    private void ResizeSelection(Vector2 mousePos, Image canvas)
    {
        if (corners.Any(c => Raylib.CheckCollisionPointCircle(mousePos, c.cornerCircle.Middle, c.cornerCircle.Radius)) && Raylib.IsMouseButtonPressed(MouseButton.Left))
        {
            isResizing = true;
        }

        if (Raylib.IsMouseButtonReleased(MouseButton.Left) && isResizing)
        {
            Raylib.ImageResize(ref selection, (int)selectionRec.Width, (int)selectionRec.Height);
            isResizing = false;
        }

    }

    private void ClearSelectionOnCanvas(Image canvas) => Raylib.ImageDrawRectangleRec(ref canvas, new(sourceRec.X + Canvas.CanvasOffset, sourceRec.Y + Canvas.CanvasOffset, sourceRec.Width, sourceRec.Height), Color.Blank);
}

public abstract class SelectionCorner
{
    public Circle cornerCircle;
    public const int circleRadius = 15;
    protected bool isMoving;

    public SelectionCorner(Vector2 cornerPos)
    {
        cornerCircle = new(cornerPos, cornerPos + Vector2.UnitX * circleRadius);
    }

    public virtual void Update(ref Rectangle selectionWindow, Action updateSourceRec, Vector2 mousePos)
    {
        if (Raylib.CheckCollisionPointCircle(mousePos, cornerCircle.Middle, cornerCircle.Radius) && Raylib.IsMouseButtonPressed(MouseButton.Left))
            isMoving = true;
        if (Raylib.IsMouseButtonReleased(MouseButton.Left))
            isMoving = false;
    }

    public void Draw() => Raylib.DrawCircle((int)cornerCircle.Middle.X, (int)cornerCircle.Middle.Y, cornerCircle.Radius, Color.Red);
}

public sealed class TopLeftSelectionCorner : SelectionCorner
{
    public TopLeftSelectionCorner(Vector2 cornerPos) : base(cornerPos) { }

    public override void Update(ref Rectangle selectionWindow, Action updateSourceRec, Vector2 mousePos)
    {
        base.Update(ref selectionWindow, updateSourceRec, mousePos);
        if (!isMoving) return;
        Vector2 mouseDelta = Raylib.GetMouseDelta();
        float newX = selectionWindow.X + mouseDelta.X;
        float newY = selectionWindow.Y + mouseDelta.Y;
        float newWidth = selectionWindow.Width - mouseDelta.X;
        float newHeight = selectionWindow.Height - mouseDelta.Y;
        selectionWindow = new Rectangle(newX, newY, newWidth, newHeight);
        updateSourceRec.Invoke();
    }
}

public sealed class TopRightSelectionCorner : SelectionCorner
{
    public TopRightSelectionCorner(Vector2 cornerPos) : base(cornerPos) { }

    public override void Update(ref Rectangle selectionWindow, Action updateSourceRec, Vector2 mousePos)
    {
        base.Update(ref selectionWindow, updateSourceRec, mousePos);
        if (!isMoving) return;
        Vector2 mouseDelta = Raylib.GetMouseDelta();
        float newY = selectionWindow.Y + mouseDelta.Y;
        float newWidth = selectionWindow.Width + mouseDelta.X;
        float newHeight = selectionWindow.Height - mouseDelta.Y;
        selectionWindow = new Rectangle(selectionWindow.X, newY, newWidth, newHeight);
        updateSourceRec.Invoke();
    }
}

public sealed class BottomLeftSelectionCorner : SelectionCorner
{
    public BottomLeftSelectionCorner(Vector2 cornerPos) : base(cornerPos) { }

    public override void Update(ref Rectangle selectionWindow, Action updateSourceRec, Vector2 mousePos)
    {
        base.Update(ref selectionWindow, updateSourceRec, mousePos);
        if (!isMoving) return;
        Vector2 mouseDelta = Raylib.GetMouseDelta();
        float newX = selectionWindow.X + mouseDelta.X;
        float newWidth = selectionWindow.Width - mouseDelta.X;
        float newHeight = selectionWindow.Height + mouseDelta.Y;
        selectionWindow = new Rectangle(newX, selectionWindow.Y, newWidth, newHeight);
        updateSourceRec.Invoke();
    }
}

public sealed class BottomRightSelectionCorner : SelectionCorner
{
    public BottomRightSelectionCorner(Vector2 cornerPos) : base(cornerPos) { }

    public override void Update(ref Rectangle selectionWindow, Action updateSourceRec, Vector2 mousePos)
    {
        base.Update(ref selectionWindow, updateSourceRec, mousePos);
        if (!isMoving) return;
        Vector2 mouseDelta = Raylib.GetMouseDelta();
        float newWidth = selectionWindow.Width + mouseDelta.X;
        float newHeight = selectionWindow.Height + mouseDelta.Y;
        selectionWindow = new Rectangle(selectionWindow.X, selectionWindow.Y, newWidth, newHeight);
        updateSourceRec.Invoke();
    }
}

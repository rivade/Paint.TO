namespace DrawingProgram;

public abstract class LayerWindowButton : Button
{
    protected Texture2D icon;
    protected Canvas canvas;

    public LayerWindowButton(ProgramManager programInstance, Rectangle buttonRect, Canvas canvasInstance) : base(programInstance, buttonRect)
    {
        canvas = canvasInstance;
    }
}

public sealed class LayerButton : LayerWindowButton
{
    public LayerButton(ProgramManager programInstance, Rectangle buttonRect, Canvas canvasInstance) : base(programInstance, buttonRect, canvasInstance) { }

    public int ThisLayerNumber { get; set; }
    public bool IsVisible { get; set; }

    private Color buttonColor;

    public override void Draw()
    {
        if (IsVisible)
            buttonColor = GetButtonColor(Color.Lime, Color.Green, Color.White, false);

        else
            buttonColor = GetButtonColor(Color.Gray, Color.LightGray, Color.White, false);

        if (canvas.currentLayer == ThisLayerNumber - 1)
            Raylib.DrawRectangle((int)buttonRect.X - 5, (int)buttonRect.Y - 5, (int)buttonRect.Width + 10, (int)buttonRect.Height + 10, Color.Red);

        Raylib.DrawRectangleRec(buttonRect, buttonColor);

        TextHandling.DrawCenteredTextPro([$"Layer {ThisLayerNumber}"],
        (int)buttonRect.X, (int)buttonRect.X + (int)buttonRect.Width,
        (int)buttonRect.Y + 20, 30, 0, Color.Black);
    }

    public override void OnClick()
    {
        canvas.currentLayer = ThisLayerNumber - 1;
    }
}

public sealed class AddLayerButton : LayerWindowButton
{
    public AddLayerButton(ProgramManager programInstance, Rectangle buttonRect, Canvas canvasInstance) : base(programInstance, buttonRect, canvasInstance)
    {
        icon = Raylib.LoadTexture("Textures/Icons/plus.png");
        infoWindow = new("Add new layer", (int)buttonRect.X, (int)buttonRect.Y + buttonSize + 5);
    }

    public override void Draw()
    {
        Raylib.DrawRectangleRec(buttonRect, GetButtonColor(Color.Lime, Color.Green, Color.White, false));
        Raylib.DrawTexture(icon, (int)buttonRect.X, (int)buttonRect.Y, Color.White);
        base.Draw();
    }

    public override void OnClick()
    {
        if (canvas.layers.Count < 5)
            canvas.layers.Add(new(program));

        canvas.currentLayer = canvas.layers.Count - 1;
    }
}

public sealed class RemoveLayerButton : LayerWindowButton
{
    public RemoveLayerButton(ProgramManager programInstance, Rectangle buttonRect, Canvas canvasInstance) : base(programInstance, buttonRect, canvasInstance)
    {
        icon = Raylib.LoadTexture("Textures/Icons/x.png");
        infoWindow = new("Remove current layer", (int)buttonRect.X, (int)buttonRect.Y + buttonSize + 5);
    }

    public override void Draw()
    {
        Raylib.DrawRectangleRec(buttonRect, GetButtonColor(Color.Red, Color.Pink, Color.White, false));
        Raylib.DrawTexture(icon, (int)buttonRect.X, (int)buttonRect.Y, Color.White);
        base.Draw();
    }

    public override void OnClick()
    {
        if (canvas.layers.Count != 1)
        {
            canvas.layers.Remove(canvas.layers[canvas.currentLayer]);

            if (canvas.currentLayer != 0) canvas.currentLayer--;
            else canvas.currentLayer = 0;
        }
    }
}

public sealed class LayerVisibilityButton : LayerWindowButton
{
    private List<Texture2D> icons = new();
    public int currentIcon = 0;

    public LayerVisibilityButton(ProgramManager programInstance, Rectangle buttonRect, Canvas canvasInstance) : base(programInstance, buttonRect, canvasInstance)
    {
        icons.Add(Raylib.LoadTexture("Textures/Icons/visible.png"));
        icons.Add(Raylib.LoadTexture("Textures/Icons/invisible.png"));
        infoWindow = new("Toggle layer visibility", (int)buttonRect.X, (int)buttonRect.Y + buttonSize + 5);
    }

    public override void Draw()
    {
        Raylib.DrawRectangleRec(buttonRect, GetButtonColor(Color.LightGray, Color.White, Color.White, false));
        Raylib.DrawTexture(icons[currentIcon], (int)buttonRect.X, (int)buttonRect.Y, Color.White);
        base.Draw();
    }

    public override void OnHover(Vector2 mousePos)
    {
        currentIcon = canvas.layers[canvas.currentLayer].isVisible ? 0 : 1;
        base.OnHover(mousePos);
    }

    public override void OnClick()
    {
        canvas.layers[canvas.currentLayer].isVisible = !canvas.layers[canvas.currentLayer].isVisible;
    }
}

public sealed class MergeLayersButton : LayerWindowButton
{
    public MergeLayersButton(ProgramManager programInstance, Rectangle buttonRect, Canvas canvasInstance) : base(programInstance, buttonRect, canvasInstance)
    {
        icon = Raylib.LoadTexture("Textures/Icons/compressicon.png");
        infoWindow = new("Merge all layers", (int)buttonRect.X, (int)buttonRect.Y + buttonSize + 5);
    }

    public override void Draw()
    {
        Raylib.DrawRectangleRec(buttonRect, GetButtonColor(Color.LightGray, Color.White, Color.White, false));
        Raylib.DrawTexture(icon, (int)buttonRect.X, (int)buttonRect.Y, Color.White);
        base.Draw();
    }

    public override void OnClick()
    {
        canvas.CompressLayers();
    }
}

public sealed class MoveLayerButton : LayerWindowButton
{
    public enum Direction
    {
        Up,
        Down
    }
    public Direction direction { get; set; }
    private Texture2D[] icons;

    public MoveLayerButton(ProgramManager programInstance, Rectangle buttonRect, Canvas canvasInstance) : base(programInstance, buttonRect, canvasInstance)
    {
        icons = [Raylib.LoadTexture("Textures/Icons/rightarrow.png"), Raylib.LoadTexture("Textures/Icons/leftarrow.png")];
    }

    public override void OnHover(Vector2 mousePos)
    {
        base.OnHover(mousePos);

        if (isHoveredOn)
        {
            switch (direction)
            {
                case Direction.Up:
                    infoWindow = new("Move layer up hierarchy", (int)buttonRect.X, (int)buttonRect.Y + buttonSize + 5);
                    break;
                case Direction.Down:
                    infoWindow = new("Move layer down hierarchy", (int)buttonRect.X, (int)buttonRect.Y + buttonSize + 5);
                    break;
            }
        }
    }

    public override void Draw()
    {
        Raylib.DrawRectangleRec(buttonRect, GetButtonColor(Color.LightGray, Color.White, Color.White, false));
        switch (direction)
        {
            case Direction.Up:
                Raylib.DrawTexture(icons[0], (int)buttonRect.X, (int)buttonRect.Y, Color.White);
                break;
            case Direction.Down:
                Raylib.DrawTexture(icons[1], (int)buttonRect.X, (int)buttonRect.Y, Color.White);
                break;
        }
        base.Draw();
    }

    public override void OnClick()
    {
        switch (direction)
        {
            case Direction.Up:
                if (canvas.layers.Count != 1 && canvas.currentLayer != canvas.layers.Count - 1)
                {
                    SwapListIndices(ref canvas.layers, canvas.currentLayer, canvas.currentLayer + 1);
                    canvas.currentLayer++;
                }
                break;

            case Direction.Down:
                if (canvas.currentLayer != 0 && canvas.layers.Count != 1)
                {
                    SwapListIndices(ref canvas.layers, canvas.currentLayer, canvas.currentLayer - 1);
                    canvas.currentLayer--;
                }
                break;
        }
    }

    private static void SwapListIndices<T>(ref List<T> list, int index1, int index2)
    {
        T tmp = list[index1];
        list[index1] = list[index2];
        list[index2] = tmp;
    }
}
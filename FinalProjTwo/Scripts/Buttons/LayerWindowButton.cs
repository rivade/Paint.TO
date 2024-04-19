namespace DrawingProgram;

public abstract class LayerWindowButton : Button
{
    protected Texture2D icon;

    protected LayerWindowButton(ProgramManager programInstance) : base(programInstance) {}

    public virtual void Update(Vector2 mousePos, Canvas canvas)
    {
        isHoveredOn = false;
        if (Raylib.CheckCollisionPointRec(mousePos, buttonRect))
        {
            isHoveredOn = true;

            if (Raylib.IsMouseButtonPressed(MouseButton.Left))
                Click(canvas);
        }

        infoWindow = null;
    }

    public virtual void Click(Canvas canvas) { }
}

public sealed class LayerButton : LayerWindowButton
{
    public LayerButton(ProgramManager programInstance) : base(programInstance) {}

    public int ThisLayerNumber { get; set; }
    public bool isVisible { get; set; }

    public override void Draw()
    {
        if (isVisible)
            GetButtonColor(Color.Lime, Color.Green, Color.White, false);

        else
            GetButtonColor(Color.Gray, Color.LightGray, Color.White, false);

        if (Canvas.currentLayer == ThisLayerNumber - 1)
            Raylib.DrawRectangle((int)buttonRect.X - 5, (int)buttonRect.Y - 5, (int)buttonRect.Width + 10, (int)buttonRect.Height + 10, Color.Red);

        Raylib.DrawRectangleRec(buttonRect, buttonColor);

        TextHandling.DrawCenteredTextPro([$"Layer {ThisLayerNumber}"],
        (int)buttonRect.X, (int)buttonRect.X + (int)buttonRect.Width,
        (int)buttonRect.Y + 20, 30, 0, Color.Black);
    }


    public override void OnClick()
    {
        Canvas.currentLayer = ThisLayerNumber - 1;
    }
}

public sealed class AddLayerButton : LayerWindowButton
{
    public AddLayerButton(ProgramManager programInstance) : base(programInstance)
    {
        icon = Raylib.LoadTexture("Textures/Icons/plus.png");
    }

    public override void Update(Vector2 mousePos, Canvas canvas)
    {
        base.Update(mousePos, canvas);

        if (isHoveredOn)
            infoWindow = new("Add new layer", (int)buttonRect.X, (int)buttonRect.Y + buttonSize + 5);
    }
    public override void Draw()
    {
        GetButtonColor(Color.Lime, Color.Green, Color.White, false);
        Raylib.DrawRectangleRec(buttonRect, buttonColor);
        Raylib.DrawTexture(icon, (int)buttonRect.X, (int)buttonRect.Y, Color.White);
        base.Draw();
    }

    public override void Click(Canvas canvas)
    {
        if (canvas.layers.Count < 5)
            canvas.layers.Add(new(program));

        Canvas.currentLayer = canvas.layers.Count - 1;
    }
}

public sealed class RemoveLayerButton : LayerWindowButton
{
    public RemoveLayerButton(ProgramManager programInstance) : base(programInstance)
    {
        icon = Raylib.LoadTexture("Textures/Icons/x.png");
    }

    public override void Update(Vector2 mousePos, Canvas canvas)
    {
        base.Update(mousePos, canvas);

        if (isHoveredOn)
            infoWindow = new("Remove current layer", (int)buttonRect.X, (int)buttonRect.Y + buttonSize + 5);
    }

    public override void Draw()
    {
        GetButtonColor(Color.Red, Color.Pink, Color.White, false);
        Raylib.DrawRectangleRec(buttonRect, buttonColor);
        Raylib.DrawTexture(icon, (int)buttonRect.X, (int)buttonRect.Y, Color.White);
        base.Draw();
    }

    public override void Click(Canvas canvas)
    {
        if (canvas.layers.Count != 1)
        {
            canvas.layers.Remove(canvas.layers[Canvas.currentLayer]);

            if (Canvas.currentLayer != 0) Canvas.currentLayer--;
            else Canvas.currentLayer = 0;
        }
    }
}

public sealed class LayerVisibilityButton : LayerWindowButton
{
    private List<Texture2D> icons = new();
    public int currentIcon = 0;

    public LayerVisibilityButton(ProgramManager programInstance) : base(programInstance)
    {
        icons.Add(Raylib.LoadTexture("Textures/Icons/visible.png"));
        icons.Add(Raylib.LoadTexture("Textures/Icons/invisible.png"));
    }

    public override void Draw()
    {
        GetButtonColor(Color.LightGray, Color.White, Color.White, false);
        Raylib.DrawRectangleRec(buttonRect, buttonColor);
        Raylib.DrawTexture(icons[currentIcon], (int)buttonRect.X, (int)buttonRect.Y, Color.White);
        base.Draw();
    }

    public override void Update(Vector2 mousePos, Canvas canvas)
    {
        currentIcon = canvas.layers[Canvas.currentLayer].isVisible ? 0 : 1;
        base.Update(mousePos, canvas);

        if (isHoveredOn)
            infoWindow = new("Toggle layer visibility", (int)buttonRect.X, (int)buttonRect.Y + buttonSize + 5);
    }

    public override void Click(Canvas canvas)
    {
        canvas.layers[Canvas.currentLayer].isVisible = !canvas.layers[Canvas.currentLayer].isVisible;
    }
}

public sealed class MergeLayersButton : LayerWindowButton
{
    public MergeLayersButton(ProgramManager programInstance) : base(programInstance)
    {
        icon = Raylib.LoadTexture("Textures/Icons/compressicon.png");
    }

    public override void Draw()
    {
        GetButtonColor(Color.LightGray, Color.White, Color.White, false);
        Raylib.DrawRectangleRec(buttonRect, buttonColor);
        Raylib.DrawTexture(icon, (int)buttonRect.X, (int)buttonRect.Y, Color.White);
        base.Draw();
    }

    public override void Update(Vector2 mousePos, Canvas canvas)
    {
        base.Update(mousePos, canvas);

        if (isHoveredOn)
            infoWindow = new("Merge all layers", (int)buttonRect.X, (int)buttonRect.Y + buttonSize + 5);
    }

    public override void Click(Canvas canvas)
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

    public MoveLayerButton(ProgramManager programInstance) : base(programInstance)
    {
        icons = [Raylib.LoadTexture("Textures/Icons/rightarrow.png"), Raylib.LoadTexture("Textures/Icons/leftarrow.png")];
    }

    public override void Update(Vector2 mousePos, Canvas canvas)
    {
        base.Update(mousePos, canvas);

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
        GetButtonColor(Color.LightGray, Color.White, Color.White, false);
        Raylib.DrawRectangleRec(buttonRect, buttonColor);
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

    public override void Click(Canvas canvas)
    {
        switch (direction)
        {
            case Direction.Up:
                if (canvas.layers.Count != 1 && Canvas.currentLayer != canvas.layers.Count - 1)
                {
                    Swap(ref canvas.layers, Canvas.currentLayer, Canvas.currentLayer + 1);
                    Canvas.currentLayer++;
                }
                break;

            case Direction.Down:
                if (Canvas.currentLayer != 0 && canvas.layers.Count != 1)
                {
                    Swap(ref canvas.layers, Canvas.currentLayer, Canvas.currentLayer - 1);
                    Canvas.currentLayer--;
                }
                break;

        }
    }

    private static void Swap<T>(ref List<T> list, int index1, int index2)
    {
        T tmp = list[index1];
        list[index1] = list[index2];
        list[index2] = tmp;
    }
}
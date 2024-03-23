namespace DrawingProgram;

public abstract class LayerWindowButton : Button
{
    public virtual void Update(Vector2 mousePos, Canvas canvas)
    {
        isHoveredOn = false;
        if (Raylib.CheckCollisionPointRec(mousePos, buttonRect))
        {
            isHoveredOn = true;

            if (Raylib.IsMouseButtonPressed(MouseButton.Left))
                Click(canvas);
        }
    }

    public virtual void Click(Canvas canvas) {}
}

public class LayerButton : LayerWindowButton
{
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

public class AddLayerButton : LayerWindowButton
{
    private Texture2D icon;

    public AddLayerButton()
    {
        icon = Raylib.LoadTexture("Icons/plus.png");
    }

    public override void Draw()
    {
        GetButtonColor(Color.Lime, Color.Green, Color.White, false);
        Raylib.DrawRectangleRec(buttonRect, buttonColor);
        Raylib.DrawTexture(icon, (int)buttonRect.X, (int)buttonRect.Y, Color.White);
    }

    public override void Click(Canvas canvas)
    {
        if (canvas.layers.Count < 5)
            canvas.layers.Add(new());

        Canvas.currentLayer = canvas.layers.Count - 1;
    }
}

public class RemoveLayerButton : LayerWindowButton
{
    private Texture2D icon;

    public RemoveLayerButton()
    {
        icon = Raylib.LoadTexture("Icons/x.png");
    }

    public override void Draw()
    {
        GetButtonColor(Color.Red, Color.Pink, Color.White, false);
        Raylib.DrawRectangleRec(buttonRect, buttonColor);
        Raylib.DrawTexture(icon, (int)buttonRect.X, (int)buttonRect.Y, Color.White);
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

public class LayerVisibilityButton : LayerWindowButton
{
    private List<Texture2D> icons = new();
    public int currentIcon = 0;

    public LayerVisibilityButton()
    {
        icons.Add(Raylib.LoadTexture("Icons/visible.png"));
        icons.Add(Raylib.LoadTexture("Icons/invisible.png"));
    }

    public override void Draw()
    {
        GetButtonColor(Color.LightGray, Color.White, Color.White, false);
        Raylib.DrawRectangleRec(buttonRect, buttonColor);
        Raylib.DrawTexture(icons[currentIcon], (int)buttonRect.X, (int)buttonRect.Y, Color.White);
    }

    public override void Update(Vector2 mousePos, Canvas canvas)
    {
        currentIcon = canvas.layers[Canvas.currentLayer].isVisible ? 0 : 1;
        base.Update(mousePos, canvas);
    }

    public override void Click(Canvas canvas)
    {
        canvas.layers[Canvas.currentLayer].isVisible = !canvas.layers[Canvas.currentLayer].isVisible;
    }
}

public class MoveLayerButton : LayerWindowButton
{
    private Texture2D icon;

    public MoveLayerButton()
    {
        icon = Raylib.LoadTexture("Icons/rightarrow.png");
    }

    public override void Draw()
    {
        GetButtonColor(Color.LightGray, Color.White, Color.White, false);
        Raylib.DrawRectangleRec(buttonRect, buttonColor);
        Raylib.DrawTexture(icon, (int)buttonRect.X, (int)buttonRect.Y, Color.White);
    }

    public override void Click(Canvas canvas)
    {
        
    }
}
namespace DrawingProgram;

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
                    infoWindow = new("Move layer up hierarchy", (int)buttonRect.X, (int)buttonRect.Y + ButtonSize + 5);
                    break;
                case Direction.Down:
                    infoWindow = new("Move layer down hierarchy", (int)buttonRect.X, (int)buttonRect.Y + ButtonSize + 5);
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
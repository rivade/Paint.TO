namespace DrawingProgram;

public sealed class AddLayerButton : LayerWindowButton
{
    public AddLayerButton(ProgramManager programInstance, Rectangle buttonRect, Canvas canvasInstance) : base(programInstance, buttonRect, canvasInstance)
    {
        icon = Raylib.LoadTexture("Textures/Icons/plus.png");
        infoWindow = new("Add new layer", (int)buttonRect.X, (int)buttonRect.Y + ButtonSize + 5);
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
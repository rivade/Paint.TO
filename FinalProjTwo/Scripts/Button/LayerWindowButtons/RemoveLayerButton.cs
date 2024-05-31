namespace DrawingProgram;

public sealed class RemoveLayerButton : LayerWindowButton
{
    public RemoveLayerButton(ProgramManager programInstance, Rectangle buttonRect, Canvas canvasInstance) : base(programInstance, buttonRect, canvasInstance)
    {
        icon = Raylib.LoadTexture("Textures/Icons/x.png");
        infoWindow = new("Remove current layer", (int)buttonRect.X, (int)buttonRect.Y + ButtonSize + 5);
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
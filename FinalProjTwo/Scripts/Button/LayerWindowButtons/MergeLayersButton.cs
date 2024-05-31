namespace DrawingProgram;

public sealed class MergeLayersButton : LayerWindowButton
{
    public MergeLayersButton(ProgramManager programInstance, Rectangle buttonRect, Canvas canvasInstance) : base(programInstance, buttonRect, canvasInstance)
    {
        icon = Raylib.LoadTexture("Textures/Icons/compressicon.png");
        infoWindow = new("Merge all layers", (int)buttonRect.X, (int)buttonRect.Y + ButtonSize + 5);
    }

    public override void Draw()
    {
        Raylib.DrawRectangleRec(buttonRect, GetButtonColor(Color.LightGray, Color.White, Color.White, false));
        Raylib.DrawTexture(icon, (int)buttonRect.X, (int)buttonRect.Y, Color.White);
        base.Draw();
    }

    public override void OnClick()
    {
        canvas.CompressLayersInProject();
    }
}
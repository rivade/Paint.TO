namespace DrawingProgram;

public sealed class OpenLayersButton : Button
{
    private LayerWindow layerWindow;

    public OpenLayersButton(ProgramManager programInstance, Rectangle buttonRect, Canvas canvasInstance) : base(programInstance, buttonRect)
    {
        layerWindow = new(programInstance, canvasInstance, 1300, 500, ["Layers:"]);
        infoWindow = new("Layers", (int)buttonRect.X, (int)buttonRect.Y - 40);
    }

    public override void OnClick()
    {
        program.popupWindow = layerWindow;
    }

    public override void Draw()
    {
        Raylib.DrawRectangleRec(buttonRect, GetButtonColor(Color.White, Color.LightGray, Color.White, false));
        base.Draw();
    }
}
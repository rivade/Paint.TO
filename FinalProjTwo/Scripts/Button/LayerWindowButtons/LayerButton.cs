namespace DrawingProgram;

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
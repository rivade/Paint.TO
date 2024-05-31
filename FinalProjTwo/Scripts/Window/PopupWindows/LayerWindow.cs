namespace DrawingProgram;

public sealed class LayerWindow : PopupWindow
{
    private List<LayerButton> layerButtons = new();
    private List<LayerWindowButton> buttons;

    public LayerWindow(ProgramManager programInstance, Canvas canvasInstance, int width, int height, string[] messagesExtern) : base(programInstance, width, height, messagesExtern)
    {
        buttons =
        [
            new AddLayerButton(programInstance, new(670, 650, Button.ButtonSize, Button.ButtonSize), canvasInstance),
            new MoveLayerButton(programInstance, new(770, 650, Button.ButtonSize, Button.ButtonSize), canvasInstance) { direction = MoveLayerButton.Direction.Down },
            new LayerVisibilityButton(programInstance, new(870, 650, Button.ButtonSize, Button.ButtonSize), canvasInstance),
            new MergeLayersButton(programInstance, new(970, 650, Button.ButtonSize, Button.ButtonSize), canvasInstance),
            new MoveLayerButton(programInstance, new(1070, 650, Button.ButtonSize, Button.ButtonSize), canvasInstance) { direction = MoveLayerButton.Direction.Up },
            new RemoveLayerButton(programInstance, new(1170, 650, Button.ButtonSize, Button.ButtonSize), canvasInstance)
        ];
    }

    public override void Logic(Canvas canvas, Vector2 mousePos)
    {
        base.Logic(canvas, mousePos);
        layerButtons = new();

        for (int i = 0; i < canvas.layers.Count; i++)
        {
            layerButtons.Add(new(program, new(i * 250 + 360, 475, 200, 100), canvas) { ThisLayerNumber = i + 1, IsVisible = canvas.layers[i].isVisible });
        }

        layerButtons.ForEach(l => l.OnHover(mousePos));
        buttons.ForEach(b => b.OnHover(mousePos));

    }

    public override void Draw()
    {
        base.Draw();

        layerButtons.ForEach(l => l.Draw());
        buttons.ForEach(b => b.Draw());
    }
}
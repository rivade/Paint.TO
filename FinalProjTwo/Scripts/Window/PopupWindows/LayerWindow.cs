namespace DrawingProgram;

public sealed class LayerWindow : PopupWindow
{
    private List<LayerButton> layerButtons = new();
    private List<LayerWindowButton> buttons;

    public LayerWindow(ProgramManager programInstance, Canvas canvasInstance, int width, int height, string[] messagesExtern) : base(programInstance, width, height, messagesExtern)
    {
        int buttonSpacing = 10;
        int numberOfButtons = 7;
        int totalWidth = numberOfButtons * Button.ButtonSize + (numberOfButtons - 1) * buttonSpacing;
        int startX = (ProgramManager.ScreenWidth - totalWidth) / 2;

        buttons =
        [
            new AddLayerButton(programInstance, new(startX, 650, Button.ButtonSize, Button.ButtonSize), canvasInstance),
            new MoveLayerButton(programInstance, new(startX + (Button.ButtonSize + buttonSpacing) * 1, 650, Button.ButtonSize, Button.ButtonSize), canvasInstance) { direction = MoveLayerButton.Direction.Down },
            new LayerVisibilityButton(programInstance, new(startX + (Button.ButtonSize + buttonSpacing) * 2, 650, Button.ButtonSize, Button.ButtonSize), canvasInstance),
            new MergeLayersButton(programInstance, new(startX + (Button.ButtonSize + buttonSpacing) * 3, 650, Button.ButtonSize, Button.ButtonSize), canvasInstance),
            new ChangeBackgroundButton(programInstance, new(startX + (Button.ButtonSize + buttonSpacing) * 4, 650, Button.ButtonSize, Button.ButtonSize), canvasInstance),
            new MoveLayerButton(programInstance, new(startX + (Button.ButtonSize + buttonSpacing) * 5, 650, Button.ButtonSize, Button.ButtonSize), canvasInstance) { direction = MoveLayerButton.Direction.Up },
            new RemoveLayerButton(programInstance, new(startX + (Button.ButtonSize + buttonSpacing) * 6, 650, Button.ButtonSize, Button.ButtonSize), canvasInstance)
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
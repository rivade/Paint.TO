namespace DrawingProgram;

public sealed class SettingsWindow : PopupWindow
{
    private List<Button> buttons;

    public SettingsWindow(ProgramManager programInstance, int width, int height, string[] messagesExtern) : base(programInstance, width, height, messagesExtern)
    {
        buttons =
        [
            new GUIColorButton(programInstance, new(340, 475, 1230, 100), () => GUIarea.colorInt++, "Change GUI color"),
            new GUIColorButton(programInstance, new(340, 600, 1230, 100), () => ToolButton.colorSetInt++, "Change toolbutton color"),
            new ChangeBackgroundButton(programInstance, new(340, 725, 1230, 100))
        ];
    }

    public override void Logic(Canvas c, Vector2 mousePos)
    {
        base.Logic(c, mousePos);
        buttons.ForEach(b => b.OnHover(mousePos));
    }

    public override void Draw()
    {
        base.Draw();
        buttons.ForEach(b => b.Draw());
    }
}
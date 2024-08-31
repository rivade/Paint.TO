namespace DrawingProgram;

public sealed class SettingsWindow : PopupWindow
{
    private List<Button> buttons;

    public unsafe SettingsWindow(ProgramManager programInstance, int width, int height, string[] messagesExtern) : base(programInstance, width, height, messagesExtern)
    {
        fixed(Color* GUIColorPtr = &GUIarea.GUIColor)
        fixed(Color* ToolButtonColorPtr = &ToolButton.toolButtonColor)
        fixed(Color* BGColorPtr = &program.canvas.backgroundColor)
        buttons =
        [
            new SettingsChangeButton(program, new(340, 450, 1230, 100), "Change GUI color", "Set GUI color", GUIColorPtr),
            new SettingsChangeButton(program, new(340, 565, 1230, 100), "Change button color", "Set button color", ToolButtonColorPtr),
            new SettingsChangeButton(program, new(340, 680, 1230, 100), "Change background Color", "Set BG color", BGColorPtr)
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
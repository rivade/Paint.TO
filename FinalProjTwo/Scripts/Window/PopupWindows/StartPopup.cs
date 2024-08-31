namespace DrawingProgram;

public sealed class StartPopup : PopupWindow
{
    public StartPopup(ProgramManager programInstance, int width, int height, string[] messagesExtern) : base(programInstance, width, height, messagesExtern) { }

    public override void Draw()
    {
        base.Draw();
        TextHandling.DrawScreenCenteredText([$"Paint.TO {VersionControl.CurrentVersion}"], (int)windowRect.Y + 30, 80, 0, Color.Black);
        TextHandling.DrawScreenCenteredText(["Patch notes:", "-GUI and Toolbutton color now fully customisable", "-Fixed auto-update system(2.5)"],
                                            (int)windowRect.Y + 175, 20, 30, Color.Black);
    }
}
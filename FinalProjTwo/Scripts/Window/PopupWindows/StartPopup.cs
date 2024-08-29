namespace DrawingProgram;

public sealed class StartPopup : PopupWindow
{
    public StartPopup(ProgramManager programInstance, int width, int height, string[] messagesExtern) : base(programInstance, width, height, messagesExtern) { }

    public override void Draw()
    {
        base.Draw();
        TextHandling.DrawScreenCenteredText(["Paint.TO v2.317"], (int)windowRect.Y + 30, 80, 0, Color.Black);
        TextHandling.DrawScreenCenteredText(["Patch notes:", "-Added background color changer (2.316)", "-Bug fixes"],
                                            (int)windowRect.Y + 175, 20, 30, Color.Black);
    }
}
using DrawingProgram;

public class UpdatePopup : PopupWindow
{
    public UpdatePopup(ProgramManager programInstance, int width, int height, string[] messagesExtern) : base(programInstance, width, height, messagesExtern)
    {

    }

    public override void Draw()
    {
        base.Draw();
        TextHandling.DrawScreenCenteredText(["Press ENTER to update", "Press ESC to close"], ProgramManager.ScreenHeight/2, 40, 50, Color.Black);
    }
    public override async void Logic(Canvas canvas, Vector2 mousePos)
    {
        base.Logic(canvas, mousePos);

        if (Raylib.IsKeyPressed(KeyboardKey.Enter))
        {
            await VersionControl.UpdateProgram(program);
        }
    }
}
namespace DrawingProgram;

public class PopupWindow : IDrawable
{
    protected Rectangle windowRect;
    public string[] messages;
    protected ClosePopupButton closeButton;
    protected ProgramManager program;

    public virtual void Draw()
    {
        Raylib.DrawRectangleRec(windowRect, GUIarea.GUIColor);
        TextHandling.DrawScreenCenteredText(messages, (int)windowRect.Y + 20, 60, 70, Color.Black);
        closeButton.Draw();
    }

    public virtual void Logic(Canvas canvas, Vector2 mousePos)
    {
        closeButton.OnHover(mousePos);
        if (!Raylib.CheckCollisionPointRec(mousePos, windowRect) && Raylib.IsMouseButtonPressed(MouseButton.Left))
            program.popupWindow = null;
    }

    public PopupWindow(ProgramManager programInstance, int width, int height, string[] messagesExtern)
    {
        program = programInstance;
        windowRect = new Rectangle(ProgramManager.ScreenWidth / 2 - width / 2, ProgramManager.ScreenHeight / 2 - height / 2, width, height);
        messages = messagesExtern;
        closeButton = new(program, windowRect);
    }
}
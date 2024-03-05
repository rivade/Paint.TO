namespace DrawingProgram;

public abstract class PopupWindow : IDrawable // Gör detta till abstract class med arv istället
{
    protected Rectangle windowRect;
    public string[] messages;

    public virtual void Draw()
    {
        Raylib.DrawRectangleRec(windowRect, Color.DarkGray);
        TextHandling.DrawCenteredText(messages, new Vector2(windowRect.X + 10, windowRect.Y + 10), 30);
    }

    public virtual void Logic(Canvas canvas)
    {

    }

    public PopupWindow(int width, int height, string[] messagesExtern, Canvas canvas)
    {
        windowRect = new Rectangle(ProgramManager.ScreenWidth / 2 - width / 2, ProgramManager.ScreenHeight / 2 - height / 2, width, height);
        messages = messagesExtern;
    }
}

public class SavePopup : PopupWindow
{
    public Dictionary<KeyboardKey, string> alphabet;
    public string text = "";

    public SavePopup(int width, int height, string[] messagesExtern, Canvas canvas) : base(width, height, messagesExtern, canvas)
    {
        alphabet = new();
        for (int i = (int)KeyboardKey.A; i <= (int)KeyboardKey.Z; i++)
        {
            alphabet.Add((KeyboardKey)i, ((char)(i + 32)).ToString());
        }
    }

    public void SaveCanvas(Canvas canvas)
    {
        if (Raylib.IsKeyPressed(KeyboardKey.Enter) && text != "")
        {
            canvas.SaveProject(text);
        }
    }

    public string GetTextInput()
    {
        KeyboardKey keyPressed = (KeyboardKey)Raylib.GetKeyPressed();
        if (keyPressed != KeyboardKey.Null && alphabet.ContainsKey(keyPressed))
        {
            text += alphabet[keyPressed];
        }

        return text;
    }

    public override void Draw()
    {
        base.Draw();
        Raylib.DrawText(text, (int)windowRect.X + 10, (int)(windowRect.Y + windowRect.Height) - 20, 30, Color.Black);
    }

    public override void Logic(Canvas canvas)
    {
        text = GetTextInput();
        SaveCanvas(canvas);
    }
}
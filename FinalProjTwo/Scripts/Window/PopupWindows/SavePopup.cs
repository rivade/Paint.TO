namespace DrawingProgram;

public sealed class SavePopup : PopupWindow
{
    public Dictionary<KeyboardKey, char> alphabet;
    public string fileName = "";

    public SavePopup(ProgramManager programInstance, int width, int height, string[] messagesExtern) : base(programInstance, width, height, messagesExtern)
    {
        alphabet = new();
        for (int i = (int)KeyboardKey.A; i <= (int)KeyboardKey.Z; i++)
        {
            alphabet.Add((KeyboardKey)i, (char)(i + 32));
        }

        for (int i = (int)KeyboardKey.Zero; i <= (int)KeyboardKey.Nine; i++)
        {
            alphabet.Add((KeyboardKey)i, (char)i);
        }
    }

    public void SaveCanvas(Canvas canvas)
    {
        if (Raylib.IsKeyPressed(KeyboardKey.Enter) && fileName != "")
        {
            Raylib.ToggleFullscreen();
            string directory = OpenDialog.GetDirectory();
            canvas.SaveProject(fileName + ".png", directory);
            Raylib.ToggleFullscreen();
        }
    }

    public string UpdateFileName()
    {
        KeyboardKey keyPressed = (KeyboardKey)Raylib.GetKeyPressed();
        if (keyPressed != KeyboardKey.Null && alphabet.ContainsKey(keyPressed) && Raylib.MeasureText(fileName, 30) < windowRect.Width - 50)
        {
            if (Raylib.IsKeyDown(KeyboardKey.LeftShift) || Raylib.IsKeyDown(KeyboardKey.RightShift))
                fileName += alphabet[keyPressed].ToString().ToUpper();

            else
                fileName += alphabet[keyPressed];
        }

        if (keyPressed == KeyboardKey.Backspace && fileName.Length != 0)
            fileName = fileName[..^1]; //Tar bort sista bokstaven/siffran i stringen

        return fileName;
    }

    public override void Draw()
    {
        base.Draw();
        Raylib.DrawRectangle((int)windowRect.X + 10, (int)(windowRect.Y + windowRect.Height) - 80, (int)windowRect.Width - 20, 50, Color.LightGray);
        Raylib.DrawText(fileName, (int)windowRect.X + 20, (int)(windowRect.Y + windowRect.Height) - 70, 30, Color.Black);
    }

    public override void Logic(Canvas canvas, Vector2 mousePos)
    {
        base.Logic(canvas, mousePos);
        fileName = UpdateFileName();
        SaveCanvas(canvas);
    }
}
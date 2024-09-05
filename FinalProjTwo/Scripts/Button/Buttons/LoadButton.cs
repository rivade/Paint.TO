namespace DrawingProgram;

public sealed class LoadButton : Button
{
    private Canvas canvas;

    public LoadButton(ProgramManager programInstance, Rectangle buttonRect, Canvas canvasInstance) : base(programInstance, buttonRect)
    {
        canvas = canvasInstance;
        infoWindow = new("Open image", (int)buttonRect.X - Raylib.MeasureText("Open image", InfoText.FontSize) - 20, (int)buttonRect.Y);
        icon = Raylib.LoadTexture("Textures/Icons/foldericon.png");
    }

    public override void OnClick()
    {
        if (Raylib.IsWindowFullscreen())
            Raylib.ToggleFullscreen();

        string fileDirectory = OpenDialog.GetFile();

        if (!string.IsNullOrEmpty(fileDirectory))
        {
            Image loadedImage = Raylib.LoadImage(fileDirectory);
            canvas.LoadProject(loadedImage);
        }

        Raylib.ToggleFullscreen();
    }

    public override void Draw()
    {
        Raylib.DrawRectangleRec(buttonRect, GetButtonColor(Color.Orange, Color.Yellow, Color.White, false));
        base.Draw();
    }
}
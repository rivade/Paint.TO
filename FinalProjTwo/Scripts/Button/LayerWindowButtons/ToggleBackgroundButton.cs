using DrawingProgram;

public sealed class ToggleBackgroundButton : LayerWindowButton
{
    private List<Texture2D> icons = new();
    private bool isBackgroundEnabled = true;

    public ToggleBackgroundButton(ProgramManager programInstance, Rectangle buttonRect, Canvas canvasInstance) : base(programInstance, buttonRect, canvasInstance)
    {
        icons.Add(Raylib.LoadTexture("Textures/Icons/yesbackground.png"));
        icons.Add(Raylib.LoadTexture("Textures/Icons/nobackground.png"));
        infoWindow = new("Toggle canvas background", (int)buttonRect.X, (int)buttonRect.Y + ButtonSize + 5);
    }

    public override void OnClick()
    {
        isBackgroundEnabled = !isBackgroundEnabled;

        if (isBackgroundEnabled) canvas.backgroundColor = Color.White;
        else canvas.backgroundColor = Color.Blank;
        canvas.UpdateBackgroundColor();
    }

    public override void Draw()
    {
        Raylib.DrawRectangleRec(buttonRect, GetButtonColor(Color.LightGray, Color.White, Color.White, false));
        Raylib.DrawTexture(icons[isBackgroundEnabled ? 0 : 1], (int)buttonRect.X, (int)buttonRect.Y, Color.White);
        base.Draw();
    }
}
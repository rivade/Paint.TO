namespace DrawingProgram;

public sealed class ClosePopupButton : Button
{
    private const int buttonWidth = 50;
    private const int buttonHeight = 30;
    private Texture2D icon = Raylib.LoadTexture("Textures/Icons/x2.png");

    public ClosePopupButton(ProgramManager programInstance, Rectangle popupRect) : base(programInstance, new())
    {
        buttonRect = new((popupRect.X + popupRect.Width) - buttonWidth, popupRect.Y, buttonWidth, buttonHeight);
        infoWindow = new("Close window", (int)buttonRect.X - Raylib.MeasureText("Close window", InfoText.FontSize) - 20, (int)buttonRect.Y);
    }

    public override void OnClick()
    {
        program.isMouseInputEnabled = false;
        program.popupWindow = null;
    }

    public override void Draw()
    {
        Raylib.DrawRectangleRec(buttonRect, GetButtonColor(Color.Red, Color.Pink, Color.White, false));
        Raylib.DrawTexture(icon, (int)buttonRect.X, (int)buttonRect.Y, Color.White);
        base.Draw();
    }
}
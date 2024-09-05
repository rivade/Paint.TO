namespace DrawingProgram;

public sealed class SaveCanvasButton : Button
{
    public SaveCanvasButton(ProgramManager programInstance, Rectangle buttonRect) : base(programInstance, buttonRect)
    {
        infoWindow = new("Save image", (int)buttonRect.X - Raylib.MeasureText("Save image", InfoText.FontSize) - 20, (int)buttonRect.Y);
        icon = Raylib.LoadTexture("Textures/Icons/saveicon.png");
    }

    public override void OnClick()
    {
        program.popupWindow = new SavePopup(program, 800, 300, ["Select file name", "Press enter to save"]);
    }

    public override void Draw()
    {
        Raylib.DrawRectangleRec(buttonRect, GetButtonColor(Color.Orange, Color.Yellow, Color.White, false));
        base.Draw();
    }
}
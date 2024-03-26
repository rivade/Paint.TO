namespace DrawingProgram;

public class InfoWindow : IDrawable
{
    private Rectangle textbox;
    private string text;
    public const int FontSize = 20;

    public InfoWindow(string infoText, int startX, int startY)
    {
        text = infoText;
        int boxWidth = Raylib.MeasureText(infoText, FontSize) + 10;
        int boxHeight = FontSize + 10;

        textbox = new(startX, startY, boxWidth, boxHeight);
    }

    public void Draw()
    {
        Raylib.DrawRectangleRec(textbox, Color.Black);
        Raylib.DrawText(text, (int)textbox.X + 5, (int)textbox.Y + 5, FontSize, Color.White);
    }
}
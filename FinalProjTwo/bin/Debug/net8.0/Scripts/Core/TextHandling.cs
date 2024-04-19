namespace DrawingProgram;

public static class TextHandling
{
    public static void DrawScreenCenteredText(string[] texts, int startY, int fontSize, int lineHeight, Color textColor)
    {
        for (int i = 0; i < texts.Length; i++)
        {
            int textWidth = Raylib.MeasureText(texts[i], fontSize);

            int xOffset = (ProgramManager.ScreenWidth - textWidth) / 2;
            Raylib.DrawText(texts[i], xOffset, startY + i*lineHeight, fontSize, textColor);
        }
    }

    public static void DrawCenteredTextPro(string[] texts, int fromX, int toX, int startY, int fontSize, int lineHeight, Color textColor)
    {
        for (int i = 0; i < texts.Length; i++)
        {
            int textWidth = Raylib.MeasureText(texts[i], fontSize);
            int textBoxWidth = toX - fromX;

            int xOffset = (textBoxWidth - textWidth) / 2;
            xOffset += fromX;
            Raylib.DrawText(texts[i], xOffset, startY + i*lineHeight, fontSize, textColor);
        }
    }
}
namespace DrawingProgram;

public static class TextHandling
{
    public static void DrawCenteredText(string[] texts, int startY, int fontSize, int lineDistance, Color textColor)
    {
        for (int i = 0; i < texts.Length; i++)
        {
            int textWidth = Raylib.MeasureText(texts[i], fontSize);

            int xOffset = (ProgramManager.ScreenWidth - textWidth) / 2;
            Raylib.DrawText(texts[i], xOffset, startY + i*lineDistance, fontSize, textColor);
        }
    }
}
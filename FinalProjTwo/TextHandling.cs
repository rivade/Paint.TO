namespace DrawingProgram;

public static class TextHandling
{
    public static void DrawCenteredText(string[] texts, Vector2 startPos, int fontSize)
    {
        for (int i = 0; i < texts.Length; i++)
        {
            int textWidth = Raylib.MeasureText(texts[i], fontSize);

            int xOffset = (ProgramManager.ScreenWidth - textWidth) / 2;
            Raylib.DrawText(texts[i], xOffset, (int)startPos.Y + i*50, fontSize, Color.Black);
        }
    }
}
namespace DrawingProgram;

public class ProgramManager
{
    public const int screenWidth = 1920 - 100;
    public const int screenHeight = 1080 - 100;

    public enum State
    {
        Menu,
        Drawing
    }
    private State _currentstate;

    private Image canvas;
    private List<ToolFolder> toolFolders = [new Drawing(), new Erasing(), new Favorites()];
    private List<IClickable> clickables;
    private List<Button> buttons;
    public static DrawTool currentTool;


    public ProgramManager()
    {
        Raylib.InitWindow(screenWidth, screenHeight, "GenericDrawingProgram");
        Raylib.SetTargetFPS(360);
        Raylib.ToggleFullscreen();
        _currentstate = State.Drawing;
        canvas = Raylib.GenImageColor(screenWidth, screenHeight, Color.White);

        buttons = ButtonGenerator.GenerateButtons(toolFolders);
        //buttons.ForEach(b => clickables.Add((IClickable)b));

        currentTool = toolFolders[0].drawTools[2];
    }

    private void DrawGraphics()
    {
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Color.Gray);
        Texture2D canvasTexture = Raylib.LoadTextureFromImage(canvas);
        Raylib.DrawTexture(canvasTexture, 0, 0, Color.White);
        buttons.ForEach(b => Raylib.DrawRectangleRec(b.buttonRect, Color.Red));
        Raylib.EndDrawing();
        Raylib.UnloadTexture(canvasTexture);
    }

    private void Logic()
    {
        DrawTool.SavePrevCanvas(canvas);
        currentTool.Draw(Color.Black, canvas, 10);
        canvas = DrawTool.UndoStroke(canvas);

        clickables.ForEach(c => c.OnClick());

        switch (Raylib.GetKeyPressed())
        {
            case (int)KeyboardKey.One:
                currentTool = toolFolders[0].drawTools[0];
            break;

            case (int)KeyboardKey.Two:
                currentTool = toolFolders[0].drawTools[2];
            break;
        }
    }

    public void Run()
    {
        while (!Raylib.WindowShouldClose())
        {
            switch (_currentstate)
            {
                case State.Menu:
                    break;

                case State.Drawing:
                    Logic();
                    DrawGraphics();
                    System.Console.WriteLine(DrawTool.strokes.Count());
                    break;
            }
        }
    }
}
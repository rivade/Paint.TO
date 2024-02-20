namespace DrawingProgram;

public class ProgramManager
{
    public const int screenWidth = 1500;
    public const int screenHeight = 900;

    public enum State
    {
        Menu,
        Drawing
    }
    private State _currentstate;

    private Image canvas;
    private List<ToolFolder> toolFolders = [new Drawing(), new Erasing(), new Favorites()];
    DrawTool currentTool;


    public ProgramManager()
    {
        Raylib.InitWindow(screenWidth, screenHeight, "GenericDrawingProgram");
        _currentstate = State.Drawing;
        canvas = Raylib.GenImageColor(screenWidth, screenHeight, Color.White);

        currentTool = toolFolders[0].drawTools[2];
    }

    private void DrawGraphics()
    {
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Color.White);
        Texture2D canvasTexture = Raylib.LoadTextureFromImage(canvas);
        Raylib.DrawTexture(canvasTexture, 0, 0, Color.White);
        Raylib.EndDrawing();
        Raylib.UnloadTexture(canvasTexture);
    }

    private void Logic()
    {
        DrawTool.SavePrevCanvas(canvas);
        currentTool.Draw(Color.Black, canvas, 10);
        canvas = DrawTool.UndoStroke(canvas);
        //clickables.ForEach(c => c.OnClick());

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
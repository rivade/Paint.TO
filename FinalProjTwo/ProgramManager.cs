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

    //interface listor
    public List<IMouseInteractable> interactables;
    public List<IDrawable> drawables;

    public static DrawTool currentTool;


    public ProgramManager()
    {
        Raylib.InitWindow(screenWidth, screenHeight, "GenericDrawingProgram");
        Raylib.ToggleFullscreen();
        _currentstate = State.Drawing;
        canvas = Raylib.GenImageColor(screenWidth, screenHeight, Color.White);

        interactables = InterListInit.GenerateInteractables(toolFolders);
        drawables = InterListInit.GenerateDrawables(toolFolders);

        currentTool = toolFolders[0].drawTools[2];
    }

    private void DrawGraphics()
    {
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Color.Gray);
        Texture2D canvasTexture = Raylib.LoadTextureFromImage(canvas);
        Raylib.DrawTexture(canvasTexture, 0, 0, Color.White);
        
        drawables.ForEach(b => b.Draw());

        Raylib.EndDrawing();
        Raylib.UnloadTexture(canvasTexture);
    }

    private void Logic()
    {
        Vector2 mousePos = Raylib.GetMousePosition();

        DrawTool.SavePrevCanvas(canvas);
        currentTool.Draw(Color.Black, canvas, 10);
        canvas = DrawTool.UndoStroke(canvas);

        interactables.ForEach(c => c.OnHover(mousePos));

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
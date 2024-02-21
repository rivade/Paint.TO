namespace DrawingProgram;

public class ProgramManager
{
    public const int CanvasWidth = 1920 - 100;
    public const int CanvasHeight = 1080 - 100;

    public enum State
    {
        Menu,
        Drawing
    }
    private State _currentstate;

    private Image canvas;
    private Icons icons;
    private List<ToolFolder> toolFolders = [new Drawing(), new Erasing(), new Favorites()];

    //interface listor
    public List<IMouseInteractable> interactables;
    public List<IDrawable> drawables;

    public static DrawTool currentTool;
    public static Color currentColor;


    public ProgramManager()
    {
        Raylib.InitWindow(CanvasWidth, CanvasHeight, "GenericDrawingProgram");
        Raylib.ToggleFullscreen();
        _currentstate = State.Drawing;
        canvas = Raylib.GenImageColor(CanvasWidth, CanvasHeight, Color.White);
        icons = new();

        interactables = InterListInit.GenerateInteractables(toolFolders);
        drawables = [.. interactables.Where(i => i is IDrawable).Cast<IDrawable>()];


        currentTool = toolFolders[0].drawTools[0];
    }

    private void DrawGraphics()
    {
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Color.Gray);
        Texture2D canvasTexture = Raylib.LoadTextureFromImage(canvas);
        Raylib.DrawTexture(canvasTexture, 0, 0, Color.White);

        drawables.ForEach(b => b.Draw());
        icons.DrawIcons(true);

        Raylib.EndDrawing();
        Raylib.UnloadTexture(canvasTexture);
    }

    private void Logic()
    {
        Vector2 mousePos = Raylib.GetMousePosition();

        if (Canvas.IsCursorOnCanvas(mousePos))
        {
            DrawTool.SavePrevCanvas(canvas);
            currentTool.Draw(Color.Black, canvas, 10, mousePos);
        }
        
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
                    break;
            }
        }
    }
}
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

    private Canvas canvas;
    private Icons icons;
    private List<ToolFolder> toolFolders = [new Drawing(), new Favorites()];

    //interface listor
    public List<IHoverable> interactables;
    public List<IDrawable> drawables;

    public static DrawTool currentTool;
    public static Color currentColor;


    public ProgramManager()
    {
        Raylib.InitWindow(CanvasWidth, CanvasHeight, "GenericDrawingProgram");
        Raylib.SetTargetFPS(1000);
        Raylib.ToggleFullscreen();
        _currentstate = State.Drawing;
        canvas = new();
        icons = new();

        interactables = InterListInit.GenerateInteractables(toolFolders);
        drawables = [.. interactables.Where(i => i is IDrawable).Cast<IDrawable>()];
        drawables.Add(icons);
        drawables.Add(canvas);


        currentTool = toolFolders[0].drawTools[0];
    }

    private void DrawGraphics()
    {
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Color.Gray);

        drawables.ForEach(d => d.Draw());
        Raylib.DrawText("Color", CanvasWidth + 12, CanvasHeight - 20, 30, Color.Black);

        Raylib.EndDrawing();
        Raylib.UnloadTexture(canvas.canvasTexture);

    }

    private void Logic()
    {
        Vector2 mousePos = Raylib.GetMousePosition();

        canvas.Update(mousePos, currentTool);

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
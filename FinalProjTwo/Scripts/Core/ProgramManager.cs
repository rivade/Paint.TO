namespace DrawingProgram;
using System.Threading.Tasks;

public class ProgramManager
{
    public static readonly int ScreenWidth = Raylib.GetMonitorWidth(Raylib.GetCurrentMonitor());
    public static readonly int ScreenHeight = Raylib.GetMonitorHeight(Raylib.GetCurrentMonitor());

    public Canvas canvas;
    private ToolFolder tools = new ToolFolder();

    private List<IMouseInteractable> interactables;
    private List<IDrawable> drawables;

    public ITool currentTool;
    public PopupWindow popupWindow;
    public bool isMouseInputEnabled;

    public ProgramManager()
    {
        Raylib.InitWindow(1920, 1080, "Paint.TO");
        Raylib.ToggleFullscreen();
        Raylib.SetExitKey(KeyboardKey.Null);
        canvas = new(this);
        interactables = ButtonCreator.GenerateButtons(this, tools, canvas);
        drawables = [canvas, new ShapeAndSelectionToolPreviews(this, (RectangleSelect)tools.toolList.Find(t => t is RectangleSelect)), new GUIarea()];
        drawables.AddRange(interactables.Where(i => i is IDrawable).Cast<IDrawable>());
        drawables.Add(new Icons());

        popupWindow = new StartPopup(this, 800, 300, []);
        currentTool = tools.toolList[0];

        CheckUpdate();
    }

    private async void CheckUpdate()
    {
        if(!await VersionControl.IsLatestVersion(this))
        popupWindow = new UpdatePopup(this, 900, 300, ["A new version is avalible!"]);
    }

    private void DrawGraphics()
    {
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Color.White);

        drawables.ForEach(d => d.Draw());
        popupWindow?.Draw();

        Raylib.EndDrawing();
    }

    private void Logic()
    {
        Vector2 mousePos = Raylib.GetMousePosition();

        if (Raylib.IsMouseButtonPressed(MouseButton.Left)) isMouseInputEnabled = true;

        popupWindow?.Logic(canvas, mousePos);
        if (popupWindow == null && isMouseInputEnabled)
            canvas.Update(mousePos, currentTool);

        interactables.ForEach(i => i.OnHover(mousePos));

        if (Raylib.IsKeyPressed(KeyboardKey.Enter) || Raylib.IsKeyPressed(KeyboardKey.Escape))
            popupWindow = null;

        DrawTool.UpdateLastMousePos(mousePos);
    }

    public void Run()
    {
        while (!Raylib.WindowShouldClose())
        {
            Logic();
            DrawGraphics();
        }
    }
}
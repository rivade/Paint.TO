namespace DrawingProgram;
using System.Threading.Tasks;

public class ProgramManager
{
    public static readonly int ScreenWidth = Raylib.GetMonitorWidth(Raylib.GetCurrentMonitor());
    public static readonly int ScreenHeight = Raylib.GetMonitorHeight(Raylib.GetCurrentMonitor());

    public Canvas canvas;
    private ToolFolder tools = new ToolFolder();
    private UserPrefs userPrefs;
    private PerspectiveCamera camera;

    private List<IMouseInteractable> interactables;
    private List<IDrawable> drawables;
    private List<IDrawable> guiAssets;

    public ITool currentTool;
    public PopupWindow popupWindow;
    public bool isMouseInputEnabled;

    public ProgramManager()
    {
        Raylib.InitWindow(1920, 1080, "Paint.TO");
        Raylib.ToggleFullscreen();
        Raylib.SetExitKey(KeyboardKey.Null);
        Image icon = Raylib.LoadImage("Textures/Icons/paintbrush.png");
        Raylib.SetWindowIcon(icon);
        Raylib.UnloadImage(icon);

        camera = new();
        canvas = new(this, camera);
        userPrefs = new();
        interactables = ButtonCreator.GenerateButtons(this, tools, canvas, userPrefs);
        drawables = [canvas, new ToolPreviews(this, (RectangleSelect)tools.toolList.Find(t => t is RectangleSelect), camera)];
        guiAssets = [new GUIarea()];
        guiAssets.AddRange(interactables.Where(i => i is IDrawable).Cast<IDrawable>());

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

        Raylib.BeginMode2D(camera.c);
        drawables.ForEach(d => d.Draw());
        Raylib.EndMode2D();

        guiAssets.ForEach(g => g.Draw());
        popupWindow?.Draw();

        Raylib.EndDrawing();
    }

    private void Logic()
    {
        Vector2 mousePos = Raylib.GetMousePosition();

        camera.Logic(mousePos);

        if (Raylib.IsMouseButtonPressed(MouseButton.Left)) isMouseInputEnabled = true;

        popupWindow?.Logic(canvas, mousePos);
        if (popupWindow == null && isMouseInputEnabled)
            canvas.Update(camera.projectCameraPointToCanvas(mousePos), currentTool);

        interactables.ForEach(i => i.OnHover(mousePos));

        if (Raylib.IsKeyPressed(KeyboardKey.Enter) || Raylib.IsKeyPressed(KeyboardKey.Escape))
            popupWindow = null;

        DrawTool.UpdateLastMousePos(camera.projectCameraPointToCanvas(mousePos));
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
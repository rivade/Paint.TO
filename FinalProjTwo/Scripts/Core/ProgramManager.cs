using System.Runtime.Serialization;

namespace DrawingProgram;

public class ProgramManager
{
    public const int ScreenWidth = 1920;
    public const int ScreenHeight = 1080;

    private Canvas canvas;
    private ToolFolder tools = new DrawingTools();

    private List<IHoverable> interactables;
    private List<IDrawable> drawables;

    public DrawTool currentTool;
    public PopupWindow popupWindow;
    public bool isMouseInputEnabled;

    private Vector2 lastMousePos;

    public ProgramManager()
    {
        Raylib.InitWindow(1920, 1080, "Paint.TO");
        Raylib.ToggleFullscreen();
        Raylib.SetExitKey(KeyboardKey.Null);
        canvas = new(this);

        interactables = InterListInit.GenerateInteractables(this, tools, canvas);
        drawables = [canvas, new ShapeIndicators(), new GUIarea()];
        drawables.AddRange(interactables.Where(i => i is IDrawable).Cast<IDrawable>());
        drawables.Add(new Icons());

        popupWindow = new StartPopup(this, 800, 300, []);
        currentTool = tools.drawTools[0];
    }

    private void DrawGraphics()
    {
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Color.White);

        drawables.ForEach(d => d.Draw());
        popupWindow?.Draw();

        Raylib.EndDrawing();

        canvas.layers.ForEach(layer => Raylib.UnloadTexture(layer.canvasTexture));
    }

    private void Logic()
    {
        Vector2 mousePos = Raylib.GetMousePosition();

        if (Raylib.IsMouseButtonPressed(MouseButton.Left)) isMouseInputEnabled = true;

        popupWindow?.Logic(canvas, mousePos);
        if (popupWindow == null && isMouseInputEnabled) 
            canvas.Update(mousePos, lastMousePos, currentTool);

        interactables.ForEach(i => i.OnHover(mousePos));

        if (Raylib.IsKeyPressed(KeyboardKey.Enter) || Raylib.IsKeyPressed(KeyboardKey.Escape))
            popupWindow = null;

        lastMousePos = mousePos;
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
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

    public static DrawTool currentTool;
    public static PopupWindow popupWindow;

    public ProgramManager()
    {
        Raylib.InitWindow(1920, 1080, "GenericDrawingProgram");
        Raylib.ToggleFullscreen();
        Raylib.SetExitKey(KeyboardKey.Null);
        canvas = new();

        interactables = InterListInit.GenerateInteractables(tools, canvas);
        drawables = [canvas, new ShapeIndicators(), new GUIarea()];
        drawables.AddRange(interactables.Where(i => i is IDrawable).Cast<IDrawable>());
        drawables.Add(new Icons());

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

        popupWindow?.Logic(canvas, mousePos);
        if (popupWindow == null) canvas.Update(mousePos, currentTool);

        interactables.ForEach(i => i.OnHover(mousePos));

        if (Raylib.IsKeyPressed(KeyboardKey.Enter) || Raylib.IsKeyPressed(KeyboardKey.Escape))
        {
            popupWindow = null;
        }
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
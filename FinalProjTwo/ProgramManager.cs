using System.Runtime.Serialization;

namespace DrawingProgram;

public class ProgramManager
{
    public const int ScreenWidth = 1920;
    public const int ScreenHeight = 1080;

    private Canvas canvas;
    private Icons icons;
    public static ToolFolder tools = new Drawing();

    //interface listor
    private List<IHoverable> interactables;
    private List<IDrawable> drawables;


    public static DrawTool currentTool;
    public static Color currentColor;
    public static PopupWindow popupWindow;

    public SaveCanvasButton saveCanvasButton = new()
    {
        buttonRect =
        new Rectangle(
        Canvas.CanvasWidth + 60,
        100,
        SaveCanvasButton.buttonSize,
        SaveCanvasButton.buttonSize)
    };


    public ProgramManager()
    {
        Raylib.InitWindow(1920, 1080, "GenericDrawingProgram");
        Raylib.ToggleBorderlessWindowed();
        Raylib.SetExitKey(KeyboardKey.Null);
        canvas = new();
        icons = new();

        interactables = InterListInit.GenerateInteractables(tools);
        interactables.Add(saveCanvasButton);
        drawables = [canvas];
        drawables.AddRange(interactables.Where(i => i is IDrawable).Cast<IDrawable>());
        drawables.Add(icons);


        currentTool = tools.drawTools[0];
    }

    private void DrawGraphics()
    {
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Color.White);
        drawables.ForEach(d => d.Draw());
        Raylib.DrawRectangleRec(RectangleTool.tempRectangle, DrawTool.drawingColor);
        LineTool.DrawThickLine(canvas.canvasImg, LineTool.tempLine.startPos, LineTool.tempLine.endPos, DrawTool.drawingColor, false);
        Raylib.DrawCircle((int)CircleTool.tempCircle.Middle.X, (int)CircleTool.tempCircle.Middle.Y, CircleTool.tempCircle.Radius, DrawTool.drawingColor);

        if (popupWindow != null)
            popupWindow.Draw();
        
        Raylib.EndDrawing();
        Raylib.UnloadTexture(canvas.canvasTexture);

    }

    private void Logic()
    {
        Vector2 mousePos = Raylib.GetMousePosition();

        if (popupWindow == null)
            canvas.Update(mousePos, currentTool);
        else
            popupWindow.Logic(canvas, mousePos);

        interactables.ForEach(c => c.OnHover(mousePos));

        if (saveCanvasButton.CreatePopup(mousePos, canvas) != null)
        {
            popupWindow = saveCanvasButton.CreatePopup(mousePos, canvas);
        }

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
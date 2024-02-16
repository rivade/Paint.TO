namespace MyGame;

public class ProgramManager
{
    public const int screenWidth = 1500;
    public const int screenHeight = 1000;

    public enum State
    {
        Menu,
        Drawing
    }
    private State _currentstate;

    public static Image canvas;
    Pencil pencil = new();

    public ProgramManager()
    {
        Raylib.InitWindow(screenWidth, screenHeight, "GenericDrawingProgram");
        _currentstate = State.Drawing;
        canvas = Raylib.GenImageColor(screenWidth, screenHeight, Color.White);
    }

    private void DrawGraphics()
    {
        Raylib.BeginDrawing();
        Raylib.ClearBackground(Color.White);
        Raylib.DrawTexture(Raylib.LoadTextureFromImage(canvas), 0, 0, Color.White);
        Raylib.EndDrawing();
    }

    private void Logic()
    {
        pencil.Draw(Color.Black);
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
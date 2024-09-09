using DrawingProgram;

// Inherits from layerwindowbutton due to the fact that it contains just the canvas instance
public class UndoButton : LayerWindowButton
{
    public UndoButton(ProgramManager programInstance, Rectangle buttonRect, Canvas canvas) : base(programInstance, buttonRect, canvas)
    {
        infoWindow = new("Undo", (int)buttonRect.X, (int)buttonRect.Y - 40);
        icon = Raylib.LoadTexture("Textures/Icons/undo.png");
    }
    
    public override void OnClick()
    {
        canvas.layers[canvas.currentLayer].UndoStroke();
    }

    public override void Draw()
    {
        Raylib.DrawRectangleRec(buttonRect, GetButtonColor(Color.LightGray, Color.White, Color.White, false));
        base.Draw();
    }
}

public class RedoButton : LayerWindowButton
{
    public RedoButton(ProgramManager programInstance, Rectangle buttonRect, Canvas canvas) : base(programInstance, buttonRect, canvas)
    {
        infoWindow = new("Redo", (int)buttonRect.X, (int)buttonRect.Y - 40);
        icon = Raylib.LoadTexture("Textures/Icons/redo.png");
    }
    
    public override void OnClick()
    {
        canvas.layers[canvas.currentLayer].RedoStroke();
    }

    public override void Draw()
    {
        Raylib.DrawRectangleRec(buttonRect, GetButtonColor(Color.LightGray, Color.White, Color.White, false));
        base.Draw();
    }
}
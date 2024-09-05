namespace DrawingProgram;

public abstract class LayerWindowButton : Button
{
    protected Canvas canvas;

    public LayerWindowButton(ProgramManager programInstance, Rectangle buttonRect, Canvas canvasInstance) : base(programInstance, buttonRect)
    {
        canvas = canvasInstance;
    }
}
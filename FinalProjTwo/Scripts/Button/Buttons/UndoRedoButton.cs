using DrawingProgram;

public class UndoRedoButton : Button
{
    private Action UndoOrRedo;

    public UndoRedoButton(ProgramManager programInstance, Rectangle buttonRect, Action action) : base(programInstance, buttonRect)
    {
        UndoOrRedo = action;
    }
    
    public override void OnClick()
    {
        UndoOrRedo.Invoke();
    }
}
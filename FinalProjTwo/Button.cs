namespace DrawingProgram;

public abstract class Button
{
    public Rectangle buttonRect {get; set;}
}

public class ToolButton : Button, IClickable
{
    public DrawTool DrawTool { get; set; }

    public void OnClick()
    {
        ProgramManager.currentTool = (DrawTool != ProgramManager.currentTool) ? DrawTool : ProgramManager.currentTool;
    }

}
namespace DrawingProgram;

public class Button
{
    public Rectangle buttonRect {get; set;}
}

public class ToolButton : Button, IClickable
{
    public DrawTool drawTool;

    public DrawTool ChangeTool(DrawTool prevTool)
    {
        return drawTool != prevTool ? drawTool : prevTool;
    }

    public void OnClick()
    {
        
    }
}
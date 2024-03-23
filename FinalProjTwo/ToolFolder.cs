namespace DrawingProgram;

public abstract class ToolFolder
{
    public List<DrawTool> drawTools = new();  
}

public class DrawingTools : ToolFolder
{
    public DrawingTools()
    {
        drawTools.Add(new Pencil());
        drawTools.Add(new PaintBrush());
        drawTools.Add(new Eraser());
        drawTools.Add(new Bucket());
        drawTools.Add(new EyeDropper());
        drawTools.Add(new Checker());
        drawTools.Add(new RectangleTool());
        drawTools.Add(new LineTool());
        drawTools.Add(new CircleTool());
    }
}
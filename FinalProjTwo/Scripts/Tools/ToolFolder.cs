namespace DrawingProgram;

public struct ToolFolder
{
    public List<ITool> toolList = new();

    public ToolFolder()
    {
        toolList.Add(new Pencil());
        toolList.Add(new PaintBrush());
        toolList.Add(new Eraser());
        toolList.Add(new Bucket());
        toolList.Add(new EyeDropper());
        toolList.Add(new Checker());
        toolList.Add(new RectangleTool());
        toolList.Add(new LineTool());
        toolList.Add(new CircleTool());
        toolList.Add(new RectangleSelect());
    }
}
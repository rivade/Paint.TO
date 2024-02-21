namespace DrawingProgram;

public abstract class ToolFolder
{
    public List<DrawTool> drawTools = new();
    public List<Button> toolButtons = new();

    public T GetDrawTool<T>() where T : DrawTool, new()
    {
        T t = new();
        return t;
    }
}

public class Drawing : ToolFolder
{
    public Drawing()
    {
        drawTools.Add(new Pencil());
        drawTools.Add(new Pen());
        drawTools.Add(new Checker());
    }
}

public class Erasing : ToolFolder
{
    public Erasing()
    {
        drawTools.Add(new Eraser());
    }
}

public class Favorites : ToolFolder
{
    public Stack<DrawTool> favTools;

    public void ChangeFavorites(DrawTool d)
    {

        //favTools.Push(GetDrawTool<DrawTool>());
    }
}
namespace DrawingProgram;

public abstract class ToolFolder
{
    public List<DrawTool> drawTools = new();
    public List<IClickable> toolButtons = new();

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
        drawTools.Add(GetDrawTool<Pencil>());
    }
}

public class Erase : ToolFolder
{

}

public class Favorites : ToolFolder
{
    public Stack<DrawTool> favTools;

    public void ChangeFavorites(DrawTool d)
    {
        
        //favTools.Push(GetDrawTool<DrawTool>());
    }
}
namespace DrawingProgram;

public abstract class InterListInit
{
    public static List<IMouseInteractable> GenerateInteractables(List<ToolFolder> inputTools)
    {
        List<IMouseInteractable> interactableList = new();
        for (int i = 0; i < inputTools[0].drawTools.Count(); i++)
        {
            interactableList.Add(new ToolButton()
            { buttonRect = new Rectangle(i*90 + 10, ProgramManager.screenHeight + 10, 80, 80), DrawTool = inputTools[0].drawTools[i]}
            );
        }

        return interactableList;
    }
    public static List<IDrawable> GenerateDrawables(List<ToolFolder> inputTools)
    {
        List<IDrawable> drawableList = new();
        for (int i = 0; i < inputTools[0].drawTools.Count(); i++)
        {
            drawableList.Add(new ToolButton()
            { buttonRect = new Rectangle(i*90 + 10, ProgramManager.screenHeight + 10, 80, 80), DrawTool = inputTools[0].drawTools[i]}
            );
        }

        return drawableList;
    }
}
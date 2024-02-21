namespace DrawingProgram;

public abstract class InterListInit
{
    public static List<IMouseInteractable> GenerateInteractables(List<ToolFolder> inputTools)
    {
        List<IMouseInteractable> interactableList = new();
        for (int i = 0; i < inputTools[0].drawTools.Count(); i++)
        {
            interactableList.Add(new ToolButton()
            { buttonRect = new Rectangle(i*90 + 10, ProgramManager.CanvasHeight + 10, ToolButton.buttonSize, ToolButton.buttonSize), 
            DrawTool = inputTools[0].drawTools[i]}
            );
        }

        return interactableList;
    }
}
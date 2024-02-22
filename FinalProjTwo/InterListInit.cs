namespace DrawingProgram;

public abstract class InterListInit
{
    public static List<IHoverable> GenerateInteractables(List<ToolFolder> inputTools)
    {
        List<IHoverable> interactableList = new();
        for (int i = 0; i < inputTools[0].drawTools.Count(); i++)
        {
            interactableList.Add(new ToolButton()
            { buttonRect = new Rectangle(i*90 + 10, ProgramManager.CanvasHeight + 10, ToolButton.buttonSize, ToolButton.buttonSize), 
            DrawTool = inputTools[0].drawTools[i]}
            );
        }

        interactableList.Add(new ColorSelectorButton() { buttonRect = 
        new Rectangle(
        ProgramManager.CanvasWidth + 10,
        ProgramManager.CanvasHeight + 10,
        ColorSelectorButton.buttonSize,
        ColorSelectorButton.buttonSize
        )});

        return interactableList;
    }
}
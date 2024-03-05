namespace DrawingProgram;

public abstract class InterListInit
{
    public static List<IHoverable> GenerateInteractables(ToolFolder inputTools)
    {
        List<IHoverable> interactableList = new();
        for (int i = 0; i < inputTools.drawTools.Count(); i++)
        {
            interactableList.Add(new ToolButton()
            {
                buttonRect = new Rectangle(i * 90 + 10, Canvas.CanvasHeight + 10, ToolButton.buttonSize, ToolButton.buttonSize),
                DrawTool = inputTools.drawTools[i]
            }
            );
        }

        interactableList.Add(new ColorSelectorButton()
        {
            buttonRect =
        new Rectangle(
        Canvas.CanvasWidth + 60,
        Canvas.CanvasHeight + 10,
        ColorSelectorButton.buttonSize,
        ColorSelectorButton.buttonSize
        )
        });

        interactableList.Add(new BrushRadiusButton()
        {
            buttonRect =
        new Rectangle(
        Canvas.CanvasWidth + 60,
        Canvas.CanvasHeight - 150,
        BrushRadiusButton.buttonSize,
        BrushRadiusButton.buttonSize
        )
        });

        interactableList.Add(new CloseButton()
        {
            buttonRect =
        new Rectangle(
        Canvas.CanvasWidth + 60,
        10,
        SaveCanvasButton.buttonSize,
        SaveCanvasButton.buttonSize)
        });

        return interactableList;
    }
}
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
        Button.buttonSize,
        Button.buttonSize
        )
        });

        interactableList.Add(new BrushRadiusButton()
        {
            buttonRect =
        new Rectangle(
        Canvas.CanvasWidth + 60,
        Canvas.CanvasHeight - 150,
        Button.buttonSize,
        Button.buttonSize
        )
        });

        interactableList.Add(new CheckerSizeButton()
        {
            buttonRect =
        new Rectangle(
        Canvas.CanvasWidth + 60,
        Canvas.CanvasHeight - 320,
        Button.buttonSize,
        Button.buttonSize
        )
        });

        interactableList.Add(new FilledShapeButton()
        {
            buttonRect =
        new Rectangle(
        Canvas.CanvasWidth + 60,
        Canvas.CanvasHeight - 150,
        Button.buttonSize,
        Button.buttonSize
        )
        });

        interactableList.Add(new CloseButton()
        {
            buttonRect =
        new Rectangle(
        Canvas.CanvasWidth + 60,
        10,
        Button.buttonSize,
        Button.buttonSize)
        });

        interactableList.Add(new SaveCanvasButton()
        {
            buttonRect =
        new Rectangle(
        Canvas.CanvasWidth + 60,
        100,
        Button.buttonSize,
        Button.buttonSize)
        });

        interactableList.Add(new LoadButton()
        {
            buttonRect =
        new Rectangle(
        Canvas.CanvasWidth + 60,
        190,
        Button.buttonSize,
        Button.buttonSize)
        });

        return interactableList;
    }
}
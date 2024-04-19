namespace DrawingProgram;

public static class InterListInit
{
    private static string[] toolNames = 
    { "Pencil", "Brush", "Eraser", "Fill color",
     "Color picker", "Checker/Dither", "Draw rectangle",
     "Draw line", "Draw circle"};

    public static List<IHoverable> GenerateInteractables(ToolFolder inputTools, Canvas canvas)
    {
        List<IHoverable> interactableList = new();
        for (int i = 0; i < inputTools.drawTools.Count(); i++)
        {
            interactableList.Add(new ToolButton(toolNames[i])
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


        interactableList.Add(new OpacityButton()
        {
            buttonRect =
        new Rectangle(
        Canvas.CanvasWidth + 60,
        Canvas.CanvasHeight - 320,
        Button.buttonSize,
        Button.buttonSize
        )
        });

        interactableList.Add(new CheckerSizeButton()
        {
            buttonRect =
        new Rectangle(
        Canvas.CanvasWidth + 60,
        Canvas.CanvasHeight - 490,
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

        interactableList.Add(new LoadButton(canvas)
        {
            buttonRect =
        new Rectangle(
        Canvas.CanvasWidth + 60,
        190,
        Button.buttonSize,
        Button.buttonSize)
        });

        interactableList.Add(new SettingsButton()
        {
            buttonRect =
        new Rectangle(
        Canvas.CanvasWidth + 60,
        280,
        Button.buttonSize,
        Button.buttonSize)
        });

        interactableList.Add(new OpenLayersButton()
        {
            buttonRect =
        new Rectangle(
        Canvas.CanvasWidth - 80,
        Canvas.CanvasHeight + 10,
        Button.buttonSize,
        Button.buttonSize)
        });

        return interactableList;
    }
}
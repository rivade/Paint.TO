namespace DrawingProgram;

public static class InterListInit
{
    private static string[] toolNames = 
    { "Pencil", "Brush", "Eraser", "Fill color",
     "Color picker", "Checker/Dither", "Draw rectangle",
     "Draw line", "Draw circle"};

    public static List<IHoverable> GenerateInteractables(ProgramManager program, ToolFolder inputTools, Canvas canvas)
    {
        List<IHoverable> interactableList = new();
        for (int i = 0; i < inputTools.drawTools.Count(); i++)
        {
            interactableList.Add(new ToolButton(program, toolNames[i])
            {
                buttonRect = new Rectangle(i * 90 + 10, Canvas.CanvasHeight + 10, ToolButton.buttonSize, ToolButton.buttonSize),
                DrawTool = inputTools.drawTools[i]
            }
            );
        }

        interactableList.Add(new ColorSelectorButton(program)
        {
            buttonRect =
        new Rectangle(
        Canvas.CanvasWidth + 60,
        Canvas.CanvasHeight + 10,
        Button.buttonSize,
        Button.buttonSize
        )
        });

        interactableList.Add(new BrushRadiusButton(program)
        {
            buttonRect =
        new Rectangle(
        Canvas.CanvasWidth + 60,
        Canvas.CanvasHeight - 150,
        Button.buttonSize,
        Button.buttonSize
        )
        });


        interactableList.Add(new OpacityButton(program)
        {
            buttonRect =
        new Rectangle(
        Canvas.CanvasWidth + 60,
        Canvas.CanvasHeight - 320,
        Button.buttonSize,
        Button.buttonSize
        )
        });

        interactableList.Add(new CheckerSizeButton(program)
        {
            buttonRect =
        new Rectangle(
        Canvas.CanvasWidth + 60,
        Canvas.CanvasHeight - 490,
        Button.buttonSize,
        Button.buttonSize
        )
        });

        interactableList.Add(new FilledShapeButton(program)
        {
            buttonRect =
        new Rectangle(
        Canvas.CanvasWidth + 60,
        Canvas.CanvasHeight - 150,
        Button.buttonSize,
        Button.buttonSize
        )
        });

        interactableList.Add(new CloseButton(program)
        {
            buttonRect =
        new Rectangle(
        Canvas.CanvasWidth + 60,
        10,
        Button.buttonSize,
        Button.buttonSize)
        });

        interactableList.Add(new SaveCanvasButton(program)
        {
            buttonRect =
        new Rectangle(
        Canvas.CanvasWidth + 60,
        100,
        Button.buttonSize,
        Button.buttonSize)
        });

        interactableList.Add(new LoadButton(program, canvas)
        {
            buttonRect =
        new Rectangle(
        Canvas.CanvasWidth + 60,
        190,
        Button.buttonSize,
        Button.buttonSize)
        });

        interactableList.Add(new SettingsButton(program)
        {
            buttonRect =
        new Rectangle(
        Canvas.CanvasWidth + 60,
        280,
        Button.buttonSize,
        Button.buttonSize)
        });

        interactableList.Add(new OpenLayersButton(program)
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
namespace DrawingProgram;
using static System.Activator;

public static class ButtonCreator
{
    private const int ButtonPadding = 10;
    private const int NonToolButtonX = Canvas.CanvasWidth + 60;

    private static readonly string[] toolNames =
    [ "Pencil", "Brush", "Eraser", "Fill color",
        "Color picker", "Checker/Dither", "Draw rectangle",
        "Draw line", "Draw circle", "Rectangle select"];

    private static readonly Dictionary<Type, Rectangle> buttonPositions = new()
    {
        { typeof(ColorSelectorButton), new Rectangle(NonToolButtonX, Canvas.CanvasHeight + 10, Button.ButtonSize, Button.ButtonSize) },
        { typeof(BrushRadiusButton), new Rectangle(NonToolButtonX, Canvas.CanvasHeight - 150, Button.ButtonSize, Button.ButtonSize) },
        { typeof(CheckerSizeButton), new Rectangle(NonToolButtonX, Canvas.CanvasHeight - 320, Button.ButtonSize, Button.ButtonSize) },
        { typeof(FilledShapeButton), new Rectangle(NonToolButtonX, Canvas.CanvasHeight - 150, Button.ButtonSize, Button.ButtonSize) },
        { typeof(CloseButton), new Rectangle(NonToolButtonX, 10, Button.ButtonSize, Button.ButtonSize) },
        { typeof(SaveCanvasButton), new Rectangle(NonToolButtonX, 100, Button.ButtonSize, Button.ButtonSize) },
        { typeof(LoadButton), new Rectangle(NonToolButtonX, 190, Button.ButtonSize, Button.ButtonSize) },
        { typeof(SettingsButton), new Rectangle(NonToolButtonX, 280, Button.ButtonSize, Button.ButtonSize) },
        { typeof(OpenLayersButton), new Rectangle(NonToolButtonX - 140, Canvas.CanvasHeight + 10, Button.ButtonSize, Button.ButtonSize) }
    };

    public static List<IMouseInteractable> GenerateButtons(ProgramManager program, ToolFolder inputTools, Canvas canvas)
    {
        var interactableList = new List<IMouseInteractable>();

        for (int i = 0; i < inputTools.toolList.Count(); i++)
        {
            var toolButton = new ToolButton(program, new Rectangle(i * 90 + ButtonPadding, Canvas.CanvasHeight + ButtonPadding, Button.ButtonSize, Button.ButtonSize), toolNames[i])
            {
                tool = inputTools.toolList[i]
            };
            interactableList.Add(toolButton);
        }

        foreach (var buttonType in buttonPositions.Keys)
        {
            Button button;
            if (buttonType == typeof(LoadButton) || buttonType == typeof(OpenLayersButton))
            {
                button = (Button)CreateInstance(buttonType, program, buttonPositions[buttonType], canvas);
            }
            else button = (Button)CreateInstance(buttonType, program, buttonPositions[buttonType]);

            interactableList.Add(button);
        }
        return interactableList;
    }
}
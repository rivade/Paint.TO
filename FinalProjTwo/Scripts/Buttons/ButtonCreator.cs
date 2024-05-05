namespace DrawingProgram;
using static System.Activator;

public static class ButtonCreator
{
    private const int ButtonPadding = 10;

    private static readonly string[] toolNames =
    [ "Pencil", "Brush", "Eraser", "Fill color",
        "Color picker", "Checker/Dither", "Draw rectangle",
        "Draw line", "Draw circle", "Rectangle select"];

    private static readonly Dictionary<Type, Rectangle> ButtonPositions = new()
    {
        { typeof(ColorSelectorButton), new Rectangle(Canvas.CanvasWidth + 60, Canvas.CanvasHeight + 10, Button.ButtonSize, Button.ButtonSize) },
        { typeof(BrushRadiusButton), new Rectangle(Canvas.CanvasWidth + 60, Canvas.CanvasHeight - 150, Button.ButtonSize, Button.ButtonSize) },
        { typeof(CheckerSizeButton), new Rectangle(Canvas.CanvasWidth + 60, Canvas.CanvasHeight - 320, Button.ButtonSize, Button.ButtonSize) },
        { typeof(FilledShapeButton), new Rectangle(Canvas.CanvasWidth + 60, Canvas.CanvasHeight - 150, Button.ButtonSize, Button.ButtonSize) },
        { typeof(CloseButton), new Rectangle(Canvas.CanvasWidth + 60, 10, Button.ButtonSize, Button.ButtonSize) },
        { typeof(SaveCanvasButton), new Rectangle(Canvas.CanvasWidth + 60, 100, Button.ButtonSize, Button.ButtonSize) },
        { typeof(LoadButton), new Rectangle(Canvas.CanvasWidth + 60, 190, Button.ButtonSize, Button.ButtonSize) },
        { typeof(SettingsButton), new Rectangle(Canvas.CanvasWidth + 60, 280, Button.ButtonSize, Button.ButtonSize) },
        { typeof(OpenLayersButton), new Rectangle(Canvas.CanvasWidth - 80, Canvas.CanvasHeight + 10, Button.ButtonSize, Button.ButtonSize) }
    };

    public static List<IMouseInteractable> GenerateButtons(ProgramManager program, ToolFolder inputTools, Canvas canvas)
    {
        var interactableList = new List<IMouseInteractable>();

        for (int i = 0; i < inputTools.drawTools.Count(); i++)
        {
            var toolButton = new ToolButton(program, new Rectangle(i * 90 + ButtonPadding, Canvas.CanvasHeight + ButtonPadding, Button.ButtonSize, Button.ButtonSize), toolNames[i])
            {
                DrawTool = inputTools.drawTools[i]
            };
            interactableList.Add(toolButton);
        }

        foreach (var buttonType in ButtonPositions.Keys)
        {
            Button button;
            if (buttonType == typeof(LoadButton) || buttonType == typeof(OpenLayersButton))
            {
                button = (Button)CreateInstance(buttonType, program, ButtonPositions[buttonType], canvas);
            }
            else button = (Button)CreateInstance(buttonType, program, ButtonPositions[buttonType]);

            interactableList.Add(button);
        }
        return interactableList;
    }
}
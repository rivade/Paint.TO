namespace DrawingProgram;

public sealed class ToolButton : Button
{
    public ITool tool { get; set; }

    private static List<Color>[] colorSets =
    [
        new List<Color> {Color.Blue, Color.SkyBlue, Color.DarkBlue},
        new List<Color> {Color.Lime, Color.Green, Color.DarkGreen},
        new List<Color> {Color.Purple, Color.Pink, Color.DarkPurple}
    ];
    private static List<Color> activeColorSet
    {
        get
        {
            if (colorSetInt >= 3)
                colorSetInt = 0;
            return colorSets[colorSetInt];
        }
    }
    public static int colorSetInt = 0;

    public ToolButton(ProgramManager programInstance, Rectangle buttonRect, string hovText) : base(programInstance, buttonRect)
    {
        infoWindow = new(hovText, (int)buttonRect.X, (int)buttonRect.Y - 40);
    }

    private bool IsActiveTool() => program.currentTool == tool;

    public override void OnClick()
    {
        if (!IsActiveTool())
            program.currentTool = tool;
    }

    public override void Draw()
    {
        Raylib.DrawRectangleRec(buttonRect, GetButtonColor(activeColorSet[0], activeColorSet[1], activeColorSet[2], IsActiveTool()));
        base.Draw();
    }
}
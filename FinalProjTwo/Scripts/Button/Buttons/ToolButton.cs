namespace DrawingProgram;

public sealed class ToolButton : Button
{
    public ITool tool { get; set; }

    public static Color toolButtonColor;

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
        toolButtonColor.A = 255;
        Color hoverColor = new(Math.Clamp(toolButtonColor.R + 50, 0, 255), Math.Clamp(toolButtonColor.G + 50, 0, 255), Math.Clamp(toolButtonColor.B + 50, 0, 255), 255);
        Color activeColor = new(Math.Clamp(toolButtonColor.R - 50, 0, 255), Math.Clamp(toolButtonColor.G - 50, 0, 255), Math.Clamp(toolButtonColor.B - 50, 0, 255), 255);
        Raylib.DrawRectangleRec(buttonRect, GetButtonColor(toolButtonColor, hoverColor, activeColor, IsActiveTool()));
        base.Draw();
    }
}
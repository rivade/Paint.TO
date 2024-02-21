namespace DrawingProgram;

public abstract class ButtonGenerator
{
    public static List<Button> GenerateButtons(List<ToolFolder> inputTools)
    {
        List<Button> buttonList = new();
        for (int i = 0; i < inputTools[0].drawTools.Count(); i++)
        {
            buttonList.Add(new ToolButton()
            { buttonRect = new Rectangle(10, ProgramManager.screenHeight + 10, 80, 80), DrawTool = inputTools[0].drawTools[i]}
            );
        }

        return buttonList;
    }
}
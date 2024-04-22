namespace DrawingProgram;

public sealed class ColorSelector : PopupWindow
{
    private List<Slider> sliders = new();
    private static List<PaletteButton> paletteButtons = new();

    public ColorSelector(ProgramManager programInstance, int width, int height, string[] messagesExtern) : base(programInstance, width, height, messagesExtern)
    {
        int sliderWidth = 500;
        int sliderHeight = 15;
        int sliderPadding = 50;
        int sliderX = (int)(windowRect.X + windowRect.Width / 2 - sliderWidth / 2);

        for (int i = 0; i < 4; i++)
        {
            sliders.Add(new(20, new(sliderX, 550 + i * sliderPadding, sliderWidth, sliderHeight)));
            paletteButtons.Add(new(program, new Rectangle(760 + i * 100, 800, Button.buttonSize, Button.buttonSize), this));
        }

        sliders[3].TranslateValueToSlider(255);
    }

    public override void Draw()
    {
        base.Draw();
        Raylib.DrawCircle(ProgramManager.ScreenWidth / 2, 400, 105, Color.White);
        Raylib.DrawCircle(ProgramManager.ScreenWidth / 2, 400, 100, DrawTool.drawingColor);
        DrawSliders();
        TextHandling.DrawScreenCenteredText(["Recent:"], 750, 40, 0, Color.Black);
        paletteButtons.ForEach(p => p.Draw());
    }

    public override void Logic(Canvas canvas, Vector2 mousePos)
    {
        base.Logic(canvas, mousePos);
        DrawTool.drawingColor.R = (byte)sliders[0].GetValue(mousePos, 0, 255);
        DrawTool.drawingColor.G = (byte)sliders[1].GetValue(mousePos, 0, 255);
        DrawTool.drawingColor.B = (byte)sliders[2].GetValue(mousePos, 0, 255);
        DrawTool.drawingColor.A = (byte)sliders[3].GetValue(mousePos, 0, 255);

        Queue<Color> tmp = new(PaletteButton.paletteColors);
        paletteButtons.ForEach(p => { p.paletteColor = tmp.Dequeue(); p.OnHover(mousePos); });
    }

    public void SetSliders()
    {
        sliders[0].TranslateValueToSlider(DrawTool.drawingColor.R);
        sliders[1].TranslateValueToSlider(DrawTool.drawingColor.G);
        sliders[2].TranslateValueToSlider(DrawTool.drawingColor.B);
        sliders[3].TranslateValueToSlider(DrawTool.drawingColor.A);
    }

    private void DrawSliders()
    {
        string[] values = ["R", "G", "B", "A"];
        for (int i = 0; i < sliders.Count; i++)
        {
            Raylib.DrawText($"{values[i]}", (int)sliders[i].SliderBar.X - 50, (int)sliders[i].SliderBar.Y - 5, 30, Color.Black);
        }
        foreach (Slider slider in sliders)
        {
            slider.Draw();
            Raylib.DrawText($"{slider.GetValue(new(), 0, 255)}", (int)slider.SliderBar.X + (int)slider.SliderBar.Width + 40, (int)slider.SliderBar.Y - 5, 30, Color.Black);
        }
    }
}
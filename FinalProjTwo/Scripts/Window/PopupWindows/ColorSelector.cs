namespace DrawingProgram;

public sealed class ColorSelector : PopupWindow
{
    private List<Slider> sliders = new();
    private static List<PaletteButton> paletteButtons = new();
    private ColorPresets colorPresetsWindow = new();

    public ColorSelector(ProgramManager programInstance, int width, int height, string[] messagesExtern) : base(programInstance, width, height, messagesExtern)
    {
        int sliderWidth = 500;
        int sliderHeight = 15;
        int sliderPadding = 50;
        int sliderX = ProgramManager.ScreenWidth / 2 - sliderWidth / 2;

        for (int i = 0; i < 4; i++)
        {
            sliders.Add(new(20, new(sliderX, 550 + i * sliderPadding, sliderWidth, sliderHeight)));
            paletteButtons.Add(new(program, this, new Rectangle(760 + i * 100, 800, Button.buttonSize, Button.buttonSize)));
        }

        sliders[3].TranslateValueToSlider(255);

        windowRect = new(200, ProgramManager.ScreenHeight / 2 - height / 2, width, height);
        closeButton = new(program, windowRect);
    }

    public override void Draw()
    {
        base.Draw();
        Raylib.DrawCircle(ProgramManager.ScreenWidth / 2, 400, 105, Color.White);
        Raylib.DrawCircle(ProgramManager.ScreenWidth / 2, 400, 100, DrawTool.drawingColor);
        DrawSliders();
        TextHandling.DrawScreenCenteredText(["Recent:"], 750, 40, 0, Color.Black);
        paletteButtons.ForEach(p => p.Draw());
        colorPresetsWindow.Draw();
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
        colorPresetsWindow.Logic(mousePos, SetSliders);
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

public class ColorPresets : IDrawable
{
    private Rectangle colorsRect;
    private Texture2D colorsTexture = Raylib.LoadTexture("Textures/colors.png");
    private Image colorsImg;

    public ColorPresets()
    {
        colorsImg = Raylib.LoadImageFromTexture(colorsTexture);
        colorsRect = new(275, 450, colorsTexture.Width, colorsTexture.Height);
    }

    public void Draw()
    {
        TextHandling.DrawCenteredTextPro(["Presets:"], (int)colorsRect.X, (int)(colorsRect.X + colorsRect.Width), (int)colorsRect.Y - 70, 50, 0, Color.Black);
        Raylib.DrawRectangle((int)colorsRect.X - 5, (int)colorsRect.Y - 5, (int)colorsRect.Width + 10, (int)colorsRect.Height + 10, Color.White);
        Raylib.DrawTexture(colorsTexture, (int)colorsRect.X, (int)colorsRect.Y, Color.White);
    }

    public void Logic(Vector2 mousePos, Action setSliders)
    {
        if (Raylib.CheckCollisionPointRec(mousePos, colorsRect) && Raylib.IsMouseButtonPressed(MouseButton.Left))
        {
            Vector2 pos = TranslatePosToImg(mousePos, colorsRect);
            DrawTool.drawingColor = Raylib.GetImageColor(colorsImg, (int)pos.X, (int)pos.Y);
            setSliders.Invoke();
        }
    }

    private Vector2 TranslatePosToImg(Vector2 mousePos, Rectangle imgRect)
    {
        return mousePos - new Vector2(imgRect.X, imgRect.Y);
    }
}
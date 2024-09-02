namespace DrawingProgram;

public sealed unsafe class ColorSelector : PopupWindow
{
    private List<Slider> sliders = new();
    private List<PaletteButton> paletteButtons;
    private ColorPresets colorPresetsWindow;

    private Color* color;

    public ColorSelector(ProgramManager programInstance, int width, int height, string[] messagesExtern, Color* colorToChange) : base(programInstance, width, height, messagesExtern)
    {
        int sliderWidth = 500;
        int sliderHeight = 15;
        int sliderPadding = 50;
        int sliderX = ProgramManager.ScreenWidth / 2 - sliderWidth / 2;

        colorPresetsWindow = new(colorToChange);
        color = colorToChange;

        fixed (Color* drawColorPtr = &DrawTool.drawingColor)

            if (colorToChange == drawColorPtr) paletteButtons = new();
        for (int i = 0; i < 4; i++)
        {
            sliders.Add(new(20, new(sliderX, 550 + i * sliderPadding, sliderWidth, sliderHeight)));
            paletteButtons?.Add(new(program, this, new Rectangle(760 + i * 100, 800, Button.ButtonSize, Button.ButtonSize)));
        }

        windowRect = new(200, ProgramManager.ScreenHeight / 2 - height / 2, width, height);
        closeButton = new(program, windowRect);
        SetSliders();
    }

    public override void Draw()
    {
        base.Draw();
        Raylib.DrawCircle(ProgramManager.ScreenWidth / 2, 400, 105, Color.White);
        Raylib.DrawCircle(ProgramManager.ScreenWidth / 2, 400, 100, new(color->R, color->G, color->B, color->A));
        DrawSliders();

        fixed (Color* drawColorPtr = &DrawTool.drawingColor)
            if (color == drawColorPtr)
                TextHandling.DrawScreenCenteredText(["Recent:"], 750, 40, 0, Color.Black);

        paletteButtons?.ForEach(p => p.Draw());
        colorPresetsWindow.Draw();
    }

    public override void Logic(Canvas canvas, Vector2 mousePos)
    {
        base.Logic(canvas, mousePos);

        fixed (Color* drawColorPtr = &DrawTool.drawingColor)
            if (color == drawColorPtr)
            {
                Queue<Color> tmp = new(PaletteButton.paletteColors);
                paletteButtons.ForEach(p => { p.paletteColor = tmp.Dequeue(); p?.OnHover(mousePos); });
            }

        color->R = (byte)sliders[0].GetValue(mousePos, 0, 255);
        color->G = (byte)sliders[1].GetValue(mousePos, 0, 255);
        color->B = (byte)sliders[2].GetValue(mousePos, 0, 255);
        color->A = (byte)sliders[3].GetValue(mousePos, 0, 255);

        colorPresetsWindow.Logic(mousePos, SetSliders);

        canvas.UpdateBackgroundColor();
    }

    public void SetSliders()
    {
        sliders[0].TranslateValueToSlider(color->R);
        sliders[1].TranslateValueToSlider(color->G);
        sliders[2].TranslateValueToSlider(color->B);
        sliders[3].TranslateValueToSlider(color->A);
    }

    private void DrawSliders()
    {
        string[] values = ["R", "G", "B", "A"];
        Color[] colors = [Color.Red, Color.Green, Color.Blue, new(color->R, color->G, color->B, (byte)255)];
        for (int i = 0; i < sliders.Count; i++)
        {
            Raylib.DrawText($"{values[i]}", (int)sliders[i].SliderBar.X - 50, (int)sliders[i].SliderBar.Y - 5, 30, Color.Black);
            Raylib.DrawRectangle((int)sliders[i].SliderBar.X - 3, (int)sliders[i].SliderBar.Y - 3, (int)sliders[i].SliderBar.Width + 6, (int)sliders[i].SliderBar.Height + 6, Color.White);
            Raylib.DrawRectangle((int)sliders[i].SliderBar.X, (int)sliders[i].SliderBar.Y, (int)sliders[i].SliderBar.Width, (int)sliders[i].SliderBar.Height, GUIarea.GUIColor);
            if (i < 3)
                sliders[i].DrawGradient(Color.Black, colors[i]);
            else if (i == 3)
                sliders[i].DrawGradient(Color.Blank, colors[i]);
            Raylib.DrawText($"{sliders[i].GetValue(new(), 0, 255)}", (int)sliders[i].SliderBar.X + (int)sliders[i].SliderBar.Width + 40, (int)sliders[i].SliderBar.Y - 5, 30, Color.Black);
        }
    }
}

public unsafe class ColorPresets : IDrawable
{
    private Rectangle colorsRect;
    private Texture2D colorsTexture = Raylib.LoadTexture("Textures/colors.png");
    private Image colorsImg;

    private Color* color;

    public ColorPresets(Color* colorToChange)
    {
        colorsImg = Raylib.LoadImageFromTexture(colorsTexture);
        colorsRect = new(275, 450, colorsTexture.Width, colorsTexture.Height);
        color = colorToChange;
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
            Color tmp = Raylib.GetImageColor(colorsImg, (int)pos.X, (int)pos.Y);
            color->R = tmp.R;
            color->G = tmp.G;
            color->B = tmp.B;
            color->A = tmp.A;
            setSliders.Invoke();
        }
    }

    private Vector2 TranslatePosToImg(Vector2 mousePos, Rectangle imgRect)
    {
        return mousePos - new Vector2(imgRect.X, imgRect.Y);
    }
}
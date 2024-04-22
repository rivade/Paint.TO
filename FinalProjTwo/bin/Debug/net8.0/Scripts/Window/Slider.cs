using System.Xml.XPath;

namespace DrawingProgram;

public class Slider
{
    public Rectangle SliderBar;
    public Circle SliderCircle;
    bool updateSliderPos = false;
    int minSlideValue;
    int maxSlideValue;

    public Slider(int circleRadius, Rectangle bar)
    {
        SliderBar = bar;
        SliderCircle = new() { Middle = new(SliderBar.X, SliderBar.Y + SliderBar.Height / 2), Radius = circleRadius };

        minSlideValue = (int)SliderBar.X;
        maxSlideValue = (int)SliderBar.X + (int)SliderBar.Width;
    }

    public int GetValue(Vector2 mousePos, int minValue, int maxValue)
    {
        Update(mousePos);
        float sliderRange = maxSlideValue - minSlideValue;
        float valueRange = maxValue - minValue;
        float relativePosition = (SliderCircle.Middle.X - minSlideValue) / sliderRange;
        int value = (int)(minValue + relativePosition * valueRange);
        return value;
    }

    public void Draw()
    {
        Raylib.DrawRectangleRec(SliderBar, Color.Gray);
        Raylib.DrawCircleV(SliderCircle.Middle, SliderCircle.Radius, Color.Gold);
    }

    private void Update(Vector2 mousePos)
    {
        if (Raylib.CheckCollisionPointCircle(mousePos, SliderCircle.Middle, SliderCircle.Radius) && Raylib.IsMouseButtonPressed(MouseButton.Left))
            updateSliderPos = true;

        if (Raylib.IsMouseButtonReleased(MouseButton.Left))
            updateSliderPos = false;

        if (updateSliderPos && Raylib.IsMouseButtonDown(MouseButton.Left) && IsCursorAboveBelowSlider(mousePos))
            SliderCircle.Middle += new Vector2(Raylib.GetMouseDelta().X, 0);
        SetSliderWithinBounds();
    }

    private bool IsCursorAboveBelowSlider(Vector2 mousePos)
    {
        int buffer = 40;
        return mousePos.X > minSlideValue - buffer && mousePos.X < maxSlideValue + buffer;
    }

    private void SetSliderWithinBounds()
    {
        if (SliderCircle.Middle.X > maxSlideValue)
            SliderCircle.Middle = new(maxSlideValue, SliderCircle.Middle.Y);

        else if (SliderCircle.Middle.X < minSlideValue)
            SliderCircle.Middle = new(minSlideValue, SliderCircle.Middle.Y);
    }

    public void TranslateValueToSlider(int value)
    {
        int sliderWidth = (int)SliderBar.Width;

        float percentage = (float)value / 255f;
        float posX = SliderBar.X + percentage * sliderWidth;

        SliderCircle.Middle = new Vector2(posX, SliderCircle.Middle.Y);
    }
}
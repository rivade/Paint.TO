namespace DrawingProgram;

public abstract class Button : IMouseInteractable, IDrawable
{
    protected ProgramManager program;
    public Rectangle buttonRect;
    protected Texture2D icon;

    public Button(ProgramManager programInstance, Rectangle button)
    {
        program = programInstance;
        buttonRect = button;
    }

    public const int ButtonSize = 80;
    protected bool isHoveredOn;
    protected InfoText infoWindow;

    public virtual void OnHover(Vector2 mousePos)
    {
        isHoveredOn = false;
        if (Raylib.CheckCollisionPointRec(mousePos, buttonRect))
        {
            isHoveredOn = true;

            if (Raylib.IsMouseButtonPressed(MouseButton.Left))
                OnClick();
        }
    }

    public abstract void OnClick();

    public virtual void Draw()
    {
        if (isHoveredOn)
            infoWindow.Draw();
        Raylib.DrawTexture(icon, (int)buttonRect.X, (int)buttonRect.Y, Color.White);
    }

    protected Color GetButtonColor(Color defaultColor, Color hoverColor, Color activeColor, bool isActive)
    {
        Color color = defaultColor;

        if (isHoveredOn && !isActive)
            color = hoverColor;

        else if (isActive)
            color = activeColor;

        return color;
    }
}
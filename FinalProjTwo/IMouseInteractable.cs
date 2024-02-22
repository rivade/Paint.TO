namespace DrawingProgram;

public interface IHoverable
{
    public void OnHover(Vector2 mousePos);
}

public interface IClickable
{
    public void OnClick();
}
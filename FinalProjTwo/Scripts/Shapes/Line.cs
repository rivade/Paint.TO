namespace DrawingProgram;

public struct Line
{
    public Vector2 startPos;
    public Vector2 endPos;

    public Line(Vector2 start, Vector2 end)
    {
        startPos = start;
        endPos = end;
    }
}
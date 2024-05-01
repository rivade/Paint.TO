namespace DrawingProgram;

public struct Line
{
    public Vector2 StartPos { get; set; }
    public Vector2 EndPos { get; set; }

    public Line(Vector2 start, Vector2 end)
    {
        StartPos = start;
        EndPos = end;
    }
}
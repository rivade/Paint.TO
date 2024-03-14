namespace DrawingProgram;

public struct Circle
{
    public int Radius {get; private set;}
    public Vector2 Middle {get; private set;}

    public static Circle CreateCircle(Vector2 midPoint, Vector2 edgePoint)
    { 
        return new Circle() { Radius = (int)Vector2.Distance(midPoint, edgePoint), Middle = midPoint };
    }
}
namespace DrawingProgram;

public struct Circle
{
    public int Radius {get; set;}
    public Vector2 Middle {get; set;}

    public Circle(Vector2 midPoint, Vector2 edgePoint)
    { 
        Radius = (int)Vector2.Distance(midPoint, edgePoint); 
        Middle = midPoint;
    }
}
namespace DrawingProgram;

public sealed class EyeDropper : DrawTool
{
    public override void Update(Image canvas, Vector2 mousePos)
    {
        Rectangle canvasRect = new(0, 0, new Vector2(canvas.Width, canvas.Height));
        if (Raylib.IsMouseButtonDown(MouseButton.Left) && Raylib.CheckCollisionPointRec(mousePos, canvasRect))
        {
            drawingColor = Raylib.GetImageColor(canvas, (int)mousePos.X, (int)mousePos.Y);
            if (drawingColor.Equals(Color.Blank)) drawingColor = Canvas.backgroundColor;
        }
        base.Update(canvas, mousePos);
    }
}
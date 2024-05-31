namespace DrawingProgram;

public sealed class Bucket : DrawTool
{
    private static readonly Vector2 CanvasArea = new(Canvas.CanvasWidth + Canvas.CanvasOffset, Canvas.CanvasHeight + Canvas.CanvasOffset);

    public override async void Update(Image canvas, Vector2 mousePos)
    {
        base.Update(canvas, mousePos);

        if (Raylib.IsMouseButtonPressed(MouseButton.Left))
        {
            Color targetColor = Raylib.GetImageColor(canvas, (int)mousePos.X, (int)mousePos.Y);
            await FloodFill(canvas, mousePos, targetColor);
        }
    }

    private static async Task FloodFill(Image img, Vector2 pt, Color targetColor)
    {
        if (targetColor.Equals(drawingColor))
        {
            return;
        }

        await Task.Run(() =>
        {
            lock (lockObj)
            {
                Stack<Vector2> pixels = new();

                pixels.Push(pt);
                while (pixels.Count != 0)
                {
                    Vector2 temp = pixels.Pop();
                    int y1 = (int)temp.Y;
                    while (y1 >= Canvas.CanvasOffset && Raylib.GetImageColor(img, (int)temp.X, y1).Equals(targetColor))
                    {
                        y1--;
                    }
                    y1++;
                    bool spanLeft = false;
                    bool spanRight = false;
                    while (y1 < CanvasArea.Y && Raylib.GetImageColor(img, (int)temp.X, y1).Equals(targetColor))
                    {
                        Raylib.ImageDrawPixel(ref img, (int)temp.X, y1, drawingColor);

                        if (!spanLeft && temp.X > Canvas.CanvasOffset && Raylib.GetImageColor(img, (int)temp.X - 1, y1).Equals(targetColor))
                        {
                            pixels.Push(new Vector2(temp.X - 1, y1));
                            spanLeft = true;
                        }
                        else if (spanLeft && temp.X - 1 == Canvas.CanvasOffset && !Raylib.GetImageColor(img, (int)temp.X - 1, y1).Equals(targetColor))
                        {
                            spanLeft = false;
                        }
                        if (!spanRight && temp.X < CanvasArea.X - 1 && Raylib.GetImageColor(img, (int)temp.X + 1, y1).Equals(targetColor))
                        {
                            pixels.Push(new Vector2(temp.X + 1, y1));
                            spanRight = true;
                        }
                        else if (spanRight && temp.X < CanvasArea.X - 1 && !Raylib.GetImageColor(img, (int)temp.X + 1, y1).Equals(targetColor))
                        {
                            spanRight = false;
                        }
                        y1++;
                    }
                }
            }
        });
    }
}
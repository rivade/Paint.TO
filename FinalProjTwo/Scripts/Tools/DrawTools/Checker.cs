namespace DrawingProgram;

public sealed class Checker : DrawTool
{
    public static int checkerSize = 5;

    public override void Update(Image canvas, Vector2 mousePos)
    {
        base.Update(canvas, mousePos);

        lock (lockObj)
        {
            if (Raylib.IsMouseButtonDown(MouseButton.Left))
                SetCheckers(canvas, mousePos, false);
            else if (Raylib.IsMouseButtonDown(MouseButton.Right))
                SetCheckers(canvas, mousePos, true);
        }
    }

    private void SetCheckers(Image canvas, Vector2 mousePos, bool offsetByOneUnit)
    {
        int rows = (int)Math.Ceiling((double)Canvas.CanvasHeight + Canvas.CanvasOffset / checkerSize);
        int cols = (int)Math.Ceiling((double)Canvas.CanvasWidth + Canvas.CanvasOffset / checkerSize);

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                int xPos = col * checkerSize;
                int yPos = row * checkerSize;

                if (offsetByOneUnit) xPos += checkerSize;

                Vector2 squareCenter = new Vector2(xPos + checkerSize / 2, yPos + checkerSize / 2);

                float distanceToMouse = Vector2.Distance(mousePos, squareCenter);

                if (distanceToMouse <= brushRadius)
                {
                    if ((row + col) % 2 == 0)
                        Raylib.ImageDrawRectangle(ref canvas, xPos, yPos, checkerSize, checkerSize, drawingColor);
                }
            }
        }
    }
}
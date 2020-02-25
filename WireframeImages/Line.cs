using System.Drawing;

namespace WireframeImages
{
    class Line : IShape
    {
        public Line(float x1, float y1, float x2, float y2, Color color)
        {
            X1 = x1;
            Y1 = y1;
            X2 = x2;
            Y2 = y2;
            Color = color;
        }

        public float X1 { get; }
        public float Y1 { get; }
        public float X2 { get; }
        public float Y2 { get; }
        public Color Color { get; }


        public void Draw(ref Graphics g, float originX, float originY, float scale, float offsetX, float offsetY, float brushSize)
        {
            g.DrawLine(new Pen(Color, brushSize),
                originX + X1 * scale + offsetX,
                originY + Y1 * scale + offsetY,
                originX + X2 * scale + offsetX,
                originY + Y2 * scale + offsetY);
        }
    }
}
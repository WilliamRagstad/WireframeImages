using System.Drawing;

namespace WireframeImages
{
    class Triangle : IShape
    {
        public Triangle(Line l1, Line l2, Line l3)
        {
            L1 = l1;
            L2 = l2;
            L3 = l3;
        }

        public Line L1 { get; }
        public Line L2 { get; }
        public Line L3 { get; }

        public void Draw(ref Graphics g, float originX, float originY, float scale, float offsetX, float offsetY, float brushSize)
        {
            L1.Draw(ref g, originX, originY, scale, offsetX, offsetY, brushSize);
            L2.Draw(ref g, originX, originY, scale, offsetX, offsetY, brushSize);
            L3.Draw(ref g, originX, originY, scale, offsetX, offsetY, brushSize);
        }
    }
}
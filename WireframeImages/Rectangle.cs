using System.Drawing;

namespace WireframeImages
{
    class Rectangle : IShape
    {
        public Rectangle(Line l1, Line l2, Line l3, Line l4)
        {
            L1 = l1;
            L2 = l2;
            L3 = l3;
            L4 = l4;
        }

        public Line L1 { get; }
        public Line L2 { get; }
        public Line L3 { get; }
        public Line L4 { get; }

        public void Draw(ref Graphics g, float originX, float originY, float scale, float offsetX, float offsetY, float brushSize)
        {
            L1.Draw(ref g, originX, originY, scale, offsetX, offsetY, brushSize);
            L2.Draw(ref g, originX, originY, scale, offsetX, offsetY, brushSize);
            L3.Draw(ref g, originX, originY, scale, offsetX, offsetY, brushSize);
            L4.Draw(ref g, originX, originY, scale, offsetX, offsetY, brushSize);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace WireframeImages
{
    interface IShape
    {
        public void Draw(ref Graphics g, float originX, float originY, float scale, float offsetX, float offsetY, float brushSize);
    }
}

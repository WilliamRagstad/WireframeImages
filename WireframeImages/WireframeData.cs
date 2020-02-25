namespace WireframeImages
{
    class WireframeData
    {
        public WireframeData(int shapesCapacity)
        {
            Shapes = new IShape[shapesCapacity];
        }
        public WireframeData(IShape[] shapes, SymmetryMode symmetryMode)
        {
            Shapes = shapes;
            SymmetryMode = symmetryMode;
        }

        public IShape[] Shapes;
        public SymmetryMode SymmetryMode;
    }

    enum SymmetryMode
    {
        None = 0b0,
        X = 0b1,
        Y = 0b10,
        XY = X | Y
    }
}
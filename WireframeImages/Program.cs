using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;

namespace WireframeImages
{
    class Program
    {
        static void PrintHelp()
        {
            Console.WriteLine("Usage: [file] [sizeW] [sizeH] [output] (scale) (offsetX) (offsetY) (brushW)");
            Console.WriteLine("  - scale of object.");
            Console.WriteLine("  - sizeW & sizeH is the dimensions of the output image.");
            Console.WriteLine("  - brushW is the width of each line.");
        }

        static void Main(string[] args)
        {
            //args = new[] { "ship.wf", "650", "260", "0.7", "ship.png" };

            if (args.Length >= 4)
            {
                string file = args[0];
                int width = int.Parse(args[1]);
                int height = int.Parse(args[2]);
                string outputFile = args[3];

                float scale = 1;
                if (args.Length >= 5)
                    scale = float.Parse(args[4].Replace(".", ",")) - 0.5f; // -0.5 will make sure the wireframe is filling in the whole image.

                float offsetX = 0;
                if (args.Length >= 6)
                    offsetX = float.Parse(args[5].Replace(".", ","));

                float offsetY = 0;
                if (args.Length >= 7)
                    offsetY = float.Parse(args[6].Replace(".", ","));

                float brushWidth = 2;
                if (args.Length >= 8)
                    brushWidth = float.Parse(args[7].Replace(".", ","));

                if (!File.Exists(file))
                {
                    Console.WriteLine("Provide a WireFrame file to draw!");
                    PrintHelp();
                    return;
                }

                Bitmap outputImage = Render(file, width, height, scale, offsetX, offsetY, brushWidth);
                if (outputImage != null)
                    outputImage.Save(outputFile, System.Drawing.Imaging.ImageFormat.Png);
            }
            else PrintHelp();
        }

        private static Bitmap Render(string dataFile, int width, int height, float scale, float offsetX, float offsetY, float brushSize)
        {
            WireframeData wd = Parse(dataFile);
            if (wd == null)
                return null;
            else
            {
                // Initialize image
                Bitmap output = new Bitmap(width, height);
                Graphics g = Graphics.FromImage(output);

                g.CompositingQuality = CompositingQuality.HighSpeed;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.CompositingMode = CompositingMode.SourceCopy;

                float orgX = (float)width  / 2;
                float orgY = (float)height / 2;

                for (int i = 0; i < wd.Shapes.Length; i++)
                    wd.Shapes[i].Draw(ref g, orgX, orgY, scale, offsetX, offsetY, brushSize);

                // Mirror
                if (wd.SymmetryMode != SymmetryMode.None)
                {
                    if ((wd.SymmetryMode & SymmetryMode.Y) != 0)
                    {
                        for (int x = 0; x < output.Width/2; ++x)
                        {
                            for (int y = 0; y < output.Height; ++y)
                            {
                                output.SetPixel(x, y, output.GetPixel(output.Width-x-1, y));
                            }
                        }
                    }
                    if ((wd.SymmetryMode & SymmetryMode.X) != 0)
                    {
                        for (int x = 0; x < output.Width; ++x)
                        {
                            for (int y = 0; y < output.Height/2; ++y)
                            {
                                output.SetPixel(x, y, output.GetPixel(x, output.Height-y-1));
                            }
                        }
                    }
                }

                g.Dispose();
                return output;
            }
        }

        private static WireframeData Parse(string file)
        {
            // Initialize variables
            List<IShape> shapes = new List<IShape>();
            SymmetryMode sm = SymmetryMode.None;
            // Read data
            StreamReader sr = new StreamReader(file);

            int lineNr = 0;
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                string l = line.Trim().ToLower();
                lineNr++;


                try
                {
                    if (string.IsNullOrEmpty(l) || l[0] == '#') continue;
                    else if (l[0] == '@' && char.IsLetter(l[1]))
                    {
                        // Directive
                        string directive = l.Substring(1, l.IndexOf(' ') - 1);
                        string[] values = l.Substring(l.IndexOf(' ') + 1).Split(',');
                        switch (directive)
                        {
                            case "sym":
                                if (values.Length > 0)
                                {
                                    if (values[0] == "x")
                                        sm = SymmetryMode.X;
                                    else if (values[0] == "y")
                                        sm = SymmetryMode.Y;
                                    else if (values[0] == "xy")
                                        sm = SymmetryMode.XY;
                                    else if (values[0] == "yx")
                                        sm = SymmetryMode.XY;
                                    else
                                        throw new Exception("Unknown parameter type for Sym");
                                }
                                else
                                    throw new Exception("Directive Sym requires one parameter!");
                                break;
                        }
                    }
                    else if (l[0] == 'r' && l[1] == ' ')
                    {
                        // Rectangle
                        string[] tokens = l.Substring(2).Split(',');
                        if (tokens.Length >= 4)
                        {
                            Color color = Color.Black;
                            if (tokens.Length == 5)
                                color = GetColor(line);

                            string[] p1 = tokens[0].Trim().Split(' ');
                            string[] p2 = tokens[1].Trim().Split(' ');
                            string[] p3 = tokens[2].Trim().Split(' ');
                            string[] p4 = tokens[3].Trim().Split(' ');
                            if (p1.Length != 2 || p2.Length != 2 || p3.Length != 2 || p4.Length != 2)
                                throw new System.Exception("r: String must contain a pair of real values!");

                            float p1x = float.Parse(p1[0].Replace('.', ','));
                            float p2x = float.Parse(p2[0].Replace('.', ','));
                            float p3x = float.Parse(p3[0].Replace('.', ','));
                            float p4x = float.Parse(p4[0].Replace('.', ','));

                            float p1y = float.Parse(p1[1].Replace('.', ','));
                            float p2y = float.Parse(p2[1].Replace('.', ','));
                            float p3y = float.Parse(p3[1].Replace('.', ','));
                            float p4y = float.Parse(p4[1].Replace('.', ','));

                            Line l1 = new Line(p1x, p1y, p2x, p2y, color);
                            Line l2 = new Line(p2x, p2y, p3x, p3y, color);
                            Line l3 = new Line(p3x, p3y, p4x, p4y, color);
                            Line l4 = new Line(p4x, p4y, p1x, p1y, color);

                            shapes.Add(new Rectangle(l1, l2, l3, l4));
                        }
                        else
                        {
                            throw new Exception($"Not a valid rectangle. Format: r [x1] [y1], [x2] [y2], [x3] [y3], [x4] [y4] (, [color])");
                        }
                    }
                    else if (l[0] == 't' && l[1] == ' ')
                    {
                        // Triangle
                        string[] tokens = l.Substring(2).Split(',');
                        if (tokens.Length >= 3)
                        {
                            Color color = Color.Black;
                            if (tokens.Length == 4)
                                color = GetColor(line);

                            string[] p1 = tokens[0].Trim().Split(' ');
                            string[] p2 = tokens[1].Trim().Split(' ');
                            string[] p3 = tokens[2].Trim().Split(' ');
                            if (p1.Length != 2 || p2.Length != 2 || p3.Length != 2)
                                throw new Exception("t: String must contain a pair of real values!");

                            float p1x = float.Parse(p1[0].Replace('.', ','));
                            float p2x = float.Parse(p2[0].Replace('.', ','));
                            float p3x = float.Parse(p3[0].Replace('.', ','));

                            float p1y = float.Parse(p1[1].Replace('.', ','));
                            float p2y = float.Parse(p2[1].Replace('.', ','));
                            float p3y = float.Parse(p3[1].Replace('.', ','));

                            Line l1 = new Line(p1x, p1y, p2x, p2y, color);
                            Line l2 = new Line(p2x, p2y, p3x, p3y, color);
                            Line l3 = new Line(p3x, p3y, p1x, p1y, color);

                            shapes.Add(new Triangle(l1, l2, l3));
                        }
                        else
                        {
                            throw new Exception($"Not a valid triangle. Format: t [x1] [y1], [x2] [y2], [x3] [y3] (, [color])");
                        }
                    }
                    else if (l[0] == 'l' && l[1] == ' ')
                    {
                        // Line
                        string[] tokens = l.Substring(2).Split(',');
                        if (tokens.Length >= 2)
                        {
                            Color color = Color.Black;
                            if (tokens.Length == 3)
                                color = GetColor(line);

                            string[] p1 = tokens[0].Trim().Split(' ');
                            string[] p2 = tokens[1].Trim().Split(' ');
                            if (p1.Length != 2 || p2.Length != 2)
                                throw new Exception("l: String must contain a pair of real values!");

                            float p1x = float.Parse(p1[0].Replace('.', ','));
                            float p2x = float.Parse(p2[0].Replace('.', ','));

                            float p1y = float.Parse(p1[1].Replace('.', ','));
                            float p2y = float.Parse(p2[1].Replace('.', ','));

                            shapes.Add(new Line(p1x, p1y, p2x, p2y, color));
                        }
                        else
                        {
                            throw new Exception($"Not a valid line. Format: l [x1] [y1], [x2] [y2] (, [color])");
                        }
                    }
                    else
                    {
                        throw new Exception($"Unknown token: " + l);
                    }
                }
                catch(Exception e)
                {
                    Console.WriteLine($"[{lineNr}] {e.Message}");
                }
            }

            return new WireframeData(shapes.ToArray(), sm);
        }

        private static Color GetColor(string line)
        {
            // Add color as well
            string[] rct = line.Split(' ');
            string colorToken = rct[rct.Length - 1].Trim();
            if (colorToken[0] == '#') // Hex Color
                return ColorTranslator.FromHtml(colorToken);
            else // Named Color
                return Color.FromName(colorToken);
        }

    }
}

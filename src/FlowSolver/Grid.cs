using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowSolver
{
    public partial class Grid
    {
        public Size Size { get; private set; }          //Velikost mřížky
        public bool[,] Walls { get; private set; }      //Na daných souřadnicích je zeď? (Souřadnice viz WallTool)
        public int[,] Dots { get; private set; }        //Na daných souřadnicích: -1...nic není, jinak je tam číslo barvy
        public bool[,] Bridges { get; private set; }    //Na daných souřadnicích je most
        //Kolekce směrů:
        public static readonly Point[] Directions = new Point[] { new Point(-1, 0), new Point(1, 0), new Point(0, -1), new Point(0, 1)};

        #region Aliasy pro výšku a šířku, kontruktor
        public int Width
        {
            get
            {
                return Size.Width;
            }
        }
        public int Height
        {
            get
            {
                return Size.Height;
            }
        }

        private Grid(Size size)
        {
            Size = size;
            Dots = new int[Width, Height];
            Bridges = new bool[Width, Height];
            Walls = new bool[2* Width, 2*Height];
            for(int x = 0; x < Width; x++)
            {
                for(int y = 0; y < Height; y++)
                {
                    Dots[x, y] = -1;
                }
            }
        }
        #endregion

        #region Přetékání a sousední pole
        public int ModH(int a)
        {
            return (a + Height) % Height;
        }
        public int Mod2H(int a)
        {
            return (a + 2*Height) % (2*Height);
        }
        public int ModW(int a)
        {
            return (a + Width) % Width;
        }
        public int Mod2W(int a)
        {
            return (a + 2 * Width) % (2 * Width);
        }

        public Point NeigbourCoordinates(Point p, Point dir)
        {
            return new Point(ModW(p.X + dir.X), ModH(p.Y + dir.Y));
        }
        public Point WallCoordinates(Point p, Point dir)
        {
            return new Point(Mod2W(2 * p.X + dir.X + 1), Mod2H(2 * p.Y + dir.Y + 1));
        }
        public bool IsWall(Point p, Point dir)
        {
            Point wc = WallCoordinates(p, dir);
            return Walls[wc.X, wc.Y];
        }
        public bool IsBridge(Point p, Point dir)
        {
            Point wc = NeigbourCoordinates(p, dir);
            return Bridges[wc.X, wc.Y];
        }
        #endregion

        #region Vytváření předdefinovaných mřížek
        public static Grid Empty(Size size)
        {
            Grid g = new Grid(size);
            for(int x = 0; x < g.Width; x++)
            {
                g.Walls[2 * x + 1, 0] = true;
            }
            for (int y = 0; y < g.Height; y++)
            {
                g.Walls[0, 2 * y + 1] = true;
            }
            return g;
        }

        public static Grid FourWarps(Size size)
        {
            Grid g = Grid.Empty(size);
            if(g.Height % 2 == 1 && g.Width % 2 == 1)
            {
                g.Walls[g.Width, 0] = g.Walls[0, g.Width] = false;
            }
            else if(g.Height % 2 == 0)
            {
                g.Walls[0, g.Height-3] = g.Walls[0, g.Height+3] = false;
            }
            else if (g.Width % 2 == 0)
            {
                g.Walls[g.Height - 3, 0] = g.Walls[g.Width + 3, 0] = false;
            }

            return g;
        }

        public static Grid FourBridges(Size size)
        {
            Grid g = Grid.Empty(size);
            g.Bridges[1, 1] = g.Bridges[1, size.Height - 2] = g.Bridges[size.Width - 2, 1] = g.Bridges[size.Width - 2, size.Height - 2] = true;
            return g;
        }

        public static Grid OneRing(Size size)
        {
            Grid g = Grid.Empty(size);
            for (int x = 1; x < g.Width-1; x++)
            {
                g.Walls[2 * x + 1, 2] = true;
                g.Walls[2 * x + 1, g.Height*2-2] = true;
            }
            for (int y = 1; y < g.Height-1; y++)
            {
                g.Walls[2, 2 * y + 1] = true;
                g.Walls[g.Width*2-2, 2 * y + 1] = true;
            }
            return g;
        }

        public static Grid TwoRings(Size size)
        {
            Grid g = Grid.OneRing(size);
            for (int x = 2; x < g.Width-2; x++)
            {
                g.Walls[2 * x + 1, 4] = true;
                g.Walls[2 * x + 1, g.Height * 2 - 4] = true;
            }
            for (int y = 2; y < g.Height-2; y++)
            {
                g.Walls[4, 2 * y + 1] = true;
                g.Walls[g.Width * 2 - 4, 2 * y + 1] = true;
            }
            return g;
        }
        #endregion

        //Manhatton vzdálenost na mřížce
        public int Distance(Point a, Point b)
        {
            return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
        }

        //Zkotroluje, zda se v zadání nevyskytuje nějaká barva právě jednou
        public bool IsValid()
        {
            int[] dotsWithColor = new int[20];
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    if (Dots[x, y] != -1)
                    {
                        dotsWithColor[Dots[x, y]]++;
                    }
                }
            }
            return dotsWithColor.All(i => i % 2 == 0);
        }

        #region Vykreslování
        //Vykreslení zadání
        public void Draw(Graphics g, List<Color> colors)
        {
            Pen pen1 = new Pen(Color.DimGray, 0.004f);
            Pen pen2 = new Pen(Color.DarkGray, 0.03f);
            pen2.StartCap = pen2.EndCap = System.Drawing.Drawing2D.LineCap.Round;
            var dirs = new List<Point>() { new Point(-1, 0), new Point(0, -1), new Point(1, 0), new Point(0, 1) };
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    g.DrawLine(Walls[2 * x, 2 * y + 1] ? pen2 : pen1, x, y, x, y + 1);
                    g.DrawLine(Walls[2 * x + 1, 2 * y] ? pen2 : pen1, x, y, x + 1, y);
                    if(x == 0)
                    {
                        g.DrawLine(Walls[2 * x, 2 * y + 1] ? pen2 : pen1, x+Width, y, x+Width, y + 1);
                    }
                    if(y == 0)
                    {
                        g.DrawLine(Walls[2 * x + 1, 2 * y] ? pen2 : pen1, x, y+Height, x + 1, y+Height);
                    }
                    if(Dots[x, y] > -1)
                    {
                        g.FillEllipse(new SolidBrush(colors[Dots[x, y]]), x+0.2f, y+0.2f, 0.6f, 0.6f);
                    }
                }
            }
            RedrawBridges(g);
        }

        //Překreslení mostů (je třeba poté co se vykreslí řešení)
        public void RedrawBridges(Graphics g)
        {
            Pen pen2 = new Pen(Color.DarkGray, 0.03f);
            pen2.StartCap = pen2.EndCap = System.Drawing.Drawing2D.LineCap.Round;
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    if (Bridges[x, y])
                    {
                        g.DrawLine(pen2, x + 0.2f, y + 0.35f, x + 0.8f, y + 0.35f);
                        g.DrawLine(pen2, x + 0.2f, y + 0.65f, x + 0.8f, y + 0.65f);

                        g.DrawLine(pen2, x + 0.35f, y + 0.2f, x + 0.35f, y + 0.345f);
                        g.DrawLine(pen2, x + 0.65f, y + 0.2f, x + 0.65f, y + 0.345f);

                        g.DrawLine(pen2, x + 0.35f, y + 0.655f, x + 0.35f, y + 0.8f);
                        g.DrawLine(pen2, x + 0.65f, y + 0.655f, x + 0.65f, y + 0.8f);
                    }
                }
            }
        }
        #endregion
    }
}

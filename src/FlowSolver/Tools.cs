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
        //Třídy reprezentující jednotlivé nástroje a jejich aplikace

        public abstract class Tool
        {
            protected abstract void Apply(Point coordinates, Grid grid);        //Použije nástroj na daných souřadnicích na dané mřížce
            protected Point lastCoordinate;                                     //Poslední souřadnice, kde byl nástroj použit
            protected abstract Point GetCoordinates(PointF location);           //Spočítá z obecných souřadnic, na které bylo kliknuto souřadnice pro daný nástroj

            //Použije nástroj pokud by se tím neopakoval
            public void ApplyIfNew(PointF location, Grid grid)                  
            {
                Point c = GetCoordinates(location);
                if (lastCoordinate != c)
                {
                    ResetTool();
                    lastCoordinate = c;
                    Apply(c, grid);
                }
            }
            //Zapomene poslední souřadnici
            public void ResetTool()
            {
                lastCoordinate = new Point(-1, -1);
            }


        }

        //Nástroj zeď
        //Souřadný systém: [0,0] v levém horním rohu, každý čtvereček mé velikost 2×2
        //Souřadnice zdí jsou takové, kdy právě jedna složka je lichá
        //Spočítá se nejbližší taková, kontroluje se, zda ani na jedné straně zdi dosud nestojí most
        //Pokud ne, tak přepíná zeď na dané souřadnici
        public class WallTool : Tool
        {
            protected override Point GetCoordinates(PointF location)
            {
                int x = (int)Math.Round(location.X);
                int y = (int)Math.Round(location.Y);
                if (Math.Abs(x - location.X) + Math.Abs(y - location.Y) < 0.2f)
                {
                    return new Point(-10, -10);
                }
                var neighs = Grid.Directions.Select(i => new Point(2 * x + i.X, 2 * y + i.Y));
                return  neighs.OrderBy(i => Math.Abs(2 * location.X - i.X) + Math.Abs(2 * location.Y - i.Y)).First();
            }
            protected override void Apply(Point coords, Grid grid)
            {
                if (0 <= coords.X && coords.X <= 2 * grid.Width + 1 && 0 <= coords.Y && coords.Y <= 2 * grid.Height + 1)
                {
                    int dx = coords.Y % 2; //schválně "naopak", zajímá mě lichost v kolmém směru
                    int dy = coords.X % 2;
                    int x = coords.X - dx;
                    int y = coords.Y - dy;
                    if (Grid.Directions.All(d => !grid.Bridges[grid.ModW((x + dx * d.X)/2), grid.ModH((y + dy * d.Y)/2)]))
                    {
                        grid.Walls[grid.Mod2W(coords.X), grid.Mod2H(coords.Y)] ^= true;
                    }
                }
            }
        }


        //Nástroj most
        //Souřadný systém: [0, 0] uprostřed čverečku v levém horním rohu, každý čvereček má velikost 1×1
        //Při kliknutí na most se most zruší, jinak pokud se nekliklo na tečku a žádným směrem není most ani zeď, vloží se most
        public class BridgeTool : Tool
        {
            protected override Point GetCoordinates(PointF location)
            {
                return new Point((int)Math.Round(location.X - 0.5f), (int)Math.Round(location.Y - 0.5f));
            }
            protected override void Apply(Point coords, Grid grid)
            {
                if (0 <= coords.X && coords.X < grid.Width && 0 <= coords.Y && coords.Y < grid.Height)
                {
                    if (grid.Dots[coords.X, coords.Y] >= 0)
                    {
                        return;
                    }
                    if (grid.Bridges[coords.X, coords.Y])
                    {
                        grid.Bridges[coords.X, coords.Y] = false;
                        return;
                    }
                    if (Grid.Directions.All(d => !grid.IsBridge(coords, d) && !grid.IsWall(coords, d) ))
                    {
                        grid.Bridges[coords.X, coords.Y] = true;
                    }
                }
            }
        }


        //Nástroj tečka
        //Souřadnice stejné jako mají mosty
        //Při kliknutí na stejnou barvu se zruší, jinak pokud se nekliklo na most a není již vloženo více než jedna tečka stejné barvy, se vloží
        public class DotTool : Tool
        {
            public int ID { get; private set; }

            public DotTool(int id)
            {
                ID = id;
            }

            protected override Point GetCoordinates(PointF location)
            {
                return new Point((int)Math.Round(location.X - 0.5f), (int)Math.Round(location.Y - 0.5f));
            }
            protected override void Apply(Point coords, Grid grid)
            {
                if (0 <= coords.X && coords.X < grid.Width && 0 <= coords.Y && coords.Y < grid.Height)
                {
                    if(grid.Dots[coords.X, coords.Y] == ID)
                    {
                        grid.Dots[coords.X, coords.Y] = -1;
                        return;
                    }
                    if(grid.Bridges[coords.X, coords.Y])
                    {
                        return;
                    }
                    int same = 0;
                    for(int x = 0; x < grid.Width; x++)
                    {
                        for(int y = 0; y < grid.Height; y++)
                        {
                            if(grid.Dots[x, y] == ID)
                            {
                                same++;
                            }
                            if (same > 1)
                            {
                                return;
                            }
                        }
                    }
                    grid.Dots[coords.X, coords.Y] = ID;
                }
            }
        }
    }
}

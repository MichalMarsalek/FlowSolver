using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowSolver
{
    public partial class Solver
    {
        public class GridState
        {
            public Grid Grid { get; private set; }                  //Zadání
            public int[,] Cells { get; private set; }               //-1: prázdné pole, jinak číslo barvy která tam je, pokud jde o most, ukládá se ta ve vodorovném směru
            public int[,] Areas { get; private set; }               //Prázdná pole se stejným číslem jsou spojitelná, s jiným nikoli
            public int AreasCount { get; private set; }             //Počet oblastí
            public List<Branch> Branches { get; private set; }      //Větve řešení
            public bool Solved { get                                //Tento stav je řešením?
                {
                    return Branches.All(i => i.Solved);
                }
            }
            public IEnumerable<Branch> UnsolvedBranches             //Nevyřešené větve
            {
                get                                
                {
                    return Branches.Where(b => !b.Solved);
                }
            }

            #region Inicializace a operace s větvemi

            //Inicializace stavu
            public GridState(Grid grid)
            {
                Grid = grid;
                Cells = new int[grid.Width, grid.Height];
                Areas = new int[grid.Width, grid.Height];
                Branches = new List<Branch>();
                for (int x = 0; x < grid.Width; x++)
                {
                    for (int y = 0; y < grid.Height; y++)
                    {
                        Cells[x, y] = -1;
                        if (grid.Dots[x, y] >= 0)
                        {
                            Cells[x, y] = grid.Dots[x, y];
                            Branches.Add(new Branch(Cells[x, y]) { new Point(x, y) });
                        }
                    }
                }
                LinkBranches();
                SortBranches();
            }

            //Copy kontruktor
            public GridState(GridState old)
            {
                Grid = old.Grid;
                Cells = (int[,])old.Cells.Clone();
                Areas = (int[,])old.Areas.Clone();
                Branches = old.Branches.Select(i => new Branch(i)).ToList();
                LinkBranches();
            }

            //Ke každé větvi přiřadí tu se stejnou barvou
            private void LinkBranches()
            {
                foreach (Branch br in Branches)
                {
                    br.Other = Branches.First(i => i != br && i.Color == br.Color);
                }
            }

            //Seřadí větve podle toho jak jsou od sebe vzdáleny /podle toho kolik mají možností na pohyb
            private void SortBranches()
            {
                Branches = Branches.OrderBy(RateBranch).ToList();
            }
            private int RateBranch(Branch b)
            {
                return 10000 * Grid.Directions.Count(d => CanGoTo(b, d)) + Grid.Distance(b.Last(), b.Other.Last());
            }
            #endregion

            #region Sousední pole
            //Série metod které umožňují přejít s daného pole daným směrem bez toho aby se řešilo přetékání apod.
            private bool IsWall(Point p, Point d)
            {
                return Grid.IsWall(p, d);
            }
            private bool IsBridge(Point p, Point d)
            {
                return Grid.IsBridge(p, d);
            }
            private int CellTo(Point p, Point d)
            {
                return Cells[Grid.ModW(p.X + d.X), Grid.ModH(p.Y + d.Y)];
            }
            private int AreaTo(Point p, Point d)
            {
                return Areas[Grid.ModW(p.X + d.X), Grid.ModH(p.Y + d.Y)];
            }
            private bool IsEmpty(Point p, Point d)
            {
                return CellTo(p, d) < 0;
            }
            private bool IsEndOfDifferentColor(Point p, Point d, int color)
            {
                return false;
                Point p2 = Grid.NeigbourCoordinates(p, d);
                return Branches.Any(b => b.Color != color && b.Last() == p2);
            }
            #endregion

            #region Vynucené pohyby
            //Provede na stavu všechny vynucené pohyby
            public bool MakeForcedMoves()
            {
                while (true)
                {
                    if (UnsolvedBranches.Any(b => ForceBridgeOn(b)))
                    {
                        continue;
                    }
                    if (!UnsolvedBranches.Any(b => ForcedMove(b)))
                    {
                        break;
                    }
                    continue;
                }
                return true;
            }            

            //Provede na větvi vynucený pohyb, pokud existuje
            private bool ForcedMove(Branch b)
            {
                Point p = b.Last();
                //Spojí dvě větve stejné barvy, pokud sousedí
                foreach (Point d in Grid.Directions)
                {
                    Point onceOver = Grid.NeigbourCoordinates(p, d);
                    if (Cells[onceOver.X, onceOver.Y] == Cells[p.X, p.Y] && !b.Contains(onceOver) && !IsWall(p, d))
                    {
                        if (b.Other.Last() == onceOver)
                        {
                            b.Add(onceOver);
                            b.Solved = b.Other.Solved = true;
                            return true;
                        }
                    }
                }
                //Provede pohyb, pokud je z daného místa jediný možný
                List<Point> possibleDirs = Grid.Directions.Where(d => IsEmpty(p, d) && !IsBridge(p, d) && !IsWall(p, d)).ToList();
                if (possibleDirs.Count == 1)
                {
                    Point d = possibleDirs[0];
                    Point p1 = Grid.NeigbourCoordinates(p, d);
                    Cells[p1.X, p1.Y] = Cells[p.X, p.Y];
                    b.Add(p1);
                    ForceBridgeOn(b);
                    return true;
                }
                return false;
            }

            private bool ForceBridgeOn(Branch b)
            {
                Point p = b.Last();
                foreach (Point d in Grid.Directions)
                {
                    Point onceOver = Grid.NeigbourCoordinates(p, d);
                    Point twiceOver = Grid.NeigbourCoordinates(onceOver, d);
                    if (IsBridge(p, d) && !b.Contains(twiceOver))
                    {
                        b.Add(onceOver);
                        //Pokud jde o horizontální směr, zapamatuj si která barva je nahoře
                        if (d.Y == 0)
                        {
                            Cells[onceOver.X, onceOver.Y] = Cells[p.X, p.Y];
                        }
                        b.Add(twiceOver);
                        if (Cells[twiceOver.X, twiceOver.Y] == Cells[p.X, p.Y])
                        {
                            b.Solved = b.Other.Solved = true;
                        }
                        else
                        {
                            Cells[twiceOver.X, twiceOver.Y] = Cells[p.X, p.Y];
                        }
                        return true;
                    }
                }
                return false;
            }
            #endregion

            #region Generování podstavů
            //Generuje všechny možné stavy do kterých lze přejít
            public IEnumerable<GridState> NextStates()
            {
                if (!HeuristicsDispose())
                {
                    for (int i = 0; i < Branches.Count; i++)
                    {
                        Branch br = Branches[i];
                        if (!br.Solved)
                        {
                            List<Point> path = TryToGoAlongTheEdge(br, br.Last());
                            if (path != null)
                            {
                                yield return ConstructStateFromPath(i, path);
                            }
                        }
                    }
                    SortBranches();
                    for (int i = 0; i < Branches.Count; i++)
                    {
                        if (!Branches[i].Solved)
                        {
                            foreach (GridState s in BranchNextStates(i))
                            {
                                yield return s;
                            }
                            break;
                        }                        
                    }
                }
            }

            private IEnumerable<GridState> BranchNextStates(int i) //Musí se chodit přes index
            {
                Point be = Branches[i].Last();
                var directions = Grid.Directions.Where(d => CanGoTo(Branches[i], d)).OrderBy(d => RateDirection(Branches[i], d)).ToList();
                foreach (Point d in directions)
                {
                    Point np = Grid.NeigbourCoordinates(be, d);
                    GridState newState = new GridState(this);
                    newState.Branches[i].Add(np);
                    newState.Cells[np.X, np.Y] = Cells[be.X, be.Y];
                    newState.MakeForcedMoves();
                    yield return newState;
                }
            }

            private int RateDirection(Branch br, Point dir)
            {
                Point nxt = Grid.NeigbourCoordinates(br.Last(), dir);
                return 10000 * Grid.Directions.Count(d => CanGoTo(nxt, br, d)) + Grid.Distance(nxt, br.Other.Last());
            }            

            //Zkouší projít s danou větví po okraji (dotýkat se levou stranou)
            private List<Point> TryToGoAlongTheEdge(Branch br, Point a)
            {
                Point b = br.Other.Last();
                if (PointOnTheEdge(a) && PointOnTheEdge(b))
                {
                    List<Point> path = new List<Point>();
                    while (a != b)
                    {
                        if ((a != br.Last() && Cells[a.X, a.Y] != -1) || Grid.Directions.Any(d => IsBridge(a, d)))
                        {
                            return null;
                        }
                        Point dir = new Point();
                        if (a.X == 0 && a.Y > 0 || a.X == Grid.Width - 1 && a.Y != Grid.Height - 1)
                        {
                            dir.X = 0;
                            dir.Y = a.X == 0 ? -1 : 1;
                        }
                        else
                        {
                            dir.Y = 0;
                            dir.X = a.Y == 0 ? 1 : -1;
                        }
                        if(IsWall(a, dir) || !IsWall(a, RotateL(dir)))
                        {
                            return null;
                        }
                        if(PointInTheCorner(a) && !IsWall(a, RotateL(RotateL(dir))))
                        {
                            return null;
                        }
                        a = new Point(a.X + dir.X, a.Y + dir.Y);
                        path.Add(a);
                    }
                    return path;
                }
                return null;
            }

            private GridState ConstructStateFromPath(int i, List<Point> path)
            {
                GridState newState = new GridState(this);
                foreach (Point p in path)
                {
                    newState.Cells[p.X, p.Y] = Branches[i].Color;
                    newState.Branches[i].Add(p);
                }
                newState.Branches[i].Solved = newState.Branches[i].Other.Solved = true;
                newState.MakeForcedMoves();
                return newState;
            }

            private bool PointOnTheEdge(Point p)
            {
                return p.X == 0 || p.X == Grid.Width - 1 || p.Y == 0 || p.Y == Grid.Height-1;
            }

            private bool PointInTheCorner(Point p)
            {
                return p.X == 0 && p.Y == 0 || p.X == 0 && p.Y == Grid.Height - 1 || p.X == Grid.Width - 1 && p.Y == 0 || p.X == Grid.Width - 1 && p.Y == Grid.Height - 1;
            }

            private Point RotateR(Point d) //Right rotation
            {
                return new Point(-d.Y, d.X);
            }

            private Point RotateL(Point d) //Left rotation
            {
                return new Point(d.Y, -d.X);
            }
            #endregion

            #region Zahazování stavů
            private bool HeuristicsDispose()
            {
                return IsDeadLock() || IsSeparated() || IsTooComplicated() || IsIsolated();
            }

            //Zkotroluje, zda některá větev nemá 0 směrů kterýma se může vydat
            private bool IsBranchDeadLock(Branch br)
            {
                Point be = br.Last();
                return !Grid.Directions.Any(d => CanGoTo(be, d));
            }

            private bool IsDeadLock()
            {
                return UnsolvedBranches.Any(b => IsBranchDeadLock(b));
            }

            //Zkontroluje jestli některá branch není zakroucená tak, že by do stejného místa mohla dojít jednodušeji

            private bool IsBranchTooComplicated(Branch br)
            {
                Point be = br.Last();
                foreach (Point d in Grid.Directions)
                {
                    if (!IsWall(be, d) && CellTo(be, d) == br.Color)
                    {
                        Point meetingPoint = Grid.NeigbourCoordinates(be, d);
                        var pointsBetween = br.SkipWhile(p => p != meetingPoint);
                        if (pointsBetween.Count() > 2 && pointsBetween.All(p => !Grid.Bridges[p.X, p.Y]))
                        {
                            return true;
                        }
                    }
                }
                return false;
            }

            private bool IsTooComplicated()
            {
                return UnsolvedBranches.Any(b => IsBranchTooComplicated(b));
            }

            //Rozdělí mřížku na oblasti a zkotroluje jestli je každá dvojice konců větví stejné barvy ve stejné oblasti a zda tedy jsu spojitelné:

            //Projde všechna políčka a pokud nějaké nemá přiřazenou barvu spustí v něm MarkCellArea
            private void CreateAreas()
            {
                AreasCount = 0;
                for (int x = 0; x < Grid.Width; x++)
                {
                    for (int y = 0; y < Grid.Height; y++)
                    {
                        Areas[x, y] = -1;
                    }
                }
                for (int x = 0; x < Grid.Width; x++)
                {
                    for (int y = 0; y < Grid.Height; y++)
                    {
                        if(MarkCellArea(new Point(x, y), AreasCount)){
                            AreasCount++;
                        }
                    }
                }
            }

            //Označí pole a všechna sousedící stejným id
            private bool MarkCellArea(Point p, int id)
            {
                if (Areas[p.X, p.Y] == -1 && Cells[p.X, p.Y] == -1 && !Grid.Bridges[p.X, p.Y])
                {
                    Areas[p.X, p.Y] = id;
                    foreach (Point d in Grid.Directions)
                    {
                        if (CanGoTo(p, d))
                        {
                            Point neigbour = Grid.NeigbourCoordinates(p, d);
                            if (Grid.Bridges[neigbour.X, neigbour.Y])
                            {
                                neigbour = Grid.NeigbourCoordinates(neigbour, d);
                            }
                            MarkCellArea(neigbour, id);
                        }
                    }
                    return true;
                }
                return false;
            }
            
            //Zjistí jestli některá dvojice větví není nepropojitelná
            private bool IsBranchSeparated(Branch br)
            {
                Point b1 = br.Last();
                Point b2 = br.Other.Last();
                foreach (Point d1 in Grid.Directions)
                {
                    if (CanGoTo(br, d1)){
                        foreach (Point d2 in Grid.Directions)
                        {
                            if (CanGoTo(br.Other, d2))
                            {
                                if (AreaTo(b1, d1) == AreaTo(b2, d2))
                                {
                                    return false;
                                }
                            }
                        }
                    }
                }
                return true;
            }

            private bool IsSeparated()
            {
                CreateAreas();
                return UnsolvedBranches.Any(b => IsBranchSeparated(b));
            }


            //Zjistí, zda existuje oblast, která nesousedí s žádným koncem větve
            private bool IsIsolated()
            {
                if(Grid.Width < 9 && Grid.Height < 9)
                {
                    return false; //Aby se daly testovat nekorektní úrovně, jejichž řešení nemusí nutně zaplnit celou plochu
                }
                bool[] nonisolatedAreas = new bool[AreasCount];
                foreach(Branch br in UnsolvedBranches)
                {
                    foreach (Point d in Grid.Directions)
                    {
                        if (CanGoTo(br, d))
                        {
                            int area = AreaTo(br.Last(), d);
                            if(area != -1)
                            {
                                nonisolatedAreas[area] = true;
                            }
                        }
                    }
                }
                return nonisolatedAreas.Any(i => !i);
            }

            private bool CanGoTo(Point from, Point dir)
            {
                return IsEmpty(from, dir) && !IsWall(from, dir);
            }

            private bool CanGoTo(Branch br, Point dir)
            {
                return CanGoTo(br.Last(), br, dir);
            }

            private bool CanGoTo(Point p, Branch br, Point dir)
            {
                return (IsEmpty(p, dir) || br.Other.Last() == Grid.NeigbourCoordinates(p, dir)) && !IsWall(p, dir);
            }

            #endregion

            #region Vykreslování
            //Vykreslení
            public void Draw(Graphics g, List<Color> colors)
            {
                foreach (Branch branch in Branches)
                {
                    if (branch.Count > 1)
                    {
                        Pen pen = new Pen(colors[branch.Color], 0.3f);
                        pen.StartCap = pen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
                        Point a = branch[0];
                        for (int i = 1; i < branch.Count; i++)
                        {
                            Point b = branch[i];
                            //Ošetří případ, kdy jde o přetečení přes okraj
                            if (Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y) > 1)
                            {
                                int dx = Grid.ModW(b.X - a.X);
                                if (dx > 1) dx -= Grid.Width;
                                int dy = Grid.ModH(b.Y - a.Y);
                                if (dy > 1) dy -= Grid.Height;
                                Pen pen2 = new Pen(colors[branch.Color], 0.3f);
                                pen2.StartCap = System.Drawing.Drawing2D.LineCap.Round;
                                g.DrawLine(pen2, new PointF(a.X + 0.5f, a.Y + 0.5f), new PointF(a.X + 0.5f * dx + 0.5f, a.Y + 0.5f * dy + 0.5f));
                                g.DrawLine(pen2, new PointF(b.X + 0.5f, b.Y + 0.5f), new PointF(b.X - 0.5f * dx + 0.5f, b.Y - 0.5f * dy + 0.5f));
                            }
                            //Ostatní případy
                            else
                            {
                                g.DrawLine(pen, new PointF(a.X + 0.5f, a.Y + 0.5f), new PointF(b.X + 0.5f, b.Y + 0.5f));
                            }
                            //Překreslit horní čáru přes most
                            if(Grid.Bridges[a.X, a.Y])
                            {
                                Pen pen3 = new Pen(Color.Black, 0.3f);
                                if (Cells[a.X, a.Y] >= 0)
                                {
                                    pen3 = new Pen(colors[Cells[a.X, a.Y]], 0.3f);
                                }
                                g.DrawLine(pen3, new PointF(a.X, a.Y + 0.5f), new PointF(a.X + 1, a.Y + 0.5f));
                            }
                            a = b;
                        }
                    }
                }
                Grid.RedrawBridges(g);
            }
            #endregion

            #region Hashe, rovnost...
            //Momentálně se nepoužívá
            public override int GetHashCode()
            {
                return Cells.GetHashCode();
            }

            public override bool Equals(object obj)
            {
                if(obj == null || GetType() != obj.GetType())
                {
                    return false;
                }
                int[,] other = ((GridState)obj).Cells;
                for(int x = 0; x < Grid.Width; x++)
                {
                    for(int y = 0; y < Grid.Height; y++)
                    {
                        if(Cells[x, y] != other[x, y])
                        {
                            return false;
                        }
                    }
                }
                return true;
            }

            #endregion
            //TODO Ukládat si seznam mostů, ukládat si pole větví přístupné přes barvy

        }
    }
}

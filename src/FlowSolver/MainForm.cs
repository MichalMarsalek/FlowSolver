using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FlowSolver
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            canvas.Origin = new PointF(-0.1f, -0.1f);
            buttonEmpty_Click(null, null);
            InitTools();
            MouseWheel += MainForm_MouseWheel;
        }

        Grid grid;                  //Aktuální mřížka
        Grid.Tool tool;             //Aktuální nástroj
        List<Color> colors;         //Seznam barev
        Solver.GridState solution;  //Aktuální řešení
        Solver solver;              //Aktuální řešič

        //Požadovaná velikost
        private Size GridSize
        {
            get
            {
                return new Size((int)numericUpDownWidth.Value, (int)numericUpDownHeight.Value);
            }
        }

        //Inicializace proměnných
        private void InitTools()
        {
            string c = "Red Green Blue Yellow Orange Cyan Magenta Brown Purple White Gray Lime Salmon DarkGreen DarkBlue HotPink";
            colors = c.Split(' ').Select(i => Color.FromName(i)).ToList();
            for(int i = -2; i < colors.Count; i++)
            {

                RadioButton rb = new RadioButton();
                rb.Size = new Size(60, 40);
                flowLayoutPanelTools.Controls.Add(rb);
                rb.CheckedChanged += Rb_CheckedChanged;
                if (i == -2)
                {
                    rb.Text = "Bridge";
                    rb.Tag = new Grid.BridgeTool();
                }
                else if (i == -1)
                {
                    rb.Text = "Wall";
                    rb.Tag = new Grid.WallTool();
                }
                else
                {
                    Bitmap img = new Bitmap(40, 40);
                    Graphics g = Graphics.FromImage(img);
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    g.FillEllipse(new SolidBrush(colors[i]), 5, 5, 30, 30);
                    rb.Image = img;
                    rb.Tag = new Grid.DotTool(i);
                    if(i == 0)
                    {
                        rb.Checked = true;
                    }
                }                
            }
        }

        #region Tlačítka vytvářející novou mřížku
        private void CreatingNewGrid()
        {
            canvas.Dimensions = new SizeF(GridSize.Width + 0.2f, GridSize.Height + 0.2f);
            solver = null;
            solution = null;
            canvas.Refresh();
        }

        private void buttonEmpty_Click(object sender, EventArgs e)
        {
            grid = Grid.Empty(GridSize);
            CreatingNewGrid();
        }

        private void buttonFourWarps_Click(object sender, EventArgs e)
        {
            grid = Grid.FourWarps(GridSize);
            CreatingNewGrid();
        }

        private void buttonFourBridges_Click(object sender, EventArgs e)
        {
            grid = Grid.FourBridges(GridSize);
            CreatingNewGrid();
        }

        private void buttonOneRing_Click(object sender, EventArgs e)
        {
            grid = Grid.OneRing(GridSize);
            CreatingNewGrid();
        }

        private void buttonTwoRings_Click(object sender, EventArgs e)
        {
            grid = Grid.TwoRings(GridSize);
            CreatingNewGrid();
        }

        private void numericUpDownWidth_ValueChanged(object sender, EventArgs e)
        {
            buttonFourWarps.Enabled = !(GridSize.Width != GridSize.Height && GridSize.Width % 2 == 1 && GridSize.Height % 2 == 1);
            buttonTwoRings.Enabled = GridSize.Height > 7 && GridSize.Width > 7;
        }
        #endregion

        #region Používání nástrojů a překreslování plátna
        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (solver != null) return;
            if (e.Button == MouseButtons.Left)
            {
                tool.ApplyIfNew(canvas.MouseLocation, grid);
                canvas.Refresh();
                CheckForValidity();
            }
        }

        private void canvas_MouseDown(object sender, MouseEventArgs e)
        {
            if (solver != null) return;
            tool.ResetTool();
            tool.ApplyIfNew(canvas.MouseLocation, grid);
            canvas.Refresh();
            CheckForValidity();
        }

        private void Rb_CheckedChanged(object sender, EventArgs e)
        {
            tool = (Grid.Tool)((RadioButton)sender).Tag;
        }

        private void MainForm_MouseWheel(object sender, MouseEventArgs e)
        {
            int index = flowLayoutPanelTools.Controls.Cast<RadioButton>().TakeWhile(c => !c.Checked).Count();
            try
            {
                if (e.Delta > 0)
                {
                    ((RadioButton)flowLayoutPanelTools.Controls[index - 1]).Checked = true;
                }
                if (e.Delta < 0)
                {
                    ((RadioButton)flowLayoutPanelTools.Controls[index + 1]).Checked = true;
                }
            }
            catch (ArgumentOutOfRangeException) { }
        }

        private void canvas_Paint(object sender, PaintEventArgs e)
        {
            grid.Draw(e.Graphics, colors);
            if (solution != null)
            {
                solution.Draw(e.Graphics, colors);
            }
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            canvas.Refresh();
        }

        #endregion

        #region Tlačítka na řešení
        private void CheckForValidity()
        {
            buttonNextState.Enabled = buttonSolve.Enabled = grid.IsValid();
        }

        private void buttonSolveForced_Click(object sender, EventArgs e)
        {
            Solver.GridState s = new Solver.GridState(grid);
            s.MakeForcedMoves();
            solution = s;
            canvas.Refresh();
        }

        private void buttonNextState_Click(object sender, EventArgs e)
        {
            if (solver == null)
            {
                solver = new Solver(grid);
            }
            solution = solver.NextState();
            canvas.Refresh();
            if (solution.Solved)
            {
                buttonSolve.Enabled = buttonNextState.Enabled = false;
            }
        }

        private void buttonSolve_Click(object sender, EventArgs e)
        {
            if (solver == null)
            {
                solver = new Solver(grid);
            }
            buttonSolve.Visible = buttonNextState.Enabled = buttonResetSolver.Enabled = false;
            buttonStop.Visible = true;
            backgroundWorker.RunWorkerAsync();
        }

        private void buttonResetSolver_Click(object sender, EventArgs e)
        {
            solver = null;
            solution = null;
            canvas.Refresh();
            buttonSolve.Enabled = buttonNextState.Enabled;
            CheckForValidity();
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            solution = solver.SolveConnections(backgroundWorker);
            if (backgroundWorker.CancellationPending)
            {
                e.Cancel = true;
            }
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Text = "Flow solver " + solver.CountScenarios;
            canvas.Refresh();
            buttonSolve.Visible = buttonNextState.Enabled = buttonResetSolver.Enabled = true;
            buttonStop.Visible = false;
            if (solution.Solved)
            {
                buttonSolve.Enabled = buttonNextState.Enabled = false;
            }
            if(!e.Cancelled && !solution.Solved)
            {
                MessageBox.Show("No solution was found. Are you sure you entered the puzzle correctly?");
            }
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            backgroundWorker.CancelAsync();
        }
        #endregion

    }
}

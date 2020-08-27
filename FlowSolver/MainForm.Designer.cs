namespace FlowSolver
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonOneRing = new System.Windows.Forms.Button();
            this.buttonTwoRings = new System.Windows.Forms.Button();
            this.buttonFourBridges = new System.Windows.Forms.Button();
            this.buttonFourWarps = new System.Windows.Forms.Button();
            this.buttonEmpty = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDownHeight = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownWidth = new System.Windows.Forms.NumericUpDown();
            this.buttonSolve = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.flowLayoutPanelTools = new System.Windows.Forms.FlowLayoutPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.buttonStop = new System.Windows.Forms.Button();
            this.buttonResetSolver = new System.Windows.Forms.Button();
            this.buttonNextState = new System.Windows.Forms.Button();
            this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.canvas = new FlowSolver.SmartCanvas();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWidth)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.buttonOneRing);
            this.groupBox1.Controls.Add(this.buttonTwoRings);
            this.groupBox1.Controls.Add(this.buttonFourBridges);
            this.groupBox1.Controls.Add(this.buttonFourWarps);
            this.groupBox1.Controls.Add(this.buttonEmpty);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.numericUpDownHeight);
            this.groupBox1.Controls.Add(this.numericUpDownWidth);
            this.groupBox1.Location = new System.Drawing.Point(4, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(158, 220);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Grid";
            // 
            // buttonOneRing
            // 
            this.buttonOneRing.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOneRing.Location = new System.Drawing.Point(10, 159);
            this.buttonOneRing.Name = "buttonOneRing";
            this.buttonOneRing.Size = new System.Drawing.Size(142, 23);
            this.buttonOneRing.TabIndex = 2;
            this.buttonOneRing.Text = "One ring";
            this.buttonOneRing.UseVisualStyleBackColor = true;
            this.buttonOneRing.Click += new System.EventHandler(this.buttonOneRing_Click);
            // 
            // buttonTwoRings
            // 
            this.buttonTwoRings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonTwoRings.Enabled = false;
            this.buttonTwoRings.Location = new System.Drawing.Point(10, 191);
            this.buttonTwoRings.Name = "buttonTwoRings";
            this.buttonTwoRings.Size = new System.Drawing.Size(142, 23);
            this.buttonTwoRings.TabIndex = 2;
            this.buttonTwoRings.Text = "Two rings";
            this.buttonTwoRings.UseVisualStyleBackColor = true;
            this.buttonTwoRings.Click += new System.EventHandler(this.buttonTwoRings_Click);
            // 
            // buttonFourBridges
            // 
            this.buttonFourBridges.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonFourBridges.Location = new System.Drawing.Point(10, 130);
            this.buttonFourBridges.Name = "buttonFourBridges";
            this.buttonFourBridges.Size = new System.Drawing.Size(142, 23);
            this.buttonFourBridges.TabIndex = 2;
            this.buttonFourBridges.Text = "Four bridges";
            this.buttonFourBridges.UseVisualStyleBackColor = true;
            this.buttonFourBridges.Click += new System.EventHandler(this.buttonFourBridges_Click);
            // 
            // buttonFourWarps
            // 
            this.buttonFourWarps.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonFourWarps.Location = new System.Drawing.Point(10, 101);
            this.buttonFourWarps.Name = "buttonFourWarps";
            this.buttonFourWarps.Size = new System.Drawing.Size(142, 23);
            this.buttonFourWarps.TabIndex = 2;
            this.buttonFourWarps.Text = "Four warps";
            this.buttonFourWarps.UseVisualStyleBackColor = true;
            this.buttonFourWarps.Click += new System.EventHandler(this.buttonFourWarps_Click);
            // 
            // buttonEmpty
            // 
            this.buttonEmpty.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonEmpty.Location = new System.Drawing.Point(10, 72);
            this.buttonEmpty.Name = "buttonEmpty";
            this.buttonEmpty.Size = new System.Drawing.Size(142, 23);
            this.buttonEmpty.TabIndex = 2;
            this.buttonEmpty.Text = "Empty";
            this.buttonEmpty.UseVisualStyleBackColor = true;
            this.buttonEmpty.Click += new System.EventHandler(this.buttonEmpty_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Height";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Width";
            // 
            // numericUpDownHeight
            // 
            this.numericUpDownHeight.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDownHeight.Location = new System.Drawing.Point(57, 44);
            this.numericUpDownHeight.Maximum = new decimal(new int[] {
            13,
            0,
            0,
            0});
            this.numericUpDownHeight.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numericUpDownHeight.Name = "numericUpDownHeight";
            this.numericUpDownHeight.Size = new System.Drawing.Size(95, 20);
            this.numericUpDownHeight.TabIndex = 0;
            this.numericUpDownHeight.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numericUpDownHeight.ValueChanged += new System.EventHandler(this.numericUpDownWidth_ValueChanged);
            // 
            // numericUpDownWidth
            // 
            this.numericUpDownWidth.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDownWidth.Location = new System.Drawing.Point(57, 18);
            this.numericUpDownWidth.Maximum = new decimal(new int[] {
            13,
            0,
            0,
            0});
            this.numericUpDownWidth.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numericUpDownWidth.Name = "numericUpDownWidth";
            this.numericUpDownWidth.Size = new System.Drawing.Size(95, 20);
            this.numericUpDownWidth.TabIndex = 0;
            this.numericUpDownWidth.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numericUpDownWidth.ValueChanged += new System.EventHandler(this.numericUpDownWidth_ValueChanged);
            // 
            // buttonSolve
            // 
            this.buttonSolve.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonSolve.Location = new System.Drawing.Point(14, 735);
            this.buttonSolve.Name = "buttonSolve";
            this.buttonSolve.Size = new System.Drawing.Size(138, 23);
            this.buttonSolve.TabIndex = 2;
            this.buttonSolve.Text = "Solve";
            this.buttonSolve.UseVisualStyleBackColor = true;
            this.buttonSolve.Click += new System.EventHandler(this.buttonSolve_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox2.Controls.Add(this.flowLayoutPanelTools);
            this.groupBox2.Location = new System.Drawing.Point(5, 229);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(157, 442);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Tools";
            // 
            // flowLayoutPanelTools
            // 
            this.flowLayoutPanelTools.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanelTools.AutoScroll = true;
            this.flowLayoutPanelTools.Location = new System.Drawing.Point(6, 20);
            this.flowLayoutPanelTools.Name = "flowLayoutPanelTools";
            this.flowLayoutPanelTools.Size = new System.Drawing.Size(150, 416);
            this.flowLayoutPanelTools.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.Controls.Add(this.buttonStop);
            this.panel2.Controls.Add(this.buttonResetSolver);
            this.panel2.Controls.Add(this.groupBox2);
            this.panel2.Controls.Add(this.groupBox1);
            this.panel2.Controls.Add(this.buttonNextState);
            this.panel2.Controls.Add(this.buttonSolve);
            this.panel2.Location = new System.Drawing.Point(830, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(170, 779);
            this.panel2.TabIndex = 4;
            // 
            // buttonStop
            // 
            this.buttonStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonStop.Location = new System.Drawing.Point(14, 735);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(138, 23);
            this.buttonStop.TabIndex = 5;
            this.buttonStop.Text = "Stop";
            this.buttonStop.UseVisualStyleBackColor = true;
            this.buttonStop.Visible = false;
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
            // 
            // buttonResetSolver
            // 
            this.buttonResetSolver.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonResetSolver.Location = new System.Drawing.Point(14, 677);
            this.buttonResetSolver.Name = "buttonResetSolver";
            this.buttonResetSolver.Size = new System.Drawing.Size(138, 23);
            this.buttonResetSolver.TabIndex = 4;
            this.buttonResetSolver.Text = "Reset";
            this.buttonResetSolver.UseVisualStyleBackColor = true;
            this.buttonResetSolver.Click += new System.EventHandler(this.buttonResetSolver_Click);
            // 
            // buttonNextState
            // 
            this.buttonNextState.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonNextState.Location = new System.Drawing.Point(14, 706);
            this.buttonNextState.Name = "buttonNextState";
            this.buttonNextState.Size = new System.Drawing.Size(138, 23);
            this.buttonNextState.TabIndex = 2;
            this.buttonNextState.Text = "Next state";
            this.buttonNextState.UseVisualStyleBackColor = true;
            this.buttonNextState.Click += new System.EventHandler(this.buttonNextState_Click);
            // 
            // backgroundWorker
            // 
            this.backgroundWorker.WorkerSupportsCancellation = true;
            this.backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker_DoWork);
            this.backgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker_RunWorkerCompleted);
            // 
            // canvas
            // 
            this.canvas.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.canvas.BackColor = System.Drawing.Color.Black;
            this.canvas.Dimensions = new System.Drawing.SizeF(12F, 12F);
            this.canvas.Location = new System.Drawing.Point(2, 0);
            this.canvas.Margin = new System.Windows.Forms.Padding(0);
            this.canvas.Name = "canvas";
            this.canvas.Origin = ((System.Drawing.PointF)(resources.GetObject("canvas.Origin")));
            this.canvas.Size = new System.Drawing.Size(825, 779);
            this.canvas.TabIndex = 0;
            this.canvas.Paint += new System.Windows.Forms.PaintEventHandler(this.canvas_Paint);
            this.canvas.MouseDown += new System.Windows.Forms.MouseEventHandler(this.canvas_MouseDown);
            this.canvas.MouseMove += new System.Windows.Forms.MouseEventHandler(this.canvas_MouseMove);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 779);
            this.Controls.Add(this.canvas);
            this.Controls.Add(this.panel2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(700, 500);
            this.Name = "MainForm";
            this.Text = "Flow solver";
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWidth)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private SmartCanvas canvas;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button buttonSolve;
        private System.Windows.Forms.Button buttonEmpty;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numericUpDownHeight;
        private System.Windows.Forms.NumericUpDown numericUpDownWidth;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button buttonFourWarps;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelTools;
        private System.Windows.Forms.Button buttonNextState;
        private System.Windows.Forms.Button buttonResetSolver;
        private System.Windows.Forms.Button buttonTwoRings;
        private System.Windows.Forms.Button buttonFourBridges;
        private System.Windows.Forms.Button buttonOneRing;
        private System.ComponentModel.BackgroundWorker backgroundWorker;
        private System.Windows.Forms.Button buttonStop;
    }
}


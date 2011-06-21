namespace Sharplike.Editlike
{
	partial class Main
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
			this.FileMenu = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
			this.quitToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.editToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.undoToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.redoToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.vIewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.zToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.zToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.newMapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openMapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.quitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.redoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.splitContainer2 = new System.Windows.Forms.SplitContainer();
			this.SharplikeView = new System.Windows.Forms.PictureBox();
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
			this.toolStripButton5 = new System.Windows.Forms.ToolStripButton();
			this.splitContainer3 = new System.Windows.Forms.SplitContainer();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.EntityPage = new System.Windows.Forms.TabPage();
			this.EntityList = new System.Windows.Forms.ListView();
			this.SquaresPage = new System.Windows.Forms.TabPage();
			this.SquareList = new System.Windows.Forms.ListView();
			this.EntityProperties = new System.Windows.Forms.PropertyGrid();
			this.LoopTimer = new System.Windows.Forms.Timer(this.components);
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.DrawSquares = new System.Windows.Forms.ToolStripButton();
			this.FillSquares = new System.Windows.Forms.ToolStripButton();
			this.PointerTool = new System.Windows.Forms.ToolStripButton();
			this.CursorTool = new System.Windows.Forms.ToolStripButton();
			this.PenTool = new System.Windows.Forms.ToolStripButton();
			this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
			this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
			this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
			this.EditorTools = new System.Windows.Forms.ToolStrip();
			this.FileMenu.SuspendLayout();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.splitContainer2.Panel1.SuspendLayout();
			this.splitContainer2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.SharplikeView)).BeginInit();
			this.toolStrip1.SuspendLayout();
			this.splitContainer3.Panel1.SuspendLayout();
			this.splitContainer3.Panel2.SuspendLayout();
			this.splitContainer3.SuspendLayout();
			this.tabControl1.SuspendLayout();
			this.EntityPage.SuspendLayout();
			this.SquaresPage.SuspendLayout();
			this.SuspendLayout();
			// 
			// FileMenu
			// 
			this.FileMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem1,
            this.editToolStripMenuItem1,
            this.vIewToolStripMenuItem});
			this.FileMenu.Location = new System.Drawing.Point(0, 0);
			this.FileMenu.Name = "FileMenu";
			this.FileMenu.Size = new System.Drawing.Size(976, 24);
			this.FileMenu.TabIndex = 0;
			this.FileMenu.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem1
			// 
			this.fileToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.toolStripSeparator3,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.toolStripSeparator4,
            this.quitToolStripMenuItem1});
			this.fileToolStripMenuItem1.Name = "fileToolStripMenuItem1";
			this.fileToolStripMenuItem1.Size = new System.Drawing.Size(37, 20);
			this.fileToolStripMenuItem1.Text = "&File";
			// 
			// newToolStripMenuItem
			// 
			this.newToolStripMenuItem.Name = "newToolStripMenuItem";
			this.newToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
			this.newToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
			this.newToolStripMenuItem.Text = "&New";
			this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
			// 
			// openToolStripMenuItem
			// 
			this.openToolStripMenuItem.Name = "openToolStripMenuItem";
			this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
			this.openToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
			this.openToolStripMenuItem.Text = "&Open";
			this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(183, 6);
			// 
			// saveToolStripMenuItem
			// 
			this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
			this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
			this.saveToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
			this.saveToolStripMenuItem.Text = "&Save";
			this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
			// 
			// saveAsToolStripMenuItem
			// 
			this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
			this.saveAsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift)
						| System.Windows.Forms.Keys.S)));
			this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
			this.saveAsToolStripMenuItem.Text = "Save &As";
			this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
			// 
			// toolStripSeparator4
			// 
			this.toolStripSeparator4.Name = "toolStripSeparator4";
			this.toolStripSeparator4.Size = new System.Drawing.Size(183, 6);
			// 
			// quitToolStripMenuItem1
			// 
			this.quitToolStripMenuItem1.Name = "quitToolStripMenuItem1";
			this.quitToolStripMenuItem1.Size = new System.Drawing.Size(186, 22);
			this.quitToolStripMenuItem1.Text = "Quit";
			// 
			// editToolStripMenuItem1
			// 
			this.editToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.undoToolStripMenuItem1,
            this.redoToolStripMenuItem1});
			this.editToolStripMenuItem1.Name = "editToolStripMenuItem1";
			this.editToolStripMenuItem1.Size = new System.Drawing.Size(39, 20);
			this.editToolStripMenuItem1.Text = "Edit";
			// 
			// undoToolStripMenuItem1
			// 
			this.undoToolStripMenuItem1.Name = "undoToolStripMenuItem1";
			this.undoToolStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
			this.undoToolStripMenuItem1.Size = new System.Drawing.Size(144, 22);
			this.undoToolStripMenuItem1.Text = "Undo";
			this.undoToolStripMenuItem1.Click += new System.EventHandler(this.undoToolStripMenuItem1_Click);
			// 
			// redoToolStripMenuItem1
			// 
			this.redoToolStripMenuItem1.Name = "redoToolStripMenuItem1";
			this.redoToolStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y)));
			this.redoToolStripMenuItem1.Size = new System.Drawing.Size(144, 22);
			this.redoToolStripMenuItem1.Text = "Redo";
			this.redoToolStripMenuItem1.Click += new System.EventHandler(this.redoToolStripMenuItem1_Click);
			// 
			// vIewToolStripMenuItem
			// 
			this.vIewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.zToolStripMenuItem,
            this.zToolStripMenuItem1});
			this.vIewToolStripMenuItem.Name = "vIewToolStripMenuItem";
			this.vIewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
			this.vIewToolStripMenuItem.Text = "View";
			// 
			// zToolStripMenuItem
			// 
			this.zToolStripMenuItem.Name = "zToolStripMenuItem";
			this.zToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Up)));
			this.zToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
			this.zToolStripMenuItem.Text = "+Z";
			this.zToolStripMenuItem.Click += new System.EventHandler(this.zToolStripMenuItem_Click);
			// 
			// zToolStripMenuItem1
			// 
			this.zToolStripMenuItem1.Name = "zToolStripMenuItem1";
			this.zToolStripMenuItem1.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Down)));
			this.zToolStripMenuItem1.Size = new System.Drawing.Size(151, 22);
			this.zToolStripMenuItem1.Text = "-Z";
			this.zToolStripMenuItem1.Click += new System.EventHandler(this.zToolStripMenuItem1_Click);
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newMapToolStripMenuItem,
            this.openMapToolStripMenuItem,
            this.toolStripSeparator1,
            this.quitToolStripMenuItem});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
			this.fileToolStripMenuItem.Text = "&File";
			// 
			// newMapToolStripMenuItem
			// 
			this.newMapToolStripMenuItem.Name = "newMapToolStripMenuItem";
			this.newMapToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
			this.newMapToolStripMenuItem.Text = "&New Map";
			// 
			// openMapToolStripMenuItem
			// 
			this.openMapToolStripMenuItem.Name = "openMapToolStripMenuItem";
			this.openMapToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
			this.openMapToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
			this.openMapToolStripMenuItem.Text = "&Open Map";
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(167, 6);
			// 
			// quitToolStripMenuItem
			// 
			this.quitToolStripMenuItem.Name = "quitToolStripMenuItem";
			this.quitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Q)));
			this.quitToolStripMenuItem.Size = new System.Drawing.Size(170, 22);
			this.quitToolStripMenuItem.Text = "&Quit";
			// 
			// editToolStripMenuItem
			// 
			this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.undoToolStripMenuItem,
            this.redoToolStripMenuItem});
			this.editToolStripMenuItem.Name = "editToolStripMenuItem";
			this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
			this.editToolStripMenuItem.Text = "&Edit";
			// 
			// undoToolStripMenuItem
			// 
			this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
			this.undoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
			this.undoToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
			this.undoToolStripMenuItem.Text = "&Undo";
			// 
			// redoToolStripMenuItem
			// 
			this.redoToolStripMenuItem.Name = "redoToolStripMenuItem";
			this.redoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
			this.redoToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
			this.redoToolStripMenuItem.Text = "&Redo";
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
			this.splitContainer1.IsSplitterFixed = true;
			this.splitContainer1.Location = new System.Drawing.Point(0, 49);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.splitContainer3);
			this.splitContainer1.Size = new System.Drawing.Size(976, 466);
			this.splitContainer1.SplitterDistance = 640;
			this.splitContainer1.TabIndex = 1;
			// 
			// splitContainer2
			// 
			this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
			this.splitContainer2.IsSplitterFixed = true;
			this.splitContainer2.Location = new System.Drawing.Point(0, 0);
			this.splitContainer2.Name = "splitContainer2";
			this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer2.Panel1
			// 
			this.splitContainer2.Panel1.Controls.Add(this.SharplikeView);
			this.splitContainer2.Panel1.Controls.Add(this.toolStrip1);
			this.splitContainer2.Size = new System.Drawing.Size(640, 466);
			this.splitContainer2.SplitterDistance = 324;
			this.splitContainer2.TabIndex = 0;
			// 
			// SharplikeView
			// 
			this.SharplikeView.BackColor = System.Drawing.SystemColors.ControlDark;
			this.SharplikeView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.SharplikeView.Location = new System.Drawing.Point(0, 25);
			this.SharplikeView.MinimumSize = new System.Drawing.Size(640, 300);
			this.SharplikeView.Name = "SharplikeView";
			this.SharplikeView.Size = new System.Drawing.Size(640, 300);
			this.SharplikeView.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			this.SharplikeView.TabIndex = 0;
			this.SharplikeView.TabStop = false;
			// 
			// toolStrip1
			// 
			this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton4,
            this.toolStripButton5});
			this.toolStrip1.Location = new System.Drawing.Point(0, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
			this.toolStrip1.Size = new System.Drawing.Size(640, 25);
			this.toolStrip1.TabIndex = 1;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// toolStripButton4
			// 
			this.toolStripButton4.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton4.Image")));
			this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton4.Name = "toolStripButton4";
			this.toolStripButton4.Size = new System.Drawing.Size(39, 22);
			this.toolStripButton4.Text = "-Z";
			this.toolStripButton4.Click += new System.EventHandler(this.toolStripButton4_Click);
			// 
			// toolStripButton5
			// 
			this.toolStripButton5.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton5.Image")));
			this.toolStripButton5.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton5.Name = "toolStripButton5";
			this.toolStripButton5.Size = new System.Drawing.Size(42, 22);
			this.toolStripButton5.Text = "+Z";
			this.toolStripButton5.Click += new System.EventHandler(this.toolStripButton5_Click);
			// 
			// splitContainer3
			// 
			this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer3.Location = new System.Drawing.Point(0, 0);
			this.splitContainer3.Name = "splitContainer3";
			// 
			// splitContainer3.Panel1
			// 
			this.splitContainer3.Panel1.Controls.Add(this.tabControl1);
			// 
			// splitContainer3.Panel2
			// 
			this.splitContainer3.Panel2.Controls.Add(this.EntityProperties);
			this.splitContainer3.Size = new System.Drawing.Size(332, 466);
			this.splitContainer3.SplitterDistance = 179;
			this.splitContainer3.TabIndex = 0;
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.EntityPage);
			this.tabControl1.Controls.Add(this.SquaresPage);
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl1.Location = new System.Drawing.Point(0, 0);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(179, 466);
			this.tabControl1.TabIndex = 0;
			// 
			// EntityPage
			// 
			this.EntityPage.Controls.Add(this.EntityList);
			this.EntityPage.Location = new System.Drawing.Point(4, 22);
			this.EntityPage.Name = "EntityPage";
			this.EntityPage.Padding = new System.Windows.Forms.Padding(3);
			this.EntityPage.Size = new System.Drawing.Size(171, 440);
			this.EntityPage.TabIndex = 0;
			this.EntityPage.Text = "Entities";
			this.EntityPage.UseVisualStyleBackColor = true;
			// 
			// EntityList
			// 
			this.EntityList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.EntityList.Location = new System.Drawing.Point(3, 3);
			this.EntityList.MultiSelect = false;
			this.EntityList.Name = "EntityList";
			this.EntityList.Size = new System.Drawing.Size(165, 434);
			this.EntityList.TabIndex = 0;
			this.EntityList.UseCompatibleStateImageBehavior = false;
			this.EntityList.View = System.Windows.Forms.View.SmallIcon;
			// 
			// SquaresPage
			// 
			this.SquaresPage.Controls.Add(this.SquareList);
			this.SquaresPage.Location = new System.Drawing.Point(4, 22);
			this.SquaresPage.Name = "SquaresPage";
			this.SquaresPage.Padding = new System.Windows.Forms.Padding(3);
			this.SquaresPage.Size = new System.Drawing.Size(171, 440);
			this.SquaresPage.TabIndex = 1;
			this.SquaresPage.Text = "Squares";
			this.SquaresPage.UseVisualStyleBackColor = true;
			// 
			// SquareList
			// 
			this.SquareList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.SquareList.Location = new System.Drawing.Point(3, 3);
			this.SquareList.MultiSelect = false;
			this.SquareList.Name = "SquareList";
			this.SquareList.Size = new System.Drawing.Size(165, 434);
			this.SquareList.TabIndex = 0;
			this.SquareList.UseCompatibleStateImageBehavior = false;
			this.SquareList.View = System.Windows.Forms.View.SmallIcon;
			// 
			// EntityProperties
			// 
			this.EntityProperties.Dock = System.Windows.Forms.DockStyle.Fill;
			this.EntityProperties.Location = new System.Drawing.Point(0, 0);
			this.EntityProperties.Name = "EntityProperties";
			this.EntityProperties.Size = new System.Drawing.Size(149, 466);
			this.EntityProperties.TabIndex = 0;
			// 
			// LoopTimer
			// 
			this.LoopTimer.Enabled = true;
			this.LoopTimer.Interval = 150;
			this.LoopTimer.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
			// 
			// DrawSquares
			// 
			this.DrawSquares.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.DrawSquares.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.DrawSquares.Name = "DrawSquares";
			this.DrawSquares.Size = new System.Drawing.Size(23, 22);
			this.DrawSquares.Text = "toolStripButton1";
			this.DrawSquares.ToolTipText = "Pen Tool";
			// 
			// FillSquares
			// 
			this.FillSquares.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.FillSquares.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.FillSquares.Name = "FillSquares";
			this.FillSquares.Size = new System.Drawing.Size(23, 22);
			this.FillSquares.Text = "toolStripButton2";
			this.FillSquares.ToolTipText = "Fill Tool";
			// 
			// PointerTool
			// 
			this.PointerTool.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.PointerTool.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.PointerTool.Name = "PointerTool";
			this.PointerTool.Size = new System.Drawing.Size(23, 22);
			this.PointerTool.Text = "toolStripButton1";
			this.PointerTool.ToolTipText = "Pointer Tool";
			// 
			// CursorTool
			// 
			this.CursorTool.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.CursorTool.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.CursorTool.Name = "CursorTool";
			this.CursorTool.Size = new System.Drawing.Size(23, 22);
			this.CursorTool.Text = "toolStripButton1";
			this.CursorTool.ToolTipText = "Cursor Tool";
			// 
			// PenTool
			// 
			this.PenTool.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.PenTool.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.PenTool.Name = "PenTool";
			this.PenTool.Size = new System.Drawing.Size(23, 22);
			this.PenTool.Text = "toolStripButton1";
			this.PenTool.ToolTipText = "Pen Tool";
			// 
			// toolStripButton2
			// 
			this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton2.Name = "toolStripButton2";
			this.toolStripButton2.Size = new System.Drawing.Size(23, 22);
			this.toolStripButton2.Text = "Fill Tool";
			// 
			// toolStripButton1
			// 
			this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton1.Name = "toolStripButton1";
			this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
			this.toolStripButton1.Text = "toolStripButton1";
			// 
			// toolStripButton3
			// 
			this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButton3.Name = "toolStripButton3";
			this.toolStripButton3.Size = new System.Drawing.Size(23, 22);
			this.toolStripButton3.Text = "toolStripButton3";
			// 
			// EditorTools
			// 
			this.EditorTools.Location = new System.Drawing.Point(0, 24);
			this.EditorTools.Name = "EditorTools";
			this.EditorTools.Size = new System.Drawing.Size(976, 25);
			this.EditorTools.TabIndex = 2;
			this.EditorTools.Text = "toolStrip1";
			// 
			// Main
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(976, 515);
			this.Controls.Add(this.splitContainer1);
			this.Controls.Add(this.EditorTools);
			this.Controls.Add(this.FileMenu);
			this.MainMenuStrip = this.FileMenu;
			this.MinimumSize = new System.Drawing.Size(700, 400);
			this.Name = "Main";
			this.Text = "@# Map Editor";
			this.FileMenu.ResumeLayout(false);
			this.FileMenu.PerformLayout();
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.ResumeLayout(false);
			this.splitContainer2.Panel1.ResumeLayout(false);
			this.splitContainer2.Panel1.PerformLayout();
			this.splitContainer2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.SharplikeView)).EndInit();
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.splitContainer3.Panel1.ResumeLayout(false);
			this.splitContainer3.Panel2.ResumeLayout(false);
			this.splitContainer3.ResumeLayout(false);
			this.tabControl1.ResumeLayout(false);
			this.EntityPage.ResumeLayout(false);
			this.SquaresPage.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.MenuStrip FileMenu;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.SplitContainer splitContainer2;
		private System.Windows.Forms.SplitContainer splitContainer3;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem newMapToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem openMapToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem quitToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem undoToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem redoToolStripMenuItem;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage EntityPage;
		private System.Windows.Forms.TabPage SquaresPage;
		private System.Windows.Forms.PictureBox SharplikeView;
		private System.Windows.Forms.ListView EntityList;
		private System.Windows.Forms.ListView SquareList;
		private System.Windows.Forms.Timer LoopTimer;
		private System.Windows.Forms.ToolStripButton DrawSquares;
		private System.Windows.Forms.ToolStripButton FillSquares;
		private System.Windows.Forms.ToolStripButton PointerTool;
		private System.Windows.Forms.ToolStripButton CursorTool;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripButton PenTool;
		private System.Windows.Forms.ToolStripButton toolStripButton2;
		private System.Windows.Forms.ToolStripButton toolStripButton1;
		private System.Windows.Forms.ToolStripButton toolStripButton3;
		private System.Windows.Forms.ToolStrip EditorTools;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
		private System.Windows.Forms.ToolStripMenuItem quitToolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem undoToolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem redoToolStripMenuItem1;
		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.ToolStripButton toolStripButton4;
		private System.Windows.Forms.ToolStripButton toolStripButton5;
		private System.Windows.Forms.ToolStripMenuItem vIewToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem zToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem zToolStripMenuItem1;
		public System.Windows.Forms.PropertyGrid EntityProperties;
	}
}


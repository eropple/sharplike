using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Windows.Forms;
using Sharplike.Core;
using Sharplike.Core.Input;
using Sharplike.Core.Rendering;
using Sharplike.Core.Runtime;
using Sharplike.Core.ControlFlow;
using Sharplike.UI;
using Sharplike.UI.Controls;
using Sharplike.Mapping;
using Sharplike.Mapping.Entities;
using Sharplike.Editlike.MapTools;
using Mono.Addins;
using UndoStack;

namespace Sharplike.Editlike
{
	public partial class Main : Form
	{
		private AbstractWindow window;

		public AbstractMap Map;
		private IMapTool currentTool;
		private IMapTool viewTool;

		public UndoStack.UndoStack UndoRedo
		{
			get;
			private set;
		}
		
		public Main()
		{
			InitializeComponent();

			this.FormClosed += new FormClosedEventHandler(Main_FormClosed);
			this.Load += new EventHandler(Main_Load);

			//SharplikeView.Paint += new PaintEventHandler(SharplikeView_Paint);
		}


		Point ScreenToTile(Point p)
		{
			Size glyphSize = Game.RenderSystem.Window.GlyphPalette.GlyphDimensions;
			return new Point(p.X / glyphSize.Width, p.Y / glyphSize.Height);
		}

		void SharplikeView_MouseDown(object sender, MouseEventArgs e)
		{
			SharplikeView.Controls[0].Capture = true;

			if (e.Button == MouseButtons.Right)
				viewTool.Start(ScreenToTile(e.Location));
			else if (currentTool != null)
				currentTool.Start(ScreenToTile(e.Location));
		}

		void SharplikeView_MouseUp(object sender, MouseEventArgs e)
		{
			if (!SharplikeView.Controls[0].Capture)
				return;

			SharplikeView.Controls[0].Capture = false;

			if (e.Button == MouseButtons.Right)
				viewTool.End(ScreenToTile(e.Location));
			else if (currentTool != null)
				currentTool.End(ScreenToTile(e.Location));
		}

		void SharplikeView_MouseMove(object sender, MouseEventArgs e)
		{
			if (SharplikeView.Controls[0].Capture)
			{
				if (e.Button == System.Windows.Forms.MouseButtons.Right)
					viewTool.Run(ScreenToTile(e.Location));
				else if (currentTool != null)
					currentTool.Run(ScreenToTile(e.Location));
			}
		}

		#region Entity Drag/Drop
		void EntityList_ItemDrag(object sender, ItemDragEventArgs e)
		{
			EntityList.DoDragDrop((e.Item as ListViewItem).Tag, DragDropEffects.All);
		}

		void Main_DragOver(object sender, DragEventArgs e)
		{
			Point tile = ScreenToTile(SharplikeView.PointToClient(new Point(e.X, e.Y)));
			AbstractSquare sq = Map.GetSafeSquare(new Vector3(tile.X + Map.View.x, tile.Y + Map.View.y, Map.View.z));
			if (sq != null && sq.IsPassable(Direction.Here))
			{
				e.Effect = DragDropEffects.Copy;
			}
			else
			{
				e.Effect = DragDropEffects.None;
			}
		}

		void Main_DragDrop(object sender, DragEventArgs e)
		{
			Point tile = ScreenToTile(SharplikeView.PointToClient(new Point(e.X, e.Y)));

			Vector3 maploc = new Vector3(tile.X + Map.View.x, tile.Y + Map.View.y, Map.View.z);
			AbstractSquare sq = Map.GetSafeSquare(maploc);
			if (sq != null && sq.IsPassable(Direction.Here))
			{
				EditorExtensionNode node = e.Data.GetData(typeof(EditorExtensionNode)) as EditorExtensionNode;
				AbstractEntity ent = node.CreateInstance() as AbstractEntity;

				ent.Location = maploc;
				ent.Map = Map;
			}
		}

		void Main_DragEnter(object sender, DragEventArgs e)
		{
		}

		void Main_DragLeave(object sender, EventArgs e)
		{
		}
		#endregion

		void Main_Load(object sender, EventArgs e)
		{
			Game.Initialize();
			Game.SetRenderSystem("OpenTK");

			String glyphPath = Game.PathTo("curses_640x300.png");
			using (Stream imgstream = File.OpenRead(glyphPath))
			{
				GlyphPalette pal = new GlyphPalette(imgstream, 16, 16);

				window = Game.RenderSystem.CreateWindow(SharplikeView.Size, pal, SharplikeView);
			}

			SharplikeView.Controls[0].MouseDown += new MouseEventHandler(SharplikeView_MouseDown);
			SharplikeView.Controls[0].MouseUp += new MouseEventHandler(SharplikeView_MouseUp);
			SharplikeView.Controls[0].MouseMove += new MouseEventHandler(SharplikeView_MouseMove);

			EntityList.ItemDrag += new ItemDragEventHandler(EntityList_ItemDrag);
			SharplikeView.Controls[0].AllowDrop = true;
			SharplikeView.Controls[0].DragDrop += new DragEventHandler(Main_DragDrop);
			SharplikeView.Controls[0].DragOver += new DragEventHandler(Main_DragOver);
			SharplikeView.Controls[0].DragEnter += new DragEventHandler(Main_DragEnter);
			SharplikeView.Controls[0].DragLeave += new EventHandler(Main_DragLeave);
			
			//Game.SetInputSystem("OpenTK");

			window.Clear();

			ReplaceMap(new MapStack(window.Size, 20, 15, "EditorMap"));
			Map.ViewFrom(new Vector3(0, 0, 0), true);

			Bitmap glyphs = Game.RenderSystem.Window.GlyphPalette.SourceBitmap;
			ImageList il = new ImageList();
			Size glyphSize = Game.RenderSystem.Window.GlyphPalette.GlyphDimensions;

			for (int y = 0; y < Game.RenderSystem.Window.GlyphPalette.RowCount; ++y)
			{
				for (int x = 0; x < Game.RenderSystem.Window.GlyphPalette.ColumnCount; ++x)
				{
					Rectangle area = new Rectangle(x * glyphSize.Width, y * glyphSize.Height, 
						glyphSize.Width, glyphSize.Height);
					Bitmap b = new Bitmap(glyphSize.Width, glyphSize.Height, glyphs.PixelFormat);
					using (Graphics bg = Graphics.FromImage(b))
					{
						bg.Clear(Color.Black);
						bg.DrawImageUnscaled(glyphs.Clone(area, glyphs.PixelFormat), new Point(0, 0));
					}
					il.Images.Add(b);
				}
			}
			EntityList.LargeImageList = il;
			EntityList.SmallImageList = il;

			SquareList.LargeImageList = il;
			SquareList.SmallImageList = il;

			foreach (EditorExtensionNode node in AddinManager.GetExtensionNodes("/Sharplike/Entities"))
			{
				ListViewItem i = new ListViewItem();
				i.Text = node.Id;
				i.ToolTipText = node.TooltipText;
				i.Tag = node;
				i.ImageIndex = node.GlyphID;

				EntityList.Items.Add(i);
			}

			foreach (EditorExtensionNode node in AddinManager.GetExtensionNodes("/Sharplike/Squares"))
			{
				ListViewItem i = new ListViewItem();
				i.Text = node.Id;
				i.ToolTipText = node.TooltipText;
				i.Tag = node;
				i.ImageIndex = node.GlyphID;
				
				SquareList.Items.Add(i);
			}

			foreach (ToolGroupExtensionNode node in AddinManager.GetExtensionNodes("/Sharplike/Editlike/Tools"))
			{
				foreach (ExtensionNode mapnode in node.ChildNodes)
				{
					if (mapnode.GetType() == typeof(MapToolExtensionNode))
					{
						ToolStripButton btn = new ToolStripButton();
						BuildButton(mapnode as MapToolExtensionNode, btn);
						EditorTools.Items.Add(btn);
					}
					else
					{
						ToolStripDropDownButton ddbtn = new ToolStripDropDownButton();
						ddbtn.DropDown.Width = 200;
						foreach (MapToolExtensionNode mnode in mapnode.ChildNodes)
						{

							ToolStripButton btn = new ToolStripButton();
							BuildButton(mnode, btn);
							if (btn.DisplayStyle == ToolStripItemDisplayStyle.Image)
								btn.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;

							Image i = mnode.Icon;
							btn.Click += delegate(object send, EventArgs ea)
							{
								ddbtn.Image = i;
								ddbtn.Tag = btn;
							};

							ddbtn.DropDownItems.Add(btn);

							if (ddbtn.Tag == null)
							{
								ddbtn.Tag = btn;
								ddbtn.Image = mnode.Icon;
							}
						}

						ddbtn.Click += delegate(object send, EventArgs ea)
						{
							btn_Click(ddbtn.Tag, ea);
						};

						EditorTools.Items.Add(ddbtn);
					}
				}
				EditorTools.Items.Add(new ToolStripSeparator());
			}

			viewTool = new ViewportTool();
			viewTool.SetActive(this, "");

			Game.Run();
		}

		private void BuildButton(MapToolExtensionNode mnode, ToolStripButton btn)
		{
			try
			{
				btn.Image = mnode.Icon;
				btn.Text = mnode.Id;
				btn.ToolTipText = mnode.Tooltip;
				btn.Tag = mnode;
				btn.AutoToolTip = true;
				if (btn.Image == null)
					btn.DisplayStyle = ToolStripItemDisplayStyle.Text;
				else
					btn.DisplayStyle = ToolStripItemDisplayStyle.Image;

				btn.Click += new EventHandler(btn_Click);
			}
			catch (Exception ex)
			{
				Console.WriteLine("Could not load tool named {0}: {1}", mnode.Id, ex.Message);
			}
		}

		public EditorExtensionNode SelectedSquareType()
		{
			if (SquareList.SelectedItems.Count == 0)
				return null;

			return (EditorExtensionNode)SquareList.SelectedItems[0].Tag;
		}

		public EditorExtensionNode SelectedEntityType()
		{
			if (EntityList.SelectedItems.Count == 0)
				return null;

			return (EditorExtensionNode)EntityList.SelectedItems[0].Tag;
		}

		void btn_Click(object sender, EventArgs e)
		{
			ToolStripButton btn = sender as ToolStripButton;
			MapToolExtensionNode node = btn.Tag as MapToolExtensionNode;

			if (currentTool != null)
				currentTool.SetInactive();

			currentTool = node.Tool;

			if (currentTool != null)
				currentTool.SetActive(this, node.Tag);
		}

		void SharplikeView_Paint(object sender, PaintEventArgs e)
		{
			window.Update();
		}

		void Main_FormClosed(object sender, FormClosedEventArgs e)
		{
			Game.Stop();
			Game.Terminate();
		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			Game.Process();
		}

		private void ReplaceMap(AbstractMap map)
		{
			if (this.Map != null)
			{
				window.RemoveRegion(this.Map);
				this.Map.Dispose();
			}

			this.Map = map;
			this.UndoRedo = new UndoStack.UndoStack();

			if (this.Map != null)
			{
				this.Map.Location = new Point(0, 0);
				this.Map.Size = window.Size;
				window.AddRegion(this.Map);
				this.Map.ViewFrom(this.Map.View, true);
			}
			window.InvalidateTiles();
		}

		private void newToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ReplaceMap(new MapStack(window.Size, 20, 15, "EditorMap"));
		}

		private void openToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OpenFileDialog fd = new OpenFileDialog();
			fd.InitialDirectory = Game.BaseDirectory;
			fd.Filter = "Sharplike Maps (*.map)|*.map|All Files (*.*)|*.*";
			fd.RestoreDirectory = true;
			fd.Multiselect = false;

			if (fd.ShowDialog() == DialogResult.OK)
			{
				currentFile = fd.FileName;
				using (Stream fs = fd.OpenFile())
				{
					ReplaceMap(AbstractMap.Deserialize<MapStack>(fs));
				}
			}
		}

		private String currentFile;

		private void saveToolStripMenuItem_Click(object sender, EventArgs e)
		{
			using (Stream fs = new FileStream(currentFile, FileMode.Create))
			{
				Map.Serialize(fs);
			}
		}

		private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SaveFileDialog fd = new SaveFileDialog();
			fd.InitialDirectory = Game.BaseDirectory;
			fd.Filter = "Sharplike Maps (*.map)|*.map|All Files (*.*)|*.*";
			fd.FileName = "Untitled.map";
			if (fd.ShowDialog() == DialogResult.OK)
			{
				currentFile = fd.FileName;
				using (Stream fs = fd.OpenFile())
				{
					Map.Serialize(fs);
				}
			}
		}

		private void undoToolStripMenuItem1_Click(object sender, EventArgs e)
		{
			UndoRedo.Undo();
		}

		private void redoToolStripMenuItem1_Click(object sender, EventArgs e)
		{
			UndoRedo.Redo();
		}

		private void toolStripButton4_Click(object sender, EventArgs e)
		{
			Map.ViewFrom(Map.View + Vector3.Down);
		}

		private void toolStripButton5_Click(object sender, EventArgs e)
		{
			Map.ViewFrom(Map.View + Vector3.Up);
		}

		private void zToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Map.ViewFrom(Map.View + Vector3.Up);

		}

		private void zToolStripMenuItem1_Click(object sender, EventArgs e)
		{
			Map.ViewFrom(Map.View + Vector3.Down);
		}
	}
}

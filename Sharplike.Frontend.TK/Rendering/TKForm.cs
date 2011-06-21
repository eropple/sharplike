///////////////////////////////////////////////////////////////////////////////
/// Sharplike, The Open Roguelike Library (C) 2010 Ed Ropple.               ///
///                                                                         ///
/// This code is part of the Sharplike Roguelike library, and is licensed   ///
/// under the Common Public Attribution License (CPAL), version 1.0. Use of ///
/// this code is purusant to this license. The CPAL grants you certain      ///
/// permissions and requirements and should be read carefully before using  ///
/// this library.                                                           ///
///                                                                         ///
/// A copy of this license can be found in the Sharplike root directory,    ///
/// and must be included with all projects released using this library.     ///
///////////////////////////////////////////////////////////////////////////////

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Sharplike.Frontend.Rendering;

namespace Sharplike.Frontend.Rendering
{
	public delegate void TKDisplayFunc();
	public delegate void TKResizeFunc(int Width, int Height);

	/// <summary>
	/// The form on which TKWindow shall render.
	/// </summary>
	public class TKForm : Form
	{
		TKDisplayFunc LoadFunc;
		TKDisplayFunc RenderFunc;
		TKResizeFunc ResizeFunc;

		public TKForm(TKDisplayFunc load, TKDisplayFunc render, TKResizeFunc resize)
		{
			LoadFunc = load;
			RenderFunc = render;
			ResizeFunc = resize;

			this.glControl1 = new TKGLControl();
			this.SuspendLayout();
			this.glControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.glControl1.BackColor = System.Drawing.Color.Green;
			this.glControl1.Location = new System.Drawing.Point(0, 0);
			this.glControl1.Name = "glControl1";
			this.glControl1.Size = new System.Drawing.Size(629, 565);
			this.glControl1.TabIndex = 0;
			this.glControl1.VSync = false;
			this.glControl1.Resize += new System.EventHandler(this.glControl1_Resize);
			this.glControl1.Paint += new System.Windows.Forms.PaintEventHandler(this.glControl1_Paint);
			this.glControl1.TabStop = false;
			// 
			// W01_First_Window
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.KeyPreview = true;
			this.Controls.Add(this.glControl1);
			this.Name = "W01_First_Window";
			this.ResumeLayout(false);
			this.FormBorderStyle = FormBorderStyle.FixedSingle;
			this.ResumeLayout(false);
		}
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
		}

		public OpenTK.GLControl glControl1;


		#region Events

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			glControl1_Resize(this, EventArgs.Empty);   // Ensure the Viewport is set up correctly

			LoadFunc();

		}

		private void glControl1_Paint(object sender, PaintEventArgs e)
		{
			glControl1.MakeCurrent();
			//	   GL.Clear(ClearBufferMask.ColorBufferBit);

			RenderFunc();

			glControl1.SwapBuffers();
		}

		private void glControl1_Resize(object sender, EventArgs e)
		{
			if (glControl1.ClientSize.Height == 0)
				glControl1.ClientSize = new System.Drawing.Size(glControl1.ClientSize.Width, 1);

			GL.Viewport(0, 0, glControl1.ClientSize.Width, glControl1.ClientSize.Height);

			ResizeFunc(glControl1.ClientSize.Width, glControl1.ClientSize.Height);
		}
		#endregion

		private void InitializeComponent()
		{
			this.SuspendLayout();
			// 
			// TKForm
			// 
			this.ClientSize = new System.Drawing.Size(284, 262);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.Name = "TKForm";
			this.ResumeLayout(false);

		}


	}
}

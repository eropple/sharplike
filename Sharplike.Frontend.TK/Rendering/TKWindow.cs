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
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Sharplike.Core;
using Sharplike.Core.Rendering;

namespace Sharplike.Frontend.Rendering
{
	/// <summary>
	/// A display window that uses OpenGL (via OpenTK) to render the game display.
	/// </summary>
	/// <remarks>
	/// Developed primarily by Alex Karantza (karantza@notsorandom.com), with some
	/// adjustments to fit into the application. Copyright transferred with permission
	/// to Ed Ropple and code included under the Sharplike License (CPAL).
	/// </remarks>
	public class TKWindow : AbstractWindow
	{

        public TKForm Form
        {
            get;
            private set;
        }
		private int paletteId;

		public TKWindow(Size displayDimensions, GlyphPalette palette)
			: base(displayDimensions, palette)
		{

			Form = new TKForm(new TKDisplayFunc(Setup),
									 new TKDisplayFunc(Render),
									 new TKResizeFunc(Resize)
									 );
			Form.FormBorderStyle = FormBorderStyle.FixedSingle;
			Form.MaximizeBox = false;
			Form.ClientSize = displayDimensions;
			Form.FormClosing += new FormClosingEventHandler(Form_FormClosing);
			Form.Show();

			paletteId = GL.GenTexture();
			Bitmap bmp = palette.SourceBitmap;
			GL.BindTexture(TextureTarget.Texture2D, paletteId);
			BitmapData bmp_data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

			GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bmp_data.Width, bmp_data.Height, 0,
				OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bmp_data.Scan0);

			bmp.UnlockBits(bmp_data);

			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
		}

		void Form_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (Game.InputSystem.HasWindowEvent("OnClosing"))
			{
				e.Cancel = true;
			}
			Game.InputSystem.WindowCommand("OnClosing");
		}

		private void Setup()
		{
			GL.ClearColor(0.1f, 0.2f, 0.5f, 0.0f);
			GL.Disable(EnableCap.DepthTest);
			GL.Disable(EnableCap.CullFace);
		}

		private new void Resize(int Width, int Height)
		{

            Int32 tileCols = this.DisplayDimensions.Width / this.GlyphPalette.GlyphDimensions.Width;
            Int32 tileRows = this.DisplayDimensions.Height / this.GlyphPalette.GlyphDimensions.Height;

            base.Size = new Size(tileCols, tileRows);
			GL.MatrixMode(MatrixMode.Projection);
			GL.LoadIdentity();
			GL.Ortho(0, Width, Height, 0, 0, 1);
		}



		private void Render()
		{
			GL.MatrixMode(MatrixMode.Modelview);

			GL.LoadIdentity();

			Int32 w = this.GlyphPalette.GlyphDimensions.Width;
			Int32 h = this.GlyphPalette.GlyphDimensions.Height;

			for (Int32 x = 0; x < this.TileDimensions.Width; x++)
			{
				for (Int32 y = 0; y < this.TileDimensions.Height; y++)
				{
					DisplayTile tile = this.tiles[x, y];

					DrawTile(tile, x, y);
					tile.MarkRenderClean();
					
				}
			}
		}

		private void DrawTile(DisplayTile tile, int x, int y)
		{

			Int32 w = this.GlyphPalette.GlyphDimensions.Width;
			Int32 h = this.GlyphPalette.GlyphDimensions.Height;
			x *= w;
			y *= h;

			GL.Enable(EnableCap.Blend);
			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

            List<IGlyphProvider> gproviders = new List<IGlyphProvider>();
            foreach (RegionTile t in tile.RegionTiles)
            {
                foreach (IGlyphProvider p in t.GlyphProviders)
                    gproviders.Add(p);
            }

            foreach (IGlyphProvider glyphpro in gproviders)
			{
				GL.Begin(BeginMode.Quads);
				GL.Color4(glyphpro.BackgroundColor);
				GL.Vertex2(x, y);
				GL.Vertex2(x + w, y);
				GL.Vertex2(x + w, h + y);
				GL.Vertex2(x, h + y);
				GL.End();
				
				foreach (Glyph glyph in glyphpro.Glyphs)
				{
					int uvrow = glyph.Index / GlyphPalette.ColumnCount;
					int uvcol = glyph.Index % GlyphPalette.ColumnCount;
	
					double u = (double)uvcol / (double)GlyphPalette.ColumnCount;
					double v = (double)uvrow / (double)GlyphPalette.RowCount;
					double du = 1.0 / (double)GlyphPalette.ColumnCount;
					double dv = 1.0 / (double)GlyphPalette.RowCount;
					
					GL.Enable(EnableCap.Texture2D);
					GL.BindTexture(TextureTarget.Texture2D, paletteId);
	
					GL.Begin(BeginMode.Quads);
					GL.Color4(glyph.Color);
					GL.TexCoord2(u, v); GL.Vertex2(x, y);
					GL.TexCoord2(du + u, v); GL.Vertex2(x + w, y);
					GL.TexCoord2(du + u, dv + v); GL.Vertex2(x + w, h + y);
					GL.TexCoord2(u, dv + v); GL.Vertex2(x, h + y);
					GL.End();
					GL.Disable(EnableCap.Texture2D);
				}
			}
			GL.Disable(EnableCap.Blend);

		}

		protected override void WindowTitleChange()
		{
			Form.Text = this.windowTitle.ToString();
		}

		public override void Update()
		{
            base.Update();
			Form.glControl1.Refresh();
			Application.DoEvents();
		}
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

using Sharplike.Core;
using Sharplike.Core.ControlFlow;
using Sharplike.Core.Rendering;
using Sharplike.Core.Input;
using Sharplike.UI;
using Sharplike.UI.Controls;


namespace Sharplike.Demos.Sharphack.State
{
	public class MainMenuState : AbstractState
	{
		AbstractWindow Window;

		Window dialogBox;

		public MainMenuState()
			: base()
		{
			Window = Game.RenderSystem.Window;

			dialogBox = new Window(new Size(40, 40), new Point(30, 5));
			dialogBox.Style = BorderStyle.Double;
			dialogBox.ForegroundColor = Color.DarkGray;

			Window.AddRegion(dialogBox);
		}

		protected override void GameLoopTick(Core.Runtime.AbstractGameLoop loop)
		{
			Window.InvalidateTiles();
		}
	}
}

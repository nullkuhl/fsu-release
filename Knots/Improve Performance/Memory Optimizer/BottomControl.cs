﻿using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace MemoryOptimizer
{
	/// <summary>
	/// Bottom control
	/// </summary>
	public partial class BottomControl : UserControl
	{
		/// <summary>
		/// constructor for BottomControl
		/// </summary>
		public BottomControl()
		{
			InitializeComponent();
			SetImage();
		}

		/// <summary>
		/// change image for BottomControl
		/// </summary>
		void SetImage()
		{
			Image image = Properties.Resources.bottomBar;
			try
			{
				string path = Path.GetDirectoryName(GetType().Assembly.Location) + "\\Skins\\blue\\bottomblue.png";
				if (File.Exists(path))				
					image = Image.FromFile(path);				
			}
			catch (Exception)
			{
			}

			lblMain.Image = image;
		}
	}
}

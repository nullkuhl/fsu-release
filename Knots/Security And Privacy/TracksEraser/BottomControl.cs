﻿using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using FreemiumUtilities.TracksEraser.Properties;

namespace FreemiumUtilities.TracksEraser
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
        /// change BottomControl image
        /// </summary>
        void SetImage()
        {
            Image image = Resources.bottomBar;
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
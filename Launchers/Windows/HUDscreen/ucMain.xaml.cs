using System;
using System.Xaml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
//using d = System.Drawing;
using io= System.IO;

namespace DirOnline
{
    /// <summary>
    /// Interaction logic for ucMain.xaml
    /// </summary>
    public partial class ucMain : UserControl
    {
        public App Owner { get; set; }
        public string rootPath = io.Path.Combine(io.Directory.GetParent(io.Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.FullName);
        public string assetPath = io.Path.Combine(io.Directory.GetParent(io.Directory.GetCurrentDirectory()).Parent.Parent.Parent.Parent.FullName, "Content\\Assets\\UI");
        public string AssetPath { get { return assetPath; } }

        public ucMain()
        {
            InitializeComponent();
            imgTile.ImageSource = new BitmapImage(new Uri(assetPath + "\\6-01-200x200.png"));
            this.KeyDown += UcMain_KeyDown;
        }

        private void UcMain_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                // At this case u used the control and cant close the UI
                Owner.ucHost.Visible = !Owner.uiVisiblity;
                Owner.Form.Focus();
            }
        }
    }
}

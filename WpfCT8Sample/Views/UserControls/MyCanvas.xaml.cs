using System;
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

namespace WpfCT8Sample.Views.UserControls
{
    /// <summary>
    /// MyCanvas.xaml の相互作用ロジック
    /// </summary>
    public partial class MyCanvas : UserControl
    {
        // 画像の解像度をViewModelに伝えるための依存関係プロパティ
        // 現在の画像の表示サイズ（縮尺）
        public static readonly DependencyProperty ActualSizeProperty =
            DependencyProperty.Register("ActualSize", typeof(Size), typeof(MyCanvas), new PropertyMetadata(default(Size)));

        public Size ActualSize
        {
            get => (Size)GetValue(ActualSizeProperty);
            set => SetValue(ActualSizeProperty, value);
        }

        // 元の画像のサイズ
        public static readonly DependencyProperty SourceSizeProperty =
            DependencyProperty.Register("SourceSize", typeof(Size), typeof(MyCanvas), new PropertyMetadata(default(Size)));

        public Size SourceSize
        {
            get => (Size)GetValue(SourceSizeProperty);
            set => SetValue(SourceSizeProperty, value);
        }

        public MyCanvas()
        {
            InitializeComponent();
        }

        // 画面の拡大縮小時に画像のサイズを取得する
        private void OnImageSizeChanged(object sender, SizeChangedEventArgs e)
        {
            var image = (Image)sender;
            ActualSize = new Size(image.ActualWidth, image.ActualHeight);
            if (image.Source != null)
            {
                SourceSize = new Size(image.Source.Width, image.Source.Height);
            }
        }
    }
}

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WpfCT8Sample.ViewModels.UserControls
{
    internal partial class MyCanvasViewModel : ObservableObject
    {
        [ObservableProperty]
        public Uri _imageUri;
        [ObservableProperty]
        private Point _position = new(100, 100);
        [ObservableProperty]
        private Size _size = new(100, 100);
        [ObservableProperty]
        private double _rotation = 0;
        [ObservableProperty]
        private double _zoom = 1;

        // MyCanvas.xaml.csのActualSizeとSourceSizeに対応するプロパティ
        [ObservableProperty]
        private Size _sourceSize;
        [ObservableProperty]
        private Size _actualSize;

        partial void OnActualSizeChanged(Size oldValue, Size newValue)
        {
            if (SourceSize == default)
            {
                return;
            }
            // ActualSizeの比率をSizeに反映させる
            double scaleX = newValue.Width / oldValue.Width;
            double scaleY = newValue.Height / oldValue.Height;
            Size = new Size(Size.Width * scaleX, Size.Height * scaleY);
            //Position = new Point(Position.X * scaleX, Position.Y * scaleY);
            var offset = new Point(newValue.Width - oldValue.Width, newValue.Height - oldValue.Height);
            Position = new Point(Position.X + offset.X, Position.Y + offset.Y);
        }
        // 提出はBoundingBoxのxamlとcs、それとこのファイル

        // BoundingBox座標系のポリゴン（描画用）
        [ObservableProperty]
        private PathGeometry _boundingBoxGeometry = new();

        // 画像の座標系のポリゴン（確認用、使用していない）
        [ObservableProperty]
        private PathGeometry _imageGeometry = new();

        private Polygon? BoundingBoxPolygon { get; set; }

        // 画像座標系のポリゴン（これが今回欲しい結果）
        private Polygon? ImagePolygon { get; set; }

        public MyCanvasViewModel()
        {
            ImageUri = new Uri("D:\\sample.png", UriKind.Absolute);         // TODO:ここで画像のパスを指定してください
        }

        // BoundingBoxのポリゴンを作成し、更に画像の座標系に変換する
        [RelayCommand]
        public void MakePolygon()
        {
            BoundingBoxPolygon = CreatePolygonFromBoundingBox();

            // BoundingBoxの場所にBoundingBoxの範囲を示す描画用ポリゴンを作成
            BoundingBoxGeometry = PolygonToPathGeometry(BoundingBoxPolygon);

            // 画像の座標系に変換
            ImageCoordinate();
        }

        // BoundingBoxの座標系を画像の座標系に変換する
        [RelayCommand]
        public void ImageCoordinate()
        {
            if (BoundingBoxPolygon == null)
            {
                return;
            }
            ConvertPolygonToImageCoordinate();
            ImageGeometry = PolygonToPathGeometry(ImagePolygon!);

            // ImagePolygon.Pointsの各座標をMessageBoxで表示する
            var points = ImagePolygon!.Points.Select(p => $"({p.X}, {p.Y})");
            MessageBox.Show("求める画像の範囲（4頂点の座標）:\n" + string.Join(Environment.NewLine, points));
        }

        public Polygon CreatePolygonFromBoundingBox()
        {
            // 四角形の各頂点座標を計算する。  
            var topLeft = new Point(-Size.Width / 2, -Size.Height / 2);
            var topRight = new Point(Size.Width / 2, -Size.Height / 2);
            var bottomRight = new Point(Size.Width / 2, Size.Height / 2);
            var bottomLeft = new Point(-Size.Width / 2, Size.Height / 2);

            // 回転用の変換を作成し、既存の頂点に適用する。  
            var rotateTransform = new RotateTransform(Rotation);

            topLeft = rotateTransform.Transform(topLeft);
            topRight = rotateTransform.Transform(topRight);
            bottomRight = rotateTransform.Transform(bottomRight);
            bottomLeft = rotateTransform.Transform(bottomLeft);

            // 位置を原位置にシフトする変換を作成し、既存の頂点に適用する。  
            var translateTransform = new TranslateTransform(Position.X + Size.Width / 2, Position.Y + Size.Height / 2);

            topLeft = translateTransform.Transform(topLeft);
            topRight = translateTransform.Transform(topRight);
            bottomRight = translateTransform.Transform(bottomRight);
            bottomLeft = translateTransform.Transform(bottomLeft);

            var polygon = new Polygon();

            // 各頂点座標をポリゴンポイントコレクションに追加する。  
            var points = polygon.Points;
            points.Add(topLeft);
            points.Add(topRight);
            points.Add(bottomRight);
            points.Add(bottomLeft);

            return polygon;
        }

        // BoundingBoxの場所にBoundingBoxの範囲を示す描画用ポリゴンを作成
        private PathGeometry PolygonToPathGeometry(Polygon polygon)
        {
            var geometry = new PathGeometry();
            var figure = new PathFigure { StartPoint = polygon.Points[0] };
            figure.Segments.Add(new PolyLineSegment(polygon.Points, true));
            geometry.Figures.Add(figure);
            return geometry;
        }

        // BoundingBoxの座標系を画像の座標系に変換する
        public void ConvertPolygonToImageCoordinate()
        {
            if (BoundingBoxPolygon == null)
            {
                return;
            }
            // 実際の画像サイズとソース画像サイズの比率を計算する  
            var scaleX = ActualSize.Width / SourceSize.Width;
            var scaleY = ActualSize.Height / SourceSize.Height;

            // 新しいPolygonを作成する  
            var newPolygon = new Polygon();

            // 既存のPolygonの各点を新しい座標系に変換する  
            foreach (var point in BoundingBoxPolygon.Points)
            {
                var x = point.X / scaleX;
                var y = point.Y / scaleY;
                newPolygon.Points.Add(new Point(x, y));
            }

            ImagePolygon = newPolygon;
        }

    }
}

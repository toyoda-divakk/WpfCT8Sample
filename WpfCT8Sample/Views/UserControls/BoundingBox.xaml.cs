using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// BoundingBox.xaml の相互作用ロジック
    /// </summary>
    public partial class BoundingBox : UserControl, INotifyPropertyChanged
    {
        public static readonly DependencyProperty TopLeftPositionProperty = DependencyProperty.Register("TopLeftPosition", typeof(Point), typeof(BoundingBox), new PropertyMetadata(default(Point)));

        public Point TopLeftPosition
        {
            get => (Point)GetValue(TopLeftPositionProperty);
            set => SetValue(TopLeftPositionProperty, value);
        }

        // Center Position
        public static readonly DependencyProperty PositionProperty = DependencyProperty.Register("Position", typeof(Point), typeof(BoundingBox), new PropertyMetadata(default(Point), OnPositionChanged));


        public Point Position
        {
            get => (Point)GetValue(PositionProperty);
            set
            {
                SetValue(TopLeftPositionProperty, new Point(value.X - CenterX, value.Y - CenterY));
                SetValue(PositionProperty, value);
            }
        }

        public static readonly DependencyProperty SizeProperty = DependencyProperty.Register("Size", typeof(Size), typeof(BoundingBox), new PropertyMetadata(new Size(100, 100), OnSizeOrZoomChanged));

        public Size Size
        {
            get => (Size)GetValue(SizeProperty);
            set => SetValue(SizeProperty, value);
        }

        public static readonly DependencyProperty RotationProperty = DependencyProperty.Register("Rotation", typeof(double), typeof(BoundingBox), new PropertyMetadata(default(double)));

        public double Rotation  // degree
        {
            get => (double)GetValue(RotationProperty);
            set => SetValue(RotationProperty, value);
        }

        public double RotationRadian { get; set; }

        public static readonly DependencyProperty ZoomProperty = DependencyProperty.Register("Zoom", typeof(double), typeof(BoundingBox), new PropertyMetadata(1.0, OnSizeOrZoomChanged));

        public double Zoom
        {
            get => (double)GetValue(ZoomProperty);
            set => SetValue(ZoomProperty, value);
        }

        public double CenterX => (Size.Width / 2) * Zoom;
        public double CenterY => (Size.Height / 2) * Zoom;

        private static void OnSizeOrZoomChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is BoundingBox boundingBox)
            {
                boundingBox.OnPropertyChanged(nameof(CenterX));
                boundingBox.OnPropertyChanged(nameof(CenterY));
            }
        }

        private static void OnPositionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var box = (BoundingBox)d;
            box.TopLeftPosition = new Point(box.Position.X - box.CenterX, box.Position.Y - box.CenterY);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public BoundingBox()
        {
            InitializeComponent();
        }

        private Point _startPoint;

        private void BoundingBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released)
                return;

            var currentPoint = e.GetPosition((IInputElement)Parent);
            var offset = currentPoint - _startPoint;
            _startPoint = currentPoint;

            Position = new Point(Position.X + offset.X, Position.Y + offset.Y);
        }

        private void BoundingBox_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _startPoint = e.GetPosition((IInputElement)Parent);
            Mouse.Capture(null);
        }


        private void DragThumb_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Mouse.Capture(null);
        }

        #region Drag four corners

        private void DragTopLeft_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _startPoint = e.GetPosition((IInputElement)Parent);
            Mouse.Capture(dragTopLeft);
        }

        private void UpdateSizeAndPosition(Vector delta, int widthMultiplier, int heightMultiplier)
        {
            // 回転角度をラジアンで取得
            var angle = RotationRadian;

            // ドラッグ量を回転を考慮して変換
            var cos = Math.Cos(angle);
            var sin = Math.Sin(angle);
            var rotatedDeltaX = (delta.X * cos + delta.Y * sin) / Zoom;
            var rotatedDeltaY = (-delta.X * sin + delta.Y * cos) / Zoom;

            // 新しいサイズを計算
            var newWidth = Math.Max(Size.Width + widthMultiplier * rotatedDeltaX, 1);
            var newHeight = Math.Max(Size.Height + heightMultiplier * rotatedDeltaY, 1);

            // 中心位置の変化量を計算（ドラッグ量の半分）
            var deltaCenterX = rotatedDeltaX / 2;
            var deltaCenterY = rotatedDeltaY / 2;

            // 中心位置を更新
            var newX = Position.X;
            var newY = Position.Y;

            if (widthMultiplier != 0)
            {
                newX += (deltaCenterX * cos - deltaCenterY * sin) * Zoom;
            }

            if (heightMultiplier != 0)
            {
                newY += (deltaCenterX * sin + deltaCenterY * cos) * Zoom;
            }

            Position = new Point(newX, newY);
            Size = new Size(newWidth, newHeight);
        }

        private Vector GetDelta(MouseEventArgs e)
        {
            var currentPoint = e.GetPosition((IInputElement)Parent);
            var delta = currentPoint - _startPoint;
            _startPoint = currentPoint;
            return delta;
        }

        private void DragTopLeft_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released)
                return;

            UpdateSizeAndPosition(GetDelta(e), -1, -1);
        }

        private void DragBottomRight_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _startPoint = e.GetPosition((IInputElement)Parent);
            Mouse.Capture(dragBottomRight);
        }

        private void DragBottomRight_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released)
                return;

            UpdateSizeAndPosition(GetDelta(e), 1, 1);
        }


        private void DragTopRight_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _startPoint = e.GetPosition((IInputElement)Parent);
            Mouse.Capture(dragTopRight);
        }

        private void DragTopRight_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released)
                return;

            UpdateSizeAndPosition(GetDelta(e), 1, -1);
        }

        private void DragBottomLeft_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _startPoint = e.GetPosition((IInputElement)Parent);
            Mouse.Capture(dragBottomLeft);
        }

        private void DragBottomLeft_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released)
                return;

            UpdateSizeAndPosition(GetDelta(e), -1, 1);
        }
        #endregion

        #region Drag four sides
        private void DragTop_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _startPoint = e.GetPosition((IInputElement)Parent);
            Mouse.Capture(dragTop);
        }

        private void DragTop_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released)
                return;

            UpdateSizeAndPosition(GetDelta(e), 0, -1);
        }

        private void DragBottom_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _startPoint = e.GetPosition((IInputElement)Parent);
            Mouse.Capture(dragBottom);
        }

        private void DragBottom_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released)
                return;

            UpdateSizeAndPosition(GetDelta(e), 0, 1);
        }

        private void DragLeft_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _startPoint = e.GetPosition((IInputElement)Parent);
            Mouse.Capture(dragLeft);
        }

        private void DragLeft_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released)
                return;

            UpdateSizeAndPosition(GetDelta(e), -1, 0);
        }

        private void DragRight_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _startPoint = e.GetPosition((IInputElement)Parent);
            Mouse.Capture(dragRight);
        }

        private void DragRight_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released)
                return;

            UpdateSizeAndPosition(GetDelta(e), 1, 0);
        }
        #endregion

        #region Rotation

        private void RotationHandler_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _startPoint = e.GetPosition((IInputElement)Parent);
            Mouse.Capture(rotationHandler);
        }

        private void RotationHandler_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released)
                return;

            var currentPoint = e.GetPosition((IInputElement)Parent);
            var center = Position;
            double angle = Math.Atan2(currentPoint.Y - center.Y, currentPoint.X - center.X) + Math.PI / 2;
            Rotation = angle * 180 / Math.PI;
            RotationRadian = angle;
        }

        #endregion

    }
}

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
    /// BoundingBox.xaml の相互作用ロジック
    /// </summary>
    public partial class BoundingBox : UserControl
    {
        public static readonly DependencyProperty PositionProperty = DependencyProperty.Register("Position", typeof(Point), typeof(BoundingBox), new PropertyMetadata(default(Point)));

        public Point Position
        {
            get => (Point)GetValue(PositionProperty);
            set => SetValue(PositionProperty, value);
        }

        public static readonly DependencyProperty SizeProperty = DependencyProperty.Register("Size", typeof(Size), typeof(BoundingBox), new PropertyMetadata(new Size(100, 100)));

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

        private void DragTopLeft_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released)
                return;

            var currentPoint = e.GetPosition((IInputElement)Parent);
            var offset = currentPoint - _startPoint;
            _startPoint = currentPoint;

            var newWidth = Math.Max(Size.Width - offset.X, 0);
            var newX = Math.Min(Position.X + offset.X, Position.X + Size.Width);

            var newHeight = Math.Max(Size.Height - offset.Y, 0);
            var newY = Math.Min(Position.Y + offset.Y, Position.Y + Size.Height);

            Size = new Size(newWidth, newHeight);
            Position = new Point(newX, newY);
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

            var currentPoint = e.GetPosition((IInputElement)Parent);
            var offset = currentPoint - _startPoint;
            _startPoint = currentPoint;

            Size = new Size(Math.Max(Size.Width + offset.X, 0), Math.Max(Size.Height + offset.Y, 0));
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

            var currentPoint = e.GetPosition((IInputElement)Parent);
            var offset = currentPoint - _startPoint;
            _startPoint = currentPoint;

            var newHeight = Math.Max(Size.Height - offset.Y, 0);
            var newY = Math.Min(Position.Y + offset.Y, Position.Y + Size.Height);

            Size = new Size(Math.Max(Size.Width + offset.X, 0), newHeight);
            Position = new Point(Position.X, newY);
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

            var currentPoint = e.GetPosition((IInputElement)Parent);
            var offset = currentPoint - _startPoint;
            _startPoint = currentPoint;

            var newWidth = Math.Max(Size.Width - offset.X, 0);
            var newX = Math.Min(Position.X + offset.X, Position.X + Size.Width);

            Size = new Size(newWidth, Math.Max(Size.Height + offset.Y, 0));
            Position = new Point(newX, Position.Y);
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

            var currentPoint = e.GetPosition((IInputElement)Parent);
            var offset = currentPoint - _startPoint;
            _startPoint = currentPoint;

            var newHeight = Math.Max(Size.Height - offset.Y, 0);
            var newY = Math.Min(Position.Y + offset.Y, Position.Y + Size.Height);

            Size = new Size(Size.Width, newHeight);
            Position = new Point(Position.X, newY);
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

            var currentPoint = e.GetPosition((IInputElement)Parent);
            var offset = currentPoint - _startPoint;
            _startPoint = currentPoint;

            Size = new Size(Size.Width, Math.Max(Size.Height + offset.Y, 0));
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

            var currentPoint = e.GetPosition((IInputElement)Parent);
            var offset = currentPoint - _startPoint;
            _startPoint = currentPoint;

            var newWidth = Math.Max(Size.Width - offset.X, 0);
            var newX = Math.Min(Position.X + offset.X, Position.X + Size.Width);

            Size = new Size(newWidth, Size.Height);
            Position = new Point(newX, Position.Y);
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

            var currentPoint = e.GetPosition((IInputElement)Parent);
            var offset = currentPoint - _startPoint;
            _startPoint = currentPoint;

            Size = new Size(Math.Max(Size.Width + offset.X, 0), Size.Height);
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
            var centerX = Position.X + Size.Width / 2;
            var centerY = Position.Y + Size.Height / 2;
            double angle = Math.Atan2(currentPoint.Y - centerY, currentPoint.X - centerX) + Math.PI / 2;
            Rotation = angle * 180 / Math.PI;
        }
        #endregion

    }
}

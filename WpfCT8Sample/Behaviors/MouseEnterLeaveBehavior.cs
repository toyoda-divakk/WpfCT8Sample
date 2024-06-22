using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;
using Microsoft.Xaml.Behaviors;
using System.Windows.Controls;

namespace WpfCT8Sample.Behaviors
{
    /// <summary>
    /// マウスが要素上に入ったときと出たときの処理を行うビヘイビア
    /// </summary>
    public class MouseEnterLeaveBehavior : Behavior<Button>   // FrameworkElement
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            // マウスイベントのハンドラを登録
            AssociatedObject.MouseEnter += OnMouseEnter;
            AssociatedObject.MouseLeave += OnMouseLeave;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            // マウスイベントのハンドラを解除
            AssociatedObject.MouseEnter -= OnMouseEnter;
            AssociatedObject.MouseLeave -= OnMouseLeave;
        }

        private void OnMouseEnter(object sender, MouseEventArgs e)
        {
            // マウスが要素上に入ったときの処理
            // 例: 背景色を変更する
            var element = sender as Button;
            if (element != null)
            {
                element.Background = new SolidColorBrush(Colors.LightBlue);
            }
        }

        private void OnMouseLeave(object sender, MouseEventArgs e)
        {
            // マウスが要素から出たときの処理
            // 例: 背景色を元に戻す
            var element = sender as Button;
            if (element != null)
            {
                element.Background = new SolidColorBrush(Colors.Transparent);
            }
        }
    }
}

using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace WpfCT8Sample.Behaviors
{
    /// <summary>
    /// BehaviorからViewModelのコマンドやメソッドを呼び出すようなことはできますか？
    /// </summary>
    public class CommandBehavior : Behavior<FrameworkElement>
    {
        // 依存関係プロパティを定義
        public static readonly DependencyProperty CommandProperty =
        DependencyProperty.Register("Command", typeof(ICommand), typeof(CommandBehavior), new PropertyMetadata(null));

        // このようにして依存関係プロパティにアクセスする
        // これで、xamlとViewModelのコマンドを関連付けて、ここからそのコマンドを呼び出すことができる
        /// <summary>
        /// コマンドを取得または設定します。
        /// </summary>
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            // 例えば、クリックイベントにコマンドを実行するロジックを追加
            this.AssociatedObject.MouseLeftButtonDown += (sender, e) =>
            {
                if (Command != null && Command.CanExecute(null))
                {
                    Command.Execute(null);
                }
            };
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            // 必要に応じてイベントハンドラを解除
        }
    }
}

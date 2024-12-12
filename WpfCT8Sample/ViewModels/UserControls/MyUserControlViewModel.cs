using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfCT8Sample.ViewModels.UserControls
{
    internal partial class MyUserControlViewModel : ObservableObject
    {
        [ObservableProperty]
        private MyCanvasViewModel _myCanvasViewModel;

        [ObservableProperty]
        public string _testButtonText;

        public MyUserControlViewModel()
        {
            TestButtonText = "Click Me";
            MyCanvasViewModel = new MyCanvasViewModel();
        }

        [RelayCommand]
        public void TestButton()
        {
            TestButtonText = "Test Button Clicked";
        }

        [RelayCommand]
        public void TestButtonFromBehavior()
        {
            TestButtonText = "Test From Behavior";
        }



    }
}

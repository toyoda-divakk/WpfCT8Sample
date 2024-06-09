using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfCT8Sample.ViewModels.UserControls;

namespace WpfCT8Sample.ViewModels
{
    internal partial class MainPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private MyUserControlViewModel _userControlViewModel;

        [ObservableProperty]
        private string _testButtonText;


        public MainPageViewModel()
        {
            TestButtonText = "Test Button";
            UserControlViewModel = new MyUserControlViewModel();
        }
    }
}

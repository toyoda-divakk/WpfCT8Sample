using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfCT8Sample.ViewModels
{
    internal partial class MainPageViewModel : ObservableObject
    {
        [ObservableProperty]
        public string _testButtonText;

        public MainPageViewModel()
        {
            TestButtonText = "Test Button";
        }
    }
}

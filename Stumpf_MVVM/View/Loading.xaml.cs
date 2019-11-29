using Stumpf_MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Stumpf_MVVM.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Loading : ContentPage
    {
        public LoadingViewModel viewmodel;
        public Loading()
        {
            InitializeComponent();
            viewmodel = new LoadingViewModel();
            BindingContext = viewmodel;
        }
    }
}
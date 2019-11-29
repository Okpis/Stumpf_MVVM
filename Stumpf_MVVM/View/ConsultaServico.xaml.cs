
using Stumpf_MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Stumpf_MVVM.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]

    public partial class ConsultaServico : ContentPage
    {
        public ConsultaServicoViewModel viewmodel;


        public ConsultaServico()
        {
            InitializeComponent();
            viewmodel = new ConsultaServicoViewModel();

            BindingContext = viewmodel;

            
            


        }
      
    }


   
}
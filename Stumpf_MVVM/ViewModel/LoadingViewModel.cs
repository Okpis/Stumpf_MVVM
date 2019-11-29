using Stumpf_MVVM.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stumpf_MVVM.ViewModel
{
    public class LoadingViewModel
    {
        public string LoadingText { get; set; }
        public LoadingViewModel()
        {
            
          
           
                LoadingText = "Conectando ao Servidor e carregando funcionários e serviços...";
            
            
        }
    }
}

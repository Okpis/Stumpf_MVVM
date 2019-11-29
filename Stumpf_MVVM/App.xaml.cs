using Npgsql;
using Stumpf_MVVM.View;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Stumpf_MVVM.Model;


namespace Stumpf_MVVM
{
    public partial class App : Application
    {
        
        public App()
        {
            InitializeComponent();
           // MainPage = new Loading();
           
        }

        protected override async void OnStart()
        {
            MainPage = new Loading();
            bool isConnected = await loading();
            if (isConnected)
            {
                await CarregaConsultaServico();
            }
       
           
            // Handle when your app starts

            // Just a simulation with 10 tries to get the data



        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        private async Task<bool> loading()
        {

            // shows Loading...
            bool isConnected = false;
            await Task.Run(() =>
             {
                 
                 try
                 {
                     NpgsqlConnection conn = ConsultaServicoModel.Conn();
                     conn.Open();
                     conn.Close();
                     isConnected = true;
                 }
                 catch (Exception ex)
                 {
                     isConnected = false;
                     Device.BeginInvokeOnMainThread(async () =>
                     {
                         await MainPage.DisplayAlert(
                         "Erro de Conexão",
                         "Não foi possível conectar ao servidor: " + ex.Message,
                         "Tentar Novamente");
                          OnStart();
                        
                     });

                 }

             });
            return isConnected;
        }
        private async Task CarregaConsultaServico()
        {
            
            await Task.Run(() =>
            {
                // after loading is complete show the real page
                Device.BeginInvokeOnMainThread(() => MainPage = new NavigationPage(new ConsultaServico()));
            });
        }
    }

}

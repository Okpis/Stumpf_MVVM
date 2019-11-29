using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Stumpf_MVVM.Droid.SQLite;
using Stumpf_MVVM.Model;
using System.IO;
using Xamarin.Forms;

[assembly: Dependency(typeof(Caminho))]
namespace Stumpf_MVVM.Droid.SQLite
{
    public class Caminho : ICaminho
    {
        public string ObterCaminho(string NomeArquivoBanco)
        {
            string caminhoDaPasta = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);

            string caminhoBanco = Path.Combine(caminhoDaPasta, NomeArquivoBanco);

            return caminhoBanco;
        }

    }
}
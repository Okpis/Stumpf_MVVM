using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Stumpf_MVVM.Model;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter.Distribute;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace Stumpf_MVVM.Droid
{
    [Activity(Label = "Stumpf_MVVM", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            Distribute.ReleaseAvailable = OnReleaseAvailable;
            
            AppCenter.Start("64372b5d-28e1-46d9-8269-47d281932dad",
                   typeof(Analytics), typeof(Crashes));
            AppCenter.Start("64372b5d-28e1-46d9-8269-47d281932dad",
                               typeof(Analytics), typeof(Crashes));
            AppCenter.Start("{Your Xamarin Android App Secret}", typeof(Distribute));
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
        }

        bool OnReleaseAvailable(ReleaseDetails releaseDetails)
        {
            // Look at releaseDetails public properties to get version information, release notes text or release notes URL
            string versionName = releaseDetails.ShortVersion;
            string versionCodeOrBuildNumber = releaseDetails.Version;
            string releaseNotes = releaseDetails.ReleaseNotes;
            Uri releaseNotesUrl = releaseDetails.ReleaseNotesUrl;
            //TESTE
            // custom dialog
            var title = "Versão " + versionName + " disponível!";
            Task answer;

            // On mandatory update, user cannot postpone
            if (releaseDetails.MandatoryUpdate)
            {
                answer = Xamarin.Forms.Application.Current.MainPage.DisplayAlert(title, releaseNotes, "Baixar e Instalar");
            }
            else
            {
                answer = Xamarin.Forms.Application.Current.MainPage.DisplayAlert(title, releaseNotes, "Baixar e Instalar", "Me lembre mais tarde...");
            }
            answer.ContinueWith((task) =>
            {
                // If mandatory or if answer was positive
                if (releaseDetails.MandatoryUpdate || (task as Task<bool>).Result)
                {
                    // Notify SDK that user selected update
                    Distribute.NotifyUpdateAction(UpdateAction.Update);
                }
                else
                {
                    // Notify SDK that user selected postpone (for 1 day)
                    // Note that this method call is ignored by the SDK if the update is mandatory
                    Distribute.NotifyUpdateAction(UpdateAction.Postpone);
                }
            });

            // Return true if you are using your own dialog, false otherwise
            return true;
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        public class NativeHelper : ICloseApp
        {
            public void CloseApp()
            {
                Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
            }
        }
    }
   
}
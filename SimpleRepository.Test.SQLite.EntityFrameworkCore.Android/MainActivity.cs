using Android.App;
using Android.OS;
using Microsoft.MobCAT;
using NUnit.Runner.Services;
using SimpleRepositoryApp.SQLite.EntityFrameworkCore;

namespace SimpleRepository.Test.SQLite.EntityFrameworkCore.Android
{
    [Activity(Label = "EFCore-Tests", Theme = "@style/MainTheme", MainLauncher = true)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);

            // register every tests included in the main application/assembly
            var nunit = new NUnit.Runner.App();

            // Do you want to automatically run tests when the app starts?
            nunit.Options = new TestOptions
            {
                AutoRun = true,
                CreateXmlResultFile = true
            };

            var storageFilepath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            TestBootstrap.Begin((datastoreName) => new EFCoreSampleRepositoryContext(Guard.NullOrWhitespace(storageFilepath), datastoreName));

            // Load the Xamarin.Forms based application
            LoadApplication(nunit);
        }
    }
}

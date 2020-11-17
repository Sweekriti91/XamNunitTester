using Foundation;
using UIKit;
using NUnit.Runner.Services;
using SimpleRepositoryApp.SQLite.EntityFrameworkCore;
using Microsoft.MobCAT;
using System.IO;
using System;

namespace SimpleRepository.Test.SQLite.EntityFrameworkCore.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("UnitTestAppDelegate")]
    public partial class UnitTestAppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();
            Xamarin.Calabash.Start();

            // register every tests included in the main application/assembly
            var nunit = new NUnit.Runner.App();

            // Do you want to automatically run tests when the app starts?
            nunit.Options = new TestOptions
            {
                AutoRun = false,
                CreateXmlResultFile = true
            };

            var storageFilepath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "..", "Library");
            TestBootstrap.Begin((datastoreName) => new EFCoreSampleRepositoryContext(Guard.NullOrWhitespace(storageFilepath), datastoreName));

            // Load the Xamarin.Forms based application
            LoadApplication(nunit);

            return base.FinishedLaunching(app, options);
        }
    }
}
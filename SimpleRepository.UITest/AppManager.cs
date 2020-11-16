using System;
using Xamarin.UITest;

namespace SimpleRepository.UITest
{
    static class AppManager
    {
        static IApp app;
        public static IApp App
        {
            get
            {
                if (app == null)
                    throw new NullReferenceException("'AppManager.App' not set. Call 'AppManager.StartApp()' before trying to access it.");
                return app;
            }
        }

        static Platform? platform;
        public static Platform Platform
        {
            get
            {
                if (platform == null)
                    throw new NullReferenceException("'AppManager.Platform' not set.");
                return platform.Value;
            }

            set
            {
                platform = value;
            }
        }

        public static void StartApp()
        {
            if (Platform == Platform.Android)
            {
                app = ConfigureApp
                    .Android
                    // Used to run a .apk file:
                    //EFCore Android
                    //.ApkFile("../../../Binaries/SimpleRepository.Test.SQLite.EntityFrameworkCore.Android.apk")
                    //SQLite Android
                    .ApkFile("../../../Binaries/SimpleRepository.Test.SQLite.SQLiteNet.Android.apk")
                    .StartApp();
            }

            if (Platform == Platform.iOS)
            {
                app = ConfigureApp
                    .iOS
                    // Used to run a .app file on an ios simulator:
                    //EFCore iOS Sim
                    //.AppBundle("../../../Binaries/SimpleRepository.Test.SQLite.EntityFrameworkCore.iOS.app")
                    //SQLite iOS
                    .AppBundle("../../../Binaries/SimpleRepository.Test.SQLite.SQLiteNet.iOS.app")
                    // Used to run a .ipa file on a physical ios device:
                    //.InstalledApp("com.company.bundleid")
                    .StartApp();
            }
        }
    }
}
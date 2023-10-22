using PM2E10784.Models;
using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PM2E10784 {
    public partial class App:Application {

        static DB dataBaseSQLite;

        public static DB DataBaseSQLite
        {
            get {
                if(dataBaseSQLite==null) {
                    dataBaseSQLite=new DB(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),"PM02.db3"));
                }

                return dataBaseSQLite;
            }
        }

        public App() {
            InitializeComponent();

            MainPage=new NavigationPage(new MainPage());
        }

        protected override void OnStart() {
        }

        protected override void OnSleep() {
        }

        protected override void OnResume() {
        }
    }
}

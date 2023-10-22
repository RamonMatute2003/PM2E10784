using Plugin.Geolocator;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;
using static Xamarin.Essentials.Permissions;

namespace PM2E10784 {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Page_map:ContentPage{
        public Page_map(){
            InitializeComponent();
        }

        protected async override void OnAppearing(){
            base.OnAppearing();
            
            var connection=Connectivity.NetworkAccess;
            var local=CrossGeolocator.Current;

            if(connection==NetworkAccess.Internet){
                if(local!=null && local.IsGeolocationAvailable&&local.IsGeolocationEnabled) {
                    var pinEstatico = new Pin {
                        Type=PinType.Place,
                        Position=new Xamarin.Forms.Maps.Position(Page_lista.temp_latitud,Page_lista.temp_longitud),
                        Label=Page_lista.temp_descripcion,
                    };

                    mapa.MoveToRegion(MapSpan.FromCenterAndRadius(new Xamarin.Forms.Maps.Position(Page_lista.temp_latitud,Page_lista.temp_longitud),Distance.FromKilometers(1)));
                    mapa.Pins.Add(pinEstatico);
                    mapa.IsShowingUser=true;
                }else{
                    if(!local.IsGeolocationEnabled) {
                        await DisplayAlert("GPS desactivado","Por favor, activa el GPS para continuar.","OK");
                    }
                }
            }else{
                await DisplayAlert("Sin Acceso a internet","Por favor, revisa tu conexion a internet para continuar.","OK");
            }
        }

        private async void load_image(object sender,EventArgs e) {
            await ShareImage(Page_lista.imageData,"ubicacion.jpg");
        }

        async Task ShareImage(byte[] imageData,string filename) {
            var file = Path.Combine(FileSystem.CacheDirectory,filename);
            File.WriteAllBytes(file,imageData);

            await Share.RequestAsync(new ShareFileRequest {
                Title="Compartir imagen",
                File=new ShareFile(file)
            });
        }
    }
}
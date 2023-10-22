using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Plugin.Media;
using System.IO;
using Plugin.Geolocator;
using Xamarin.Forms.Maps;

namespace PM2E10784 {
    public partial class MainPage:ContentPage {
        Plugin.Media.Abstractions.MediaFile photo=null;

        public MainPage() {
            InitializeComponent();
        }

        protected async override void OnAppearing() {
            base.OnAppearing();

            var connection = Connectivity.NetworkAccess;
            var local = CrossGeolocator.Current;

            if(connection==NetworkAccess.Internet) {
                if(local!=null&&local.IsGeolocationAvailable&&local.IsGeolocationEnabled) {
                    CheckAndRequestLocationPermissionAsync();
                }else{
                    await DisplayAlert("GPS desactivado","Por favor, activa el GPS para continuar.","OK");
                }
            } else {
                await DisplayAlert("Sin Acceso a internet","Por favor, revisa tu conexion a internet para continuar.","OK");
            }
        }

        public byte[] image_to_array_byte() {
            if(photo!=null) {
                using(MemoryStream memory = new MemoryStream()) {
                    Stream stream = photo.GetStream();
                    stream.CopyTo(memory);
                    byte[] data = memory.ToArray();
                    return data;
                }
            }

            return null;
        }

        private async void btn_save(object sender, EventArgs e){

            if(img_photo.Source==null || (img_photo.Source is FileImageSource fileSource && string.IsNullOrEmpty(fileSource.File))) {
                await DisplayAlert("Advertencia","La imagen esta vacia","OK");
            } else{
                if(string.IsNullOrEmpty(txt_description.Text)||string.IsNullOrEmpty(txt_latitude.Text)||string.IsNullOrEmpty(txt_longitude.Text)) {
                    await DisplayAlert("Advertencia","No hay una descripcion, ingresa una","OK");
                }else{

                    try{
                        var list = new Models.Sitios {
                            Latitud=Convert.ToDouble(txt_latitude.Text),
                            Longitud=Convert.ToDouble(txt_longitude.Text),
                            Descripcion=txt_description.Text,
                            Imagen=image_to_array_byte()
                        };

                        if(await App.DataBaseSQLite.GuardarUbicacion(list)>0) {
                            await DisplayAlert("Exitoso","Persona agregada","OK");
                        } else {
                            await DisplayAlert("Error","Ha ocurrido algun error","OK");
                        }
                    }catch(Exception ex){
                        await DisplayAlert("Error","Ha ocurrido un error inesperado"+ex,"OK");
                    }

                    
                }
            }
        }

        private async Task CheckAndRequestLocationPermissionAsync(){
            var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

            if(status!=PermissionStatus.Granted) {
                status=await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            }

            if(status==PermissionStatus.Granted) {
                GetLocationAsync();
            } else if(status==PermissionStatus.Denied) {
                await DisplayAlert("Advertencia","Esta aplicacion no puede funcionar si no tiene los permisos","OK");
            }
        }

        public async Task GetLocationAsync() {
            try {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium);
                var location = await Geolocation.GetLocationAsync(request);

                if(location!=null) {
                    txt_latitude.Text=""+location.Latitude;
                    txt_longitude.Text=""+location.Longitude;
                }

            } catch(FeatureNotSupportedException fnsEx) {
                await DisplayAlert("Advertencia",fnsEx+"","OK");
            } catch(PermissionException pEx) {
                await DisplayAlert("Advertencia","Esta aplicacion no puede funcionar si no tiene los permisos","OK");
            } catch(Exception ex) {
                await DisplayAlert("Advertencia",ex+"","OK");
            }
        }

        private async void btn_lista(object sender,EventArgs e) {
            await Navigation.PushAsync(new Page_lista());
        }

        private async Task<bool> RequestCameraPermissionsAsync() {
            var cameraStatus = await Permissions.CheckStatusAsync<Permissions.Camera>();

            if(cameraStatus!=PermissionStatus.Granted) {
                cameraStatus=await Permissions.RequestAsync<Permissions.Camera>();
            }

            return cameraStatus==PermissionStatus.Granted;
        }

        private async void btn_photo_Clicked(object sender,EventArgs e) {
            if(await RequestCameraPermissionsAsync()) {
                photo=await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions {
                    Directory="MiAlbum",
                    Name="Foto.jpg",
                    SaveToAlbum=true
                });

                if(photo!=null) {
                    img_photo.Source=ImageSource.FromStream(() => {
                        return photo.GetStream();
                    });
                }
            }else{
                await DisplayAlert("Advertencia","Sin permisos de camara","OK");
            }
        }
    }
}

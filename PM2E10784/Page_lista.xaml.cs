using PM2E10784.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PM2E10784 {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Page_lista:ContentPage{
        public static string temp_descripcion;
        public static double temp_longitud, temp_latitud;
        public int temp_id;
        Sitios selectedItem=null;
        public static byte[] imageData;

        public Page_lista() {
            InitializeComponent();
        }

        protected async override void OnAppearing(){
            base.OnAppearing();

            list.ItemsSource=await App.DataBaseSQLite.ObtenerListaUbicaciones();
        }

        private void OnItemTapped(Object sender,SelectionChangedEventArgs e) {
            selectedItem=e.CurrentSelection.FirstOrDefault() as Sitios;

            temp_descripcion=selectedItem.Descripcion;
            temp_longitud=selectedItem.Longitud;
            temp_latitud=selectedItem.Latitud;
            temp_id=selectedItem.Id;
            imageData=selectedItem.Imagen;
        }

        private async void question_map(object sender,EventArgs e) {
            if(selectedItem!=null){
                var response=await DisplayAlert("Accion","Desea ver la ubicacion indicada","NO","YES");

                if(!response){
                    await Navigation.PushAsync(new Page_map());
                }
            }else{
                await DisplayAlert("Alerta","Seleccione una ubicacion","Ok");
            }
        }

        private async void question_delete(object sender,EventArgs e) {
            if(selectedItem!=null){
                var response = await DisplayAlert("Accion","Desea eliminar la ubicacion indicada","NO","YES");
                
                if(!response){
                    await App.DataBaseSQLite.EliminarUbicacion(selectedItem);
                    list.ItemsSource=await App.DataBaseSQLite.ObtenerListaUbicaciones();
                    selectedItem=null;
                }
            }else{
                await DisplayAlert("Alerta","Seleccione un ubicacion","Ok");
            }
        }
    }
}
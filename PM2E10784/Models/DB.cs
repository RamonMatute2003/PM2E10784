using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PM2E10784.Models{
    public class DB{
        readonly SQLiteAsyncConnection db;

        public DB(String pathdb) {
            db=new SQLiteAsyncConnection(pathdb);
            db.CreateTableAsync<Sitios>().Wait();
        }

        public Task<List<Sitios>> ObtenerListaUbicaciones() {
            return db.Table<Sitios>().ToListAsync();
        }

        public Task<Sitios> ObtenerUbicacion(int pcodigo) {
            return db.Table<Sitios>()
                .Where(i => i.Id==pcodigo)
                .FirstOrDefaultAsync();
        }

        public Task<int> GuardarUbicacion(Sitios ubicacion) {
            if(ubicacion.Id!=0) {
                return db.UpdateAsync(ubicacion);
            } else {
                return db.InsertAsync(ubicacion);
            }
        }

        public Task<int> EliminarUbicacion(Sitios ubicacion) {
            return db.DeleteAsync(ubicacion);
        }
    }
}

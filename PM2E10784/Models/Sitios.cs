using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace PM2E10784.Models {
    public class Sitios{
        [PrimaryKey, AutoIncrement] 
        public int Id { get; set; }

        public byte[] Imagen { get; set; }

        [MaxLength(100)]
        public double Latitud { get; set; }

        [MaxLength(100)]
        public double Longitud { get; set; }

        [MaxLength(200)]
        public string Descripcion { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using FONDEGUA_DOCS.Modelo;

namespace FONDEGUA_DOCS.Cache
{
    public class datos_documento:conexion
    {
        int id, id_tipo, id_asoci, id_acci, caja;
        string titulo, descripcion, ruta, fecha;

        public int Id { get => id; set => id = value; }
        public int Id_tipo { get => id_tipo; set => id_tipo = value; }
        public int Id_asoci { get => id_asoci; set => id_asoci = value; }
        public int Id_acci { get => id_acci; set => id_acci = value; }
        public int Caja { get => caja; set => caja = value; }
        public string Titulo { get => titulo; set => titulo = value; }
        public string Descripcion { get => descripcion; set => descripcion = value; }
        public string Ruta { get => ruta; set => ruta = value; }
        public string Fecha { get => fecha; set => fecha = value; }

        
    }
}

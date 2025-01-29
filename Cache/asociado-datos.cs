using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FONDEGUA_DOCS.Modelo;

namespace FONDEGUA_DOCS.Cache
{
    public class asociado_datos:conexion
    {
        int id;
        string nombre, estado;
        long no;

        public int Id { get => id; set => id = value; }
        public string Nombre { get => nombre; set => nombre = value; }
        public string Estado { get => estado; set => estado = value; }
        public long No { get => no; set => no = value; }

        public long getNo(int id)
        {
            using (var conection = GetConnection())
            {
                conection.Open();
                using (var command = new OleDbCommand())
                {
                    command.Connection = conection;
                    command.CommandText = $"SELECT Asociados.No_iden FROM Asociados WHERE (((Asociados.Id)={id}));";
                    command.CommandType = CommandType.Text;
                    OleDbDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            No = reader.GetInt32(0);
                        }
                        return No;
                    }
                    else
                    {
                        return 0;
                    }
                   


                }
            }
        }
    }
}

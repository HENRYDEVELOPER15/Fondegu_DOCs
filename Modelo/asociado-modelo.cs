using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;
using System.Windows.Markup;

namespace FONDEGUA_DOCS.Modelo
{
    public class asociado_modelo : conexion
    {
        public string InsertarAsociado(string nombre, long id, string estado)
        {
            using (var conection = GetConnection())
            {
                conection.Open();
                using (var command = new OleDbCommand())
                {
                    string msg;
                    command.Connection = conection;
                    command.CommandText = $"SELECT COUNT(*) FROM Asociados WHERE No_iden = {id}";
                    int count = (int)command.ExecuteScalar();

                    if (count > 0)
                    {
                        msg = "El ID ya existe en la base de datos.";
                    }
                    else
                    {
                        command.CommandText = "INSERT INTO Asociados (Nombre, Estado, No_iden) " +
                        $"VALUES ('{nombre}', '{estado}', {id})";
                        command.ExecuteNonQuery();
                        msg = "Asociado Guardado";
                    }


                    return msg;
                }
            }
        }

        public DataTable getAsociados()
        {
            DataTable dataTable = new DataTable();
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = new OleDbCommand())
                {
                    command.Connection = connection;
                    command.CommandText = " SELECT No_iden AS Identificación, Nombre, Estado, Id FROM Asociados WHERE Estado = 'Activo' OR Estado = 'Inactivo';";
                    OleDbDataAdapter adapter = new OleDbDataAdapter(command);
                    adapter.Fill(dataTable);
                }
                return dataTable;
            }
        }

        public void ActualizarAsociado(int id, string nombre, string estado, long No)
        {
            using (var conection = GetConnection())
            {
                conection.Open();
                using (var command = new OleDbCommand())
                {
                    command.Connection = conection;
                    command.CommandText = $"UPDATE Asociados SET Asociados.Nombre = '{nombre}', Asociados.No_iden = {No}, Asociados.Estado = '{estado}' WHERE (((Asociados.Id)={id}))";
                    command.ExecuteNonQuery();

                }
            }
        }

        public void BorrrarAsociado(int id)
        {
            using (var conection = GetConnection())
            {
                conection.Open();
                using (var command = new OleDbCommand())
                {
                    command.Connection = conection;
                    command.CommandText = $"DELETE Asociados.Id FROM Asociados WHERE (((Asociados.Id)={id}));";
                    command.ExecuteNonQuery();

                }
            }
        }

        public DataTable getAsociados(string referencia)
        {
            DataTable dataTable = new DataTable();
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = new OleDbCommand())
                {
                    command.Connection = connection;
                    command.CommandText = $"SELECT No_iden AS Identificación, Nombre, Estado, Id FROM Asociados WHERE No_iden LIKE '%{referencia}%' OR Nombre LIKE '%{referencia}%';";
                    OleDbDataAdapter adapter = new OleDbDataAdapter(command);
                    adapter.Fill(dataTable);
                }
                return dataTable;
            }
        }

        
    }
}

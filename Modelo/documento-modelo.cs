using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using FONDEGUA_DOCS.Cache;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Data;
using System.Data.SqlClient;

namespace FONDEGUA_DOCS.Modelo
{
    public class documento_modelo:conexion
    {
        public List<datos_Tipos> CargarTipos()
        {
            List<datos_Tipos> tipos = new List<datos_Tipos>();
            using (var conection = GetConnection())
            {
                conection.Open();
                using (var command = new OleDbCommand())
                {
                    command.Connection = conection;
                    command.CommandText = "SELECT * FROM Tipo_Documentos";
                    OleDbDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        tipos.Add(new datos_Tipos
                        {
                            Id = reader.GetInt32(0),
                            Nombre = reader.GetString(1)
                        });
                    }

                    reader.Close();
                }
                return tipos;
            }
        }

        public int InsertarAccion(int id_usuario)
        {
            using (var conection = GetConnection())
            {
                conection.Open();
                using (var command = new OleDbCommand())
                {
                    
                    command.Connection = conection;
                    command.CommandText = "INSERT INTO Acciones (Fecha_accion, id_usuario) " +
                        $"VALUES (@fecha, {id_usuario})";
                    string fechaFormateada = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                    command.Parameters.AddWithValue("@fecha", fechaFormateada);
                    command.ExecuteNonQuery();
                    command.CommandText = "SELECT @@IDENTITY";
                    int id_acc = (int)command.ExecuteScalar();
                    return id_acc;
                }
            }
        }

        public int InsertarTipo(string tipo)
        {
            using (var conection = GetConnection())
            {
                conection.Open();
                using (var command = new OleDbCommand())
                {
                    command.Connection = conection;
                    command.CommandText = "SELECT id FROM Tipo_Documentos WHERE Clase_Documento = @tipo";
                    command.Parameters.AddWithValue("@tipo", tipo);
                    object result = command.ExecuteScalar();
                    int idTipo = result != null ? Convert.ToInt32(result) : 0;
                    if (idTipo == 0)
                    {
                        command.CommandText = "INSERT INTO Tipo_Documentos (Clase_Documento) VALUES (@tipo)";
                        command.ExecuteNonQuery();

                        command.CommandText = "SELECT @@IDENTITY";
                        idTipo = (int)command.ExecuteScalar();
                    }
                    return idTipo;
                }
            }
        }
        public void InsertarDocumento(string titulo, string Des, string ruta, string año, int caja, int id_asociado, int id_tipo, int id_accion)
        {
            using (var conection = GetConnection())
            {
                conection.Open();
                using (var command = new OleDbCommand()) 
                {
                    command.Connection = conection;
                    command.CommandText = "INSERT INTO Documentos (Titulo, Descripcion, Ruta_doc, Fecha_origen, id_asociado, id_tipo, id_accion, Ubicacion) " +
                        $"VALUES (@titulo, @desc, @ruta, '{año}', @id_a, @id_ti, @id_acc, @caja)";
                    command.Parameters.AddWithValue("@titulo", titulo);
                    command.Parameters.AddWithValue("@desc", Des);
                    command.Parameters.AddWithValue("@ruta", ruta);
                    command.Parameters.AddWithValue("@id_a", id_asociado);
                    command.Parameters.AddWithValue("@id_ti", id_tipo);
                    command.Parameters.AddWithValue("@id_acc",id_accion);
                    command.Parameters.AddWithValue("@caja", caja);
                    command.ExecuteNonQuery();

                }
            }
        }

        public int VerificarAsociado(long id)
        {
            using (var conection = GetConnection())
            {
                conection.Open();
                using (var command = new OleDbCommand())
                {
                    command.Connection = conection;
                    command.CommandText = "SELECT Id FROM Asociados WHERE No_iden = @id";
                    command.Parameters.AddWithValue("@id", id);
                    object result = command.ExecuteScalar();
                    int idTipo = result != null ? Convert.ToInt32(result) : 0;
                    return idTipo;
                }
            }
        }

        public DataTable getTabla()
        {
            DataTable dataTable = new DataTable();
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = new OleDbCommand())
                {
                    command.Connection = connection;
                    //command.CommandText = "SELECT Titulo As Titulo, Ubicacion AS Caja, Fecha_origen AS Año FROM Documentos ORDER BY Id DESC;";
                    command.CommandText = "SELECT * FROM Documentos ORDER BY Fecha_origen DESC;";
                    OleDbDataAdapter adapter = new OleDbDataAdapter(command);
                    adapter.Fill(dataTable);
                }
                return dataTable;
            }
        }

        public DataTable consultarDocumentos(string referencia)
        {
            DataTable dataTable = new DataTable();
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = new OleDbCommand())
                {
                    command.Connection = connection;
                    //command.CommandText = "SELECT Titulo As Titulo, Ubicacion AS Caja, Fecha_origen AS Año FROM Documentos ORDER BY Id DESC;";
                    command.CommandText = $"SELECT Documentos.* " +
                        $"FROM Tipo_Documentos INNER JOIN Documentos ON Tipo_Documentos.Id = Documentos.id_tipo " +
                        $"WHERE (((Documentos.Titulo) Like '%{referencia}%')) OR (((Tipo_Documentos.Clase_Documento) Like '%{referencia}%') OR ((Documentos.Fecha_origen) Like '%{referencia}%')) ORDER BY Documentos.Fecha_origen DESC;";
                    OleDbDataAdapter adapter = new OleDbDataAdapter(command);
                    adapter.Fill(dataTable);
                }
                return dataTable;
            }
        }

        public DataTable consultarDocumentos(string tipo, string año)
        {
            DataTable dataTable = new DataTable();
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = new OleDbCommand())
                {
                    command.Connection = connection;
                    //command.CommandText = "SELECT Titulo As Titulo, Ubicacion AS Caja, Fecha_origen AS Año FROM Documentos ORDER BY Id DESC;";
                    command.CommandText = $"SELECT Documentos.* " +
                        $"FROM Tipo_Documentos INNER JOIN Documentos ON Tipo_Documentos.Id = Documentos.id_tipo " +
                        $"WHERE (((Tipo_Documentos.Clase_Documento)='{tipo}') OR ((Documentos.Fecha_origen) LIKE '%{año}%'));";
                    OleDbDataAdapter adapter = new OleDbDataAdapter(command);
                    adapter.Fill(dataTable);
                }
                return dataTable;
            }
        }

        public DataTable getDocsAsociados(int id)
        {
            DataTable dataTable = new DataTable();
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = new OleDbCommand())
                {
                    command.Connection = connection;
                    command.CommandText = $"SELECT * FROM Documentos WHERE id_asociado = {id} ORDER BY Fecha_origen DESC;";
                    OleDbDataAdapter adapter = new OleDbDataAdapter(command);
                    adapter.Fill(dataTable);
                }
                return dataTable;
            }
        }

        public void ActualizarDocumento(int id, string titulo, string desc, string ruta, string fecha, int ida, int idt, int idac, int ubic)
        {
            using (var conection = GetConnection())
            {
                conection.Open();
                using (var command = new OleDbCommand())
                {
                    command.Connection = conection;
                    command.CommandText = $"UPDATE Documentos SET Documentos.Titulo = '{titulo}',  Documentos.Descripcion = '{desc}',  Documentos.Ruta_doc = '{ruta}',  Documentos.Fecha_origen = '{fecha}',  Documentos.id_asociado = '{ida}',  Documentos.id_tipo = '{idt}',  Documentos.id_accion = '{idac}',  Documentos.Ubicacion = '{ubic}' WHERE (((Documentos.Id)={id}))";
                    command.ExecuteNonQuery();

                }
            }
        }

        public void BorrrarDocumento(int id)
        {
            using (var conection = GetConnection())
            {
                conection.Open();
                using (var command = new OleDbCommand())
                {
                    command.Connection = conection;
                    command.CommandText = $"DELETE Documentos.Id FROM Documentos WHERE (((Documentos.Id)={id}));";
                    command.ExecuteNonQuery();

                }
            }
        }
    }
}

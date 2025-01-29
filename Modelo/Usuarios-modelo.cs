using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Data;
using System.Security.Cryptography;

namespace FONDEGUA_DOCS.Modelo
{
    public class Usuarios_modelo : conexion
    {
        private string usuario;
        private string rol;
        private string nombre;

        private int id;
        public string Usuario { get => usuario; set => usuario = value; }
        public string Rol { get => rol; set => rol = value; }
        public string Nombre { get => nombre; set => nombre = value; }
        public int Id { get => id; set => id = value; }

        public bool Login(string username, string password)
        {
            using (var conection = GetConnection())
            {
                conection.Open();
                using (var command = new OleDbCommand())
                {
                    password = EncriptarContrasena(password);
                    command.Connection = conection;
                    command.CommandText = $"select * from Usuarios where Usuario=@user and Contraseña=@pass";
                    command.Parameters.AddWithValue("@user", username);
                    command.Parameters.AddWithValue("@pass", password);
                    command.CommandType = CommandType.Text;
                    OleDbDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Id = reader.GetInt32(0);
                            Usuario = reader.GetString(1);
                            Rol = reader.GetString(3);
                            Nombre = reader.GetString(4);
                        }
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
            }
        }

        // Método para encriptar la contraseña usando SHA256
        private string EncriptarContrasena(string contraseña)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(contraseña));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}

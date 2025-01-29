using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;

namespace FONDEGUA_DOCS.Modelo
{
    public abstract class conexion
    {
        private readonly string connectionString = "";
        public conexion()
        {
            string dbLocation = Properties.Settings.Default.DatabaseLocation3;
            connectionString = $@"Provider = Microsoft.ACE.OLEDB.12.0; Data Source = {dbLocation}";
        }
        protected OleDbConnection GetConnection()
        {
            return new OleDbConnection(connectionString);
        }
    }
}

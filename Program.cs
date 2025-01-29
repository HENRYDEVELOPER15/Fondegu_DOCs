using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FONDEGUA_DOCS
{
    internal static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Verificar si la configuración de la base de datos está guardada
            if (string.IsNullOrEmpty(Properties.Settings.Default.DatabaseLocation3) ||
                string.IsNullOrEmpty(Properties.Settings.Default.FolderLocation3))
            {
                // Ruta de la carpeta "Documentos" del usuario
                string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                // Nombre de la carpeta que queremos crear
                string folderName = "FONDEGUA_Docs";
                // Ruta completa donde se creará la carpeta
                string folderPath = Path.Combine(documentsPath, folderName);

                // Verificar si la carpeta existe, si no, crearla
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                // Guardar la ruta en la configuración de la aplicación
                Properties.Settings.Default.FolderLocation3 = folderPath;
                Properties.Settings.Default.Save();

                // Mostrar el formulario de conexión
                FormularioConfiguracion connectionForm = new FormularioConfiguracion();
                Application.Run(connectionForm);
            }
            // Continuar con el flujo normal
            Application.Run(new Login());

        }
    }
}

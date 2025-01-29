using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FONDEGUA_DOCS
{
    public partial class FormularioConfiguracion : Form
    {
        public FormularioConfiguracion()
        {
            InitializeComponent();
        }

        private void btnRuta_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Access Database (*.accdb)|*.accdb";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    txtRuta.Text = openFileDialog.FileName;
                }
            }
        }

        private void iconButton1_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    textBox1.Text = folderDialog.SelectedPath;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string nuevaRuta = textBox1.Text;

            if (nuevaRuta != Properties.Settings.Default.FolderLocation3)
            {
                DialogResult result = MessageBox.Show("¿Desea mover todos los archivos de la carpeta actual a la nueva ubicación?",
                                                      "Confirmación de cambio de carpeta",
                                                      MessageBoxButtons.YesNo,
                                                      MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // Mover todos los archivos de la carpeta antigua a la nueva
                    string carpetaAntigua = Properties.Settings.Default.FolderLocation3;

                    if (Directory.Exists(carpetaAntigua) && Directory.Exists(nuevaRuta))
                    {
                        MoverArchivos(carpetaAntigua, nuevaRuta);
                    }
                }

                // Actualizar la configuración con la nueva ruta
                Properties.Settings.Default.FolderLocation3 = nuevaRuta;
            }
            Properties.Settings.Default.DatabaseLocation3 = txtRuta.Text;

            Properties.Settings.Default.Save();

            MessageBox.Show("Configuraciones guardadas correctamente.");
        }

        private void MoverArchivos(string carpetaAntigua, string nuevaCarpeta)
        {
            try
            {
                // Obtener todos los archivos en la carpeta antigua
                string[] archivos = Directory.GetFiles(carpetaAntigua);

                foreach (string archivo in archivos)
                {
                    // Obtener el nombre del archivo sin la ruta
                    string nombreArchivo = Path.GetFileName(archivo);
                    // Construir la nueva ruta del archivo en la nueva carpeta
                    string nuevaRutaArchivo = Path.Combine(nuevaCarpeta, nombreArchivo);

                    // Mover el archivo a la nueva ubicación
                    File.Move(archivo, nuevaRutaArchivo);
                }

                MessageBox.Show("Archivos movidos correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al mover los archivos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void FormularioConfiguracion_Load(object sender, EventArgs e)
        {
            txtRuta.Text = Properties.Settings.Default.DatabaseLocation3;
            textBox1.Text = Properties.Settings.Default.FolderLocation3;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

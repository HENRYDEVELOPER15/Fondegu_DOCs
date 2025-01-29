using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using FONDEGUA_DOCS.Modelo;

namespace FONDEGUA_DOCS
{
    public partial class index : Form
    {
        public Usuarios_modelo user;
        public index(Usuarios_modelo use)
        {
            InitializeComponent();
            panel2.Resize += new EventHandler(panel2_Resize);
            user = use;
            AbrirFormEnPanel(new Directorio(this));
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void panel2_Resize(object sender, EventArgs e)
        {
            pictureBox1.Left = panel2.Width / 2 - pictureBox1.Width / 2 + 30;
            label1.Left = panel2.Width / 2 - label1.Width / 2 + 30;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click_1(object sender, EventArgs e)
        {

        }

        private void index_Load(object sender, EventArgs e)
        {
            lblNombre.Text = user.Nombre;
            lblRol.Text = user.Rol;
        }

        public void AbrirFormEnPanel(object formhija)
        {
            if (this.panelContenedor.Controls.Count > 0)
                this.panelContenedor.Controls.RemoveAt(0);
            Form fh = formhija as Form;
            fh.TopLevel = false;
            fh.Dock = DockStyle.Fill;
            this.panelContenedor.Controls.Add(fh);
            this.panelContenedor.Tag = fh;
            fh.Show();

        }
        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void index_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }

        private void rjButton1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("¿Desea Cerrar Sesion?", "Warning",
           MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                this.Close();
        }

        private void index_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (MessageBox.Show("¿Desea Salir?", "Warning",
           MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                Application.Exit();

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }


        private void iconButton1_Click(object sender, EventArgs e)
        {

        }

        private void panelPadre_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnDirectorio_Click(object sender, EventArgs e)
        {
            label2.Text = "Panel de Busqueda (Directorio)";
            AbrirFormEnPanel(new Directorio(this));
        }

        private void btnAsociados_Click(object sender, EventArgs e)
        {
            label2.Text = "Panel de Busqueda (Asociados)";
            AbrirFormEnPanel(new Asociados(this));
        }

        private void btnConfig_Click(object sender, EventArgs e)
        {
            FormularioConfiguracion configuracion = new FormularioConfiguracion();
            configuracion.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Leer la carpeta de origen y el archivo de base de datos desde la configuración
            string sourceFolder = Properties.Settings.Default.FolderLocation3;
            string databaseFile = Properties.Settings.Default.DatabaseLocation3;

            // Verificar si la carpeta de origen y el archivo de base de datos existen
            if (Directory.Exists(sourceFolder) && File.Exists(databaseFile))
            {
                // Abrir un diálogo para seleccionar la carpeta de destino
                using (FolderBrowserDialog destFolderDialog = new FolderBrowserDialog())
                {
                    destFolderDialog.Description = "Seleccione la carpeta de destino para la copia de seguridad";
                    if (destFolderDialog.ShowDialog() == DialogResult.OK)
                    {
                        string destFolder = destFolderDialog.SelectedPath;

                        // Crear las subcarpetas dentro de la carpeta de destino
                        string filesBackupFolder = Path.Combine(destFolder, "Archivos");
                        string databaseBackupFolder = Path.Combine(destFolder, "BaseDatos");

                        // Crear las subcarpetas si no existen
                        if (!Directory.Exists(filesBackupFolder))
                        {
                            Directory.CreateDirectory(filesBackupFolder);
                        }
                        if (!Directory.Exists(databaseBackupFolder))
                        {
                            Directory.CreateDirectory(databaseBackupFolder);
                        }

                        // Copiar todos los archivos de la carpeta de origen a la subcarpeta "Archivos"
                        foreach (string filePath in Directory.GetFiles(sourceFolder))
                        {
                            string fileName = Path.GetFileName(filePath);
                            string destFilePath = Path.Combine(filesBackupFolder, fileName);

                            File.Copy(filePath, destFilePath, true); // Sobrescribir si ya existe
                        }

                        // Copiar el archivo de la base de datos a la subcarpeta "BaseDatos"
                        string dbFileName = Path.GetFileName(databaseFile);
                        string destDbFilePath = Path.Combine(databaseBackupFolder, dbFileName);
                        File.Copy(databaseFile, destDbFilePath, true); // Sobrescribir si ya existe

                        MessageBox.Show("Copia de seguridad completada exitosamente.");
                    }
                }
            }
            else
            {
                MessageBox.Show("La carpeta de origen o el archivo de base de datos no existen.");
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FONDEGUA_DOCS.Modelo;
using FONDEGUA_DOCS.Cache;
using System.IO;

namespace FONDEGUA_DOCS
{
    public partial class nuevoDoc : Form
    {
        index padre;
        datos_documento direc;
        documento_modelo modelo = new documento_modelo();
        public nuevoDoc(index padre)
        {
            InitializeComponent();
            this.padre = padre;
            LlenarComboBoxTipos();
            
            radioButton1.Checked = true;
        }

        public nuevoDoc(datos_documento datos, index padre)
        {
            InitializeComponent();
            this.padre = padre;
            direc= datos;
            iconButton1.Visible = true;
            iconButton2.Visible = false;
            iconButton4.Visible = true;
            LlenarComboBoxTipos();
            radioButton2.Checked = true;
        }
        private string CopiarArchivo(string rutaArchivo)
        {
            try
            {
                
                if (!Directory.Exists(Properties.Settings.Default.FolderLocation3))
                {
                    Directory.CreateDirectory(@Properties.Settings.Default.FolderLocation3);
                }

                string nombreArchivo = Path.GetFileName(rutaArchivo);
                string nombreFile = txtTitulo.Text + "-" + txtaño.Value.ToString() +"-"+comboBox1.Text+ "-" + txtidAso.Text + ".pdf";
                nombreFile = nombreFile.Replace(" ", "");
                string rutaDestino = Path.Combine(Properties.Settings.Default.FolderLocation3, nombreFile);

                File.Copy(rutaArchivo, rutaDestino, overwrite: true);
                //MessageBox.Show("Archivo copiado a: " + rutaDestino, "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return nombreFile;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al copiar el archivo: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }
        private void iconButton2_Click(object sender, EventArgs e)
        {


            string titulo = txtTitulo.Text;
            string desc = txtDesc.Text;
            string ruta = txtRuta.Text;
            string año = txtaño.Value.ToString() + "-"+comboBox1.Text;
            int caja = textBox4.Text != "" ? int.Parse(textBox4.Text) : caja=0;
            long id_aso = long.Parse(txtidAso.Text);
            int id_acc = modelo.InsertarAccion(padre.user.Id);
            int id_tipo = modelo.InsertarTipo(cmbClase.Text);

            int id = modelo.VerificarAsociado(id_aso);
            if(titulo != "" && textBox4.Text !="" && comboBox1.Text !="")
            {
                if (id != 0)
                {
                    string rutacopy = ruta != ""? CopiarArchivo(ruta):rutacopy = "";
                    modelo.InsertarDocumento(titulo, desc, rutacopy, año, caja, id, id_tipo, id_acc);
                    MessageBox.Show("Documento Guardado");
                    limpiarcampos();
                }
                else
                {
                    MessageBox.Show("Asociado no encontrado. Por favor, regístrelo en la base de datos antes de continuar.");
                }
            }
            else
            {
                MessageBox.Show("Faltan parametros por digitar");
            }

        }

        private void limpiarcampos()
        {
            txtTitulo.Text = "";
            txtRuta.Text = "";
            txtDesc.Text = "";
            textBox4.Text = "";
            txtidAso.Text = "";

        }

        private void button1_Click(object sender, EventArgs e)
        {
            nuevoA nuevoA = new nuevoA(this);
            nuevoA.ShowDialog();
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                txtidAso.Enabled = true;
                button1.Enabled = true; 


            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                txtidAso.Enabled = false;
                button1.Enabled = false;
                txtidAso.Text= "1";
            }
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void iconButton1_Click(object sender, EventArgs e)
        {

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Seleccione un archivo PDF";
            openFileDialog.Filter = "Archivos PDF (*.pdf)|*.pdf";
            openFileDialog.InitialDirectory = @"C:\";


            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;
                txtRuta.Text = filePath;
            }
        }

        private void LlenarComboBoxTipos()
        {
            List<datos_Tipos> tipos = modelo.CargarTipos();

            cmbClase.DataSource = tipos;
            cmbClase.DisplayMember = "Nombre";
            cmbClase.ValueMember = "Id";
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void iconButton3_Click(object sender, EventArgs e)
        {
            padre.AbrirFormEnPanel(new Directorio(padre));
        }

        private void nuevoDoc_Load(object sender, EventArgs e)
        {

        }

        private void txtaño_ValueChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
        public void EliminarArchivo(string filePath)
        {
            // Verificar si el archivo existe
            if (File.Exists(filePath))
            {
                try
                {
                    // Eliminar el archivo
                    File.Delete(filePath);
                    MessageBox.Show("El archivo ha sido eliminado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    // En caso de error
                    MessageBox.Show("Error al intentar eliminar el archivo: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("El archivo no existe.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void iconButton4_Click(object sender, EventArgs e)
        {
            DialogResult resultado = MessageBox.Show("¿Estás seguro de que deseas eliminar este registro?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (resultado == DialogResult.Yes)
            {
                string folderLocation = Properties.Settings.Default.FolderLocation3;

                // Concatenar la ruta completa del archivo
                string rutaCompleta = Path.Combine(folderLocation, direc.Ruta);

                EliminarArchivo(rutaCompleta);
                modelo.BorrrarDocumento(direc.Id);
                MessageBox.Show("Registro eliminado correctamente.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                padre.AbrirFormEnPanel(new Directorio(padre));
            }
            else
            {
                MessageBox.Show("Operación cancelada.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
          
        }

        private void iconButton1_Click_1(object sender, EventArgs e)
        {
            string titulo = txtTitulo.Text;
            string desc = txtDesc.Text;
            string ruta = txtRuta.Text;
            string año = txtaño.Value.ToString() + "-" + comboBox1.Text;
            int caja = textBox4.Text != "" ? int.Parse(textBox4.Text) : caja = 0;
            long id_aso = long.Parse(txtidAso.Text);
            int id_acc = modelo.InsertarAccion(padre.user.Id);
            int id_tipo = modelo.InsertarTipo(cmbClase.Text);

            int id = modelo.VerificarAsociado(id_aso);
            if (titulo != "" && textBox4.Text != "" && comboBox1.SelectedIndex !=0)
            {
                if (id != 0)
                {
                    if (direc.Ruta != ruta)
                    {
                        string folderLocation = Properties.Settings.Default.FolderLocation3;
                        string rutaCompleta = Path.Combine(folderLocation, direc.Ruta);
                        EliminarArchivo(rutaCompleta);
                        string rutacopy = CopiarArchivo(ruta);
                        modelo.ActualizarDocumento(direc.Id, titulo, desc, rutacopy, año, id, id_tipo, id_acc, caja);
                    }
                    else
                    {
                        modelo.ActualizarDocumento(direc.Id, titulo, desc, ruta, año, id, id_tipo, id_acc, caja);
                    }
                   
                    MessageBox.Show("Documento Guardado");
                    limpiarcampos();
                }
                else
                {
                    MessageBox.Show("Asociado no encontrado. Por favor, regístrelo en la base de datos antes de continuar.");
                }
            }
            else
            {
                MessageBox.Show("Faltan parametros por digitar");
            }
        }

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void txtidAso_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
    }
}

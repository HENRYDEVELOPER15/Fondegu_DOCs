using FONDEGUA_DOCS.Cache;
using FONDEGUA_DOCS.Modelo;
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
using System.Windows.Media.Media3D;

namespace FONDEGUA_DOCS
{
    public partial class Directorio : Form
    {
        documento_modelo modelo = new documento_modelo();
        index padre;
        asociado_datos asoc;
        public Directorio(index index)
        {
            InitializeComponent();
            padre = index;
        }

        public Directorio(index index, asociado_datos a)
        {
            InitializeComponent();
            asoc = a;
            padre = index;
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            padre.label2.Text = "Nuevo Documento";
            padre.AbrirFormEnPanel(new nuevoDoc(padre));
        }

        private void cargarDatos()
        {
            //dataGridView1.DataSource = modelo.getTabla();
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[2].Visible = false;
            dataGridView1.Columns[3].Visible = false;
            dataGridView1.Columns[5].Visible = false;
            dataGridView1.Columns[6].Visible = false;
            dataGridView1.Columns[7].Visible = false;
            dataGridView1.Columns[4].HeaderText = "Año";
            dataGridView1.Columns[8].HeaderText = "Caja";

            DataGridViewButtonColumn viewButtonColumn = new DataGridViewButtonColumn();
            viewButtonColumn.Name = "VerArchivo";
            viewButtonColumn.HeaderText = "Ver Archivo";
            viewButtonColumn.Text = "Ver";
            viewButtonColumn.UseColumnTextForButtonValue = true;

            viewButtonColumn.DefaultCellStyle.BackColor = Color.White;   
            viewButtonColumn.DefaultCellStyle.ForeColor = Color.DarkGreen;    
            viewButtonColumn.DefaultCellStyle.SelectionBackColor = Color.DarkGreen; 
            viewButtonColumn.DefaultCellStyle.SelectionForeColor = Color.White; 
            viewButtonColumn.FlatStyle = FlatStyle.Flat;
            dataGridView1.Columns.Add(viewButtonColumn);

            dataGridView1.CellClick += new DataGridViewCellEventHandler(dataGridView1_CellClick);
        }
        private void Directorio_Load(object sender, EventArgs e)
        {
            
            if(asoc != null)
            {
                dataGridView1.DataSource = modelo.getDocsAsociados(asoc.Id);
                
            }
            else
            {
                dataGridView1.DataSource = modelo.getTabla();
            }
            cargarDatos();
            LlenarComboBoxTipos();
            
        }
        private void LlenarComboBoxTipos()
        {
            List<datos_Tipos> tipos = modelo.CargarTipos();

            comboBox1.DataSource = tipos;
            comboBox1.DisplayMember = "Nombre";
            comboBox1.ValueMember = "Id";
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == ((DataGridView)sender).Columns["VerArchivo"].Index && e.RowIndex >= 0)
            {
                // Captura la ruta del archivo desde la columna "Ruta_doc" correspondiente a la fila seleccionada
                string rutaDoc = ObtenerRutaDoc(e.RowIndex);

                string folderLocation = Properties.Settings.Default.FolderLocation3;

                // Concatenar la ruta completa del archivo
                string rutaCompleta = Path.Combine(folderLocation, rutaDoc);

                // Llama a la función para abrir el archivo en Google Chrome
                if (rutaDoc != "")
                {
                    AbrirArchivoEnChrome(rutaCompleta);
                }
                else
                {
                    MessageBox.Show("Este registro no tiene Archivo");
                }
                
            }
        }

        private string ObtenerRutaDoc(int rowIndex)
        {
            // Asumiendo que la columna Ruta_doc está en el DataTable original que llenó el DataGridView
            DataRowView rowView = (DataRowView)dataGridView1.Rows[rowIndex].DataBoundItem;
            return rowView["Ruta_doc"].ToString();
        }

        private void AbrirArchivoEnChrome(string rutaDoc)
        {
            try
            {
                // Usa Google Chrome para abrir el archivo
                System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = "chrome.exe",
                    Arguments = rutaDoc,
                    CreateNoWindow = true,
                    WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden
                };
                System.Diagnostics.Process.Start(psi);
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo abrir el documento: " + ex.Message);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            dataGridView1.DataSource = modelo.consultarDocumentos(textBox1.Text);
        }

        private void btnbuscar_Click(object sender, EventArgs e)
        {
            string fecha = numericUpDown1.Value.ToString()+"-"+comboBox2.Text;
            dataGridView1.DataSource = modelo.consultarDocumentos(comboBox1.Text, fecha);
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // Obtén la fila seleccionada
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                datos_documento datos = new datos_documento();
                nuevoDoc nuevo = new nuevoDoc(datos, padre);
                //MessageBox.Show(row.Cells[1].Value.ToString() + " "+ row.Cells[2].Value.ToString()+ " "+row.Cells[3].Value.ToString() + " " + row.Cells[4].Value.ToString() + " " + row.Cells[5].Value.ToString() + " " + row.Cells[6].Value.ToString() + " " + row.Cells[7].Value.ToString() + " " + row.Cells[8].Value.ToString() + " " + row.Cells[9].Value.ToString());
                datos.Id =(int)row.Cells[1].Value;
                datos.Titulo = row.Cells[2].Value.ToString();
                datos.Descripcion = row.Cells[3].Value.ToString();
                datos.Ruta = row.Cells[4].Value.ToString();
                datos.Fecha = row.Cells[5].Value.ToString();
                datos.Id_asoci = (int)row.Cells[6].Value;
                datos.Id_tipo = (int)row.Cells[7].Value;
                datos.Id_acci = (int)row.Cells[8].Value;
                datos.Caja = (int)row.Cells[9].Value;

                nuevo.txtTitulo.Text = datos.Titulo;
                nuevo.txtRuta.Text = datos.Ruta;
                nuevo.txtDesc.Text = datos.Descripcion;
                string[] partes = datos.Fecha.Split('-');
                int parte1 =int.Parse(partes[0]);
                string parte2 = partes[1];
                nuevo.txtaño.Value = parte1;
                nuevo.comboBox1.SelectedItem= parte2;
                nuevo.textBox4.Text = datos.Caja.ToString();
                nuevo.cmbClase.SelectedValue = datos.Id_tipo;
                asociado_datos asociado = new asociado_datos();

                nuevo.txtidAso.Text = asociado.getNo(datos.Id_asoci).ToString();
                padre.AbrirFormEnPanel(nuevo);



            }
        }
    }
}

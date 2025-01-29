using FONDEGUA_DOCS.Cache;
using FONDEGUA_DOCS.Modelo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FONDEGUA_DOCS
{
    public partial class Asociados : Form 
    {
        index padre;
        public Asociados(index index)
        {
            padre = index;
            InitializeComponent();
        }
        asociado_modelo amodelo = new asociado_modelo();
        private void Asociados_Load(object sender, EventArgs e)
        {
            cargarDatos();
            configuaraTabla();
        }
        public void cargarDatos()
        {
            dataGridView1.DataSource = amodelo.getAsociados();
        }
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            nuevoA nuevoA = new nuevoA(this);
            nuevoA.ShowDialog();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
           dataGridView1.DataSource = amodelo.getAsociados(textBox1.Text);
        }

        private void configuaraTabla()
        {
            DataGridViewButtonColumn viewButtonColumn = new DataGridViewButtonColumn();
            viewButtonColumn.Name = "VerHistorico";
            viewButtonColumn.HeaderText = "";
            viewButtonColumn.Text = "...";
            viewButtonColumn.UseColumnTextForButtonValue = true;

            dataGridView1.Columns.Add(viewButtonColumn);

            dataGridView1.Columns[3].Visible = false;

            dataGridView1.CellClick += new DataGridViewCellEventHandler(dataGridView1_CellClick);
        }

        private void borrarToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == ((DataGridView)sender).Columns["VerHistorico"].Index && e.RowIndex >= 0)
            {
            
                contextMenuStrip1.Show(Cursor.Position);

                contextMenuStrip1.Tag = e.RowIndex;
            }
        }

        private void editarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int rowIndex = (int)contextMenuStrip1.Tag;
            asociado_datos datos= new asociado_datos();

            //MessageBox.Show(dataGridView1.Rows[rowIndex].Cells[1].Value.ToString());
            // Obtener los datos de la fila seleccionada
            datos.Id = Convert.ToInt32(dataGridView1.Rows[rowIndex].Cells[4].Value);
            datos.Nombre = dataGridView1.Rows[rowIndex].Cells[2].Value.ToString();
            datos.Estado = dataGridView1.Rows[rowIndex].Cells[3].Value.ToString();
            datos.No = long.Parse(dataGridView1.Rows[rowIndex].Cells[1].Value.ToString());

            nuevoA nuevo = new nuevoA(datos, this);
            nuevo.textBox1.Text = datos.Nombre;
            nuevo.textBox2.Text = datos.No.ToString();
            nuevo.comboBox1.SelectedItem = datos.Estado;
            nuevo.ShowDialog();
        }

        private void verHistorialToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int rowIndex = (int)contextMenuStrip1.Tag;
            asociado_datos datos = new asociado_datos();
            datos.Id = Convert.ToInt32(dataGridView1.Rows[rowIndex].Cells[4].Value);
            datos.Nombre = dataGridView1.Rows[rowIndex].Cells[2].Value.ToString();
            datos.Estado = dataGridView1.Rows[rowIndex].Cells[3].Value.ToString();
            datos.No = long.Parse(dataGridView1.Rows[rowIndex].Cells[1].Value.ToString());

            Directorio directorio = new Directorio(padre, datos);
            padre.label2.Text = "Panel de Busqueda: (Asociado: "+datos.Nombre+" - "+datos.No.ToString()+")";
            padre.AbrirFormEnPanel(directorio);

        }

        private void borrarToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            int rowIndex = (int)contextMenuStrip1.Tag;
            asociado_datos datos = new asociado_datos();
            datos.Id = Convert.ToInt32(dataGridView1.Rows[rowIndex].Cells[4].Value);

            DialogResult resultado = MessageBox.Show("¿Estás seguro de que deseas eliminar este registro?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);


            if (resultado == DialogResult.Yes)
            {
                amodelo.BorrrarAsociado(datos.Id);
             
                MessageBox.Show("Registro eliminado correctamente.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cargarDatos();
            }
            else
            {

                MessageBox.Show("Operación cancelada.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }


           

        }
    }
}

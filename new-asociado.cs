using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FONDEGUA_DOCS.Cache;
using FONDEGUA_DOCS.Modelo;
namespace FONDEGUA_DOCS
{
    public partial class nuevoA : Form
    {
        public nuevoA()
        {
            InitializeComponent();
        }

        private bool nuevo;
        nuevoDoc doc;
        Asociados asociados;
        public nuevoA(nuevoDoc nuevoDoc)
        {
            nuevo   =  true;
            doc = nuevoDoc;
            InitializeComponent();
        }

        public nuevoA(Asociados nuevo)
        {
            asociados = nuevo;
            InitializeComponent();
        }
        asociado_datos asociado;
        public nuevoA(asociado_datos aso, Asociados direc)
        {
            asociado = aso;
            asociados = direc;
            InitializeComponent();
        }
        asociado_modelo modelo = new asociado_modelo();

        private void button1_Click(object sender, EventArgs e)
        {
            if (asociado != null)
            {
                modelo.ActualizarAsociado(asociado.Id, textBox1.Text, comboBox1.Text, long.Parse(textBox2.Text));
                MessageBox.Show("Datos Actualizados");
                asociados.cargarDatos();
                this.Hide();
            }
            else
            {
                if (textBox1.Text != "" && textBox2.Text != "")
                {
                    
                    string msg = modelo.InsertarAsociado(textBox1.Text, long.Parse(textBox2.Text), comboBox1.Text);
                    MessageBox.Show(msg);
                    if (nuevo)
                    {
                        doc.txtidAso.Text = textBox2.Text;
                    }
                    else if(asociados != null){
                        asociados.cargarDatos();
                    }
                    
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Por favor, digite los datos requeridos");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void nuevoA_Load(object sender, EventArgs e)
        {

        }
    }
}

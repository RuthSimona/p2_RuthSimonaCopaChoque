using CadParcial2Rscc;
using ClnParcial2Rscc;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CpParcial2Rscc
{
    public partial class FrmSerie : Form
    {
        public bool esNuevo = false;

        public FrmSerie()
        {
            InitializeComponent();
            CargarCategorias();
        }

        private void FrmSerie_Load(object sender, EventArgs e)
        {
            Size = new Size(1000, 400);
            listar();
        }

        private void CargarCategorias()
        {
            var categorias = new List<string>
            {
                "Ciencia ficción",
                "Romance",
                "Drama",
                "Comedia",
                "Acción"
            };

            cbxCategoria.DataSource = categorias;
        }

        private void listar()
        {
            var lista = SerieCln.ListarPa(txtParametro.Text.Trim());
            dgvLista.DataSource = lista;
            dgvLista.Columns["id"].Visible = false;
            dgvLista.Columns["estado"].Visible = false;
            dgvLista.Columns["titulo"].HeaderText = "Título";
            dgvLista.Columns["sinopsis"].HeaderText = "Sinopsis";
            dgvLista.Columns["director"].HeaderText = "Director";
            dgvLista.Columns["episodios"].HeaderText = "Episodios";
            dgvLista.Columns["fecha_estreno"].HeaderText = "Fecha de Estreno";
            dgvLista.Columns["categoria"].HeaderText = "Categoría"; 
            btnEditar.Enabled = lista.Count > 0;
            btnEliminar.Enabled = lista.Count > 0;
            if (lista.Count > 0) dgvLista.Rows[0].Cells["titulo"].Selected = true;
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            esNuevo = true;
            Size = new Size(1000, 600);
            txtTitulo.Focus();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            esNuevo = false;
            Size = new Size(1000, 600);

            int index = dgvLista.CurrentCell.RowIndex;
            int id = Convert.ToInt32(dgvLista.Rows[index].Cells["id"].Value);
            var serie = SerieCln.ObtenerUno(id);
            if (serie != null)
            {
                txtTitulo.Text = serie.titulo;
                txtSinopsis.Text = serie.sinopsis;
                txtDirector.Text = serie.director;
                nudEpisodios.Value = serie.episodios ?? 0;
                dtpFechaEstreno.Value = serie.fecha_estreno ?? DateTime.Now;
                cbxCategoria.SelectedItem = serie.categoria ?? "";
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Size = new Size(1000, 400);
            limpiar();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            listar();
        }

        private void txtParametro_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter) listar();
        }

        private bool validar()
        {
            bool esValido = true;
            erpTitulo.SetError(txtTitulo, "");
            erpSinopsis.SetError(txtSinopsis, "");
            erpDirector.SetError(txtDirector, "");
            erpEpisodios.SetError(nudEpisodios, "");

            if (string.IsNullOrEmpty(txtTitulo.Text))
            {
                esValido = false;
                erpTitulo.SetError(txtTitulo, "El campo Título es obligatorio");
            }
            if (string.IsNullOrEmpty(txtSinopsis.Text))
            {
                esValido = false;
                erpSinopsis.SetError(txtSinopsis, "El campo Sinopsis es obligatorio");
            }
            if (string.IsNullOrEmpty(txtDirector.Text))
            {
                esValido = false;
                erpDirector.SetError(txtDirector, "El campo Director es obligatorio");
            }
            if (nudEpisodios.Value <= 0)
            {
                esValido = false;
                erpEpisodios.SetError(nudEpisodios, "El campo Episodios debe ser mayor a 0");
            }
            if (cbxCategoria.SelectedItem == null)
            {
                esValido = false;
                MessageBox.Show("Debe seleccionar una categoría", "::: Mensaje :::",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            return esValido;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (validar())
            {
                var serie = new Serie
                {
                    titulo = txtTitulo.Text.Trim(),
                    sinopsis = txtSinopsis.Text.Trim(),
                    director = txtDirector.Text.Trim(),
                    episodios = (int)nudEpisodios.Value,
                    fecha_estreno = dtpFechaEstreno.Value,
                    categoria = cbxCategoria.SelectedItem.ToString(),
                    estado = 1
                };

                if (esNuevo)
                {
                    SerieCln.Insertar(serie);
                }
                else
                {
                    int index = dgvLista.CurrentCell.RowIndex;
                    serie.id = Convert.ToInt32(dgvLista.Rows[index].Cells["id"].Value);
                    SerieCln.Actualizar(serie);
                }
                listar();
                btnCancelar.PerformClick();
                MessageBox.Show("Serie guardada correctamente", "::: Mensaje :::",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void limpiar()
        {
            txtTitulo.Text = string.Empty;
            txtSinopsis.Text = string.Empty;
            txtDirector.Text = string.Empty;
            nudEpisodios.Value = 0;
            dtpFechaEstreno.Value = DateTime.Now;
            cbxCategoria.SelectedIndex = -1;
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            int index = dgvLista.CurrentCell.RowIndex;
            int id = Convert.ToInt32(dgvLista.Rows[index].Cells["id"].Value);
            string titulo = dgvLista.Rows[index].Cells["titulo"].Value.ToString();
            DialogResult dialog = MessageBox.Show($"¿Está seguro que desea dar de baja la serie {titulo}?",
                "::: Mensaje :::", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dialog == DialogResult.OK)
            {
                SerieCln.Eliminar(id, "SIS457");
                listar();
                MessageBox.Show("Serie dada de baja correctamente", "::: Mensaje :::",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void cbxCategoria_SelectedIndexChanged(object sender, EventArgs e)
        {
            //
        }
    }
}

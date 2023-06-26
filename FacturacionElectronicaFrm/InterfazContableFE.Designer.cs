namespace FacturacionElectronicaFrm
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.lbEventos = new System.Windows.Forms.ListBox();
            this.btnIniciar = new System.Windows.Forms.Button();
            this.btnDetener = new System.Windows.Forms.Button();
            this.btnCerrar = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.lstCotizaciones = new System.Windows.Forms.Button();
            this.lstCotEncabezado = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // lbEventos
            // 
            this.lbEventos.FormattingEnabled = true;
            this.lbEventos.HorizontalScrollbar = true;
            this.lbEventos.Location = new System.Drawing.Point(645, 23);
            this.lbEventos.Name = "lbEventos";
            this.lbEventos.ScrollAlwaysVisible = true;
            this.lbEventos.Size = new System.Drawing.Size(797, 212);
            this.lbEventos.TabIndex = 1;
            // 
            // btnIniciar
            // 
            this.btnIniciar.Location = new System.Drawing.Point(80, 69);
            this.btnIniciar.Name = "btnIniciar";
            this.btnIniciar.Size = new System.Drawing.Size(75, 23);
            this.btnIniciar.TabIndex = 2;
            this.btnIniciar.Text = "Iniciar";
            this.btnIniciar.UseVisualStyleBackColor = true;
            this.btnIniciar.Click += new System.EventHandler(this.btnIniciar_Click);
            // 
            // btnDetener
            // 
            this.btnDetener.Location = new System.Drawing.Point(216, 69);
            this.btnDetener.Name = "btnDetener";
            this.btnDetener.Size = new System.Drawing.Size(75, 23);
            this.btnDetener.TabIndex = 3;
            this.btnDetener.Text = "Detener";
            this.btnDetener.UseVisualStyleBackColor = true;
            // 
            // btnCerrar
            // 
            this.btnCerrar.Location = new System.Drawing.Point(361, 69);
            this.btnCerrar.Name = "btnCerrar";
            this.btnCerrar.Size = new System.Drawing.Size(75, 23);
            this.btnCerrar.TabIndex = 4;
            this.btnCerrar.Text = "Cerrar";
            this.btnCerrar.UseVisualStyleBackColor = true;
            this.btnCerrar.Click += new System.EventHandler(this.btnCerrar_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(33, 254);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(1333, 288);
            this.dataGridView1.TabIndex = 5;
            // 
            // lstCotizaciones
            // 
            this.lstCotizaciones.Location = new System.Drawing.Point(80, 173);
            this.lstCotizaciones.Name = "lstCotizaciones";
            this.lstCotizaciones.Size = new System.Drawing.Size(101, 47);
            this.lstCotizaciones.TabIndex = 6;
            this.lstCotizaciones.Text = "LISTAR COTIZACIONES";
            this.lstCotizaciones.UseVisualStyleBackColor = true;
            this.lstCotizaciones.Click += new System.EventHandler(this.lstCotizaciones_Click);
            // 
            // lstCotEncabezado
            // 
            this.lstCotEncabezado.Location = new System.Drawing.Point(253, 173);
            this.lstCotEncabezado.Name = "lstCotEncabezado";
            this.lstCotEncabezado.Size = new System.Drawing.Size(101, 47);
            this.lstCotEncabezado.TabIndex = 7;
            this.lstCotEncabezado.Text = "LISTAR COTIZACION_ENCABEZADO";
            this.lstCotEncabezado.UseVisualStyleBackColor = true;
            this.lstCotEncabezado.Click += new System.EventHandler(this.lstCotEncabezado_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(400, 173);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(101, 47);
            this.button1.TabIndex = 8;
            this.button1.Text = "LISTAR CLIENTES";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1454, 600);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.lstCotEncabezado);
            this.Controls.Add(this.lstCotizaciones);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.btnCerrar);
            this.Controls.Add(this.btnDetener);
            this.Controls.Add(this.btnIniciar);
            this.Controls.Add(this.lbEventos);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Facturacion Electronica Interfaz Contable";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox lbEventos;
        private System.Windows.Forms.Button btnIniciar;
        private System.Windows.Forms.Button btnDetener;
        private System.Windows.Forms.Button btnCerrar;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button lstCotizaciones;
        private System.Windows.Forms.Button lstCotEncabezado;
        private System.Windows.Forms.Button button1;
    }
}


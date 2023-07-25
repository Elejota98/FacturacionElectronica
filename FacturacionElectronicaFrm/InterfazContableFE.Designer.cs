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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.lbEventos = new System.Windows.Forms.ListBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.restaurarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.minimizarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cerrarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.lblTexto = new System.Windows.Forms.Label();
            this.btnDescargarDoc = new System.Windows.Forms.Button();
            this.btnEncabezado = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbEventos
            // 
            this.lbEventos.FormattingEnabled = true;
            this.lbEventos.HorizontalScrollbar = true;
            this.lbEventos.Location = new System.Drawing.Point(133, 24);
            this.lbEventos.Name = "lbEventos";
            this.lbEventos.ScrollAlwaysVisible = true;
            this.lbEventos.Size = new System.Drawing.Size(797, 212);
            this.lbEventos.TabIndex = 1;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(28, 282);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(1153, 209);
            this.dataGridView1.TabIndex = 5;
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Text = "notifyIcon1";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.restaurarToolStripMenuItem,
            this.minimizarToolStripMenuItem,
            this.cerrarToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(128, 70);
            // 
            // restaurarToolStripMenuItem
            // 
            this.restaurarToolStripMenuItem.Name = "restaurarToolStripMenuItem";
            this.restaurarToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
            this.restaurarToolStripMenuItem.Text = "Restaurar";
            this.restaurarToolStripMenuItem.Click += new System.EventHandler(this.restaurarToolStripMenuItem_Click);
            // 
            // minimizarToolStripMenuItem
            // 
            this.minimizarToolStripMenuItem.Name = "minimizarToolStripMenuItem";
            this.minimizarToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
            this.minimizarToolStripMenuItem.Text = "Minimizar";
            // 
            // cerrarToolStripMenuItem
            // 
            this.cerrarToolStripMenuItem.Name = "cerrarToolStripMenuItem";
            this.cerrarToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
            this.cerrarToolStripMenuItem.Text = "Cerrar";
            this.cerrarToolStripMenuItem.Click += new System.EventHandler(this.cerrarToolStripMenuItem_Click);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // lblTexto
            // 
            this.lblTexto.AutoSize = true;
            this.lblTexto.Font = new System.Drawing.Font("Arial Rounded MT Bold", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTexto.ForeColor = System.Drawing.Color.Red;
            this.lblTexto.Location = new System.Drawing.Point(192, 239);
            this.lblTexto.Name = "lblTexto";
            this.lblTexto.Size = new System.Drawing.Size(662, 40);
            this.lblTexto.TabIndex = 6;
            this.lblTexto.Text = "¡Un Nuevo Cliente Solicita Registrase!";
            this.lblTexto.Visible = false;
            // 
            // btnDescargarDoc
            // 
            this.btnDescargarDoc.Font = new System.Drawing.Font("Arial Rounded MT Bold", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDescargarDoc.Location = new System.Drawing.Point(507, 515);
            this.btnDescargarDoc.Name = "btnDescargarDoc";
            this.btnDescargarDoc.Size = new System.Drawing.Size(136, 60);
            this.btnDescargarDoc.TabIndex = 7;
            this.btnDescargarDoc.Text = "Descargar Documento";
            this.btnDescargarDoc.UseVisualStyleBackColor = true;
            this.btnDescargarDoc.Visible = false;
            this.btnDescargarDoc.Click += new System.EventHandler(this.btnDescargarDoc_Click);
            // 
            // btnEncabezado
            // 
            this.btnEncabezado.Location = new System.Drawing.Point(1009, 163);
            this.btnEncabezado.Name = "btnEncabezado";
            this.btnEncabezado.Size = new System.Drawing.Size(102, 73);
            this.btnEncabezado.TabIndex = 8;
            this.btnEncabezado.Text = "Cotizaciones Encabezado";
            this.btnEncabezado.UseVisualStyleBackColor = true;
            this.btnEncabezado.Click += new System.EventHandler(this.btnEncabezado_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(1117, 163);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(102, 73);
            this.button1.TabIndex = 9;
            this.button1.Text = "Cotizaciones";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_2);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1246, 600);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnEncabezado);
            this.Controls.Add(this.btnDescargarDoc);
            this.Controls.Add(this.lblTexto);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.lbEventos);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Facturacion Electronica Interfaz Contable";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.SizeChanged += new System.EventHandler(this.Form1_SizeChanged);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lbEventos;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem restaurarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem minimizarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cerrarToolStripMenuItem;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label lblTexto;
        private System.Windows.Forms.Button btnDescargarDoc;
        private System.Windows.Forms.Button btnEncabezado;
        private System.Windows.Forms.Button button1;
    }
}


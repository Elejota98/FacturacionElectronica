namespace FacturacionElectronicaServicio
{
    partial class SincronizacionFacturacionElectronica
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SincronizacionFacturacionElectronica));
            this.lbEventos = new System.Windows.Forms.ListBox();
            this.Inicio = new System.Windows.Forms.Timer(this.components);
            this.dvgRta = new System.Windows.Forms.DataGridView();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.restaurarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.minimizarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cerrarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.dvgRta)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbEventos
            // 
            this.lbEventos.FormattingEnabled = true;
            this.lbEventos.HorizontalScrollbar = true;
            this.lbEventos.ItemHeight = 16;
            this.lbEventos.Location = new System.Drawing.Point(31, 26);
            this.lbEventos.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.lbEventos.Name = "lbEventos";
            this.lbEventos.ScrollAlwaysVisible = true;
            this.lbEventos.Size = new System.Drawing.Size(1451, 740);
            this.lbEventos.TabIndex = 4;
            // 
            // Inicio
            // 
            this.Inicio.Tick += new System.EventHandler(this.Inicio_Tick);
            // 
            // dvgRta
            // 
            this.dvgRta.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dvgRta.Location = new System.Drawing.Point(31, 498);
            this.dvgRta.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dvgRta.Name = "dvgRta";
            this.dvgRta.RowHeadersWidth = 51;
            this.dvgRta.Size = new System.Drawing.Size(1467, 185);
            this.dvgRta.TabIndex = 5;
            this.dvgRta.Visible = false;
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "FacturacionElecronicaService";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.restaurarToolStripMenuItem,
            this.minimizarToolStripMenuItem,
            this.cerrarToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(145, 76);
            // 
            // restaurarToolStripMenuItem
            // 
            this.restaurarToolStripMenuItem.Name = "restaurarToolStripMenuItem";
            this.restaurarToolStripMenuItem.Size = new System.Drawing.Size(144, 24);
            this.restaurarToolStripMenuItem.Text = "Restaurar";
            this.restaurarToolStripMenuItem.Click += new System.EventHandler(this.restaurarToolStripMenuItem_Click);
            // 
            // minimizarToolStripMenuItem
            // 
            this.minimizarToolStripMenuItem.Name = "minimizarToolStripMenuItem";
            this.minimizarToolStripMenuItem.Size = new System.Drawing.Size(144, 24);
            this.minimizarToolStripMenuItem.Text = "Minimizar";
            // 
            // cerrarToolStripMenuItem
            // 
            this.cerrarToolStripMenuItem.Name = "cerrarToolStripMenuItem";
            this.cerrarToolStripMenuItem.Size = new System.Drawing.Size(144, 24);
            this.cerrarToolStripMenuItem.Text = "Cerrar";
            this.cerrarToolStripMenuItem.Click += new System.EventHandler(this.cerrarToolStripMenuItem_Click);
            // 
            // SincronizacionFacturacionElectronica
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1564, 846);
            this.Controls.Add(this.lbEventos);
            this.Controls.Add(this.dvgRta);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "SincronizacionFacturacionElectronica";
            this.Text = "SincronizacionFacturacionElectronica";
            this.Load += new System.EventHandler(this.SincronizacionFacturacionElectronica_Load);
            this.SizeChanged += new System.EventHandler(this.SincronizacionFacturacionElectronica_SizeChanged);
            ((System.ComponentModel.ISupportInitialize)(this.dvgRta)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox lbEventos;
        private System.Windows.Forms.Timer Inicio;
        private System.Windows.Forms.DataGridView dvgRta;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem restaurarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem minimizarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cerrarToolStripMenuItem;
    }
}


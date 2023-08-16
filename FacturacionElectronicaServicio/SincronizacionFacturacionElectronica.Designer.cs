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
            this.lbEventos = new System.Windows.Forms.ListBox();
            this.Inicio = new System.Windows.Forms.Timer(this.components);
            this.dvgRta = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dvgRta)).BeginInit();
            this.SuspendLayout();
            // 
            // lbEventos
            // 
            this.lbEventos.FormattingEnabled = true;
            this.lbEventos.HorizontalScrollbar = true;
            this.lbEventos.Location = new System.Drawing.Point(23, 21);
            this.lbEventos.Name = "lbEventos";
            this.lbEventos.ScrollAlwaysVisible = true;
            this.lbEventos.Size = new System.Drawing.Size(1089, 212);
            this.lbEventos.TabIndex = 4;
            // 
            // Inicio
            // 
            this.Inicio.Tick += new System.EventHandler(this.Inicio_Tick);
            // 
            // dvgRta
            // 
            this.dvgRta.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dvgRta.Location = new System.Drawing.Point(23, 405);
            this.dvgRta.Name = "dvgRta";
            this.dvgRta.Size = new System.Drawing.Size(1100, 150);
            this.dvgRta.TabIndex = 5;
            // 
            // SincronizacionFacturacionElectronica
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1173, 687);
            this.Controls.Add(this.dvgRta);
            this.Controls.Add(this.lbEventos);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "SincronizacionFacturacionElectronica";
            this.Text = "SincronizacionFacturacionElectronica";
            this.Load += new System.EventHandler(this.SincronizacionFacturacionElectronica_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dvgRta)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox lbEventos;
        private System.Windows.Forms.Timer Inicio;
        private System.Windows.Forms.DataGridView dvgRta;
    }
}


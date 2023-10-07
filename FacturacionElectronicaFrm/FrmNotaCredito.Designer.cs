namespace FacturacionElectronicaFrm
{
    partial class FrmNotaCredito
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmNotaCredito));
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnConsultar = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.Sede = new System.Windows.Forms.Label();
            this.dtmFecha = new System.Windows.Forms.DateTimePicker();
            this.cboEstacionamientos = new System.Windows.Forms.ComboBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.chkSelect = new System.Windows.Forms.CheckBox();
            this.lblRtaFacturas = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.dvgListadoInterfaz = new System.Windows.Forms.DataGridView();
            this.chkSeleccionar = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.lbEventos = new System.Windows.Forms.ListBox();
            this.dtmFechaFin = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dvgListadoInterfaz)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.dtmFechaFin);
            this.panel1.Controls.Add(this.btnConsultar);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.Sede);
            this.panel1.Controls.Add(this.dtmFecha);
            this.panel1.Controls.Add(this.cboEstacionamientos);
            this.panel1.Location = new System.Drawing.Point(19, 26);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1371, 184);
            this.panel1.TabIndex = 0;
            // 
            // btnConsultar
            // 
            this.btnConsultar.BackColor = System.Drawing.Color.Green;
            this.btnConsultar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConsultar.Font = new System.Drawing.Font("Arial Rounded MT Bold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConsultar.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnConsultar.Location = new System.Drawing.Point(1188, 60);
            this.btnConsultar.Margin = new System.Windows.Forms.Padding(4);
            this.btnConsultar.Name = "btnConsultar";
            this.btnConsultar.Size = new System.Drawing.Size(136, 52);
            this.btnConsultar.TabIndex = 4;
            this.btnConsultar.Text = "Consultar";
            this.btnConsultar.UseVisualStyleBackColor = false;
            this.btnConsultar.Click += new System.EventHandler(this.btnConsultar_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial Rounded MT Bold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(472, 75);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(173, 29);
            this.label2.TabIndex = 3;
            this.label2.Text = "Fecha Desde";
            // 
            // Sede
            // 
            this.Sede.AutoSize = true;
            this.Sede.Font = new System.Drawing.Font("Arial Rounded MT Bold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Sede.Location = new System.Drawing.Point(21, 71);
            this.Sede.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Sede.Name = "Sede";
            this.Sede.Size = new System.Drawing.Size(60, 23);
            this.Sede.TabIndex = 2;
            this.Sede.Text = "Sede";
            // 
            // dtmFecha
            // 
            this.dtmFecha.CustomFormat = "yyyy-MM-dd";
            this.dtmFecha.Font = new System.Drawing.Font("Arial Rounded MT Bold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtmFecha.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtmFecha.Location = new System.Drawing.Point(654, 69);
            this.dtmFecha.Margin = new System.Windows.Forms.Padding(4);
            this.dtmFecha.Name = "dtmFecha";
            this.dtmFecha.Size = new System.Drawing.Size(153, 31);
            this.dtmFecha.TabIndex = 1;
            // 
            // cboEstacionamientos
            // 
            this.cboEstacionamientos.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboEstacionamientos.Font = new System.Drawing.Font("Arial Rounded MT Bold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboEstacionamientos.FormattingEnabled = true;
            this.cboEstacionamientos.Location = new System.Drawing.Point(109, 72);
            this.cboEstacionamientos.Margin = new System.Windows.Forms.Padding(4);
            this.cboEstacionamientos.Name = "cboEstacionamientos";
            this.cboEstacionamientos.Size = new System.Drawing.Size(355, 31);
            this.cboEstacionamientos.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.button3);
            this.panel2.Controls.Add(this.button4);
            this.panel2.Controls.Add(this.button2);
            this.panel2.Controls.Add(this.chkSelect);
            this.panel2.Controls.Add(this.lblRtaFacturas);
            this.panel2.Controls.Add(this.button1);
            this.panel2.Controls.Add(this.dvgListadoInterfaz);
            this.panel2.Location = new System.Drawing.Point(16, 218);
            this.panel2.Margin = new System.Windows.Forms.Padding(4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1374, 302);
            this.panel2.TabIndex = 2;
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.Green;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Font = new System.Drawing.Font("Arial Rounded MT Bold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button3.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.button3.Location = new System.Drawing.Point(1183, 201);
            this.button3.Margin = new System.Windows.Forms.Padding(4);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(148, 66);
            this.button3.TabIndex = 13;
            this.button3.Text = "Cliente Registrado";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button3_Click_2);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(1212, 18);
            this.button4.Margin = new System.Windows.Forms.Padding(4);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(100, 28);
            this.button4.TabIndex = 12;
            this.button4.Text = "button4";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Visible = false;
            this.button4.Click += new System.EventHandler(this.button4_Click_1);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(1212, 80);
            this.button2.Margin = new System.Windows.Forms.Padding(4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(100, 28);
            this.button2.TabIndex = 10;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Visible = false;
            this.button2.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // chkSelect
            // 
            this.chkSelect.AutoSize = true;
            this.chkSelect.Location = new System.Drawing.Point(65, 18);
            this.chkSelect.Margin = new System.Windows.Forms.Padding(4);
            this.chkSelect.Name = "chkSelect";
            this.chkSelect.Size = new System.Drawing.Size(137, 20);
            this.chkSelect.TabIndex = 9;
            this.chkSelect.Text = "Seleccionar Todo";
            this.chkSelect.UseVisualStyleBackColor = true;
            this.chkSelect.CheckedChanged += new System.EventHandler(this.chkSelect_CheckedChanged);
            // 
            // lblRtaFacturas
            // 
            this.lblRtaFacturas.AutoSize = true;
            this.lblRtaFacturas.Font = new System.Drawing.Font("Arial Rounded MT Bold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRtaFacturas.Location = new System.Drawing.Point(212, 15);
            this.lblRtaFacturas.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblRtaFacturas.Name = "lblRtaFacturas";
            this.lblRtaFacturas.Size = new System.Drawing.Size(0, 23);
            this.lblRtaFacturas.TabIndex = 6;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Green;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Arial Rounded MT Bold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.button1.Location = new System.Drawing.Point(1191, 142);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(136, 52);
            this.button1.TabIndex = 5;
            this.button1.Text = "Generar";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // dvgListadoInterfaz
            // 
            this.dvgListadoInterfaz.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dvgListadoInterfaz.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.chkSeleccionar});
            this.dvgListadoInterfaz.Location = new System.Drawing.Point(17, 52);
            this.dvgListadoInterfaz.Margin = new System.Windows.Forms.Padding(4);
            this.dvgListadoInterfaz.Name = "dvgListadoInterfaz";
            this.dvgListadoInterfaz.RowHeadersWidth = 51;
            this.dvgListadoInterfaz.Size = new System.Drawing.Size(1103, 230);
            this.dvgListadoInterfaz.TabIndex = 1;
            // 
            // chkSeleccionar
            // 
            this.chkSeleccionar.HeaderText = "Seleccionar";
            this.chkSeleccionar.MinimumWidth = 6;
            this.chkSeleccionar.Name = "chkSeleccionar";
            this.chkSeleccionar.Visible = false;
            this.chkSeleccionar.Width = 125;
            // 
            // lbEventos
            // 
            this.lbEventos.FormattingEnabled = true;
            this.lbEventos.HorizontalScrollbar = true;
            this.lbEventos.ItemHeight = 16;
            this.lbEventos.Location = new System.Drawing.Point(19, 528);
            this.lbEventos.Margin = new System.Windows.Forms.Padding(4);
            this.lbEventos.Name = "lbEventos";
            this.lbEventos.ScrollAlwaysVisible = true;
            this.lbEventos.Size = new System.Drawing.Size(1604, 260);
            this.lbEventos.TabIndex = 3;
            // 
            // dtmFechaFin
            // 
            this.dtmFechaFin.CustomFormat = "yyyy-MM-dd";
            this.dtmFechaFin.Font = new System.Drawing.Font("Arial Rounded MT Bold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtmFechaFin.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtmFechaFin.Location = new System.Drawing.Point(918, 69);
            this.dtmFechaFin.Margin = new System.Windows.Forms.Padding(4);
            this.dtmFechaFin.Name = "dtmFechaFin";
            this.dtmFechaFin.Size = new System.Drawing.Size(153, 31);
            this.dtmFechaFin.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial Rounded MT Bold", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(831, 71);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 23);
            this.label1.TabIndex = 6;
            this.label1.Text = "Hasta";
            // 
            // FrmNotaCredito
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1665, 841);
            this.Controls.Add(this.lbEventos);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FrmNotaCredito";
            this.Text = "FrmNotaCredito";
            this.Load += new System.EventHandler(this.FrmNotaCredito_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dvgListadoInterfaz)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label Sede;
        private System.Windows.Forms.DateTimePicker dtmFecha;
        private System.Windows.Forms.ComboBox cboEstacionamientos;
        private System.Windows.Forms.Button btnConsultar;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridView dvgListadoInterfaz;
        private System.Windows.Forms.DataGridViewCheckBoxColumn chkSeleccionar;
        private System.Windows.Forms.Label lblRtaFacturas;
        private System.Windows.Forms.ListBox lbEventos;
        private System.Windows.Forms.CheckBox chkSelect;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtmFechaFin;
    }
}
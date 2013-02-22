namespace Prototype_Token_Interface
{
    partial class Edit_Keys
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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			this.DATAGRID_Keys = new System.Windows.Forms.DataGridView();
			this.LABEL_SuperColumnHeader = new System.Windows.Forms.Label();
			this.TEXTBOX_Instruction = new System.Windows.Forms.TextBox();
			this.BUTTON_OK = new System.Windows.Forms.Button();
			this.BUTTON_Cancel = new System.Windows.Forms.Button();
			( ( System.ComponentModel.ISupportInitialize )( this.DATAGRID_Keys ) ).BeginInit();
			this.SuspendLayout();
			// 
			// DATAGRID_Keys
			// 
			this.DATAGRID_Keys.AllowUserToAddRows = false;
			this.DATAGRID_Keys.AllowUserToDeleteRows = false;
			this.DATAGRID_Keys.AllowUserToResizeColumns = false;
			this.DATAGRID_Keys.AllowUserToResizeRows = false;
			this.DATAGRID_Keys.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.DATAGRID_Keys.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.DATAGRID_Keys.Location = new System.Drawing.Point( 12, 72 );
			this.DATAGRID_Keys.Name = "DATAGRID_Keys";
			this.DATAGRID_Keys.ReadOnly = true;
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle1.Font = new System.Drawing.Font( "Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ) );
			dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.DATAGRID_Keys.RowHeadersDefaultCellStyle = dataGridViewCellStyle1;
			this.DATAGRID_Keys.Size = new System.Drawing.Size( 356, 544 );
			this.DATAGRID_Keys.TabIndex = 0;
			this.DATAGRID_Keys.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler( this.DATAGRID_Keys_CellValueChanged );
			this.DATAGRID_Keys.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler( this.DATAGRID_Keys_Row_Column_Cell_Enter );
			this.DATAGRID_Keys.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler( this.DATAGRID_Keys_RowPostPaint );
			this.DATAGRID_Keys.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler( this.DATAGRID_Keys_PreviewKeyDown );
			this.DATAGRID_Keys.RowLeave += new System.Windows.Forms.DataGridViewCellEventHandler( this.DATAGRID_Keys_RowLeave );
			this.DATAGRID_Keys.MouseHover += new System.EventHandler( this.DATAGRID_Keys_MouseHover );
			this.DATAGRID_Keys.CurrentCellChanged += new System.EventHandler( this.DATAGRID_Keys_Row_Column_Cell_Enter );
			this.DATAGRID_Keys.KeyDown += new System.Windows.Forms.KeyEventHandler( this.DATAGRID_Keys_KeyDown );
			this.DATAGRID_Keys.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler( this.DATAGRID_Keys_Row_Column_Cell_Enter );
			this.DATAGRID_Keys.Click += new System.EventHandler( this.DATAGRID_Keys_Row_Column_Cell_Enter );
			// 
			// LABEL_SuperColumnHeader
			// 
			this.LABEL_SuperColumnHeader.AutoSize = true;
			this.LABEL_SuperColumnHeader.Font = new System.Drawing.Font( "Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ) );
			this.LABEL_SuperColumnHeader.Location = new System.Drawing.Point( 233, 50 );
			this.LABEL_SuperColumnHeader.Name = "LABEL_SuperColumnHeader";
			this.LABEL_SuperColumnHeader.Size = new System.Drawing.Size( 139, 21 );
			this.LABEL_SuperColumnHeader.TabIndex = 1;
			this.LABEL_SuperColumnHeader.Text = "Keyboard Shortcut";
			// 
			// TEXTBOX_Instruction
			// 
			this.TEXTBOX_Instruction.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.TEXTBOX_Instruction.Font = new System.Drawing.Font( "Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ) );
			this.TEXTBOX_Instruction.Location = new System.Drawing.Point( 12, 10 );
			this.TEXTBOX_Instruction.Multiline = true;
			this.TEXTBOX_Instruction.Name = "TEXTBOX_Instruction";
			this.TEXTBOX_Instruction.ReadOnly = true;
			this.TEXTBOX_Instruction.Size = new System.Drawing.Size( 355, 33 );
			this.TEXTBOX_Instruction.TabIndex = 3;
			this.TEXTBOX_Instruction.Text = "Select a shortcut in the list below, and type the shortcut as you would invoke it" +
				" (all at once).  To erase a shortcut, hit \"Backspace\".";
			// 
			// BUTTON_OK
			// 
			this.BUTTON_OK.Font = new System.Drawing.Font( "Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ) );
			this.BUTTON_OK.Location = new System.Drawing.Point( 12, 628 );
			this.BUTTON_OK.Name = "BUTTON_OK";
			this.BUTTON_OK.Size = new System.Drawing.Size( 150, 30 );
			this.BUTTON_OK.TabIndex = 4;
			this.BUTTON_OK.Text = "OK";
			this.BUTTON_OK.UseVisualStyleBackColor = true;
			this.BUTTON_OK.Click += new System.EventHandler( this.BUTTON_OK_Click );
			// 
			// BUTTON_Cancel
			// 
			this.BUTTON_Cancel.Font = new System.Drawing.Font( "Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ) );
			this.BUTTON_Cancel.Location = new System.Drawing.Point( 218, 628 );
			this.BUTTON_Cancel.Name = "BUTTON_Cancel";
			this.BUTTON_Cancel.Size = new System.Drawing.Size( 150, 30 );
			this.BUTTON_Cancel.TabIndex = 5;
			this.BUTTON_Cancel.Text = "Cancel";
			this.BUTTON_Cancel.UseVisualStyleBackColor = true;
			this.BUTTON_Cancel.Click += new System.EventHandler( this.BUTTON_Cancel_Click );
			// 
			// Edit_Keys
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size( 379, 668 );
			this.Controls.Add( this.BUTTON_Cancel );
			this.Controls.Add( this.BUTTON_OK );
			this.Controls.Add( this.DATAGRID_Keys );
			this.Controls.Add( this.LABEL_SuperColumnHeader );
			this.Controls.Add( this.TEXTBOX_Instruction );
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.KeyPreview = true;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "Edit_Keys";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Customize the Toolbar\'s shortcuts.";
			this.Load += new System.EventHandler( this.Edit_Keys_Load );
			this.KeyUp += new System.Windows.Forms.KeyEventHandler( this.Edit_Keys_KeyUp );
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler( this.Edit_Keys_FormClosing );
			( ( System.ComponentModel.ISupportInitialize )( this.DATAGRID_Keys ) ).EndInit();
			this.ResumeLayout( false );
			this.PerformLayout();

        }

        #endregion

		private System.Windows.Forms.DataGridView DATAGRID_Keys;
		private System.Windows.Forms.Label LABEL_SuperColumnHeader;
		private System.Windows.Forms.TextBox TEXTBOX_Instruction;
		private System.Windows.Forms.Button BUTTON_OK;
		private System.Windows.Forms.Button BUTTON_Cancel;

    }
}
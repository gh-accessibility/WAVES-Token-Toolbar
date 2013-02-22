namespace Prototype_Token_Interface
{
    partial class Level3
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( bool disposing )
        {
            if ( disposing && ( components != null ) )
            {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			this.components = new System.ComponentModel.Container();
			this.TOOLTIP_Display = new System.Windows.Forms.ToolTip( this.components );
			this.SuspendLayout();
			// 
			// TOOLTIP_Display
			// 
			this.TOOLTIP_Display.AutoPopDelay = 5000;
			this.TOOLTIP_Display.InitialDelay = 500;
			this.TOOLTIP_Display.IsBalloon = true;
			this.TOOLTIP_Display.ReshowDelay = 250;
			// 
			// Level3
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size( 596, 165 );
			this.KeyPreview = true;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "Level3";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Exhaustive Token and Element Repository";
			this.TopMost = true;
			this.Load += new System.EventHandler( this.Level3_Load );
			this.KeyUp += new System.Windows.Forms.KeyEventHandler( this.Level3_KeyUp );
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler( this.Level3_FormClosing );
			this.Resize += new System.EventHandler( this.Level3_Resize );
			this.KeyDown += new System.Windows.Forms.KeyEventHandler( this.Level3_KeyDown );
			this.ResumeLayout( false );

        }

        #endregion

		private System.Windows.Forms.ToolTip TOOLTIP_Display;


    }
}
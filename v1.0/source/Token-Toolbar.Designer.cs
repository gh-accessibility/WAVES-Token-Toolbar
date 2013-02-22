namespace Prototype_Token_Interface
{
    partial class Token_Toolbar
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager( typeof( Token_Toolbar ) );
			this.MATHML_Display = new MathML.Rendering.MathMLControl();
			this.BUTTON_ShowLevel2 = new System.Windows.Forms.Button();
			this.BUTTON_ShowLevel3 = new System.Windows.Forms.Button();
			this.TEXTBOX_MathExpression = new System.Windows.Forms.TextBox();
			this.LABEL_Wrapper = new System.Windows.Forms.Label();
			this.LISTBOX_ShowExcerpts = new System.Windows.Forms.ListBox();
			this.LABEL_Warning = new System.Windows.Forms.Label();
			this.MENU_bar = new System.Windows.Forms.MenuStrip();
			this.MENU_Formula = new System.Windows.Forms.ToolStripMenuItem();
			this.MENU_Formula_New = new System.Windows.Forms.ToolStripMenuItem();
			this.MENU_Formula_ImportMathML = new System.Windows.Forms.ToolStripMenuItem();
			this.MENU_Formula_Undo = new System.Windows.Forms.ToolStripMenuItem();
			this.MENU_Formula_ClearExcerpt = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.MENU_Formula_Read = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.MENU_Formula_CopyClipboard = new System.Windows.Forms.ToolStripMenuItem();
			this.MENU_Formula_ExportMathML = new System.Windows.Forms.ToolStripMenuItem();
			this.MENU_Formula_ExportGraphic = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
			this.MENU_Formula_Close = new System.Windows.Forms.ToolStripMenuItem();
			this.MENU_View = new System.Windows.Forms.ToolStripMenuItem();
			this.MENU_View_AlwaysOnTop = new System.Windows.Forms.ToolStripMenuItem();
			this.MENU_View_HideMenu = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
			this.MENU_View_MathJax = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
			this.MENU_View_Level2 = new System.Windows.Forms.ToolStripMenuItem();
			this.MENU_View_Repository = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
			this.MENU_View_Autotune = new System.Windows.Forms.ToolStripMenuItem();
			this.MENU_Options = new System.Windows.Forms.ToolStripMenuItem();
			this.MENU_Options_EnglishExcerpts = new System.Windows.Forms.ToolStripMenuItem();
			this.MENU_Options_HighContrast = new System.Windows.Forms.ToolStripMenuItem();
			this.MENU_Options_ReadAsPower = new System.Windows.Forms.ToolStripMenuItem();
			this.MENU_Options_VoicePrompting = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.MENU_Options_EditKeyboard = new System.Windows.Forms.ToolStripMenuItem();
			this.MENU_Options_EditMasterTokenList = new System.Windows.Forms.ToolStripMenuItem();
			this.MENU_Help = new System.Windows.Forms.ToolStripMenuItem();
			this.MENU_Help_Index = new System.Windows.Forms.ToolStripMenuItem();
			this.MENU_Help_Shortcuts = new System.Windows.Forms.ToolStripMenuItem();
			this.MENU_Help_About = new System.Windows.Forms.ToolStripMenuItem();
			this.BUTTON_Close = new System.Windows.Forms.Button();
			this.DIALOG_Save = new System.Windows.Forms.SaveFileDialog();
			this.DIALOG_Load = new System.Windows.Forms.OpenFileDialog();
			this.BUTTON_MultiFunction = new System.Windows.Forms.Button();
			this.listBox1 = new System.Windows.Forms.ListBox();
			this.TOOLTIP_Display = new System.Windows.Forms.ToolTip( this.components );
			this.listBox2 = new System.Windows.Forms.ListBox();
			this.BROWSER_MathJaxRenderer = new System.Windows.Forms.WebBrowser();
			this.MENU_bar.SuspendLayout();
			this.SuspendLayout();
			// 
			// MATHML_Display
			// 
			this.MATHML_Display.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.MATHML_Display.BackColor = System.Drawing.Color.FromArgb( ( ( int )( ( ( byte )( 255 ) ) ) ), ( ( int )( ( ( byte )( 255 ) ) ) ), ( ( int )( ( ( byte )( 128 ) ) ) ) );
			this.MATHML_Display.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.MATHML_Display.Font = new System.Drawing.Font( "DejaVu Sans Condensed", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ) );
			this.MATHML_Display.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
			this.MATHML_Display.HorizontalShift = 0F;
			this.MATHML_Display.InputLocation = null;
			this.MATHML_Display.Location = new System.Drawing.Point( 7, 485 );
			this.MATHML_Display.MathElement = null;
			this.MATHML_Display.MathFontSizeInPoints = 16;
			this.MATHML_Display.Name = "MATHML_Display";
			this.MATHML_Display.Padding = new System.Windows.Forms.Padding( 10 );
			this.MATHML_Display.ReadOnly = true;
			this.MATHML_Display.SelectedElement = null;
			this.MATHML_Display.SelectionColor = System.Drawing.Color.LightBlue;
			this.MATHML_Display.Size = new System.Drawing.Size( 380, 80 );
			this.MATHML_Display.TabIndex = 160;
			this.MATHML_Display.TabStop = false;
			this.TOOLTIP_Display.SetToolTip( this.MATHML_Display, "A graphical rendering of the MathML constructed from the current formula." );
			this.MATHML_Display.VerticalShift = 0F;
			this.MATHML_Display.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler( this.MATHML_Display_PreviewKeyDown );
			// 
			// BUTTON_ShowLevel2
			// 
			this.BUTTON_ShowLevel2.Font = new System.Drawing.Font( "Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ) );
			this.BUTTON_ShowLevel2.Location = new System.Drawing.Point( 498, 79 );
			this.BUTTON_ShowLevel2.Name = "BUTTON_ShowLevel2";
			this.BUTTON_ShowLevel2.Size = new System.Drawing.Size( 75, 30 );
			this.BUTTON_ShowLevel2.TabIndex = 200;
			this.BUTTON_ShowLevel2.TabStop = false;
			this.BUTTON_ShowLevel2.Tag = "Level Too Toggle";
			this.BUTTON_ShowLevel2.Text = "Level 2 ↓";
			this.BUTTON_ShowLevel2.UseVisualStyleBackColor = true;
			this.BUTTON_ShowLevel2.Visible = false;
			this.BUTTON_ShowLevel2.Click += new System.EventHandler( this.MENU_View_Level2_Click );
			this.BUTTON_ShowLevel2.Enter += new System.EventHandler( this.Button_Static_Enter );
			this.BUTTON_ShowLevel2.MouseEnter += new System.EventHandler( this.Button_Static_Enter );
			// 
			// BUTTON_ShowLevel3
			// 
			this.BUTTON_ShowLevel3.Font = new System.Drawing.Font( "Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ) );
			this.BUTTON_ShowLevel3.Location = new System.Drawing.Point( 498, 115 );
			this.BUTTON_ShowLevel3.Name = "BUTTON_ShowLevel3";
			this.BUTTON_ShowLevel3.Size = new System.Drawing.Size( 75, 30 );
			this.BUTTON_ShowLevel3.TabIndex = 300;
			this.BUTTON_ShowLevel3.TabStop = false;
			this.BUTTON_ShowLevel3.Tag = "Open Level Three";
			this.BUTTON_ShowLevel3.Text = "Level 3 →";
			this.BUTTON_ShowLevel3.UseVisualStyleBackColor = true;
			this.BUTTON_ShowLevel3.Visible = false;
			this.BUTTON_ShowLevel3.Click += new System.EventHandler( this.MENU_View_Repository_Click );
			this.BUTTON_ShowLevel3.Enter += new System.EventHandler( this.Button_Static_Enter );
			this.BUTTON_ShowLevel3.MouseEnter += new System.EventHandler( this.Button_Static_Enter );
			// 
			// TEXTBOX_MathExpression
			// 
			this.TEXTBOX_MathExpression.BackColor = System.Drawing.Color.White;
			this.TEXTBOX_MathExpression.CausesValidation = false;
			this.TEXTBOX_MathExpression.Font = new System.Drawing.Font( "Lucida Sans Unicode", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ) );
			this.TEXTBOX_MathExpression.Location = new System.Drawing.Point( 46, 211 );
			this.TEXTBOX_MathExpression.Name = "TEXTBOX_MathExpression";
			this.TEXTBOX_MathExpression.ReadOnly = true;
			this.TEXTBOX_MathExpression.Size = new System.Drawing.Size( 341, 27 );
			this.TEXTBOX_MathExpression.TabIndex = 105;
			this.TEXTBOX_MathExpression.TabStop = false;
			this.TOOLTIP_Display.SetToolTip( this.TEXTBOX_MathExpression, "A textual representation of the selected line" );
			this.TEXTBOX_MathExpression.TextChanged += new System.EventHandler( this.TEXTBOX_MathExpression_TextChanged );
			this.TEXTBOX_MathExpression.MouseMove += new System.Windows.Forms.MouseEventHandler( this.TEXTBOX_MathExpression_MouseMove );
			this.TEXTBOX_MathExpression.Leave += new System.EventHandler( this.TEXTBOX_MathExpression_Leave );
			this.TEXTBOX_MathExpression.MouseDown += new System.Windows.Forms.MouseEventHandler( this.TEXTBOX_MathExpression_MouseDown );
			// 
			// LABEL_Wrapper
			// 
			this.LABEL_Wrapper.AutoSize = true;
			this.LABEL_Wrapper.Font = new System.Drawing.Font( "Segoe UI", 8.25F );
			this.LABEL_Wrapper.Location = new System.Drawing.Point( 4, 216 );
			this.LABEL_Wrapper.Name = "LABEL_Wrapper";
			this.LABEL_Wrapper.Size = new System.Drawing.Size( 36, 13 );
			this.LABEL_Wrapper.TabIndex = 102;
			this.LABEL_Wrapper.Text = "mrow";
			this.TOOLTIP_Display.SetToolTip( this.LABEL_Wrapper, "A description of the selected line" );
			this.LABEL_Wrapper.Resize += new System.EventHandler( this.LABEL_Wrapper_Resize );
			// 
			// LISTBOX_ShowExcerpts
			// 
			this.LISTBOX_ShowExcerpts.BackColor = System.Drawing.Color.FromArgb( ( ( int )( ( ( byte )( 224 ) ) ) ), ( ( int )( ( ( byte )( 224 ) ) ) ), ( ( int )( ( ( byte )( 255 ) ) ) ) );
			this.LISTBOX_ShowExcerpts.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
			this.LISTBOX_ShowExcerpts.Font = new System.Drawing.Font( "Lucida Sans Unicode", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ) );
			this.LISTBOX_ShowExcerpts.FormattingEnabled = true;
			this.LISTBOX_ShowExcerpts.IntegralHeight = false;
			this.LISTBOX_ShowExcerpts.ItemHeight = 16;
			this.LISTBOX_ShowExcerpts.Location = new System.Drawing.Point( 7, 28 );
			this.LISTBOX_ShowExcerpts.Name = "LISTBOX_ShowExcerpts";
			this.LISTBOX_ShowExcerpts.Size = new System.Drawing.Size( 380, 177 );
			this.LISTBOX_ShowExcerpts.TabIndex = 99;
			this.LISTBOX_ShowExcerpts.TabStop = false;
			this.TOOLTIP_Display.SetToolTip( this.LISTBOX_ShowExcerpts, "A list-based representation of the math formula. Use Up and Down arrow keys to na" +
					"vigate list." );
			this.LISTBOX_ShowExcerpts.DrawItem += new System.Windows.Forms.DrawItemEventHandler( this.LISTBOX_PaintListItems );
			this.LISTBOX_ShowExcerpts.MeasureItem += new System.Windows.Forms.MeasureItemEventHandler( this.LISTBOX_ShowExcerpts_MeasureItem );
			this.LISTBOX_ShowExcerpts.SelectedIndexChanged += new System.EventHandler( this.LISTBOX_XMLpieces_SelectedIndexChanged );
			this.LISTBOX_ShowExcerpts.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler( this.LISTBOX_ShowExcerpts_PreviewKeyDown );
			this.LISTBOX_ShowExcerpts.KeyDown += new System.Windows.Forms.KeyEventHandler( this.LISTBOX_ShowExcerpts_KeyDown );
			// 
			// LABEL_Warning
			// 
			this.LABEL_Warning.AutoSize = true;
			this.LABEL_Warning.BackColor = System.Drawing.Color.FromArgb( ( ( int )( ( ( byte )( 255 ) ) ) ), ( ( int )( ( ( byte )( 255 ) ) ) ), ( ( int )( ( ( byte )( 128 ) ) ) ) );
			this.LABEL_Warning.Font = new System.Drawing.Font( "Segoe UI Semibold", 15.75F, ( ( System.Drawing.FontStyle )( ( System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic ) ) ), System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ) );
			this.LABEL_Warning.ForeColor = System.Drawing.Color.FromArgb( ( ( int )( ( ( byte )( 192 ) ) ) ), ( ( int )( ( ( byte )( 0 ) ) ) ), ( ( int )( ( ( byte )( 0 ) ) ) ) );
			this.LABEL_Warning.Location = new System.Drawing.Point( 124, 511 );
			this.LABEL_Warning.Name = "LABEL_Warning";
			this.LABEL_Warning.Size = new System.Drawing.Size( 146, 30 );
			this.LABEL_Warning.TabIndex = 101;
			this.LABEL_Warning.Text = "MathML error";
			this.TOOLTIP_Display.SetToolTip( this.LABEL_Warning, "Cannot render formula-- fix error in formula.  Usually errors stem from having em" +
					"pty lines above." );
			this.LABEL_Warning.Visible = false;
			// 
			// MENU_bar
			// 
			this.MENU_bar.Items.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.MENU_Formula,
            this.MENU_View,
            this.MENU_Options,
            this.MENU_Help} );
			this.MENU_bar.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
			this.MENU_bar.Location = new System.Drawing.Point( 0, 0 );
			this.MENU_bar.Name = "MENU_bar";
			this.MENU_bar.Size = new System.Drawing.Size( 390, 24 );
			this.MENU_bar.TabIndex = 50;
			this.MENU_bar.Text = "menuStrip1";
			this.MENU_bar.MenuActivate += new System.EventHandler( this.MENU_bar_MenuActivate );
			this.MENU_bar.Enter += new System.EventHandler( this.MENU_bar_Enter );
			// 
			// MENU_Formula
			// 
			this.MENU_Formula.AccessibleDescription = "Formula Menu";
			this.MENU_Formula.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.MENU_Formula_New,
            this.MENU_Formula_ImportMathML,
            this.MENU_Formula_Undo,
            this.MENU_Formula_ClearExcerpt,
            this.toolStripSeparator1,
            this.MENU_Formula_Read,
            this.toolStripSeparator2,
            this.MENU_Formula_CopyClipboard,
            this.MENU_Formula_ExportMathML,
            this.MENU_Formula_ExportGraphic,
            this.toolStripSeparator5,
            this.MENU_Formula_Close} );
			this.MENU_Formula.Name = "MENU_Formula";
			this.MENU_Formula.Size = new System.Drawing.Size( 63, 20 );
			this.MENU_Formula.Tag = "Formula Menu";
			this.MENU_Formula.Text = "Formula";
			this.MENU_Formula.Paint += new System.Windows.Forms.PaintEventHandler( this.MENU_SpeakIdentity );
			this.MENU_Formula.MouseEnter += new System.EventHandler( this.MENU_SpeakIdentity );
			this.MENU_Formula.DropDownOpening += new System.EventHandler( this.MENU_OnMenuBar_DropDownOpening );
			this.MENU_Formula.DropDownClosed += new System.EventHandler( this.MENU_OnMenuBar_DropDownClosed );
			// 
			// MENU_Formula_New
			// 
			this.MENU_Formula_New.Image = ( ( System.Drawing.Image )( resources.GetObject( "MENU_Formula_New.Image" ) ) );
			this.MENU_Formula_New.Name = "MENU_Formula_New";
			this.MENU_Formula_New.Size = new System.Drawing.Size( 181, 22 );
			this.MENU_Formula_New.Tag = "New Formula";
			this.MENU_Formula_New.Text = "New";
			this.MENU_Formula_New.Click += new System.EventHandler( this.MENU_Formula_New_Click );
			// 
			// MENU_Formula_ImportMathML
			// 
			this.MENU_Formula_ImportMathML.Image = ( ( System.Drawing.Image )( resources.GetObject( "MENU_Formula_ImportMathML.Image" ) ) );
			this.MENU_Formula_ImportMathML.Name = "MENU_Formula_ImportMathML";
			this.MENU_Formula_ImportMathML.Size = new System.Drawing.Size( 181, 22 );
			this.MENU_Formula_ImportMathML.Tag = "Import Math M L from file";
			this.MENU_Formula_ImportMathML.Text = "Import MathML ...";
			this.MENU_Formula_ImportMathML.Click += new System.EventHandler( this.MENU_Formula_ImportMathML_Click );
			// 
			// MENU_Formula_Undo
			// 
			this.MENU_Formula_Undo.Name = "MENU_Formula_Undo";
			this.MENU_Formula_Undo.Size = new System.Drawing.Size( 181, 22 );
			this.MENU_Formula_Undo.Tag = "Undo Last Change";
			this.MENU_Formula_Undo.Text = "Undo";
			this.MENU_Formula_Undo.Click += new System.EventHandler( this.MENU_Formula_Undo_Click );
			// 
			// MENU_Formula_ClearExcerpt
			// 
			this.MENU_Formula_ClearExcerpt.Name = "MENU_Formula_ClearExcerpt";
			this.MENU_Formula_ClearExcerpt.Size = new System.Drawing.Size( 181, 22 );
			this.MENU_Formula_ClearExcerpt.Tag = "Clear Excerpt";
			this.MENU_Formula_ClearExcerpt.Text = "Clear Excerpt";
			this.MENU_Formula_ClearExcerpt.Click += new System.EventHandler( this.MENU_Formula_ClearExcerpt_Click );
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size( 178, 6 );
			// 
			// MENU_Formula_Read
			// 
			this.MENU_Formula_Read.Name = "MENU_Formula_Read";
			this.MENU_Formula_Read.Size = new System.Drawing.Size( 181, 22 );
			this.MENU_Formula_Read.Tag = "Read Math M L Formula";
			this.MENU_Formula_Read.Text = "Read";
			this.MENU_Formula_Read.Click += new System.EventHandler( this.MENU_Formula_Read_Click );
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size( 178, 6 );
			// 
			// MENU_Formula_CopyClipboard
			// 
			this.MENU_Formula_CopyClipboard.Image = ( ( System.Drawing.Image )( resources.GetObject( "MENU_Formula_CopyClipboard.Image" ) ) );
			this.MENU_Formula_CopyClipboard.Name = "MENU_Formula_CopyClipboard";
			this.MENU_Formula_CopyClipboard.Size = new System.Drawing.Size( 181, 22 );
			this.MENU_Formula_CopyClipboard.Tag = "Copy Math M L to Clipboard";
			this.MENU_Formula_CopyClipboard.Text = "Copy to Clipboard";
			this.MENU_Formula_CopyClipboard.Click += new System.EventHandler( this.MENU_Formula_CopyClipboard_Click );
			// 
			// MENU_Formula_ExportMathML
			// 
			this.MENU_Formula_ExportMathML.Image = ( ( System.Drawing.Image )( resources.GetObject( "MENU_Formula_ExportMathML.Image" ) ) );
			this.MENU_Formula_ExportMathML.Name = "MENU_Formula_ExportMathML";
			this.MENU_Formula_ExportMathML.Size = new System.Drawing.Size( 181, 22 );
			this.MENU_Formula_ExportMathML.Tag = "Export formula as Math M L";
			this.MENU_Formula_ExportMathML.Text = "Export as MathML ...";
			this.MENU_Formula_ExportMathML.Click += new System.EventHandler( this.MENU_Formula_ExportMathML_Click );
			// 
			// MENU_Formula_ExportGraphic
			// 
			this.MENU_Formula_ExportGraphic.Name = "MENU_Formula_ExportGraphic";
			this.MENU_Formula_ExportGraphic.Size = new System.Drawing.Size( 181, 22 );
			this.MENU_Formula_ExportGraphic.Tag = "Export math as picture";
			this.MENU_Formula_ExportGraphic.Text = "Export as graphic ...";
			this.MENU_Formula_ExportGraphic.Click += new System.EventHandler( this.MENU_Formula_ExportGraphic_Click );
			// 
			// toolStripSeparator5
			// 
			this.toolStripSeparator5.Name = "toolStripSeparator5";
			this.toolStripSeparator5.Size = new System.Drawing.Size( 178, 6 );
			// 
			// MENU_Formula_Close
			// 
			this.MENU_Formula_Close.Image = ( ( System.Drawing.Image )( resources.GetObject( "MENU_Formula_Close.Image" ) ) );
			this.MENU_Formula_Close.Name = "MENU_Formula_Close";
			this.MENU_Formula_Close.Size = new System.Drawing.Size( 181, 22 );
			this.MENU_Formula_Close.Tag = "Close toolbar";
			this.MENU_Formula_Close.Text = "Close";
			this.MENU_Formula_Close.Click += new System.EventHandler( this.MENU_Formula_Close_Click );
			// 
			// MENU_View
			// 
			this.MENU_View.AccessibleDescription = "View Menu";
			this.MENU_View.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.MENU_View_AlwaysOnTop,
            this.MENU_View_HideMenu,
            this.toolStripSeparator7,
            this.MENU_View_MathJax,
            this.toolStripSeparator4,
            this.MENU_View_Level2,
            this.MENU_View_Repository,
            this.toolStripSeparator6,
            this.MENU_View_Autotune} );
			this.MENU_View.Name = "MENU_View";
			this.MENU_View.Size = new System.Drawing.Size( 44, 20 );
			this.MENU_View.Tag = "View Menu";
			this.MENU_View.Text = "View";
			this.MENU_View.DropDownOpening += new System.EventHandler( this.MENU_OnMenuBar_DropDownOpening );
			this.MENU_View.DropDownClosed += new System.EventHandler( this.MENU_OnMenuBar_DropDownClosed );
			// 
			// MENU_View_AlwaysOnTop
			// 
			this.MENU_View_AlwaysOnTop.Name = "MENU_View_AlwaysOnTop";
			this.MENU_View_AlwaysOnTop.Size = new System.Drawing.Size( 230, 22 );
			this.MENU_View_AlwaysOnTop.Text = "Always on Top";
			this.MENU_View_AlwaysOnTop.Click += new System.EventHandler( this.MENU_View_AlwaysOnTop_Click );
			// 
			// MENU_View_HideMenu
			// 
			this.MENU_View_HideMenu.Name = "MENU_View_HideMenu";
			this.MENU_View_HideMenu.Size = new System.Drawing.Size( 230, 22 );
			this.MENU_View_HideMenu.Tag = "Hide Menu";
			this.MENU_View_HideMenu.Text = "Hide Menu";
			this.MENU_View_HideMenu.Click += new System.EventHandler( this.MENU_View_HideMenu_Click );
			// 
			// toolStripSeparator7
			// 
			this.toolStripSeparator7.Name = "toolStripSeparator7";
			this.toolStripSeparator7.Size = new System.Drawing.Size( 227, 6 );
			// 
			// MENU_View_MathJax
			// 
			this.MENU_View_MathJax.Name = "MENU_View_MathJax";
			this.MENU_View_MathJax.Size = new System.Drawing.Size( 230, 22 );
			this.MENU_View_MathJax.Tag = "Use external MathJax renderer";
			this.MENU_View_MathJax.Text = "Use external MathJax renderer";
			this.MENU_View_MathJax.Click += new System.EventHandler( this.MENU_View_MathJax_Click );
			// 
			// toolStripSeparator4
			// 
			this.toolStripSeparator4.Name = "toolStripSeparator4";
			this.toolStripSeparator4.Size = new System.Drawing.Size( 227, 6 );
			// 
			// MENU_View_Level2
			// 
			this.MENU_View_Level2.Name = "MENU_View_Level2";
			this.MENU_View_Level2.Size = new System.Drawing.Size( 230, 22 );
			this.MENU_View_Level2.Tag = "Show Level 2 Tokens";
			this.MENU_View_Level2.Text = "Show Level 2 Tokens";
			this.MENU_View_Level2.Click += new System.EventHandler( this.MENU_View_Level2_Click );
			// 
			// MENU_View_Repository
			// 
			this.MENU_View_Repository.Name = "MENU_View_Repository";
			this.MENU_View_Repository.Size = new System.Drawing.Size( 230, 22 );
			this.MENU_View_Repository.Tag = "Open Token Repository";
			this.MENU_View_Repository.Text = "Show Token Repository ...";
			this.MENU_View_Repository.Click += new System.EventHandler( this.MENU_View_Repository_Click );
			// 
			// toolStripSeparator6
			// 
			this.toolStripSeparator6.Name = "toolStripSeparator6";
			this.toolStripSeparator6.Size = new System.Drawing.Size( 227, 6 );
			this.toolStripSeparator6.Visible = false;
			// 
			// MENU_View_Autotune
			// 
			this.MENU_View_Autotune.Enabled = false;
			this.MENU_View_Autotune.Name = "MENU_View_Autotune";
			this.MENU_View_Autotune.Size = new System.Drawing.Size( 230, 22 );
			this.MENU_View_Autotune.Text = "Auto-tune to Math Book";
			this.MENU_View_Autotune.Visible = false;
			this.MENU_View_Autotune.Click += new System.EventHandler( this.MENU_View_Autotune_Click );
			// 
			// MENU_Options
			// 
			this.MENU_Options.AccessibleDescription = "Options Menu";
			this.MENU_Options.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.MENU_Options_EnglishExcerpts,
            this.MENU_Options_HighContrast,
            this.MENU_Options_ReadAsPower,
            this.MENU_Options_VoicePrompting,
            this.toolStripSeparator3,
            this.MENU_Options_EditKeyboard,
            this.MENU_Options_EditMasterTokenList} );
			this.MENU_Options.Name = "MENU_Options";
			this.MENU_Options.Size = new System.Drawing.Size( 61, 20 );
			this.MENU_Options.Tag = "Options Menu";
			this.MENU_Options.Text = "Options";
			this.MENU_Options.DropDownOpening += new System.EventHandler( this.MENU_OnMenuBar_DropDownOpening );
			this.MENU_Options.DropDownClosed += new System.EventHandler( this.MENU_OnMenuBar_DropDownClosed );
			// 
			// MENU_Options_EnglishExcerpts
			// 
			this.MENU_Options_EnglishExcerpts.Name = "MENU_Options_EnglishExcerpts";
			this.MENU_Options_EnglishExcerpts.Size = new System.Drawing.Size( 212, 22 );
			this.MENU_Options_EnglishExcerpts.Text = "English Excerpts";
			this.MENU_Options_EnglishExcerpts.Click += new System.EventHandler( this.MENU_Options_EnglishExcerpts_Click );
			// 
			// MENU_Options_HighContrast
			// 
			this.MENU_Options_HighContrast.Name = "MENU_Options_HighContrast";
			this.MENU_Options_HighContrast.Size = new System.Drawing.Size( 212, 22 );
			this.MENU_Options_HighContrast.Tag = "High Contrast";
			this.MENU_Options_HighContrast.Text = "High Contrast";
			this.MENU_Options_HighContrast.Click += new System.EventHandler( this.MENU_Options_HighContrast_Click );
			// 
			// MENU_Options_ReadAsPower
			// 
			this.MENU_Options_ReadAsPower.Name = "MENU_Options_ReadAsPower";
			this.MENU_Options_ReadAsPower.Size = new System.Drawing.Size( 212, 22 );
			this.MENU_Options_ReadAsPower.Tag = "Read Superscript as Power";
			this.MENU_Options_ReadAsPower.Text = "Read Superscript as Power";
			this.MENU_Options_ReadAsPower.Click += new System.EventHandler( this.MENU_Options_ReadAsPower_Click );
			// 
			// MENU_Options_VoicePrompting
			// 
			this.MENU_Options_VoicePrompting.Name = "MENU_Options_VoicePrompting";
			this.MENU_Options_VoicePrompting.Size = new System.Drawing.Size( 212, 22 );
			this.MENU_Options_VoicePrompting.Tag = "Voice Prompting";
			this.MENU_Options_VoicePrompting.Text = "Voice Prompting";
			this.MENU_Options_VoicePrompting.Click += new System.EventHandler( this.MENU_Options_VoicePrompting_Click );
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size( 209, 6 );
			// 
			// MENU_Options_EditKeyboard
			// 
			this.MENU_Options_EditKeyboard.Name = "MENU_Options_EditKeyboard";
			this.MENU_Options_EditKeyboard.Size = new System.Drawing.Size( 212, 22 );
			this.MENU_Options_EditKeyboard.Tag = "Edit Keyboard Shortcuts";
			this.MENU_Options_EditKeyboard.Text = "Edit Keyboard Shortcuts ...";
			this.MENU_Options_EditKeyboard.Click += new System.EventHandler( this.MENU_Options_EditKeyboard_Click );
			// 
			// MENU_Options_EditMasterTokenList
			// 
			this.MENU_Options_EditMasterTokenList.Name = "MENU_Options_EditMasterTokenList";
			this.MENU_Options_EditMasterTokenList.Size = new System.Drawing.Size( 212, 22 );
			this.MENU_Options_EditMasterTokenList.Tag = "Edit Master Token List";
			this.MENU_Options_EditMasterTokenList.Text = "Edit Master Token List ...";
			this.MENU_Options_EditMasterTokenList.Click += new System.EventHandler( this.MENU_Options_EditMasterTokenList_Click );
			// 
			// MENU_Help
			// 
			this.MENU_Help.AccessibleDescription = "Help Menu";
			this.MENU_Help.DropDownItems.AddRange( new System.Windows.Forms.ToolStripItem[] {
            this.MENU_Help_Index,
            this.MENU_Help_Shortcuts,
            this.MENU_Help_About} );
			this.MENU_Help.Name = "MENU_Help";
			this.MENU_Help.Size = new System.Drawing.Size( 44, 20 );
			this.MENU_Help.Tag = "Help Menu";
			this.MENU_Help.Text = "Help";
			this.MENU_Help.DropDownOpening += new System.EventHandler( this.MENU_OnMenuBar_DropDownOpening );
			this.MENU_Help.DropDownClosed += new System.EventHandler( this.MENU_OnMenuBar_DropDownClosed );
			// 
			// MENU_Help_Index
			// 
			this.MENU_Help_Index.Image = ( ( System.Drawing.Image )( resources.GetObject( "MENU_Help_Index.Image" ) ) );
			this.MENU_Help_Index.Name = "MENU_Help_Index";
			this.MENU_Help_Index.Size = new System.Drawing.Size( 156, 22 );
			this.MENU_Help_Index.Tag = "Browse Help Index";
			this.MENU_Help_Index.Text = "Index ...";
			this.MENU_Help_Index.Click += new System.EventHandler( this.MENU_Help_Index_Click );
			// 
			// MENU_Help_Shortcuts
			// 
			this.MENU_Help_Shortcuts.Enabled = false;
			this.MENU_Help_Shortcuts.Name = "MENU_Help_Shortcuts";
			this.MENU_Help_Shortcuts.Size = new System.Drawing.Size( 156, 22 );
			this.MENU_Help_Shortcuts.Tag = "Show Shortcuts";
			this.MENU_Help_Shortcuts.Text = "Show Shortcuts";
			this.MENU_Help_Shortcuts.Visible = false;
			// 
			// MENU_Help_About
			// 
			this.MENU_Help_About.Name = "MENU_Help_About";
			this.MENU_Help_About.Size = new System.Drawing.Size( 156, 22 );
			this.MENU_Help_About.Tag = "About WAVES Toolbar";
			this.MENU_Help_About.Text = "About ...";
			this.MENU_Help_About.Click += new System.EventHandler( this.MENU_Help_About_Click );
			// 
			// BUTTON_Close
			// 
			this.BUTTON_Close.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.BUTTON_Close.Font = new System.Drawing.Font( "Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ) );
			this.BUTTON_Close.Location = new System.Drawing.Point( 498, 43 );
			this.BUTTON_Close.Name = "BUTTON_Close";
			this.BUTTON_Close.Size = new System.Drawing.Size( 75, 30 );
			this.BUTTON_Close.TabIndex = 100;
			this.BUTTON_Close.TabStop = false;
			this.BUTTON_Close.Text = "CLOSE";
			this.BUTTON_Close.UseVisualStyleBackColor = true;
			this.BUTTON_Close.Visible = false;
			this.BUTTON_Close.Click += new System.EventHandler( this.BUTTON_Close_Click );
			// 
			// DIALOG_Load
			// 
			this.DIALOG_Load.FileName = "openFileDialog1";
			// 
			// BUTTON_MultiFunction
			// 
			this.BUTTON_MultiFunction.Enabled = false;
			this.BUTTON_MultiFunction.Location = new System.Drawing.Point( 279, 320 );
			this.BUTTON_MultiFunction.Name = "BUTTON_MultiFunction";
			this.BUTTON_MultiFunction.Size = new System.Drawing.Size( 61, 160 );
			this.BUTTON_MultiFunction.TabIndex = 30;
			this.BUTTON_MultiFunction.Text = "Copy to/from Clipboard";
			this.BUTTON_MultiFunction.UseVisualStyleBackColor = true;
			this.BUTTON_MultiFunction.Click += new System.EventHandler( this.BUTTON_MultiFunction_Click );
			this.BUTTON_MultiFunction.Enter += new System.EventHandler( this.BUTTON_MultiFunction_Enter );
			this.BUTTON_MultiFunction.MouseEnter += new System.EventHandler( this.BUTTON_MultiFunction_Enter );
			// 
			// listBox1
			// 
			this.listBox1.BackColor = System.Drawing.Color.FromArgb( ( ( int )( ( ( byte )( 192 ) ) ) ), ( ( int )( ( ( byte )( 255 ) ) ) ), ( ( int )( ( ( byte )( 192 ) ) ) ) );
			this.listBox1.Enabled = false;
			this.listBox1.Font = new System.Drawing.Font( "Lucida Sans Unicode", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ) );
			this.listBox1.FormattingEnabled = true;
			this.listBox1.IntegralHeight = false;
			this.listBox1.ItemHeight = 16;
			this.listBox1.Location = new System.Drawing.Point( -103, 468 );
			this.listBox1.Name = "listBox1";
			this.listBox1.Size = new System.Drawing.Size( 108, 97 );
			this.listBox1.TabIndex = 301;
			this.listBox1.TabStop = false;
			this.listBox1.Visible = false;
			// 
			// TOOLTIP_Display
			// 
			this.TOOLTIP_Display.AutoPopDelay = 5000;
			this.TOOLTIP_Display.InitialDelay = 500;
			this.TOOLTIP_Display.IsBalloon = true;
			this.TOOLTIP_Display.ReshowDelay = 250;
			// 
			// listBox2
			// 
			this.listBox2.FormattingEnabled = true;
			this.listBox2.Location = new System.Drawing.Point( 384, 270 );
			this.listBox2.Name = "listBox2";
			this.listBox2.Size = new System.Drawing.Size( 120, 199 );
			this.listBox2.TabIndex = 302;
			this.listBox2.Visible = false;
			// 
			// BROWSER_MathJaxRenderer
			// 
			this.BROWSER_MathJaxRenderer.AllowWebBrowserDrop = false;
			this.BROWSER_MathJaxRenderer.Location = new System.Drawing.Point( 12, 496 );
			this.BROWSER_MathJaxRenderer.MinimumSize = new System.Drawing.Size( 20, 20 );
			this.BROWSER_MathJaxRenderer.Name = "BROWSER_MathJaxRenderer";
			this.BROWSER_MathJaxRenderer.Size = new System.Drawing.Size( 380, 80 );
			this.BROWSER_MathJaxRenderer.TabIndex = 303;
			this.BROWSER_MathJaxRenderer.TabStop = false;
			this.BROWSER_MathJaxRenderer.Url = new System.Uri( "", System.UriKind.Relative );
			this.BROWSER_MathJaxRenderer.Visible = false;
			this.BROWSER_MathJaxRenderer.WebBrowserShortcutsEnabled = false;
			// 
			// Token_Toolbar
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.CancelButton = this.BUTTON_Close;
			this.ClientSize = new System.Drawing.Size( 390, 568 );
			this.Controls.Add( this.BROWSER_MathJaxRenderer );
			this.Controls.Add( this.listBox2 );
			this.Controls.Add( this.listBox1 );
			this.Controls.Add( this.BUTTON_MultiFunction );
			this.Controls.Add( this.LABEL_Warning );
			this.Controls.Add( this.LISTBOX_ShowExcerpts );
			this.Controls.Add( this.LABEL_Wrapper );
			this.Controls.Add( this.BUTTON_Close );
			this.Controls.Add( this.BUTTON_ShowLevel3 );
			this.Controls.Add( this.BUTTON_ShowLevel2 );
			this.Controls.Add( this.TEXTBOX_MathExpression );
			this.Controls.Add( this.MATHML_Display );
			this.Controls.Add( this.MENU_bar );
			this.DoubleBuffered = true;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
			this.KeyPreview = true;
			this.MainMenuStrip = this.MENU_bar;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "Token_Toolbar";
			this.Text = "WAVES Token Toolbar";
			this.TopMost = true;
			this.Load += new System.EventHandler( this.Token_Toolbar_Load );
			this.KeyPress += new System.Windows.Forms.KeyPressEventHandler( this.Token_Toolbar_KeyPress );
			this.KeyUp += new System.Windows.Forms.KeyEventHandler( this.Token_Toolbar_KeyUp );
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler( this.Token_Toolbar_FormClosing );
			this.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler( this.Token_Toolbar_PreviewKeyDown );
			this.KeyDown += new System.Windows.Forms.KeyEventHandler( this.Token_Toolbar_KeyDown );
			this.MENU_bar.ResumeLayout( false );
			this.MENU_bar.PerformLayout();
			this.ResumeLayout( false );
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BUTTON_ShowLevel2;
        private System.Windows.Forms.Button BUTTON_ShowLevel3;
        public MathML.Rendering.MathMLControl MATHML_Display;
        public System.Windows.Forms.TextBox TEXTBOX_MathExpression;
        private System.Windows.Forms.Label LABEL_Warning;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.Button BUTTON_Close;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        public System.Windows.Forms.Label LABEL_Wrapper;
        public System.Windows.Forms.ListBox LISTBOX_ShowExcerpts;
        private System.Windows.Forms.SaveFileDialog DIALOG_Save;
        private System.Windows.Forms.OpenFileDialog DIALOG_Load;
        public System.Windows.Forms.ToolStripMenuItem MENU_Formula;
        public System.Windows.Forms.ToolStripMenuItem MENU_Formula_New;
        public System.Windows.Forms.ToolStripMenuItem MENU_Formula_Read;
        public System.Windows.Forms.ToolStripMenuItem MENU_Formula_CopyClipboard;
        public System.Windows.Forms.ToolStripMenuItem MENU_Formula_ExportMathML;
        public System.Windows.Forms.ToolStripMenuItem MENU_Formula_ExportGraphic;
        public System.Windows.Forms.ToolStripMenuItem MENU_Options;
        public System.Windows.Forms.ToolStripMenuItem MENU_Options_HighContrast;
        public System.Windows.Forms.ToolStripMenuItem MENU_Options_VoicePrompting;
        public System.Windows.Forms.ToolStripMenuItem MENU_Help;
        public System.Windows.Forms.ToolStripMenuItem MENU_Options_EditKeyboard;
        public System.Windows.Forms.ToolStripMenuItem MENU_Options_EditMasterTokenList;
        public System.Windows.Forms.ToolStripMenuItem MENU_Help_Index;
        public System.Windows.Forms.ToolStripMenuItem MENU_Help_Shortcuts;
        public System.Windows.Forms.ToolStripMenuItem MENU_Help_About;
        public System.Windows.Forms.ToolStripMenuItem MENU_View;
        public System.Windows.Forms.ToolStripMenuItem MENU_Formula_ImportMathML;
        public System.Windows.Forms.ToolStripMenuItem MENU_View_HideMenu;
        public System.Windows.Forms.ToolStripMenuItem MENU_View_Level2;
        public System.Windows.Forms.ToolStripMenuItem MENU_View_Repository;
        public System.Windows.Forms.ToolStripMenuItem MENU_Formula_ClearExcerpt;
        public System.Windows.Forms.ToolStripMenuItem MENU_Formula_Close;
        public System.Windows.Forms.ToolStripMenuItem MENU_View_Autotune;
        public System.Windows.Forms.ToolStripMenuItem MENU_Options_EnglishExcerpts;
        public System.Windows.Forms.MenuStrip MENU_bar;
        public System.Windows.Forms.ToolStripMenuItem MENU_View_AlwaysOnTop;
        public System.Windows.Forms.ToolStripMenuItem MENU_Options_ReadAsPower;
		private System.Windows.Forms.Button BUTTON_MultiFunction;
		public System.Windows.Forms.ListBox listBox1;
		private System.Windows.Forms.ToolTip TOOLTIP_Display;
		private System.Windows.Forms.ListBox listBox2;
		public System.Windows.Forms.ToolStripMenuItem MENU_Formula_Undo;
		private System.Windows.Forms.WebBrowser BROWSER_MathJaxRenderer;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
		public System.Windows.Forms.ToolStripMenuItem MENU_View_MathJax;

    }
}


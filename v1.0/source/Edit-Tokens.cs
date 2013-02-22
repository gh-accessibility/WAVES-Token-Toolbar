using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Prototype_Token_Interface
{
    /// <summary>
    /// A full representation of one instance of the Master Token Editor.
    /// </summary>
    public partial class Edit_Tokens : Form
    {
        /// <summary>
        /// Tracks whether any changes have been made to the master token list or any English equivalents
        /// </summary>
        private bool anyChangesMaster = false; 

        /// <summary>
        /// Tracks whether any changes have been made to the elevated token list
        /// </summary>
        private bool anyChangesElev = false;

		/// <summary>
		/// An instance of the Toolbar that opened this window, to expose its information to the token editor
		/// </summary>
		private Token_Toolbar WavesToolbar;


		/// <summary>
		/// Close window and dismiss changes.
		/// </summary>
		private bool cancelPressed = false;

		/// <summary>
		/// Close window, allowing changes.
		/// </summary>
		private bool confirmPressed = false;


// *******************************************************************************************************************
//   The following functions are instrumental for editor initialization.
// *******************************************************************************************************************

        public Edit_Tokens()
        {
            InitializeComponent();
        }

        private void Edit_Tokens_Load( object sender, EventArgs e )
        {
			WavesToolbar = ( Token_Toolbar )this.Owner;

            InitElementGrid();            
            InitElemEnglishApprox();
            InitMOgrid();
            InitMIgrid();

            InitElementList();
            InitLevel1List();
            InitLevel2List();

            // Set the form's color scheme appropriately
            SetFormColors();

            // Set whether this window will be on top of all other windows
            this.TopMost = WavesToolbar.MENU_View_AlwaysOnTop.Checked;
        } // Form's Load event... initialize everything


        /// <summary>
        /// Initialize the Element grid.  The only thing that will be editable in this grid is the speech, since 
        ///   a user could bork the editor if they issued wrong data here.  It requires knowledge of MathML to edit.
        /// </summary>
        private void InitElementGrid()
        {
            DATAGRID_Elements.DataSource = Token_Toolbar.MasterToken._mathMLelement;

            DATAGRID_Elements.Columns[ 0 ].HeaderText = "Element";
			DATAGRID_Elements.Columns[ 1 ].HeaderText = "Speech";
			DATAGRID_Elements.AutoResizeColumns();
            //DATAGRID_Elements.Columns[ 2 ].Width = 40;
            DATAGRID_Elements.Columns[ 0 ].ReadOnly = true;
            DATAGRID_Elements.Columns[ 2 ].Visible = false;
			DATAGRID_Elements.Width = SetWidth( DATAGRID_Elements );
			DATAGRID_Elements.Height = SetHeight( DATAGRID_Elements );

            DATAGRID_Elements.Select();
        } // Initialize the Element datagridview


        /// <summary>
        /// Initialize the MathML English Approximation grid.  
        /// </summary>
        private void InitElemEnglishApprox()
        {
            // This was not as easy as it I thought it would be because this is stored in a two dimensional array and not a simple class.
            DATAGRID_ElemEnglish.ColumnCount = 4;

			// Set a style for showing cells which are "out of bounds"
			DataGridViewCellStyle dgvcs_invalid = new DataGridViewCellStyle( DATAGRID_ElemEnglish.DefaultCellStyle );
			dgvcs_invalid.BackColor = Color.Silver;
			dgvcs_invalid.ForeColor = Color.Silver;
			dgvcs_invalid.SelectionBackColor = Color.Silver;

            for ( int i = 0; i < Token_Toolbar.MasterToken._elemCount; i++ ) {
                var row = new DataGridViewRow();

                for ( int col = 0; col < 4; col++ ) {
                    var cell = new DataGridViewTextBoxCell();
                    cell.Value = Token_Toolbar.MasterToken._elemEnglish[ i ][ col ];
                    row.Cells.Add( cell );

					if( ( col > 0 ) & ( Token_Toolbar.MasterToken._mathMLelement[ i ]._args < col ) ) {
						cell.ReadOnly = true;
						cell.Style = new DataGridViewCellStyle( dgvcs_invalid );
					}
					else {
						cell.ReadOnly = false;
						cell.MaxInputLength = 15;
					}
                }

                DATAGRID_ElemEnglish.Rows.Add( row );
            }

            DATAGRID_ElemEnglish.Columns[ 0 ].HeaderText = "Base";
            DATAGRID_ElemEnglish.Columns[ 1 ].HeaderText = "Argument #1";
            DATAGRID_ElemEnglish.Columns[ 2 ].HeaderText = "Argument #2";
            DATAGRID_ElemEnglish.Columns[ 3 ].HeaderText = "Argument #3";

            for ( int col = 0; col < 4; col++ ) 
                DATAGRID_ElemEnglish.Columns[ col ].SortMode = DataGridViewColumnSortMode.NotSortable;

            DATAGRID_ElemEnglish.Columns[ 0 ].Width = 100;
            DATAGRID_ElemEnglish.Columns[ 1 ].Width = 100;
            DATAGRID_ElemEnglish.Columns[ 2 ].Width = 100;
            DATAGRID_ElemEnglish.Columns[ 3 ].Width = 100;
            DATAGRID_ElemEnglish.Width = SetWidth( DATAGRID_ElemEnglish );
            DATAGRID_ElemEnglish.Height = SetHeight( DATAGRID_ElemEnglish );

			DATAGRID_ElemEnglish.Location = new Point( DATAGRID_Elements.Location.X + DATAGRID_Elements.Width, DATAGRID_Elements.Location.Y );
			
			// Set this grid's label prompt to be placed direcly over this grid
			label2.Location = new Point( DATAGRID_ElemEnglish.Location.X - 4, label2.Location.Y );


			GROUPBOX_Elements.Width = DATAGRID_Elements.Width + DATAGRID_ElemEnglish.Width + 20;

			// Re-select the first element grid
			DATAGRID_Elements.Rows[ 0 ].Cells[ 0 ].Selected = true;

        } // Initialize the Element's English Approximations datagridview


		/// <summary>
		/// Initialize the MO token grid. 
		/// </summary>
		private void InitMOgrid()
        {
            DATAGRID_MO.DataSource = Token_Toolbar.MasterToken._mo;
            DATAGRID_MO.Columns[ 0 ].HeaderText = "MO";
			DATAGRID_MO.Columns[ 1 ].HeaderText = "Speech";
			DATAGRID_MO.AutoResizeColumns();
            DATAGRID_MO.Width = 225;
            DATAGRID_MO.Columns[ 0 ].Width = 50;
            DATAGRID_MO.Columns[ 1 ].Width = 155;
            DATAGRID_MO.Columns[ 2 ].Visible = false;

			for (int i = 0; i < DATAGRID_MO.Rows.Count; i++)
				DATAGRID_MO.Rows[ i ].Cells[ 1 ].Style.Font = new Font( "Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ) );

            DATAGRID_MO.Select();
        } // Initialize the MO datagridview


        /// <summary>
        /// Initialize the MI token grid.
        /// </summary>
        private void InitMIgrid()
        {
			DATAGRID_MI.DataSource = Token_Toolbar.MasterToken._mi;
            DATAGRID_MI.Columns[ 0 ].HeaderText = "MI";
			DATAGRID_MI.Columns[ 1 ].HeaderText = "Speech";
            DATAGRID_MI.AutoResizeColumns();
            DATAGRID_MI.Width = 225;
            DATAGRID_MI.Columns[ 0 ].Width = 50;
            DATAGRID_MI.Columns[ 1 ].Width = 155;
            DATAGRID_MI.Columns[ 2 ].Visible = false;

			for( int i = 0; i < DATAGRID_MI.Rows.Count; i++ )
				DATAGRID_MI.Rows[ i ].Cells[ 1 ].Style.Font = new Font( "Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ) );

        } // Initialize the MI datagridview


        /// <summary>
        /// Initialize the ListBox containing the Important MathML elements. 
        /// </summary>
        private void InitElementList()
        {
            LISTBOX_Element.Items.Clear();

			for( int i = 0; i < WavesToolbar.NumElements; i++ )
				LISTBOX_Element.Items.Add( WavesToolbar.ImpElement[ i ] );

            LISTBOX_Element.SelectedIndex = -1;
            BUTTON_ImpElem_Add.Enabled = ( LISTBOX_Element.Items.Count < 5 );
        }


        /// <summary>
        /// Initialize the ListBox containing the Level 1 tokens.
        /// </summary>
        private void InitLevel1List()
        {
            LISTBOX_Level1.Items.Clear();

            for ( int i = 0; i < WavesToolbar.NumLevel1; i++ )
				LISTBOX_Level1.Items.Add( WavesToolbar.ImpLevel1[ i ] );

            LISTBOX_Level1.SelectedIndex = -1;
            BUTTON_Level1_Add.Enabled = ( LISTBOX_Level1.Items.Count < 5 );

        }


        /// <summary>
        /// Initialize the ListBox containing the Level 2 tokens.
        /// </summary>
        private void InitLevel2List()
        {
            LISTBOX_Level2.Items.Clear();

			for( int i = 0; i < WavesToolbar.NumLevel2; i++ )
				LISTBOX_Level2.Items.Add( WavesToolbar.ImpLevel2[ i ] );

            LISTBOX_Level2.SelectedIndex = -1;
            BUTTON_Level2_Add.Enabled = ( LISTBOX_Level2.Items.Count < 10 );

        }


        /// <summary>
        /// This function will set the colors of controls on this form based off of whether the user wants the high contrast color option, or not.
        /// </summary>
        private void SetFormColors()
        {
            if ( WavesToolbar.MENU_Options_HighContrast.Checked ) {
                // Apply High Contrast color scheme
                this.BackColor = Color.DarkBlue;
                this.ForeColor = Color.DodgerBlue;

                DATAGRID_Elements.BackgroundColor = Color.Azure;
                DATAGRID_Elements.DefaultCellStyle.BackColor = Color.Azure;
                DATAGRID_Elements.EnableHeadersVisualStyles = false;
                DATAGRID_Elements.ForeColor = Color.Black;

                DATAGRID_ElemEnglish.BackgroundColor = Color.Azure;
                DATAGRID_ElemEnglish.DefaultCellStyle.BackColor = Color.Azure;
                DATAGRID_ElemEnglish.EnableHeadersVisualStyles = false;
                DATAGRID_ElemEnglish.ForeColor = Color.Black;

                DATAGRID_MO.BackgroundColor = Color.Azure;
                DATAGRID_MO.DefaultCellStyle.BackColor = Color.Azure;
                DATAGRID_MO.EnableHeadersVisualStyles = false;
                DATAGRID_MO.ForeColor = Color.Black;

                DATAGRID_MI.BackgroundColor = Color.Azure;
                DATAGRID_MI.DefaultCellStyle.BackColor = Color.Azure;
                DATAGRID_MI.EnableHeadersVisualStyles = false;
                DATAGRID_MI.ForeColor = Color.Black;

                LISTBOX_Element.BackColor = Color.Azure;
                LISTBOX_Element.ForeColor = Color.Black;
                LISTBOX_Level1.BackColor = Color.Azure;
                LISTBOX_Level1.ForeColor = Color.Black;
                LISTBOX_Level2.BackColor = Color.Azure;
                LISTBOX_Level2.ForeColor = Color.Black;
                LABEL_ElemSelect.BackColor = Color.Azure;
                LABEL_ElemSelect.ForeColor = Color.Black;
                LABEL_TokenSelect.BackColor = Color.Azure;
                LABEL_TokenSelect.ForeColor = Color.Black;

                //BUTTON_AddMI.BackColor = Color.Azure;
                BUTTON_AddMI.ForeColor = Color.Black;
                //BUTTON_AddMO.BackColor = Color.Azure;
                BUTTON_AddMO.ForeColor = Color.Black;
                //BUTTON_RemoveMI.BackColor = Color.Azure;
                BUTTON_RemoveMI.ForeColor = Color.Black;
                //BUTTON_RemoveMO.BackColor = Color.Azure;
                BUTTON_RemoveMO.ForeColor = Color.Black;
                BUTTON_ImpElem_Add.ForeColor = Color.Black;
                BUTTON_Level1_Add.ForeColor = Color.Black;
                BUTTON_Level2_Add.ForeColor = Color.Black;
                BUTTON_ImpElem_Remove.ForeColor = Color.Black;
                BUTTON_Level1_Remove.ForeColor = Color.Black;
                BUTTON_Level2_Remove.ForeColor = Color.Black;
                BUTTON_ImpElem_Up.ForeColor = Color.Black;
                BUTTON_Level1_Up.ForeColor = Color.Black;
                BUTTON_Level2_Up.ForeColor = Color.Black;
                BUTTON_ImpElem_Down.ForeColor = Color.Black;
                BUTTON_Level1_Down.ForeColor = Color.Black;
                BUTTON_Level2_Down.ForeColor = Color.Black;


                //for (int i = 0; i < this.Controls.Count; i++)
                //{
                //    this.Controls[i].BackColor = Color.Azure;
                //    this.Controls[i].ForeColor = Color.Black;
                //}
            }
            else {
                // Apply normal color scheme

                this.BackColor = SystemColors.Control;
                this.ForeColor = SystemColors.ControlText;

                DATAGRID_Elements.BackgroundColor = SystemColors.ControlLightLight;
                DATAGRID_Elements.DefaultCellStyle.BackColor = SystemColors.ControlLightLight;
                DATAGRID_Elements.EnableHeadersVisualStyles = true;
                DATAGRID_Elements.ForeColor = SystemColors.ControlText;

                DATAGRID_ElemEnglish.BackgroundColor = SystemColors.ControlLightLight;
                DATAGRID_ElemEnglish.DefaultCellStyle.BackColor = SystemColors.ControlLightLight;
                DATAGRID_ElemEnglish.EnableHeadersVisualStyles = true;
                DATAGRID_ElemEnglish.ForeColor = SystemColors.ControlText;

                DATAGRID_MO.BackgroundColor = SystemColors.ControlLightLight;
                DATAGRID_MO.DefaultCellStyle.BackColor = SystemColors.ControlLightLight;
                DATAGRID_MO.EnableHeadersVisualStyles = true;
                DATAGRID_MO.ForeColor = SystemColors.ControlText;

                DATAGRID_MI.BackgroundColor = SystemColors.ControlLightLight;
                DATAGRID_MI.DefaultCellStyle.BackColor = SystemColors.ControlLightLight;
                DATAGRID_MI.EnableHeadersVisualStyles = true;
                DATAGRID_MI.ForeColor = SystemColors.ControlText;

                LISTBOX_Element.BackColor = SystemColors.ControlLightLight;
                LISTBOX_Element.ForeColor = SystemColors.ControlText;
                LISTBOX_Level1.BackColor = SystemColors.ControlLightLight;
                LISTBOX_Level1.ForeColor = SystemColors.ControlText;
                LISTBOX_Level2.BackColor = SystemColors.ControlLightLight;
                LISTBOX_Level2.ForeColor = SystemColors.ControlText;
                LABEL_ElemSelect.BackColor = SystemColors.ControlLightLight;
                LABEL_ElemSelect.ForeColor = SystemColors.ControlText;
                LABEL_TokenSelect.BackColor = SystemColors.ControlLightLight;
                LABEL_TokenSelect.ForeColor = SystemColors.ControlText;

                //BUTTON_AddMI.BackColor = SystemColors.ControlLightLight;
                BUTTON_AddMI.ForeColor = SystemColors.ControlText;
                //BUTTON_AddMO.BackColor = SystemColors.ControlLightLight;
                BUTTON_AddMO.ForeColor = SystemColors.ControlText;
                //BUTTON_RemoveMI.BackColor = SystemColors.ControlLightLight;
                BUTTON_RemoveMI.ForeColor = SystemColors.ControlText;
                //BUTTON_RemoveMO.BackColor = SystemColors.ControlLightLight;
                BUTTON_RemoveMO.ForeColor = SystemColors.ControlText;
                BUTTON_ImpElem_Add.ForeColor = SystemColors.ControlText;
                BUTTON_Level1_Add.ForeColor = SystemColors.ControlText;
                BUTTON_Level2_Add.ForeColor = SystemColors.ControlText;
                BUTTON_ImpElem_Remove.ForeColor = SystemColors.ControlText;
                BUTTON_Level1_Remove.ForeColor = SystemColors.ControlText;
                BUTTON_Level2_Remove.ForeColor = SystemColors.ControlText;
                BUTTON_ImpElem_Up.ForeColor = SystemColors.ControlText;
                BUTTON_Level1_Up.ForeColor = SystemColors.ControlText;
                BUTTON_Level2_Up.ForeColor = SystemColors.ControlText;
                BUTTON_ImpElem_Down.ForeColor = SystemColors.ControlText;
                BUTTON_Level1_Down.ForeColor = SystemColors.ControlText;
                BUTTON_Level2_Down.ForeColor = SystemColors.ControlText;

                //for (int i = 0; i < this.Controls.Count; i++)
                //{
                //    this.Controls[i].BackColor = SystemColors.Control;
                //    this.Controls[i].ForeColor = SystemColors.ControlText;
                //}
            }

        } // Set Form Colors (Apply color scheme)


        /// <summary>
        /// Determines the width of a grid control based off of the width of all its columns.
        /// </summary>
        /// <param name="dgv">The DataGridView list whose width to compute</param>
        /// <returns>The resultant width in pixels</returns>
        private int SetWidth( DataGridView dgv )
        {
            int newWidth = 0;

            for ( int i = 0; i < dgv.Columns.Count; i++ ) {
                if ( dgv.Columns[ i ].Visible == true )     newWidth += dgv.Columns[ i ].Width + 1;
            }
			newWidth++;

            return newWidth;
        }


        /// <summary>
        /// Determines the height of a grid control based off of the height of all its rows
        /// </summary>
        /// <param name="dgv">The DataGridView list whose height to compute</param>
        /// <returns>The resultant height in pixels</returns>
        private int SetHeight( DataGridView dgv )
        {
            int newHeight = dgv.ColumnHeadersHeight;

            for ( int i = 0; i < dgv.Rows.Count; i++ ) 
                newHeight += dgv.Rows[ i ].Height;

			newHeight += 2;

            return newHeight;
        }


// *******************************************************************************************************************
//   The following functions are instrumental for saving any edits made, and re-initializing the interface.
//     This is done when the user closes this window.
// *******************************************************************************************************************

        private void Edit_Tokens_FormClosing( object sender, FormClosingEventArgs e )
        {
            DialogResult answer = DialogResult.No;
			if( confirmPressed )
				answer = DialogResult.Yes;

			if( anyChangesElev )
            {
                // Ask for user to confirm changes if any were made to the elevated tokens

				if ( ( !cancelPressed ) & ( !confirmPressed ) )
					answer = MessageBox.Show( "You've made edits to the elevated tokens.\nWill you save your changes?", "WAVES-Token-Toolbar", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question );

                if ( answer == DialogResult.Cancel ) {
                    // If the user pressed "Cancel", then the form will not close, and nothing will be changed.
                    e.Cancel = true;
                } // User answered "Cancel"
                else {
                    if ( answer == DialogResult.Yes ) {
                        // Save the Elevated Tokens xml
                        SaveElevatedTokensToXml();

                        anyChangesElev = false;
                    }
                }
            }

            if ( ( anyChangesMaster ) && ( !e.Cancel ) ) {
                // Ask for user to confirm changes if any were made to the master token list

				if( ( !cancelPressed ) & ( !confirmPressed ) )
					answer = MessageBox.Show( "You've made edits to the master token list.\nWill you save your changes?", "WAVES-Token-Toolbar", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question );

                if ( answer == DialogResult.Cancel ) {
                    // If the user pressed "Cancel", then the form will not close, and nothing will be changed.
                    e.Cancel = true;
                } // User answered "Cancel"
                else {
                    if ( answer == DialogResult.Yes ) {
                        // Save the Master Tokens xml
                        SaveMasterTokenListToXml();

                        anyChangesMaster = false;
                    }
                }
            }


            // Re-initialize the window, but this should only be done if it is still open
            //   This window could be closed by closing the Token Toolbar window!
			if( WavesToolbar != null ) {
				WavesToolbar.Enabled = true;
				WavesToolbar.ReinitializeControls();
            }
         
        }


		private void BUTTON_OK_Click( object sender, EventArgs e )
		{
			confirmPressed = true;
			Close();
		}

		private void BUTTON_Cancel_Click( object sender, EventArgs e )
		{
			cancelPressed = true;
			Close();
		}



        /// <summary>
        /// This function will take care of saving changes made to the master token list (master-token-list.xml), which is loaded 
        ///   upon application startup, and will be re-loaded when the interface is reinitialized after the Token Editor closes.
        /// </summary>
        private void SaveMasterTokenListToXml()
        {
            // It is important to note that if you are in a build environment, it does not save it to the /config
            //   directory, and so you will need to save the version of "elevated-tokens.xml" over the version in /config
            //   if you want these changes to persist between builds.
			string thisDesc;

            // Preamble
            string toSave = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>\n";

            // Root node
			toSave += "<master-tokens>\n";

            // MO tokens
            toSave += "  <mo>\n";
            for ( int i = 0; i < Token_Toolbar.MasterToken._moCount; i++ ) {
                toSave += "    <token>";
				  thisDesc = WavesToolbar.UnicodeLookup.GetUnicodeDescription( Token_Toolbar.MasterToken._mo[ i ]._symbol[ 0 ] );
				  toSave += AddSymbol( Token_Toolbar.MasterToken._mo[ i ]._symbol, thisDesc );
                  toSave += "<speech>" + Token_Toolbar.MasterToken._mo[ i ]._speech + "</speech>";
                  toSave += "<args>0</args>";
                toSave += "</token>\n";
            }
            toSave += "  </mo>\n";

            // MI Tokens
            toSave += "  <mi>\n";
            for ( int i = 0; i < Token_Toolbar.MasterToken._miCount; i++ ) {
                toSave += "    <token>";
				  thisDesc = WavesToolbar.UnicodeLookup.GetUnicodeDescription( Token_Toolbar.MasterToken._mi[ i ]._symbol[ 0 ] );
				  if ( Token_Toolbar.MasterToken._mi[ i ]._symbol.Length == 1 ) 
					  toSave += AddSymbol( Token_Toolbar.MasterToken._mi[ i ]._symbol, thisDesc );
				  else
					  toSave += AddSymbol( Token_Toolbar.MasterToken._mi[ i ]._symbol, Token_Toolbar.MasterToken._mi[ i ]._speech );
//                  toSave += "<symbol>" + ToXmlEntity( Token_Toolbar.MasterToken._mi[ i ]._symbol ) + "</symbol>";
                  toSave += "<speech>" + Token_Toolbar.MasterToken._mi[ i ]._speech + "</speech>";
                  toSave += "<args>0</args>";
                toSave += "</token>\n";
            }
            toSave += "  </mi>\n";

            // MathML Elements.  This follows a slightly different template than the tokens, since these
            //   have English Equivalents...
            toSave += "  <element>\n";
            for ( int i = 0; i < Token_Toolbar.MasterToken._elemCount; i++ ) {
                toSave += "    <token>\n";
				  toSave += "      <symbol>" + ToXmlEntity( Token_Toolbar.MasterToken._mathMLelement[ i ]._symbol ) + "</symbol>";				  
                  toSave += "<speech>" + Token_Toolbar.MasterToken._mathMLelement[ i ]._speech + "</speech>";
                  toSave += "<args>" + Token_Toolbar.MasterToken._mathMLelement[ i ]._args + "</args>\n";
                  toSave += "      <english>";
                    toSave += "<base>" + Token_Toolbar.MasterToken._elemEnglish[ i ][ 0 ] + "</base>";
                    for ( int j = 1; j < 4; j++ ) {
                        if ( Token_Toolbar.MasterToken._mathMLelement[ i ]._args >= j ) {
                            toSave += "<mrow-" + j.ToString() + ">";
                            toSave += Token_Toolbar.MasterToken._elemEnglish[ i ][ j ];
                            toSave += "</mrow-" + j.ToString() + ">";
                        }
                    }
                  toSave += "</english>\n";
                toSave += "    </token>\n";
            }
            toSave += "  </element>\n";

            // That's it.  Phew.
			toSave += "</master-tokens>\n";

            // Save the file as one long string
            System.IO.StreamWriter fileWriter = new System.IO.StreamWriter( Token_Toolbar.CurDir + "\\master-token-list.xml" );
            fileWriter.Write( toSave );
            fileWriter.Close();

        } // Save Master Token List changes to master-token-list.xml

		/// <summary>
		/// Returns a string representing a symbol that will be saved to the Master Token XML.  This may add a comment if necessary.
		/// </summary>
		/// <param name="sym">The symbol to save</param>
		/// <param name="sym">Description of the symbol</param>
		/// <returns></returns>
		private string AddSymbol( string sym, string des )
		{
			if( sym.Length == 1 ) {
				if( ( WavesToolbar.UnicodeLookup.GetUnicodeEntityDecimal( sym[ 0 ] ) >= 8289 ) &
					 ( WavesToolbar.UnicodeLookup.GetUnicodeEntityDecimal( sym[ 0 ] ) <= 8292 ) )
					sym = WavesToolbar.UnicodeLookup.GetUnicodeEntityHex( sym[ 0 ] );
			}

			string toSave = "<symbol>" + ToXmlEntity( sym );

			char c = sym[ 0 ];
			// If the symbol requires a description, use the description for the token (passed in as "des")
			if( ( sym.Length > 1 ) | ( c > 127 ) ) {
				toSave += "<!-- " + des + " -->";
			}
			toSave += "</symbol>";

			return toSave;
		}


        /// <summary>
        /// This function will take care of saving changes made to the elevated tokens (elevated-tokens.xml), which is loaded upon 
        ///   application startup, and will be re-loaded when the interface is reinitialized after the Token Editor closes.
        /// </summary>
        private void SaveElevatedTokensToXml()
        {
            // It is important to note that if you are in a build environment, it does not save it to the /config
            //   directory, and so you will need to save the version of "elevated-tokens.xml" over the version in /config
            //   if you want these changes to persist between builds.

            // Preamble
            string toSave = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>\n";
            
            // Root node
			toSave += "<elevated-tokens>\n";

            // All elevated MathML elements
            toSave += "  <elements>\n";
            for ( int i = 0; i < LISTBOX_Element.Items.Count; i++ )
                toSave += "    <symbol>" + ToXmlEntity( LISTBOX_Element.Items[ i ].ToString() ) + "</symbol>\n";
            toSave += "  </elements>\n";

            // Level 1 Tokens
            toSave += "  <level-1>\n";
            for (int i = 0; i < LISTBOX_Level1.Items.Count; i++)
                toSave += "    <symbol>" + ToXmlEntity( LISTBOX_Level1.Items[ i ].ToString() ) + "</symbol>\n";
            toSave += "  </level-1>\n";

            // Level 2 Tokens
            toSave += "  <level-2>\n";
            for (int i = 0; i < LISTBOX_Level2.Items.Count; i++)
                toSave += "    <symbol>" + ToXmlEntity( LISTBOX_Level2.Items[ i ].ToString() ) + "</symbol>\n";
            toSave += "  </level-2>\n";

            // That's it
			toSave += "</elevated-tokens>\n";

            // Save the file as one long string
            System.IO.StreamWriter fileWriter = new System.IO.StreamWriter( Token_Toolbar.CurDir + "\\elevated-tokens.xml" );
            fileWriter.Write( toSave );
            fileWriter.Close();

        } // Save Elevated Token changes to elevated-tokens.xml


        /// <summary>
        /// Converts a character to its equivalent XML entity, if necessary (for example, "&" = "&amp")
        /// </summary>
        /// <param name="sym">Character to (potentially) convert</param>
        /// <returns>An XML safe representation of the character</returns>
        private string ToXmlEntity( string sym )
        {
            switch ( sym ) {
                case "&":
                    return "&amp;";
                case ">":
                    return "&gt;";
                case "<":
                    return "&lt;";
                case "\'":
                    return "\'";
                case "\"":
                    return "\"";
                default:
                    return sym;
            }
        }


// *******************************************************************************************************************
//   The following functions are instrumental for editing DataGridView controls (Master Token List).
// *******************************************************************************************************************

        private void DATAGRID_ElemEnglish_CellEndEdit( object sender, DataGridViewCellEventArgs e )
        {
            // Since the Element English Approximation datagridview control is not classically bound to its data,
            //   when edits are made in the grid, the changes to the actual data have to be explicitly committed.
 
            DataGridView dgv = ( DataGridView )sender;

            int row = dgv.CurrentCellAddress.Y;
            int col = dgv.CurrentCellAddress.X;

            Token_Toolbar.MasterToken._elemEnglish[ row ][ col ] = ( string )dgv.CurrentCell.Value;

            anyChangesMaster = true;
        }

        private void DATAGRID_Master_CellEndEdit( object sender, DataGridViewCellEventArgs e )
        {
            anyChangesMaster = true;
        }


        private void BUTTON_AddMO_Click( object sender, EventArgs e )
        {
			// Must add one (empty) MO token to the Master List.  This requires the DataSource to be unbound.
			DATAGRID_MO.DataSource = null;
			Token_Toolbar.MasterToken._mo.Add( new Token() );
			Token_Toolbar.MasterToken._moCount++;

			// Reinitialize the MO grid, to reflect the changes made
			InitMOgrid();

			DATAGRID_MO.CurrentCell = DATAGRID_MO.Rows[ Token_Toolbar.MasterToken._mo.Count() - 1 ].Cells[ 0 ];
			anyChangesMaster = true;
		}

        private void BUTTON_RemoveMO_Click( object sender, EventArgs e )
        {
			// Must remove the currently selected MO token to the Master List.  This requires the DataSource to be unbound.
			int row = DATAGRID_MO.CurrentCellAddress.Y;
			DATAGRID_MO.DataSource = null;
			Token_Toolbar.MasterToken._mo.RemoveAt( row );
			Token_Toolbar.MasterToken._moCount--;

			// Reinitialize the MO grid, to reflect the changes made
			ReconnectMO( row );

			anyChangesMaster = true;
        }


		private void BUTTON_DemoteMO_Click( object sender, EventArgs e )
		{
			int pos = DATAGRID_MO.CurrentCellAddress.Y;
			int target = pos - 1;

			string tempSym = Token_Toolbar.MasterToken._mo[ pos ]._symbol;
			string tempSpeech = Token_Toolbar.MasterToken._mo[ pos ]._speech;

			Token_Toolbar.MasterToken._mo[ pos ]._symbol = Token_Toolbar.MasterToken._mo[ target ]._symbol;
			Token_Toolbar.MasterToken._mo[ pos ]._speech = Token_Toolbar.MasterToken._mo[ target ]._speech;

			Token_Toolbar.MasterToken._mo[ target ]._symbol = tempSym;
			Token_Toolbar.MasterToken._mo[ target ]._speech = tempSpeech;

			anyChangesMaster = true;
			ReconnectMO( target );

		}


		private void BUTTON_PromoteMO_Click( object sender, EventArgs e )
		{
			int pos = DATAGRID_MO.CurrentCellAddress.Y;
			int target = pos + 1;

			string tempSym = Token_Toolbar.MasterToken._mo[ pos ]._symbol;
			string tempSpeech = Token_Toolbar.MasterToken._mo[ pos ]._speech;

			Token_Toolbar.MasterToken._mo[ pos ]._symbol = Token_Toolbar.MasterToken._mo[ target ]._symbol;
			Token_Toolbar.MasterToken._mo[ pos ]._speech = Token_Toolbar.MasterToken._mo[ target ]._speech;

			Token_Toolbar.MasterToken._mo[ target ]._symbol = tempSym;
			Token_Toolbar.MasterToken._mo[ target ]._speech = tempSpeech;

			anyChangesMaster = true;
			ReconnectMO( target );

		}

	

        private void BUTTON_AddMI_Click( object sender, EventArgs e )
        {
			// Must add one (empty) MI token to the Master List.  This requires the DataSource to be unbound.
			DATAGRID_MI.DataSource = null;
			Token_Toolbar.MasterToken._mi.Add( new Token() );
			Token_Toolbar.MasterToken._miCount++;

			// Reinitialize the MI grid, to reflect the changes made
			InitMIgrid();

			DATAGRID_MI.CurrentCell = DATAGRID_MI.Rows[ Token_Toolbar.MasterToken._mi.Count() - 1 ].Cells[ 0 ];
			anyChangesMaster = true;
        }


        private void BUTTON_RemoveMI_Click( object sender, EventArgs e )
        {
			// Must remove the currently selected MI token to the Master List.  This requires the DataSource to be unbound.
			int row = DATAGRID_MI.CurrentCellAddress.Y;
			DATAGRID_MI.DataSource = null;
			Token_Toolbar.MasterToken._mi.RemoveAt( row );
			Token_Toolbar.MasterToken._miCount--;

			// Reinitialize the MI grid, to reflect the changes made
			ReconnectMI( row );

			anyChangesMaster = true;
		}


		private void BUTTON_DemoteMI_Click( object sender, EventArgs e )
		{
			int pos = DATAGRID_MI.CurrentCellAddress.Y;
			int target = pos - 1;

			string tempSym = Token_Toolbar.MasterToken._mi[ pos ]._symbol;
			string tempSpeech = Token_Toolbar.MasterToken._mi[ pos ]._speech;

			Token_Toolbar.MasterToken._mi[ pos ]._symbol = Token_Toolbar.MasterToken._mi[ target ]._symbol;
			Token_Toolbar.MasterToken._mi[ pos ]._speech = Token_Toolbar.MasterToken._mi[ target ]._speech;

			Token_Toolbar.MasterToken._mi[ target ]._symbol = tempSym;
			Token_Toolbar.MasterToken._mi[ target ]._speech = tempSpeech;

			anyChangesMaster = true;
			ReconnectMI( target );
		}


		private void BUTTON_PromoteMI_Click( object sender, EventArgs e )
		{
			int pos = DATAGRID_MI.CurrentCellAddress.Y;
			int target = pos + 1;

			string tempSym = Token_Toolbar.MasterToken._mi[ pos ]._symbol;
			string tempSpeech = Token_Toolbar.MasterToken._mi[ pos ]._speech;

			Token_Toolbar.MasterToken._mi[ pos ]._symbol = Token_Toolbar.MasterToken._mi[ target ]._symbol;
			Token_Toolbar.MasterToken._mi[ pos ]._speech = Token_Toolbar.MasterToken._mi[ target ]._speech;

			Token_Toolbar.MasterToken._mi[ target ]._symbol = tempSym;
			Token_Toolbar.MasterToken._mi[ target ]._speech = tempSpeech;

			anyChangesMaster = true;
			ReconnectMI( target );
			//SwapDataGridViewItem( DATAGRID_MI, 1 );
		}



        private void DATAGRID_MO_RowEnter( object sender, DataGridViewCellEventArgs e )
        {
            DataGridView dgv = ( DataGridView )sender;
            SetCurrentTokenText( dgv, LABEL_TokenSelect );

            int row = dgv.CurrentCellAddress.Y;
            BUTTON_RemoveMO.Enabled = ( row > -1 );
			BUTTON_PromoteMO.Enabled = ( ( row > -1 ) & ( row < ( dgv.Rows.Count - 1) ) );
			BUTTON_DemoteMO.Enabled = ( row > 0 );
		}

        private void DATAGRID_MI_RowEnter( object sender, DataGridViewCellEventArgs e )
        {
            DataGridView dgv = ( DataGridView )sender;
            SetCurrentTokenText( dgv, LABEL_TokenSelect );

            int row = dgv.CurrentCellAddress.Y;
            BUTTON_RemoveMI.Enabled = ( row > -1 );
			BUTTON_PromoteMI.Enabled = ( ( row > -1 ) & ( row < ( dgv.Rows.Count - 1 ) ) );
			BUTTON_DemoteMI.Enabled = ( row > 0 );
		}

        private void DATAGRID_Elements_RowEnter( object sender, DataGridViewCellEventArgs e )
        {
            SetCurrentTokenText( ( DataGridView )sender, LABEL_ElemSelect );
			DATAGRID_ElemEnglish.CurrentCell = null;
        }

		private void DATAGRID_ElemEnglish_CellEnter( object sender, DataGridViewCellEventArgs e )
		{
			SetCurrentTokenText( ( DataGridView )sender, LABEL_ElemSelect );
			DATAGRID_Elements.CurrentCell = null;
		}


		/// <summary>
		/// Swaps two Data Grid Items (either MO or MI) in a specified DataGridView. 
		/// </summary>
		/// <param name="dgv">DataGridView which contains the tokens to swap.</param>
		/// <param name="target">-1 if moving item up, 1 if moving item down</param>
		private void SwapDataGridViewItem( DataGridView dgv, int pos, int target )
		{
			var thisRow = dgv.Rows[ pos ];
			var targetRow = dgv.Rows[ target ];

			dgv.Rows.Remove( thisRow );
			dgv.Rows.Remove( targetRow );
			dgv.Rows.Insert( pos, targetRow );
			dgv.Rows.Insert( target, thisRow );

			anyChangesMaster = true;
		}


		/// <summary>
		/// Connects the MI DataGridView back to its source, and makes sure the proper buttons are enabled/disabled
		/// </summary>
		/// <param name="newRow">The row to select within the grid</param>
		private void ReconnectMI( int newRow )
		{
			// Reinitialize the MI grid, to reflect the changes made
			InitMIgrid();

			if( newRow < Token_Toolbar.MasterToken._miCount )
				DATAGRID_MI.CurrentCell = DATAGRID_MI.Rows[ newRow ].Cells[ 0 ];
			else {
				if( newRow > 0 )
					DATAGRID_MI.CurrentCell = DATAGRID_MI.Rows[ newRow - 1 ].Cells[ 0 ];
				else {
					BUTTON_RemoveMI.Enabled = false;
					BUTTON_PromoteMI.Enabled = false;
					BUTTON_DemoteMI.Enabled = false;
				}
			}
		}


		/// <summary>
		/// Connects the MO DataGridView back to its source, and makes sure the proper buttons are enabled/disabled
		/// </summary>
		/// <param name="newRow">The row to select within the grid</param>
		private void ReconnectMO( int newRow )
		{
			// Reinitialize the MI grid, to reflect the changes made
			InitMOgrid();

			if( newRow < Token_Toolbar.MasterToken._moCount )
				DATAGRID_MO.CurrentCell = DATAGRID_MO.Rows[ newRow ].Cells[ 0 ];
			else {
				if( newRow > 0 )
					DATAGRID_MO.CurrentCell = DATAGRID_MO.Rows[ newRow - 1 ].Cells[ 0 ];
				else {
					BUTTON_RemoveMO.Enabled = false;
					BUTTON_PromoteMO.Enabled = false;
					BUTTON_DemoteMO.Enabled = false;
				}
			}
		}
		
		
		/// <summary>
        /// Sets the text of the specified Label.  Used to determine which token/element to Add to Elevated Tokens listings.
        /// </summary>
        /// <param name="dgv">DataGridView list that contains the token or element to select</param>
        /// <param name="lab">The label to edit (determined by whether the selected item is a token or element)</param>
        private void SetCurrentTokenText( DataGridView dgv, Label lab )
        {
            int row = dgv.CurrentCellAddress.Y;

			// Make the Element Description grid behave like the Element grid here
			if( dgv == DATAGRID_ElemEnglish )
				dgv = DATAGRID_Elements;

            if ( row > -1 )
            {
                lab.Text = ( string )dgv.Rows[ row ].Cells[ 0 ].Value;
            }

        }


// *******************************************************************************************************************
//   The following functions are instrumental for editor Listboxes (mostly Elevated Tokens).
// *******************************************************************************************************************


        private void LISTBOX_Element_SelectedIndexChanged( object sender, EventArgs e )
        {
            ListBox lb = ( ListBox )sender;
            int ind = lb.SelectedIndex;

            //if (lb.Items.Count < 6)
                BUTTON_ImpElem_Add.Enabled = ( lb.Items.Count < 5 );

//            if (ind > -1)
//            {
                BUTTON_ImpElem_Remove.Enabled = ( ind > -1 );
                BUTTON_ImpElem_Up.Enabled = ( ind > 0 );
                BUTTON_ImpElem_Down.Enabled = ( ( ind > -1 ) && ( ind < ( lb.Items.Count - 1 ) ) );
//            }
//            else
//            {
//                BUTTON_ImpElem_Remove.Enabled = false;
//                BUTTON_ImpElem_Up.Enabled = false;
//                BUTTON_ImpElem_Down.Enabled = false;
//            }
        }

        private void LISTBOX_Level1_SelectedIndexChanged( object sender, EventArgs e )
        {
            ListBox lb = ( ListBox )sender;
            int ind = lb.SelectedIndex;

            BUTTON_Level1_Add.Enabled = ( lb.Items.Count < 5 );
            BUTTON_Level1_Remove.Enabled = ( ind > -1 );
            BUTTON_Level1_Up.Enabled = ( ind > 0 );
            BUTTON_Level1_Down.Enabled = ( ( ind > -1 ) && ( ind < ( lb.Items.Count - 1 ) ) );

        }

        private void LISTBOX_Level2_SelectedIndexChanged( object sender, EventArgs e )
        {
            ListBox lb = ( ListBox )sender;
            int ind = lb.SelectedIndex;

            BUTTON_Level2_Add.Enabled = ( lb.Items.Count < 10 );
            BUTTON_Level2_Remove.Enabled = ( ind > -1 );
            BUTTON_Level2_Up.Enabled = ( ind > 0 );
            BUTTON_Level2_Down.Enabled = ( ( ind > -1 ) && ( ind < ( lb.Items.Count - 1 ) ) );
        }



        private void BUTTON_ImpElem_Up_Click( object sender, EventArgs e )
        {
            SwapListItem( LISTBOX_Element, -1 );
        }

        private void BUTTON_ImpElem_Down_Click( object sender, EventArgs e )
        {
            SwapListItem( LISTBOX_Element, 1 );
        }

        private void BUTTON_Level1_Up_Click( object sender, EventArgs e )
        {
            SwapListItem( LISTBOX_Level1, -1 );
        }

        private void BUTTON_Level1_Down_Click( object sender, EventArgs e )
        {
            SwapListItem( LISTBOX_Level1, 1 );
        }

        private void BUTTON_Level2_Up_Click( object sender, EventArgs e )
        {
            SwapListItem( LISTBOX_Level2, -1 );
        }

        private void BUTTON_Level2_Down_Click( object sender, EventArgs e )
        {
            SwapListItem( LISTBOX_Level2, 1 );
        }



        private void BUTTON_ImpElem_Add_Click( object sender, EventArgs e )
        {
            AddListItem( LISTBOX_Element, LABEL_ElemSelect.Text );
        }

        private void BUTTON_ImpElem_Remove_Click( object sender, EventArgs e )
        {
            RemoveListItem( LISTBOX_Element );
        }

        private void BUTTON_Level1_Add_Click( object sender, EventArgs e )
        {
            AddListItem( LISTBOX_Level1, LABEL_TokenSelect.Text );
        }

        private void BUTTON_Level1_Remove_Click( object sender, EventArgs e )
        {
            RemoveListItem( LISTBOX_Level1 );
        }

        private void BUTTON_Level2_Add_Click( object sender, EventArgs e )
        {
            AddListItem( LISTBOX_Level2, LABEL_TokenSelect.Text );
        }

        private void BUTTON_Level2_Remove_Click( object sender, EventArgs e )
        {
            RemoveListItem( LISTBOX_Level2 );
        }


        /// <summary>
        /// Swaps two List Items (either tokens or elements) in a specified Elevated Item ListBox. 
        /// </summary>
        /// <param name="lb">ListBox which contains the tokens/elements to swap.</param>
        /// <param name="target">-1 if moving item up, 1 if moving item down</param>
        private void SwapListItem( ListBox lb, int target )
        {
            int current = lb.SelectedIndex;
            target = current + target;

            string temp = ( string )lb.Items[ target ];
            lb.Items[ target ] = lb.Items[ current ];
            lb.Items[ current ] = temp;

            lb.SelectedIndex = target;
            
            anyChangesElev = true;
        }


        /// <summary>
        /// Appends a token or element to a specified Elevated Item ListBox
        /// </summary>
        /// <param name="lb">ListBox to which to add an item</param>
        /// <param name="item">The name of the item to add</param>
        private void AddListItem( ListBox lb, string item )
        {
            lb.Items.Add( item );
            lb.SelectedIndex = lb.Items.Count - 1;

            anyChangesElev = true;
        }


        /// <summary>
        /// Removes the selected token or element from the specified Elevated Item ListBox
        /// </summary>
        /// <param name="lb">ListBox from which to delete the item</param>
        private void RemoveListItem( ListBox lb )
        {
            int sel = lb.SelectedIndex;

            lb.Items.RemoveAt( lb.SelectedIndex );

            if ( sel > ( lb.Items.Count - 1 ) )
                sel = lb.Items.Count - 1;
            lb.SelectedIndex = sel;

            anyChangesElev = true;
        }

		// *******************************************************************************************************************
		//   The following events ensure uniform interaction with the GUI when arrows are pressed.  Essentially, Up/Down
		//     should select a different excerpt in the list; Left/Right should select a new Token/Element
		// *******************************************************************************************************************

		private void DATAGRID_Elements_KeyDown( object sender, KeyEventArgs e )
		{
			DataGridView dgv = ( DataGridView )sender;
			int col = dgv.CurrentCellAddress.X;
			int row = dgv.CurrentCellAddress.Y;

			if( ( e.KeyData == Keys.Right ) & ( col == 1 ) ) {
				DATAGRID_ElemEnglish.Rows[ row ].Cells[ 0 ].Selected = true;
				e.SuppressKeyPress = true;
				DATAGRID_Elements.CurrentCell = null;
				DATAGRID_ElemEnglish.Select();
			}
		}

		private void DATAGRID_ElemEnglish_KeyDown( object sender, KeyEventArgs e )
		{
			DataGridView dgv = ( DataGridView )sender;
			int col = dgv.CurrentCellAddress.X;
			int row = dgv.CurrentCellAddress.Y;

			if( ( e.KeyData == Keys.Left ) & ( col == 0 ) ) {
				DATAGRID_Elements.Rows[ row ].Cells[ 1 ].Selected = true;
				e.SuppressKeyPress = true;
				DATAGRID_ElemEnglish.CurrentCell = null;
				DATAGRID_Elements.Select();
			}
		}


		



//		private void BUTTON_AddMI_Click( object sender, EventArgs e )
//		{
			// Must add one (empty) MI token to the Master List

//			int addRow = Token_Toolbar.MasterToken._miCount;

//			List<Token> newMI = new List<Token>();
//			newMI = Token_Toolbar.MasterToken._mi;
			//Token[] newMI = new Token[ Token_Toolbar.MasterToken._miCount + 1 ];

			// Copy the MI list, all the way up to the last row
//			for( int i = 0; i < addRow; i++ )
//				newMI[ i ] = Token_Toolbar.MasterToken._mi[ i ];

//			newMI[ addRow ] = new Token();

			// Commit changes to the master list
//			Token_Toolbar.MasterToken._mi = newMI;
//			Token_Toolbar.MasterToken._miCount = newMI.Count();

//			anyChangesMaster = true;

			// Reinitialize the MO grid, to reflect the changes made
//			InitMIgrid();

//			DATAGRID_MI.CurrentCell = DATAGRID_MI.Rows[ addRow ].Cells[ 0 ];
//		}


//		private void BUTTON_RemoveMI_Click( object sender, EventArgs e )
//		{
			// Must remove the currently selected MI token to the Master List

//			DataGridView dgv = DATAGRID_MI;
//			int row = dgv.CurrentCellAddress.Y;
//			dgv.DataSource = null;

//			List<Token> newMI = new List<Token>();
//			newMI = Token_Toolbar.MasterToken._mi;
			//Token[] newMI = new Token[ Token_Toolbar.MasterToken._miCount - 1 ];

			// Copy the MI list up until the current row
//			for( int i = 0; i < row; i++ )
//				newMI[ i ] = Token_Toolbar.MasterToken._mi[ i ];

			// Copy the rest of the MI list, starting at the location of the deleted row
//			for( int i = row; i < newMI.Count(); i++ )
//				newMI[ i ] = Token_Toolbar.MasterToken._mi[ i + 1 ];

			// Commit changes to the master list
//			Token_Toolbar.MasterToken._mi = newMI;
//			Token_Toolbar.MasterToken._miCount = newMI.Count();

//			anyChangesMaster = true;

			// Reinitialize the MI grid, to reflect the changes made
//			InitMIgrid();

//			if( row < Token_Toolbar.MasterToken._miCount )
//				DATAGRID_MI.CurrentCell = DATAGRID_MI.Rows[ row ].Cells[ 0 ];
//			else {
//				if( row > 0 )
//					DATAGRID_MI.CurrentCell = DATAGRID_MI.Rows[ row - 1 ].Cells[ 0 ];
//				else
//					BUTTON_RemoveMI.Enabled = false;
//			}

//		}


    }
}

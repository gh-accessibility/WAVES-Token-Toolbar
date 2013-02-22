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
    /// A full representation of one instance of the Shortcut Key Editor.
    /// </summary>
    public partial class Edit_Keys : Form
    {
        /// <summary>
        /// This boolean ensures that no changes are made to the data upon window initialization
        /// </summary>
        private static bool lockKeys;

        /// <summary>
        /// Tracks whether any changes has been made to the keyboard shortcuts
        /// </summary>
        private static bool anyChangesMade = false;

        /// <summary>
        /// A full backup of all keyboard shortcuts, for fallback if the user provides invalid shortcuts
        /// </summary>
        private KeyCombo[] keyCommandsBackup = new KeyCombo[ Token_Toolbar.KeyCommand.Count() ];

		/// <summary>
		/// An instance of the Toolbar that opened this window, to expose its information to the shortcut editor
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

        public Edit_Keys()
        {
            InitializeComponent();
        }

        private void Edit_Keys_Load( object sender, EventArgs e )
        {
			WavesToolbar = ( Token_Toolbar )this.Owner;

            // Make sure nothing gets edited when the DataViewGrid gets populated
            lockKeys = true;

            // Create a second copy of our list of KeyCommand.  Since when the DataListGrid is edited, it also actually
            //   edits the "DataSource" (which will be KeyCommand), we need this second copy in case the user makes a
            //   mistake in selecting a shortcut key for a menu.  If this validation fails, the backup will be a fallback.
            for ( int i = 0; i < Token_Toolbar.KeyCommand.Count(); i++ ) {
                keyCommandsBackup[ i ] = new KeyCombo();
                keyCommandsBackup[ i ]._alt = Token_Toolbar.KeyCommand[ i ]._alt;
                keyCommandsBackup[ i ]._ctrl = Token_Toolbar.KeyCommand[ i ]._ctrl;
                keyCommandsBackup[ i ]._mainKey = Token_Toolbar.KeyCommand[ i ]._mainKey;
            }

            // Initialize the DataGridView control.
            DATAGRID_Keys.DataSource = Token_Toolbar.KeyCommand;
            DATAGRID_Keys.RowHeadersWidth = 225;
            SetHeaders();
            DATAGRID_Keys.AutoResizeColumns();
            DATAGRID_Keys.Columns[ 2 ].Width = 75;
            
            // This makes the 4th column invisible
            DATAGRID_Keys.Columns[ 3 ].Visible = false;

            // Set the form's color scheme appropriately
            SetFormColors();

            // Set whether this window will be on top of all other windows
			this.TopMost = WavesToolbar.MENU_View_AlwaysOnTop.Checked;
            
            // Everything is alright to go!
            lockKeys = false;
        } // Form's Load event... initialize everything


        /// <summary>
        /// This function will set the colors of controls on this form based off of whether the user wants the
        ///   high contrast color option, or not.
        /// </summary>
        private void SetFormColors()
        {
			if( WavesToolbar.MENU_Options_HighContrast.Checked ) {
                // Apply High Contrast color scheme
                this.BackColor = Color.DarkBlue;
                this.ForeColor = Color.DodgerBlue;

                DATAGRID_Keys.BackgroundColor = Color.Azure;
                DATAGRID_Keys.DefaultCellStyle.BackColor = Color.Azure;
                DATAGRID_Keys.EnableHeadersVisualStyles = false;

                DATAGRID_Keys.ForeColor = Color.Black;
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

                DATAGRID_Keys.BackgroundColor = SystemColors.ControlLightLight;
                DATAGRID_Keys.DefaultCellStyle.BackColor = SystemColors.ControlLightLight;
                DATAGRID_Keys.EnableHeadersVisualStyles = true;
                DATAGRID_Keys.ForeColor = SystemColors.ControlText;
                //for (int i = 0; i < this.Controls.Count; i++)
                //{
                //    this.Controls[i].BackColor = SystemColors.Control;
                //    this.Controls[i].ForeColor = SystemColors.ControlText;
                //}
            }

        } // Set Form Colors (Apply color scheme)


        /// <summary>
        /// Set each of the Row Headers in the Key Combination DataGridList
        /// </summary>
        private void SetHeaders()
        {
			//DATAGRID_Keys.
			DATAGRID_Keys.TopLeftHeaderCell.Value = "Menu → Task";

			// These are the headers for each of the Columns
			DATAGRID_Keys.Columns[ 0 ].HeaderCell.Value = "Ctrl";
			DATAGRID_Keys.Columns[ 1 ].HeaderCell.Value = "Alt";
			DATAGRID_Keys.Columns[ 2 ].HeaderCell.Value = "Main Key";

            // These are the headers for each of the Shortcut Keys
            DATAGRID_Keys.Rows[ 0 ].HeaderCell.Value = "Formula → New";
            DATAGRID_Keys.Rows[ 1 ].HeaderCell.Value = "Formula → Import MathML";
			DATAGRID_Keys.Rows[ 2 ].HeaderCell.Value = "Formula → Undo";
			DATAGRID_Keys.Rows[ 3 ].HeaderCell.Value = "Formula → Clear Excerpt";
            DATAGRID_Keys.Rows[ 4 ].HeaderCell.Value = "Formula → Read";
            DATAGRID_Keys.Rows[ 5 ].HeaderCell.Value = "Formula → Copy to Clipboard";
            DATAGRID_Keys.Rows[ 6 ].HeaderCell.Value = "Formula → Export as MathML";
            DATAGRID_Keys.Rows[ 7 ].HeaderCell.Value = "Formula → Export as Graphic";
            DATAGRID_Keys.Rows[ 8 ].HeaderCell.Value = "Formula → Close";
            DATAGRID_Keys.Rows[ 9 ].HeaderCell.Value = "View → Always on Top";
            DATAGRID_Keys.Rows[ 10 ].HeaderCell.Value = "View → Menu";
			DATAGRID_Keys.Rows[ 11 ].HeaderCell.Value = "View → Use external MathJax renderer";
			DATAGRID_Keys.Rows[ 12 ].HeaderCell.Value = "View → Tokens";
            DATAGRID_Keys.Rows[ 13 ].HeaderCell.Value = "View → Repository";
            DATAGRID_Keys.Rows[ 14 ].HeaderCell.Value = "View → Auto-tune to Math Book";
            DATAGRID_Keys.Rows[ 15 ].HeaderCell.Value = "Options → English Excerpts";
            DATAGRID_Keys.Rows[ 16 ].HeaderCell.Value = "Options → High Contrast";
            DATAGRID_Keys.Rows[ 17 ].HeaderCell.Value = "Options → Read Superscript as Power";
            DATAGRID_Keys.Rows[ 18 ].HeaderCell.Value = "Options → Voice Prompting";
            DATAGRID_Keys.Rows[ 19 ].HeaderCell.Value = "Options → Edit Keyboard Shortcuts";
            DATAGRID_Keys.Rows[ 20 ].HeaderCell.Value = "Options → Edit Master Token List";
            DATAGRID_Keys.Rows[ 21 ].HeaderCell.Value = "Help → Index";
            DATAGRID_Keys.Rows[ 22 ].HeaderCell.Value = "Help → About";
        } // Set Row Headers
        

// *******************************************************************************************************************
//   The following functions make the changes made in this editor efficatious.
// *******************************************************************************************************************
        
        private void Edit_Keys_FormClosing( object sender, FormClosingEventArgs e )
        {
            // First of all, test the shortcut that we are currently editing, to make sure it is valid.
            TestShortcutKey( DATAGRID_Keys );

            // A message box appears to ask user whether changes should be saved (if any changes were indeed made).
			DialogResult answer = DialogResult.No;
			if( confirmPressed )
				answer = DialogResult.Yes;

			if( ( anyChangesMade ) & ( !cancelPressed ) & ( !confirmPressed ) ) {
                answer = MessageBox.Show( "You've made edits to the shortcut keys.\nWill you save your changes?", "WAVES-Token-Toolbar", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question );
            }
            
            if (answer == DialogResult.Cancel ) {
                // If the user pressed "Cancel", then the form will not close, and nothing will be changed.
                e.Cancel = true;
            } // User answered "Cancel"
            else
            {
                if ( answer == DialogResult.Yes ) {
                    // If so, save information, in an .xml file, but in a textual way
                    SaveKeysToXml();

                    // Then, re-initialize the menu shortcut keys on the main form.
					WavesToolbar.AssignKeyToMenu( Token_Toolbar.KeyCommand[ 0 ], WavesToolbar.MENU_Formula_New );
					WavesToolbar.AssignKeyToMenu( Token_Toolbar.KeyCommand[ 1 ], WavesToolbar.MENU_Formula_ImportMathML );
					WavesToolbar.AssignKeyToMenu( Token_Toolbar.KeyCommand[ 2 ], WavesToolbar.MENU_Formula_Undo );
					WavesToolbar.AssignKeyToMenu( Token_Toolbar.KeyCommand[ 3 ], WavesToolbar.MENU_Formula_ClearExcerpt );
					WavesToolbar.AssignKeyToMenu( Token_Toolbar.KeyCommand[ 4 ], WavesToolbar.MENU_Formula_Read );
					WavesToolbar.AssignKeyToMenu( Token_Toolbar.KeyCommand[ 5 ], WavesToolbar.MENU_Formula_CopyClipboard );
					WavesToolbar.AssignKeyToMenu( Token_Toolbar.KeyCommand[ 6 ], WavesToolbar.MENU_Formula_ExportMathML );
					WavesToolbar.AssignKeyToMenu( Token_Toolbar.KeyCommand[ 7 ], WavesToolbar.MENU_Formula_ExportGraphic );
					WavesToolbar.AssignKeyToMenu( Token_Toolbar.KeyCommand[ 8 ], WavesToolbar.MENU_Formula_Close );
					WavesToolbar.AssignKeyToMenu( Token_Toolbar.KeyCommand[ 9 ], WavesToolbar.MENU_View_AlwaysOnTop );
					WavesToolbar.AssignKeyToMenu( Token_Toolbar.KeyCommand[ 10 ], WavesToolbar.MENU_View_HideMenu );
					WavesToolbar.AssignKeyToMenu( Token_Toolbar.KeyCommand[ 11 ], WavesToolbar.MENU_View_MathJax );
					WavesToolbar.AssignKeyToMenu( Token_Toolbar.KeyCommand[ 12 ], WavesToolbar.MENU_View_Level2 );
					WavesToolbar.AssignKeyToMenu( Token_Toolbar.KeyCommand[ 13 ], WavesToolbar.MENU_View_Repository );
					WavesToolbar.AssignKeyToMenu( Token_Toolbar.KeyCommand[ 14 ], WavesToolbar.MENU_View_Autotune );
					WavesToolbar.AssignKeyToMenu( Token_Toolbar.KeyCommand[ 15 ], WavesToolbar.MENU_Options_EnglishExcerpts );
					WavesToolbar.AssignKeyToMenu( Token_Toolbar.KeyCommand[ 16 ], WavesToolbar.MENU_Options_HighContrast );
					WavesToolbar.AssignKeyToMenu( Token_Toolbar.KeyCommand[ 17 ], WavesToolbar.MENU_Options_ReadAsPower );
					WavesToolbar.AssignKeyToMenu( Token_Toolbar.KeyCommand[ 18 ], WavesToolbar.MENU_Options_VoicePrompting );
					WavesToolbar.AssignKeyToMenu( Token_Toolbar.KeyCommand[ 19 ], WavesToolbar.MENU_Options_EditKeyboard );
					WavesToolbar.AssignKeyToMenu( Token_Toolbar.KeyCommand[ 20 ], WavesToolbar.MENU_Options_EditMasterTokenList );
					WavesToolbar.AssignKeyToMenu( Token_Toolbar.KeyCommand[ 21 ], WavesToolbar.MENU_Help_Index );
					WavesToolbar.AssignKeyToMenu( Token_Toolbar.KeyCommand[ 22 ], WavesToolbar.MENU_Help_About );
                    //Token_Toolbar.AssignKeyToMenu(Token_Toolbar.KeyCommand[20], owner.MENU_Help_Shortcuts);
                    //Token_Toolbar.AssignKeyToMenu(Token_Toolbar.KeyCommand[21], );

                    // Yeah, this looks useless.  However, this hack should help should the shortcut keys for two menu items
                    //   get swapped.  For instance, switching Del and Ctrl+Del for "Clear Excerpt" and "New Formula" functions.
                    //Token_Toolbar.AssignKeyToMenu(Token_Toolbar.KeyCommand[20], owner.MENU_Help_Shortcuts);
                    //Token_Toolbar.AssignKeyToMenu(Token_Toolbar.KeyCommand[19], owner.MENU_Help_Index);
					WavesToolbar.AssignKeyToMenu( Token_Toolbar.KeyCommand[ 21 ], WavesToolbar.MENU_Help_Index );
					WavesToolbar.AssignKeyToMenu( Token_Toolbar.KeyCommand[ 20 ], WavesToolbar.MENU_Options_EditMasterTokenList );
					WavesToolbar.AssignKeyToMenu( Token_Toolbar.KeyCommand[ 19 ], WavesToolbar.MENU_Options_EditKeyboard );
					WavesToolbar.AssignKeyToMenu( Token_Toolbar.KeyCommand[ 18 ], WavesToolbar.MENU_Options_VoicePrompting );
					WavesToolbar.AssignKeyToMenu( Token_Toolbar.KeyCommand[ 17 ], WavesToolbar.MENU_Options_ReadAsPower );
					WavesToolbar.AssignKeyToMenu( Token_Toolbar.KeyCommand[ 16 ], WavesToolbar.MENU_Options_HighContrast );
					WavesToolbar.AssignKeyToMenu( Token_Toolbar.KeyCommand[ 15 ], WavesToolbar.MENU_Options_EnglishExcerpts );
					WavesToolbar.AssignKeyToMenu( Token_Toolbar.KeyCommand[ 14 ], WavesToolbar.MENU_View_Autotune );
					WavesToolbar.AssignKeyToMenu( Token_Toolbar.KeyCommand[ 13 ], WavesToolbar.MENU_View_Repository );
					WavesToolbar.AssignKeyToMenu( Token_Toolbar.KeyCommand[ 12 ], WavesToolbar.MENU_View_Level2 );
					WavesToolbar.AssignKeyToMenu( Token_Toolbar.KeyCommand[ 11 ], WavesToolbar.MENU_View_MathJax );
					WavesToolbar.AssignKeyToMenu( Token_Toolbar.KeyCommand[ 10 ], WavesToolbar.MENU_View_HideMenu );
					WavesToolbar.AssignKeyToMenu( Token_Toolbar.KeyCommand[ 9 ], WavesToolbar.MENU_View_AlwaysOnTop );
					WavesToolbar.AssignKeyToMenu( Token_Toolbar.KeyCommand[ 8 ], WavesToolbar.MENU_Formula_Close );
					WavesToolbar.AssignKeyToMenu( Token_Toolbar.KeyCommand[ 7 ], WavesToolbar.MENU_Formula_ExportGraphic );
					WavesToolbar.AssignKeyToMenu( Token_Toolbar.KeyCommand[ 6 ], WavesToolbar.MENU_Formula_ExportMathML );
					WavesToolbar.AssignKeyToMenu( Token_Toolbar.KeyCommand[ 5 ], WavesToolbar.MENU_Formula_CopyClipboard );
					WavesToolbar.AssignKeyToMenu( Token_Toolbar.KeyCommand[ 4 ], WavesToolbar.MENU_Formula_Read );
					WavesToolbar.AssignKeyToMenu( Token_Toolbar.KeyCommand[ 3 ], WavesToolbar.MENU_Formula_ClearExcerpt );
					WavesToolbar.AssignKeyToMenu( Token_Toolbar.KeyCommand[ 2 ], WavesToolbar.MENU_Formula_Undo );
					WavesToolbar.AssignKeyToMenu( Token_Toolbar.KeyCommand[ 1 ], WavesToolbar.MENU_Formula_ImportMathML );
					WavesToolbar.AssignKeyToMenu( Token_Toolbar.KeyCommand[ 0 ], WavesToolbar.MENU_Formula_New );
                } // User answered "yes"
                else
                {
                    // If the answer was no, we must reset the KeyCommand to what they were!
                    for ( int i = 0; i < keyCommandsBackup.Count(); i++ ) {
                        Token_Toolbar.KeyCommand[ i ]._ctrl = keyCommandsBackup[ i ]._ctrl;
                        Token_Toolbar.KeyCommand[ i ]._alt = keyCommandsBackup[ i ]._alt;
                        Token_Toolbar.KeyCommand[ i ]._mainKey = keyCommandsBackup[ i ]._mainKey;
                    }
                } // User answered "no"

            } // All answers accounted for

			if( WavesToolbar != null ) {
				WavesToolbar.Enabled = true;
				//WavesToolbar.ReinitializeControls();
			}
        } // Form Closing event... based on user confirmation, accept or discard changes.  Save XML and change menus if
          //   changes were accepted.


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
        /// Save the changes to the shortcut key combinations to key-mapping.xml, which is retrieved upon application startup.
        /// </summary>
        private void SaveKeysToXml()
        {
            // It is important to note that if you are in a build environment, it does not save it to the /config
            //   directory, and so you will need to save the version of "key-mapping.xml" over the version in /config
            //   if you want these changes to persist between builds.
            
            // Preamble
            string toSave = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>\n";

            // Root node
			toSave += "<shortcuts>\n";

            // Each key combination is delimited by its own command name.
            //   Further, each key combination specifies a Control key state, and Alt key state, and a main key.
            for ( int i = 0; i < Token_Toolbar.KeyCommand.Count(); i++ ) {
                toSave += "  <" + GetSaveToken( i ) + ">\n";
                toSave += "      <control>" + GetBoolString( Token_Toolbar.KeyCommand[ i ]._ctrl ) + "</control>";
                toSave += "<alt>" + GetBoolString( Token_Toolbar.KeyCommand[ i ]._alt ) + "</alt>";
                toSave += "<key>" + ( Token_Toolbar.KeyCommand[ i ]._mainKey ) + "</key>\n";
                toSave += "  </" + GetSaveToken( i ) + ">\n";
            }

            // That's it once the previous loop finishes
			toSave += "</shortcuts>\n";

            // Save the file as one long string
            System.IO.StreamWriter fileWriter = new System.IO.StreamWriter( Token_Toolbar.CurDir + "\\key-mapping.xml" );
            fileWriter.Write( toSave );
            fileWriter.Close();

        } // Save Shortcut Key changes to key-mapping.xml


        /// <summary>
        /// Returns a string representation of a boolean.
        /// </summary>
        /// <param name="b">Boolean to convert</param>
        /// <returns>"yes" for true, "no" for false.</returns>
        private string GetBoolString( bool b )
        {
            if ( b )    return "yes";
            else        return "no";
        } // Get Bool String... gives a "yes/no" respresentation of a boolean


        /// <summary>
        /// Returns the proper xml instance for the keystroke combination at the specified ID.
        /// </summary>
        /// <param name="id">Keystroke combination index</param>
        /// <returns>XML node name for the Keystroke combination</returns>
        private string GetSaveToken( int id )
        {
            // This function is a way of converting an id into the xml tag necessary for the key-mapping.xml file.
            string answer = "";

            switch ( id ) {
                case 0:
                    answer = "formula_new";
                    break;

                case 1:
                    answer = "formula_import-mathml";
                    break;

				case 2:
					answer = "formula_undo";
					break;

				case 3:
                    answer = "formula_clear-excerpt";
                    break;

                case 4:
                    answer = "formula_read";
                    break;

                case 5:
                    answer = "formula_copy-to-clipboard";
                    break;

                case 6:
                    answer = "formula_export-as-mathml";
                    break;

                case 7:
                    answer = "formula_export-as-graphic";
                    break;

                case 8:
                    answer = "formula_close";
                    break;

                case 9:
                    answer = "view_always-on-top";
                    break;

                case 10:
                    answer = "view_menu";
                    break;

				case 11:
					answer = "view_mathjax";
					break;

				case 12:
                    answer = "view_tokens";
                    break;

				case 13:
                    answer = "view_repository";
                    break;

                case 14:
                    answer = "view_auto-tune-to-math-book";
                    break;

                case 15:
                    answer = "options_english-excerpts";
                    break;

                case 16:
                    answer = "options_high-contrast";
                    break;

                case 17:
                    answer = "options_read-superscript-as-power";
                    break;

                case 18:
                    answer = "options_voice-prompting";
                    break;

                case 19:
                    answer = "options_edit-keyboard-shortcuts";
                    break;

                case 20:
                    answer = "options_edit-master-token-list";
                    break;

                case 21:
                    answer = "help_index";
                    break;

                case 22:
                    answer = "help_about";
                    break;

                default:
                    break;
            }

            return answer;
        } // Get Save Token- returns the name of a tag for saving the key-mapping.xml file, by id number



        private void Edit_Keys_KeyUp( object sender, KeyEventArgs e )
        {
            // This window will close when the Escape key is pressed
            if ( e.KeyCode == Keys.Escape )         this.Close();
        } // Key Up event


// *******************************************************************************************************************
//   These following functions describe the behavior of the DataGridView list, which drives the entire editing process.
// *******************************************************************************************************************

        private void DATAGRID_Keys_PreviewKeyDown( object sender, PreviewKeyDownEventArgs e )
        {
            // This function makes sure the Escape key gets trapped and does not fire for the DataGridView control.
            if ( e.KeyData == Keys.Escape )			e.IsInputKey = false;
        }


        private void DATAGRID_Keys_Row_Column_Cell_Enter( object sender, DataGridViewCellEventArgs e )
        {
            // When this event fires, make sure that the current row is selected (not a column or a cell)
            DataGridView dgv = ( DataGridView )sender;

            int row = dgv.CurrentCellAddress.Y;
            if ( row == -1 ) return;

            for ( int i = 0; i < dgv.Rows.Count; i++ ) {
                if ( i != row ) 
					dgv.Rows[ i ].Selected = false;
                else 
					dgv.Rows[ i ].Selected = true;
            }

			//dgv.CurrentCell.ToolTipText = "Hi!";
        } // Overloaded DATAGRID_Keys_Row_Column_Cell_Enter... selects the current Row to indicate that each shortcut
        // is set all-at-once, and not piecemeal


        private void DATAGRID_Keys_Row_Column_Cell_Enter( object sender, EventArgs e )
        {
            // When this event fires, make sure that the current row is selected (not a column or a cell)
            DataGridView dgv = ( DataGridView )sender;

            int row = dgv.CurrentCellAddress.Y;
            if ( row == -1 ) return;

            for ( int i = 0; i < dgv.Rows.Count; i++ ) {
                if ( i != row ) 
					dgv.Rows[ i ].Selected = false;
                else 
					dgv.Rows[ i ].Selected = true;
            }

			//dgv.CurrentCell.ToolTipText = "Hello.";
        } // Overloaded DATAGRID_Keys_Row_Column_Cell_Enter... selects the current Row to indicate that each shortcut
        // is set all-at-once, and not piecemeal


		private void DATAGRID_Keys_MouseHover( object sender, EventArgs e )
		{
			//DataGridView dgv = ( DataGridView )sender;
			//dgv.CurrentCell.ToolTipText = "Hello.";
			//dgv.ShowCellToolTips = true;
		}
    

        private void DATAGRID_Keys_RowLeave( object sender, DataGridViewCellEventArgs e )
        {
             // Don't want the event to take effect if grid is in process of populating
            if ( lockKeys == true )        return;

            DataGridView dgv = ( DataGridView )sender;
            TestShortcutKey( dgv );
        } // Upon leaving a row, make sure each edit is valid


        /// <summary>
        /// This function tests the currently edited shortcut key to make sure it is a valid shortcut combination
        /// </summary>
        /// <param name="dgv">A handle to the Key Combination DataGridView</param>
        private void TestShortcutKey( DataGridView dgv )
        {
            // First, get the keycode for the currently edited row
            int row = dgv.CurrentCellAddress.Y;
            KeyCombo com = new KeyCombo( ( bool )dgv[ 0, row ].EditedFormattedValue, ( bool )dgv[ 1, row ].EditedFormattedValue, ( string )dgv[ 2, row ].EditedFormattedValue );

            // If this combo is the same as the backup version... it never changed
            if ( ( com._mainKey == keyCommandsBackup[ row ]._mainKey ) &&
                 ( com._alt == keyCommandsBackup[ row ]._alt ) &&
                 ( com._ctrl == keyCommandsBackup[ row ]._ctrl ) )                 return;

            // The function "IsValidShortcut" does the heavy lifting for this operation
            if ( com.IsValidShortcut() ) {
                // These assignments are automatic and are not explicitly needed!
                //   KeyCommand[row].Ctrl = (bool)dgv[0, row].Value;
                //   KeyCommand[row].Alt = (bool)dgv[1, row].Value;
                //   KeyCommand[row].mainKey = (string)dgv[2, row].Value;

                anyChangesMade = true;
            }
            else
            {
                // The user entered invalid data.  Restore the originally backed up combo (although this might be Keys.None)!
                dgv[ 0, row ].Value = keyCommandsBackup[ row ]._ctrl;
                dgv[ 1, row ].Value = keyCommandsBackup[ row ]._alt;
                dgv[ 2, row ].Value = keyCommandsBackup[ row ]._mainKey;
            }

        } // Test Shortcut Key... computes the shortcut key combination to test to see if it is valid


        private void DATAGRID_Keys_KeyDown( object sender, KeyEventArgs e )
        {
            // This function sets one key-combination to a shortcut key instance.  This may be invalid... this will be
            //   checked for later.
            if ( e.KeyData == Keys.Escape )
				e.SuppressKeyPress = true;

            DataGridView dgv = ( DataGridView )sender;

            int col = dgv.CurrentCellAddress.X;
            int row = dgv.CurrentCellAddress.Y;

            if ( e.KeyCode == Keys.Back ) {
                // Backspace key removes a shortcut
                dgv[ 0, row ].Value = false;
                dgv[ 1, row ].Value = false;
                dgv[ 2, row ].Value = "";

                keyCommandsBackup[ row ]._ctrl = false;
                keyCommandsBackup[ row ]._alt = false;
                keyCommandsBackup[ row ]._mainKey = "";

                anyChangesMade = true;
                return;
            }

            // Keycodes which will do nothing
            if ( e.KeyCode == Keys.None )             return;
            if ( e.KeyCode == Keys.ShiftKey )         return;
            if ( e.KeyCode == Keys.Down )             return;
            if ( e.KeyCode == Keys.Up )               return;


            // Set Control key state
            if ( e.Modifiers == Keys.Control )        dgv[ 0, row ].Value = true;
            else                                      dgv[ 0, row ].Value = false;

            // Set Alt key state
            if ( e.Modifiers == Keys.Alt )            dgv[ 1, row ].Value = true;
            else                                      dgv[ 1, row ].Value = false;

			// Set Control and Alt state if both have been pressed
			if( e.Modifiers == ( Keys.Alt | Keys.Control ) ) {
				dgv[ 0, row ].Value = true;
				dgv[ 1, row ].Value = true;
			}

            // Extract the main key's code from the Keys data
            int baseKeyCode = GetLeastSignificantBits( ( int )e.KeyData );
            KeysConverter kc = new KeysConverter();
            string keyChar = kc.ConvertToString( baseKeyCode );

            // Make sure that a main key has actually been pressed (not just Control or Alt)
            if ( ( keyChar == "Menu" ) || ( keyChar == "ControlKey" ) )       return;

            // Set main key
            dgv[ 2, row ].Value = keyChar;

        } // Keys Down event... fires when any combination of keys are pressed


        /// <summary>
        /// Returns the least significant 8 bits of a 32-bit integer.
        /// </summary>
        /// <param name="intValue">A 4 byte integer (int)</param>
        /// <returns>A 4 byte integer in the form 0x000000xx</returns>
        private int GetLeastSignificantBits( int intValue )
        {
            // This is useful in receiving only the mere ASCII info from the Keys.KeyCode member
            // http://www.java2s.com/Code/CSharp/Data-Types/ObtainingtheMostSignificantorLeastSignificantBitsofaNumber.htm

            return ( intValue & 0x000000FF );
        }



// *******************************************************************************************************************
//   These following functions used to have a purpose, but they don't now.
// *******************************************************************************************************************

        private void DATAGRID_Keys_RowPostPaint( object sender, DataGridViewRowPostPaintEventArgs e )
        {
            // This removes the triangle in the header of the current row.

            // Since we want an entire row to be selected at all times, this actually is counter-productive.
            //e.PaintHeader(DataGridViewPaintParts.All & ~DataGridViewPaintParts.ContentBackground);
        } // Make sure that the unsightly triangle is painted over

        private void DATAGRID_Keys_CellValueChanged( object sender, DataGridViewCellEventArgs e )
        {
            // We use a different method to change the values of the cells... and of the underlying DataSource
            if ( lockKeys == true )        return;
        }

    }
}

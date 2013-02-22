using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

//using XML_functions;
using System.Speech.Synthesis;              // Namespace for Speech Synthesis
using System.IO;

namespace Prototype_Token_Interface
{
    /// <summary>
    /// A full representation of one instance of the Token Repository.
    /// </summary>
    public partial class Level3 : Form
    {
        /// <summary>
        /// Number of MO tokens in Master Token List.  This is responsible for dynamically repositioning MO's when resizing window
        /// </summary>
		private int moCount = 0;

        /// <summary>
        /// Number of MI tokens in Master Token List.  This is responsible for dynamically repositioning MI's when resizing window
        /// </summary>
		private int miCount = 0;

        /// <summary>
        /// Number of MathML elements in Master Token List
        /// </summary>
        private int elemCount = 0;

        /// <summary>
        /// Makes sure that if the Repository is opened via keyboard press, that it isn't immediately closed
        /// </summary>
        private bool closeImmunity = true;

		/// <summary>
		/// An instance of the Toolbar that opened this window, to expose its information to the token editor
		/// </summary>
		private Token_Toolbar WavesToolbar;


        public Level3()
        {
            InitializeComponent();
        }

// *******************************************************************************************************************
//   These functions initialize the Token and Element buttons in the Repository window, and configure the GUI.
// *******************************************************************************************************************
        
        private void Level3_Load( object sender, EventArgs e )
        {
			WavesToolbar = ( Token_Toolbar )this.Owner;

            // Populate the Repository with all pre-loaded elements and tokens
            moCount = PopulateToolbar( "mo", 0 );
            miCount = PopulateToolbar( "mi", moCount );
            elemCount = PopulateElements();

            // Set the color scheme
            SetFormColors();

            // The first MathML element is selected
            this.Controls[ moCount + miCount ].Select();

            // Set whether this window will be on top of all other windows
			this.TopMost = WavesToolbar.MENU_View_AlwaysOnTop.Checked;

			// Set the Repository's default size
			this.Width = 750;
        } // Repository Load event


		private void Level3_FormClosing( object sender, FormClosingEventArgs e )
		{
			if( WavesToolbar != null ) {
				WavesToolbar.Enabled = true;
				//WavesToolbar.ReinitializeControls();
			}
		}


        /// <summary>
        /// Dynamically creates a button on the toolbar for every token of the specified type in the master-token-list.xml file.
        /// </summary>
        /// <param name="type">"mo" or "mi"</param>
        /// <param name="start">The Control ID at which to begin adding buttons</param>
        /// <returns>The number of buttons created</returns>
        private int PopulateToolbar( string type, int start )
        {
            // Declaring variables.  They depend on global data already loaded from Master Token XML
            List<Token> tokenType = new List<Token>();
            int count = 0;
            int newHeight = 0;

            if ( type == "mo" )
            {
                tokenType = Token_Toolbar.MasterToken._mo;
                count = Token_Toolbar.MasterToken._moCount;
            }
            else if ( type == "mi" )
            {
                tokenType = Token_Toolbar.MasterToken._mi;
                count = Token_Toolbar.MasterToken._miCount;
            } 

            // Populate the Repository with all mo tokens
            for ( int i = start; i < start + count; i++ )
            {
                // For each token, create a button in the Repository which is captioned by the token
                Button button = new Button();

                Token t = tokenType[ i - start ];
                if ( t != null )
                {
                    button.Text = t._symbol;

					if( t._symbol.Length < 1 ) {
						button.Font = new System.Drawing.Font( "Lucida Sans Unicode", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ) );
					}
					else {
						// This would be a function <mi>... with >1 character
						button.Font = new System.Drawing.Font( "Lucida Sans Unicode", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ) );
					}
					//button.Font.Height = 20;

                    // Placement information for the new button
                    int dimensionX = ( this.Width - 25 ) / 50;
                    int locX = 10 + ( 50 * ( i % dimensionX ) );
                    int locY = 70 + ( 40 * Convert.ToInt32( i / dimensionX ) );
                    button.Location = new Point( locX, locY );

                    button.Width = 40;
                    button.Height = 30;

                    // Event information for the new button.  It takes effect when clicked.
                    button.Click += new EventHandler( Button_Level3_Click );
                    button.Enter += new EventHandler( Button_Level3_Enter );
                    button.MouseEnter += new EventHandler( Button_Level3_Enter );
                    button.KeyDown += new KeyEventHandler( Button_Level3_KeyDown );
                    button.PreviewKeyDown += new PreviewKeyDownEventHandler( Button_Level3_PreviewKeyDown );
                    button.Tag = i;

                    // Add the button to the form
                    this.Controls.Add( button );

                    // Determine the final height of the window
                    newHeight = locY + 85;
                }
            }

            this.Height = newHeight;
            return count;
        }


        /// <summary>
        /// Dynamically creates a button on the toolbar for every MathML element in the master-token-list.xml file.
        /// </summary>
        /// <returns>The number of buttons created</returns>
        private int PopulateElements()
        {
            int xIntPlacement = 0;

            // Populate the Repository with all MathML elements
            for ( int i = 0; i < Token_Toolbar.MasterToken._elemCount; i++ )
            {
                // For each token, create a button in the Repository which is captioned by the token
                Button button = new Button();

				Token t = Token_Toolbar.MasterToken._mathMLelement[ i ] as Token;

                if ( t._symbol != "mrow" )
                {
					button.Name = t._symbol;

					string iconPath = Token_Toolbar.GetBasePath() + "\\graphics\\elem-" + t._symbol + ".ico";
					if( File.Exists( iconPath ) ) {
						// Given a pictoral representation of this element exists, use it as the button's face
						button.Image = Image.FromFile( iconPath );
						button.ImageAlign = ContentAlignment.MiddleCenter;
					}
					else {
						// If no pictoral representation for this element exists, we have to settle for text
						button.Text = t._symbol;
					}

//                    try
//                    {
//                        button.Image = Image.FromFile( Token_Toolbar.GetBasePath() + "\\graphics\\elem-" + t._symbol + ".ico" );
//                        button.ImageAlign = ContentAlignment.MiddleCenter;
//                    }
//                    catch
//                    {
//                        button.Text = t._symbol;                      
//                    }
//					button.Name = t._symbol;

					if( t._symbol.Length < 5 ) {
						button.Font = new System.Drawing.Font( "Lucida Sans Unicode", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ) );
					}
					else {
						button.Font = new System.Drawing.Font( "Lucida Sans Unicode", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ) );
					}

                    // Placement information for the new button
                    int dimensionX = ( this.Width - 25 ) / 50;
                    int locX = 10 + ( 50 * ( xIntPlacement % dimensionX ) );
                    int locY = 10;
                    button.Location = new Point( locX, locY );

                    button.Width = 40;
                    button.Height = 40;

                    // Event information for the new button.  It takes effect when clicked.
                    button.Click += new EventHandler( Button_Element_Click );
                    button.Enter += new EventHandler( Button_Level3_Enter );
                    button.MouseEnter += new EventHandler( Button_Level3_Enter );
                    button.KeyDown += new KeyEventHandler( Button_Level3_KeyDown );
                    button.PreviewKeyDown += new PreviewKeyDownEventHandler( Button_Level3_PreviewKeyDown );

                    button.Tag = -1;
                    for ( int j = 0; j < Token_Toolbar.MasterToken._elemCount; j++ )
                    {
                        if ( Token_Toolbar.MasterToken._mathMLelement[ j ]._symbol == button.Name ) 
							button.Tag = j;
                    }
                    //button.Tag = t.speech;
                    //button.Name = t.speech;

                    // Add the button to the form
                    this.Controls.Add( button );

                    // Determine the final height of the window
                    xIntPlacement++;
                }
            }

            this.MinimumSize = new Size( 140 + ( xIntPlacement * 40 ), this.Height );
            //this.Height = newHeight;
            return Token_Toolbar.MasterToken._elemCount;

        } // PopulateElements function


        /// <summary>
        /// This function will set the colors of controls on this form based off of whether the user wants the high contrast color option, or not.
        /// </summary>
        private void SetFormColors()
        {
			if( WavesToolbar.MENU_Options_HighContrast.Checked )
            {
                // Apply High Contrast color scheme
                this.BackColor = Color.DarkBlue;
                this.ForeColor = Color.DodgerBlue;

                for ( int i = 0; i < this.Controls.Count; i++ )
                {
                    this.Controls[ i ].BackColor = Color.Azure;
                    this.Controls[ i ].ForeColor = Color.Black;
                }
            }
            else
            {
                // Apply normal color scheme

                this.BackColor = SystemColors.Control;
                this.ForeColor = SystemColors.ControlText;

                for ( int i = 0; i < this.Controls.Count; i++ )
                {
                    this.Controls[ i ].BackColor = SystemColors.Control;
                    this.Controls[ i ].ForeColor = SystemColors.ControlText;
                }
            }
        } // SetFormColors function


// *******************************************************************************************************************
//   These functions determine the behavior of the form and the buttons therein
// *******************************************************************************************************************
        
        /// <summary>
        /// Add a token to the formula being constructed in the Token Toolbar.
        /// </summary>
        /// <param name="sender">The dynamically created Button that was clicked</param>
        /// <param name="e">Event data pertinent to Button clicks</param>
        void Button_Level3_Click( object sender, EventArgs e )
        {
            // This event is raised when a dynamic token button is clicked on the form
            //   These dynamic buttons were created from the Level 1/2 elevated tokens .xml.
            //   The caption of the button is appended to the textbox's "expression".
            Button button = sender as Button;
            if ( button != null )
            {
                int id = ( int )button.Tag;
				this.Owner.Controls.Find( "TEXTBOX_MathExpression", true ).First().Text += ( string )button.Text;
            }
        } // Dynamic Token Button click event


        /// <summary>
        /// Add an element to the formula being constructed in the Token Toolbar.
        /// </summary>
        /// <param name="sender">The dynamically created Button that was clicked</param>
        /// <param name="e">Event data pertinent to Button clicks</param>
        private void Button_Element_Click( object sender, EventArgs e )
        {
            // Cannot add any MathML tags to an <mtext> element, but all other tags allow nesting
            // This is somewhat fun, because this requires a call to the Token_Toolbar's implementation
            //   of adding Elements.

            string label = this.Owner.Controls.Find( "LABEL_Wrapper", true ).First().Text;

            // Cannot add any MathML tags to an <mtext> element, but all other tags allow nesting
            if ( label.Substring(label.Length - 5, 5) == "mtext" )        return;
            if ( label.Substring(label.Length - 7, 7) == "literal" )      return;

            Button b = ( Button )sender;

			string tag = b.Name;
            int args = Token_Toolbar.MasterToken._mathMLelement[ ( int )b.Tag ]._args;

			WavesToolbar.TEXTBOX_MathExpression.SelectionStart = WavesToolbar.TEXTBOX_MathExpression.Text.Length;
			int jumpHere = WavesToolbar.AddTagToFormula( tag, args, WavesToolbar.LISTBOX_ShowExcerpts.SelectedIndex, true );

			WavesToolbar.LISTBOX_ShowExcerpts.SelectedIndex = jumpHere;
        } // Dynamic Element Button click event



        private void Level3_Resize( object sender, EventArgs e )
        {
            // This "Level 3 token" Repository can resize, based on the user's wishes.  The dynamic buttons are 
            //   re-placed on the form as necessary.
            int numControls = moCount + miCount;  // not "this.Controls.Count"
            int dimensionX = ( this.Width - 25 ) / 50;

            for ( int i = 0; i < numControls; i++ )
            {
                int j = i;
                
                int locX = 10 + ( 50 * ( j % dimensionX ) );
                int locY = 70 + ( 40 * Convert.ToInt32( j / dimensionX ) );
                this.Controls[i].Location = new Point( locX, locY );
            }  
     
            if ( numControls > 0 )        this.Height = 150 + ( ( ( int )numControls / dimensionX ) * 40 );
        } // Repository Resize event


        private void Level3_KeyUp( object sender, KeyEventArgs e )
        {
            // This event determines what to do when a key has been released.  Because of the argument difference between
            //   the KeyPress event and this KeyUp event, the KeyUp event is actually the more versatile of the two events.
            //   This function mostly deals with key-combos and keycodes that are important to the overall working of the
            //   toolbar.

            // This function has been made more complex by the necessity of making the keys that drive
            //   the toolbar dynamic, based off of a companion XML.

            if ( e.KeyCode == Keys.Escape ) this.Close();

            // Test for Token Repository key to close the window. Close immunity is so the window doesn't close upon opening.
            if ( !closeImmunity )
            {
                if ( Token_Toolbar.KeyCommand[ 13 ].TestKeyCombo( e ) ) 
					this.Close();
            }
            else
            {
                closeImmunity = false;
            }

            int numControls = moCount + miCount;  // not "this.Controls.Count"

            // The Down key activates the dynamic button in the same column, next row down
            if ( e.KeyData == Keys.Down )
            {
                int dimensionX = ( this.Width - 25 ) / 50;
                int newControl;
                int curFocus = 0;

                // Get the index of the button which is selected
                for ( int i = 0; i < this.Controls.Count; i++ )
                {
                    if ( this.Controls[ i ].ContainsFocus )
						curFocus = i;
                }

                // Find out the selected button's X-coordinate
                int curControlX = this.Controls[ curFocus ].Location.X;
               
                // Compute the next button to select
                newControl = curFocus + dimensionX;
                
                try
                {
                    if (this.Controls[ newControl ].Location.X != curControlX)
                    {
                        // If this check is true, then we'll need to do more work to make the button underneath the previous one selected.
                        //   Specifically, we'll need to continually check the previous button, until it has an X-location that would place it
                        //   on the next row.

                        while ( this.Controls[ newControl ].Location.X != curControlX)
                            newControl--;
                    }
                }
                catch
                {
                    // Of course, the if statement in the try block could fail, if it checks a non-existant (uninitialized) Control id.  
                    if ( newControl >= moCount + miCount + dimensionX )
                    {
                        // If newControl was outside the bounds, it will become the control in the first row, with the same column
                        //   as the originally focused button.
                        newControl = ( curControlX - 10 ) / 50;
                    }
                    else
                    {
                        // Otherwise, we will need to step through the Element buttons, to find the one in the correct column.
                        newControl = 0;
                        for ( int i = moCount + miCount; i < this.Controls.Count; i++ )
                            if ( this.Controls[ i ].Location.X == curControlX )                 
								newControl = i;

                        // If we still do not have a correct button to select, select the one in the first row, with the same column
                        //   as the originally focused button.
                        if ( newControl == 0 ) 
							newControl = ( curControlX - 10 ) / 50;
                    }
                }

                // Activate the button we have determined.
                this.ActiveControl = this.Controls[ newControl ];
            }

            // The Up key activates the dynamic button in the same column, next row up
            if ( e.KeyData == Keys.Up )
            {
                int dimensionX = ( this.Width - 25 ) / 50;
                int newControl;
                int curFocus = 0;

                // Get the index of the button which is selected
                for ( int i = 0; i < this.Controls.Count; i++ )
                {
                    if ( this.Controls[ i ].ContainsFocus )
						curFocus = i;
                }

                // Find out the selected button's X-coordinate
                int curControlX = this.Controls[ curFocus ].Location.X;

                // Compute the next button to select
                newControl = curFocus - dimensionX;

                if ( newControl < 0 )                         
                {
                    // If this is true, then find the Element button that is in the correct column as the previously selected button
                    //   If there is no Element button in that column, select the last Element button on the window.
                    newControl = (moCount + miCount + dimensionX + newControl);
                    if (newControl > (this.Controls.Count - 1))
                        newControl = this.Controls.Count - 1;
                }

                if ( this.Controls[ newControl ].Location.X != curControlX )
                {
                    // If this check is true, then we'll need to do more work to make this work seemlessly.
                    //   Specifically, we'll need to continually check the previous button, until it has an X-location that would place it
                    //   on the next row.
                    // This code will execute when attempting to move from the Element row to the last row of MO/MI tokens

                    while ( this.Controls[ curFocus ].Location.X != 10 + ( 50 * ( newControl % dimensionX ) ) )
                        newControl++;
                    //{
                    //    if (this.Controls[curFocus].Location.X < 10 + (50 * (newControl % dimensionX)))        newControl++;
                    //    if (this.Controls[curFocus].Location.X > 10 + (50 * (newControl % dimensionX)))        newControl--;
                    //}
                }

                // Carefully activate the newly selected button.  If we, despite our best intentions, are still out of bounds, activate
                //   the last button on the window.
                try
                {
                    this.ActiveControl = this.Controls[ newControl ];
                }
                catch
                {
                    this.ActiveControl = this.Controls[ this.Controls.Count - 1 ];
                }
            }
            
        } // KeyUp event



        private void Button_Level3_Enter( object sender, EventArgs e )
        {
            // This event is bound to both "Enter" and "MouseEnter" events.
            //   Its purpose is to provide a speech prompt for what the button will do
            Button button = sender as Button;

			string saySpeech = "";                  // This will be the speech to be spoken
			string showSymbol = "";
			string typeSymbol = "token";
			
			if( button != null ) {
                // Check master token list for this button's speech text

                foreach ( Token tok in Token_Toolbar.MasterToken._mo ) {
                    if ( tok != null )
						if( tok._symbol == ( string )button.Text ) {
							saySpeech = tok._speech;
							showSymbol = tok._symbol;
						}
                }

                if ( saySpeech == "" ) {
                    foreach ( Token tok in Token_Toolbar.MasterToken._mi ) {
                        if ( tok != null )
							if( tok._symbol == ( string )button.Text ) {
								saySpeech = tok._speech;
								showSymbol = tok._symbol;
							}
                    }
                }

                if ( saySpeech == "" ) {
                    if ( ( int )button.Tag != -1 ) {
                        saySpeech = Token_Toolbar.MasterToken._mathMLelement[ ( int )button.Tag ]._speech;
						showSymbol = Token_Toolbar.MasterToken._mathMLelement[ ( int )button.Tag ]._symbol;
						typeSymbol = "element";
                    }
                }

                // And so, we should now have speech to say... flush the Speaker buffer and initialize the current sound byte
				if( WavesToolbar.MENU_Options_VoicePrompting.Checked ) {
					WavesToolbar.Speaker.SpeakAsyncCancelAll();
					WavesToolbar.Speaker.SpeakAsync( saySpeech );
				}

				string tooltipMessage = "Add " + typeSymbol + " \"" + showSymbol + "\" to formula.";
				TOOLTIP_Display.SetToolTip( button, tooltipMessage );

            }
        } // Entered Dynamic Button, either by mouse or keyboard


// *******************************************************************************************************************
//   The following functions are instrumental for suppressing the ordinary workings of the arrow keys.  I needed
//     to implement my own control-walking algorithm.
// *******************************************************************************************************************

        private void Button_Level3_PreviewKeyDown( object sender, PreviewKeyDownEventArgs e )
        {
            if ( ( e.KeyData == Keys.Up ) || ( e.KeyData == Keys.Down ) )         e.IsInputKey = true;
            if ( ( e.KeyData == Keys.Left ) || ( e.KeyData == Keys.Right ) )      e.IsInputKey = false;
        }

        private void Button_Level3_KeyDown( object sender, KeyEventArgs e )
        {
            if ( ( e.KeyData == Keys.Up ) || ( e.KeyData == Keys.Down ) ||
                 ( e.KeyData == Keys.Left ) || ( e.KeyData == Keys.Right ) )      e.SuppressKeyPress = true;
        }

        private void Level3_KeyDown( object sender, KeyEventArgs e )
        {
            if ( ( e.KeyData == Keys.Up ) || ( e.KeyData == Keys.Down ) )         e.SuppressKeyPress = true;
        }


// *******************************************************************************************************************
//   These following functions used to have a purpose, but they don't now.
// *******************************************************************************************************************

        private void BUTTON_Close_Click( object sender, EventArgs e )
        {
            // This is what happens when the invisible "Close" button is clicked.  It is linked as the Repository's
            //   "Cancel button", so it is actually "clicked" when the user presses "Escape" when the Repository is active.
            this.Close();
        } // Close Button click event


        private void Level3_KeyPress( object sender, KeyPressEventArgs e )
        {
            if ( e.KeyChar == '\b' )
            {
            }
        }

        
    }
}

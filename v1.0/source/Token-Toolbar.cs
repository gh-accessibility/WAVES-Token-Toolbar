using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Xml.XPath;                     // For XML DOM-walking functionality
using System.Xml;

using System.Text.RegularExpressions;       // To find strings within a haystack
//using XML_functions;                        // Using my .dll for XML functions
//using System.Speech.Recognition;            // Namespace for Speech Recognition
using System.Speech.Synthesis;              // Namespace for Speech Synthesis
using MathML;                               // MathML DOM and XML functionality. 
//   DLL written by Andre T. Somogyi, license under GNU public licence
using MathML.Rendering;                     // MathML Rendering control
//   DLL written by Andre T. Somogyi, license under GNU public licence
using System.Configuration;
using System.IO;                 // Required for App.Config access for MathML control




namespace Prototype_Token_Interface
{
	/// <summary>
	/// A full representation of the Token Toolbar, with all of its global data, controls (dynamic and static), and events.  The parent of all other dialog boxes.
	/// </summary>
	public partial class Token_Toolbar : System.Windows.Forms.Form
	{
		private class HighContrastProfessionalColorScheme : ProfessionalColorTable
		{
			public override Color MenuStripGradientBegin
			{ get { return Color.MidnightBlue; } }

			public override Color MenuStripGradientEnd
			{ get { return Color.Black; } }
		}

		private class NormalProfessionalColorScheme : ProfessionalColorTable
		{
			public override Color MenuStripGradientBegin
			{ get { return SystemColors.Control; } }

			public override Color MenuStripGradientEnd
			{ get { return SystemColors.ControlLightLight; } }
		}

		/// <summary>
		/// Represents the directory in which the Application began executing.
		/// </summary>
		static public string CurDir = System.IO.Path.GetDirectoryName( System.Reflection.Assembly.GetExecutingAssembly().Location );

		/// <summary>
		/// The number of controls registered on the Token Toolbar.  Helps the organization of the toolbar when dynamically creating buttons.
		/// </summary>
		public const int NUM_CONTROLS = 13;

		/// <summary>
		/// Determines the Height of the toolbar window, changing as Level 2 is opened/closed, or Menu bar is toggled visible/invisible.
		/// </summary>
		public const int WAVES_HEIGHT = 640;

		/// <summary>
		/// Determines how many tokens the toolbar will show. Choose 1 for only Level 1 tokens, or 2 for both Level 1 and 2 tokens.
		/// </summary>
		public int Level = 1;

		/// <summary>
		/// Number of Level 1 token buttons created in the Toolbar
		/// </summary>
		public int NumLevel1 = 0;

		/// <summary>
		/// Number of Level 2 token buttons created in the Toolbar
		/// </summary>
		public int NumLevel2 = 0;

		/// <summary>
		/// Number of MathML Element buttons created in the Toolbar
		/// </summary>
		public int NumElements = 0;

		/// <summary>
		/// One instance of the Token Repository may be opened.
		/// </summary>
		public Level3 TokenRepository;

		/// <summary>
		/// A running account of the MathML representation of the text currently entered in the TEXTBOX
		/// </summary>
		public string XmlText = "";

		/// <summary>
		/// Determines if a MathML excerpt is used or not. Used when loading a MathML file from disk.
		/// </summary>                                
		private bool[] idTaken = new bool[ 1000 ];

		/// <summary>
		/// A list of MathML excerpts which make up the whole formula when parsed
		/// </summary>
		private List<string> xmlPiece = new List<string>();

		/// <summary>
		/// The current XML Excerpt being edited. In all normal cases, this is the selected item in LISTBOX_ShowExcerpts.
		/// </summary>
		public int EditXmlExcerpt = 0;

		/// <summary>
		/// // This array will hold the number of arguments each MathML element tag requires
		/// </summary>
		public int[] NumTagArgs;

		/// <summary>
		/// Set to true when Space bar is pressed; this is a hack to keep a button from being clicked when the Space bar is pressed.
		/// </summary>
		public bool SpaceLock = false;

		/// <summary>
		/// Hack to keep the Listbox from firing its voice prompting if the user has just clicked "Read formula"
		/// </summary>
		public bool ReadLock = false;


		/// <summary>
		/// The namespace ID being used by the current mathML document.  This string will either be empty (no namespacing)
		/// or the ID and a trailing ":" character.  The Toolbar defaults to an empty namespace for creating mathML from scratch.
		/// When loading existing documents, it treats namespacing as transparent to the user.
		/// </summary>
		private string nameSpace = "";

		/// <summary>
		/// This is the URL that is present in the xlnms tag.  If none was present, then this variable will be blank.
		/// </summary>
		private string mathTagNameSpaceUrl = "";


		/// <summary>
		/// MathML document currently being, or to be, rendered
		/// </summary>
		MathMLDocument doc = new MathMLDocument();

		/// <summary>
		/// All spoken text, via PromptBuilders, are actually spoken via the Speaker.
		/// </summary>
		public SpeechSynthesizer Speaker = new SpeechSynthesizer();

		/// <summary>
		/// Keyboard combination descriptions for all Toolbar commands that can be initialized via keyboard
		/// </summary>
		static public KeyCombo[] KeyCommand = new KeyCombo[ 23 ];

		/// <summary>
		/// Contains all currently registered MathML tokens and elements, and all pertinent information pertaining to them.
		/// </summary>
		static public Master_Token_List MasterToken = new Master_Token_List();

		/// <summary>
		/// References all MathML elements to be placed on the Toolbar
		/// </summary>
		public List<string> ImpElement = new List<string>();
		//public string[] Imp_Element;

		/// <summary>
		/// References all MathML tokens to be placed on the Toolbar as Level 1
		/// </summary>
		public List<string> ImpLevel1 = new List<string>();

		/// <summary>
		/// References all MathML tokens to be placed on the Toolbar as Level 2
		/// </summary>
		public List<string> ImpLevel2 = new List<string>();


		/// <summary>
		/// This is a list of all functions that might be found in MI tags.  These are reserved sequences of letters which are
		/// allowed to exist within the same MI tag.  They are dynamically determined based on the MI tags registered.
		/// </summary>
		public List<string> RegisteredFunctions = new List<string>() { "sin", "cos", "tan", "cot", "sec", "csc", "ln", 
			"log", "lim", "and", "or", "xor", "not", "mod", "inv" };


		/// <summary>
		/// This is a list of all MathML strings, functioning as a queue, stored in the order the changes were made.  
		/// </summary>
		private List<string> UndoSteps = new List<string>();


		/// <summary>
		/// Makes sure that the Undo List does not update when Executing Undo
		/// </summary>
		private bool UndoLock = false;



		/// <summary>
		/// An instance of the Unicode Dictionary.
		/// </summary>
		public Unicode_Dictionary UnicodeLookup = new Unicode_Dictionary();


		/// <summary>
		/// The position of the cursor in the TextBox, when it is selected.
		/// </summary>
		private int TextBoxCursor;


		/// <summary>
		/// The Dynamic webpage that gets shown in the MathJax renderer.
		/// </summary>
		private string MathJaxWebpageShell;


		/// <summary>
		/// A temporary place to insert a token when a button is pressed
		/// </summary>
		private int TempInsertHere = -1;


		/// <summary>
		/// The URL of the temporary webpage that is viewed in the MathJax renderer.
		/// </summary>
		private string MathJaxTempPage;


		public Token_Toolbar()
		{
			InitializeComponent();
		}

		// This class may be necessary if I needed a custom rendering of MenuSeperator bars for the High Contrast scheme.
		//   The system's paint routine for these separators does not take custom colors into consideration.

		//        private class MyRenderer : ToolStripProfessionalRenderer
		//        {
		//            protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
		//            {
		//                if (!e.Vertical || (e.Item as ToolStripSeparator) == null)
		//                    base.OnRenderSeparator(e);
		//                else
		//                {
		//                    Rectangle bounds = new Rectangle(Point.Empty, e.Item.Size);
		//                    bounds.Y += 3;
		//                    bounds.Height = Math.Max(0, bounds.Height - 6);
		//                    if (bounds.Height >= 4) bounds.Inflate(0, -2);
		//                    Pen pen = new Pen(Color.DarkBlue);
		//                    int x = bounds.Width / 2;
		//                    e.Graphics.DrawLine(pen, x, bounds.Top, x, bounds.Bottom - 1);
		//                    pen.Dispose();
		//                    pen = new Pen(Color.Blue);
		//                    e.Graphics.DrawLine(pen, x + 1, bounds.Top + 1, x + 1, bounds.Bottom);
		//                    pen.Dispose();
		//                }
		//            }
		//        } 


		// *******************************************************************************************************************
		//   The following functions are instrumental for application startup.
		// *******************************************************************************************************************

		private void Token_Toolbar_Load( object sender, EventArgs e )
		{
			// Test for presence of Key Mapping XML.  If present, load all keyboard shortcuts.
			LoadKeyCombos();

			// Remember settings from last run, or create new ones
			LoadSettings();

			// Size window appropriately
			Height = WAVES_HEIGHT;
			TopMost = MENU_View_AlwaysOnTop.Checked;

			// The close button should not be visible to user; it is only "pressed" via the Escape key
			BUTTON_Close.Left = -100;

			// Create Unicode Dictionary for use in identifying Math Characters
			//CreateUnicodeDictionary();

			// Create all necessary controls and set their colors appropriately
			PrepareNewControls();

			// Start with a New slate
			ClearFormula();

			// Attempt to initialize the MathML Rendering object
			try {
				MATHML_Display.MathElement = ( MathMLMathElement )doc.DocumentElement;
			}
			catch {
				ShowError( 1 );
				Close();
				return;
				//Visible = false;
			}

			//BROWSER_MathJaxRenderer.Navigate( MathJaxTempPage );

			// Prepare the Speaker
			Speaker.Rate = 1;
			Speaker.Volume = 100;

			string hex = UnicodeLookup.GetUnicodeEntityHex( 'c' );
			string desc = UnicodeLookup.GetUnicodeDescription( 'c' );


			// Start the application on the first Level 2 token
			//ActiveControl = Controls[NUM_CONTROLS];

			//listBox2.Font = new Font( "math-roman.ttf", 12, FontStyle.Regular );
			//for( int i = 0; i < 16384; i++ ) {
			//	listBox2.Items.Add( i.ToString() + ": " + Convert.ToChar( i ) );
			//}
		} // Toolbar Load event


		/// <summary>
		/// Dynamically (re-)creates all controls prescribed by outside (xml) resources, and formats them appropriately.
		/// </summary>
		private void PrepareNewControls()
		{
			// This function's job is to create all controls prescribed by outside (xml) resources,
			//   and then to format them appropriately (high contrast or no).

			// Load Master Token XML into memory.  Quit program execution if it is not present.
			if( MasterToken.ParseMasterDOM() == -1 ) {
				ShowError( 3 );
				Close();
				return;
			}


			// This retrieves all important tokens and elements for placement on the Toolbar
			if( !ParseElevatedTokenDOM() ) {
				Close();
				return;
			}

			// Populate the toolbar with Level 1 and Level 2 tokens
			int tabStop = 0;
			PopulateElements( ref tabStop );
			PopulateToolbar( 1, ref tabStop );
			PopulateToolbar( 2, ref tabStop );

			// Reposition elements and resize window if necessary
			RepositionAllControls();

			//Controls[9].Width = Controls[9].Width;
			// Prepare the form's color scheme
			SetFormColors();


			// Finally, determine which <mi> tokens require multiple characters
			RegisteredFunctions = new List<string>();
			
			for (int i = 0; i < MasterToken._miCount; i++) {
				if( MasterToken._mi[ i ]._symbol.Length > 1 )
					RegisteredFunctions.Add( MasterToken._mi[ i ]._symbol );
			}
			//registeredFunctions
			//		public List<string> registeredFunctions = new List<string> { "sin", "cos", "tan", "cot", "sec", "csc", "ln", 
			//"log", "lim", "and", "or", "xor", "not", "mod", "inv" };

		}


		/// <summary>
		/// Deletes all dynamic controls in the Toolbar, to prepare for Toolbar re-initialization. Important when Master Token List has been edited.
		/// </summary>
		public void ReinitializeControls()
		{
			// Delete all dynamic controls from the form, and collect the garbage
			int deleteFrom = Controls.Count - NumLevel1 - NumLevel2 - NumElements;

			for( int i = ( Controls.Count - 1 ); i > ( deleteFrom - 1 ); i-- ) {

				Controls.RemoveAt( i );
				//Controls[i].Dispose();

			}
			// GC.Collect();


			// Reset all member variables, so they can be re-initialized
			//NUM_CONTROLS = deleteFrom;
			NumElements = 0;
			NumLevel1 = 0;
			NumLevel2 = 0;

			//MasterToken = null;
			MasterToken = new Master_Token_List();

			ImpElement = null;
			ImpLevel1 = null;
			ImpLevel2 = null;

			// Re-load Master Token List and Elevated Tokens, then reset the Toolbar
			PrepareNewControls();
		}


		private void Token_Toolbar_FormClosing( object sender, FormClosingEventArgs e )
		{
			// When the application closes, save all paths.
			Properties.Settings.Default.Save();

		}


		/// <summary>
		/// Retrieve the user's configuration settings from any previous Toolbar execution.  If this is the first execution, default settings are observed.
		/// </summary>
		private void LoadSettings()
		{
			// This configuration suppresses a warning from the MathML Renderer control during runtime
			//ConfigurationSettings.AppSettings.Set("mathml-rendering-config", ".");
			ConfigurationManager.AppSettings.Set( "mathml-rendering-config", "." );

			// Set all application paths are set, according to the user's settings.  
			// If this is the user's first time running the program, the paths will default to the path passed in as an argument.

			// Load the settings as they were saved at the end of last session
			Properties.Settings.Default.Reload();

			// Set contextual help on/off as necessary
			MENU_View_HideMenu.Checked = Properties.Settings.Default.HideMenu;
			MENU_View_AlwaysOnTop.Checked = Properties.Settings.Default.AlwaysOnTop;
			MENU_Options_HighContrast.Checked = Properties.Settings.Default.HighContrast;
			MENU_Options_VoicePrompting.Checked = Properties.Settings.Default.VoicePrompting;
			MENU_Options_EnglishExcerpts.Checked = Properties.Settings.Default.EnglishExcerpts;
			MENU_Options_ReadAsPower.Checked = Properties.Settings.Default.SuperAsExponent;
			MENU_View_MathJax.Checked = Properties.Settings.Default.UseMathJax;

			// Set Window size, based off of whether the Menu is shown or not
			if( Properties.Settings.Default.HideMenu ) {
				//WAVES_HEIGHT = 125;
				Text = "WAVES Token Toolbar -- press " + KeyCommand[ 8 ].GetKeyCombo() + " to show menu";
				MENU_bar.Visible = false;
			}
			else {
				//WAVES_HEIGHT = 150;
				Text = "WAVES Token Toolbar";
				MENU_bar.Visible = true;
			}

			// Set whether toolbar will always be on top of all other windows
			TopMost = MENU_View_AlwaysOnTop.Checked;

			BROWSER_MathJaxRenderer.Visible = MENU_View_MathJax.Checked;

			// Load the Webpage shell for the MathJax renderer.  This creates a blank page to start.
			MathJaxTempPage = "file:///" + CurDir.Replace( '\\', '/' ).Replace( " ", "%20" ) + "/current.html";
			System.IO.StreamReader reader = new StreamReader( CurDir + "\\mathjax.html" );
			MathJaxWebpageShell = reader.ReadToEnd();
			reader.Close();

			//UpdateMathJaxRenderer();
		} // Load settings


		/// <summary>
		/// Report an error in Toolbar initialization.
		/// </summary>
		/// <param name="errorCode">Determines what message to show</param>
		public static void ShowError( int errorCode )
		{
			// This function is called when a required .xml is not present.
			// The user is notified and the program terminates.

			string message = "";

			switch( errorCode ) {
				case 1:
					message = "Application failed to start.\n" +
					"These three files must be in application's directory:\n\n" +
					"font-configuration-1-math-roman.xml\n" +
					"font-configuration-2-cm-stretchy.xml\n" +
					"font-configuration-3-times-new-roman.xml";
					break;

				case 2:
					message = "Application failed to start.\n" +
					"Required configuration .xml missing:\n\n" +
					"elevated-tokens.xml";
					break;

				case 3:
					message = "Application failed to start.\n" +
					"Required configuration .xml missing:\n\n" +
					"master-token-list.xml";
					break;

				case 4:
					message = "Application failed to start.\n" +
					"Required configuration .xml missing:\n\n" +
					"key-mapping.xml";
					break;

				default:
					message = "Unexpected program error.  Termination imminent.";
					break;
			}

			MessageBox.Show( message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error );
		} // ShowError function.



		/// <summary>
		/// Loads all Keystroke combinations for various editor commands from an external XML file.
		/// </summary>
		/// <param name="filename">The XML document to parse</param>
		/// <returns>0 if successful, -1 if failed</returns>
		private int ParseKeyMappingDOM( string filename )
		{
			// First of all, we test to see if the XML file we are going to be parsing exists...
			if( System.IO.File.Exists( filename ) ) {
				// Load the XML document
				XmlDocument keyXML = new XmlDocument();
				keyXML.Load( filename );

				int id = 0;

				// Parse the document, looking for only direct children of the "/all" node.
				//   Note:  This will work so long as the direct children aren't called "control", "alt", or "key"!
				//foreach (XmlNode node in keyXML.SelectNodes("/all/*"))
				foreach( XmlNode node in keyXML.DocumentElement.ChildNodes ) {
					//                    if (node.Name.ToLower() != "control" && node.Name.ToLower() != "alt" && node.Name.ToLower() != "key")
					//                    {
					// This is a shortcut key.  Add it into our data structure.
					// id is used, and then increment after the execution of the next statement
					KeyCommand[ id++ ] = new KeyCombo( node.Name, node.ChildNodes[ 0 ].InnerText, node.ChildNodes[ 1 ].InnerText, node.ChildNodes[ 2 ].InnerText );
					//id++;
					//                    }
				}

				// Assign the shortcut keys to the menus
				AssignKeyToMenu( KeyCommand[ 0 ], MENU_Formula_New );
				AssignKeyToMenu( KeyCommand[ 1 ], MENU_Formula_ImportMathML );
				AssignKeyToMenu( KeyCommand[ 2 ], MENU_Formula_Undo );
				AssignKeyToMenu( KeyCommand[ 3 ], MENU_Formula_ClearExcerpt );
				AssignKeyToMenu( KeyCommand[ 4 ], MENU_Formula_Read );
				AssignKeyToMenu( KeyCommand[ 5 ], MENU_Formula_CopyClipboard );
				AssignKeyToMenu( KeyCommand[ 6 ], MENU_Formula_ExportMathML );
				AssignKeyToMenu( KeyCommand[ 7 ], MENU_Formula_ExportGraphic );
				AssignKeyToMenu( KeyCommand[ 8 ], MENU_Formula_Close );
				AssignKeyToMenu( KeyCommand[ 9 ], MENU_View_AlwaysOnTop );
				AssignKeyToMenu( KeyCommand[ 10 ], MENU_View_HideMenu );
				AssignKeyToMenu( KeyCommand[ 11 ], MENU_View_MathJax );
				AssignKeyToMenu( KeyCommand[ 12 ], MENU_View_Level2 );
				AssignKeyToMenu( KeyCommand[ 13 ], MENU_View_Repository );
				AssignKeyToMenu( KeyCommand[ 14 ], MENU_View_Autotune );
				AssignKeyToMenu( KeyCommand[ 15 ], MENU_Options_EnglishExcerpts );
				AssignKeyToMenu( KeyCommand[ 16 ], MENU_Options_HighContrast );
				AssignKeyToMenu( KeyCommand[ 17 ], MENU_Options_ReadAsPower );
				AssignKeyToMenu( KeyCommand[ 18 ], MENU_Options_VoicePrompting );
				AssignKeyToMenu( KeyCommand[ 19 ], MENU_Options_EditKeyboard );
				AssignKeyToMenu( KeyCommand[ 20 ], MENU_Options_EditMasterTokenList );
				AssignKeyToMenu( KeyCommand[ 21 ], MENU_Help_Index );
				AssignKeyToMenu( KeyCommand[ 22 ], MENU_Help_About );
				//                AssignKeyToMenu(KeyCommand[20], MENU_Help_Shortcuts);
				//                AssignKeyToMenu(KeyCommand[21], MENU_Help_About);

				// Alert calling function of success
				return 0;
			}
			else {
				// Show error and alert calling function of FAILURE

				Token_Toolbar.ShowError( 4 );
				return -1;
			}
		}


		/// <summary>
		/// This function should be called only once to load all keyboard shortcuts from the external .xml file (key-mapping.xml).
		/// </summary>
		private void LoadKeyCombos()
		{
			int returnCode = ParseKeyMappingDOM( CurDir + "\\key-mapping.xml" );

			// If parsing failed (most likely because file was not present), stop program execution
			if( returnCode == -1 )
				Close();
			return;
		}


		/// <summary>
		/// Assigns a keystoke combination to an existing ToolStripMenuItem, if possible.
		/// </summary>
		/// <param name="key">Keystroke combination to assign</param>
		/// <param name="menu">Menu item to which to assign the keystroke combination</param>
		public void AssignKeyToMenu( KeyCombo key, ToolStripMenuItem menu )
		{
			Keys newCombo = new Keys();
			KeysConverter kc = new KeysConverter();

			//menu.ShortcutKeys = Keys.None;
			try {
				// This command can throw an exception if mainKey is not set.  
				// This, however, is fine; it just means there is no keyboard shortcut for this item.
				newCombo = ( Keys )kc.ConvertFromString( key._mainKey );
				if( key._ctrl )
					newCombo = newCombo | Keys.Control;
				if( key._alt )
					newCombo = newCombo | Keys.Alt;

				menu.ShortcutKeys = ( Keys )newCombo;
			}
			catch //(Exception ex)
			{
				menu.ShortcutKeys = Keys.None;
				//MessageBox.Show(ex.ToString());
			}

		}

		/// <summary>
		/// Returns the least significant 8 bits of a 32-bit integer.
		/// </summary>
		/// <param name="intValue">A 4 byte integer (int)</param>
		/// <returns>A 4 byte integer in the form 0x000000xx</returns>
		public static int GetLeastSignificantBits( int intValue )
		{
			// This is useful in receiving only the mere ASCII info from the Keys.KeyCode member
			// http://www.java2s.com/Code/CSharp/Data-Types/ObtainingtheMostSignificantorLeastSignificantBitsofaNumber.htm

			return ( intValue & 0x000000FF );
		}


		/// <summary>
		/// Loads all Keystroke combinations for various editor commands from an external XML file.
		/// </summary>
		/// <returns>"true" if successful, "false" for failure</returns>
		public bool ParseElevatedTokenDOM()
		{
			// Declare needed variables to walk the DOM only once.  CatNum is a device to determine where we are in the
			//   DOM.
			int catNum = 0;

			// Re-new the lists, to be re-populated.  This is because when re-initialized, these lists are set to null.
			ImpElement = new List<string>();
			ImpLevel1 = new List<string>();
			ImpLevel2 = new List<string>();
			
			string filename = Token_Toolbar.CurDir + "\\elevated-tokens.xml";

			// First of all, we test to see if the XML file we are going to be parsing exists...
			if( System.IO.File.Exists( filename ) ) {
				// Load the XML document
				XmlDocument tokenXML = new XmlDocument();
				tokenXML.Load( filename );

				foreach( XmlNode tokCategory in tokenXML.DocumentElement.ChildNodes ) {
					foreach( XmlNode sym in tokCategory.ChildNodes ) {
						switch( catNum ) {
							case 0:
								// Elements
								ImpElement.Add( sym.InnerText );
								break;

							case 1:
								// Level 1 tokens
								ImpLevel1.Add( sym.InnerText );
								break;

							case 2:
								// Level 2 tokens
								ImpLevel2.Add( sym.InnerText );
								break;

							default:
								break;
						}
					}

					// After parsing this whole category, let's parse the next one differently.
					catNum++;
				}

				NumElements = ImpElement.Count();
				NumLevel1 = ImpLevel1.Count();
				NumLevel2 = ImpLevel2.Count();

				// Alert calling function of success
				return true;
			}
			else {
				// Can't do anything if this xml doesn't exist.  If the file is not found, returns -1 for FAILURE 
				ShowError( 2 );
				return false;
			}

		} // close function: ParseElevatedTokenDOM


		/// <summary>
		/// Dynamically creates a button on the toolbar for every MathML element specified in the elevated-tokens.xml file.
		/// </summary>
		/// <param name="tabStop">The TabIndex property of the first element button.</param>
		private void PopulateElements( ref int tabStop )
		{
			// Edit to modify button positions
			const int START_X = 20;
			const int START_Y = WAVES_HEIGHT - 125;

			const int BUTTON_WIDTH = 40;
			const int BUTTON_HEIGHT = 40;

			const int BUTTON_SPACING_X = 50;
			//const int sizeSpacingY = 50;


			for( int i = 0; i < NumElements; i++ ) {
				// For each elevated token, create a button on the toolbar which is captioned by the token
				Button button = new Button();

				button.Font = new System.Drawing.Font( "Lucida Sans Unicode", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ) );
				button.Name = ImpElement[ i ];

				string iconPath = GetBasePath() + "\\graphics\\elem-" + ImpElement[ i ] + ".ico";
				if( File.Exists( iconPath ) ) {
					// Given a pictoral representation of this element exists, use it as the button's face
					button.Image = Image.FromFile( iconPath );
					button.ImageAlign = ContentAlignment.MiddleCenter;
				}
				else {
					// If no pictoral representation for this element exists, we have to settle for text
					button.Text = ImpElement[ i ];
				}

				// Placement information for the new button
				int locX = START_X + ( BUTTON_SPACING_X * i );
				int locY = START_Y;
				if( MENU_bar.Visible == false )
					locY -= MENU_bar.Height;
				button.Location = new Point( locX, locY );

				button.Width = BUTTON_WIDTH;
				button.Height = BUTTON_HEIGHT;

				// Event information for the new button.  It takes effect when clicked.
				button.Click += new EventHandler( Button_Element_Click );
				button.Enter += new EventHandler( Button_Dynamic_Enter );
				button.MouseEnter += new EventHandler( Button_Dynamic_Enter );
				button.PreviewKeyDown += new PreviewKeyDownEventHandler( Button_Dynamic_PreviewKeyDown );

				button.Tag = -1;
				for( int j = 0; j < MasterToken._elemCount; j++ ) {
					if( MasterToken._mathMLelement[ j ]._symbol == button.Name )
						button.Tag = j;
				}

				// Assign a Tab Index to this button
				button.TabIndex = tabStop;
				tabStop++;
				button.TabStop = true;

				// Add the button to the form
				Controls.Add( button );
//				NUM_CONTROLS++;

				// In case this is the last token, make sure the toolbar window is wide enough to show all buttons
				//dimensionX = locX + 150;
			}

		}

		// PopulateElements function


		/// <summary>
		/// Dynamically creates a button on the toolbar for every token in the specified Level in the elevated-tokens.xml file.
		/// </summary>
		/// <param name="lv">1 if placing first Level tokens, 2 if placing second Level tokens</param>
		/// <param name="tabStop">The TabIndex property of the first token button in the specified Level.</param>
		private void PopulateToolbar( int lv, ref int tabStop )
		{
			// Edit to modify button positions
			const int START_X = 20;

			const int BUTTON_WIDTH = 40;
			const int BUTTON_HEIGHT = 30;

			const int BUTTON_SPACING_X = 50;
			const int BUTTON_SPACING_Y = 40;

			const int START_Y = WAVES_HEIGHT - 112;	

			int loopMax = 0;
			if( lv == 1 ) 
				loopMax = NumLevel1;
			else
				loopMax = NumLevel2;

			
			for( int i = 0; i < loopMax; i++ ) {
				// For each elevated token, create a button on the toolbar which is captioned by the token
				Button button = new Button();

				//button.Font = new System.Drawing.Font( "DejaVu Sans Condensed", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ) );
				//ElevatedToken t = output[i] as ElevatedToken;
				string tok;
				if( lv == 1 )
					tok = ImpLevel1[ i ];
				else
					tok = ImpLevel2[ i ];

				if( tok.Length == 1 )
					button.Font = new System.Drawing.Font( "Lucida Sans Unicode", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ) );
				else
					button.Font = new System.Drawing.Font( "Lucida Sans Unicode", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( ( byte )( 0 ) ) );

				button.Text = tok;

				// Placement information for the new button
				int locX = START_X + ( BUTTON_SPACING_X * i );
				int locY = START_Y + ( BUTTON_SPACING_Y * lv );
				button.Location = new Point( locX, locY );

				button.Width = BUTTON_WIDTH;
				button.Height = BUTTON_HEIGHT;
				if( lv > Level )
					button.Visible = false;

				// Event information for the new button.  It takes effect when clicked.
				button.Click += new EventHandler( Button_Token_Click );
				button.Enter += new EventHandler( Button_Dynamic_Enter );
				button.MouseEnter += new EventHandler( Button_Dynamic_Enter );
				button.PreviewKeyDown += new PreviewKeyDownEventHandler( Button_Dynamic_PreviewKeyDown );
				button.Tag = i;

				// Assign a Tab Index to this button
				button.TabIndex = tabStop;
				tabStop++;

				// Add the button to the form
				Controls.Add( button );

				// In case this is the last token, make sure the toolbar window is wide enough to show all buttons
				//dimensionX = locX + 150;
			}

		} // PopulateToolbar function


		// *******************************************************************************************************************
		//   The following functions are concerned with the Toolbar's presentation, the look of the GUI.
		// *******************************************************************************************************************


		/// <summary>
		/// Reposition all Controls... based on whether Level 2 is shown, and whether the menu is visible
		/// </summary>
		private void RepositionAllControls()
		{
			// Constants involved with Window size
			const int FULL_WIDTH = 360;
			
			// Edit to modify button presentation
			const int BUTTON_SPACING_X = 50;
			const int BUTTON_SPACING_Y = 40;
			const int LEVEL_DELIMITING_SPACING_Y = 10;

			const int BUTTON_START_X = 20;
			const int BUTTON_START_Y = 320;

			//const int buttonWidth = 40;
			//const int buttonHeight = 30;

	
			// Resize to default
			Size = new Size( FULL_WIDTH, WAVES_HEIGHT );

			if( MENU_bar.Visible ) {
				LISTBOX_ShowExcerpts.Location = new Point( 7, 4 + MENU_bar.Height );
				LISTBOX_ShowExcerpts.Size = new Size( FULL_WIDTH - 20, WAVES_HEIGHT - MENU_bar.Height - 360 );
			}
			else {
				LISTBOX_ShowExcerpts.Location = new Point( 7, 4 );
				LISTBOX_ShowExcerpts.Size = new Size( FULL_WIDTH - 20, WAVES_HEIGHT - MENU_bar.Height - 336 );
			}

			LABEL_Wrapper.Location = new Point( -2, LISTBOX_ShowExcerpts.Location.Y + LISTBOX_ShowExcerpts.Height + 9);
			TEXTBOX_MathExpression.Location = new Point( TEXTBOX_MathExpression.Location.X, LISTBOX_ShowExcerpts.Location.Y + LISTBOX_ShowExcerpts.Height + 4 );

			BUTTON_ShowLevel2.Visible = false;
			BUTTON_ShowLevel3.Visible = false;


			BROWSER_MathJaxRenderer.Location = new Point( 10, WAVES_HEIGHT - 153 );
			BROWSER_MathJaxRenderer.Size = new Size( FULL_WIDTH - 26, WAVES_HEIGHT - BROWSER_MathJaxRenderer.Location.Y - MENU_bar.Height - 12 );

			MATHML_Display.Location = new Point( 7, WAVES_HEIGHT - 155 );
			MATHML_Display.Size = new Size( FULL_WIDTH - 20, WAVES_HEIGHT - MATHML_Display.Location.Y - MENU_bar.Height - 10 );

			int warnLocX = Convert.ToInt32( ( FULL_WIDTH - LABEL_Warning.Size.Width ) / 2 );
			int warnLocY = MATHML_Display.Location.Y + Convert.ToInt32( MATHML_Display.Size.Height / 2 );
			LABEL_Warning.Location = new Point( warnLocX, warnLocY );


			int numDynamicControls = NumElements + NumLevel1 + NumLevel2;
			int firstDynamicControl = NUM_CONTROLS;

			int tokID = 0;

			for( tokID = 0; tokID < NumElements; tokID++ ) {
				int curID = firstDynamicControl + tokID;
				if (Controls.Count > curID) 
					Controls[ curID ].Location = new Point( BUTTON_START_X + ( BUTTON_SPACING_X * ( tokID % 5 ) ), BUTTON_START_Y );
			}

			for( tokID = 0; tokID < NumLevel1; tokID++ ) {
				int curID = firstDynamicControl + NumElements + tokID;
				if( Controls.Count > curID )
					Controls[ curID ].Location = new Point( BUTTON_START_X + ( BUTTON_SPACING_X * ( tokID % 5 ) ), BUTTON_START_Y + BUTTON_SPACING_Y + LEVEL_DELIMITING_SPACING_Y );
			}

			for( tokID = 0; tokID < NumLevel2; tokID++ ) {
				int curID = firstDynamicControl + NumElements + NumLevel1 + tokID;
				if( Controls.Count > curID )
					Controls[ curID ].Location = new Point( BUTTON_START_X + ( BUTTON_SPACING_X * ( tokID % 5 ) ), BUTTON_START_Y + LEVEL_DELIMITING_SPACING_Y + ( BUTTON_SPACING_Y * ( 2 + Convert.ToInt32( tokID / 5 ) ) ) );
			}

		} // Reposition all Controls... based on whether Level 2 is shown, and whether the menu is visible


		/// <summary>
		/// This function will set the colors of controls on this form based off of whether the user wants the high contrast color option, or not.
		/// </summary>
		private void SetFormColors()
		{
			Color menuBack = Color.Black;
			Color menuFore = Color.White;

			if( MENU_Options_HighContrast.Checked ) {
				// Apply High Contrast color scheme
				BackColor = Color.Black;
				ForeColor = Color.DodgerBlue;

				//MENU_bar.BackColor = Color.FromArgb(64, 64, 64);
//				MENU_bar.BackColor = Color.Black;
//				MENU_bar.ForeColor = Color.LightBlue;
				MENU_bar.Renderer = new ToolStripProfessionalRenderer( new HighContrastProfessionalColorScheme() );

				LABEL_Wrapper.ForeColor = Color.Azure;

				TEXTBOX_MathExpression.BackColor = Color.White;
				TEXTBOX_MathExpression.ForeColor = Color.Black;

				LISTBOX_ShowExcerpts.BackColor = Color.DarkGreen;
				LISTBOX_ShowExcerpts.ForeColor = Color.LightGoldenrodYellow;

				LABEL_Warning.BackColor = Color.White;
				MATHML_Display.BackColor = Color.White;

				MATHML_Display.ForeColor = Color.Black;

				for( int i = NUM_CONTROLS - NumElements; i < Controls.Count; i++ ) {
					Controls[ i ].BackColor = Color.Azure;
					Controls[ i ].ForeColor = Color.Black;
				}

				menuBack = Color.MidnightBlue;
				menuFore = Color.Azure;
			}
			else {
				// Apply normal color scheme


				BackColor = SystemColors.Control;
				ForeColor = SystemColors.ControlText;

				//MENU_bar.BackColor = SystemColors.ControlLightLight;
				//MENU_bar.ForeColor = SystemColors.ControlText;
				MENU_bar.Renderer = new ToolStripProfessionalRenderer( new NormalProfessionalColorScheme() );

				TEXTBOX_MathExpression.BackColor = SystemColors.ControlLightLight;
				TEXTBOX_MathExpression.ForeColor = SystemColors.ControlText;

				LABEL_Wrapper.ForeColor = SystemColors.ControlText;

				LISTBOX_ShowExcerpts.BackColor = Color.FromArgb( 224, 224, 255 );
				LISTBOX_ShowExcerpts.ForeColor = Color.Black;

				LABEL_Warning.BackColor = SystemColors.Control;
				MATHML_Display.BackColor = SystemColors.Control;
				MATHML_Display.ForeColor = Color.Black;



				for( int i = NUM_CONTROLS - NumElements; i < Controls.Count; i++ ) {
					Controls[ i ].BackColor = SystemColors.Control;
					Controls[ i ].ForeColor = SystemColors.ControlText;
				}

				menuBack = SystemColors.ControlLightLight;
				menuFore = SystemColors.ControlText;
			}

			return;
			//MENU_Formula.BackColor = menuBack;

//			MENU_Formula_ClearExcerpt.BackColor = menuBack;
//			MENU_Formula_Close.BackColor = menuBack;
//			MENU_Formula_CopyClipboard.BackColor = menuBack;
//			MENU_Formula_ExportGraphic.BackColor = menuBack;
//			MENU_Formula_ExportMathML.BackColor = menuBack;
//			MENU_Formula_ImportMathML.BackColor = menuBack;
//			MENU_Formula_New.BackColor = menuBack;
//			MENU_Formula_Read.BackColor = menuBack;
			//MENU_Help.BackColor = menuBack;
//			MENU_Help_About.BackColor = menuBack;
//			MENU_Help_Index.BackColor = menuBack;
//			MENU_Help_Shortcuts.BackColor = menuBack;
			//MENU_Options.BackColor = menuBack;
//			MENU_Options_ReadAsPower.BackColor = menuBack;
//			MENU_Options_EditKeyboard.BackColor = menuBack;
//			MENU_Options_EditMasterTokenList.BackColor = menuBack;
//			MENU_Options_EnglishExcerpts.BackColor = menuBack;
//			MENU_Options_HighContrast.BackColor = menuBack;
//			MENU_Options_VoicePrompting.BackColor = menuBack;
			//MENU_View.BackColor = menuBack;
//			MENU_View_Autotune.BackColor = menuBack;
//			MENU_View_Repository.BackColor = menuBack;
//			MENU_View_HideMenu.BackColor = menuBack;
//			MENU_View_AlwaysOnTop.BackColor = menuBack;
//			MENU_View_Level2.BackColor = menuBack;
			//toolStripSeparator1.BackColor = menuBack;
			//toolStripSeparator2.BackColor = menuBack;
			//toolStripSeparator3.BackColor = menuBack;
			//toolStripSeparator4.BackColor = menuBack;
			//toolStripSeparator5.BackColor = menuBack;
			//toolStripSeparator6.BackColor = menuBack;

//			MENU_Formula.ForeColor = menuFore;
//			MENU_Formula_ClearExcerpt.ForeColor = menuFore;
//			MENU_Formula_Close.ForeColor = menuFore;
//			MENU_Formula_CopyClipboard.ForeColor = menuFore;
//			MENU_Formula_ExportGraphic.ForeColor = menuFore;
//			MENU_Formula_ExportMathML.ForeColor = menuFore;
//			MENU_Formula_ImportMathML.ForeColor = menuFore;
//			MENU_Formula_New.ForeColor = menuFore;
//			MENU_Formula_Read.ForeColor = menuFore;
//			MENU_Help.ForeColor = menuFore;
//			MENU_Help_About.ForeColor = menuFore;
//			MENU_Help_Index.ForeColor = menuFore;
//			MENU_Help_Shortcuts.ForeColor = menuFore;
//			MENU_Options.ForeColor = menuFore;
//			MENU_Options_ReadAsPower.ForeColor = menuFore;
//			MENU_Options_EditKeyboard.ForeColor = menuFore;
//			MENU_Options_EditMasterTokenList.ForeColor = menuFore;
//			MENU_Options_EnglishExcerpts.ForeColor = menuFore;
//			MENU_Options_HighContrast.ForeColor = menuFore;
//			MENU_Options_VoicePrompting.ForeColor = menuFore;
//			MENU_View.ForeColor = menuFore;
//			MENU_View_Autotune.ForeColor = menuFore;
//			MENU_View_Repository.ForeColor = menuFore;
//			MENU_View_HideMenu.ForeColor = menuFore;
//			MENU_View_AlwaysOnTop.ForeColor = menuFore;
//			MENU_View_Level2.ForeColor = menuFore;
			//toolStripSeparator1.ForeColor = menuFore;
			//toolStripSeparator2.ForeColor = menuFore;
			//toolStripSeparator3.ForeColor = menuFore;
			//toolStripSeparator4.ForeColor = menuFore;
			//toolStripSeparator5.ForeColor = menuFore;
			//toolStripSeparator6.ForeColor = menuFore;

			// This code will reset the MATHML rendering control... too bad it takes this to do it!
//			try {
//				doc = null;
//				doc = new MathMLDocument();
//				doc.InnerXml = XmlText;
//			}
//			catch { }

			

			// Set up the ListBox for adding stuff again
//			int temp = LISTBOX_ShowExcerpts.SelectedIndex;
//			LISTBOX_ShowExcerpts.SelectedIndex = -1;
//			LISTBOX_ShowExcerpts.SelectedIndex = temp;
			//LISTBOX_ShowExcerpts.Items.Add(TagifyID(0) + "-<" + GetExcerptDescription(xmlPiece[0], 0) + ">  " + DeXml(xmlPiece[0]));
			//TEXTBOX_MathExpression.Text = "";
		} // Set Form Colors. This changes the colors of the GUI, based on the user's selection of "high contrast".



		// *******************************************************************************************************************
		//   The following functions ensure that the top-Level menus are still readable when they are opened.
		// *******************************************************************************************************************

		private void MENU_OnMenuBar_DropDownClosed( object sender, EventArgs e )
		{
			MENU_Closing( ( ToolStripMenuItem )sender );
		}

		private void MENU_OnMenuBar_DropDownOpening( object sender, EventArgs e )
		{
			MENU_Opening( ( ToolStripMenuItem )sender );
		}

		/// <summary>
		/// Sets the Font Color of an opening Main menu item to black (so it can be read while highlighted).
		/// </summary>
		/// <param name="tsmi">Menu item that is opening.</param>
		private void MENU_Opening( ToolStripMenuItem tsmi )
		{
			tsmi.ForeColor = Color.Black;
		}

		/// <summary>
		/// Resets the Font Color of an closing Main menu item to its color scheme default.
		/// </summary>
		/// <param name="tsmi">Menu item that is closing.</param>
		private void MENU_Closing( ToolStripMenuItem tsmi )
		{
			tsmi.ForeColor = Color.FromArgb( ( byte )~tsmi.BackColor.R, ( byte )~tsmi.BackColor.G, ( byte )~tsmi.BackColor.B );
		}



		// *******************************************************************************************************************
		//   The following functions receive input from the user via keyboard key presses.
		// *******************************************************************************************************************

		private void Token_Toolbar_KeyPress( object sender, KeyPressEventArgs e )
		{
			// This event determines what to do when a key has been pressed.  Mostly, this has to do with typing.
			//   Therefore, we are only concerned with alpha-numeric characters, certain keyboard math tokens,
			//   the space bar, and the backspace key (\b).
			//   This effects the value in the textbox.
			//listBox1.Items.Add( "TT-KeyPress" );
			if( ( e.KeyChar < 27 ) & ( e.KeyChar != 8 ) )
				return;

			string s = e.KeyChar.ToString();
			
			// The Space bar is used in Windows Forms to activate the current control.  Since this is not
			//   desired behavior, a "lock" has been implemented to stop 
			if( s == " " )
				SpaceLock = true;


			if( this == sender ) {
				string xmlWrapper = GetXmlWrapper( xmlPiece[ EditXmlExcerpt ] );
				string t = TEXTBOX_MathExpression.Text;
				
				int insertHere = t.Length;
				if( TEXTBOX_MathExpression.ContainsFocus )
					insertHere = TEXTBOX_MathExpression.SelectionStart;


				// add any letter or number
				//if (Char.IsLetterOrDigit(s, 0))  t = t + e.KeyChar;

				switch( s ) {
					case "@":
					case "#":
					case "^":
					case "&":
					case "_":
					case "`":
					case "~":
					case "\'":
					case "\"":
					case ";":
					case "\\":
						if( xmlWrapper == "mtext" )
							t = t.Insert( insertHere, e.KeyChar.ToString() );
						insertHere++;
						break;

					case "/":
						t = t.Insert( insertHere, "÷" );
						insertHere++;
						break;

					case "*":
						t = t.Insert( insertHere, "×" );
						insertHere++;
						break;

					case "\b":
						if( insertHere > 0 ) {
							bool isTag = false;
							int checkChar = insertHere - 1;
							while( ( !isTag ) & ( checkChar > -1 ) & ( checkChar > insertHere - 6 ) ) {
								if( t.Substring( checkChar, 1 ) == "⊰" )
									isTag = true;
								else
									checkChar--;
							}

//							if( t.Substring( t.Length - 1 ) == "⊱" ) {
							if( isTag ) {
								// If we are here, we are attempting to delete a tag (via backspace key). 
								// This can only be done if we're in an <mrow> element, because other elements 
								//   like <mfrac> and <root> can fail if they don't have the correct number of arguments.
								// Thus, if the current MathML excerpt is non-mrow, reject the backspace.

								if( xmlWrapper == "mrow" ) {
									t = t.Substring( 0, checkChar ) + t.Substring( checkChar + 5 );
									insertHere = checkChar;
									//t = t.Substring( 0, t.Length - 5 );
								}
							}
							else {
								// Otherwise, just delete the regular character

								if( insertHere > 0 ) {
									t = t.Substring( 0, insertHere - 1 ) + t.Substring( insertHere );
									insertHere--;
								}

								//t = t.Substring( 0, ( t.Length - 1 ) );
							}
						}

						break;

					default:
						t = t.Insert( insertHere, e.KeyChar.ToString() );
						insertHere++;
						break;
				}

				TextBoxCursor = insertHere;
				TEXTBOX_MathExpression.Text = t;
			}
		} // KeyPress event


		private void Token_Toolbar_KeyUp( object sender, KeyEventArgs e )
		{
			// This event determines what to do when a key has been released.  Because of the argument difference between
			//   the KeyPress event and this KeyUp event, the KeyUp event is actually the more versatile of the two events.
			//   This function used to deal with key-combos and keycodes that are important to the overall working of the
			//   toolbar.

			// It is now a shadow of what it once was.  It only drives the selection of excerpts, via up/down arrow presses.

			// The Down key selects the next excerpt in the List
			if( e.KeyData == Keys.Down ) {
				if( LISTBOX_ShowExcerpts.SelectedIndex < ( LISTBOX_ShowExcerpts.Items.Count - 1 ) )
					LISTBOX_ShowExcerpts.SelectedIndex += 1;
			}

			// The Up Arrow key selects the last excerpt in the List, and activates the List
			if( e.KeyData == Keys.Up ) {
				if( LISTBOX_ShowExcerpts.SelectedIndex > 0 )
					LISTBOX_ShowExcerpts.SelectedIndex -= 1;
			}


			// Test the particular keyboard shortcut that fired this event, and 
			//   test the keyboard state against the shortcuts that fire dynamic button clicks
			KeyCombo keysPressed = new KeyCombo( e.Control, e.Alt, GetLeastSignificantBits( ( int )e.KeyData ).ToString() );

			Button button = GetButtonFromShortcut( keysPressed );

			if( button != null ) {
				if( !TEXTBOX_MathExpression.Focused )
					TEXTBOX_MathExpression.SelectionStart = TEXTBOX_MathExpression.Text.Length;
				button.PerformClick();
			}
			//showShortcut.GetWavesTokenShortcut( this, button, false );
		} // KeyUp event


		/// <summary>
		/// This returns the Button to activate, if necessary, from a Shortcut.
		/// </summary>
		/// <param name="keys">The keystroke combination being tried</param>
		/// <returns>The Button to click, or null if the shortcut didn't click any button</returns>
		private Button GetButtonFromShortcut( KeyCombo keys )
		{
			// As written, there are twenty possible shortcuts to fire dynamic buttons:
			//   Ctrl + [ 1 through 5 ]			→	Add MathML Element 1-5
			//	 Ctrl + [ 6 through 0 ]			→	Add Level 1 Token 1-5
			//	 Ctrl + Alt + [ 1 through 0]	→	And Level 2 Token 1-10
			int butIndex = 0;
			int buttonNum = Convert.ToInt32( keys._mainKey );

			if ( ( buttonNum < 48 ) | ( buttonNum > 57 ) )
				return null;

			buttonNum -= 48;
			if( buttonNum == 0 )
				buttonNum = 10;
			buttonNum--;

			if ( keys._ctrl == true ) {
				if( keys._alt == true ) {
					// This will be a level 2 token
					butIndex = NUM_CONTROLS + ( NumElements + NumLevel1 ) + buttonNum;
				}
				else {
					if( buttonNum < 5 ) {
						// This is an element
						butIndex = NUM_CONTROLS + buttonNum;
						if( butIndex >= ( NUM_CONTROLS + NumElements ) )
							butIndex = 0;
					}
					else {
						// This is a level 1 token
						butIndex = NUM_CONTROLS + buttonNum + NumElements - 5;
						if( butIndex >= ( NUM_CONTROLS + NumElements + NumLevel1 ) )
							butIndex = 0;
					}
				}
			}

			if( butIndex == 0 )
				return null;

			if( butIndex < Controls.Count )
				return ( Button )Controls[ butIndex ];
			else
				return null;
		}


		// *******************************************************************************************************************
		//   The following events ensure uniform interaction with the GUI when arrows are pressed.  Essentially, Up/Down
		//     should select a different excerpt in the list; Left/Right should select a new Token/Element
		// *******************************************************************************************************************

		private void Token_Toolbar_KeyDown( object sender, KeyEventArgs e )
		{
			// Make sure that the Up/Down arrows do not advance the activated control...
			//   they are reserved to navigate the formula's listbox
			e.SuppressKeyPress = false;
			if( ( e.KeyData == Keys.Up ) || ( e.KeyData == Keys.Down ) )
				e.SuppressKeyPress = true;

			// If the Menu is hidden, unhide it when user pressed the Alt key!
			if( ( e.Modifiers == Keys.Alt ) && ( !MENU_bar.Visible ) )
				MENU_View_HideMenu.PerformClick();

			return;
		}

		private void Token_Toolbar_PreviewKeyDown( object sender, PreviewKeyDownEventArgs e )
		{
			e.IsInputKey = false;
			if( ( e.KeyData == Keys.Up ) || ( e.KeyData == Keys.Down ) )
				e.IsInputKey = true;

			return;
		}


		private void LISTBOX_ShowExcerpts_PreviewKeyDown( object sender, PreviewKeyDownEventArgs e )
		{
			// If the Listbox is active, The Right/Left arrows should not navigate the list...
			//   what should be done is to select the appropriate dynamic button
			if( ( e.KeyData == Keys.Left ) || ( e.KeyData == Keys.Right ) ) {
				e.IsInputKey = true;

				SelectNewDynamicButton( e.KeyData );
			}
			return;
		}




		private void LISTBOX_ShowExcerpts_KeyDown( object sender, KeyEventArgs e )
		{
			// If the Listbox is active, The Right/Left arrows should not navigate the list
			if( ( e.KeyData == Keys.Left ) || ( e.KeyData == Keys.Right ) ) {
				e.SuppressKeyPress = true;
			}
			return;
		}

		void Button_Dynamic_PreviewKeyDown( object sender, PreviewKeyDownEventArgs e )
		{
			if( ( e.KeyData == Keys.Up ) || ( e.KeyData == Keys.Down ) )
				e.IsInputKey = true;
		}

		private void MATHML_Display_PreviewKeyDown( object sender, PreviewKeyDownEventArgs e )
		{
			if( ( e.KeyData == Keys.Left ) || ( e.KeyData == Keys.Right ) ) {
				SelectNewDynamicButton( e.KeyData );
			}
		}


		/// <summary>
		/// Determines the next button to select, based on whether the Left or Right arrow was pressed.
		/// Right selects the next button (or the first button if at the end).  Left selects the previous button, (or the last if at the beginning).
		/// </summary>
		/// <param name="kd">Determines whether the Left or Right arrow was pressed.</param>
		void SelectNewDynamicButton( Keys kd )
		{
			if( kd == Keys.Right )
				Controls[ NUM_CONTROLS ].Select();
			else if( kd == Keys.Left ) {
				if( Level == 1 )
					Controls[ Controls.Count - NumLevel2 - 1 ].Select();
				else
					Controls[ Controls.Count - 1 ].Select();
			}
		}

		/// <summary>
		/// This sets the descriptive text on the Multi-Function button, based on whether there is a working formula present or not.
		/// </summary>
		private void UpdateMultiFunctionButton()
		{
			if( XmlText == "<math><mrow></mrow></math>" ) {
				BUTTON_MultiFunction.Text = "Paste MathML from Clipboard";
				BUTTON_MultiFunction.Enabled = true;
			}
			else {
				BUTTON_MultiFunction.Text = "Copy MathML to Clipboard";
				BUTTON_MultiFunction.Enabled = true;
			}
		}

		/// <summary>
		/// If there's a formula to be copied, copy it; otherwise get a formula from the clipboard
		/// </summary>
		/// <param name="sender">This is the Multi-Function button</param>
		/// <param name="e">Info pertinent to Button Click events</param>
		private void BUTTON_MultiFunction_Click( object sender, EventArgs e )
		{
			if( BUTTON_MultiFunction.Text == "Copy MathML to Clipboard" ) {
				// Copy formula to Clipboard.
				string cutifyXML = CutifyMathML( XmlText );
				
				if( cutifyXML != "" ) {
					MessageBox.Show( "XML copied to Clipboard:\n\n" + cutifyXML,
					  Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information );
					Clipboard.SetText( cutifyXML );
				}

				// Additionally, to make this work like an accountant's adding machine, this will also reset the formula

				ClearFormula();
			}
			else {
				// Otherwise, we need to determine if there is a mathml excerpt already copied to the clipboard.
				if( Clipboard.ContainsText() ) {
					string textToParse = Clipboard.GetText();
					if( textToParse.Length > 4 ) {
						
						// We will assume that what we are receiving is a MathML formula.  This will be ascertained as we attempt
						//	 to construct a formula from it.  As such, parse it in as a formula.
						ConstructFormulaFromMathmlString( textToParse );
						UndoAddStep();
						//if( textToParse.Substring( 0, 5 ) == "<math" ) {
						//}
						//else {
						//	MessageBox.Show( "There is text on the Clipboard, but it is not in proper MathML format.", "WAVES-Token-Toolbar", MessageBoxButtons.OK, MessageBoxIcon.Exclamation );
						//}
					}
					else {
						MessageBox.Show( "There is not enough text on the Clipboard for this to be MathML.", "WAVES-Token-Toolbar", MessageBoxButtons.OK, MessageBoxIcon.Exclamation );
					}
				}
				else {
					MessageBox.Show( "There is nothing text-related on the Clipboard, much less any MathML island.", "WAVES-Token-Toolbar", MessageBoxButtons.OK, MessageBoxIcon.Exclamation );
				}
			}
		} 




		// *******************************************************************************************************************
		//   The following functions fire when a Menu item is clicked or selected via keyboard shortcut
		// *******************************************************************************************************************

		private void MENU_Formula_New_Click( object sender, EventArgs e )
		{
			// Clears the entire MathML Formula (all excerpts).
			ClearFormula();
		} // New (Clear) Formula Menu click event

		private void MENU_Formula_ImportMathML_Click( object sender, EventArgs e )
		{
			// This event handler is used when either the Menu or Keyboard equivalent of "Import MathML" is clicked.
			// It displays a Load Dialog window which prompts the user to choose a MathML file.

			string basePath = GetBasePath();
			string path = GetFilePathFromDialog( basePath + "\\savedata\\mathml\\", true, 0 );

			if( path != null ) {
				ClearFormula();

				// Why don't I just use doc.Load, and a DOM walk?
				string s;
				using( System.IO.StreamReader file = new System.IO.StreamReader( @path, true ) ) {
					s = file.ReadToEnd();
				}

				ConstructFormulaFromMathmlString( s );
				UndoAddStep();
				//MessageBox.Show("MathML retrieved from file:\n\n" + path,
				//  Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);

			}

		}

		private void MENU_Formula_Undo_Click( object sender, EventArgs e )
		{
			UndoExecute();
		}


		private void MENU_Formula_ClearExcerpt_Click( object sender, EventArgs e )
		{
			// Clears the current MathML excerpt.  
			TEXTBOX_MathExpression.Text = "";
			//XmlText = "";

		} // Clear Text Menu click event

		private void MENU_Formula_Read_Click( object sender, EventArgs e )
		{
			// Reads the whole MathML formula to the user.
			ReadLock = true;
			ReadXmlFormula();
		} // Read Formula Menu click event

		private void MENU_Formula_CopyClipboard_Click( object sender, EventArgs e )
		{
			// This menu copies the MathML representation of the displayed formula to the Clipboard.
			//   Additionally, it tells the user what got copied.
			string cutifyXML = CutifyMathML( XmlText );

			if( cutifyXML != "" ) {
				MessageBox.Show( "XML copied to Clipboard:\n\n" + cutifyXML,
				  Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information );
				Clipboard.SetText( cutifyXML );
			}

		} // Copy Text as XML Menu click event


		private void MENU_Formula_ExportMathML_Click( object sender, EventArgs e )
		{
			// This event handler is used when either the Menu or Keyboard equivalent of "Save MathML" is clicked.
			// It displays a Save Dialog window which prompts the user to choose a location and filename for the MathML.

			string basePath = GetBasePath();
			string path = GetSavePathFromDialog( basePath + "\\savedata\\mathml\\", false, 0 );

			if( path != null ) {
				// This is necessary to make sure that we aren't appending data to the file!
				try {
					System.IO.File.Delete( path );
				}
				catch { }

				string cutifyXML = CutifyMathML( XmlText );

				if( cutifyXML != "" ) {
					using( System.IO.StreamWriter file = new System.IO.StreamWriter( @path, true ) ) {
						file.WriteLine( cutifyXML );
					}
					//Clipboard.SetText(cutifyXML);

					MessageBox.Show( "MathML saved to file:\n\n" + path,
					  Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information );
				}
			}

		}

		private void MENU_Formula_ExportGraphic_Click( object sender, EventArgs e )
		{
			// The user can save the chart anywhere they wish, give it any name they wish, and save it in any one
			//   of five common graphical formats: PNG (default), BMP, GIF, JPG, or TIF.

			// Show the "Browse File" dialog box to allow user to rename file if they so desire.
			string basePath = GetBasePath();
			string path = GetSavePathFromDialog( basePath + "\\savedata\\graphic\\", false, 1 );

			if( path != null ) {
				// If we are here, we successfully clicked "Save" (not "Cancel".


				// Determine the graphical format for our graph by querying the save file extension 
				System.Drawing.Imaging.ImageFormat saveAs = System.Drawing.Imaging.ImageFormat.Png;
				string saveExt = ParseFileExt( path );

				// String.Compare returns "< 0" for less than, "> 0" for greater than, or 0 for equals.  Yeah.
				if( String.Compare( ( saveExt ), "gif", true ) == 0 ) { saveAs = System.Drawing.Imaging.ImageFormat.Gif; }
				if( String.Compare( ( saveExt ), "jpg", true ) == 0 ) { saveAs = System.Drawing.Imaging.ImageFormat.Jpeg; }
				if( String.Compare( ( saveExt ), "bmp", true ) == 0 ) { saveAs = System.Drawing.Imaging.ImageFormat.Bmp; }
				if( String.Compare( ( saveExt ), "tif", true ) == 0 ) { saveAs = System.Drawing.Imaging.ImageFormat.Tiff; }
				if( String.Compare( ( saveExt ), "tiff", true ) == 0 ) { saveAs = System.Drawing.Imaging.ImageFormat.Tiff; }

				// Save the visible graph (the graph on the visible TABCONTROL page.
				MATHML_Display.Save( path, saveAs );
			}
		}

		private void MENU_Formula_Close_Click( object sender, EventArgs e )
		{
			// This is what happens when the "Close" menu item is clicked.             
			Close();
		} // Close Window click event


		private void MENU_View_AlwaysOnTop_Click( object sender, EventArgs e )
		{
			// Toggles whether the WAVES Toolbar and all children dialog boxes are "Top Level" windows.  
			//   Top Level windows cannot be placed behind other normal windows.
			MENU_View_AlwaysOnTop.Checked = !MENU_View_AlwaysOnTop.Checked;
			TopMost = MENU_View_AlwaysOnTop.Checked;
		}


		private void MENU_View_HideMenu_Click( object sender, EventArgs e )
		{
			// This toggles the menu bar open or closed.  If closed, it can only be opened by a keystroke combination.
			//   Hopefully, this keystroke combination exists! This combo will be noted in the application title bar.
			MENU_View_HideMenu.Checked = !MENU_View_HideMenu.Checked;
			MENU_bar.Visible = !MENU_View_HideMenu.Checked;

			if( MENU_bar.Visible ) {
				//WAVES_HEIGHT = 150;
				Text = "WAVES Token Toolbar";
				Properties.Settings.Default.HideMenu = false;
			}
			else {
				//WAVES_HEIGHT = 125;
				Text = "WAVES Token Toolbar -- press " + KeyCommand[ 10 ].GetKeyCombo() + " to show menu";
				Properties.Settings.Default.HideMenu = true;
			}

			RepositionAllControls();
		}


		private void MENU_View_MathJax_Click( object sender, EventArgs e )
		{
			MENU_View_MathJax.Checked = ( !MENU_View_MathJax.Checked );
			Properties.Settings.Default.UseMathJax = MENU_View_MathJax.Checked;

			UpdateMathJaxRenderer();
			BROWSER_MathJaxRenderer.Visible = MENU_View_MathJax.Checked;
		}


		private void MENU_View_Level2_Click( object sender, EventArgs e )
		{
			MENU_View_Level2.Checked = ( !MENU_View_Level2.Checked );
			
			// Toggles open/closed the Level 2 tokens (on the bottom of the window)
			ToggleLevel2();
		} // Show Level 2 Menu or Button click event

		private void MENU_View_Repository_Click( object sender, EventArgs e )
		{
			// Toggles open/closed the Token Repository which contains all <mo> and <mi> tokens in
			//   the master list.
			ToggleLevel3();

			// Disable the main WAVES Toolbar, so as to not interfere with the Shortcut Editor while it is open.
			//   Once the Master Token Editor is closed, the WAVES Toolbar will be re-enabled.
			Enabled = false;
		} // Show Token Repository Menu or Button click event


		private void MENU_View_Autotune_Click( object sender, EventArgs e )
		{
			//
		}


		private void MENU_Options_EnglishExcerpts_Click( object sender, EventArgs e )
		{
			// This option toggles whether the WAVES interface will show MathML information, or English-based equivalents.
			MENU_Options_EnglishExcerpts.Checked = !MENU_Options_EnglishExcerpts.Checked;
			Properties.Settings.Default.EnglishExcerpts = MENU_Options_EnglishExcerpts.Checked;

			// Will want to rebuild the Listbox to show the changes to the user
			for( int i = 0; i < LISTBOX_ShowExcerpts.Items.Count; i++ ) {
				LISTBOX_ShowExcerpts.Items[ i ] = TagifyID( i ) + "-<" + GetExcerptDescription( xmlPiece[ i ], i ) + ">  " + RetrieveExcerptFromListbox( LISTBOX_ShowExcerpts.Items[ i ].ToString() );
			}
		}

		private void MENU_Options_HighContrast_Click( object sender, EventArgs e )
		{
			// Toggle High-Contrast color scheme.
			MENU_Options_HighContrast.Checked = !MENU_Options_HighContrast.Checked;
			Properties.Settings.Default.HighContrast = MENU_Options_HighContrast.Checked;
			SetFormColors();
		}

		private void MENU_Options_ReadAsPower_Click( object sender, EventArgs e )
		{
			// Toggle whether MathML <msup> elements are read as Superscripts, or as math powers (base to the exponent).
			MENU_Options_ReadAsPower.Checked = !MENU_Options_ReadAsPower.Checked;
			Properties.Settings.Default.SuperAsExponent = MENU_Options_ReadAsPower.Checked;
		}


		private void MENU_Options_VoicePrompting_Click( object sender, EventArgs e )
		{
			// Toggle Voice Prompting
			MENU_Options_VoicePrompting.Checked = !MENU_Options_VoicePrompting.Checked;
			Properties.Settings.Default.VoicePrompting = MENU_Options_VoicePrompting.Checked;
		}

		private void MENU_Options_EditKeyboard_Click( object sender, EventArgs e )
		{
			// This option shows the Keyboard Shortcut Editor
			Edit_Keys EK = new Edit_Keys();
			EK.Owner = ( System.Windows.Forms.Form )this;
			EK.Location = new Point( MousePosition.X, MousePosition.Y );
			EK.Show();

			// Disable the main WAVES Toolbar, so as to not interfere with the Shortcut Editor while it is open.
			//   Once the Master Token Editor is closed, the WAVES Toolbar will be re-enabled.
			Enabled = false;
		}

		private void MENU_Options_EditMasterTokenList_Click( object sender, EventArgs e )
		{
			// This option shows the Master Token Editor.
			// First, make sure that the Token Repository isn't open (the editor could changed data important to the Repository)
			try {
				if( TokenRepository.Visible == true ) {
					MessageBox.Show( "The Token Repository is open.\nPlease close it and try again.", "WAVES-Token-Toolbar", MessageBoxButtons.OK, MessageBoxIcon.Exclamation );
					return;
				}
			}
			catch { }

			// Shows the Master Tokens Editor
			Edit_Tokens ET = new Edit_Tokens();
			ET.Owner = ( System.Windows.Forms.Form )this;
			ET.Location = new Point( MousePosition.X, MousePosition.Y );
			ET.Show();

			// Disable the main WAVES Toolbar, so as to not interfere with the Master Token Editor while it is open.
			//   Once the Master Token Editor is closed, the WAVES Toolbar will be re-enabled.
			Enabled = false;
		}

		//        private void MENU_Options_EditImportantTokens_Click(object sender, EventArgs e)
		//        {
		// This function is never called... this is a disabled menu-item
		//            ReinitializeControls();
		//        }

		private void MENU_Help_Index_Click( object sender, EventArgs e )
		{
			// This will show the Help window
			System.Diagnostics.Process.Start( "WAVES-Toolbar.chm" );
		}


		private void MENU_Help_About_Click( object sender, EventArgs e )
		{
			// This will show an About Box
			About_WAVES aboutBox = new About_WAVES();
			aboutBox.ShowDialog();
		}



		// *******************************************************************************************************************
		//   The following functions help drive the file dialog windows
		// *******************************************************************************************************************

		/// <summary>
		/// Opens a Load File dialog box to retrieve a MathML file to load. Returns the name of the file that will be opened.
		/// </summary>
		/// <param name="initPath">Directory to display when dialog box opens</param>
		/// <param name="mustExist">Whether the file dialog checks if a specified file must exist</param>
		/// <param name="fileType">Not used</param>
		/// <returns>Name of file to open</returns>
		private string GetFilePathFromDialog( string initPath, bool mustExist, int fileType )
		{
			// Returns a string that represents the full path and name of a file.  
			// The File dialog box is given an initial directory, and may or may not ensure that a selected file exists
			//   prior to allowing an answer.
			// If operation is cancelled, the directory is set to NULL.

			string result = null;

			DIALOG_Load.InitialDirectory = initPath;
			DIALOG_Load.CheckFileExists = mustExist;

			if( DIALOG_Load.ShowDialog() == DialogResult.OK ) {
				result = DIALOG_Load.FileName;
			}
			return result;
		}

		//        private string getPathFromDialog()
		//        {
		// This function returns a string received from the Directory Dialog box.
		// If operation is cancelled, the directory is set to NULL.
		//   This will do bad things if the user attempts to run stuff.

		//            if (browseDataPath.ShowDialog() == DialogResult.OK)
		//            {
		//                return browseDataPath.SelectedPath;
		//            }
		//            else { return null; }
		//        }


		/// <summary>
		/// Opens a Save File dialog box to retrieve a MathML file to load. Returns the name of the file that will be opened.
		/// </summary>
		/// <param name="initPath">Directory to display when dialog box opens</param>
		/// <param name="mustExist">Whether the file dialog checks if a specified file must exist</param>
		/// <param name="filetype">0 if saving as MathML, 1 if saving a picture</param>
		/// <returns>Name of file to open</returns>
		private string GetSavePathFromDialog( string initPath, bool mustExist, int filetype )
		{
			// This function opens a Save File dialog box.
			// If operation is cancelled, the directory is set to NULL.

			string result = null;

			DIALOG_Save.InitialDirectory = initPath;                 // Set initial directory. Should be the Results directory.
			DIALOG_Save.CheckFileExists = mustExist;                 // Whether we must save over an existing file. Default is no.
			DIALOG_Save.FileName = "";                               // Filename has path info stripped off.
			DIALOG_Save.DefaultExt = ".mathml";                      // mathml is only save format.

			// Prompt user to save the file.  
			switch( filetype ) {
				case 0:
					// User wanted to save the MathML
					DIALOG_Save.Title = "Save MathML as...";
					DIALOG_Save.FileName = "";

					DIALOG_Save.Filter = "MATHML- textual MathML markup (*.mathml)|*.mathml|Text file (*.txt)|*.txt|eXtensible Markup Language file (*.xml)|*.xml";
					DIALOG_Save.FilterIndex = 3;
					break;

				case 1:
					// User wanted to save the graphic
					DIALOG_Save.Title = "Save MathML Graphic representation as...";
					DIALOG_Save.FileName = "";

					DIALOG_Save.Filter = "BMP- Windows or OS/2 Bitmap (*.bmp)|*.bmp|GIF- Graphic Interchange Format (*.gif)|*.gif|JPG- JPeg (*.jpg)|*.jpg|PNG- Portable Network Graphic (*.png)|*.png|TIF- Tagged Image File Format (*.tif)|*.tif";
					DIALOG_Save.FilterIndex = 4;

					break;

				default:
					break;
			}

			// These properties set the Save As portion of the dialog.  The Filter formats the dropdown list.
			DIALOG_Save.RestoreDirectory = true;

			// If the dialog box does not get cancelled... return the filename to which to save, otherwise return NULL
			if( DIALOG_Save.ShowDialog() == DialogResult.OK ) {
				result = DIALOG_Save.FileName;
			}
			return result;
		}



		// *******************************************************************************************************************
		//   The following functions receive input from the user, in the form of mouse clicks.
		// *******************************************************************************************************************

		private void BUTTON_Close_Click( object sender, EventArgs e )
		{
			// This is what happens when the invisible "Close" button is clicked.  It is linked as the Toolbar's
			//   "Cancel button", so it is actually "clicked" when the user presses "Escape" when the Toolbar is active.
			Close();
		}  // Invisible Close Button click event



		/// <summary>
		/// This event is raised when a dynamic token button is clicked on the toolbar.  The caption of the button is appended to the textbox's "expression".
		/// </summary>
		/// <param name="sender">The object (castable to "Button") that raised the event</param>
		/// <param name="e">Event data pertinent to Button clicks</param>
		void Button_Token_Click( object sender, EventArgs e )
		{
			//   These dynamic buttons were created from the Level 1/2 elevated tokens .xml.
			//   The caption of the button is appended to the textbox's "expression".

			// When a button is clicked via space bar press, don't do anything!
			if( IsSpaceLocked() )
				return;

			Button button = sender as Button;
			if( button != null ) {
				int id = ( int )button.Tag;

				string t = TEXTBOX_MathExpression.Text;
				int insertHere = t.Length;

				if( TEXTBOX_MathExpression.Focused )
					insertHere = TEXTBOX_MathExpression.SelectionStart;
				else if( TempInsertHere > -1 )
					insertHere = TempInsertHere;

				if( insertHere > t.Length )
					insertHere = t.Length;

				t = t.Insert( insertHere, ( string )button.Text );
				insertHere++;

				TempInsertHere++;

				TEXTBOX_MathExpression.Text = t;
//				TEXTBOX_MathExpression.Text += ( string )button.Text;
			}
		} // Dynamic Token Button click event


		/// <summary>
		/// This event is raised when a dynamic MathML element button is clicked on the toolbar.  This adds a complex MathML element to the formula.
		/// </summary>
		/// <param name="sender">The object (castable to "Button") that raised the event</param>
		/// <param name="e">Event data pertinent to Button clicks</param>
		private void Button_Element_Click( object sender, EventArgs e )
		{
			// When a button is clicked via space bar press, don't do anything!
			if( IsSpaceLocked() )
				return;

			UndoLock = true;

			// Cannot add any MathML tags to an <mtext> element, but all other tags allow nesting
			if( LABEL_Wrapper.Text.Substring( LABEL_Wrapper.Text.Length - 5, 5 ) == "mtext" )
				return;
			if( LABEL_Wrapper.Text.Substring( LABEL_Wrapper.Text.Length - 7, 7 ) == "literal" )
				return;


			Button b = ( Button )sender;

			string tag = b.Name;
			int args = MasterToken._mathMLelement[ ( int )b.Tag ]._args;

			int jumpHere = AddTagToFormula( tag, args, EditXmlExcerpt, true );
			EditXmlExcerpt = jumpHere;

			UndoLock = false;
			LISTBOX_ShowExcerpts.SelectedIndex = jumpHere;
		} // Dynamic Element Button click event


		/// <summary>
		/// Checks whether the Space Bar has been locked.  Once checked, the lock is released.  Used to trap the space bar from firing effective button clicks.
		/// </summary>
		/// <returns>"True" if the space bar has been trapped, "False" otherwise</returns>
		private bool IsSpaceLocked()
		{
			bool answer = SpaceLock;
			SpaceLock = false;
			return answer;
		}


		// *******************************************************************************************************************
		//   The following functions provide the user with speech prompting when a control is entered.
		//       Most controls may be entered by [TAB] or by arrow key, or by entering it's location via mouse.
		// *******************************************************************************************************************

		/// <summary>
		/// Bind to both "Enter" and "MouseEnter" events for dynamic token/element buttons to allow voice prompting.
		/// </summary>
		/// <param name="sender">The object (castable to "Button") that raised the event</param>
		/// <param name="e">Event data pertinent to Enter and MouseEnter events</param>
		private void Button_Dynamic_Enter( object sender, EventArgs e )
		{
			// This event is bound to both "Enter" and "MouseEnter" events.
			//   Its purpose is to provide a speech prompt and visible prompt for what the dynamic button represents

			Button button = sender as Button;
		
			string saySpeech = "";                  // This will be the speech to be spoken
			string showSymbol = "";
			//string typeSymbol = "token";

			KeyCombo showShortcut = new KeyCombo( true, false, "" );
							
			if( button != null ) {
				// Check master token list for this button's speech text

				// First, check <mo> tags
				foreach( Token tok in MasterToken._mo ) {
					if( tok != null )
						if( tok._symbol == ( string )button.Text ) {
							saySpeech = tok._speech;
							showSymbol = tok._symbol;

							showShortcut.GetWavesTokenShortcut( this, button, false );
						}
				}

				// Then, check <mi> tags if not yet found
				if( saySpeech == "" ) {
					foreach( Token tok in MasterToken._mi ) {
						if( tok != null )
							if( tok._symbol == ( string )button.Text ) {
								saySpeech = tok._speech;
								showSymbol = tok._symbol;

								showShortcut.GetWavesTokenShortcut( this, button, false );
							}
					}
				}

				// Then, check element tags if not yet found
				if( saySpeech == "" ) {
					foreach( Token tok in MasterToken._mathMLelement ) {
						if( tok._symbol == ( string )button.Name ) {
							saySpeech = tok._speech;
							showSymbol = tok._symbol;
							//typeSymbol = "element";

							showShortcut.GetWavesTokenShortcut( this, button, true );
						}
					}
				}

				// And so, we should now have speech to say... flush the Speaker buffer and initialize the current sound byte
				if( MENU_Options_VoicePrompting.Checked ) {
					Speaker.SpeakAsyncCancelAll();
					Speaker.SpeakAsync( saySpeech );
				}

				string tooltipMessage = "Add \"" + showSymbol + "\" to formula (" + showShortcut.GetKeyCombo() + ")";
				TOOLTIP_Display.SetToolTip( button, tooltipMessage );
			}

			// Log the control on the form that has been entered
			//for( int i = 0; i < Controls.Count; i++ ) {
			//	if( Controls[ i ] == button )
			//		DynamicSelected = i;
			//}

		} // Entered Dynamic Button, either by mouse or keyboard



		private void BUTTON_MultiFunction_Enter( object sender, EventArgs e )
		{
			Speaker.SpeakAsyncCancelAll();

			string tooltipMessage = "";
			if( BUTTON_MultiFunction.Text.Substring( 0, 4 ) == "Copy" ) {
				Speaker.SpeakAsync( "Copy formula to clipboard and clear formula" );
				tooltipMessage = "Copy formula to the clipboard as MathML, and clear the formula.";
			}
			else {
				Speaker.SpeakAsync( "Paste formula from clipboard." );
				tooltipMessage = "Paste a MathML formula from the clipboard if possible.";
			}

			TOOLTIP_Display.SetToolTip( BUTTON_MultiFunction, tooltipMessage );
		} // Entered Multifunction Button, either by mouse or keyboard



		private void Button_Static_Enter( object sender, EventArgs e )
		{
			// This event is bound to all static buttons on the form (those not created at runtime).
			//   Its purpose is to speak the button's function.

			if( MENU_Options_VoicePrompting.Checked ) {
				Button button = sender as Button;
				StopSpeech();
				Speaker.SpeakAsync( ( string )button.Tag );         // The tag is designed to describe the button's function
			}
		} // Entered Static Button, either by mouse or keyboard 


		private void MENU_SpeakIdentity( object sender, EventArgs e )
		{
			// This event is bound to all static menu items on the form (those not created at runtime).
			//   Its purpose is to speak the menu item's function.

			// Unfortunately, this event does not fire if menu items was entered by keyboard navigation!
			ToolStripMenuItem menu = sender as ToolStripMenuItem;
			//StopSpeech();
			//Speaker.SpeakAsync((string)menu.Tag);         // The tag is designed to describe the menu item's function

		} // Entered a menu item... tell user what it is




		// *******************************************************************************************************************
		//   The following event functions allow the user to edit the MathML formula
		// *******************************************************************************************************************


		private void LISTBOX_ShowExcerpts_MeasureItem( object sender, MeasureItemEventArgs e )
		{
			ListBox lb = (ListBox)sender;

			e.ItemHeight = ( (int)lb.Font.SizeInPoints * 2 ) + 4;
		}


		private void LISTBOX_PaintListItems( object sender, DrawItemEventArgs e )
		{
			// If the item state is selected them change the back color 
			//   Courtesy of http://stackoverflow.com/questions/3663704/how-to-change-listbox-selection-background-color
			if( ( e.State & DrawItemState.Selected ) == DrawItemState.Selected )
				if( MENU_Options_HighContrast.Checked )
					e = new DrawItemEventArgs( e.Graphics, e.Font, e.Bounds, e.Index, e.State ^ DrawItemState.Selected, e.ForeColor, Color.Navy );
				else
					e = new DrawItemEventArgs( e.Graphics, e.Font, e.Bounds, e.Index, e.State ^ DrawItemState.Selected, e.ForeColor, Color.Azure );

			// This function paints all list items to the ListBox
			e.DrawBackground();
			e.DrawFocusRectangle();

			if( e.Index > -1 ) {
				string drawItem = LISTBOX_ShowExcerpts.Items[ e.Index ].ToString();
				Color curColor = LISTBOX_ShowExcerpts.ForeColor;
				Font curFont = new Font( LISTBOX_ShowExcerpts.Font, FontStyle.Regular );
				int drawY = e.Bounds.Top + 2;
				int drawX = e.Bounds.Left;

				for( int i = 0; i < drawItem.Length; i++ ) {
					// Draw each individual character
					// "⊰{0,-3:000}⊱"

					if( drawItem[ i ] == '⊰' ) {
						curColor = Color.Gray;
						curFont = new Font( LISTBOX_ShowExcerpts.Font, FontStyle.Regular );
						drawX += 3;
					}

					if( ( drawItem[ i ] == '<' ) & ( drawItem.IndexOf( '<', 0 ) == i ) ) {
						if( MENU_Options_HighContrast.Checked )
							curColor = Color.Yellow;
						else
							curColor = Color.DarkGreen;
						curFont = new Font( LISTBOX_ShowExcerpts.Font, FontStyle.Regular );
					}

					string desc = UnicodeLookup.GetUnicodeDescription( drawItem[ i ] );
					if( ( !desc.Contains( "INVISIBLE" ) ) & ( desc != "FUNCTION APPLICATION" ) & ( !desc.Contains( "<control>" ) ) ) {
						e.Graphics.DrawString( drawItem[ i ].ToString(), curFont, new SolidBrush( curColor ), drawX, drawY );
						SizeF sizeOfChar = e.Graphics.MeasureString( drawItem[ i ].ToString(), curFont );
						if( curFont.Style == FontStyle.Bold ) 
							drawX = drawX + ( ( int )sizeOfChar.Width - 3 );
						else
							drawX = drawX + ( ( int )sizeOfChar.Width - 5 );

					}

					if( ( ( drawItem[ i ] == '>' ) & ( drawItem.IndexOf( '>', 0 ) == i ) ) |
						( drawItem[ i ] == '⊱' ) ) {
						if( MENU_Options_HighContrast.Checked )
							curColor = Color.Azure;
						else
							curColor = Color.MidnightBlue;
						curFont = new Font( LISTBOX_ShowExcerpts.Font, FontStyle.Bold );
						drawX += 3;
					}

					if( ( drawItem[ i ] == '>' ) & ( drawItem.IndexOf( '>', 0 ) == i ) )
						drawX = 150;

				}
			}

			//e.Graphics.DrawString( drawItem,
			//					  new Font( LISTBOX_ShowExcerpts.Font, FontStyle.Regular ),
			//					  new SolidBrush( LISTBOX_ShowExcerpts.ForeColor ),
			//					  e.Bounds );
 
		}


		private void LISTBOX_XMLpieces_SelectedIndexChanged( object sender, EventArgs e )
		{
			// When the Listbox's selected excerpt changes, we must prepare to edit another excerpt.
			if( LISTBOX_ShowExcerpts.SelectedIndex > -1 ) {
				EditXmlExcerpt = LISTBOX_ShowExcerpts.SelectedIndex;
			}
			else {
				// Do not do anything if there is nothing selected (ie SelectedIndex = -1)
				return;
			}

			// The Textbox receives only the MathML excerpt, without wrapper
			TEXTBOX_MathExpression.Text = RetrieveExcerptFromListbox( LISTBOX_ShowExcerpts.Items[ EditXmlExcerpt ].ToString() );

			// The label to the left of the Textbox receives the ID and the Wrapper
			LABEL_Wrapper.Text = TagifyID( EditXmlExcerpt ) + " " + GetExcerptDescription( xmlPiece[ EditXmlExcerpt ], EditXmlExcerpt );

			// Tell the user that a different excerpt has been selected.
			if( MENU_Options_VoicePrompting.Checked ) {
				string s = LISTBOX_ShowExcerpts.SelectedItem.ToString();

				string excerpt = RetrieveExcerptFromListbox( s );
				int excerptNum = Convert.ToInt32( s.Substring( 1, 3 ) );

				PromptBuilder p = new PromptBuilder();
				p.AppendText( "Line " + excerptNum.ToString() + " selected. " );
				p.AppendBreak( new TimeSpan( 0, 0, 0, 0, 100 ) );
				StopSpeech();
				Speaker.SpeakAsync( p );

				PromptBuilder p2 = new PromptBuilder();
				p2 = ReadText( excerptNum, false );
				Speaker.SpeakAsync( p2 );
			}
			//LISTBOX_XMLpieces_Select(sender, e);
		} // LISTBOX_XMLpieces_SelectedIndexChanged



		private void TEXTBOX_MathExpression_Leave( object sender, EventArgs e )
		{
			TempInsertHere = TEXTBOX_MathExpression.SelectionStart;
		}


		private void TEXTBOX_MathExpression_MouseDown( object sender, MouseEventArgs e )
		{
			// This code ensures that the user cannot insert a token/element within:
			//   1) A line tag
			//   2) Any row that requires a certain number of arguments ( Args > 0 )
			TextBox tb = ( TextBox )sender;
			TestTextBoxPosition( tb );

			// The Textbox must never become activated
			//Controls[ DynamicSelected ].Select();
		}


		private void TEXTBOX_MathExpression_MouseMove( object sender, MouseEventArgs e )
		{
			// This code disables Text selection within the TextBox.  It also maintains that the cursor doesn't get misplaced.
			TextBox tb = ( TextBox )sender;
			TestTextBoxPosition( tb );
			
			tb.SelectionLength = 0;

		}


		/// <summary>
		/// This function ensures that the cursor within the TextBox does not get placed where squiffy things could happen.
		/// </summary>
		/// <param name="tb">The textbox to process</param>
		private void TestTextBoxPosition( TextBox tb )
		{
			// Make sure cursor is not within an element tag
			for( int pos = tb.SelectionStart - 4; ( ( pos < ( tb.SelectionStart + 1 ) ) & ( pos < tb.Text.Length ) ); pos++ ) {
				if( pos > -1 ) {
					if( tb.Text[ pos ] == '⊰' ) {
						// Move the cursor
						tb.SelectionStart = pos;
						break;
					}
				}
			}

			for( int pos = tb.SelectionStart; ( ( pos < ( tb.SelectionStart + 5 ) ) & ( pos < tb.Text.Length ) ); pos++ ) {
				if( tb.Text[ pos ] == '⊱' ) {
					// Move the cursor
					tb.SelectionStart = pos - 4;
					break;
				}
			}

			// Make sure user cannot insert data within a tag that requires arguments ( not mrow, mtext, mfenced, msrow ).
			string tag = GetXmlWrapper( xmlPiece[ EditXmlExcerpt ] );
			if( ( tag != "mrow" ) & ( tag != "mtext" ) & ( tag != "mfenced" ) & ( tag != "msrow" ) )
				tb.SelectionStart = tb.Text.Length;

		}


		private void TEXTBOX_MathExpression_TextChanged( object sender, EventArgs e )
		{
			// When the Textbox changes, the current MathML excerpt is updated to reflect the change.
			string t = TEXTBOX_MathExpression.Text;
			int ind = EditXmlExcerpt;

			// The Listbox must be updated to reflect the change to the formula
			LISTBOX_ShowExcerpts.Items[ ind ] = TagifyID( ind ) + "-<" + GetExcerptDescription( xmlPiece[ ind ], ind ) + ">  " + t;

			// Also, update the XML piece List.
			xmlPiece[ ind ] = TextToXML( t );

			// XmlText contains the entire MathML formula.  It must be parsed together, starting at the
			//   initial element (<math>).
			XmlText = ParseMathList( 0 );
			XmlText = "<math>" + XmlText + "</math>";

			UndoAddStep();

			// Attempt to render the formula in the MathML Rendering control.  This can fail, if some
			//   tags don't have the correct number of arguments.  
			UpdateMathmlRenderer();
//			try {
				// Reset Rendering control
//				MATHML_Display.ResetText();

				// Re-instantiate MathMLDocument, if an error in rendering happens, it must be
				//   re-instantiated in order to be rendered.
//				doc = null;
//				doc = new MathMLDocument();
//				string xmlNoInvisPlus = XmlText.Replace( "⁤", "⁢" );
//				doc.InnerXml = xmlNoInvisPlus;

				// Show the formula
//				MATHML_Display.MathElement = ( MathMLMathElement )doc.DocumentElement;

				// If we got here, everything went smoothly.  The warning is not needed.
//				LABEL_Warning.Visible = false;
//			}
//			catch {
				// The Rendering control could not render the formula properly.  There is a problem in
				//   the formula somewhere; alert the user.
//				LABEL_Warning.Visible = true;
//			}

			UpdateMultiFunctionButton();

			if( TEXTBOX_MathExpression.ContainsFocus ) {
				TEXTBOX_MathExpression.SelectionStart = TextBoxCursor;
				TEXTBOX_MathExpression.SelectionLength = 0;
			}
			else
				TextBoxCursor = TEXTBOX_MathExpression.Text.Length;

			// Test to see if <mrow>, <mfrac>, etc. tags are removed.  
			//   This is not a needed operation anymore.

			//            int numExcerpts = LISTBOX_XMLpieces.Items.Count;
			//            int keepList = 0;

			//            for (int i = 1; i < numExcerpts; i++)
			//            {
			//                for (int j = 0; j < i; j++)
			//                {
			//                    string tagNum = TagifyID(i);

			//                    if (xmlPiece[j].Contains(tagNum)) keepList = i;
			//                }
			//if (keepList < i) ; //break;
			//            }

			//RepopulateListBox(keepList + 1);

		} // TEXTBOX_MathExpression_TextChanged


		private void LABEL_Wrapper_Resize( object sender, EventArgs e )
		{
			// Resize Textbox as label lengthens or shortens
			TEXTBOX_MathExpression.Visible = false;
			TEXTBOX_MathExpression.Location = new Point( LABEL_Wrapper.Width - 2, TEXTBOX_MathExpression.Location.Y );
			TEXTBOX_MathExpression.Width = Width - 10 - LABEL_Wrapper.Width;
			TEXTBOX_MathExpression.Visible = true;
		} // Label (showing Prompt for excerpt) change event




		// *******************************************************************************************************************
		//   The following functions are instrumental in working with MathML excerpts, excerptIDs, MathML tags
		//     for display and editing purposes.  They're really quite handy.
		// *******************************************************************************************************************


		/// <summary>
		/// Attempts to update the renderer with the working MathML string.  If this fails, a warning label appears indicating
		///   a problem within the MathML.
		/// </summary>
		private void UpdateMathmlRenderer()
		{
			// Attempt to render the formula in the MathML Rendering control.  This can fail, if some
			//   tags don't have the correct number of arguments.  
			try {
				// Reset Rendering control
				MATHML_Display.ResetText();

				if( XmlText.IndexOf( "<math" ) == -1 )
					XmlText = "<math>" + XmlText + "</math>";

				// Re-instantiate MathMLDocument, if an error in rendering happens, it must be
				//   re-instantiated in order to be rendered.
				doc = null;
				doc = new MathMLDocument();
				string xmlNoInvisPlus = XmlText.Replace( "⁤", "⁢" );
				doc.InnerXml = xmlNoInvisPlus;

				// Show the formula
				MATHML_Display.MathElement = ( MathMLMathElement )doc.DocumentElement;

				// If we got here, everything went smoothly.  The warning is not needed.
				LABEL_Warning.Visible = false;
			}
			catch {
				// The Rendering control could not render the formula properly.  There is a problem in
				//   the formula somewhere; alert the user.
				LABEL_Warning.Visible = true;
			}

			if( MENU_View_MathJax.Checked ) {
				// Update the MathJax Renderer if necessary
				//BROWSER_MathJaxRenderer
				UpdateMathJaxRenderer();
			}
		}


		/// <summary>
		/// This function recreates the temp webpage that exists in the executable's directory, and resets the MathJax Renderer to use the updated file
		/// </summary>
		private void UpdateMathJaxRenderer()
		{
			string newFileText = MathJaxWebpageShell;
			string cuteXml = CutifyMathML( XmlText );

			if( mathTagNameSpaceUrl == "" ) {
				if ( nameSpace.Length > 0 )
					newFileText = newFileText.Replace( "html xmlns:m=", "html xmlns:" + ( nameSpace.Substring( 0, nameSpace.Length - 1 ) ) + "=" );
			}
			//string nameSpace =
			if( nameSpace == "" )
				cuteXml = cuteXml.Replace( "math xmlns", "math display='block' xmlns" );
			else
				cuteXml = cuteXml.Replace( ":math", ":math display='block'" );

			newFileText = newFileText.Replace( "<!-- Math content goes here -->", cuteXml );

			System.IO.StreamWriter writer = new StreamWriter( CurDir + "\\current.html" );
			writer.WriteLine( newFileText );
			writer.Close();
			//file:///C:/Work/Subversion/Grants/Math%20Input%20Prototype/current/WAVES-Token-Interface/bin/Debug/current.html
			//"file:///C:/Work/Subversion/Grants/Math%20Input%20Prototype/current/WAVES-Token-Interface/bin/Debug/current.html"
			//System.Uri uri = new System.Uri( MathJaxTempPage );
			//BROWSER_MathJaxRenderer.Url = uri;
			//BROWSER_MathJaxRenderer.Url = new System.Uri( "file:///C:/Work/Subversion/Grants/Math%20Input%20Prototype/current/WAVES-Token-Interface/bin/Debug/current.html" );

			Uri targetPage = null;
			if( Uri.TryCreate( MathJaxTempPage, UriKind.RelativeOrAbsolute, out targetPage ) == true ) {

				BROWSER_MathJaxRenderer.Navigate( targetPage );
			}

		}

		/// <summary>
		/// "Tagifies" an integer.  Returns a five character string, the ID tag: ⊰###⊱
		/// </summary>
		/// <param name="i">A 32-bit integer. Expects only values between 0 and 999; Bad Things will occur otherwise.</param>
		/// <returns>A five character ID string: ⊰###⊱</returns>
		private string TagifyID( int i )
		{
			return string.Format( "⊰{0,-3:000}⊱", i );
		} // TagifyID


		/// <summary>
		/// Retrieves only the MathML excerpt portion of the ListBox's currently selected item (removes the ID Tag and element prompt).
		/// </summary>
		/// <param name="s">A string, representing one Item in the ListBox "LISTBOX_ShowExcerpts".</param>
		/// <returns>The MathML excerpt represented by the ListBox's currently selected item.</returns>
		private string RetrieveExcerptFromListbox( string s )
		{
			// This function retrieves only the MathML excerpt portion of the ListBox's currently 
			//   selected item.  The item to be chopped is received as a string argument.
			return s.Substring( s.IndexOf( ">  " ) + 3 );
		} // RetrieveExcerptFromListbox


		/// <summary>
		/// Clears the working MathML formula and sets up the Toolbar workspace again.  
		/// </summary>
		private void ClearFormula()
		{
			// This check is necessary, since when the workspace is initialized for the first time,
			//   the Listbox has no items to select!
			if( LISTBOX_ShowExcerpts.Items.Count > 0 )
				LISTBOX_ShowExcerpts.SelectedIndex = 0;

			// Clear the Lists
			UndoLock = true;
			LISTBOX_ShowExcerpts.Items.Clear();
			xmlPiece.Clear();
			UndoReset();

			// Clear the namespace identifier, but add a URL to the <math> token eventually
			nameSpace = "";
			mathTagNameSpaceUrl = "http://www.w3.org/1998/Math/MathML";

			// Add an empty 1st element
			xmlPiece.Add( "<mrow></mrow>" );

			// Update the running MathML formula
			string l = ParseMathList( 0 );
			XmlText = "<math>" + l + "</math>";

			try {
				doc = null;
				doc = new MathMLDocument();
			}
			catch { }

			// This converts all Invisible Plus tokens to Invisible Times tokens for purposes of rendering.
			//   This is done because Invisible Plus renders as a ridiculously huge space.
			string xmlNoInvisPlus = XmlText.Replace( "⁤", "⁢" );
			doc.InnerXml = xmlNoInvisPlus;
			//doc.InnerXml = XmlText;

			UpdateMultiFunctionButton();

			// Set up the ListBox for adding stuff again
			LISTBOX_ShowExcerpts.Items.Add( TagifyID( 0 ) + "-<" + GetExcerptDescription( xmlPiece[ 0 ], 0 ) + ">  " + DeXml( xmlPiece[ 0 ] ) );
			TEXTBOX_MathExpression.Text = "";

			LISTBOX_ShowExcerpts.SelectedIndex = 0;

			UndoLock = false;
		} // Clear Formula


		/// <summary>
		/// Takes MathML in the form of a String (cutified or no), and configures the WAVES Toolbar to edit it as a formula.
		/// </summary>
		/// <param name="s">A string representing the MathML to convert to a formula.</param>
		private void ConstructFormulaFromMathmlString( string s )
		{
			// Remove all line breaks, with their trailing whitespaces.  Essentially, de-cutify the mathml.
			for( int i = 100; i > -1; i -= 2 ) {
				string ws = "";
				for( int j = 0; j < i; j++ )
					ws += " ";
				ws = "\n" + ws;

				s = s.Replace( ws, "" );
			}
			s = s.Replace( "\r", "" );

			// Determine the namespace ID of the <math> tag, if any.  This will be the namespace ID that all tags will share in this formula.
			int mathLength = s.IndexOf( '>' ) + 1;
			string mathTag = s.Substring( 0, s.IndexOf( '>' ) + 1 );

			if ( mathTag.IndexOf( ':' ) > 0 )
				nameSpace = mathTag.Substring( 1, mathTag.IndexOf( ':', 0 ) );
			if ( nameSpace.Contains( "http" ) )
				nameSpace = "";

			// Remove all namespacing for the purposes of editing and displaying the MathML
			if ( ( nameSpace != "" ) & ( nameSpace != null ) ) {
				s = s.Replace( nameSpace, "" );
				mathTag = mathTag.Replace( nameSpace, "" );
			}

			// Remove all extraneous attributes from the all MathML tags, and remove any MathML <!-- comments -->... 
			//   This will be in case we retrieved MathML with ID and width tags, among others
			s = RemoveXmlAttributesFromMathString( s );

			// Must reset the document for display in the MathML Renderer
			doc = null;			
			doc = new MathMLDocument();

			if( s != null ) {
				// This converts all Invisible Plus tokens to Invisible Times tokens for purposes of rendering.
				//   This is done because Invisible Plus renders as a ridiculously huge space.
				string xmlNoInvisPlus = s.Replace( "⁤", "⁢" );
				doc.InnerXml = xmlNoInvisPlus;

				if( doc.DocumentElement.Name != "math" ) {
					MessageBox.Show( "This MathML document is malformed:\n\n  The root element must be a \"math\" element.\n\nFix the source MathML and try again.", "WAVES_Token_Toolbar", MessageBoxButtons.OK, MessageBoxIcon.Exclamation );
					ClearFormula();
					return;
				}

				// Remove the <math> wrapper from the long XML string.  By this point, the opening <math> tag has been
				//   converted to a tag without attributes.
				string innerS = s.Replace( "<math>", "" );
				innerS = innerS.Replace( "</math>", "" );

				// Clear the Listbox to prepare it for receiving the new formula
				LISTBOX_ShowExcerpts.Items.Clear();
				xmlPiece.Clear();
				for( int i = 0; i < idTaken.Count(); i++ )
					idTaken[ i ] = false;

				// For multi-line formulas, we would need to parse MathML more than just once!
				//   And even still, the MathML renderer only does one-line rendering.  
				//   Therefore, Mspace/Msytle/Mphantom would need to be supported for multi-line rendering.
				
				ParseMathMLtoList( innerS, 0 );

				xmlPiece.Sort();
				for( int i = 0; i < xmlPiece.Count; i++ )
					xmlPiece[ i ] = xmlPiece[ i ].Substring( 6 );

				for( int i = 0; i < xmlPiece.Count(); i++ ) {
					LISTBOX_ShowExcerpts.Items.Add( TagifyID( i ) + "-<" + GetExcerptDescription( xmlPiece[ i ], i ) + ">  " + DeXml( xmlPiece[ i ] ) );
				}
			
				XmlText = ParseMathList( 0 );
				UndoLock = true;
				LISTBOX_ShowExcerpts.SelectedIndex = 0;
				UndoLock = false;

				// Reset Undo structure
			}
			else {
				// Something did not work in the DOM-ification of the MathML string...
				ClearFormula();
				return;
			}

		}


		/// <summary>
		/// This function removes all attributes from MathML tags, and all comments from the MO and MI tokens that have them.  
		/// This is accomplished by walking the MathML DOM.  The resultant string will be much easier to read.
		/// </summary>
		/// <param name="xml">The string representing the MathML to clean up.</param>
		/// <returns>A string representing cleaned up MathML.</returns>
		private string RemoveXmlAttributesFromMathString( string mathStr )
		{
			// We will recreate the MathML string from walking the string as an XML document.
			string answer = "";

			// First, attempt to place the string within a MathML DOM.  This can fail for numerous reasons, but none of them 
			//   should be fatal to the operation of WAVES.  If it does fail, let the user know why, and abort Paste/Import.
			MathMLDocument mathml = new MathMLDocument();

			try {
				mathml.InnerXml = mathStr;
			}
			catch( Exception theError ) {
				MessageBox.Show( "This MathML document is malformed:\n\n  " + theError.Message + "\n\nFix the source MathML and try again.", "WAVES_Token_Toolbar", MessageBoxButtons.OK, MessageBoxIcon.Exclamation );
				return null;
			}

			// Retrieve the Namespace URL from the DocumentElement
			mathTagNameSpaceUrl = mathml.DocumentElement.NamespaceURI;
			if( ( mathTagNameSpaceUrl == "" ) & ( nameSpace == "" ) )
				mathTagNameSpaceUrl = "http://www.w3.org/1998/Math/MathML";


			// Walk the DOM we've created, recreating the MathML string as we go.
			answer = RemoveAttributesFromNode( mathml.DocumentElement );


			return answer;
		}

		/// <summary>
		/// This function returns a string representing all nodes within the MathML Document that belong at a specified root element.
		/// Iterates recursively.
		/// </summary>
		/// <param name="mathml">The full MathML DOM.</param>
		/// <param name="node">The root element to parse.</param>
		/// <returns></returns>
		private string RemoveAttributesFromNode( XmlNode node )
		{
			if( node.Name == "#text" ) {
				// This is a node's textual content.  Return it as "wrote", but with &, <, and > characters with HTML entities.
				string tx = node.InnerText;
				tx = tx.Replace( "&", "&amp;" );
				tx = tx.Replace( ">", "&gt;" );
				tx = tx.Replace( "<", "&lt;" );

				return tx;
			}
			else if( node.Name == "#comment" ) {
				// Comments are removed!
				return "";
			}
			else {
				// This is a MathML tag, therefore, add the tag 
				string answer = "<" + node.Name + ">";

				// Walk the children of this node
				foreach( XmlNode n in node.ChildNodes ) 
					answer += RemoveAttributesFromNode( n );

				// Finally, close the tag and conclude
				answer += "</" + node.Name + ">";
				return answer;
			}
		}



		/// <summary>
		/// This function, set up to iterate recursively, steps through the MathML formula, replacing ID tags with actual MathML elements.
		/// Once it returns, you'll have a well-formed MathML excerpt.
		/// </summary>
		/// <param name="ind">The index of the formula to parse.</param>
		/// <returns>A well-formed MathML representation of the formula at index "ind".</returns>
		private string ParseMathList( int ind )
		{
			string str = xmlPiece[ ind ];
			
			int pos = str.IndexOf( '⊰', 0 );
			while( pos > -1 ) 
			{
				// Replace this ID tag with the MathML that it represents
				string replaceStr = str.Substring( ( pos + 1 ), 3 );
				string addStr = ParseMathList( Convert.ToInt32( replaceStr ) );
				str = str.Replace( "⊰" + replaceStr + "⊱", addStr );

				pos = str.IndexOf( '⊰', pos );
			} 
			
			return str;
		} // Parse Math List (recursive)


		/// <summary>
		/// This function, set up to iterate recursively, steps through a MathML excerpt, and sets up the lists to match it, tagified and all.
		/// </summary>
		/// <param name="math">A string representing well-formed XML at the given MathML node.</param>
		/// <param name="thisID">The ID of the node to parse.</param>
		private void ParseMathMLtoList( string math, int thisID )
		{
			idTaken[ thisID ] = true;

			string thisListString = "";

			// Calculate the wrapper on this excerpt of XML
			string wrapper = "";
			int lenWrapper = 0;
			for( lenWrapper = 1; math.Substring( lenWrapper, 1 ) != ">"; lenWrapper++ )
				wrapper += math.Substring( lenWrapper, 1 );


			if( wrapper != "mtext" ) {
				// Remove the wrapper from the XML excerpt
				string oldMath = math;
				math = DetermineComplexTag( oldMath.Substring( 0 ) );

				math = math.Substring( lenWrapper + 1, math.Length - ( lenWrapper * 2 ) - 3 );

				// Step through the excerpt, finding the wrappers that must be parsed again
				int oldPos = 0;
				int pos = math.IndexOf( '<', oldPos );

				if( pos > -1 )
					thisListString += math.Substring( oldPos, pos );

				while( pos > -1 ) {
					//			for( int i = 0; i < math.Length; i++ ) {
					//				if( math.Substring( i, 1 ) == "<" ) {
					if( ( math.Substring( pos, 4 ) != "<mi>" ) &&
						 ( math.Substring( pos, 4 ) != "<mn>" ) &&
						 ( math.Substring( pos, 4 ) != "<mo>" ) &&
						//( ( math.Length > ( pos + 7 ) ) && ( math.Substring( pos, 7 ) != "<mtext>" ) ) &&
						 ( math.Substring( pos, 4 ) != "<!--" ) &&
						 ( math.Substring( pos, 2 ) != "</" ) ) {
						// This is not a <mi>, <mn>, or <mo> element, and must be recursively parsed.

						// Get the next available excerpt slot
						int nextID = 0;
						for( nextID = 0; idTaken[ nextID ]; nextID++ )
							;

						thisListString += TagifyID( nextID );

						// Now, we need to isolate the part of the math string that represents this complex tag.
						//   Then we recursively call this function to get its excerpt.
						string newMath = DetermineComplexTag( math.Substring( pos ) );

						ParseMathMLtoList( newMath, nextID );

						// And then we can continue stepping through the remainder of the math tag (after this
						//   complex tag is closed).
						//int j;
						//for (j = i; math.Substring(j, 1) != ">"; j++) ;
						nextID++;

						oldPos = pos + newMath.Length - 1;
						pos = math.IndexOf( '<', ( oldPos + 1 ) );
						//pos = pos + newMath.Length;
					}
					else {
						// This is a <mi>, <mn>, or <mo> element.  Simply keep going as normal.
						//thisListString += math.Substring( pos, 1 );
						oldPos = pos;
						pos = math.IndexOf( '<', ( oldPos + 1 ) );

						if( pos > -1 )
							thisListString += math.Substring( oldPos, ( pos - oldPos ) );
						else
							thisListString += math.Substring( oldPos );
					}
				}
			}
			else {
				// This is an <mtext> element. The textual content should be treated as straight text, and not math.
				string closeWrapper = "</" + wrapper + ">";
				string openWrapper = "<" + wrapper + ">";

				thisListString = math.Replace( openWrapper, "" );
				thisListString = thisListString.Replace( closeWrapper, "" );
			}

			// Replace HTML entities with actual characters
			thisListString = thisListString.Replace( "&lt;", "<" );
			thisListString = thisListString.Replace( "&gt;", ">" );
			thisListString = thisListString.Replace( "&amp;", "&" );

			xmlPiece.Add( TagifyID( thisID ) + ":<" + wrapper + ">" + thisListString + "</" + wrapper + ">" );
			return;

		} // Parse MathML to List (recursive)


		/// <summary>
		/// This function figures out the starting tag of a mathml excerpt, and then determines where that tag closes.  
		/// It returns the value of the well formed excerpt.
		/// </summary>
		/// <param name="math">String representing MathML in which to find Complex MathML element tags</param>
		/// <returns>A well formed excerpt</returns>
		private string DetermineComplexTag( string math )
		{
			// This function figures out the starting tag of a mathml excerpt, and then 
			//   determines where that tag closes.  It returns the value of the well formed
			//   excerpt.
			string answer = "";

			string wrapper = math.Substring( 1, math.IndexOf( '>', 1 ) - 1 );
			string close_wrapper = "</" + wrapper + ">";
			wrapper = "<" + wrapper + ">";

			// Find where this tag closes.  There may be multiple tags like the one we're looking for (like mfracs
			//   nested within mfracs), so we need to keep a running count of stuff like that.
			int numNested = 0;
			for( int i = ( wrapper.Length ); i > -1; i = math.IndexOf( '<', ( i + 1 ) ) ) {
				// If this tag matches the close-tag that we're searching for, we need to close this one before
				//   we find the close-tag we are looking for.
				if( math.Substring( i, wrapper.Length ) == wrapper )
					numNested++;

				if( math.Substring( i, close_wrapper.Length ) == close_wrapper ) {
					if( numNested == 0 ) {
						// This close-tag is the one we are looking for!
						answer = math.Substring( 0, i + close_wrapper.Length );
						break;
					}
					else {
						// This close-tag closes an internal tag that we're not looking for.
						numNested--;
					}
				}
			}
			
			// Thanks to KirkWoll at http://stackoverflow.com/questions/541954/how-would-you-count-occurences-of-a-string-within-a-string-c
			//int count = new Regex(wrapper).Matches(math).Count;

			return answer;
		}


		/// <summary>
		/// Strips all MathML tags from an XML string.
		/// </summary>
		/// <param name="str">XML string to strip</param>
		/// <returns>The resultant string</returns>
		private string DeXml( string str )
		{
			// Strip all registered MathML elements from the formula
			for( int i = 0; i < MasterToken._elemCount; i++ ) {
				str = str.Replace( "<" + MasterToken._mathMLelement[ i ]._symbol + ">", "" );
				str = str.Replace( "</" + MasterToken._mathMLelement[ i ]._symbol + ">", "" );
			}

			// Additionally, <mo>, <mi>, and <mn> tags are essential MathML tags
			str = str.Replace( "<mo>", "" );
			str = str.Replace( "<mi>", "" );
			str = str.Replace( "<mn>", "" );

			str = str.Replace( "</mo>", "" );
			str = str.Replace( "</mi>", "" );
			str = str.Replace( "</mn>", "" );

			return str;
		} // de-XML


		/// <summary>
		/// This function retrieves the wrapper of a MathML excerpt. It may be in MathML, or an English equivalent, based on user preference.
		/// </summary>
		/// <param name="str">The MathML excerpt string to parse</param>
		/// <param name="id">The ID of the MathML excerpt</param>
		/// <returns>Description of the MathML excerpt</returns>
		private string GetExcerptDescription( string str, int id )
		{
			string xml = GetXmlWrapper( str );

			// Find the xml wrapper in the Master Element List
			int elemID = GetMasterElementID( xml );

			// Determine which prompt to show based off of user setting
			if( Properties.Settings.Default.EnglishExcerpts ) {
				if( ( xml == "mrow" ) && ( id != 0 ) )
					return DetermineMrowEnglishMeaning( xml, id );
				else
					if( elemID < MasterToken._elemCount )
						return MasterToken._elemEnglish[ elemID ][ 0 ];
					else {
						// This tag is not registered in the Master Token List!
						return "????";
					}
			}
			else
				return xml;

		}


		/// <summary>
		/// Determines the English description of an mrow argument of another complex MathML element.
		/// </summary>
		/// <param name="xmlWrapper">The complex MathML element that uses this "mrow" as an argument</param>
		/// <param name="id">The MathML id of this "mrow" argument</param>
		/// <returns>The English description of this mrow argument.</returns>
		private string DetermineMrowEnglishMeaning( string xmlWrapper, int id )
		{
			// This function will determine the English description of an mrow element.  The first item necessary
			//   is to determine the parent's excerpt ID
			string answer = "????";

			for( int parentElem = 0; parentElem < id; parentElem++ ) {
				string ex = RetrieveExcerptFromListbox( LISTBOX_ShowExcerpts.Items[ parentElem ].ToString() );

				if( ex.Contains( TagifyID( id ) ) ) {
					// If execution reaches here, we have determined the parent's excerpt ID.  Now, we need to find 
					//   the parent's XML wrapper, and find that within the Master Element List
					string xml = GetXmlWrapper( xmlPiece[ parentElem ] );
					int elemID = GetMasterElementID( xml );

					// The syntax of a non-mrow Excerpt consists only of tags.  Therefore, we can determine which
					//   description to apply based on where the tagified ID sits within the excerpt.
					int pos = ex.IndexOf( '⊰', 0 );
					int numArg = 1;

					while( pos > -1 ) {
						if( Convert.ToInt32( ex.Substring( pos + 1, 3 ) ) == id ) {
							if( elemID < MasterToken._elemCount ) {
								if( numArg <= MasterToken._mathMLelement[ elemID ]._args )
									answer = MasterToken._elemEnglish[ elemID ][ numArg ];
								else 
									// If this is the case, assume that this is an <mrow> within an <mrow>
									answer = "extend-row";
							}
							else 
								// This is an <mrow> argument of an unregistered MathML element, and WAVES cannot identify it
								answer = "?-row-?";
						}

						// Check next tag
						pos = ex.IndexOf( '⊰', pos + 1 );
						numArg++;
					}					
				}
			}

			return answer;
		}


		/// <summary>
		/// Retrieves the ID of the specified MathML element wrapper in the Master Token List
		/// </summary>
		/// <param name="xmlWrapper">The element wrapper string to search for</param>
		/// <returns>The position the element wrapper occupies in the Master Token List</returns>
		private int GetMasterElementID( string xmlWrapper )
		{
			int id;
			for( id = 0; id < MasterToken._elemCount; id++ )
				if( MasterToken._mathMLelement[ id ]._symbol == xmlWrapper )
					break;

			return id;
		}


		/// <summary>
		/// This function retrieves the Xml wrapper (in other words, the containing MathML element)
		/// for the MathML excerpt passed in as an argument.
		/// </summary>
		/// <param name="str">A well formed MathML excerpt string</param>
		/// <returns>The MathML wrapper for this excerpt "mn", "mrow", "mfrac", etc.</returns>
		private string GetXmlWrapper( string str )
		{
			return str.Substring( str.IndexOf( '<' ) + 1, str.IndexOf( '>', 1 ) - ( str.IndexOf( '<' ) + 1 ) );

//			string answer = "";
//			int i = 1;
//			while( str.Substring( i, 1 ) != ">" ) {
//				answer += str.Substring( i, 1 );
//				i++;
//			}

//			return answer;
		} // Get MathML Wrapper


		/// <summary>
		/// Used in conjunction with Adding a New MathML tag, this function finds the first ID which is not currently in use
		/// and returns that ID as an integer.
		/// </summary>
		/// <param name="curMax">The number of MathML excerpts currently initialized in excerpt lists</param>
		/// <returns>The integer ID of an unused MathML excerpt.</returns>
		private int FindTagExcerpt( int curMax )
		{
			bool[] XMLtag = new bool[ curMax ];
			int answer = -1;

			// Find the first available new XML excerpt to add this tag to
			for( int i = 1; i < curMax; i++ )
				XMLtag[ i ] = false;
			XMLtag[ 0 ] = true;

			foreach( string s in xmlPiece ) {
				for( int i = 1; i < curMax; i++ ) {
					string comp = TagifyID( i );
					comp = comp.Substring( 1, 3 );

					if( s.Contains( comp ) )
						XMLtag[ i ] = true;
				}
			}

			for( int i = 1; i < curMax; i++ ) {
				if( XMLtag[ i ] == false ) {
					answer = i;
					break;
				}
			}

			// If we are adding a new tag, it better not have an ID greater than 999!  That would be a
			//   friggin' huge MathML construct, anyway...
			if( answer < 0 )
				if( xmlPiece.Count < 999 )
					answer = xmlPiece.Count;

			return answer;

		} // function FindTagExcerpt


		/// <summary>
		/// Called when converting text to MathML, intelligently adds the Invisible Times operator before "mi" tokens and 
		/// "msqrt", and "mroot" elements when necessary
		/// </summary>
		/// <param name="c">A Char array representing text that will be converted to MathML</param>
		/// <param name="i">Position in the Char array to test.</param>
		/// <returns>Invisible times operator if necessary, empty string if not</returns>
		public string MayAddInvisibleTimes( char[] c, int i )
		{
			string answer = "";

			if( i > 0 )
				if( Char.IsLetterOrDigit( c[ i - 1 ] ) )
					answer += "<mo>⁢</mo>";

			return answer;
		} // MayAddInvisibleTimes... for many symbols


		/// <summary>
		/// Called when converting text to MathML, intelligently adds the Invisible Times operator before "mfrac" elements when necessary
		/// </summary>
		/// <param name="c">A Char array representing text that will be converted to MathML</param>
		/// <param name="i">Position in the Char array to test.</param>
		/// <returns>Invisible times operator if necessary, empty string if not</returns>
		public string MayAddInvisibleTimesLimited( char[] c, int i )
		{
			string answer = "";

			if( i > 0 ) {
				if( Char.IsLetter( c[ i - 1 ] ) )
					answer += "<mo>⁢</mo>";
				//if (Char.IsDigit(c[i - 1]))
				//    answer += "<mtext>and⁢</mtext>";
			}

			return answer;
		} // MayAddInvisibleTimesLimited... for fractions!


		/// <summary>
		/// Called when converting text to MathML, intelligently adds the Invisible Plus operator before "mfrac" elements when necessary
		/// </summary>
		/// <param name="c">A Char array representing text that will be converted to MathML</param>
		/// <param name="i">Position in the Char array to test.</param>
		/// <returns>Invisible plus operator if necessary, empty string if not</returns>
		public string MayAddInvisiblePlus( char[] c, int i )
		{
			string answer = "";

			if( i > 0 ) {
				if( Char.IsDigit( c[ i - 1 ] ) )
					answer += "<mo>⁤</mo>";
				//if (Char.IsDigit(c[i - 1]))
				//    answer += "<mtext>and⁢</mtext>";
			}

			return answer;
		} // MayAddInvisibleTimesLimited... for fractions!


		public string MayAddInvisibleApplyFunc( string tx, int i )
		{
			string answer = "";

			for( int id = 0; id < MasterToken._miCount; id++ ) {
				int symLen = MasterToken._mi[ id ]._symbol.Length;

				if( ( i >= symLen ) & ( symLen > 1 ) ) {
					if( tx.Substring( i - symLen, symLen ) == MasterToken._mi[ id ]._symbol )
						answer += "<mo>⁡</mo>";

				}
			}

			if ( ( answer == "" ) & ( i > 0 ) )
				if ( Char.IsLetter( tx[ i - 1 ] ) )
					answer += "<mo>⁡</mo>";

			return answer;
		}

		/// <summary>
		/// Takes a natural math string (an excerpt from LISTBOX_ShowExcerpts), adds MathML tags to all "mo", "mi", and "mn" tokens, and 
		/// wraps the string in its MathML element tag.  It leaves all Tagified IDs intact.
		/// </summary>
		/// <param name="tx">The natural math string from LISTBOX_ShowExcerpts. It should have ID and XML wrapper stripped.</param>
		/// <returns>A MathML excerpt representing the currently selected item in LISTBOX_ShowExcerpts</returns>
		public string TextToXML( string tx )
		{
			// Not surprisingly, this function takes a regular string, and tagifies the various tokens
			//   symbols, and numbers into MathML.  Element IDs remain intact, however.

			// A description of the (below) variables:
			//   answer--           a running string that contains the compiled MathML
			//   buildingNumber--   for numbers, this contains a running record of digits to put into a <m:mn> tag
			//   c[]--              contains a Char array representation of the textbox string to translate into MathML

			string answer = "";
			string buildingNumber = "";
			int ind = EditXmlExcerpt;

			string xmlWrapper = GetXmlWrapper( xmlPiece[ ind ] );

			// If this is a text element, what is typed is what is shown.  No MathML elements are allowed in <mtext>.
			if( xmlWrapper == "mtext" )
				return "<mtext>" + tx + "</mtext>";

			// If this new element is a power, then parentheses may potentially need to be added 
			//   if there is more than one token within the base
			if( ( xmlWrapper == "mrow" ) ) {
				// Must determine if this represents the base of an <msup>
				for ( int i = 0; i < EditXmlExcerpt; i++ ) {
					string thisExcerpt = xmlPiece[ i ];
					if( GetXmlWrapper( thisExcerpt ) == "msup" ) {
						if( RetrieveExcerptFromListbox( LISTBOX_ShowExcerpts.Items[ i ].ToString() ).Substring( 0, 5 ) == TagifyID( EditXmlExcerpt ) ) {
							// This mrow is a base of a power... either it should have one element, or it should receive automatic parentheses

							// However, the Listbox should not contain those parentheses!!
							string newExcerpt = "";
							newExcerpt = RetrieveExcerptFromListbox( LISTBOX_ShowExcerpts.Items[ EditXmlExcerpt ].ToString() );
							
							while ( ( newExcerpt.Length > 0 ) && ( newExcerpt[ 0 ] == '(' ) )
								newExcerpt = newExcerpt.Substring( 1 );

							while ( ( newExcerpt.Length > 0 ) && ( newExcerpt[ newExcerpt.Length - 1 ] == ')' ) )
								newExcerpt = newExcerpt.Substring( 0, newExcerpt.Length - 1 );

							LISTBOX_ShowExcerpts.Items[ EditXmlExcerpt ] = TagifyID( EditXmlExcerpt ) + "-<" + GetExcerptDescription( xmlPiece[ EditXmlExcerpt ], EditXmlExcerpt ) + ">  " + newExcerpt;

							if( IsMultipleTokens( tx ) )
								if ( ( tx[ 0 ] != '(' ) | ( tx[ tx.Length - 1 ] != ')' ) )
									tx = "(" + tx + ")";
						}
					}
				}
			}

			Char[] c = tx.ToCharArray();


			// This function will convert the text in the Textbox to XML
			for( int i = 0; i < ( tx.Length ); i++ ) {
				bool useNumber = true;
				// If this character is "⊰", leave it and any tag alone
				if( c[ i ] == '⊰' ) {
					if( tx.Length > ( i + 4 ) ) {
						// Test for any needed invisible times or other language
						String tagIDnum = ( c[ i + 1 ].ToString() + c[ i + 2 ].ToString() + c[ i + 3 ].ToString() );
						int tagNum = Convert.ToInt32( tagIDnum );
						string newElement = GetXmlWrapper( xmlPiece[ tagNum ] );

						// If this new element is a radical, then test to add invisible times.  This cannot happen within an mfenced element.
						if( xmlWrapper != "mfenced" ) {
							if( ( newElement == "msqrt" ) || ( newElement == "mroot" ) || ( newElement == "msub" ) || ( newElement == "msup" ) )
								answer += MayAddInvisibleTimes( c, i );
						}

						if( newElement == "mfenced" )
							answer += MayAddInvisibleApplyFunc( tx, i );

							// If this new element is a fraction, then test to add either an
							//   invisible times (if a symbol) or a vocalized "and" (if is a digit)
							if( ( newElement == "mfrac" ) ) {
								answer += MayAddInvisibleTimesLimited( c, i );
								answer += MayAddInvisiblePlus( c, i );
							}

						// Add ID tag
						answer += "⊰" + c[ i + 1 ] + c[ i + 2 ] + c[ i + 3 ] + "⊱";
						useNumber = false;
						i = i + 4;
					}
					else {
						// If we are here, we shouldn't be :-) 
						// Tags are fully deleted in the KeyPress event.

						//string t = TEXTBOX_MathExpression.Text;

						//if (xmlWrapper != "mrow")       TEXTBOX_MathExpression.Text += "⊱";
						//else                            TEXTBOX_MathExpression.Text = t.Substring(0, t.Length - 4);

						//break;
					}
				}
				// Letters convert to an <m:mi> tag
				if( Char.IsLetter( c[ i ] ) ) {
					// Well. Almost all the time an <mi> tag is one variable, or one token.  However, function names are also <mi> tokens;
					//   so some multi-letter <mi> tokens must be accounted for rather manually.  
					string miHere = DetermineMiFunction( tx, i );

					if( miHere != null ) {
						// There is a special function here... therefore this <mi> tag will have multiple letters in it.
						if( xmlWrapper != "mfenced" )
							answer += MayAddInvisibleTimes( c, i );
						answer += "<mi>" + tx.Substring( i, miHere.Length ) + "</mi>";
						i = i + ( miHere.Length - 1 );
					}
					else {
						// There is no special function here
						if( xmlWrapper != "mfenced" )
							answer += MayAddInvisibleTimes( c, i );
						answer += "<mi>" + c[ i ] + "</mi>";
					}
				} // If the current char is a letter
				// A succession of digits convert to an <m:mn> tag.
				// Decimal points are also valid, as are internal commas.
				else if( TestForMN( tx, i, buildingNumber ) ) {
					if( useNumber == true ) {
						buildingNumber += c[ i ];

						if( tx.Length == ( i + 1 ) ) {
							// This is the last Char in the copy string.  Place an <mn>
							answer += "<mn>" + buildingNumber + "</mn>";
							buildingNumber = "";
						}
						else
							if( !( Char.IsDigit( c[ i + 1 ] ) ) && ( c[ i + 1 ] != '.' ) && ( c[ i + 1 ] != ',' ) ) {
								// The next Char in the copy string is not a digit, decimal point, or comma. Place an <mn>
								answer += "<mn>" + buildingNumber + "</mn>";
								buildingNumber = "";
							}
							else {
								// The next Char in the copy string *is* a digit.  
								//   Therefore Do not place an <mn>, make a note of this Char.
							}
					}
					else
						useNumber = true;
				} // If the current char is a digit
				// Otherwise, the token should match up with one of the mo[] or mi[] tags from the main MathML XML list
				else {
					// Check if the number buffer needs flushing (necessary because commas are valid with <mn> and as an <mo>
					if( buildingNumber != "" ) {
						// The next Char in the copy string is not a digit, decimal point, or comma. Place an <mn>
						answer += "<mn>" + buildingNumber + "</mn>";
						buildingNumber = "";
					} // If the number buffer has unused numbers (important because of comma check)

					int j = 0;
					for( j = 0; j < ( MasterToken._moCount ); j++ ) {
						Token t = MasterToken._mo[ j ];
						Char[] sym = t._symbol.ToCharArray();
						if( c[ i ] == sym[ 0 ] ) {
							// These tokens may end up rendering an additional "invisible times"
							if( ( xmlWrapper != "mfenced" ) && ( c[ i ].Equals( '(' ) ) )
								answer += MayAddInvisibleTimes( c, i );

							// These checks are important for proper rendering in the 
							//   MathML Rendering control.  
							if( c[ i ].Equals( '>' ) )
								answer += "<mo>&gt;</mo>";
							else if( c[ i ].Equals( '<' ) )
								answer += "<mo>&lt;</mo>";
							else if( c[ i ].Equals( '"' ) )
								answer += "<mo>\"</mo>";
							else if( c[ i ].Equals( "'" ) )
								answer += "<mo>\'</mo>";
							else if( c[ i ].Equals( '&' ) )
								answer += "<mo>&amp;</mo>";
//							else if( c[ i ].Equals( '|' ) )
//								answer += "<mo>❘</mo>";
							else
								answer += "<mo>" + sym[ 0 ] + "</mo>";
						}
					} // Check the mo group
					for( j = 0; j < ( MasterToken._miCount ); j++ ) {
						Token t = MasterToken._mi[ j ];
						Char[] sym = t._symbol.ToCharArray();
						if( c[ i ] == sym[ 0 ] ) {
							// These tokens may end up rendering an additional "invisible times"
							if( ( xmlWrapper != "mfenced" ) && ( ( c[ i ].Equals( 'π' ) ) | ( c[ i ].Equals( '℮' ) ) | ( c[ i ].Equals( 'ι' ) ) ) )
								answer += MayAddInvisibleTimes( c, i );

							answer += "<mi>" + sym[ 0 ] + "</mi>";
						}
					} // Check the mi group

				} // We need to find this Char in the XML
			}

			// Finally, wrap the result in the excerpt's MathML tag.
			answer = "<" + xmlWrapper + ">" + answer + "</" + xmlWrapper + ">";
			return answer;
		} // Text to XML (MathML)


		/// <summary>
		/// This function is used to determine whether parentheses should be placed around a group of tokens (msup)
		/// </summary>
		/// <param name="tx">A string representing any number of tokens</param>
		/// <returns>True if > 1 token is represented, false otherwise</returns>
		private bool IsMultipleTokens( string tx )
		{
			bool answer = false;

			// Remove parentheses before evaluating whether this is truly multiple tokens
			while ( ( tx.Length > 0 ) && ( tx[ 0 ] == '(' ) )
				tx = tx.Substring( 1 );

			while ( ( tx.Length > 0 ) && ( tx[ tx.Length - 1 ] == ')' ) )
				tx = tx.Substring( 0, tx.Length - 1 );

			// Check if the length of this expression is more than one token
			if( tx.Length > 1 ) {
				for ( int i = 0; i < tx.Length; i++)
					if ( !Char.IsDigit( tx[ i ] ) )
						answer = true;
			}

			return answer;
		}

		/// <summary>
		/// This function is used by the TextToXML function to determine if an "mn" tag should continue to be processed.  
		/// "Mn" tags may be any number, and can be a succession of digits, can contain decimal points, and internal commas.
		/// </summary>
		/// <param name="tx">The natural math string from LISTBOX_ShowExcerpts, passed in to TextToXML</param>
		/// <param name="pos">The character position within "tx" to check</param>
		/// <param name="buffer">The running data to be placed within the current "mn" tag</param>
		/// <returns>True if the tested char belongs within the "mn" tag, false otherwise</returns>
		private bool TestForMN( string tx, int pos, string buffer )
		{
			// This function is used by the TextToXML function to determine if we are rendering a <mn> tag
			// The test got to be complex enough that this seemed the way to go.

			Char[] c = tx.ToCharArray();

			// If this is the last character in the text string, we only care if it is a number.
			// Otherwise, we are interested in valid decimal points and comma separators.
			if( pos == ( tx.Length - 1 ) )
				return ( Char.IsDigit( c[ pos ] ) );
			else
				return ( Char.IsDigit( c[ pos ] ) ||
					 c[ pos ] == '.' ||
					 ( ( c[ pos ] == ',' ) && ( Char.IsDigit( c[ pos + 1 ] ) ) && ( buffer != "" ) ) );

			// The original check in TextToXML:
			//if ((Char.IsDigit(c[i]) || c[i] == '.' || ((c[i] == ',') && (buildingNumber != "")))

		} // Function TestForMN


		/// <summary>
		/// The function is used by TextToXml to determine whether a letter is begins a function name, which should all be
		/// placed within one MI tag.  Returns "null" if this letter does not begin a function name, otherwise returns the name of the function.
		/// </summary>
		/// <param name="tx">The full string to be converted to MathML</param>
		/// <param name="pos">The character position at which the function might begin</param>
		/// <returns></returns>
		private string DetermineMiFunction( string tx, int pos )
		{
			string answer = null;

			if( ( pos > 0 ) && ( Char.IsLetter( tx[ pos - 1 ] ) ) )
				return null;
			
			foreach( string func in RegisteredFunctions ) {
				// Test if this function is here... but make sure that the string is long enough to not overflow the string bounds!

				if( func.Length <= ( tx.Length - pos ) ) {
					if( tx.Substring( pos, func.Length ) == func ) {
						answer = func;
						break;
					}
				}
			}

			return answer;
		}

		/// <summary>
		/// This function contains the logic to add one new MathML excerpt to the formula.
		/// </summary>
		/// <param name="tagType">The MathML element wrapper for the new excerpt</param>
		/// <param name="numArgs">The number of "mrow" arguments that the new MathML element requires</param>
		/// <param name="modExcerpt">The excerpt ID to add the reference to this new excerpt. A tagified ID will be placed here.</param>
		/// <param name="jumpNow">Set this to true if it is desired for this new MathML excerpt to be selected in its Listbox</param>
		/// <returns>The excerpt ID to select within its Listbox</returns>
		public int AddTagToFormula( string tagType, int numArgs, int modExcerpt, bool jumpNow )
		{
			// Find the next free XML excerpt, to place the new tag
			int newTagPlace = FindTagExcerpt( xmlPiece.Count );

			// If this tag cannot be added, tell user and abort
			if( newTagPlace == -1 ) {
				MessageBox.Show( "Cannot add this MathML tag... too many tags in the formula.", "Token Toolbar", MessageBoxButtons.OK, MessageBoxIcon.Exclamation );
				return modExcerpt;
			}

			LISTBOX_ShowExcerpts.SelectedIndex = modExcerpt;

			if( newTagPlace < LISTBOX_ShowExcerpts.Items.Count ) {
				// This tag can be placed within the List as it stands; probably it got deleted
				xmlPiece[ newTagPlace ] = "<" + tagType + "></" + tagType + ">";

				LISTBOX_ShowExcerpts.Items[ newTagPlace ] = TagifyID( newTagPlace ) + "-<" + GetExcerptDescription( xmlPiece[ newTagPlace ], newTagPlace ) + ">  ";
				//LISTBOX_ShowExcerpts.Items[newTagPlace] = TagifyID(newTagPlace) + "-<" + tagType + ">  ";
//				TEXTBOX_MathExpression.Text += TagifyID( newTagPlace );
			}
			else {
				// This tag must be appended at the end of the list.  
				xmlPiece.Add( "<" + tagType + "></" + tagType + ">" );
				LISTBOX_ShowExcerpts.Items.Add( TagifyID( newTagPlace ) + "-<" + GetExcerptDescription( xmlPiece[ newTagPlace ], newTagPlace ) + ">  " );
				//LISTBOX_ShowExcerpts.Items.Add(TagifyID(newTagPlace) + "-<" + tagType + ">  ");
//				TEXTBOX_MathExpression.Text += TagifyID( newTagPlace );
			}

			string t = TEXTBOX_MathExpression.Text;
			int insertHere = t.Length;
			if( TEXTBOX_MathExpression.Focused )
				insertHere = TEXTBOX_MathExpression.SelectionStart;
			else if( TempInsertHere > -1 )
				insertHere = TempInsertHere;
			if( insertHere > t.Length )
				insertHere = t.Length;

			t = t.Insert( insertHere, TagifyID( newTagPlace ) );
			insertHere = insertHere + 5;

			TEXTBOX_MathExpression.Text = t;
			TempInsertHere = -1;
			//				TEXTBOX_MathExpression.Text += TagifyID( newTagPlace );


			// Automatically add the required number of <mrow> arguments to this tag
			int j = newTagPlace;
			for( int i = 0; i < numArgs; i++ ) {
				int throwAway;
				jumpNow = false;
				if( i == 0 ) {
					TEXTBOX_MathExpression.SelectionStart = TEXTBOX_MathExpression.Text.Length;
					j = AddTagToFormula( "mrow", -1, newTagPlace, true );
				}
				else {
					TEXTBOX_MathExpression.SelectionStart = TEXTBOX_MathExpression.Text.Length;
					throwAway = AddTagToFormula( "mrow", -1, newTagPlace, false );
				}
			}

			// Re-initialize the last excerpt... important when Englishizing it.
			int lastID = LISTBOX_ShowExcerpts.Items.Count - 1;
			LISTBOX_ShowExcerpts.Items[ lastID ] = TagifyID( lastID ) + "-<" + GetExcerptDescription( xmlPiece[ lastID ], lastID ) + ">  ";
			// By now, we have to determine which element should be selected to be edited.
			return j;
		} // function AddTagToFormula... adds one new tag 


		/// <summary>
		/// Receives a MathML formula, and adds newline characters, indentation, token commenting to make it much more easily human-readable.  Additionally, any namespacing is added.  The resultant MathML string is suitable to be copied to the Clipboard or saved to a file.
		/// </summary>
		/// <param name="tx">The MathML formula to convert.</param>
		/// <returns>A readable MathML formula, ready to copy to clipboard or save to .mathml file.</returns>
		private string CutifyMathML( string tx )
		{
			// This code block will make the MathML "cute"... 
			//   it will add New Lines and indentation to make it more readable

			string cutifyXML = "";
			int curIndent = 0;
			int indentIncrement = 2;

			int pos = tx.IndexOf( '<', 0 );

			while( pos > -1 ) {
				int newPos = tx.IndexOf( '<', pos + 1 );
				string test = tx.Substring( pos, 4 );
				string testText = tx.Substring( pos, 7 );

				if ( test.Substring( 0, 2 ) == "</" ) {
					// This is a closing tag; newline and indent is always added after a closing tag.
					
					// If this closing tag is not major, then we'll need to remove the last two "spaces"
					test += tx.Substring( pos + 4, 1 );
					if ( tx.Length > ( pos + 7 ) )
						testText = tx.Substring( pos + 7, 1 );
					if( ( ( test != "</mo>" ) & ( test != "</mi>" ) & ( test != "</mn>" ) & ( testText != "</mtext>" ) ) && ( cutifyXML.Substring( cutifyXML.Length - 2 ) == "  " ) ) {
						// This tag is not <math>
						cutifyXML = cutifyXML.Substring( 0, cutifyXML.Length - 2 );
					}

					curIndent -= indentIncrement;
					cutifyXML += AppendTagToXML( tx, pos, newPos, curIndent );
				}
				else {
					// This is an opening tag; check for a opening major tag here.  If it is NOT, add newline and indent afterwards.

					curIndent += indentIncrement;
					if( ( test != "<mo>" ) & ( test != "<mi>" ) & ( test != "<mn>" ) & ( testText != "<mtext>" ) ) {
						// This tag is not <math>
						cutifyXML += AppendTagToXML( tx, pos, newPos, curIndent );
					}
					else
						cutifyXML += AppendTagToXML( tx, pos, newPos, -1 );

				}

				pos = newPos;
			}

			// After all that is done, the last thing to do before returning the big ol' string is to add any namespacing that was present
			// before editing

			if( ( mathTagNameSpaceUrl != "" ) & ( mathTagNameSpaceUrl != null ) ) {
				// Insert xmlns attribute in <math> element
				int xmlnsPos = cutifyXML.IndexOf( '>' );
				string before = cutifyXML.Substring( 0, xmlnsPos );
				string after = cutifyXML.Substring( xmlnsPos );

				cutifyXML = before + " xmlns='" + mathTagNameSpaceUrl + "'" + after;
			}

			//nameSpace
			
			return cutifyXML;

		} // function CutifyMathML... adds indentation to MathML for copying


		/// <summary>
		/// Creates a string containing a specified number of spaces. Useful for adding indentation to cutified-MathML.
		/// </summary>
		/// <param name="num">Number of spaces to add to the end of the string.</param>
		/// <returns>The resultant string of spaces.</returns>
		private string AppendSpacesToXML( int num )
		{
			string append = "";
			for( int i = 0; i < num; i++ )
				append += " ";
			return append;
		}


		/// <summary>
		/// Returns the tag to append to the cutified-MathML. 
		/// </summary>
		/// <param name="tx">The string from which to retrieve the tag</param>
		/// <param name="pointA">Beginning of the tag</param>
		/// <param name="pointB">Ending of the tag</param>
		/// <param name="addIndent">Add a newline and this many indentation spaces before the tag.  If this parameter is zero, no new line is added.</param>
		/// <returns></returns>
		private string AppendTagToXML( string tx, int pointA, int pointB, int addIndent )
		{
			string answer = "";
			string toAdd = "";
			string first4 = "";

			// This is the string to append, from the main mathML string
			if( pointB > -1 )
				toAdd = tx.Substring( pointA, pointB - pointA );
			else
				toAdd = tx.Substring( pointA );

			// Figure out the first characters of the added string, if possible, to check if a descriptive comment should be added
			if ( toAdd.Length > 3 )
				first4 = toAdd.Substring( 0, 4 );

			// If the last five characters of the appended text denote that this is the end of an <mo> or <mi> tag,
			//   a descriptive comment should be added if the token represented is has a Unicode value > xff
			if( ( first4 == "<mo>" ) & ( toAdd.Length > 4 ) ) {
				string t = toAdd.Substring( 4 );
				char tChar = t[ 0 ];
				if( ( t.Length > 1 ) | ( tChar > 127 ) ) {
					// Add a description to this token
					foreach( Token tok in Token_Toolbar.MasterToken._mo ) {
						if( ( tok._symbol == t ) |
							( ( tok._symbol == ">" ) & ( t == "&gt;" ) ) |
							( ( tok._symbol == "<" ) & ( t == "&lt;" ) ) |
							( ( tok._symbol == "&" ) & ( t == "&amp;" ) ) )
							toAdd += "<!-- " + UnicodeLookup.GetUnicodeDescription( tok._symbol[ 0 ] ) + " -->";
							//toAdd += "<!-- " + tok._speech + " -->";
					}
				}
			}

			if( ( first4 == "<mi>" ) & ( toAdd.Length > 4 ) ) {
				string t = toAdd.Substring( 4 );
				char tChar = t[ 0 ];

				if( t.Length > 1 ) {
					// Add a description to this function
					foreach( Token tok in Token_Toolbar.MasterToken._mi ) {
						if( tok._symbol == t )
							toAdd += "<!-- " + tok._speech + " -->";
					}
				}
				else if ( tChar > 127 ) {
					// Add a description to this token
					foreach( Token tok in Token_Toolbar.MasterToken._mi ) {
						if( tok._symbol == t )
							toAdd += "<!-- " + UnicodeLookup.GetUnicodeDescription( tok._symbol[ 0 ] ) + " -->";
							//toAdd += "<!-- " + tok._speech + " -->";
					}
				}
			}

			if( ( nameSpace != "" ) & ( nameSpace != null ) ) {
				if( toAdd[ 1 ] == '/' )
					toAdd = "</" + nameSpace + toAdd.Substring( 2 );
				else
					toAdd = "<" + nameSpace + toAdd.Substring( 1 );
			}

			answer = toAdd;

			// Begin assembling the full appended text.  First, add any obligatory new line...
			if( addIndent > -1 )
				answer += "\n" + AppendSpacesToXML( addIndent );


			//tx.Substring( pointA, pointB - pointA );
			return answer;

		}

		// *******************************************************************************************************************
		//   The following function(s) are instrumental for reading files.
		// *******************************************************************************************************************


		/// <summary>
		/// Returns the current directory, with "\bin\Debug" and "\bin\Release" stripped in case the application is running
		/// in a debug environment.
		/// </summary>
		/// <returns>The current directory, with "\bin\Debug" and "\bin\Release" stripped</returns>
		public static string GetBasePath()
		{
			// Strips "\bin\Debug" and "\bin\Release" from current directory when determining when to load/save data files

			if( CurDir.Substring( CurDir.Length - 5, 5 ) == "Debug" )
				return CurDir.Substring( 0, CurDir.Length - 10 );
			else if( CurDir.Substring( CurDir.Length - 7, 7 ) == "Release" )
				return CurDir.Substring( 0, CurDir.Length - 12 );
			else
				return CurDir;

		}


		/// <summary>
		/// This function takes a file's full pathname, and returns the file name only (no folder structure).
		/// </summary>
		/// <param name="fileName">The name of a file, with path information intact</param>
		/// <returns>The name of the file, with path information removed</returns>
		private string ParseFileName( string fileName )
		{
			if( fileName == null ) { return null; }

			char[] delimiterChars = { '\\' };
			string[] words = fileName.Split( delimiterChars );
			return words[ words.Length - 1 ];
		}


		/// <summary>
		/// This function takes a file's full pathname, and returns the file's extension only (no folder structure or rest of the filename)
		/// </summary>
		/// <param name="fileName">The name of a file, with path information intact</param>
		/// <returns>The file's extension (after the .)</returns>
		private string ParseFileExt( string fileName )
		{
			// This function takes a file's full pathname, and returns the file's extension only 
			//   (no folder structure, no rest of the filename)

			char[] delimiterChars = { '\\', '.' };
			string[] words = fileName.Split( delimiterChars );
			return words[ words.Length - 1 ];
		}


		// *******************************************************************************************************************
		//   The following functions make Undo functionality work
		// *******************************************************************************************************************

		private void UndoReset()
		{
			UndoSteps.Clear();
			MENU_Formula_Undo.Enabled = false;
		}


		private void UndoAddStep()
		{
			if( !UndoLock ) {
				int prevUndo = UndoSteps.Count() - 1;
				if( ( prevUndo == -1 ) || ( XmlText != UndoSteps[ prevUndo ] ) ) {
					UndoSteps.Add( XmlText );
					MENU_Formula_Undo.Enabled = true;
				}
			}
		}


		private void UndoExecute()
		{
			int toUndo = UndoSteps.Count() - 1;

			if( toUndo > 0 ) {
				ConstructFormulaFromMathmlString( UndoSteps[ toUndo - 1 ] );
				UpdateMathmlRenderer();

				UndoSteps.RemoveAt( toUndo );
			}
			else if( toUndo == 0 ) {
				if ( MessageBox.Show( "Do you want to clear the formula?", "WAVES-Token-Toolbar", MessageBoxButtons.YesNo, MessageBoxIcon.Question ) == DialogResult.Yes )
					ClearFormula();
			}
		}

		

		// *******************************************************************************************************************
		//   The following functions are instrumental for reading MathML.
		// *******************************************************************************************************************


		/// <summary>
		/// Reads the formula currently rendered in the MathML renderer.  If that formula is invalid, the reading is aborted.
		/// </summary>
		private void ReadXmlFormula()
		{
			// If there is a problem with the formula, abort the reading
			if( LABEL_Warning.Visible == true )
				return;

			// This function sets up the reading of the entire MathML formula, and then reads it.
			PromptBuilder p = new PromptBuilder();

			// Append the Prompt Builders constructed from concatenating all nodes within the 
			//   initial <math> element
			for( int i = 0; i < MATHML_Display.MathElement.Arguments.Count; i++ ) {
				p.AppendPromptBuilder( ReadNode( MATHML_Display.MathElement.Arguments.Item( i ) ) );
			}

			// ... and, finally, read the MathML contents.
			StopSpeech();
			Speaker.SpeakAsync( p );
		} // Read XML Formula


		/// <summary>
		/// This function reads all text from within an interior node of the MathML construct.
		/// </summary>
		/// <param name="node">The node to recursively process ("0" would read the entire formula)</param>
		/// <returns>A PromptBuilder representing a spoken rendering of the specified MathML node</returns>
		private PromptBuilder ReadNode( System.Xml.XmlNode node )
		{
			// This function reads all text from within an interior node of the MathML construct.  It
			//   is designed to recursively iterate on all non-trivial MathML elements.  In other words,
			//   it reads all <mo>, <mi>, <mn>, <mtext>, and raw text within the node, but recursively
			//   iterates on all children of this node that are not any of the named tags (like <mroot>,
			//   <mfrac>, <msubsup>, etc.)

			TimeSpan shortPause = new TimeSpan( 0, 0, 0, 0, 50 );
			TimeSpan longPause = new TimeSpan( 0, 0, 0, 0, 250 );

			PromptBuilder p = new PromptBuilder();
			p.AppendText( " " );

			// Return an empty PromptBuilder if the node is empty
			if( node == null )
				return p;

			// These elements need to be parsed
			//if (node.Name == "mrow") p.AppendPromptBuilder(ReadNode(node, mo, mi));
			if( node.Name == "mfrac" ) {
				// Fraction
				p.AppendBreak( shortPause );
				p.AppendText( " Start fraction " );
				p.AppendPromptBuilder( ReadNode( node.ChildNodes.Item( 0 ) ) );
				p.AppendBreak( shortPause );

				p.AppendText( " over " );
				p.AppendPromptBuilder( ReadNode( node.ChildNodes.Item( 1 ) ) );
				p.AppendBreak( shortPause );

				p.AppendText( " End fraction " );
				p.AppendBreak( longPause );
				return p;
			}
			if( node.Name == "msqrt" ) {
				// Square Root
				p.AppendBreak( shortPause );
				p.AppendText( " The square root of " );
				p.AppendPromptBuilder( ReadNode( node.ChildNodes.Item( 0 ) ) );
				p.AppendBreak( shortPause );

				p.AppendText( " End Square root " );
				p.AppendBreak( longPause );
				return p;
			}
			if( node.Name == "mroot" ) {
				// Generic Root
				PromptBuilder rootValue = new PromptBuilder();

				p.AppendBreak( shortPause );
				p.AppendText( " Thee " );

				// Read Radical may stylize the radical (square, cube, first, twelfth, etc.)
				rootValue = ReadRadical( node.ChildNodes.Item( 1 ) );

				p.AppendPromptBuilder( rootValue );
				p.AppendBreak( shortPause );

				p.AppendText( " Root of " );
				p.AppendPromptBuilder( ReadNode( node.ChildNodes.Item( 0 ) ) );
				p.AppendBreak( shortPause );

				p.AppendText( " End root " );
				p.AppendBreak( longPause );
				return p;
			}
			if( node.Name == "msub" ) {
				// Subscript
				p.AppendBreak( shortPause );
				p.AppendText( " Subscripted text " );
				p.AppendPromptBuilder( ReadNode( node.ChildNodes.Item( 0 ) ) );
				p.AppendBreak( shortPause );

				p.AppendText( " Subscript " );
				p.AppendPromptBuilder( ReadNode( node.ChildNodes.Item( 1 ) ) );
				p.AppendBreak( shortPause );

				p.AppendText( " End subscript " );
				p.AppendBreak( longPause );
				return p;
			}
			if( node.Name == "msup" ) {
				// Superscript
				if( MENU_Options_ReadAsPower.Checked ) {
					// Read as a power
					bool isBaseSimple = TestForSimpleExpression( node.ChildNodes.Item( 0 ) );

					if( isBaseSimple ) {
						// Read naturally if the base is simple
						p.AppendBreak( shortPause );
						p.AppendPromptBuilder( ReadNode( node.ChildNodes.Item( 0 ) ) );

						p.AppendPromptBuilder( ReadExponent( node.ChildNodes.Item( 1 ) ) );

					}
					else {
						// Otherwise, read more formally
						p.AppendBreak( shortPause );
						p.AppendText( " Power Base" );
						p.AppendPromptBuilder( ReadNode( node.ChildNodes.Item( 0 ) ) );
						p.AppendBreak( shortPause );
						p.AppendPromptBuilder( ReadExponent( node.ChildNodes.Item( 1 ) ) );
						p.AppendText( " End power " );
						p.AppendBreak( longPause );
					}
				}
				else {
					// Read very formally as a superscript
					p.AppendBreak( shortPause );
					p.AppendText( " Superscripted text " );
					p.AppendPromptBuilder( ReadNode( node.ChildNodes.Item( 0 ) ) );
					p.AppendBreak( shortPause );

					p.AppendText( " Superscript " );
					p.AppendPromptBuilder( ReadNode( node.ChildNodes.Item( 1 ) ) );
					p.AppendBreak( shortPause );

					p.AppendText( " End superscript " );
					p.AppendBreak( longPause );
				}
				return p;
			}
			if( node.Name == "msubsup" ) {
				// Subscript/superscript
				p.AppendBreak( shortPause );
				p.AppendText( " Sub and super scripted text " );
				p.AppendPromptBuilder( ReadNode( node.ChildNodes.Item( 0 ) ) );
				p.AppendBreak( shortPause );

				p.AppendText( " Subscript " );
				p.AppendPromptBuilder( ReadNode( node.ChildNodes.Item( 1 ) ) );
				p.AppendBreak( shortPause );

				p.AppendText( " Superscript " );
				p.AppendPromptBuilder( ReadNode( node.ChildNodes.Item( 2 ) ) );
				p.AppendBreak( shortPause );

				p.AppendText( " End superscript " );
				p.AppendBreak( longPause );
				return p;
			}
			if( node.Name == "mover" ) {
				// Over
				p.AppendBreak( shortPause );
				p.AppendText( " Start over text " );
				p.AppendPromptBuilder( ReadNode( node.ChildNodes.Item( 1 ) ) );
				p.AppendBreak( shortPause );

				p.AppendText( " over " );
				p.AppendPromptBuilder( ReadNode( node.ChildNodes.Item( 0 ) ) );
				p.AppendBreak( shortPause );

				p.AppendText( " End over text " );
				p.AppendBreak( longPause );
				return p;
			}
			if( node.Name == "munder" ) {
				// Under
				p.AppendBreak( shortPause );
				p.AppendText( " Start under text " );
				p.AppendPromptBuilder( ReadNode( node.ChildNodes.Item( 1 ) ) );
				p.AppendBreak( shortPause );

				p.AppendText( " under " );
				p.AppendPromptBuilder( ReadNode( node.ChildNodes.Item( 0 ) ) );
				p.AppendBreak( shortPause );

				p.AppendText( " End under text " );
				p.AppendBreak( longPause );
				return p;
			}
			if( node.Name == "munderover" ) {
				// Under/Over
				p.AppendBreak( shortPause );
				p.AppendText( " Start over/under text " );
				p.AppendPromptBuilder( ReadNode( node.ChildNodes.Item( 1 ) ) );
				p.AppendBreak( shortPause );

				p.AppendText( " over " );
				p.AppendPromptBuilder( ReadNode( node.ChildNodes.Item( 2 ) ) );
				p.AppendBreak( shortPause );

				p.AppendText( " under " );
				p.AppendPromptBuilder( ReadNode( node.ChildNodes.Item( 0 ) ) );
				p.AppendBreak( shortPause );

				p.AppendText( " End over/under text " );
				p.AppendBreak( longPause );
				return p;
			}
			if( node.Name == "mfenced" ) {
				// Fenced (a set of numbers/variables)
				if( ( node.PreviousSibling != null ) && ( node.PreviousSibling.Name == "mi" ) ) {
					// Read as a Function parameter set
					p.AppendBreak( shortPause );
					p.AppendText( " of " );

					int counter = 1;
					foreach( XmlNode n in node.ChildNodes ) {
						p.AppendPromptBuilder( ReadNode( n ) );
						if ( counter++ < node.ChildNodes.Count ) 
							p.AppendText( " and " );
						//counter++;
						p.AppendBreak( shortPause );
					}

					if ( node.ChildNodes.Count > 1 )
						p.AppendText( " end parameters " );

					p.AppendBreak( longPause );
					return p;
				}
				else {
					// Read as an ordinary set
					p.AppendBreak( shortPause );
					p.AppendText( " Open set " );

					foreach( XmlNode n in node.ChildNodes ) {
						p.AppendPromptBuilder( ReadNode( n ) );
						p.AppendBreak( longPause - shortPause );
					}

					p.AppendText( " Close set " );
					p.AppendBreak( longPause );
					return p;
				}
			}



			for( int i = 0; i < node.ChildNodes.Count; i++ ) {
				System.Xml.XmlNode readNode = node.ChildNodes.Item( i );

				if( readNode.Name == "#text" ) {
					p.AppendText( " " + readNode.InnerText + " " );                              // This is straight text
					p.AppendBreak( shortPause );
				}

				if( readNode.Name == "mn" )
					p.AppendText( readNode.InnerText + " " );    // This is a number
				if( readNode.Name == "mo" )                                                     // This is an operator
                {
					// Must search master list for its pronunciation
					for( int j = 0; j < ( MasterToken._moCount ); j++ ) {
						Token t = MasterToken._mo[ j ];
						if( readNode.InnerText == t._symbol ) {
							// Barf.  Need this hack because Invisible Plus does not render correctly... so it looks like Invisible Times.
							if( t._symbol == "⁢" ) {			// If t.symbol is Invisible Times, check if it should be Invisible Plus
								if( ( readNode.PreviousSibling.Name == "mn" ) & ( readNode.NextSibling.Name == "mfrac" ) ) {
									// Ah.  In this case, find how to say Invisible Plus
									for( int k = 0; k < MasterToken._moCount; k++ )
										if( MasterToken._mo[ k ]._symbol == "⁤" )
											p.AppendText( MasterToken._mo[ k ]._speech + " " );
								}
								else
									p.AppendText( t._speech + " " );
							}
							else
								p.AppendText( t._speech + " " );
						}
					} // Check the mo group
				}
				if( readNode.Name == "mi" )                                                      // This is a letter or symbol
                {
					bool speakMi = true;
					
					// Check for symbols first... if not found in the master list, the token will be pronouced as a letter
					for( int j = 0; j < ( MasterToken._miCount ); j++ ) {
						Token t = MasterToken._mi[ j ];
						if( readNode.InnerText == t._symbol ) {
							p.AppendText( t._speech + " " );
							speakMi = false;
							break;
						}
					} // Check the mi group   

					if( speakMi )
						p.AppendTextWithHint( Convert.ToString( readNode.InnerText ) + " ", SayAs.SpellOut );

				}

				// Basically, if this node is not named one of these things, then it should be
				//   read now.  This means <mrow> and <mtext> elements.
				if( ( readNode.Name != "mi" ) && ( readNode.Name != "mo" ) && ( readNode.Name != "mn" ) &&
					 ( readNode.Name != "#text" ) )
					p.AppendPromptBuilder( ReadNode( readNode ) );

			}

			// Make sure that a trailing space is appended to the end of an <mrow> element
			if( node.Name == "mrow" )
				p.AppendText( " " );

			return p;
		} // Read Node (recursive)


		/// <summary>
		/// Used to determine whether this node represents a complex or a simple expression (true if simple, false if complex)
		/// </summary>
		/// <param name="node">The node to test</param>
		/// <returns>Returns True for simple, False for complex</returns>
		private bool TestForSimpleExpression( System.Xml.XmlNode node )
		{
			bool answer = true;

			for( int i = 0; i < node.ChildNodes.Count; i++ )
				if( ( node.ChildNodes[ i ].Name != "mi" ) && ( node.ChildNodes[ i ].Name != "mn" ) )
					answer = false;

			return answer;
		}


		/// <summary>
		/// If superscripts are read as powers, this function is used to "read" an exponent.
		/// </summary>
		/// <param name="node">The node that represents the exponent of a power (2nd "mrow" argument of "msup" MathML element)</param>
		/// <returns>Could be "squared", "cubed", "to the n power", or "exponent ... end exponent"</returns>
		private PromptBuilder ReadExponent( System.Xml.XmlNode node )
		{
			PromptBuilder p = new PromptBuilder();
			TimeSpan shortPause = new TimeSpan( 0, 0, 0, 0, 50 );

			if( ( node.ChildNodes.Count == 1 ) && ( node.ChildNodes[ 0 ].Name == "mn" ) ) {
				if( node.ChildNodes[ 0 ].InnerText == "2" ) {
					// Squared
					p.AppendText( " squared " );
				}
				else if( node.ChildNodes[ 0 ].InnerText == "3" ) {
					// Cubed
					p.AppendText( " cubed " );
				}
				else if( node.ChildNodes[ 0 ].InnerText.Contains( '.' ) ) {
					// to the x power
					p.AppendText( " to the " );
					p.AppendPromptBuilder( ReadNode( node ) );
					p.AppendText( " power " );
				}
				else {
					// to the n-th power

					string wordToRead = AddNumericalSuffix( node.ChildNodes[ 0 ].InnerText );

					p.AppendText( " to the " );
					p.AppendText( wordToRead );
					p.AppendText( " power " );
				}
			}
			else if( TestForSimpleExpression( node ) ) {
				// Variable exponent... to the x power
				p.AppendText( " to the " );
				p.AppendPromptBuilder( ReadNode( node ) );
				p.AppendText( " power " );
			}
			else {
				// Complex exponent
				p.AppendText( " exponent " );
				p.AppendPromptBuilder( ReadNode( node ) );
				p.AppendText( " end exponent " );
			}
			p.AppendBreak( shortPause );

			return p;
		}


		/// <summary>
		/// This function is used to read a radical.
		/// </summary>
		/// <param name="node">The node that represents the radical portion of a "mroot" MathML element</param>
		/// <returns>Could be "square", "cube", "n-th", or "x"</returns>
		private PromptBuilder ReadRadical( System.Xml.XmlNode node )
		{
			PromptBuilder p = new PromptBuilder();
			TimeSpan shortPause = new TimeSpan( 0, 0, 0, 0, 50 );

			if( ( node.ChildNodes.Count == 1 ) && ( node.ChildNodes[ 0 ].Name == "mn" ) ) {
				if( node.ChildNodes[ 0 ].InnerText == "2" ) {
					// Squared
					p.AppendText( " squared " );
				}
				else if( node.ChildNodes[ 0 ].InnerText == "3" ) {
					// Cubed
					p.AppendText( " cube " );
				}
				else if( node.ChildNodes[ 0 ].InnerText.Contains( '.' ) ) {
					// Contains a decimal... just read it
					p.AppendPromptBuilder( ReadNode( node ) );
				}
				else {
					// to the n-th power

					string wordToRead = AddNumericalSuffix( node.ChildNodes[ 0 ].InnerText );

					p.AppendText( wordToRead );
				}
			}
			else {
				// If not a simple number, simply read the node
				p.AppendPromptBuilder( ReadNode( node ) );
			}

			return p;
		}


		/// <summary>
		/// Intelligently adds the correct suffix to a stringified integer.
		/// </summary>
		/// <param name="number">A string representing an integer ( as in "int.ToString()" )</param>
		/// <returns>The integer, with the appropriate suffix added ("-st", "-nd", "-rd", or "-th")</returns>
		private string AddNumericalSuffix( string number )
		{
			// Intelligently add the correct suffix to a stringified number
			string suffix = "th";

			// Numbers that end in a "1" have a "st" suffix, but only if not an "11"
			if( number.Substring( number.Length - 1 ) == "1" )
				if( number.Length > 1 )
					if( number.Substring( number.Length - 2 ) != "11" )
						suffix = "st";

			// Numbers that end in a "2" have an "nd" suffix, but only if not a "12"
			if( number.Substring( number.Length - 1 ) == "2" )
				if( number.Length > 1 )
					if( number.Substring( number.Length - 2 ) != "12" )
						suffix = "nd";

			// Numbers that end in a "3" have an "nd" suffix, but only if not a "13"
			if( number.Substring( number.Length - 1 ) == "3" )
				if( number.Length > 1 )
					if( number.Substring( number.Length - 2 ) != "13" )
						suffix = "rd";

			return ( number + suffix );
		}


		/// <summary>
		/// This function reads an excerpt of MathML from "LISTBOX_ShowExcerpts".
		/// </summary>
		/// <param name="excerpt">The ID of the MathML excerpt to read</param>
		/// <param name="goDeeper">True to read ID tags recursively, false to read ID tags woodenly</param>
		/// <returns>A PromptBuilder representing a spoken rendering of the specified math excerpt</returns>
		private PromptBuilder ReadText( int excerpt, bool goDeeper )
		{
			PromptBuilder p = new PromptBuilder();

			string excerptAndPrompt = LISTBOX_ShowExcerpts.Items[ excerpt ].ToString();
			string excerptTag = GetXmlWrapper( excerptAndPrompt );
			//excerptTag = excerptTag.Substring( 6 );

			string tx = RetrieveExcerptFromListbox( excerptAndPrompt );

			Char[] c = tx.ToCharArray();

			bool speakMiChar = true;


			// These checks will determine if extra words are needed to delineate that we are in a 
			//   special mathML tag
			bool spokeTag = false;
			string textExcerpt = "";

			for( int j = 1; j < ( MasterToken._elemCount ); j++ ) {
				Token t = MasterToken._mathMLelement[ j ];
				if( excerptTag == t._symbol ) {
					p.AppendText( t._speech );
					spokeTag = true;
				}
				if( t._symbol == "mtext" )
					textExcerpt = MasterToken._elemEnglish[ j ][ 0 ];
			} // Check the mo group

			if( !spokeTag )
				p.AppendText( excerptTag );

			p.AppendBreak( new TimeSpan( 0, 0, 0, 0, 250 ) );

			// Check for Text tag; should be read literally
			if( ( excerptTag == "mtext" ) | ( excerptTag == textExcerpt ) ) {
				p.AppendText( tx );
				return p;
			}

			// This function will convert the text in the Textbox to speech
			for( int i = 0; i < ( tx.Length ); i++ ) {
				// Letters convert to an <m:mi> tag
				if( Char.IsLetter( c[ i ] ) & ( Convert.ToInt32( c[ i ] ) < 256 ) ) {			// Greek letters are considered letters!
					// First, check if this letter begins a function name!
					speakMiChar = true;

					for( int j = 0; j < ( MasterToken._miCount ); j++ ) {
						Token t = MasterToken._mi[ j ] as Token;
						string sym = t._symbol;

						if( sym.Length <= ( tx.Length - i ) ) {
							if( tx.Substring( i, sym.Length ) == sym ) {
								p.AppendText( t._speech + " " );
								i = i + sym.Length - 1;
								speakMiChar = false;
								break;
							}
						}
					} // Check the mi group   

					if( speakMiChar ) 
						p.AppendTextWithHint( Convert.ToString( c[ i ] ) + " ", SayAs.SpellOut );
				} // Letter check

				if( Char.IsDigit( c[ i ] ) || c[ i ] == '.' ) {
					int startNum = i;
					int finNum = i;

					for( finNum = i; finNum < tx.Length; finNum++ )
						if( !Char.IsDigit( c[ finNum ] ) && c[ finNum ] != '.' && c[ finNum ] != ',' )
							break;

					string sayNum = "";

					for( int k = i; k < finNum; k++ )
						sayNum += c[ k ];

					sayNum = Convert.ToDouble( sayNum ).ToString();
					p.AppendTextWithHint( sayNum + " ", SayAs.Text );

					i = finNum - 1;
				} // check for Numbers

				if( i < tx.Length ) {

					// Otherwise, must find the speech text in the Master Token List XML
					int j = 0;
					for( j = 0; j < ( MasterToken._moCount ); j++ ) {
						Token t = MasterToken._mo[ j ] as Token;
						Char[] sym = t._symbol.ToCharArray();
						if( c[ i ] == sym[ 0 ] )
							p.AppendText( t._speech + " " );
					} // Check the mo group

					if( speakMiChar ) {
						for( j = 0; j < ( MasterToken._miCount ); j++ ) {
							Token t = MasterToken._mi[ j ] as Token;
							string sym = t._symbol;
							if( ( i + sym.Length ) < tx.Length ) 
								if( tx.Substring( i, sym.Length ) == sym )
									p.AppendText( t._speech + " " );
						} // Check the mi group   
					}


					// ⊰- Finally, we need to test for Excerpt tags!
					if( c[ i ] == '⊰' ) {
						int thisID = Convert.ToInt32( tx.Substring( i + 1, 3 ) );

						if( goDeeper == true ) {
							PromptBuilder appendThis = new PromptBuilder();
							appendThis = ReadText( thisID, true );
							p.AppendPromptBuilder( appendThis );
						}
						else {
							p.AppendText( "Line " );
							p.AppendText( thisID.ToString() + " " );
						}
						i = i + 4;

					} // Check for excerpt tags // -⊱                       

					p.AppendBreak( new TimeSpan( 0, 0, 0, 0, 125 ) );
				}

			}

			return p;
		} // Read One MathML Excerpt


		/// <summary>
		/// This function stops the Speaker from saying whatever was being said.  This improves responsiveness of voice prompting and formula reading.
		/// </summary>
		private void StopSpeech()
		{
			Speaker.SpeakAsyncCancelAll();
			Speaker.Rate = 1;
			Speaker.Volume = 100;
		}



		// *******************************************************************************************************************
		//   Functions that complete the tasks that special keypresses/buttons accomplish
		// *******************************************************************************************************************

		/// <summary>
		/// This function shows or hides the Level 2 tokens from the user. 
		/// </summary>
		private void ToggleLevel2()
		{
			if ( Level == 1 ) {
				// Make all Level 2 tokens visible
				for( int i = ( NUM_CONTROLS + NumElements + NumLevel1 ); i < ( NUM_CONTROLS + NumElements + NumLevel1 + NumLevel2 ); i++ )
					Controls[ i ].Visible = true;
				Level = 2;
			}
			else {
				// Make all Level 2 tokens invisible
				for( int i = ( NUM_CONTROLS + NumElements + NumLevel1 ); i < ( NUM_CONTROLS + NumElements + NumLevel1 + NumLevel2 ); i++ )
					Controls[ i ].Visible = false;
				Level = 1;
			}
		} // Toggle Level 2


		/// <summary>
		/// This function opens or closes the token Repository window. 
		/// </summary>
		private void ToggleLevel3()
		{
			// For help on getting Level3's button clicks to register in this form's Textbox...
			//   http://social.msdn.microsoft.com/Forums/en-US/csharplanguage/thread/46408a73-045f-4d61-a43c-c3e2aa99b049/

			try {
				if( TokenRepository.Visible == true ) {
					// This code executes if the Repository is visible
					TokenRepository.Close();
					TokenRepository = null;
				}
				else {
					// This code will dispose properly of a closed Repository, and trigger a re-showing (creates an exception)
					TokenRepository = null;
					TokenRepository.Visible = true;
				}
			}
			catch {
				// This code shows the Repository
				TokenRepository = new Level3();
				TokenRepository.Owner = ( System.Windows.Forms.Form )this;
				TokenRepository.Location = new Point( MousePosition.X, MousePosition.Y );
				TokenRepository.Show();
			}
		} // Toggle Level 3


		private void MENU_SpeakIdentity( object sender, PaintEventArgs e )
		{

		}

		private void MENU_bar_Enter( object sender, EventArgs e )
		{
			// This event never fires when the keyboard enters the menu...
			//int a = 0;
		}

		private void MENU_bar_MenuActivate( object sender, EventArgs e )
		{
			// This event does, but I'm not sure how to get individual menu items to read out their function...
			//MessageBox.Show("You are in the MenuStrip.MenuActivate event.");
		}

		

		





		// *******************************************************************************************************************
		//   These are functions that are not needed in this release
		// *******************************************************************************************************************


		//        void sre_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
		//        {
		// Execution never gets here.  I don't know why.
		//            MessageBox.Show("Speech recognized: " + e.Result.Text);
		//        } // Speech recognition event!

		//        private void RepopulateListBox(int dim)
		//        {
		//return;
		//            List<string> XMLpieceCopy = new List<string>();
		//            XMLpieceCopy.Clear();
		//            foreach (string s in xmlPiece)
		//                XMLpieceCopy.Add(s);

		//            LISTBOX_XMLpieces.Items.Clear();
		//            xmlPiece.Clear();

		//            for (int i = 0; i < dim; i++)
		//            {
		//                xmlPiece.Add(XMLpieceCopy[i]);
		//                LISTBOX_XMLpieces.Items.Add(TagifyID(i) + "-<" + GetXmlWrapper(XMLpieceCopy[i]) + ">  " + DeXml(XMLpieceCopy[i]));
		//            }


		//xmlPiece.Clear();
		//xmlPiece.Add("<mrow>@1<mo>=</mo><mn>31</mn><mi>π</mi></mrow>");
		//xmlPiece.Add("<mroot>@2@3</mroot>");
		//xmlPiece.Add("<mfrac>@4@5</mfrac>");
		//xmlPiece.Add("<mn>8</mn>");
		//xmlPiece.Add("<mrow><mn>2</mn><mi>x</mi><mo>+</mo><mn>7</mn><mi>y</mi></mrow>");
		//xmlPiece.Add("<mn>98</mn>");

		//string l = ParseMathList(0);
		//XmlText = l;

		//        } //RepopulateListBox



		//        private ArrayList loadTokenXML(string filename, int lv)
		//        {
		// This function retrieves all tokens of a specified Level from elevated-tokens.xml.

		// Retrieve all "selections" value directly, that will retrieve the token symbols
		//            RetrieveData[] rd;
		//            myXML.initStructure(out rd, 1); 
		//            rd[0].SetData(false, "");

		// Load the specified Level of token into an array of "symbols" which are considered "Elevated Token"s
		//ArrayList output = myXML.loadTokenXML("D:\\Work\\C#\\Prototype-Token-Interface\\Prototype-Token-Interface\\elevated-tokens.xml", "Level-" + lv.ToString());
		//            ArrayList output = myXML.loadXML(CurDir + "\\elevated-tokens.xml", "/all/Level-" + lv.ToString() + "/symbol", rd);

		//            return output;
		//        }  // loadTokenXML function



		//        private void Button_AddToken_Enter(object sender, EventArgs e)
		//        {
		// This event is bound to the "Add Token" button on the form.
		//   Its purpose is to speak the button's function, as well as the token selected to be added.
		//            if (IsSpaceLocked())        return;

		//            string s = COMBOBOX_OtherTags.SelectedItem.ToString();

		//            foreach (Token t in Master_Token_List.MathMLelement)
		//            {
		//                if (s == t.symbol) s = t.speech;
		//            }

		//            PromptBuilder p = new PromptBuilder();

		//            p.AppendText("Add ");
		//            p.AppendBreak(new TimeSpan(0, 0, 0, 0, 10));
		//            p.AppendText("M ");
		//            p.AppendBreak(new TimeSpan(0, 0, 0, 0, 50));
		//            p.AppendText(s);
		//            p.AppendBreak(new TimeSpan(0, 0, 0, 0, 50));
		//            p.AppendText(" token ");

		//s = "Add " + s + " token";

		//            StopSpeech();
		//            Speaker.SpeakAsync(p); 
		//        } // Entered Add Tag Button, either by mouse or keyboard


		//        private void COMBOBOX_TagSelect(object sender, EventArgs e)
		//        {
		// This event is bound to the "Token select" ComboBox on the form.
		//   Its purpose is to speak which token is currently selected.

		//            string s = COMBOBOX_OtherTags.SelectedItem.ToString();

		//            foreach (Token t in Master_Token_List.MathMLelement)
		//            {
		//                if (s == t.symbol) s = t.speech;
		//            }

		//            PromptBuilder p = new PromptBuilder();

		//            p.AppendText("M ");
		//            p.AppendBreak(new TimeSpan(0, 0, 0, 0, 25));
		//            p.AppendText(s);
		//            p.AppendBreak(new TimeSpan(0, 0, 0, 0, 75));
		//            p.AppendText(" token selected! ");

		//s = "Add " + s + " token";

		//            StopSpeech();
		//            Speaker.SpeakAsync(p);
		//        } // Changed or Entered Add Tag Combobox, either by mouse or keyboard



//		private string TagifyID( int i )
//		{
			//			string answer = i.ToString();
			//			if( i < 100 )
			//				answer = "0" + answer;
			//			if( i < 10 )
			//				answer = "0" + answer;
			//			answer = "⊰" + answer + "⊱";
			//			return answer;
//		} // TagifyID


//		private string ParseMathList( int ind )
//		{
//			string str = xmlPiece[ ind ];
//			
//			for( int i = 0; i < str.Length; i++ ) {
//				if( str.Substring( i, 1 ) == "⊰" ) {
//					// Transform the Tag into an integer (ID)
//					i++;
//					string replaceStr = str.Substring( i, 3 );
//					int nextCall = Convert.ToInt32( replaceStr );
//					string addStr = ParseMathList( nextCall );
//
//					// Replace all ID tags with the MathML that they represent
//					str = str.Replace( "⊰" + replaceStr + "⊱", addStr );
//				}
//			}
//
//			return str;
//		} // Parse Math List (recursive)


//		private string CutifyMathML( string tx )
//		{
			// This code block will make the MathML "cute"... 
			//   it will add New Lines and indentation to make it more readable

//			string cutifyXML = "";
//			int curIndent = 0;
//			int indentIncrement = 2;
//			bool needNewLine = false;

//			int pos = tx.IndexOf( '<', 0 );

//			while( pos > -1 ) {
//				int newPos = tx.IndexOf( '<', pos + 1 );
//				string test = tx.Substring( pos, 4 );

//				if( test.Substring( 0, 2 ) == "</" ) {
					// This is a closing tag; check for a closing major tag here.  An closing tag is major if it is NOT <mo>, <mi>, or <mn>

//					test += tx.Substring( pos + 4, 1 );
//					if( ( test != "</mo>" ) & ( test != "</mi>" ) & ( test != "</mn>" ) ) {
						// This is closing major tag
//						if( needNewLine ) {
//							curIndent -= indentIncrement;
//							cutifyXML += AppendTagToXML( tx, pos, newPos, curIndent );
//						}
//						else
//							cutifyXML += AppendTagToXML( tx, pos, newPos, -1 );

						//cutifyXML += "\n" + AppendSpacesToXML( curIndent );
//					}
//					else {
//						cutifyXML += AppendTagToXML( tx, pos, newPos, -1 );
//					}

//					needNewLine = true;
//				}
//				else {
					// This is an opening tag; check for a opening major tag here.  An opening tag is major if it is NOT <mo>, <mi>, or <mn>

//					if( ( test != "<mo>" ) & ( test != "<mi>" ) & ( test != "<mn>" ) ) {
						// This is an opening major tag
//						if( needNewLine ) {
//							cutifyXML += AppendTagToXML( tx, pos, newPos, curIndent );
//							needNewLine = false;
//						}
//						else
//							cutifyXML += AppendTagToXML( tx, pos, newPos, -1 );

//						curIndent += indentIncrement;
//						cutifyXML += "\n" + AppendSpacesToXML( curIndent );
//					}
//					else {
//						cutifyXML += AppendTagToXML( tx, pos, newPos, -1 );
//						needNewLine = true;
//					}
//				}

//				pos = newPos;
//			}

//			return cutifyXML;

			//			for( int i = 0; i < tx.Length; i++ ) {
			// Test for the ending tag
			// The "try" is here because if we overflow the bound of XmlText, that's okay
			//				try {
			//					if( tx.Substring( i, 7 ) == "</math>" ) {
			//						cutifyXML += "\n";
			//					}
			//				}
			//				catch { }

			// Check for a major tag here
			//				foreach( Token t in MasterToken._mathMLelement ) {
			// The "try" is here because if we overflow the bound of XmlText, that's okay
			//					try {
			// If opened...
			//						if( tx.Substring( i, t._symbol.Length + 2 ) == "<" + t._symbol + ">" ) {
			//							cutifyXML += "\n";
			//							for( int j = 0; j < curIndent; j++ )
			//								cutifyXML += " ";
			//							curIndent = curIndent + indentIncrement;
			//						}

			// If closed...
			//						if( tx.Substring( i, t._symbol.Length + 3 ) == "</" + t._symbol + ">" ) {
			//							cutifyXML += "\n";
			//							curIndent = curIndent - indentIncrement;
			//							for( int j = 0; j < curIndent; j++ )
			//								cutifyXML += " ";
			//						}
			//					}
			//					catch { }
			//				}

			//				cutifyXML += tx.Substring( i, 1 );
			//			}

			//			return cutifyXML;
//		} // function CutifyMathML... adds indentation to MathML for copying


		/// <summary>
		/// This function shows or hides the Level 2 tokens from the user. 
		/// </summary>
//		private void ToggleLevel2()
//		{
			// If the window is 184/160 pixels high, then it needs to be opened.
			//			if( Height == WavesHeight ) {
//			if( Level == 1 ) {
				// Open Level 2
				//				for( int i = 0; i < 40; i++ ) {
				//					Height = WavesHeight + i;
				//					LISTBOX_ShowExcerpts.Height = WavesHeight - 66 + i;
				//				}
				//				Height = WavesHeight + 40;
				//				LISTBOX_ShowExcerpts.Height = WavesHeight - 50;

				// Make all Level 2 tokens visible
//				for( int i = ( NUM_CONTROLS + NumElements + NumLevel1 ); i < ( NUM_CONTROLS + NumElements + NumLevel1 + NumLevel2 ); i++ )
//					Controls[ i ].Visible = true;

				//				BUTTON_ShowLevel2.Text = "Level 2 ↑";
				// Also, make the first Level 2 token button the active control (clicked if user presses "Enter")
				//				ActiveControl = Controls[ NUM_CONTROLS + NumLevel1 ];

//				Level = 2;
//			}
//			else
			// Otherwise, close it back to the original height.
//            {
				// Close Level 2
				//				for( int i = 40; i > 0; i-- ) {
				//					Height = WavesHeight + i;
				//					LISTBOX_ShowExcerpts.Height = WavesHeight - 66 + i;
				//				}
				//				Height = WavesHeight;
				//				LISTBOX_ShowExcerpts.Height = WavesHeight - 90;

				// Make the first Level 1 token the active control
				//				ActiveControl = Controls[ NUM_CONTROLS ];

				//				BUTTON_ShowLevel2.Text = "Level 2 ↓";
				// Make all Level 2 tokens invisible
//				for( int i = ( NUM_CONTROLS + NumElements + NumLevel1 ); i < ( NUM_CONTROLS + NumElements + NumLevel1 + NumLevel2 ); i++ )
//					Controls[ i ].Visible = false;

//				Level = 1;
//			}
//		} // Toggle Level 2


		//        private void BUTTON_ShowLevel2_Click(object sender, EventArgs e)
		//        {
		// Toggles open/closed the Level 2 tokens (on the bottom of the window)
		//if (IsSpaceLocked()) return;

		//ToggleLevel2();
		//        } // Show Level 2 Button click event


		//        private void BUTTON_ShowLevel3_Click(object sender, EventArgs e)
		//        {
		// Toggles open/closed the Token Repository which contains all <mo> and <mi> tokens in
		//   the master list.
		//            if (IsSpaceLocked()) return;

		//            ToggleLevel3();
		//        } // Show Level 3 Button click event


		//        private void BUTTON_CopyXML_Click(object sender, EventArgs e)
		//        {
		// This button copies the MathML representation of the displayed formula to the Clipboard.
		//   Additionally, it tells the user what got copied.
		//            if (IsSpaceLocked()) return;

		//            string cutifyXML = CutifyMathML(XmlText);

		//            if (cutifyXML != "")
		//            {
		//                MessageBox.Show("XML copied to Clipboard:\n\n" + cutifyXML,
		//                  Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
		//                Clipboard.SetText(cutifyXML);
		//            }

		//        } // Copy Text as XML Button click event


		//        private void BUTTON_ClearText_Click(object sender, EventArgs e)
		//        {
		// Clears the current MathML excerpt.  
		//            if (IsSpaceLocked()) return;

		//            TEXTBOX_MathExpression.Text = "";
		//XmlText = "";

		//        } // Clear Text Button click event


		//        private void BUTTON_ClearFormula_Click(object sender, EventArgs e)
		//        {
		// Clears the entire MathML Formula (all excerpts).
		//            if (IsSpaceLocked()) return;

		//            ClearFormula();
		//        } // Clear Formula Button click event


		//        private void BUTTON_ReadText_Click(object sender, EventArgs e)
		//        {
		// Reads the whole MathML formula to the user.
		//            if (IsSpaceLocked()) return;

		//            ReadXmlFormula();
		//        }


		//        private void BUTTON_AddOtherTag_Click(object sender, EventArgs e)
		//        {
		//            // This adds a complex MathML element to the formula.
		//            if (IsSpaceLocked())        return;

		//            string tag = COMBOBOX_OtherTags.SelectedItem.ToString();
		//            int args = NumTagArgs[COMBOBOX_OtherTags.SelectedIndex];

		//            int jumpHere = AddTagToFormula(tag, args, EditXmlExcerpt, true);
		//            EditXmlExcerpt = jumpHere;
		//            LISTBOX_XMLpieces.SelectedIndex = jumpHere;
		//        } // Add Other Tag Button click event





		// *******************************************************************************************************************
		//   These are test functions
		// *******************************************************************************************************************


		//        private void outputToken()
		//        {
		//            ArrayList output = loadTokenXML("D:\\Work\\C#\\Prototype-Token-Interface\\Prototype-Token-Interface\\master-token-list.xml", "mo");
		//            string s = "mo -- ";

		//            foreach (Token tok in output)
		//            {
		//                s += (tok.symbol + "  ");
		//            }
		//            s += "\nmi -- ";

		//            output = loadTokenXML("D:\\Work\\C#\\Prototype-Token-Interface\\Prototype-Token-Interface\\master-token-list.xml", "mi");

		//            foreach (Token tok in output)
		//            {
		//                s += (tok.symbol + "  ");
		//            }

		//            output = loadTokenXML("D:\\Work\\C#\\Prototype-Token-Interface\\Prototype-Token-Interface\\master-token-list.xml", "other");
		//            s += "\nother -- ";

		//            foreach (Token tok in output)
		//            {
		//                s += (tok.symbol + "  ");
		//            }

		//            MessageBox.Show(s);
		//        }


		// Testing for loadXML functionality
		//        public class TestCase
		//        {
		//            public string id;
		//            public string kind;
		//            public string arg1;
		//            public string arg2;
		//            public string expected;
		//        }

		//        private void outputTest()
		//        {
		//            RetrieveData[] rd = new RetrieveData[5];

		//            rd[0] = RetrieveData.SetData(true, "kind");
		//            rd[1] = RetrieveData.SetData(false, "expected");
		//            rd[2] = RetrieveData.SetData(false, "inputs/arg2");
		//            rd[3] = RetrieveData.SetData(true, "id");
		//            rd[4] = RetrieveData.SetData(false, "inputs/arg1");

		//            ArrayList output = myXML.loadXML("D:\\Work\\C#\\Prototype-Token-Interface\\Prototype-Token-Interface\\test.xml", "/suite/testcase", rd);
		//            string s = "";


		//            foreach (ArrayList a in output)
		//            {
		//                TestCase tc = new TestCase();
		//                string[] st = myXML.explodeXMLdata(a);

		//                tc.kind = st[0];
		//                tc.expected = st[1];
		//                tc.arg2 = st[2];
		//                tc.id = st[3];
		//                tc.arg1 = st[4];

		//                s += (tc.id + ": (" + tc.kind + ") " + tc.arg1 + " + " + tc.arg2 + " = " + tc.expected + "\n");
		////                s += (tc.kind + ") " + tc.arg2 + ", " + tc.expected + "\n");
		//            }
		//            MessageBox.Show(s);
		//        }



		// *******************************************************************************************************************
		//   These functions are relics from before I implemented DOM-walking for loading XMLs only once
		// *******************************************************************************************************************


		//public bool ParseElevatedTokenDOM_old()
		//{
		//	string filename = Token_Toolbar.CurDir + "\\elevated-tokens.xml";

			// First of all, we test to see if the XML file we are going to be parsing exists...
		//	if( System.IO.File.Exists( filename ) ) {
				// Load the XML document
		//		XmlDocument tokenXML = new XmlDocument();
		//		tokenXML.Load( filename );

		//		XmlNode ElementNode = tokenXML.SelectSingleNode( "/all/elements" );
		//		XmlNode Level1Node = tokenXML.SelectSingleNode( "/all/level-1" );
		//		XmlNode Level2Node = tokenXML.SelectSingleNode( "/all/level-2" );

		//		NumElements = ElementNode.ChildNodes.Count;
				//Imp_Element = new string[ NumElements ];

		//		NumLevel1 = Level1Node.ChildNodes.Count;
				//Imp_Level1 = new string[ NumLevel1 ];

		//		NumLevel2 = Level2Node.ChildNodes.Count;
				//Imp_Level2 = new string[ NumLevel2 ];

		//		int id = 0;
		//		foreach( XmlNode node in tokenXML.SelectNodes( "//all//elements//*" ) ) {
		//			Imp_Element[ id ] = node.InnerText;
		//			id++;
		//		}

		//		id = 0;
		//		foreach( XmlNode node in tokenXML.SelectNodes( "//all//level-1//*" ) ) {
		//			Imp_Level1[ id ] = node.InnerText;
		//			id++;
		//		}
				
		//		id = 0;
		//		foreach( XmlNode node in tokenXML.SelectNodes( "//all//level-2//*" ) ) {
		//			Imp_Level2[ id ] = node.InnerText;
		//			id++;
		//		}

				// Alert calling function of success
		//		return true;
		//	}
		//	else {
				// Can't do anything if this xml doesn't exist.  If the file is not found, returns -1 for FAILURE 
		//		ShowError( 2 );
		//		return false;
		//	}

		//} // close function: ParseElevatedTokenDOM



		//        private bool countElevatedTokens()
		//        {
		// This is a brute force method of determining how many tokens are Level 1 and Level 2 tokens.

		// Set the Retrieve data structure appropriately
		//            RetrieveData[] rd;
		//            myXML.initStructure(out rd, 1);
		//            rd[0].SetData(false, "");

		// Load the specified Level of token into an array of "symbols" which are considered "Elevated Token"s.
		// We also check if the file actually exists here.
		//            ArrayList output = myXML.loadXML(CurDir + "\\elevated-tokens.xml", "/all/elements/symbol", rd);
		//            if (output == null)
		//            {
		//                ShowError(2);
		//                Close();
		//                return false;
		//            }

		// If execution reaches here, the file has been read, and the number of elements is determined.
		//            NumElements = output.Count;

		// First, check to see if the Elevated Token xml list even exists.
		//            output = myXML.loadXML(CurDir + "\\elevated-tokens.xml", "/all/Level-1/symbol", rd); 
		//            NumLevel1 = output.Count;

		// Determine number of Level 2 tokens.
		//            output = myXML.loadXML(CurDir + "\\elevated-tokens.xml", "/all/Level-2/symbol", rd);
		//            NumLevel2 = output.Count;

		//            return true;
		//        }




		//        private void PopulateElements(ref int tabStop)
		//        {
		// This function's job is to dynamically create a button on the toolbar for every token
		// specified in the elevated-tokens.xml file.

		//            int dimensionX = 0;

		// Retrieve all "selections" value directly, that will retrieve the token symbols
		//            RetrieveData[] rd;
		//            myXML.initStructure(out rd, 1);
		//            rd[0].SetData(false, "");


		// Load the specified Level of token into an array of "symbols" which are considered "Elevated Token"s
		//            ArrayList output = myXML.loadXML(CurDir + "\\elevated-tokens.xml", "/all/elements/symbol", rd);

		//            for (int i = 0; i < (output.Count); i++)
		//            {
		// For each elevated token, create a button on the toolbar which is captioned by the token
		//                Button button = new Button();

		//                button.Font = new System.Drawing.Font("Lucida Sans Unicode", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

		//ElevatedToken t = output[i] as ElevatedToken;
		//                string[] str = myXML.explodeXMLdata((ArrayList)output[i]);
		//button.Text = str[0];
		//                try
		//                {
		//                    button.Image = Image.FromFile(GetBasePath() + "\\graphics\\elem-" + str[0] + ".ico");
		//                    button.ImageAlign = ContentAlignment.MiddleCenter;
		//                    button.Name = str[0];
		//                }
		//                catch
		//                {
		//                    button.Text = str[0];
		//                    button.Name = str[0];
		//                }

		// Placement information for the new button
		//                int locX = 250 + (50 * i);
		//                int locY = WavesHeight - 125;
		//                if (MENU_bar.Visible == false) locY -= 24;
		//                button.Location = new Point(locX, locY);

		//                button.Width = 40;
		//                button.Height = 40;

		// Event information for the new button.  It takes effect when clicked.
		//               button.Click += new EventHandler(Button_Element_Click);
		//                button.Enter += new EventHandler(Button_Dynamic_Enter);
		//                button.MouseEnter += new EventHandler(Button_Dynamic_Enter);

		//                button.Tag = -1;
		//                for (int j = 0; j < Master_Token_List.elemCount; j++)
		//                {
		//                    if (Master_Token_List.MathMLelement[j].symbol == button.Name) button.Tag = j;
		//                }

		// Assign a Tab Index to this button
		//                button.TabIndex = tabStop;
		//                tabStop++;

		// Add the button to the form
		//                Controls.Add(button);
		//                NUM_CONTROLS++;

		// In case this is the last token, make sure the toolbar window is wide enough to show all buttons
		//                dimensionX = locX + 150;
		//            }

		// Reposition elements and resize window if necessary
		//            RepositionAllControls();
		//        } // PopulateElements function




		//        private void PopulateToolbar(int lv, ref int tabStop)
		//        {
		// This function's job is to dynamically create a button on the toolbar for every token
		// specified in the elevated-tokens.xml file.

		//            int dimensionX = 0;

		// Retrieve all "selections" value directly, that will retrieve the token symbols
		//            RetrieveData[] rd;
		//            myXML.initStructure(out rd, 1);
		//            rd[0].SetData(false, "");


		// Load the specified Level of token into an array of "symbols" which are considered "Elevated Token"s
		//ArrayList output = myXML.loadTokenXML("D:\\Work\\C#\\Prototype-Token-Interface\\Prototype-Token-Interface\\elevated-tokens.xml", "Level-" + lv.ToString());
		//            ArrayList output = myXML.loadXML(CurDir + "\\elevated-tokens.xml", "/all/Level-" + lv.ToString() + "/symbol", rd);

		//            for (int i = 0; i < (output.Count); i++)
		//            {
		// For each elevated token, create a button on the toolbar which is captioned by the token
		//                Button button = new Button();

		//                button.Font = new System.Drawing.Font("Lucida Sans Unicode", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

		//ElevatedToken t = output[i] as ElevatedToken;
		//                string[] str = myXML.explodeXMLdata((ArrayList)output[i]);
		//                button.Text = str[0];

		// Placement information for the new button
		//                int locX = 250 + (50 * i);
		//                int locY = WavesHeight - 112 + (40 * lv);
		//                button.Location = new Point(locX, locY);

		//                button.Width = 40;
		//                button.Height = 30;
		//                if (lv == 2) button.Visible = false;

		// Event information for the new button.  It takes effect when clicked.
		//                button.Click += new EventHandler(Button_Token_Click);
		//                button.Enter += new EventHandler(Button_Dynamic_Enter);
		//                button.MouseEnter += new EventHandler(Button_Dynamic_Enter);
		//                button.Tag = i;

		// Assign a Tab Index to this button
		//                button.TabIndex = tabStop;
		//                tabStop++;

		// Add the button to the form
		//                Controls.Add(button);

		// In case this is the last token, make sure the toolbar window is wide enough to show all buttons
		//                dimensionX = locX + 150;
		//            }

		//        } // PopulateToolbar function




		//        public static void setKeyCombo(int id, string type, ToolStripMenuItem menu)
		//        {
		// Set Up the RetrieveData structure to receive Key Data
		//            RetrieveData[] rd;
		//            myXML.initStructure(out rd, 3);

		//            rd[0].SetData(false, "control");
		//            rd[1].SetData(false, "alt");
		//            rd[2].SetData(false, "key");

		// Query the .xml file for the information requested
		//            ArrayList output = myXML.loadXML(CurDir + "\\key-mapping.xml", "/all/" + type, rd);
		//            string[] str = myXML.explodeXMLdata((ArrayList)output[0]);

		// Set the key combination
		//KeyCommand[id] = new KeyCombo(str[0], str[1], str[2]);

		// Set the shortcut key property for the appropriate Menu Item, which was passed in
		//            AssignKeyToMenu(KeyCommand[id], menu);
		//        }


		// *******************************************************************************************************************
		//   These functions are relics from before I implemented English Excerpt abstraction to XML file
		// *******************************************************************************************************************


		//        private string GetExcerptDescription(string str, int id)
		//        {
		// This function retrieves the Xml wrapper (in other words, the containing MathML element)
		//   for the MathML excerpt passed in as an argument.

		//            string xml = GetXmlWrapper(str);

		//            if (Properties.Settings.Default.EnglishExcerpts)
		//            {

		//                switch (xml)
		//                {
		//                    case "mrow":
		//                        if (id == 0) return "BEGIN";
		//                        else return DetermineMrowEnglishMeaning(str, id);

		//                    case "mfrac":
		//                        return "fraction";

		//                    case "msqrt":
		//                        return "square root";

		//                    case "mroot":
		//                        return "root";

		//                    case "mtext":
		//                        return "literal";

		//                    case "msub":
		//                        return "subscript";

		//                    case "msup":
		//                        return "superscript";

		//                    case "msubsup":
		//                        return "sub-super";

		//                    case "mover":
		//                        return "over";

		//                    case "munder":
		//                        return "under";

		//                    case "munderover":
		//                        return "under-over";

		//                    default:
		//                        return "????";

		//                }
		//            }
		//            else return xml;

		//        }



		//        private string DetermineMrowEnglishMeaning(string str, int id)
		//        {
		//            string answer = "????";

		//            for (int i = 0; i < id; i++)
		//            {
		//                string ex = RetrieveExcerptFromListbox(LISTBOX_ShowExcerpts.Items[i].ToString());
		//                string tag = GetXmlWrapper(xmlPiece[i]);

		//                if (ex.Contains(TagifyID(id)))
		//                {
		//                    switch (tag)
		//                    {
		//                        case "mtext":
		//                            if (Convert.ToInt32(ex.Substring(1, 3)) == id) answer = "text";

		//                            break;

		//                        case "mfrac":
		//                            if (Convert.ToInt32(ex.Substring(1, 3)) == id) answer = "numerator";
		//                            else answer = "denominator";

		//                            break;

		//                        case "msqrt":
		//                            if (Convert.ToInt32(ex.Substring(1, 3)) == id) answer = "radicand";

		//                            break;

		//                        case "mroot":
		//                            if (Convert.ToInt32(ex.Substring(1, 3)) == id) answer = "radicand";
		//                            else answer = "radical";

		//                            break;

		//                        case "msup":
		//                            if (Convert.ToInt32(ex.Substring(1, 3)) == id) answer = "base";
		//                            else answer = "super";

		//                            break;

		//                        case "msub":
		//                            if (Convert.ToInt32(ex.Substring(1, 3)) == id) answer = "base";
		//                            else answer = "sub";

		//                            break;

		//                        case "msubsup":
		//                            if (Convert.ToInt32(ex.Substring(1, 3)) == id) answer = "base";
		//                            else if (ex.Length > 5)
		//                                if (Convert.ToInt32(ex.Substring(6, 3)) == id) answer = "sub";
		//                                else answer = "super";


		//                            break;

		//                        case "munder":
		//                            if (Convert.ToInt32(ex.Substring(1, 3)) == id) answer = "base";
		//                            else answer = "below";

		//                            break;

		//                        case "mover":
		//                            if (Convert.ToInt32(ex.Substring(1, 3)) == id) answer = "base";
		//                            else answer = "above";

		//                            break;

		//                        case "munderover":
		//                            if (Convert.ToInt32(ex.Substring(1, 3)) == id) answer = "base";
		//                            else if (ex.Length > 5)
		//                                if (Convert.ToInt32(ex.Substring(6, 3)) == id) answer = "below";
		//                                else answer = "above";

		//                            break;



		//                        default:
		//                            answer = tag;
		//                            break;
		//                    }
		//                }
		//            }

		//            return answer;
		//        }



		// *******************************************************************************************************************
		//   Speech recognition code snippet
		// *******************************************************************************************************************

		// This code will enable speech recognition for our toolbar!
		// This amazing, and amazingly simple code, was found at http://msdn.microsoft.com/en-us/library/hh361683.aspx

		// Create a new SpeechRecognitionEngine instance.
		//                SpeechRecognizer recognizer = new SpeechRecognizer();

		// Create a simple grammar that recognizes "red", "green", or "blue".
		//                Choices colors = new Choices();
		//                colors.Add(new string[] { "red", "green", "blue" });

		// Create a GrammarBuilder object and append the Choices object.
		//                GrammarBuilder gb = new GrammarBuilder();
		//                gb.Append(colors);

		// Create the Grammar instance and load it into the speech recognition engine.
		//                Grammar g = new Grammar(gb);
		//                recognizer.LoadGrammar(g);

		// Register a handler for the SpeechRecognized event.
		//                recognizer.SpeechRecognized +=
		//                  new EventHandler<SpeechRecognizedEventArgs>(sre_SpeechRecognized);



		// *******************************************************************************************************************
		//   These functions are really ancient!
		// *******************************************************************************************************************

		//        private void MENU_Help_Shortcuts_Click(object sender, EventArgs e)
		//        {
		//            string help = "Token Toolbar special keys\n\n" +
		//                "<ENTER> -- Append current token to textbox\n" +
		//                "<ESC> -- Close Token Toolbar\n" +
		//                "<" + KeyCommand[0].GetKeyCombo() + "> -- Show this message\n" +
		//                "<" + KeyCommand[1].GetKeyCombo() + "> -- Show/hide Level 2 tokens\n" +
		//                "<" + KeyCommand[2].GetKeyCombo() + "> -- Show/hide Level 3 token window\n" +
		//                "<" + KeyCommand[3].GetKeyCombo() + "> -- Copy formula to Clipboard as MathML\n" +
		//                "<" + KeyCommand[4].GetKeyCombo() + "> -- Read all text in textbox\n" +
		//                "<" + KeyCommand[6].GetKeyCombo() + "> -- Clear entire MathML formula\n" +
		//                "<" + KeyCommand[5].GetKeyCombo() + "> -- Clear MathML excerpt in textbox\n";

		// PromptBuilder stuff from http://www.java2s.com/Tutorial/CSharp/0470__Windows-Presentation-Foundation/SpeechSynthesizerdemo.htm
		//            PromptBuilder promptBuilder = new PromptBuilder();

		//            promptBuilder.AppendText("Token Toolbar special keys. ");
		//            promptBuilder.AppendBreak(new TimeSpan(0, 0, 0, 0, 250));
		//            promptBuilder.AppendText("ENTER key- Append current token to textbox. ");
		//            promptBuilder.AppendText("ESCAPE key- Close Token Toolbar. ");
		//            promptBuilder.AppendText(KeyCommand[0].GetKeyCombo() + "- Show this message. ");
		//            promptBuilder.AppendText(KeyCommand[1].GetKeyCombo() + "- Show/hide Level too tokens. ");
		//            promptBuilder.AppendText(KeyCommand[2].GetKeyCombo() + "- Show/hide Level 3 token window. ");
		//            promptBuilder.AppendText(KeyCommand[3].GetKeyCombo() + "- Copy formula to Clipboard as Math M L. ");
		//            promptBuilder.AppendBreak(new TimeSpan(0, 0, 0, 0, 250));
		//            promptBuilder.AppendText(KeyCommand[4].GetKeyCombo() + "- Reed all text in textbox. ");
		//            promptBuilder.AppendText(KeyCommand[6].GetKeyCombo() + "- Clear entire Math M L formula. ");
		//            promptBuilder.AppendText(KeyCommand[5].GetKeyCombo() + "- Clear Math M L excerpt in textbox. ");

		//            StopSpeech();
		//            Speaker.SpeakAsync(promptBuilder);
		//            MessageBox.Show(help, "Token Toolbar- Special keys help", MessageBoxButtons.OK, MessageBoxIcon.Information);

		//        } // Show Shortcuts Dialog Menu click event




		//        private void LISTBOX_XMLpieces_Select(object sender, EventArgs e)
		//        {
		// This event is bound to the formula's Listbox, which shows a list of all MathML excerpts
		//   currently making up the formula.
		//   When selected, the Listbox reports the ID and contents of the excerpt selected.

		//            if (LISTBOX_XMLpieces.SelectedItem == null)     return;
		//            if (ReadLock == true)
		//            {
		//                ReadLock = false;
		//                return;
		//            }

		//            string s = LISTBOX_XMLpieces.SelectedItem.ToString();

		//            string excerpt = RetrieveExcerptFromListbox(s);
		//            int excerptNum = Convert.ToInt32(s.Substring(1, 3));

		//            PromptBuilder p = new PromptBuilder();
		//            PromptBuilder p2 = new PromptBuilder();

		//            p.AppendText("Excerpt " + excerptNum.ToString() + " selected ");
		//            p.AppendBreak(new TimeSpan(0, 0, 0, 0, 100));

		//            StopSpeech();
		//            Speaker.SpeakAsync(p);
		// Maybe make this more understandable
		//            p2 = ReadText(excerptNum, false);
		//p.AppendText(excerpt);

		//s = "Add " + s + " token";

		//            Speaker.SpeakAsync(p2);

		//        } 



	}
}

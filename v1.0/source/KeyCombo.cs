using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Prototype_Token_Interface
{
    /// <summary>
    /// Represents one keystroke combination used as a shortcut for a toolbar command.
    /// </summary>
    public class KeyCombo
    {
        private string Name;
        private bool Ctrl;
        private bool Alt;
        private string Key;

        public bool _ctrl
        {
            get { return Ctrl; }
            set { Ctrl = value; }
        }

        public bool _alt
        {
            get { return Alt; }
            set { Alt = value; }
        }

        public string _mainKey
        {
            get { return Key; }
            set { Key = value; }
        }

        public string _name
        {
            get { return Name; }
            set { Name = value; }
        }

        /// <summary>
        /// Initializes an empty keystroke combination object.
        /// </summary>
        public KeyCombo()
        {
            _name = null;
            _ctrl = false;
            _alt = false;
            _mainKey = null;
        }

        /// <summary>
        /// Initializes an keystroke combination object without a name.
        /// </summary>
        /// <param name="c">Control key state</param>
        /// <param name="a">Alt key state</param>
        /// <param name="k">Main key</param>
        public KeyCombo( bool c, bool a, string k )
        {
            _name = null;
            _ctrl = c;
            _alt = a;
            _mainKey = k;
        }

        /// <summary>
        /// Initializes a keystroke combination object with a name.
        /// </summary>
        /// <param name="n">The keystroke combination's name</param>
        /// <param name="c">Control key state</param>
        /// <param name="a">Alt key state</param>
        /// <param name="k">Main key</param>
        public KeyCombo( string n, bool c, bool a, string k )
        {
            _name = n;
            _ctrl = c;
            _alt = a;
            _mainKey = k;
        }

        /// <summary>
        /// Initializes a keystroke combination object with a name.
        /// </summary>
        /// <param name="n">The keystroke combination's name</param>
        /// <param name="c">String representing the Control key state</param>
        /// <param name="a">String representing the Alt key state</param>
        /// <param name="k">Main key</param>
        public KeyCombo( string n, string c, string a, string k )
        {
            _name = n;
            _ctrl = SetBoolKeyValue( c );
            _alt = SetBoolKeyValue( a );
            _mainKey = k;
        }

        /// <summary>
        /// Resets an initialized keystroke combination, leaving the name unchanged.
        /// </summary>
        /// <param name="c">The new Control key state</param>
        /// <param name="a">The new Alt key state</param>
        /// <param name="k">The new Main key</param>
        public void SetData( bool c, bool a, string k )
        {
            _ctrl = c;
            _alt = a;
            _mainKey = k;
        }

        /// <summary>
        /// Resets an initialized keystroke combination, including the name.
        /// </summary>
        /// <param name="n">The new name</param>
        /// <param name="c">The new Control key state</param>
        /// <param name="a">The new Alt key state</param>
        /// <param name="k">The new Main key</param>
        public void SetData( string n, string c, string a, string k )
        {
            _name = n;
            _ctrl = SetBoolKeyValue( c );
            _alt = SetBoolKeyValue( a );
            _mainKey = k;
        }
 
        /// <summary>
        /// Converts a string to a True or False value
        /// </summary>
        /// <param name="s">The string to convert</param>
        /// <returns>True if "true", "t", "yes", "y", or "1", False otherwise</returns>
        public bool SetBoolKeyValue( string s )
        {
            switch ( s.ToLower() ) {
                case "true":
                case "t":
                case "yes":
                case "y":
                case "1":
                    return true;
                default:
                    return false;
            }
        }


        /// <summary>
        /// Determines if a specified this keystroke combination has been initiated by the user.
        /// </summary>
        /// <param name="e">A snapshot of the keyboard's state, taken when a KeyUp or KeyDown event fires</param>
        /// <returns>True if initiated, False otherwise</returns>
        public bool TestKeyCombo( KeyEventArgs e )
        {
            // This function determines if a specified key-combination has been initiated by the user.
            //   Obviously returns true if yes, otherwise returns false.

            KeysConverter kc = new KeysConverter();

            // Make sure the Control key is in the proper state
            if ( _ctrl )       if ( e.Modifiers != Keys.Control )    return false;
            if ( !_ctrl )      if ( e.Control )                      return false;

            // Make sure the Alt key is in the proper state
            if ( _alt )        if ( e.Modifiers != Keys.Alt )        return false;
            if ( !_alt )       if ( e.Alt )                          return false;

            // st[2] is the main key.  "e.KeyCode" carries additional info when Control or Alt are
            //   pressed, so we need to get only the main key code, and test against s[2].
            int bareKey = GetLeastSignificantBits( Convert.ToInt32( e.KeyCode ) );
            string keyChar = kc.ConvertToString( bareKey );

            if ( keyChar != _mainKey )                               return false;

            // If execution reaches here, the correct key is pressed and the Control and Alt keys
            //   are in the correct state.  Therefore, the check succeeds.
            return true;
        }


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


        /// <summary>
        /// This function returns a string representation, suitable for display or speech, of this keystroke combination.
        /// </summary>
        /// <returns></returns>
        public string GetKeyCombo( )
        {
            string answer = "";

            if ( _ctrl == true )	answer += "Ctrl+";
            if ( _alt == true )		answer += "Alt+" ;
            answer += _mainKey;

            return answer;
        }


        /// <summary>
        /// This function returns a "Keys" representation of the key described by a keystroke combination.  
        /// This is needed when testing for shortcut validity.
        /// </summary>
        /// <returns></returns>
        public Keys GetKeyCode()
        {
            Keys k = new Keys();
            KeysConverter kc = new KeysConverter();

            try {
                // This command can throw an exception if mainKey is not set.  
                // This, however, is fine; it just means there is no keyboard shortcut for this item.
                k = ( Keys )kc.ConvertFromString( _mainKey );
                if ( _ctrl )	k = k | Keys.Control;
                if ( _alt )		k = k | Keys.Alt;

                return k;
            }
            catch {
                return Keys.None;
            }
        }


		/// <summary>
		/// Determines the shortcut that a specified button should react to.
		/// </summary>
		/// <param name="button">The button to which the determined shortcut should be assigned</param>
		/// <param name="tok">The token (or element) which the specified button represents</param>
		/// <param name="isElement">Whether the specified button represents an element or token... use true if it is an element</param>
		public void GetWavesTokenShortcut( Token_Toolbar tt, Button button, bool isElement )
		{
			// The Control key is always required for button shortcuts
			_ctrl = true;

			// Assume the Alt key is not required; will reset it if it is
			_alt = false;

			// Assume the main key is "0"... it will be determined later, but this will serve as a default
			_mainKey = "0";

			// Determine the main key by button position
			if( isElement ) {
				// If this button is an element, the answer will always be between 1 and 5 inclusive; Alt key not set
				int num;
				for( num = ( Token_Toolbar.NUM_CONTROLS ); num < ( Token_Toolbar.NUM_CONTROLS + tt.NumElements ); num++ )
					if( button == tt.Controls[ num ] )
						break;

				_mainKey = ( num - Token_Toolbar.NUM_CONTROLS + 1 ).ToString();
			}
			else {
				// This is an MO or MI token.
				int num;
				for( num = ( Token_Toolbar.NUM_CONTROLS + tt.NumElements ); num < tt.Controls.Count; num++ )
					if( button == tt.Controls[ num ] )
						break;

				// MO and MI tokens are either 1st or 2nd level.  
				//   There are 5 possible 1st level tokens (Control + <6 to 0>).
				//   There are 10 possible 2nd level tokens (Control + Alt + <1 to 0>).
				if( num < ( Token_Toolbar.NUM_CONTROLS + tt.NumElements + tt.NumLevel1 ) ) {
					// This is a level 1 token
					_mainKey = ( num + ( 5 - tt.NumElements ) - Token_Toolbar.NUM_CONTROLS + 1 ).ToString();
				}
				else {
					// This is a level 2 token
					_mainKey = ( num - ( tt.NumElements + tt.NumLevel1 + Token_Toolbar.NUM_CONTROLS ) + 1 ).ToString();
					_alt = true;
				}

				// If the answer we determined is "10", then make the key "0".  This is required because of the "0"'s place on the keyboard.
				if( _mainKey == "10" )
					_mainKey = "0";
			}

		}

        /// <summary>
        /// Determines whether a shortcut key combination is valid.  Very important when users modify these shortcuts!
        /// </summary>
        /// <param name="shortcut">A "Keys" representation of the keyboard shortcut</param>
        /// <returns>True if valid, False if not</returns>
        public bool IsValidShortcut()
        {
            // This function, IsValidShortcut, is a beautiful solution that I found to determine whether a certain combination of
            //   of modifiers is indeed a valid shortcut.
            // Source:  http://stackoverflow.com/questions/7804371/how-to-assign-keys-pageup-to-toolstripmenuitem-shortcutkeys

			// First of all, get a Keys representation of our KeyCombo
			Keys shortcut = GetKeyCode();

			// should have a key and one or more modifiers.
			Keys keyCode = ( Keys )( shortcut & Keys.KeyCode );
            Keys modifiers = ( Keys )( shortcut & Keys.Modifiers );

			// Make sure the the number keys "1" through "0" cannot be used to create a keyboard shortcut... they are reserved
			//   <ctrl>+num and <ctrl>+<alt>+num are reserved shortcuts for adding tokens and elements to the formula!
			int buttonNum = Convert.ToInt32( (int)keyCode );
			if( ( buttonNum >= 48 ) & ( buttonNum <= 57 ) )
				return false;

            if ( shortcut == Keys.None ) {
                return false;
            }
            else if ( ( keyCode == Keys.Delete ) || ( keyCode == Keys.Insert ) ) {
                return true;
            }
            else if ( ( ( int )keyCode >= ( int )Keys.F1 ) && ( ( int )keyCode <= ( int )Keys.F24 ) ) {
                // function keys by themselves are valid
                return true;
            }
            else if ( ( keyCode != Keys.None ) && ( modifiers != Keys.None ) ) {
                switch ( keyCode ) {
                    case Keys.Menu:
                    case Keys.ControlKey:
                    case Keys.ShiftKey:
                        // shift, control and alt arent valid on their own. 
                        return false;
                    default:
                        if ( modifiers == Keys.Shift ) {
                            // shift + somekey isnt a valid modifier either
                            return false;
                        }
                        return true;
                }
            }
            // has to have a valid keycode and valid modifier.
            return false;
        }
    }

}

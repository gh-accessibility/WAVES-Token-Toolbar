using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prototype_Token_Interface
{
	public class Unicode_Dictionary
	{
		/// <summary>
		/// Creates a dictionary for looking up Unicode character Information
		/// </summary>
		private Dictionary<char,string> charname_map = new Dictionary<char, string>( 65536 );



		// *******************************************************************************************************************
		//   The following event functions allow the user to edit the MathML formula
		// *******************************************************************************************************************

		/// <summary>
		/// This constructor creates a new instance of the Unicode Dictionary from the unicode-data.txt file.  It was originally
		///   retrieved from unicode.org
		/// </summary>
		public Unicode_Dictionary()
		{
			string[] unicodedata = System.IO.File.ReadAllLines( "unicode-data.txt", Encoding.UTF8 );

			for( int i = 0; i < unicodedata.Length; i++ ) {
				string[] fields = unicodedata[ i ].Split( ';' );
				int char_code = int.Parse( fields[ 0 ], System.Globalization.NumberStyles.HexNumber );
				string char_name = fields[ 1 ];
				if( char_code >= 0 && char_code <= 0xFFFF ) {									//UTF-16 BMP code points only
					bool is_range = char_name.EndsWith( ", First>" );
					if( is_range ) {															//add all characters within a specified range
						char_name.Replace( ", First", String.Empty );							//remove range indicator from name
						fields = unicodedata[ ++i ].Split( ';' );
						int end_char_code = int.Parse( fields[ 0 ], System.Globalization.NumberStyles.HexNumber );
						if( !fields[ 1 ].EndsWith( ", Last>" ) )
							throw new Exception( "Expected end-of-range indicator." );
						for( int code_in_range = char_code; code_in_range <= end_char_code; code_in_range++ )
							charname_map.Add( ( char )code_in_range, char_name );
					}
					else
						charname_map.Add( ( char )char_code, char_name );
				}
			}
		}

//		private void CreateUnicodeDictionary()
//		{
//		}

		public string GetUnicodeDescription( char c )
		{
			string character_name;
			if( !charname_map.TryGetValue( c, out character_name ) )
				character_name = "!-MISSING DESCRIPTION-!";

			return character_name;
		}


		public string GetUnicodeDescriptionPretty( char c )
		{
			string character_name;
			if( !charname_map.TryGetValue( c, out character_name ) )
				character_name = "!-Character Name Missing-!";

			character_name = character_name.ToLower();

			// This sick little line was found at 
			//   http://stackoverflow.com/questions/1943273/convert-all-first-letter-to-upper-case-rest-lower-for-each-word
			character_name = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase( character_name );

			return character_name;
		}


		public int GetUnicodeEntityDecimal( char c )
		{
			return Convert.ToInt16( c );

			//string character_name;
			//if( !charname_map.TryGetValue( c, out character_name ) )
			//	character_name = "<Character Name Missing>";

			//character_name = character_name.ToLower();

			// This sick little line was found at 
			//   http://stackoverflow.com/questions/1943273/convert-all-first-letter-to-upper-case-rest-lower-for-each-word
			//character_name = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase( character_name );

			//return character_name;
		}

		public string GetUnicodeEntityHex( char c )
		{
			int dec = GetUnicodeEntityDecimal( c );

			// return an entity like "&#x2147;".  The hex representation should always be four hex digits.
			string hex = dec.ToString( "X" );

			while( hex.Length < 4 )
				hex = "0" + hex;

			hex = "&#x" + hex + ";";

			return hex;

			// From http://stackoverflow.com/questions/1139957/c-sharp-convert-integer-to-hex-and-back-again
			// Convert integer 182 as a hex in a string variable
			//string hexValue = decValue.ToString("X");
			// Convert the hex string back to the number
			//int decAgain = int.Parse(hexValue, System.Globalization.NumberStyles.HexNumber);
		}



	}
}

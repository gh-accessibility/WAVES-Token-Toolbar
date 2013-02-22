using System;
using System.Collections;                   // Required for Arraylist implementation (LoadXML requires this)
using System.Collections.Generic;
using System.Linq;
using System.Text;

//using XML_functions;                        // Using my .dll for XML functions
using System.Xml.XPath;                     // For XML DOM-walking functionality
using System.Xml;


namespace Prototype_Token_Interface
{
    /// <summary>
    /// Represents one MathML Token or Element instance in the Master Token List.
    /// </summary>
    public class Token
    {
        private string symbol;
        private string speech;
        private int args;

        public string _symbol
        {
            get { return symbol; }
            set { symbol = value; }
        }

        public string _speech
        {
            get { return speech; }
            set { speech = value; }
        }

        public int _args
        {
            get { return args; }
            set { args = value; }
        }

        /// <summary>
        /// Initializes an empty MathML Token/element.
        /// </summary>
        public Token()
        {
            _symbol = "";
            _speech = "";
            _args = 0;
        }

		public Token( string sym, string sp, int a )
		{
			_symbol = sym;
			_speech = sp;
			_args = a;
		}

        /// <summary>
        /// Initializes a MathML token/element.
        /// </summary>
        /// <param name="sym">The token symbol or element name</param>
        /// <param name="sp">A speech representation of the token/element</param>
        /// <param name="a">For MathML elements, the number of "mrow" arguments it requires</param>
        public void SetToken( string sym, string sp, int a )
        {
            _symbol = sym;
            _speech = sp;
            _args = a;
        }

    }

    /// <summary>
    /// Contains all MathML Token and Element information, which drives the Token Toolbar interface.
    /// </summary>
    public class Master_Token_List
    {
        // Declare three separate arrays of Tokens.  The actual dimensions will be determined later, when
        //   the necessary sizes is determined.
		private List<Token> mo = new List<Token>();
		private List<Token> mi = new List<Token>();
		private List<Token> MathMLelement = new List<Token>();

        // These variables record the number of Tokens in each listing, for easy reference
        private int moCount = 0;
        private int miCount = 0;
        private int elemCount = 0;

        //private string[][] elemEnglish;
		private List <string[]> elemEnglish = new List<string[]>();

        /// <summary>
        /// MathML iterator "MI" tokens.
        /// </summary>
		public List<Token> _mi
        {
            get { return mi; }
            set { mi = value; }
        }

        /// <summary>
        /// MathML operator "MO" tokens.
        /// </summary>
		public List<Token> _mo
        {
            get { return mo; }
            set { mo = value; }
        }

        /// <summary>
        /// MathML complex elements.
        /// </summary>
		public List<Token> _mathMLelement
        {
            get { return MathMLelement; }
            set { MathMLelement = value; }
        }

        /// <summary>
        /// The number of MO tokens registered
        /// </summary>
        public int _moCount
        {
            get { return moCount; }
            set { moCount = value; }
        }

        /// <summary>
        /// The number of MI tokens registered
        /// </summary>
        public int _miCount
        {
            get { return miCount; }
            set { miCount = value; }
        }

        /// <summary>
        /// The number of MathML complex elements registered
        /// </summary>
        public int _elemCount
        {
            get { return elemCount; }
            set { elemCount = value; }
        }

        /// <summary>
        /// A matrix of English representations of MathML complex elements. 
        ///   elemEnglish[x][0] is the base descriptor for the element, while
        ///   elemEnglish[x][n] is the descriptor for the n-th "mrow" argument of the element.
        /// </summary>
        public List<string[]> _elemEnglish
        {
            get { return elemEnglish; }
            set { elemEnglish = value; }
        }


        /// <summary>
        /// Initializes an empty Master Token List.
        /// </summary>
        public Master_Token_List()
        {
            _mo = null;
            _mi = null;
            _mathMLelement = null;
            _moCount = 0;
            _miCount = 0;
            _elemCount = 0;
            _elemEnglish = null;    
        }


        /// <summary>
        /// Retrieves all MathML token and element information from external XML file (master-token-list.xml).
        /// </summary>
        /// <returns>0 if successful, -1 for failure</returns>
        public int ParseMasterDOM()
        {
			// Declare needed variables to walk the DOM only once.  CatNum is a device to determine where we are in the
			//   DOM.
			int catNum = 0;

			mo = new List<Token>();
			mi = new List<Token>();
			MathMLelement = new List<Token>();
			elemEnglish = new List<string[]>();

            // This file walks the Master Token List's DOM, loading in all information into the correct data structures
            string filename = Token_Toolbar.CurDir + "\\master-token-list.xml";

            // First of all, we test to see if the XML file we are going to be parsing exists...
            if ( System.IO.File.Exists( filename ) )
            {
                // Load the XML document
                XmlDocument masterXML = new XmlDocument();
                masterXML.Load( filename );

				foreach( XmlNode tokCategory in masterXML.DocumentElement.ChildNodes ) {
					foreach( XmlNode sym in tokCategory.ChildNodes ) {
						switch( catNum ) {
							case 0:
								// For the first category, we must parse MO tokens
								_moCount++;
								mo.Add( new Token( sym.ChildNodes[ 0 ].InnerText, sym.ChildNodes[ 1 ].InnerText, Convert.ToInt32( sym.ChildNodes[ 2 ].InnerText ) ) );
								break;

							case 1:
								// For the second category, we must parse MI tokens
								_miCount++;
								mi.Add( new Token( sym.ChildNodes[ 0 ].InnerText, sym.ChildNodes[ 1 ].InnerText, Convert.ToInt32( sym.ChildNodes[ 2 ].InnerText ) ) );
								break;

							case 2:
								// In the final category, we're parsing elements...
								_elemCount++;
								MathMLelement.Add( new Token( sym.ChildNodes[ 0 ].InnerText, sym.ChildNodes[ 1 ].InnerText, Convert.ToInt32( sym.ChildNodes[ 2 ].InnerText ) ) );

								// ... and the descriptive prompts for each element
								string[] engPrompts = new string[ 4 ];
								int id = 0;
								foreach( XmlNode english in sym.ChildNodes[ 3 ].ChildNodes ) {

									engPrompts[ id ] = english.InnerText;
									id++;
								}
								elemEnglish.Add( engPrompts );
								break;

							default:
								// Execution should never get here, unless we have an invalid Master Token List!
								break;
						}
					}

					// After parsing this whole category, let's parse the next one differently.
					catNum++;
				}

                // Alert calling function of success
                return 0;
            }
            else
            {
                // Can't do anything if this xml doesn't exist.  If the file is not found, returns -1 for FAILURE 
                return -1;
            }
        } // close function: loadMasterTokenXML


//		public int ParseMasterDOM_old()
//		{
			// This file walks the Master Token List's DOM, loading in all information into the correct data structures
//			string filename = Token_Toolbar.CurDir + "\\master-token-list.xml";

			// First of all, we test to see if the XML file we are going to be parsing exists...
//			if( System.IO.File.Exists( filename ) ) {
				// Load the XML document
//				XmlDocument masterXML = new XmlDocument();
//				masterXML.Load( filename );

//				XmlNode MOnode = masterXML.SelectSingleNode( "/all/mo" );
//				XmlNode MInode = masterXML.SelectSingleNode( "/all/mi" );
//				XmlNode ElementNode = masterXML.SelectSingleNode( "/all/element" );

//				_moCount = MOnode.ChildNodes.Count;
//				_mo = new Token[ _moCount ];

//				_miCount = MInode.ChildNodes.Count;
//				_mi = new Token[ _miCount ];

//				_elemCount = ElementNode.ChildNodes.Count;
//				_mathMLelement = new Token[ _elemCount ];

//				_elemEnglish = new string[ _elemCount ][];


//				int id = 0;
//				foreach( XmlNode node in masterXML.SelectNodes( "//all//mo//token" ) ) {
//					_mo[ id ] = new Token();
//					_mo[ id ].SetToken( node.ChildNodes[ 0 ].InnerText, node.ChildNodes[ 1 ].InnerText, Convert.ToInt32( node.ChildNodes[ 2 ].InnerText ) );
//					id++;
//				}

//				id = 0;
//				foreach( XmlNode node in masterXML.SelectNodes( "//all//mi//token" ) ) {
//					_mi[ id ] = new Token();
//					_mi[ id ].SetToken( node.ChildNodes[ 0 ].InnerText, node.ChildNodes[ 1 ].InnerText, Convert.ToInt32( node.ChildNodes[ 2 ].InnerText ) );
//					id++;
//				}

//				id = 0;
//				foreach( XmlNode node in masterXML.SelectNodes( "//all//element//token" ) ) {
//					_mathMLelement[ id ] = new Token();
//					_mathMLelement[ id ].SetToken( node.ChildNodes[ 0 ].InnerText, node.ChildNodes[ 1 ].InnerText, Convert.ToInt32( node.ChildNodes[ 2 ].InnerText ) );
		
//					_elemEnglish[ id ] = new string[ 4 ];
//					int id2 = 0;
//					foreach( XmlNode child in node.ChildNodes[ 3 ].ChildNodes ) {
//						_elemEnglish[ id ][ id2 ] = child.InnerText;
//						id2++;
//					}
//					id++;
//				}

				// Alert calling function of success
//				return 0;
//			}
//			else {
				// Can't do anything if this xml doesn't exist.  If the file is not found, returns -1 for FAILURE 
//				return -1;
//			}
//		} // close function: loadMasterTokenXML



//        public static int ParseMasterDOM()
//        {
           
//            string filename = Token_Toolbar.CurDir + "\\master-token-list.xml";

            // First of all, we test to see if the XML file we are going to be parsing exists...
//            if (System.IO.File.Exists(filename))
//            {
                // Load the XML document
//                XmlDocument masterXML = new XmlDocument();
//                masterXML.Load(filename);

//                XmlNode MOnode = masterXML.SelectSingleNode("//all//mo");
//                XmlNode MInode = masterXML.SelectSingleNode("//all//mi");
//                XmlNode ElementNode = masterXML.SelectSingleNode("//all//element");

//                MasterToken.moCount = MOnode.ChildNodes.Count;
//                mo = new Token[moCount];

//                miCount = MInode.ChildNodes.Count;
//                mi = new Token[miCount];

//                elemCount = ElementNode.ChildNodes.Count;
//                MathMLelement = new Token[elemCount];
                
//                elemEnglish = new string[elemCount][];


//                int id = 0;
//                foreach (XmlNode node in masterXML.SelectNodes("//all//mo//token"))
//                {
//                    mo[id] = new Token();
//                    mo[id].SetToken(node.ChildNodes[0].InnerText, node.ChildNodes[1].InnerText, Convert.ToInt32(node.ChildNodes[2].InnerText));
//                    id++;
//                }

//                id = 0;
//                foreach (XmlNode node in masterXML.SelectNodes("//all//mi//token"))
//                {
//                    mi[id] = new Token();
//                    mi[id].SetToken(node.ChildNodes[0].InnerText, node.ChildNodes[1].InnerText, Convert.ToInt32(node.ChildNodes[2].InnerText));
//                    id++;
//                }

//                id = 0;
//                foreach (XmlNode node in masterXML.SelectNodes("//all//element//token"))
//                {
//                    MathMLelement[id] = new Token();
//                    MathMLelement[id].SetToken(node.ChildNodes[0].InnerText, node.ChildNodes[1].InnerText, Convert.ToInt32(node.ChildNodes[2].InnerText));

//                    elemEnglish[id] = new string[4];
//                    int id2 = 0;
//                    foreach (XmlNode child in node.ChildNodes[3].ChildNodes)
//                    {
//                        elemEnglish[id][id2] = child.InnerText;
//                        id2++;
//                    }
//                    id++;
//                }

                // Alert calling function of success
//                return 0;
//            }
//            else
//            {
                // Can't do anything if this xml doesn't exist.  If the file is not found, returns -1 for FAILURE 
//                return -1;
//            }

            


            // Set up the data structure for retrieving token data
//            RetrieveData[] rd;
//            myXML.initStructure(out rd, 3);

//            rd[0].SetData(false, "symbol");
//            rd[1].SetData(false, "speech");
//            rd[2].SetData(false, "args");


            // Load the necessary mo data from the xml into memory
//            ArrayList output = myXML.loadXML(Filename, "/all/mo/token", rd);

//            moCount = output.Count;
//            mo = new Token[moCount];

//            for (int i = 0; i < moCount; i++)
//            {
//                string[] str = myXML.explodeXMLdata((ArrayList)output[i]);

//                mo[i] = new Token();
//                mo[i].SetToken(str[0], str[1], Convert.ToInt32(str[2]));
//            }


            // Load the necessary mi data from the xml into memory
//            output = myXML.loadXML(Filename, "/all/mi/token", rd);

//            miCount = output.Count;
//            mi = new Token[miCount];

//            for (int i = 0; i < miCount; i++)
//            {
//                string[] str = myXML.explodeXMLdata((ArrayList)output[i]);

//                mi[i] = new Token();
//                mi[i].SetToken(str[0], str[1], Convert.ToInt32(str[2]));
//            }


            // Load the necessary mo data from the xml into memory
//            output = myXML.loadXML(Filename, "/all/element/token", rd);

//            elemCount = output.Count;
//            MathMLelement = new Token[elemCount];

//            for (int i = 0; i < elemCount; i++)
//            {
//                string[] str = myXML.explodeXMLdata((ArrayList)output[i]);

//                MathMLelement[i] = new Token();
//                MathMLelement[i].SetToken(str[0], str[1], Convert.ToInt32(str[2]));
//            }

//            return 0;


//        } // close function: loadMasterTokenXML


        // This is a backed up version of the "loadAllTokens" function.  It uses LoadXML three times.
//        public static int loadAllTokens()
//        {
            // Can't do anything if this xml doesn't exist.  If the file is not found, returns -1 for FAILURE 
//            string Filename = Token_Toolbar.CurDir + "\\master-token-list.xml";
//            if (!System.IO.File.Exists(Filename)) return -1;


            // Set up the data structure for retrieving token data
//            RetrieveData[] rd;
//            myXML.initStructure(out rd, 3);

//            rd[0].SetData(false, "symbol");
//            rd[1].SetData(false, "speech");
//            rd[2].SetData(false, "args");


            // Load the necessary mo data from the xml into memory
//            ArrayList output = myXML.loadXML(Filename, "/all/mo/token", rd);

//            moCount = output.Count;
//            mo = new Token[moCount];

//            for (int i = 0; i < moCount; i++)
//            {
//                string[] str = myXML.explodeXMLdata((ArrayList)output[i]);

//                mo[i] = new Token();
//                mo[i].SetToken(str[0], str[1], Convert.ToInt32(str[2]));
//            }


            // Load the necessary mi data from the xml into memory
//            output = myXML.loadXML(Filename, "/all/mi/token", rd);

//            miCount = output.Count;
//            mi = new Token[miCount];

//            for (int i = 0; i < miCount; i++)
//            {
//                string[] str = myXML.explodeXMLdata((ArrayList)output[i]);

//                mi[i] = new Token();
//                mi[i].SetToken(str[0], str[1], Convert.ToInt32(str[2]));
//            }


            // Load the necessary mo data from the xml into memory
//            output = myXML.loadXML(Filename, "/all/element/token", rd);

//            elemCount = output.Count;
//            MathMLelement = new Token[elemCount];

//            for (int i = 0; i < elemCount; i++)
//            {
//                string[] str = myXML.explodeXMLdata((ArrayList)output[i]);

//                MathMLelement[i] = new Token();
//                MathMLelement[i].SetToken(str[0], str[1], Convert.ToInt32(str[2]));
//            }

//            return 0;


//        } // close function: loadMasterTokenXML

    }
}

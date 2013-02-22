*******************************************************************************************************
WAVES MathML Token Toolbar v1.0								Primary programmer: 	Jeffrey Ludwig
																						Project Leads:				Dave Schleppenbach
																															Dennis Leas
																															
		Copyright © 2013 gh, llc.										http://ghbraille.com
		Licensed under the GNU GPL, v3								http://www.gnu.org/licenses/gpl.html
		
		Please direct comments, questions, and bug reports to:
			gh, llc
			700 Farabee Court
			Lafayette, IN  47905
			(765) 775-3776
			
		Or, send feedback to:	 mathspeak@gh-accessibility.com
*******************************************************************************************************


Introduction.
******************************************************************************************

This application was written in C# using Visual Studio 2008 Professional edition.

It uses gNumerator, which consists of two libraries, for working with and displaying 
MathML formulas.  The project's website states that:

	"The gNumerator application is will be released under the GNU General Public License, and its'
	library components (MathML DOM, renderer, interpreter, etc..) will be released under the LGPL, 
	which makes them usable in a commercial application".

All credits for gNumerator go to Andre T. Somogyi, its programmer. 

For more info on gNumerator, see
	http://numerator.sourceforge.net/general_info.php
	http://numerator.sourceforge.net/components.php


	
Purpose and features.
******************************************************************************************

This application is intended to help anyone create well-formed MathML quickly using keyboard
and mouse inputs.

Features:
	+ User can enter numbers, letters, and MathML tokens and elements organically.
	+ Support for most MathML content elements that are currently supported.
	+ User can Import MathML from a file or from the clipboard
	+ User can likewise Export Toolbar output to a MathML file, a graphic, or to the clipboard in a clean, indented format
	+ Full internal speech prompting, to let the user know what they have done and are doing
	+ MathML rendering to show user what they have created
	+ Math rendering is accomplished using either the native gNumerator, or the online MathML renderer MathJax.
	+ A custom MathML reader for telling the user what they have created
	+ Toolbar can interpret the meaning of registered MathML elements for the user
	+ Extensible support for other MathML tokens and elements
	+ Customizable first and second level MathML tokens
	+ Token Depository for tokens that did not make the cut for the first two levels
	+ Customizable key stroke mapping
	+ Additional keystrokes are present for MathML tokens and elements featured on the Toolbar
	+ Strong support for automatically inserting invisible times, invisible plus, and apply function operators when needed
	+ Parentheses automatically added in the base of a power when needed
	

Limitations:
	- Only one line allowed per formula.  Only the first <mrow> within a <math> tag gets rendered.
	- No editing support for presentation MathML such as the elements <mspace>, <mphantom>, <mstyle>, <mstack> and <mpadded>
	- No editing support for <ms>, <menclose>, <merror>, <mlongdiv>, or <mmultiscripts>
	- No editing support for tables, for example <mtable>, <mtr>, <mtd>
	- Exported MathML markup does not support the class attribute "MathML-Unit" for <mi> elements that require it. 
	- Does not read according to a specification (such as MathSpeak).  
	- No support for Voice Recognition.

	
	
To get this application running...
******************************************************************************************

1. Build the application with Visual Studio (obviously it is written in C#)

	DLLs
	--------------------------------------------
		MathML.dll
		MathML.Rendering.dll
		XML-functions.dll
	
		Without these libraries, the code will not compile at all.
	
	
2. Install these fonts.  In other words, place the fonts in your "%windir%\Fonts" directory.

	Fonts
	--------------------------------------------
		cm-stretchy.ttf
		math-roman.ttf
		l_10646.ttf				<-- This is Lucida Sans Unicode, and is needed for Toolbar to display math related content
		
3. You will need these files in the same directory as the .exe:

	.xml configuration files
	--------------------------------------------
		master-token-list.xml
		elevated-tokens.xml
		key-mapping.xml
	
		The Master Token List contains a comprehensive list of the tokens the application knows about.
		The Elevated Tokens lists the important tokens that will be present in the initial window.
		Key Mapping contains the application's keyboard shortcuts.
		
		
	font configuration .xml's for proper Math Rendering
	--------------------------------------------
		font-configuration-1-math-roman.xml
		font-configuration-2-cm-stretchy.xml
		font-configuration-3-times-new-roman.xml 
		 
		Without #1, some tokens will not render properly.
		Without #2, the application will not start.
		It is unclear what happens when #3 is not present; the application may run perfectly.



For help in operating this program...
******************************************************************************************

	Once the WAVES Toolbar has been successfully compiled, you can get help on most basic functions of the toolbar by
	selecting "Help -> Index" from the menu system, or by pressing "F1".  
	
	
		
*******************************************************************************************************
End document
*******************************************************************************************************
	

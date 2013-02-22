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
	public partial class About_WAVES : Form
	{
		public About_WAVES()
		{
			InitializeComponent();
		}

		private void linkLabel1_LinkClicked( object sender, LinkLabelLinkClickedEventArgs e )
		{
			System.Diagnostics.Process.Start( "http://ghbraille.com" );
		}

		private void LINK_gnu_LinkClicked( object sender, LinkLabelLinkClickedEventArgs e )
		{
			System.Diagnostics.Process.Start( "http://www.gnu.org/licenses/gpl.html" );
		}

		private void LINK_email_LinkClicked( object sender, LinkLabelLinkClickedEventArgs e )
		{
			System.Diagnostics.Process.Start( "mailto:mathspeak@gh-accessibility.com" );
		}

		private void BUTTON_ok_Click( object sender, EventArgs e )
		{
			Close();
		}
	}
}

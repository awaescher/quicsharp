using CSBREPL.Engine;
using EasyScintilla.Stylers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSBREPL.WinUI
{
	public partial class MainForm : Form
	{
		public MainForm()
		{
			InitializeComponent();

			txtCode.Styler = new CustomCSharpStyler();
			txtOut.Styler = new BatchStyler();
		}

		protected async override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);

			if (e.KeyCode == Keys.F5)
			{
				object o = await new ScriptRunner().Run(txtCode.Text);

				if (o is Exception ex)
					txtOut.Text = ex.ToString();
				else
					txtOut.Text = new VariableStringRenderer().Render(o as IEnumerable<Variable>);
			}
		}
		
	}
}

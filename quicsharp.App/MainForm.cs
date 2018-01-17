using quicsharp.Engine;
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

namespace quicsharp.App
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
				await RunScriptAndShowOutputAsync();

		}

		private async Task RunScriptAndShowOutputAsync()
		{
			var result = await RunScriptAsync();
			ShowScriptOutput(result);
		}

		private Task<object> RunScriptAsync()
		{
			return new ScriptRunner(new CodePreparer()).Run(txtCode.Text);
		}

		private void ShowScriptOutput(object scriptResult)
		{
			if (scriptResult is Exception ex)
				txtOut.Text = ex.ToString();
			else
				txtOut.Text = new HumanizerStringRenderer().Render(scriptResult as IEnumerable<Variable>);
		}
	}
}

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
using EasyScintilla;

namespace quicsharp.App
{
	public partial class MainForm : Form
	{
		private VariableStringRenderer _renderer;

		public MainForm()
		{
			InitializeComponent();

			txtCode.Styler = new CustomCSharpStyler();
			txtOut.Styler = new BatchStyler();
			_renderer = new VariableStringRenderer();
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
			return new ScriptCsScriptRunner(new CodePreparer(), new UiConsole(txtOut)).Run(txtCode.Text);
		}

		private void ShowScriptOutput(object scriptResult)
		{
			if (scriptResult is Exception ex)
				txtOut.Text = ex.ToString();
			//else
			//	txtOut.Text = _renderer.Render(scriptResult as IEnumerable<Variable>);
		}

		public class UiConsole : ISmallConsole
		{
			public UiConsole(SimpleEditor editor)
			{
				Editor = editor ?? throw new ArgumentNullException(nameof(editor));
			}

			public SimpleEditor Editor { get; }

			public void Clear()
			{
				Action act = () => Editor.ClearAll();
				Editor.Invoke(act);
			}

			public void WriteLine(string message)
			{
				Action act = () => Editor.AppendText(message);
				Editor.Invoke(act);
			}
		}

	}
}

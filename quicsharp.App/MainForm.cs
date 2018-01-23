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
using quicksharp.Engine;
using quicksharp.Engine.Errors;
using System.CodeDom.Compiler;
using quicksharp.Engine.Loggers;
using System.Reflection;
using quicksharp.Engine.Interfaces;
using quicsharp.Engine;

namespace quicsharp.App
{
	public partial class MainForm : Form
	{
		private ScriptExecutor _executor;

		public MainForm()
		{
			InitializeComponent();

			txtCode.Styler = new CustomCSharpStyler();
			txtOut.Styler = new BatchStyler();

			_executor = new ScriptExecutor(new TextBoxScriptLogger(txtOut));
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);

			if (e.KeyCode == Keys.F5)
				_executor.Execute(txtCode.Text);
		}
		
	}
}

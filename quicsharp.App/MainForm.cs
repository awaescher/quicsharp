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

		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);

			if (e.KeyCode == Keys.F5)
				RunScriptAndShowOutputAsync();

		}

		private void RunScriptAndShowOutputAsync()
		{
			var logger = new TextBoxScriptLogger(txtOut);
			logger.ShowErrors(new string[0]);

			string[] lines = txtCode.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

			SourceInfo info = ScriptGenerator.GetSource(lines);
//
		//	txtCode.Text = info.SourceCode;

			var res = CSharpScriptCompiler.Compile(info);

			if (res.Errors.HasErrors)
			{
				var errors = new List<ScriptError>();

				foreach (var item in res.Errors)
				{
					var compilerError = item as CompilerError;

					if (compilerError != null)
					{
						var error = new ScriptError() { ErrorNumber = compilerError.ErrorNumber, Line = compilerError.Line, Message = compilerError.ErrorText };
						ScriptErrorExtender.TryExtend(error);
						errors.Add(error);
					}
				}

				logger.ShowErrors(errors.ToArray());
			}
			else
			{
				try
				{
					testCode(res.CompiledAssembly, logger);
				}
				catch (Exception ex)
				{
					logger.ShowErrors(ex);
				}
			}
		}

		private void testCode(Assembly assembly, IScriptLogger logger)
		{
			var scriptLoggerType = GetFirstLoggerHost(assembly.GetTypes());
			if (scriptLoggerType != null)
			{
				var scriptLogger = Activator.CreateInstance(scriptLoggerType) as IScriptLoggerHost;
				scriptLogger.Execute(logger);
			}
		}

		private Type GetFirstLoggerHost(Type[] assemblyTypes)
		{
			var loggerHostType = typeof(IScriptLoggerHost);

			foreach (var type in assemblyTypes)
			{
				foreach (var face in type.GetInterfaces())
				{
					if (face.Equals(loggerHostType))
						return type;
				}
			}

			return null;
		}


	}
}

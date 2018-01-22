using ScriptCs;
using ScriptCs.Contracts;
using ScriptCs.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace quicsharp.Engine
{
	public interface ISmallConsole
	{
		void WriteLine(string message);
		void Clear();
	}

	public class ConsoleAdapter : IConsole
	{
		public ConsoleAdapter(ISmallConsole console)
		{
			Console = console ?? throw new ArgumentNullException(nameof(console));
		}

		public ConsoleColor ForegroundColor { get; set; }

		public int Width => 0;

		public ISmallConsole Console { get; }

		public void Clear()
		{
			Console.Clear();
		}

		public void Exit()
		{
		}

		public string ReadLine(string prompt)
		{
			return "";
		}

		public void ResetColor()
		{
		}

		public void Write(string value)
		{
		}

		public void WriteLine()
		{
		}

		public void WriteLine(string value)
		{
			Console.WriteLine(value);
		}
	}

	public class ScriptCsScriptRunner
	{
		public ScriptCsScriptRunner(CodePreparer preparer, ISmallConsole console)
		{
			Preparer = preparer ?? throw new ArgumentNullException(nameof(preparer));
			Console = console ?? throw new ArgumentNullException(nameof(console));
		}

		public async Task<object> Run(string code)
		{
			ScriptResult result = null;
			ScriptExecutor executor = null;

			try
			{
				var console = new ConsoleAdapter(Console);
				console.Clear();

				var logProvider = new DefaultLogProvider();

				var builder = new ScriptServicesBuilder(console, logProvider);

				builder.ScriptEngine<ScriptCs.Engine.Roslyn.CSharpScriptInMemoryEngine>(); // CSharpScriptEngine
				var services = builder.Build();

				executor = (ScriptExecutor)services.Executor;
				executor.Initialize(Enumerable.Empty<string>(), Enumerable.Empty<IScriptPack>());

				await Task.Run(() =>
				{
					result = executor.ExecuteScript(code);
				});
			}
			catch (Exception ex)
			{
				return await Task.FromResult<object>(ex);
			}

			return result;
		}

		//internal ScriptOptions DefaultScriptOptions => ScriptOptions.Default
		//	.WithImports(DefaultScriptImports)
		//	.WithReferences(DefaultScriptReferens)
		//	.WithEmitDebugInformation(true);

		internal IEnumerable<string> DefaultScriptReferens => new string[] {
			Assembly.GetExecutingAssembly().Location,
			"System.Data.Linq",		// Linq support
			"Microsoft.CSharp",		// dynamic-keyword,
			"System.Windows.Forms"
		};

		internal IEnumerable<string> DefaultScriptImports => new string[] {
			"System",
			"System.Linq",
			"System.Collections",
			"System.Collections.Generic",
			"System.Collections.Concurrent",
			"System.Console",
			"System.Diagnostics",
			"System.Globalization",
			"System.IO",
			"System.Reflection",
			"System.Runtime",
			"System.Text",
			"System.Text.Encoding",
			"System.Text.RegularExpressions",
			"System.Threading",
			"System.Threading.Tasks",
			"System.Threading.Tasks.Parallel",
			"System.Threading.Thread",
			"System.Windows.Forms",
			this.GetType().Namespace
		};

		public CodePreparer Preparer { get; }
		public ISmallConsole Console { get; }
	}
}

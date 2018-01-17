using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace quicsharp.Engine
{
	public class ScriptRunner
	{
		public ScriptRunner(CodePreparer preparer)
		{
			Preparer = preparer ?? throw new ArgumentNullException(nameof(preparer));
		}

		public async Task<object> Run(string code)
		{
			var options = DefaultScriptOptions;

			var preparationResult = Preparer.Prepare(code);
			code = preparationResult.Code;

			if (preparationResult.Imports.Any())
				options = options.AddImports(preparationResult.Imports);

			if (preparationResult.References.Any())
				options = options.AddReferences(preparationResult.References);

			try
			{
				var result = await CSharpScript.RunAsync<object>(code, options);
				return result.Variables.Select(v => new Variable(v)).ToArray();
			}
			catch (Exception ex)
			{
				return await Task.FromResult<object>(ex);
			}
		}

		internal ScriptOptions DefaultScriptOptions => ScriptOptions.Default
			.WithImports(DefaultScriptImports)
			.WithReferences(DefaultScriptReferens)
			.WithEmitDebugInformation(true);

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
			"System.Windows.Forms"
		}.Union(GetOwnNamespaces());

		private IEnumerable<string> GetOwnNamespaces()
		{
			var types = Assembly.GetExecutingAssembly().DefinedTypes;
			return types.Select(t => t.Namespace).Distinct();
		}

		public CodePreparer Preparer { get; }
	}
}

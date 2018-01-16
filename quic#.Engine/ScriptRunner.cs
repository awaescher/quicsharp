﻿using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace quicsharp.Engine
{
	public class ScriptRunner
	{
		public async Task<object> Run(string code)
		{
			try
			{
				var result = await CSharpScript.RunAsync<object>(code, DefaultScriptOptions);
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
			"System.Data.Linq"
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
			"System.Threading.Thread"
		};
	}
}
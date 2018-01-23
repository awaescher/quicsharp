using quicksharp.Engine;
using quicksharp.Engine.Errors;
using quicksharp.Engine.Interfaces;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace quicsharp.Engine
{
	public class ScriptExecutor
	{
		public IScriptLogger Logger { get; }

		public ScriptExecutor(IScriptLogger logger)
		{
			Logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public void Execute(string code)
		{
			Logger.InitLog();

			string[] codeLines = code.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

			var sourceInfo = ScriptGenerator.GetSource(codeLines);
			var compilerResult = CSharpScriptCompiler.Compile(sourceInfo);

			if (compilerResult.Errors.HasErrors)
				ShowErrors(compilerResult);
			else
				TryExecuteScript(compilerResult);
		}

		private void TryExecuteScript(CompilerResults compilerResult)
		{
			try
			{
				ExecuteCode(compilerResult.CompiledAssembly);
			}
			catch (Exception ex)
			{
				Logger.ShowErrors(ex);
			}
		}

		private void ShowErrors(CompilerResults compilerResult)
		{
			var compilerErrors = compilerResult.Errors.OfType<CompilerError>()
				.Select(compilerError => new ScriptError()
				{
					ErrorNumber = compilerError.ErrorNumber,
					Line = compilerError.Line,
					Message = compilerError.ErrorText
				})
				.ToArray();

			Logger.ShowErrors(compilerErrors);
		}

		private void ExecuteCode(Assembly assembly)
		{
			var scriptType = GetScriptType(assembly.GetTypes());
			if (scriptType != null)
			{
				var script = Activator.CreateInstance(scriptType) as IScript;
				script.Execute(Logger);
			}
		}

		private Type GetScriptType(Type[] assemblyTypes)
		{
			var script = typeof(IScript);

			foreach (var type in assemblyTypes)
			{
				foreach (var face in type.GetInterfaces())
				{
					if (face.Equals(script))
						return type;
				}
			}

			return null;
		}
	}
}

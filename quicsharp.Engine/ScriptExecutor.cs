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
		private string _engineAssemblyName;

		public ScriptExecutor(IScriptLogger logger)
		{
			_engineAssemblyName = GetType().Assembly.GetName().Name;
			Logger = logger ?? throw new ArgumentNullException(nameof(logger));
		}

		public void Execute(string code, object target)
		{
			Logger.InitLog();

			string[] codeLines = code.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

			var sourceInfo = ScriptGenerator.GetSource(codeLines);
			var compilerResult = CSharpScriptCompiler.Compile(sourceInfo);

			if (compilerResult.Errors.HasErrors)
				ShowErrors(compilerResult, sourceInfo);
			else
				TryExecuteScript(compilerResult, target);
		}

		private void TryExecuteScript(CompilerResults compilerResult, object target)
		{
			// we have to provide the assembly when the AppDomain wants to load our current one: quicsharp.Engine
			// if it does not lie next to the executable, the generated assembly won't be able to find quicsharp.Engine
			// to which it has (and needs) a reference.
			// so, wait for the AssemblyResolve-event and return our assembly manually
			// thank you so much, Hans Passant
			// https://stackoverflow.com/questions/28527384/c-sharp-assembly-path-issue-when-using-referencedassemblies
			AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;

			try
			{
				ExecuteCode(compilerResult.CompiledAssembly, target);
			}
			catch (Exception ex)
			{
				Logger.ShowErrors(ex);
			}
			finally
			{
				AppDomain.CurrentDomain.AssemblyResolve -= CurrentDomain_AssemblyResolve;
			}
		}

		private Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
		{
			if (args.Name.Contains(_engineAssemblyName))
				return GetType().Assembly;

			return null;
		}

		private void ShowErrors(CompilerResults compilerResult, SourceInfo sourceInfo)
		{
			var compilerErrors = compilerResult.Errors.OfType<CompilerError>()
				.Select(compilerError => new ScriptError()
				{
					ErrorNumber = compilerError.ErrorNumber,
					Line = sourceInfo.CalculateVisibleLineNumber(compilerError.Line),
					Message = compilerError.ErrorText
				})
				.ToArray();

			Logger.ShowErrors(compilerErrors);
		}

		private void ExecuteCode(Assembly assembly, object target)
		{
			var scriptType = assembly.GetType("quicksharp.Engine.DynamicScript");

			if (scriptType == null)
				throw new ArgumentNullException(nameof(scriptType), "Could not find generated script type: quicksharp.Engine.DynamicScript");

			var script = Activator.CreateInstance(scriptType) as IScript;
			script.Execute(Logger, target);
		}
	}
}

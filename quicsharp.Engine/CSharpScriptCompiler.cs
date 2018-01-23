using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace quicksharp.Engine
{
	internal static class CSharpScriptCompiler
	{
		internal static CompilerResults Compile(SourceInfo sourceInfo)
		{
			var codeProvider = new CSharpCodeProvider();
			var parameters = new CompilerParameters();

			parameters.ReferencedAssemblies.Add("System.dll");
			parameters.ReferencedAssemblies.Add("System.Core.dll");
			parameters.ReferencedAssemblies.Add("System.IO.dll");
			parameters.ReferencedAssemblies.Add("System.Drawing.dll");
			parameters.ReferencedAssemblies.Add("System.Xml.dll");
			parameters.ReferencedAssemblies.Add("System.Linq.dll");
			parameters.ReferencedAssemblies.Add("Microsoft.CSharp.dll");
			parameters.ReferencedAssemblies.Add("System.Windows.Forms.dll");
			parameters.ReferencedAssemblies.Add("System.Reflection.dll");
			parameters.ReferencedAssemblies.Add(Assembly.GetExecutingAssembly().Location);

			foreach (var reference in sourceInfo.References)
				parameters.ReferencedAssemblies.Add(reference);

			parameters.GenerateExecutable = false;
			parameters.GenerateInMemory = true;

			return codeProvider.CompileAssemblyFromSource(parameters, sourceInfo.SourceCode);
		}
	}
}

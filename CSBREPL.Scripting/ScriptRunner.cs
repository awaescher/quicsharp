using Microsoft.CodeAnalysis.CSharp.Scripting;
using System;
using System.Threading.Tasks;

namespace CSBREPL.Scripting
{
	public static class ScriptRunner
	{
		public static Task<object> Run(string code)
		{
			try
			{
				return CSharpScript.EvaluateAsync(code);
			}
			catch (Exception ex)
			{
				return Task.FromException<object>(ex);	
			}
		}
	}
}

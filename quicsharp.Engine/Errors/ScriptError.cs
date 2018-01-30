using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace quicksharp.Engine.Errors
{
	[DebuggerDisplay("{ErrorNumber} @{Line}: {Message}")]
	public class ScriptError
	{
		public string Message { get; set; }

		public string ErrorNumber { get; set; }

		public int Line { get; set; }

		public object Data { get; set; }

		public override string ToString()
		{
			var sb = new StringBuilder();

			if (!string.IsNullOrEmpty(ErrorNumber))
				sb.Append($"#{ErrorNumber}: ");

			if (Line > 0)
				sb.Append($"@{Line}: ");

			sb.Append(Message);

			return sb.ToString();
		}

		internal static ScriptError From(Exception ex)
		{
			return new ScriptError()
			{
				ErrorNumber = "",
				Line = 0,
				Message = ex.Message,
				Data = ex
			};
		}

		internal static ScriptError From(CompilerError error)
		{
			return new ScriptError()
			{
				ErrorNumber = error.ErrorNumber,
				Line = error.Line,
				Message = error.ErrorText,
				Data = error
			};
		}
	}
}

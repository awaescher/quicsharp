using System;
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
	}
}

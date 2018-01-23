using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace quicksharp.Engine.Errors
{
	public class ScriptError
	{
		public string Message { get; set; }

		public string ErrorNumber { get; set; }

		public int Line { get; set; }
	}
}

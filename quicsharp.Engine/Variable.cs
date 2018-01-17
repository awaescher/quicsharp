using Microsoft.CodeAnalysis.Scripting;
using System;
using System.Diagnostics;

namespace quicsharp.Engine
{
	[DebuggerDisplay("{Name}: {Value}")]
	public class Variable
	{
		public Variable(ScriptVariable scriptVariable)
		{
			IsReadOnly = scriptVariable.IsReadOnly;
			Name = scriptVariable.Name;
			Type = scriptVariable.Type;
			Value = scriptVariable.Value;
		}

		public bool IsReadOnly { get; }

		public string Name { get; }

		public Type Type { get; }

		public object Value { get; }
	}
}

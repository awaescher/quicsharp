using Microsoft.CodeAnalysis.Scripting;
using System;

namespace CSBREPL.Engine
{
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

using System;
using System.Collections.Generic;
using System.Linq;

namespace CSBREPL.Engine
{
	public class VariableStringRenderer
	{
		public string Render(IEnumerable<Variable> variables)
		{
			return string.Join(Environment.NewLine, variables.Select(v => Render(v)));
		}

		public string Render(Variable variable)
		{
			if (variable == null)
				return "(null)";

			return $"{variable.Name}: {variable.Value ?? "(null)"}";
		}
	}
}

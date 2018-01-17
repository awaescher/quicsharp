using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace quicsharp.Engine
{
	public class VariableStringRenderer
	{
		public string Render(IEnumerable<Variable> variables)
		{
			return string.Join(Environment.NewLine, 
				variables
					.Where(v => ShouldRender(v))
					.Select(v => Render(v))
				);
		}

		public virtual bool ShouldRender(Variable variable)
		{
			if (variable == null)
				return false;

			return variable.Name.Length > 0 && variable.Name[0] != '_';
		}

		public virtual string Render(Variable variable)
		{
			return $"{variable.Name}: {variable.Value ?? "(null)"}";
		}
	}
}

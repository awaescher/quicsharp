using Humanizer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace quicsharp.Engine
{
	public class HumanizerStringRenderer : VariableStringRenderer
	{
		public override string Render(Variable variable)
		{
			var value = "(null)";

			if (variable.Value != null)
			{
				if (variable.Value is IEnumerable<object> enumerable)
					value = enumerable.Humanize();
				else
					value = variable.Value.ToString();
			}

			return $"{variable.Name}: {value}";
		}
	}
}

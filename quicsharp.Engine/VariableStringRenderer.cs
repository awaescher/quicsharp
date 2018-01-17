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
			var value = RenderValue(variable.Value);

			if (value.Contains(Environment.NewLine))
				return $"{Environment.NewLine}{variable.Name}:{Environment.NewLine}{value}{Environment.NewLine}";

			return $"{variable.Name}: {value}";
		}

		public virtual string RenderValue(object value)
		{
			var indent = IndentMultiline ? "\t" : "";

			if (value is IEnumerable<string> strings)
				return string.Join(Environment.NewLine, strings.Select(s => indent + s));
			else if (value is IEnumerable<object> objects)
				return string.Join(Environment.NewLine, objects.Select(o => indent + RenderValue(o)).ToArray());

			return value.ToString();
		}

		protected virtual bool IndentMultiline => true;
	}
}

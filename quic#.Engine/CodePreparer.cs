using System;
using System.Collections.Generic;

namespace quicsharp.Engine
{
	public class CodePreparer
	{
		public PreparationResult Prepare(string code)
		{
			var result = new PreparationResult();

			var lines = code.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
			var newLines = new List<string>();

			foreach (var line in lines)
			{
				if (line.StartsWith("#i"))
					result.Imports.Add(line.Substring(3));
				else if (line.StartsWith("#r"))
					result.References.Add(line.Substring(3));
				else
					newLines.Add(line);
			}

			result.Code = string.Join(Environment.NewLine, newLines);

			return result;
		}
	}
}

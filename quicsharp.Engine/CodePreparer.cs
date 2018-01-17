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

			for (int i = 0; i < lines.Length; i++)
			{
				var line = lines[i];

				// Note: Comment lines out instead of removing them to keep the line numbers the same
				//		 for the user and the compiler. This is important for error messages with line numbers.
				if (line.StartsWith("#i"))
				{
					result.Imports.Add(line.Substring(3));
					lines[i] = "//" + line;
				}
				else if (line.StartsWith("#r"))
				{
					result.References.Add(line.Substring(3));
					lines[i] = "//" + line;
				}
			}

			result.Code = string.Join(Environment.NewLine, lines);

			return result;
		}
	}
}

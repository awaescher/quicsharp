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
				var cleanLine = lines[i].TrimStart();
				string referenceName = null;

				if (cleanLine.StartsWith("#ri") || cleanLine.StartsWith("#ir"))
				{
					referenceName = RemoveComments(cleanLine.Substring(4));
					result.References.Add(referenceName);
					result.Imports.Add(referenceName);
				}
				else if (cleanLine.StartsWith("#i"))
				{
					referenceName = RemoveComments(cleanLine.Substring(3));
					result.Imports.Add(referenceName);
				}
				else if (cleanLine.StartsWith("#r"))
				{
					referenceName = RemoveComments(cleanLine.Substring(3));
					result.References.Add(referenceName);
				}

				// Note: Comment lines out instead of removing them to keep the line numbers the same
				//		 for the user and the compiler. This is important for error messages with line numbers.
				if (referenceName != null)
					lines[i] = "//" + lines[i];
			}

			result.Code = string.Join(Environment.NewLine, lines);

			return result;
		}

		private string RemoveComments(string line)
		{
			var commentStartIndex = line.IndexOf("//");

			if (commentStartIndex < 0)
				commentStartIndex = line.IndexOf("/*");

			return commentStartIndex < 0 ? line : line.Substring(0, commentStartIndex).Trim();
		}
	}
}

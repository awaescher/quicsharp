using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace quicsharp.Engine.LinePreprocessors
{
	internal class CommentRemover : IPreprocessor
	{
		public void Process(ref string[] lines)
		{
			bool opensMultilineComment = false;
			bool closesMultilineComment = false;
			bool isMultilineCommentStillOpen = false;

			for (int i = 0; i < lines.Length; i++)
			{
				lines[i] = Regex.Replace(lines[i], @"(//.*)$", "");

				isMultilineCommentStillOpen = opensMultilineComment || isMultilineCommentStillOpen; // check from prior itertion

				lines[i] = Regex.Replace(lines[i], @"(/\*.*?\*/)", "");

				opensMultilineComment = lines[i].Contains("/*");
				closesMultilineComment = lines[i].Contains("*/");

				if (opensMultilineComment)
				{
					// remove opening comment (without ending in-line)
					// CODE /* COMMENT
					lines[i] = Regex.Replace(lines[i], @"(/\*.*)$", "");
				}
				else if (closesMultilineComment)
				{
					// closing line - remove clsoing comment (without opening in-line)
					// COMMENT */ CODE
					lines[i] = Regex.Replace(lines[i], @"^(.*\*/)", "");
					isMultilineCommentStillOpen = false;
				}

				if (isMultilineCommentStillOpen && !closesMultilineComment)
				{
					lines[i] = ""; // normal comment line
				}
			}
		}
	}
}

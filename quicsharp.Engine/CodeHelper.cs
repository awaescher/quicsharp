using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace quicsharp.Engine
{
	public static class CodeHelper
	{
		public static string[] ToLines(this string code) => code.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

		public static string EnsureSemicolon(this string value) => value.TrimEnd().EndsWith(";") ? value : value + ";";

		public static string RemoveSemicolon(this string value) => value.LastIndexOf(';') > -1 ? value.Substring(0, value.LastIndexOf(';')).TrimEnd() : value;
	}
}

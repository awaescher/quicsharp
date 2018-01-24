using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace quicsharp.Tests
{
	public static class CodeHelper
	{
		public static string[] ToLines(this string code) => code.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
	}
}

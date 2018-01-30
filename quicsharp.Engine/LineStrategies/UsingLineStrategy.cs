using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using quicksharp.Engine;

namespace quicsharp.Engine.LineStrategies
{
	internal class UsingLineStrategy : LineStrategy
	{
		internal override bool IsResponsible(string line)
		{
			return line.TrimStart().StartsWith("#using", StringComparison.OrdinalIgnoreCase);
		}

		internal override bool ShouldSkip(string line) => true;

		internal override void ExtendSourceIfApplicable(SourceInfo sourceInfo, string line)
		{
			var referenceName = Regex.Replace(line, @"^(.*#using\s*)", "");
			var usingLine = $"using {referenceName}".EnsureSemicolon();
			sourceInfo.Usings.Add(usingLine);
		}
	}
}

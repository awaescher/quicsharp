using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using quicksharp.Engine;

namespace quicsharp.Engine.LineStrategies
{
	internal class ReferenceLineStrategy : LineStrategy
	{
		internal override bool IsResponsible(string line)
		{
			return line.TrimStart().StartsWith("#reference", StringComparison.OrdinalIgnoreCase);
		}

		internal override bool ShouldSkip(string line) => true;

		internal override void ExtendSourceIfApplicable(SourceInfo sourceInfo, string line)
		{
			var referenceName = Regex.Replace(line, @"^(.*#reference\s*)", "");
			sourceInfo.References.Add(referenceName);
		}
	}
}

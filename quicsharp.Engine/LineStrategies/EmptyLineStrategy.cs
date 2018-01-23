using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace quicsharp.Engine.LineStrategies
{
	internal class EmptyLineStrategy : LineStrategy
	{
		internal override bool IsResponsible(string line)
		{
			return line.Trim().StartsWith("//");
		}

		internal override bool ShouldSkip(string line) => true;

		internal override LoggerLineInfo GetLoggerInfoIfApplicable(string line) => null;
	}
}

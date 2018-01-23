using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace quicsharp.Engine.LineStrategies
{
	internal class InspectLineStrategy : LineStrategy
	{
		internal override bool IsResponsible(string line)
		{
			return line.Trim().StartsWith("*");
		}

		internal override bool ShouldSkip(string line) => false;

		internal override LoggerLineInfo GetLoggerInfoIfApplicable(string line)
		{
			var cleanedLine = line.TrimStart().Substring(1);
			var viewString = cleanedLine.Trim();

			var name = "Inspect: " + viewString;

			var end = viewString.LastIndexOf(';');
			if (end > 0)
				viewString = viewString.Substring(0, end).TrimEnd();
			viewString = "RuntimeHelper.Inspect(" + viewString + ")";

			var value = viewString;

			return new LoggerLineInfo(name, value);
		}
	}
}

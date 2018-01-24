using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace quicsharp.Engine.LineStrategies
{
	internal class PrintLineStrategy : LineStrategy
	{
		internal override bool IsResponsible(string line)
		{
			return line.Trim().StartsWith("?");
		}

		internal override bool ShouldSkip(string line) => false;

		internal override LoggerLineInfo GetLoggerInfoIfApplicable(string line)
		{
			var cleanedLine = line.TrimStart().Substring(1);

			var nameAndValue = cleanedLine.Trim();

			var end = nameAndValue.LastIndexOf(';');
			if (end > 0)
				nameAndValue = nameAndValue.Substring(0, end).TrimEnd();

			return new LoggerLineInfo(nameAndValue, nameAndValue);
		}
	}
}

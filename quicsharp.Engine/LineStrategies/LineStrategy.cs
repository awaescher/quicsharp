using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace quicsharp.Engine.LineStrategies
{
	internal abstract class LineStrategy
	{
		internal abstract bool IsResponsible(string line);

		internal abstract bool ShouldSkip(string line);

		internal abstract LoggerLineInfo GetLoggerInfoIfApplicable(string line);
	}
}

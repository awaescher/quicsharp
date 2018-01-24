using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using quicksharp.Engine;

namespace quicsharp.Engine.LineStrategies
{
	internal abstract class LineStrategy
	{
		internal abstract bool IsResponsible(string line);

		internal abstract bool ShouldSkip(string line);

		internal virtual LoggerLineInfo GetLoggerInfoIfApplicable(string line) => null;

		internal virtual void ExtendSourceIfApplicable(SourceInfo sourceInfo, string line)
		{
		}
	}
}

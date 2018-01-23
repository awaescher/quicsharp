using System;
using System.Collections.Generic;
using System.Text;

namespace quicksharp.Engine.Interfaces
{
	public interface IScriptLoggerHost
	{
		void Execute(IScriptLogger logger);
	}
}

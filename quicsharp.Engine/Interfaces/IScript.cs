using System;
using System.Collections.Generic;
using System.Text;

namespace quicksharp.Engine.Interfaces
{
	public interface IScript
	{
		void Execute(IScriptLogger logger, object target);
	}
}

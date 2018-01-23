using quicksharp.Engine.Errors;
using quicksharp.Engine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace quicksharp.Engine.Loggers
{
	public abstract class ScriptLogger : IScriptLogger
	{
		private bool _hasLogs = false;

		public ScriptLogger()
		{
		}

		public virtual void InitLog()
		{
			_hasLogs = false;
		}

		public virtual void EndLog()
		{
			if (!_hasLogs)
				TryLog(null, "(no logs.)");
		}

		public void TryLog(string expression, object value)
		{
			try
			{
				Log(expression, value);

				_hasLogs = true;
			}
			catch (Exception ex)
			{
				ShowErrors(ex);
			}
		}

		public abstract void Log(string expression, object value);

		public abstract void ShowErrors(params ScriptError[] errors);

		public virtual void ShowErrors(params Exception[] exceptions)
		{
			ShowErrors(exceptions.Select(ex => ScriptError.From(ex)).ToArray());
		}
	}
}

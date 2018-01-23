using quicksharp.Engine.Errors;
using quicksharp.Engine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace quicksharp.Engine.Loggers
{
	public abstract class BaseScriptLogger : IScriptLogger
	{
		private bool _hasLogs = false;

		public BaseScriptLogger()
		{
		}

		public void InitLog()
		{
			_hasLogs = false;
		}

		public void EndLog()
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

		public virtual void ShowErrors(params ScriptError[] errors)
		{
			ShowErrors(errors.Select(e => e.Message).ToArray());
		}

		public virtual void ShowErrors(params Exception[] errors)
		{
			ShowErrors(errors.Select(e => e.Message).ToArray());
		}

		public abstract void ShowErrors(params string[] errors);
	}
}

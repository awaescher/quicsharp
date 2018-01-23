using quicksharp.Engine.Errors;
using System;
using System.Collections.Generic;
using System.Text;

namespace quicksharp.Engine.Interfaces
{
	public interface IScriptLogger
	{
		void InitLog();

		void TryLog(string expression, object value);

		void EndLog();

		void ShowErrors(params ScriptError[] errors);

		void ShowErrors(params Exception[] exceptions);
	}
}

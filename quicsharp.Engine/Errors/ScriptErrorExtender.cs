using System;
using System.Collections.Generic;
using System.Text;

namespace quicksharp.Engine.Errors
{
	public static class ScriptErrorExtender
	{
		private static Dictionary<string, string> _additions = null;

		static ScriptErrorExtender()
		{
			_additions = new Dictionary<string, string>();
			_additions.Add("CS0117", "Try a leading \"!\" to enforce the resolver to check a member, like \"?_target.Tag.!Name.Length\" if _target.Tag is of type object containing a value having a property \"Name\".");
		}

		public static void TryExtend(ScriptError error)
		{
			var addition = GetAddition(error.ErrorNumber);
			if (!string.IsNullOrEmpty(addition))
				error.Message += ". " + addition;
		}

		public static string GetAddition(string errorNumber)
		{
			if (_additions.ContainsKey(errorNumber))
				return _additions[errorNumber];
			return null;
		}
	}
}

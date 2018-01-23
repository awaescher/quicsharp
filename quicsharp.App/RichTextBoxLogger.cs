using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace quicksharp.Engine.Loggers
{
	public class TextBoxScriptLogger : ScriptLogger
	{
		private ScintillaNET.Scintilla _box = null;

		public TextBoxScriptLogger(ScintillaNET.Scintilla box)
		{
			_box = box;

			if (box != null)
				box.ResetText();
		}

		public override void InitLog()
		{
			base.InitLog();

			_box.ResetText();
		}

		public override void Log(string expression, object value)
		{
			string log = null;

			if (value != null && value.GetType() != typeof(string))
			{
				var enumerable = value as IEnumerable;
				if (enumerable != null)
				{
					// #lambda value = string.Join(Environment.NewLine, enumerable);

					var sb = new StringBuilder();

					foreach (var item in enumerable)
					{
						if (item == null)
							sb.AppendLine("(null)");
						else
							sb.AppendLine(item.ToString());
					}
					value = sb.ToString();
				}
			}

			if (value == null)
				value = "(null)";
			if (value == DBNull.Value)
				value = "(DBNull)";

			if (!string.IsNullOrEmpty(expression) && value != null)
				log = ">> " + expression + Environment.NewLine + value.ToString();
			else
			{
				if (!string.IsNullOrEmpty(expression))
					log = ">> " + expression;
				else
					log = value.ToString();
			}

			_box.AppendText(log + Environment.NewLine + Environment.NewLine);
		}

		public override void ShowErrors(params string[] errors)
		{
			if (_box == null)
				throw new NullReferenceException("_box is not set!");

			_box.ResetText();
			_box.Text = string.Join(Environment.NewLine, errors);
		}
	}

}

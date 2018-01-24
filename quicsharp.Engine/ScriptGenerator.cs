using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using quicsharp.Engine.LineStrategies;
using quicsharp.Engine.LinePreprocessors;

namespace quicksharp.Engine
{
	internal static class ScriptGenerator
	{
		private static IPreprocessor[] _preprocessors = new IPreprocessor[] {
				new CommentRemover()
			};

		private static LineStrategy[] _lineStrategies = new LineStrategy[] {
				new EmptyLineStrategy(),
				new InspectLineStrategy(),
				new PrintLineStrategy()
			};

		private const string SOURCE = @"
using System;
using System.IO;
using System.Text;
using System.Reflection;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using quicksharp.Engine.Errors;
using quicksharp.Engine.Interfaces;
using quicksharp.Engine.Loggers;
%USINGS%

namespace quicksharp.Engine
{
    public class DynamicScript : IScript
    {

	public void Execute(IScriptLogger logger)
        {
            try
            {

                logger.InitLog();
        
%LINES%
            }
            catch(Exception ex)
            {
                logger.ShowErrors(ex);
            }
            finally
            {
                logger.EndLog();
            }
        }
    }
}
";

		internal static SourceInfo GetSource(string[] lines)
		{
			var info = new SourceInfo();

			string indent = "\t\t";

			var sb = new StringBuilder();

			foreach (var preprocessor in _preprocessors)
				preprocessor.Process(ref lines);

			foreach (var line in lines)
			{
				var strategy = _lineStrategies.FirstOrDefault(s => s.IsResponsible(line));

				LoggerLineInfo loggerInfo = null;

				if (strategy != null)
				{
					if (strategy.ShouldSkip(line))
					{
						sb.AppendLine("");
						continue;
					}

					loggerInfo = strategy.GetLoggerInfoIfApplicable(line);
				}

				if (loggerInfo == null)
				{
					// this is any "normal" code, nothing to log but process normally. Like loops and ifs, etc.
					sb.AppendLine(indent + line.TrimStart());
				}
				else
				{
					var displayNameWithQuotationMarks = loggerInfo.DisplayName.Replace("\"", "\\" + "\"");
					sb.AppendLine(indent + $"logger.TryLog(\"{displayNameWithQuotationMarks}\", {loggerInfo.Value});");
				}

				/*

				string expressionString = line.TrimStart();

				bool isEmpty = string.IsNullOrEmpty(line.Trim());
				bool isComment = expressionString.StartsWith("//");

				bool shouldSkip = isEmpty || isComment;

				if (shouldSkip)
				{
					sb.AppendLine("");
					continue;
				}

				string valueString = expressionString;

				var sendLineToLogger = true;

				if (expressionString.Length > 1 && expressionString.StartsWith("#"))
				{
					// insert namespace "#System.Windows.Forms"
					string usingString = expressionString.Substring(1).Trim();
					if (!usingString.StartsWith("using", StringComparison.OrdinalIgnoreCase))
						usingString = "using " + usingString;

					if (!info.Usings.Contains(usingString))
						info.Usings.Add(usingString);
				}
				else if (expressionString.Length > 1 && expressionString.StartsWith("$"))
				{
					// insert namespace "$System.Windows.Forms.dll"
					string reference = expressionString.Substring(1).Trim();
					if (!info.References.Contains(reference))
						info.References.Add(reference);
				}
				else
				{
					if (line.Contains("::"))
					{
						// "....::....." --> named variable

						string[] parts = line.Split(new string[] { "::" }, StringSplitOptions.RemoveEmptyEntries);

						if (parts.Length == 2)
						{
							expressionString = parts[0].Trim();
							valueString = parts[1].TrimStart();
						}
					}
										else if (expressionString.Length > 1 && expressionString.StartsWith("?"))
										{
											// "?..." --> Print
											valueString = expressionString.Substring(1).Trim();
											var end = valueString.LastIndexOf(';');
											if (end > 0)
												valueString = valueString.Substring(0, end).TrimEnd();
											expressionString = valueString;
										}
										else if (expressionString.Length > 1 && expressionString.StartsWith("*"))
										{
											// "*..." --> Inspect

											string viewString = expressionString.Substring(1).Trim();

											expressionString = "Inspect: " + viewString;

											var end = viewString.LastIndexOf(';');
											if (end > 0)
												viewString = viewString.Substring(0, end).TrimEnd();
											viewString = "RuntimeHelper.Inspect(" + viewString + ")";

											valueString = viewString;
										}
					else
					{
						// this is any "normal" code, nothing to log but process normally. Like loops and ifs, etc.

						sb.AppendLine(indent + expressionString);
						sendLineToLogger = false;
					}

					if (sendLineToLogger)
					{
						expressionString = expressionString.Replace("\"", "\\" + "\"");
						sb.AppendLine(string.Format(indent + "logger.TryLog(\"{0}\", {1});", expressionString, valueString));
					}

				}
					*/

			}

			info.SourceCode = sb.ToString();
			ResolveSource(ref info);

			return info;
		}

		internal static void ResolveSource(ref SourceInfo sourceInfo)
		{
			string usingString = "";
			if (sourceInfo.Usings.Any())
				usingString = string.Join(Environment.NewLine, sourceInfo.Usings.ToArray());

			int lineNumberOffsetFromTemplate = -1;
			var templateLines = SOURCE.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
			for (int i = 0; i < templateLines.Length; i++)
			{
				if (templateLines[i] == "%LINES%")
				{
					lineNumberOffsetFromTemplate = i;
					break;
				}
			}

			if (lineNumberOffsetFromTemplate == -1)
				throw new InvalidOperationException("Code template is not valid.");

			sourceInfo.LineNumberOffsetFromTemplate = lineNumberOffsetFromTemplate;
			sourceInfo.SourceCode = SOURCE.Replace("%LINES%", sourceInfo.SourceCode).Replace("%USINGS%", usingString);
		}


	}
}

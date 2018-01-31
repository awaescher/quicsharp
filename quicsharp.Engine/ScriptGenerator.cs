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
				new ReferenceLineStrategy(),
				new UsingLineStrategy(),
				new EmptyLineStrategy(),
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

	public void Execute(IScriptLogger logger, object target)
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
			var sourceInfo = new SourceInfo();

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
					strategy.ExtendSourceIfApplicable(sourceInfo, line);

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

			}

			sourceInfo.SourceCode = sb.ToString();
			ResolveSource(ref sourceInfo);

			return sourceInfo;
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

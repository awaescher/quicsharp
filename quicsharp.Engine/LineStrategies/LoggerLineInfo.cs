namespace quicsharp.Engine.LineStrategies
{
	internal class LoggerLineInfo
	{
		public LoggerLineInfo(string displayName, string value)
		{
			DisplayName = displayName;
			Value = value;
		}

		internal string DisplayName { get;  }
		internal string Value { get; }
	}
}
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace quicsharp.Engine.LineStrategies
{
	internal class EmptyLineStrategy : LineStrategy
	{
		internal override bool IsResponsible(string line)
		{
			return line == null || string.IsNullOrEmpty(line.Trim());
		}

		internal override bool ShouldSkip(string line) => true;
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace quicsharp.Engine.LinePreprocessors
{
	internal interface IPreprocessor
	{
		void Process(ref string[] lines);
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace quicsharp.Engine
{
	public class PreparationResult
	{
		public PreparationResult()
		{
			Imports = new List<string>();
			References = new List<string>();
		}

		public string Code { get; set; }

		public List<string> Imports { get; set; }

		public List<string> References { get; set; }
	}
}

#define VERBOSE
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoPipeline
{
	static class Console2
	{
		public static void WriteLine (params string[]p)
		{
#if VERBOSE
			Console.WriteLine (p);
#endif
		}
	}
}

using System;
using System.IO;
using System.Text;
using Newtonsoft.Json.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace NoPipeline
{
	
	class Program
	{
		public const string Version = "1.1.0.2";
		
		static void Main(string[] args)
		{
			Console2.WriteLine("NoPipeline v" + Version);

			// Print help information if parameter was not provided.
			if (args.Length != 1)
			{
				PrintHelp();
				return;
			}
			
			var configPath = Path.Combine(Environment.CurrentDirectory, args[0].Replace("\\", "/"));

			Run(configPath);

			#if DEBUG
			//	Console.ReadKey();
			#endif
		}



		static void Run(string configPath)
		{
			
			// Read config file name from the input parameter.

			string MGCBConfigPath, NPLConfigPath;

			if (configPath.EndsWith(".mgcb"))
			{
				MGCBConfigPath = configPath;
				NPLConfigPath = Path.ChangeExtension(configPath, ".npl");
			}
			else
			{
				NPLConfigPath = configPath;
				MGCBConfigPath = Path.ChangeExtension(configPath, ".mgcb");
			}

			// Check if configuration file exists.
			if (!File.Exists(NPLConfigPath) || !File.Exists(NPLConfigPath))
			{
				Console2.WriteLine(NPLConfigPath + " not found!");
				PrintHelp();
				return;
			}

			var content = new Content();
			
			// Create MGCB object to read mgcb file.
			var MGCBReader = new MGCBConfigReader();
			MGCBReader.Read(content, MGCBConfigPath);

			Console2.WriteLine();
			Console2.WriteLine("-------------------------------------");
			Console2.WriteLine();

			// Create ContentProcessor object to read config file and update content
			// content will be overwrited from config file.
			var NPLReader = new NPLConfigReader();
			NPLReader.Read(content, NPLConfigPath);

			Console2.WriteLine("-------------------------------------");
			Console2.WriteLine();

			// Check all rules in content object and update timestamp of files if required.
			content.CheckIntegrity(Path.GetDirectoryName(MGCBConfigPath));

			// Saving MGCB file.

			Console2.WriteLine();
			Console2.WriteLine("-------------------------------------");
			Console2.WriteLine();

			Console2.WriteLine("Saving new config as " + MGCBConfigPath);
			Console2.WriteLine();

			File.WriteAllText(MGCBConfigPath, content.Build());

			Console.ForegroundColor = ConsoleColor.Green;
			Console2.WriteLine("Done! \\^u^/");
			Console.ForegroundColor = ConsoleColor.Gray;

		}

		/// <summary>
		/// Prints help message.
		/// </summary>
		static void PrintHelp()
		{
			Console2.WriteLine("Run with path to .mgcb or .npl config as an argument:");
			Console2.WriteLine("    NoPipeline.exe Content/Content.mgcb");
			Console2.WriteLine("or");
			Console2.WriteLine("    NoPipeline.exe Content/Content.npl");
		}

	}
}

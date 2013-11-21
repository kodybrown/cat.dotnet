using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cat.csharp
{
	public class csharp : ICataloger
	{
		public string Name { get { return _name; } }
		private string _name = "csharp";

		public string Description { get { return _description; } }
		private string _description = "C# source files (.cs).";

		public bool CanCat( CatOptions catOptions, string fileName )
		{
			return Path.GetExtension(fileName).Equals(".cs", StringComparison.CurrentCultureIgnoreCase);
		}

		public bool Cat( CatOptions catOptions, string fileName )
		{
			return Cat(catOptions, fileName, 0, long.MaxValue);
		}

		public bool Cat( CatOptions catOptions, string fileName, int lineStart, long linesToWrite )
		{
			int lineNumber;
			int padLen;
			int winWidth = Console.WindowWidth - 1;
			string l, lt;

			lineStart = Math.Max(lineStart, 0);
			lineNumber = 0;
			padLen = catOptions.showLineNumbers ? 3 : 0;
			if (linesToWrite < 0) {
				linesToWrite = long.MaxValue;
			}

			using (StreamReader reader = File.OpenText(fileName)) {
				while (!reader.EndOfStream) {
					l = reader.ReadLine();
					lt = l.Trim();
					lineNumber++;

					if (lineNumber < lineStart) {
						continue;
					}

					if (catOptions.ignoreLines.Length > 0 && l.StartsWith(catOptions.ignoreLines, StringComparison.CurrentCultureIgnoreCase)) {
						continue;
					} else if (catOptions.ignoreBlankLines && l.Length == 0) {
						continue;
					} else if (catOptions.ignoreWhitespaceLines && lt.Length == 0) {
						continue;
					}

					if (catOptions.showLineNumbers) {
						Console.BackgroundColor = catOptions.lineNumBackColor;
						Console.ForegroundColor = catOptions.lineNumForeColor;
						Console.Write("{0," + padLen + "}", lineNumber);
						Console.BackgroundColor = catOptions.defaultBackColor;
						Console.ForegroundColor = catOptions.defaultForeColor;
					}

					if (lt.Length > 0) {
						if (catOptions.wrapText) {
							Console.WriteLine(Bricksoft.PowerCode.Text.Wrap(l.TrimEnd(), winWidth, 0, padLen));
						} else {
							Console.WriteLine(l.TrimEnd());
						}
					} else {
						Console.WriteLine("  ");
					}

					if (lineNumber >= linesToWrite) {
						break;
					}
				}

				reader.Close();
			}

			return true;
		}
		
	}
}

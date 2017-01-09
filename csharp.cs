/*!
	Copyright (C) 2008-2013 Kody Brown (kody@bricksoft.com).

	MIT License:

	Permission is hereby granted, free of charge, to any person obtaining a copy
	of this software and associated documentation files (the "Software"), to
	deal in the Software without restriction, including without limitation the
	rights to use, copy, modify, merge, publish, distribute, sublicense, and/or
	sell copies of the Software, and to permit persons to whom the Software is
	furnished to do so, subject to the following conditions:

	The above copyright notice and this permission notice shall be included in
	all copies or substantial portions of the Software.

	THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
	IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
	FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
	AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
	LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
	FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
	DEALINGS IN THE SOFTWARE.
*/

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

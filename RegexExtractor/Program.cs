using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

class Program
{
	const string inputFilePath = "input.txt";
	const string outputFilePath = "output.txt";
	const string patternsFilePath = "patterns.txt";
	static string[] patterns;

	static void Main()
	{
		patterns = File.ReadAllLines(patternsFilePath)
			.Where(x => !string.IsNullOrEmpty(x))
			.ToArray();

		using (StreamWriter writer = new StreamWriter(outputFilePath))
		{
			foreach (var capture in GetAllLines()
					.AsParallel()
					.AsOrdered()
					.Select(x => string.Join("\t", GetCaptures(x)))
					.Where(x => x != string.Empty))
			{
				writer.WriteLine(capture);
			}
		}
	}

	static IEnumerable<string> GetAllLines()
	{
		using (StreamReader reader = new StreamReader(inputFilePath))
		{
			string line;
			while ((line = reader.ReadLine()) != null)
			{
				yield return line;
			}
		}
	}

	static IEnumerable<string> GetCaptures(string line)
	{
		foreach (string pattern in patterns)
		{
			Regex regex = new Regex(pattern);
			Match match = regex.Match(line);

			if (match.Success)
			{
				foreach (var grp in match.Groups.Cast<Group>().Skip(1))
				{
					yield return grp.Value;
				}
			}
		}
	}
}

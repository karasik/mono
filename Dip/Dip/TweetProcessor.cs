using System;
using System.Web.Script.Serialization;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace Dip.Util
{
	public static class TweetProcessor
	{
		public static void JsonToText (string inputFilename, string outputFilename)
		{
			var f = new StreamReader(inputFilename);
			var t = new StreamWriter(outputFilename);
			var serializer = new JavaScriptSerializer ();
			int c = 0;
			while (true)
			{
				string s = f.ReadLine();
				if (s == null)
				{
					break;
				}
				string v = serializer.Deserialize<Tweet>(s).text_norm;
				t.WriteLine (v);

				// Debug information.
				Console.Clear();
				Console.WriteLine ("Tweets processed: {0}", c++);
			}
			f.Close();
			t.Close();
		}

		public static List<string> GetFilteredDataFromText(string inputFilename, string[] keywords)
		{
			var data = GetDataFromText(inputFilename);
			var ret = new List<string> ();
			foreach (var s in data)
			{
				string[] words = s.Split (' ');
				if (words.Any(x => keywords.Contains(x)))
				{
					ret.Add (s.Replace("#", ""));
				}
			}
			return ret;
		}

		public static List<string> GetDataFromText(string inputFilename)
		{
			var f = new StreamReader (inputFilename);
			var ret = new List<string> ();
			while (true)
			{
				string s = f.ReadLine ();
				if (s == null)
				{
					break;
				}
				ret.Add (s.Replace("#", ""));
			}
			f.Close ();
			return ret;
		}

		public static void WriteToFile(IEnumerable<String> data, string outputFilename)
		{
			var t = new StreamWriter (outputFilename);
			foreach (string s in data)
			{
				t.WriteLine(s);
			}
			t.Close ();
		}

		private class Tweet
		{
			public string text_norm { get; set; }
		}
	}
}


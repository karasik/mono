using Dip.Util;
using System;
using System.Collections.Generic;

namespace Dip
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			// TweetProcesser.JsonToText ("../../data/TexasDataProc.data.uncompressed", "../../data/TextTexasData.txt");
			/*
			string[] disasters = new string[] {
				"flood",
				"erupt",
				"tsunami",
				"blizzard",
				"drought",
				"storm",
				"tornado",
				"epidem",
				"hurrican",
				"cyclon",
				"disast",
				"catastr",
				"thunderstorm",
				"earthquak"
			};

			List<string> data = TweetProcessor.GetFilteredDataFromText ("../../data/TextTexasData.txt", disasters);
			TweetProcessor.WriteToFile (data, "../../data/DisasterTweets.txt");
			*/

			var data = TweetProcessor.GetDataFromText ("../../data/DisasterTweets.txt");

			Console.WriteLine ("There are {0} tweets.", data.Count);
			var l = new LDA (data, 10);
			Console.WriteLine ("Starting inference...");

			l.Inference (500);

			Console.WriteLine ("Done!");

		}
	}
}

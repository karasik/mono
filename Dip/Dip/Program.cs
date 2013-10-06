using Dip.Util;
using System;
using System.Collections.Generic;
using System.Linq;

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
			int topicCount = 50;
			int topTermsCount = 10;

			Console.WriteLine ("There are {0} tweets.", data.Count);
			var l = new LDA (data, topicCount);
			Console.WriteLine ("Starting inference...");

			l.Inference (500);

			for (int topic=0; topic<topicCount; topic++)
			{
				Console.WriteLine ("{0}:", topic);
				string[] ret = l.GetTopTermsForTopic (topic, topTermsCount);
				foreach (var s in ret) {
					Console.Write ("{0} ", s);
				}
				Console.WriteLine ();
			}


			Console.WriteLine ("Done!");

		}
	}
}

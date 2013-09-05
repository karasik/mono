using System.Collections.Generic;
using System.Linq;
using System;

namespace Dip
{
	public class LDA
	{
		// Number of words.
		private int termCount;

		// Number of documents.
		private int documentCount;

		// Number of topics.
		private int topicCount;

		// Documents itself.
		private int[][] documents;

		// Mapping from int to words.
		private string[] intToWordMap;

		// Is inference performed.
		private bool inferenced;

		// Document-topic (documentCount x topicCount) distribution.
		private double[,] theta;

		// Topic-term (topicCount x termCount) distribution.
		private double[,] phi;

		public LDA (List<string> data, int k)
		{
			topicCount = k;
			documentCount = data.Count;
			string[][] words = data.Select ((string s) => s.Split (' ')).ToArray ();
			var wordToIntMap = new Dictionary<string, int> ();
			int wordId = 0;
			foreach (string[] doc in words)
			{
				foreach (string word in doc) 
				{
					if (!wordToIntMap.ContainsKey(word))
					{
						wordToIntMap [word] = wordId++;
					}
				}
			}
			termCount = wordToIntMap.Count;
			intToWordMap = new string[termCount];
			foreach (var pair in wordToIntMap)
			{
				intToWordMap [pair.Value] = pair.Key;
			}
		}

		public void Inference(int iterationCount)
		{
			if (inferenced) {
				return;
			}


			int[,] documentTopicCount = new int[documentCount, topicCount];
			int[,] topicTermCount = new int[topicCount, termCount];
			int[] termTopicAssignment = new int[termCount];
			int[] topicTermSum = new int[topicCount];

			// Initialization
			Random r = new Random ();
			for (int document=0; document<documentCount; document++)
			{
				for (int term=0; term<documents[document].Length; term++)
				{
					// Sample multinomial 1/K
					int topic = r.Next (topicCount);

					termTopicAssignment [documents [document] [term]] = topic;
					documentTopicCount [document, topic]++;
					topicTermCount [topic, documents[document][term]]++;
					topicTermSum [topic]++;
				}
			}

			theta = new double[documentCount, topicCount];
			phi = new double[topicCount, termCount];
			for (int iteration=1; iteration<=iterationCount; iteration++) 
			{
				for (int document=0; document<documentCount; document++) 
				{
					for (int term=0; term<documents[document].Length; term++) 
					{
						int topic = termTopicAssignment [documents [document] [term]];
						// Decrement.
						termTopicAssignment [documents [document] [term]] = topic;
						documentTopicCount [document, topic]--;
						topicTermCount [topic, documents[document][term]]--;
						topicTermSum [topic]--;

						// Sample topic.
						int newTopic = sampleTopic (r, documentTopicCount, topicTermCount, topicTermSum);
						// Increment.
						termTopicAssignment [documents [document] [term]] = newTopic;
						documentTopicCount [document, newTopic]++;
						topicTermCount [newTopic, documents[document][term]]++;
						topicTermSum [newTopic]++;
					}
				}
			}
		}
		
		private int sampleTopic (Random r, int[,] documentTopicCount, int[,] topicTermCount, int[] topicTermSum)
		{
			double[] prob = new double[topicCount];
			for (int topic=0; topic<topicCount; topic++)
			{

			}

			throw new NotImplementedException ();
		}
	}
}

using System.Collections.Generic;
using System.Linq;
using System;

namespace Dip
{
	public class LDA
	{
		// Number of words.
		private readonly int termCount;

		// Number of documents.
		private readonly int documentCount;

		// Number of topics.
		private readonly int topicCount;

		// Dirichlet hyperparameters.
		private readonly double alpha, beta;

		// Documents itself.
		private readonly int[][] documents;

		// Mapping from int to words.
		private readonly string[] intToWordMap;

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
			documents = new int[documentCount][];
			for (int document=0; document<words.Length; document++)
			{
				documents[document] = new int[words[document].Length];
				for (int term=0; term<words[document].Length; term++) 
				{
					if (!wordToIntMap.ContainsKey(words[document][term]))
					{
						wordToIntMap [words[document][term]] = wordId;
						documents [document] [term] = wordId;
						wordId++;
					}
					else
					{
						documents [document] [term] = wordToIntMap[words[document][term]];
					}
				}
			}
			termCount = wordToIntMap.Count;
			intToWordMap = new string[termCount];
			foreach (var pair in wordToIntMap)
			{
				intToWordMap [pair.Value] = pair.Key;
			}



			alpha = 50.0 / topicCount;
			beta = 1e-2;
		}

		public void Inference(int iterationCount)
		{
			if (inferenced) 
			{
				return;
			}


			int[,] documentTopicCount = new int[documentCount, topicCount];
			int[,] topicTermCount = new int[topicCount, termCount];
			int[] termTopicAssignment = new int[termCount];
			int[] topicTermSum = new int[topicCount];

			// Initialization
			Random r = new Random (1488);
			for (int document=0; document<documentCount; document++)
			{
				for (int term=0; term<documents[document].Length; term++)
				{
					// Sample multinomial 1/K
					int topic = r.Next (topicCount);
					Console.WriteLine ("Initially sampled topic {0} for word {1}", topic, documents[document][term]);

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
						documentTopicCount [document, topic]--;
						topicTermCount [topic, documents[document][term]]--;
						topicTermSum [topic]--;

						// Sample topic.
						int newTopic = sampleTopic (r, documentTopicCount, topicTermCount, topicTermSum, document, term);
						// Increment.
						termTopicAssignment [documents [document] [term]] = newTopic;
						documentTopicCount [document, newTopic]++;
						topicTermCount [newTopic, documents[document][term]]++;
						topicTermSum [newTopic]++;
					}
				}
			}
		}
		
		private int sampleTopic (Random r, int[,] documentTopicCount, int[,] topicTermCount, int[] topicTermSum, int document, int term)
		{
			double[] prob = new double[topicCount];
			double sum = 0;
			for (int topic=0; topic<topicCount; topic++)
			{
				prob [topic] = (topicTermCount [topic, documents [document][term]] + beta)
						/ (topicTermSum[topic] + beta)
						* (documentTopicCount[document, topic] + alpha);

				sum += prob [topic];
			}

			// Sample topic according to distribution.
			double d = r.NextDouble () * sum;
			double accumulator = 0;
			for (int topic=0; topic<topicCount; topic++)
			{
				if (d >= accumulator && d < accumulator + prob[topic])
				{
					Console.WriteLine ("Sampled topic {0} for word {1}", topic, intToWordMap[documents[document][term]]);
					return topic;
				}
				accumulator += prob [topic];
			}

			Console.WriteLine ("Total: {0}", accumulator);
			throw new ApplicationException("Wrong topic sampling in LDA.sampleTopic");
		}
	}
}

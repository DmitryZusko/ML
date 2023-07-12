using Microsoft.VisualStudio.TestTools.UnitTesting;
using NeuralNetwork;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace NeuralNetwork.Tests
{
	[TestClass()]
	public class NeuralNetworkTests
	{
		[TestMethod()]
		public void FeedForwardTest()
		{
			var dataset = new List<Tuple<double, double[]>>
			{
				//new Tuple<double, double[]> (1, new double[] {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,1}),
				//new Tuple<double, double[]> (1, new double[] {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,1,1}),
				//new Tuple<double, double[]> (1, new double[] {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1})
			};
			string datasetStr = "";

			//datasetStr = JsonConvert.SerializeObject(dataset);

			using (FileStream fsr = new FileStream("LearningDataset_improoved.json", FileMode.Open))
            {
                using(StreamReader sr = new StreamReader(fsr))
                {
					datasetStr = sr.ReadToEnd();
				}
            }
			dataset = JsonConvert.DeserializeObject<List<Tuple<double, double[]>>>(datasetStr);

			var topology = new Topology(19, 1, 0.00005, 17, 15, 9); // 14 - hidden layer з 14ма нейронами 
			var neuralNetwork = new NeuralNetwork(topology);
			Stopwatch _stopwatch = new Stopwatch();
			_stopwatch.Start();

			var difference = neuralNetwork.Learn(dataset, 1000, 5);
			_stopwatch.Stop();

			using (FileStream fs = new FileStream("weights.json", FileMode.Create))
			{
				List<List<List<double>>> weights = new List<List<List<double>>>();
    
				foreach (var l in neuralNetwork.Layers)
				{
					List<List<double>> layerWeight = new List<List<double>>();
					foreach (var neuro in l.Neurons)
					{
						layerWeight.Add(neuro.Weights);
					}
					weights.Add(layerWeight);
				}
    
				var weightsJson = JsonConvert.SerializeObject(weights);
    
				using (StreamWriter writer = new StreamWriter(fs))
				{
					writer.Write(weightsJson);
				}
			}

			var ts = _stopwatch.Elapsed;
			var elapsedTime = String.Format("{0:00}:{1:00}.{2:00}",
				ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
			Console.WriteLine(elapsedTime.ToString());

			var results = new List<double>();
			foreach (var data in dataset)
			{
				var res = neuralNetwork.FeedForward(data.Item2).Output;
				results.Add(res);
			}

			for (int i = 0; i < results.Count; i++)
			{
				var expected = Math.Round(dataset[i].Item1, 0);
				var actual = Math.Round(results[i], 0);
				Assert.AreEqual(expected, actual);
			}
		}
	}
}
     
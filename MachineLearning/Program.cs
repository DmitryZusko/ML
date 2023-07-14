using MachineLearning;
using Newtonsoft.Json;

#region simple test data
//var dataset = new List<Tuple<double[], double>>
//{
//    new Tuple<double[], double> (new double[] {1, 0}, 1),
//    new Tuple<double[], double> (new double[] {0, 1}, 1),
//    new Tuple<double[], double> (new double[] {1, 1}, 1),
//    new Tuple<double[], double> (new double[] {0, 0}, 0),
//};

//var topology = new NeuralNetworkTopology { InputNeuronsCount = 2, HiddenLayersNeuronsCount = new List<int> { 2 }, OutputNeuronsCount = 1, LearningRate = 0.8 };

//var dataset = new List<Tuple<double[], double>>
//{
//    new Tuple<double[], double> (new double[] {1, 1, 0}, 1),
//    new Tuple<double[], double> (new double[] {0, 1, 1}, 1),
//    new Tuple<double[], double> (new double[] {1, 1, 1 }, 1),
//    new Tuple<double[], double> (new double[] {0, 0, 1}, 0),
//    new Tuple<double[], double> (new double[] {1, 0, 1}, 0),
//    new Tuple<double[], double> (new double[] {0, 1, 0}, 0),
//    new Tuple<double[], double> (new double[] {1, 0, 0}, 0),
//    new Tuple<double[], double> (new double[] {0, 0, 0}, 0),
//};

//var topology = new NeuralNetworkTopology { InputNeuronsCount = 3, HiddenLayersNeuronsCount = new List<int> { 3, 2 }, OutputNeuronsCount = 1, LearningRate = 0.25 };

//var dataset = new List<Tuple<double[], double>>
//{
//    new Tuple<double[], double> (new double[] {1, 1, 0}, 1),
//    new Tuple<double[], double> (new double[] {0, 1, 1}, 1),
//    new Tuple<double[], double> (new double[] {1, 1, 1 }, 0),
//    new Tuple<double[], double> (new double[] {0, 0, 1}, 0),
//    new Tuple<double[], double> (new double[] {1, 0, 1}, 1),
//    new Tuple<double[], double> (new double[] {0, 1, 0}, 0),
//    new Tuple<double[], double> (new double[] {1, 0, 0}, 0),
//    new Tuple<double[], double> (new double[] {0, 0, 0}, 0),
//};

//var topology = new NeuralNetworkTopology { InputNeuronsCount = 3, HiddenLayersNeuronsCount = new List<int> { 3 }, OutputNeuronsCount = 1, LearningRate = .5 };

//var dataset = new List<Tuple<double[], double>>
//{
//    new Tuple<double[], double> (new double[] {1, 0, 0, 0, 0}, .2),
//    new Tuple<double[], double> (new double[] {0, 1, 0, 0, 0}, .2),
//    new Tuple<double[], double> (new double[] {0, 0, 1, 0, 0}, .2),
//    new Tuple<double[], double> (new double[] {0, 0, 0, 0, 1}, .2),
//    new Tuple<double[], double> (new double[] {0, 1, 0, 0, 1}, .4),
//    new Tuple<double[], double> (new double[] {0, 1, 0, 1, 0}, .4),
//    new Tuple<double[], double> (new double[] {1, 0, 1, 0, 0}, .4),
//    new Tuple<double[], double> (new double[] {1, 0, 0, 0, 1}, .4),
//    new Tuple<double[], double> (new double[] {0, 1, 0, 1, 0}, .4),
//    //new Tuple<double[], double> (new double[] {0, 1, 0, 1, 0}, .4),
//    new Tuple<double[], double> (new double[] {1, 1, 0, 1, 0}, .6),
//    new Tuple<double[], double> (new double[] {1, 1, 0, 1, 0}, .6),
//    new Tuple<double[], double> (new double[] {1, 0, 0, 1, 1}, .6),
//    new Tuple<double[], double> (new double[] {0, 1, 1, 1, 0}, .6),
//    new Tuple<double[], double> (new double[] {1, 1, 1, 1, 0}, .6),
//    new Tuple<double[], double> (new double[] {0, 1, 1, 1, 1}, .8),
//    new Tuple<double[], double> (new double[] {1, 1, 1, 1, 1}, 1.0),
//};

//var topology = new NeuralNetworkTopology { InputNeuronsCount = 5, HiddenLayersNeuronsCount = new List<int> { 3 }, OutputNeuronsCount = 1, LearningRate = .9 };

//var dataset = new List<Tuple<double[], double>>
//{
//    new Tuple<double[], double> (new double[] {1, 0, 0, 0}, .5),
//    new Tuple<double[], double> (new double[] {0, 1, 0, 0}, .2),
//    new Tuple<double[], double> (new double[] {0, 0, 1, 0}, .2),
//    new Tuple<double[], double> (new double[] {0, 0, 0, 1}, .1),
//    new Tuple<double[], double> (new double[] {1, 0, 0, 1}, .6),
//    new Tuple<double[], double> (new double[] {0, 1, 0, 1}, .3),
//    new Tuple<double[], double> (new double[] {0, 0, 1, 1}, .3),
//    new Tuple<double[], double> (new double[] {1, 1, 0, 0}, .7),
//    new Tuple<double[], double> (new double[] {1, 1, 0, 1}, .8),
//    new Tuple<double[], double> (new double[] {1, 0, 1, 1}, .8),
//    new Tuple<double[], double> (new double[] {1, 1, 1, 1}, 1.0),
//};

//var topology = new NeuralNetworkTopology { InputNeuronsCount = 4, HiddenLayersNeuronsCount = new List<int> { 4 }, OutputNeuronsCount = 1, LearningRate = .9 };

#endregion

string datasetStr = "";

using (FileStream fsr = new FileStream("LearningDataset_improoved.json", FileMode.Open))
{
    using (StreamReader sr = new StreamReader(fsr))
    {
        datasetStr = sr.ReadToEnd();
    }
}

var dataset = JsonConvert.DeserializeObject<List<Tuple<double, double[]>>>(datasetStr);

var topology = new NeuralNetworkTopology { InputNeuronsCount = 19, HiddenLayersNeuronsCount = new List<int> { 14 }, OutputNeuronsCount = 1, LearningRate = .15 };

var network = new NeuralNetwork(topology);

network.StartLearning(dataset, 100, network.Topology.LearningRate, true);

//foreach (var data in dataset)
//{
//    result = network.FeedForward(data.Item2.ToList());
//    Console.WriteLine($"expected: {data.Item1} --- result: {result.Output}");
//}

//foreach (var data in dataset)
//{
//    result = network.FeedForward(data.Item1.ToList());
//    Console.WriteLine($"expected: {data.Item2} --- result: {Math.Round(result.Output, 2)}");
//}

//result = network.FeedForward(new List<double> { 0, 0, 0, 1, 0 });
//Console.WriteLine($"expected: {.2} --- result: {result.Output}");
//result = network.FeedForward(new List<double> { 0, 1, 0, 1, 0 });
//Console.WriteLine($"expected: {.4} --- result: {result.Output}");

Console.WriteLine();
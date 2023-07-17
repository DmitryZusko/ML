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


var dataset = LoadData("LearningDataset_improoved.json");

//var weights = LoadWeights("weights.json");

var topology = new NeuralNetworkTopology { InputNeuronsCount = 19, HiddenLayersNeuronsCount = new List<int> { 64 }, OutputNeuronsCount = 1, LearningRate = .025 };

var network = new NeuralNetwork(topology);

//var network = new NeuralNetwork(topology, weights);

network.StartLearning(dataset, 1000, network.Topology.LearningRate, true);

#region more tests
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
#endregion

Console.WriteLine();


List<List<List<double>>> LoadWeights(string filePath)
{
    string weightsStr = "";

    using (FileStream fsr = new FileStream(filePath, FileMode.Open))
    {
        using (StreamReader sr = new StreamReader(fsr))
        {
            weightsStr = sr.ReadToEnd();
        }
    }

    return JsonConvert.DeserializeObject<List<List<List<double>>>>(weightsStr);

}

List<Tuple<double, double[]>> LoadData(string filePath)
{
    string datasetStr = "";

    using (FileStream fsr = new FileStream(filePath, FileMode.Open))
    {
        using (StreamReader sr = new StreamReader(fsr))
        {
            datasetStr = sr.ReadToEnd();
        }
    }

    return JsonConvert.DeserializeObject<List<Tuple<double, double[]>>>(datasetStr);

}
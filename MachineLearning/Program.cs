using MachineLearning;

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

var dataset = new List<Tuple<double[], double>>
{
    new Tuple<double[], double> (new double[] {1, 0, 0, 0, 0}, .2),
    new Tuple<double[], double> (new double[] {0, 1, 0, 0, 0}, .2),
    new Tuple<double[], double> (new double[] {0, 0, 1, 0, 0}, .2),
    new Tuple<double[], double> (new double[] {0, 0, 0, 0, 1}, .2),
    new Tuple<double[], double> (new double[] {0, 1, 0, 0, 1}, .4),
    new Tuple<double[], double> (new double[] {0, 1, 0, 1, 0}, .4),
    new Tuple<double[], double> (new double[] {1, 0, 1, 0, 0}, .4),
    new Tuple<double[], double> (new double[] {1, 0, 0, 0, 1}, .4),
    new Tuple<double[], double> (new double[] {0, 1, 0, 1, 0}, .4),
    //new Tuple<double[], double> (new double[] {0, 1, 0, 1, 0}, .4),
    new Tuple<double[], double> (new double[] {1, 1, 0, 1, 0}, .6),
    new Tuple<double[], double> (new double[] {1, 1, 0, 1, 0}, .6),
    new Tuple<double[], double> (new double[] {1, 0, 0, 1, 1}, .6),
    new Tuple<double[], double> (new double[] {0, 1, 1, 1, 0}, .6),
    new Tuple<double[], double> (new double[] {1, 1, 1, 1, 0}, .6),
    new Tuple<double[], double> (new double[] {0, 1, 1, 1, 1}, .8),
    new Tuple<double[], double> (new double[] {1, 1, 1, 1, 1}, 1.0),
};

var topology = new NeuralNetworkTopology { InputNeuronsCount = 5, HiddenLayersNeuronsCount = new List<int> { 3 }, OutputNeuronsCount = 1, LearningRate = .9 };

var network = new NeuralNetwork(topology);

network.StartLearning(dataset, 10_000, network.Topology.LearningRate);

var result = new Neuron();

foreach (var data in dataset)
{
    result = network.FeedForward(data.Item1.ToList());
    Console.WriteLine($"expected: {data.Item2} --- result: {result.Output,2}");
}

//foreach (var data in dataset)
//{
//    result = network.FeedForward(data.Item1.ToList());
//    Console.WriteLine($"expected: {data.Item2} --- result: {Math.Round(result.Output, 2)}");
//}

result = network.FeedForward(new List<double> { 0, 0, 0, 1, 0 });
Console.WriteLine($"expected: {.2} --- result: {result.Output,1}");
result = network.FeedForward(new List<double> { 0, 1, 0, 1, 0 });
Console.WriteLine($"expected: {.4} --- result: {result.Output,1}");

Console.WriteLine();
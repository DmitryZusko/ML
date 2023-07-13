using MachineLearning;

var dataset = new List<Tuple<double[], double>>
{
    new Tuple<double[], double> (new double[] {1, 0}, 1),
    new Tuple<double[], double> (new double[] {0, 1}, 1),
    new Tuple<double[], double> (new double[] {1, 1}, 1),
    new Tuple<double[], double> (new double[] {0, 0}, 0),
};

var topology = new NeuralNetworkTopology { InputNeuronsCount = 2, HiddenLayersNeuronsCount = new List<int> { 2 }, OutputNeuronsCount = 1, LearningRate = 0.15 };

var network = new NeuralNetwork(topology);

for (int i = 0; i < 10000; i++)
{
    foreach (var data in dataset)
    {
        var result = network.FeedForward(data.Item1.ToList());

        var error = data.Item2 - result.Output;

    }
}

var end = network.FeedForward(new List<double> { 1, 1 });

Console.WriteLine();
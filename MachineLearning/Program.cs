﻿using MachineLearning;
using Newtonsoft.Json;


var dataset = LoadData("LearningDataset_improoved.json");

var topology = new NeuralNetworkTopology { InputNeuronsCount = 19, HiddenLayersNeuronsCount = new List<int> { 64 }, OutputNeuronsCount = 1, LearningRate = .15 };

var network = new NeuralNetwork(topology, false);

network.LoadWeightsFromJsonFile("weights.json");

var dataForCalculation = new List<List<double>>();
foreach (var data in dataset)
{
    dataForCalculation.Add(data.Item2.ToList());
}

var results = network.CalculateResults(dataForCalculation);

Console.WriteLine($"Min: {results.Min()} | Max: {results.Max()}");

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
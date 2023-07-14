using Newtonsoft.Json;
using System.Diagnostics;

namespace MachineLearning
{
    public class NeuralNetwork
    {
        public List<Layer> Layers { get; set; } = new List<Layer>();
        public NeuralNetworkTopology Topology { get; set; } = null!;

        public NeuralNetwork(NeuralNetworkTopology topology)
        {
            Topology = topology;

            CreateInputLayer();
            CreateHiddenLayers();
            CreateOutputLayer();
        }

        public void StartLearning(List<Tuple<double, double[]>> dataset, int epochCount, double learningRate, bool collectData)
        {
            var stopwatch = Stopwatch.StartNew();
            for (var i = 1; i < epochCount; i++)
            {
                //var dynamicLearningRate = 1 * learningRate / (Math.Log10(i) + 1);
                foreach (var data in dataset)
                {
                    var result = Learn(data.Item2, data.Item1, learningRate);

                    Console.WriteLine(i);
                    Console.WriteLine($"expected: {data.Item1} --- result: {result.Output}");
                }
            }

            stopwatch.Stop();
            if (collectData) RunAndSnapshot(dataset, epochCount, learningRate, stopwatch.ElapsedMilliseconds);
        }

        private Neuron Learn(double[] data, double expected, double learningRate)
        {
            var output = FeedForward(data.ToList());

            var error = expected - output.Output;

            SetErrors(error);

            for (var layerIndex = Layers.Count - 1; layerIndex > 0; layerIndex--)
            {
                var boundedResults = Layers[layerIndex - 1].GetResults();

                foreach (var neuron in Layers[layerIndex].Neurons)
                {
                    neuron.Learn(boundedResults, learningRate);
                }
            }

            return output;
        }

        private void SetErrors(double error)
        {
            foreach (var neuron in Layers.Last().Neurons)
            {
                neuron.Error = error;
            }

            for (var layerIndex = Layers.Count - 2; layerIndex > 0; layerIndex--)
            {
                var curLayer = Layers[layerIndex];
                var prevLayer = Layers[layerIndex + 1];

                for (var curNeuronIndex = 0; curNeuronIndex < curLayer.Neurons.Count; curNeuronIndex++)
                {
                    var currentNeuron = curLayer.Neurons[curNeuronIndex];
                    currentNeuron.Error = 0;

                    for (var prevNeuronIndex = 0; prevNeuronIndex < prevLayer.Neurons.Count; prevNeuronIndex++)
                    {
                        var prevNeuron = prevLayer.Neurons[prevNeuronIndex];
                        currentNeuron.Error += prevNeuron.Error * prevNeuron.Weights[curNeuronIndex];
                    }
                }
            }
        }

        public Neuron FeedForward(List<double> inputSignals)
        {
            FeedInputLayer(inputSignals);

            FeedLayersAfterInput();

            return Layers
                        .Last()
                        .Neurons
                        .First();
        }

        private void FeedInputLayer(List<double> inputSignals)
        {
            for (var i = 0; i < inputSignals.Count(); i++)
            {
                var neuron = Layers[0].Neurons[i];
                neuron.FeedForward(new List<double> { inputSignals[i] });
            }
        }

        private void FeedLayersAfterInput()
        {
            for (var layer = 1; layer < Layers.Count; layer++)
            {
                var prevLayerResults = Layers[layer - 1].GetResults();
                foreach (var neuron in Layers[layer].Neurons)
                {
                    neuron.FeedForward(prevLayerResults);
                }
            }
        }

        private void CreateInputLayer()
        {
            var layer = new Layer();

            for (int i = 0; i < Topology.InputNeuronsCount; i++)
            {
                layer.Neurons.Add(new Neuron(1, NeuronTypes.Input));
            }

            Layers.Add(layer);
        }

        private void CreateHiddenLayers()
        {
            foreach (var layerSize in Topology.HiddenLayersNeuronsCount)
            {
                var layer = new Layer();
                var lastLayerNeuronsCount = Layers.Last().Neurons.Count;

                for (var i = 0; i < layerSize; i++)
                {
                    layer.Neurons.Add(new Neuron(lastLayerNeuronsCount, NeuronTypes.Hidden));
                }

                Layers.Add(layer);
            }
        }

        private void CreateOutputLayer()
        {
            var layer = new Layer();
            var lastLayerNeuronsCount = Layers.Last().Neurons.Count;

            for (int i = 0; i < Topology.OutputNeuronsCount; i++)
            {
                layer.Neurons.Add(new Neuron(lastLayerNeuronsCount, NeuronTypes.Output));
            }

            Layers.Add(layer);
        }

        private void RunAndSnapshot(List<Tuple<double, double[]>> dataset, int epochCount, double learningRate, long learTime)
        {
            Console.WriteLine();
            Console.WriteLine("============== FINAL ROUND ==============");
            Console.WriteLine();


            //create data folder
            var folderPath = $"RESULTS\\{DateTime.Now.ToString("MM/dd/yy")}_{epochCount}_{learningRate}_{learTime}";

            foreach (var layer in Topology.HiddenLayersNeuronsCount)
            {
                folderPath += $"_{layer}";
            }

            Directory.CreateDirectory(folderPath);

            //save current weights
            List<List<List<double>>> weights = new List<List<List<double>>>();

            foreach (var layer in Layers)
            {
                List<List<double>> layerWeight = new List<List<double>>();
                foreach (var neuro in layer.Neurons)
                {
                    layerWeight.Add(neuro.Weights);
                }
                weights.Add(layerWeight);
            }

            var weightsJson = JsonConvert.SerializeObject(weights);

            using (var file = new FileStream(Path.Combine(folderPath, "weights.json"), FileMode.Create))
            {
                using (var writer = new StreamWriter(file))
                {
                    writer.WriteLine(weightsJson);
                }
            }

            //run
            var errors = new List<double>();

            foreach (var data in dataset)
            {
                var result = FeedForward(data.Item2.ToList());
                Console.WriteLine($"expected: {data.Item1} --- result: {result.Output}");
                errors.Add(data.Item1 - result.Output);
            }

            //save errors
            using (var file = new FileStream(Path.Combine(folderPath, "errors.txt"), FileMode.Create))
            {
                using (var writer = new StreamWriter(file))
                {
                    foreach (var e in errors)
                    {
                        writer.WriteLine(e);
                    }
                }
            }


        }
    }
}

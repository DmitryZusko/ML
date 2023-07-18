using Newtonsoft.Json;
using System.Diagnostics;

namespace MachineLearning
{
    public class NeuralNetwork
    {
        public List<Layer> Layers { get; set; } = new List<Layer>();
        public NeuralNetworkTopology Topology { get; set; } = null!;
        //private UserConnection _userConnection;
        //public UserConnection UserConnection
        //{
        //    get
        //    {
        //        return _userConnection ??
        //            (_userConnection = HttpContext.Current.Session["UserConnection"] as UserConnection);
        //    }
        //    set
        //    {
        //        _userConnection = value;
        //    }
        //}

        //якщо створюється мережа з нуля, generateWeights = true генерує рандомні значення вагів
        public NeuralNetwork(NeuralNetworkTopology topology, bool generateWeights = false)
        {
            Topology = topology;

            CreateInputLayer(generateWeights);
            CreateHiddenLayers(generateWeights);
            CreateOutputLayer(generateWeights);
        }

        public void LoadWeightsFromJsonFile(string filePath)
        {
            string weightsStr = "";

            using (FileStream fsr = new FileStream(filePath, FileMode.Open))
            {
                using (StreamReader sr = new StreamReader(fsr))
                {
                    weightsStr = sr.ReadToEnd();
                }
            }

            var weights = JsonConvert.DeserializeObject<List<List<List<double>>>>(weightsStr);

            SetWeights(weights);
        }

        //public void LoadWeightsFromLookup(string lookupName)
        //{
        //    List<List<List<double>>> weights = new();

        //    Select selectWeights = new Select(UserConnection)
        //    .Column("GenWeightsJson").Top(1)
        //    .From(lookupName)
        //    .OrderByDesc("ModifiedOn")
        //    as Select;

        //    using (var dbExecutor = UserConnection.EnsureDBConnection())
        //    {
        //        using (var reader = selectWeights.ExecuteReader(dbExecutor))
        //        {
        //            if (reader.Read())
        //            {
        //                string jWeights = (reader.GetValue(0) != System.DBNull.Value) ? (string)reader.GetValue(0) : "";
        //                if (!string.IsNullOrEmpty(jWeights))
        //                {
        //                    weights = JsonConvert.DeserializeObject<List<List<List<double>>>>(jWeights);
        //                }
        //            }
        //        }
        //    }

        //    SetWeights(weights);
        //}

        public List<double> CalculateResults(List<List<double>> dataset)
        {
            var results = new List<double>();

            foreach (var data in dataset)
            {
                results.Add(FeedForward(data).Output);
            }

            return results;
        }

        //collectData = true прожене датасет додатковий раз після навчання і збереже отримані помилки і ваги в окремі файли
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

        private void CreateInputLayer(bool generateWeights)
        {
            var layer = new Layer();

            for (int i = 0; i < Topology.InputNeuronsCount; i++)
            {
                layer.Neurons.Add(new Neuron(1, NeuronTypes.Input, generateWeights));
            }

            Layers.Add(layer);
        }

        private void CreateHiddenLayers(bool generateWeights)
        {
            foreach (var layerSize in Topology.HiddenLayersNeuronsCount)
            {
                var layer = new Layer();
                var lastLayerNeuronsCount = Layers.Last().Neurons.Count;

                for (var i = 0; i < layerSize; i++)
                {
                    layer.Neurons.Add(new Neuron(lastLayerNeuronsCount, NeuronTypes.Hidden, generateWeights));
                }

                Layers.Add(layer);
            }
        }

        private void CreateOutputLayer(bool generateWeights)
        {
            var layer = new Layer();
            var lastLayerNeuronsCount = Layers.Last().Neurons.Count;

            for (int i = 0; i < Topology.OutputNeuronsCount; i++)
            {
                layer.Neurons.Add(new Neuron(lastLayerNeuronsCount, NeuronTypes.Output, generateWeights));
            }

            Layers.Add(layer);
        }

        private void SetWeights(List<List<List<double>>>? weights)
        {
            if (weights?.Count != Layers.Count) throw new Exception("Bad topology!");

            for (var i = 0; i < weights.Count; i++)
            {
                Layers[i].SetWeights(weights[i]);
            }
        }

        private Neuron FeedForward(List<double> inputSignals)
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
            //записуємо помилки для Output Layer
            foreach (var neuron in Layers.Last().Neurons)
            {
                neuron.Error = error;
            }

            //записуємо помилки для Hidden Layers. Input Layer помилок не має
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

        //learnTime - час навчання в мілісекундах, [learnTime]/60_000 = [хвилини]
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

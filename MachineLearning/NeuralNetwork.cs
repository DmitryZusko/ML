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

        public Neuron FeedForward(List<double> inputSignals)
        {
            FeedInputLayer(inputSignals);

            FeedOtherLayers();

            return Layers
                        .Last()
                        .Neurons
                        .First();
        }

        private void FeedOtherLayers()
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
        public void Backpropagate(double error, double learningRate)
        {
            LearnOutputLayer(learningRate);

            LearnHiddenLayers(learningRate);
        }

        public void CalculateDelta(double error)
        {
            foreach (var neuron in Layers.Last().Neurons)
            {
                neuron.CalculateDelta(error);
            }

            for (var layerIndex = Layers.Count - 2; layerIndex > 0; layerIndex--)
            {
                var layer = Layers[layerIndex];
                var prevLayer = Layers[layerIndex + 1];

                for (var neuronIndex = 0; neuronIndex < layer.Neurons.Count; neuronIndex++)
                {
                    var previosDelta = 1.0;
                    foreach (var neuron in prevLayer.Neurons)
                    {
                        previosDelta *= neuron.Delta * neuron.Weights[neuronIndex];
                    }

                    layer.Neurons[neuronIndex].CalculateDelta(previosDelta);
                }
            }
        }

        private void LearnHiddenLayers(double learningRate)
        {
            for (var layerIndex = Layers.Count - 2; layerIndex > 0; layerIndex--)
            {
                var currentLayer = Layers[layerIndex];
                var prevLayer = Layers[layerIndex - 1];

                foreach (var neuron in currentLayer.Neurons)
                {
                    for (var weightIndex = 0; weightIndex < neuron.Weights.Count; weightIndex++)
                    {
                        neuron.Weights[weightIndex] += learningRate * neuron.Delta * prevLayer.Neurons[weightIndex].Output;
                    }
                }
            }
        }

        private void LearnOutputLayer(double learningRate)
        {
            var outputLayer = Layers.Last();
            var prevLayer = Layers[Layers.Count - 2];
            foreach (var neuron in outputLayer.Neurons)
            {
                for (var i = 0; i < neuron.Weights.Count; i++)
                {
                    neuron.Weights[i] += learningRate * neuron.Delta * prevLayer.Neurons[i].Output;
                }
            }
        }

        private void FeedInputLayer(List<double> inputSignals)
        {
            for (var i = 0; i < inputSignals.Count(); i++)
            {
                var neuron = Layers[0].Neurons[i];
                neuron.FeedForward(new List<double> { inputSignals[i] });
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
                var lastLayerNeuronsCount = Layers.Last().AmountOfNeurons;

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
            var lastLayerNeuronsCount = Layers.Last().AmountOfNeurons;

            for (int i = 0; i < Topology.OutputNeuronsCount; i++)
            {
                layer.Neurons.Add(new Neuron(lastLayerNeuronsCount, NeuronTypes.Output));
            }

            Layers.Add(layer);
        }

        public void SetErrors(double diff)
        {
            foreach (var outputNeuron in Layers.Last().Neurons)
            {
                outputNeuron.Delta = diff;
            }

            for (var layer = Layers.Count - 2; layer > 0; layer--)
            {
                for (var neuronIndex = 0; neuronIndex < Layers[layer].Neurons.Count; neuronIndex++)
                {
                    var neuron = Layers[layer].Neurons[neuronIndex];
                    neuron.Delta = 0;

                    foreach (var nextNeuron in Layers[layer + 1].Neurons)
                    {
                        neuron.Delta += nextNeuron.Delta * nextNeuron.Weights[neuronIndex] / nextNeuron.Weights.Sum();
                    }
                }
            }
        }
    }
}

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
    }
}

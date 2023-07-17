namespace MachineLearning
{
    public class Layer
    {
        public List<Neuron> Neurons { get; set; } = new List<Neuron>();

        public Layer() { }
        public Layer(List<Neuron> neurons) => Neurons = neurons;

        public List<double> GetResults()
        {
            var results = new List<double>();
            foreach (var neuron in Neurons)
            {
                results.Add(neuron.Output);
            }

            return results;
        }

        public void SetWeights(List<List<double>> weights)
        {
            if (weights.Count != Neurons.Count) throw new Exception("Bad topology!");

            for (var i = 0; i < weights.Count; i++)
            {
                Neurons[i].Weights = weights[i];
            }
        }
    }
}

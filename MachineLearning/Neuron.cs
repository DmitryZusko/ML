namespace MachineLearning
{
    public class Neuron
    {
        public List<double> Weights { get; set; } = new List<double>();
        public NeuronTypes NeuronType { get; set; }
        public double Output { get; set; }
        public double Error { get; set; }

        public Neuron() { }

        public Neuron(int inputCount, NeuronTypes type, bool generateWeights)
        {
            NeuronType = type;

            if (generateWeights) InitWeights(inputCount);
        }

        public double FeedForward(List<double> inputs)
        {
            var sum = 0.0;

            for (var i = 0; i < inputs.Count; i++)
            {
                sum += inputs[i] * Weights[i];
            }


            Output = NeuronType == NeuronTypes.Input ? sum : Utils.Sigmoid(sum);
            return Output;
        }

        public void Learn(List<double> boundedResults, double learningRate)
        {
            for (int i = 0; i < Weights.Count; i++)
            {
                Weights[i] += learningRate * Error * Output * (1 - Output) * boundedResults[i];
            }
        }

        private void InitWeights(int inputCount)
        {
            if (NeuronType == NeuronTypes.Input)
            {
                Weights = Enumerable.Repeat(1.0, inputCount).ToList();
                return;
            }

            for (var i = 0; i < inputCount; i++)
            {
                Weights.Add(Utils.GenerateRandomValue());
            }
        }

    }
}

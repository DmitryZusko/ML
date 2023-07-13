namespace MachineLearning
{
    public class NeuralNetworkTopology
    {
        public int InputNeuronsCount { get; set; }
        public int OutputNeuronsCount { get; set; }
        public List<int> HiddenLayersNeuronsCount { get; set; } = new List<int>();
        public double LearningRate { get; set; }
    }
}

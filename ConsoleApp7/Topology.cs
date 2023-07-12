namespace NeuralNetwork
{
    // Опис нейронної мережі
    public class Topology
    { 
        public int InputCount { get;} //вхідні дані
        
        public int OutputCount { get; } //вихідні дані

        public double LearningRate { get; }
        
        public List<int> HiddenLayers { get; } // зберігає нейронни в Hidden Layer
        
        public Topology(int inputCount, int outputCount, double learningRate, params int[] layers)
        {
            LearningRate = learningRate;
            InputCount = inputCount;
            OutputCount = outputCount;
            HiddenLayers = new List<int>();
            HiddenLayers.AddRange(layers);
        }
    }
}
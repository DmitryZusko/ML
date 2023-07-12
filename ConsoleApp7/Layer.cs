namespace NeuralNetwork
{
    public class Layer
    {
        public List<Neuron> Neurons { get; }
      
        public int Count => Neurons?.Count ?? 0; //перевірка на null

        public NeuronType Type;

        public Layer(List<Neuron> neurons, NeuronType type = NeuronType.Normal)
        {
			//перевіряємо всі вхідні нейрони на відповідність типу
			Neurons = neurons;
            Type = type;
        }

        public List<double> GetSignals()
        {
            var result = new List<double>();
            
            foreach (var neuron in Neurons)
            {
                result.Add(neuron.Output);
            }
            return result;
        }

        public override string ToString()
        {
            return Type.ToString();
        }
    }
    
}
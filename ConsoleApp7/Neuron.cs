namespace NeuralNetwork
{
	public class Neuron
	{
		public List<double> Weights { get; set; }// сеарелізувати в JSON
		public List<double> Inputs { get; }
		public NeuronType NeuronType { get; }
		public double Output { get; private set; }
		public double Delta { get; private set; }
		// для нейрону потрібно розуміти, яка кількість зв'язків з ваговими коефіцієнтами до нього надходить
		public Neuron(int inputCount, NeuronType type = NeuronType.Normal)
		{
			NeuronType = type;
			Weights = new List<double>();
			Inputs = new List<double>();
			
			InitWeightsRandomValue(inputCount);
		}

		private void InitWeightsRandomValue(int inputCount)
		{
			var rnd = new Random();

			for (int i = 0; i < inputCount; i++)
			{
				if (NeuronType == NeuronType.Input)
				{
					Weights.Add(1);
				}
				else
				{
					Weights.Add(rnd.NextDouble());
				}
				Inputs.Add(0);
			}
		}

		public double FeedForward(List<double> inputs)// метод для обчислення параметрів отриманих на вході
		{
			for (int i = 0; i < inputs.Count; i++)
			{
				Inputs[i] = inputs[i];
			}

			var sum = 0.0;
			for (int i = 0; i < inputs.Count; i++)
			{
				sum += inputs[i] * Weights[i];
			}

			if (NeuronType != NeuronType.Input)
			//if (NeuronType == NeuronType.Output)
			{
				Output = Sigmoid(sum);// закидаємо в сігмоїду, для отримання коефу в діапазоні 0-1
			}
			else
			{
				Output = sum;
			}

			return Output;
		}

		private double Sigmoid(double x)//обраховуємо Sigmoid виходячи з її формули
		{
			var result = 1.0 / (1.0 + Math.Pow(Math.E, -x));
			return result;
		}

		private double SigmoidDx(double x)// обраховуємо sigm(x)dx
		{
			var sigmoid = Sigmoid(x);
			var result = sigmoid / (1 - sigmoid);// тобто sigm(x)dx = sigm / (1 - sigm)
			return result;
		}

		public void Learn(double error, double learningRate)// змінюємо наш нейрон, подаємо йому різницю на яку потрібно змінити наші коефіцієнти 
		{
			//learningRate - коефіцієнт, що впливає на швидкість навчання 
			
			if (NeuronType == NeuronType.Input)
			{
				return;
			}

			Delta = error * SigmoidDx(Output); // в якості х в SigmoidDx передаємо теперішнє значення х, тобто output нашого нейрону


			for (int i = 0; i < Weights.Count; i++)
			{
				var weight = Weights[i];
				var input = Inputs[i];

				var newWeigth = weight - input * Delta * learningRate;// новий коеф обраховується за значенням wi  = wi - oi * delta * learnRate
				Weights[i] = newWeigth;
				
				
			}
		}

		public override string ToString()
		{
			return Output.ToString();
		}
	}
}

{
  "Version": "8.0.9.1582",
  "UId": "d3947c7a-8bae-47a6-8b5f-5ffc804e9745",
  "ManagerName": "SourceCodeSchemaManager",
  "Name": "GenNeuralNetwork",
  "Caption": "NeuralNetwork",
  "ExtendParent": false,
  "DenyExtending": false,
  "Description": "",
  "SourceCode": "namespace Terrasoft.Configuration.NeuralNetwork
\n{
\n\tusing System;
\n\tusing System.Collections.Generic;
\n\tusing System.Linq;
\n\tusing Newtonsoft.Json.Linq;
\n\tusing Newtonsoft.Json;
\n\tusing Terrasoft.Core;
\n\tusing Terrasoft.Core.DB;
\n\tusing Terrasoft.Core.Entities;
\n\tusing Terrasoft.Core.Configuration;
\n\tusing Terrasoft.Common;
\n\tusing Terrasoft.Web.Common;
\n\tusing Terrasoft.Web.Http.Abstractions;
\n\t
\n\tpublic enum NeuronType // тип нейрону 
\n\t{
\n\t\tInput = 0,
\n\t\tNormal = 1,
\n\t\tOutput = 2
\n
\n\t\t}
\n\tpublic class Layer
\n\t{
\n\t\tpublic List<Neuron> Neurons { get; }
\n
\n\t\tpublic int Count => Neurons?.Count ?? 0; //перевірка на null
\n
\n\t\tpublic NeuronType Type;
\n
\n\t\tpublic Layer(List<Neuron> neurons, NeuronType type = NeuronType.Normal)
\n\t\t{
\n\t\t\t//перевіряємо всі вхідні нейрони на відповідність типу
\n\t\t\tNeurons = neurons;
\n\t\t\tType = type;
\n\t\t}
\n
\n\t\tpublic List<double> GetSignals()
\n\t\t{
\n\t\t\tvar result = new List<double>();
\n
\n\t\t\tforeach (var neuron in Neurons)
\n\t\t\t{
\n\t\t\t\tresult.Add(neuron.Output);
\n\t\t\t}
\n\t\t\treturn result;
\n\t\t}
\n
\n\t\tpublic override string ToString()
\n\t\t{
\n\t\t\treturn Type.ToString();
\n\t\t}
\n\t}
\n
\n\tpublic class Neuron
\n\t{
\n\t\tpublic List<double> Weights { get; set; }// сеарелізувати в JSON
\n\t\tpublic List<double> Inputs { get; }
\n\t\tpublic NeuronType NeuronType { get; }
\n\t\tpublic double Output { get; private set; } // готовий результата після обрахування значення нейрону після функції активації та обчислень значення нейрону
\n\t\tpublic double Delta { get; private set; }
\n\t\t// для нейрону потрібно розуміти, яка кількість зв'язків з ваговими коефіцієнтами до нього надходить
\n\t\tpublic Neuron(int inputCount, NeuronType type = NeuronType.Normal)
\n\t\t{
\n\t\t\tNeuronType = type;
\n\t\t\tWeights = new List<double>();
\n\t\t\tInputs = new List<double>();
\n
\n\t\t\tInitWeightsRandomValue(inputCount);
\n\t\t}
\n
\n\t\tprivate void InitWeightsRandomValue(int inputCount)
\n\t\t{
\n\t\t\tvar rnd = new Random();
\n
\n\t\t\tfor (int i = 0; i < inputCount; i++)
\n\t\t\t{
\n\t\t\t\tif (NeuronType == NeuronType.Input)
\n\t\t\t\t{
\n\t\t\t\t\tWeights.Add(1);
\n\t\t\t\t}
\n\t\t\t\telse
\n\t\t\t\t{
\n\t\t\t\t\tWeights.Add(rnd.NextDouble());
\n\t\t\t\t}
\n\t\t\t\tInputs.Add(0);
\n\t\t\t}
\n\t\t}
\n
\n\t\tpublic double FeedForward(List<double> inputs)// метод для обчислення параметрів отриманих на вході
\n\t\t{
\n\t\t\tfor (int i = 0; i < inputs.Count; i++)
\n\t\t\t{
\n\t\t\t\tInputs[i] = inputs[i];
\n\t\t\t}
\n
\n\t\t\tvar sum = 0.0;
\n\t\t\tfor (int i = 0; i < inputs.Count; i++)
\n\t\t\t{
\n\t\t\t\tsum += inputs[i] * Weights[i];
\n\t\t\t}
\n
\n\t\t\tif (NeuronType != NeuronType.Input)
\n\t\t\t{
\n\t\t\t\tOutput = Sigmoid(sum);// закидаємо в сігмоїду, для отримання коефу в діапазоні 0-1
\n\t\t\t}
\n\t\t\telse
\n\t\t\t{
\n\t\t\t\tOutput = sum;
\n\t\t\t}
\n
\n\t\t\treturn Output;
\n\t\t}
\n
\n\t\tprivate double Sigmoid(double x)//обраховуємо Sigmoid виходячи з її формули
\n\t\t{
\n\t\t\tvar result = 1.0 / (1.0 + Math.Pow(Math.E, -x));
\n\t\t\treturn result;
\n\t\t}
\n
\n\t\tprivate double SigmoidDx(double x)// обраховуємо sigm(x)dx
\n\t\t{
\n\t\t\tvar sigmoid = Sigmoid(x);
\n\t\t\tvar result = sigmoid / (1 - sigmoid);// тобто sigm(x)dx = sigm / (1 - sigm)
\n\t\t\treturn result;
\n\t\t}
\n
\n\t\tpublic void Learn(double error, double learningRate)// змінюємо наш нейрон, подаємо йому різницю на яку потрібно змінити наші коефіцієнти 
\n\t\t{
\n\t\t\t//learningRate - коефіцієнт, що впливає на швидкість навчання 
\n
\n\t\t\tif (NeuronType == NeuronType.Input)
\n\t\t\t{
\n\t\t\t\treturn;
\n\t\t\t}
\n
\n\t\t\tDelta = error * SigmoidDx(Output); // в якості х в SigmoidDx передаємо теперішнє значення х, тобто output нашого нейрону
\n
\n
\n\t\t\tfor (int i = 0; i < Weights.Count; i++)
\n\t\t\t{
\n\t\t\t\tvar weight = Weights[i];
\n\t\t\t\tvar input = Inputs[i];
\n
\n\t\t\t\tvar newWeigth = weight - input * Delta * learningRate;// новий коеф обраховується за значенням wi  = wi - oi * delta * learnRate
\n\t\t\t\tWeights[i] = newWeigth;
\n\t\t\t}
\n\t\t}
\n
\n\t\tpublic override string ToString()
\n\t\t{
\n\t\t\treturn Output.ToString();
\n\t\t}
\n\t}
\n
\n\t// Опис нейронної мережі
\n\tpublic class Topology
\n\t{ 
\n\t\tpublic int InputCount { get;} //вхідні дані
\n
\n\t\tpublic int OutputCount { get; } //вихідні дані
\n
\n\t\tpublic double LearningRate { get; }
\n
\n\t\tpublic List<int> HiddenLayers { get; } // зберігає нейронни в Hidden Layer
\n
\n\t\tpublic Topology(int inputCount, int outputCount, double learningRate, params int[] layers)
\n\t\t{
\n\t\t\tLearningRate = learningRate;
\n\t\t\tInputCount = inputCount;
\n\t\t\tOutputCount = outputCount;
\n\t\t\tHiddenLayers = new List<int>();
\n\t\t\tHiddenLayers.AddRange(layers);
\n\t\t}
\n\t}
\n\t//сама нейронна мережа 
\n\tpublic class NeuralNetwork
\n\t{
\n\t\tpublic Topology Topology { get; }
\n\t\tpublic List<Layer> Layers { get; }
\n\t\t
\n\t\tprivate UserConnection _userConnection;
\n\t\tpublic UserConnection UserConnection {
\n\t\t\tget {
\n\t\t\t\treturn _userConnection ??
\n\t\t\t\t\t(_userConnection = HttpContext.Current.Session[\"UserConnection\"] as UserConnection);
\n\t\t\t}
\n\t\t\tset {
\n\t\t\t\t_userConnection = value;
\n\t\t\t}
\n\t\t}
\n
\n\t\tpublic NeuralNetwork(Topology topology)
\n\t\t{
\n\t\t\tTopology = topology;
\n
\n\t\t\tLayers = new List<Layer>();
\n
\n\t\t\tCreateInputLayer();
\n\t\t\tCreateHiddenLayers();
\n\t\t\tCreateOutputLayer();
\n\t\t\tLoadFromDBWeights();
\n\t\t}
\n\t\t//переганяємо дані по нейронам
\n\t\tpublic Neuron FeedForward(params double[] inputSignals)
\n\t\t{
\n\t\t\tSendSignalsToInputNeurons(inputSignals);
\n\t\t\tFeedForwardAllLayersAfterInput();
\n
\n\t\t\tif (Topology.OutputCount == 1)
\n\t\t\t{
\n\t\t\t\treturn Layers.Last().Neurons[0];
\n\t\t\t}
\n\t\t\telse
\n\t\t\t{
\n\t\t\t\treturn Layers.Last().Neurons.OrderByDescending(n => n.Output).First();
\n\t\t\t}
\n\t\t}
\n
\n\t\tpublic double Learn(List<Tuple<double, double[]>> dataset, int epoch,int acc)
\n\t\t{
\n\t\t\tvar error = 0.0;
\n
\n\t\t\tfor (int i = 0; i < epoch; i++)
\n\t\t\t{
\n\t\t\t\tforeach (var data in dataset)
\n\t\t\t\t{
\n\t\t\t\t\terror += Backpropagation(acc,data.Item1, data.Item2);
\n\t\t\t\t}
\n\t\t\t}
\n
\n\t\t\tvar result = error / epoch;
\n\t\t\tCacheInDBWeights();
\n\t\t\treturn result;
\n\t\t}
\n\t\t
\n\t\tpublic double WorkHard(List<double> dataset, int epoch)
\n\t\t{
\n\t\t\tdouble result = 0d;
\n\t\t\t// отут магія
\n\t\t\tvar neuro = FeedForward(dataset.ToArray());
\n\t\t\tresult = neuro.Output;
\n\t\t\treturn result;
\n\t\t}
\n
\n
\n\t\t//метод обратного распространения ошибки
\n\t\tprivate double Backpropagation(int acc, double exprected, params double[] inputs)
\n\t\t{
\n\t\t\tvar actual = FeedForward(inputs).Output;
\n\t\t\t// для output layer помилка вираховується трішки по іншому(err = actual - expected)
\n\t\t\tvar difference = actual - exprected;
\n\t\t\t
\n\t\t\tif(Math.Round(difference,acc) == 0)
\n\t\t\t\treturn difference*difference;
\n\t\t\t\t
\n\t\t\tforeach (var neuron in Layers.Last().Neurons)
\n\t\t\t{
\n\t\t\t\tneuron.Learn(difference, Topology.LearningRate);
\n\t\t\t}
\n
\n\t\t\tfor (int j = Layers.Count - 2; j >= 0; j--)
\n\t\t\t{
\n\t\t\t\tvar layer = Layers[j];
\n\t\t\t\tvar previousLayer = Layers[j + 1];
\n
\n\t\t\t\tfor (int i = 0; i < layer.Count; i++)
\n\t\t\t\t{
\n\t\t\t\t\tvar neuron = layer.Neurons[i];
\n
\n\t\t\t\t\tfor (int k = 0; k < previousLayer.Count; k++)
\n\t\t\t\t\t{
\n\t\t\t\t\t\tvar previousNeuron = previousLayer.Neurons[k];
\n\t\t\t\t\t\tvar error = previousNeuron.Weights[i] * previousNeuron.Delta;
\n\t\t\t\t\t\tneuron.Learn(error, Topology.LearningRate);
\n\t\t\t\t\t}
\n\t\t\t\t}
\n\t\t\t}
\n
\n\t\t\tvar result = difference * difference;
\n\t\t\treturn result;
\n\t\t}
\n
\n\t\tprivate void FeedForwardAllLayersAfterInput()
\n\t\t{
\n\t\t\tfor (int i = 1; i < Layers.Count; i++)
\n\t\t\t{
\n\t\t\t\tvar layer = Layers[i];
\n\t\t\t\tvar previousLayerSingals = Layers[i - 1].GetSignals();
\n
\n\t\t\t\tforeach (var neuron in layer.Neurons)
\n\t\t\t\t{
\n\t\t\t\t\tneuron.FeedForward(previousLayerSingals);
\n\t\t\t\t}
\n\t\t\t}
\n\t\t}
\n
\n\t\tprivate void SendSignalsToInputNeurons(params double[] inputSignals)
\n\t\t{
\n\t\t\tfor (int i = 0; i < inputSignals.Length; i++)
\n\t\t\t{
\n\t\t\t\tvar signal = new List<double>() { inputSignals[i] };
\n\t\t\t\tvar neuron = Layers[0].Neurons[i];
\n
\n\t\t\t\tneuron.FeedForward(signal);//відправляємо дані на input, так як ми будемо по черзі проходити по кожному слою
\n\t\t\t}
\n\t\t}
\n
\n\t\tprivate void CreateOutputLayer()
\n\t\t{
\n\t\t\tvar outputNeurons = new List<Neuron>();
\n\t\t\tvar lastLayer = Layers.Last();
\n\t\t\tfor (int i = 0; i < Topology.OutputCount; i++)
\n\t\t\t{
\n\t\t\t\tvar neuron = new Neuron(lastLayer.Count, NeuronType.Output);
\n\t\t\t\toutputNeurons.Add(neuron);
\n\t\t\t}
\n\t\t\tvar outputLayer = new Layer(outputNeurons, NeuronType.Output);
\n\t\t\tLayers.Add(outputLayer);
\n\t\t}
\n
\n\t\tprivate void CreateHiddenLayers()
\n\t\t{
\n\t\t\tfor (int j = 0; j < Topology.HiddenLayers.Count; j++)
\n\t\t\t{
\n\t\t\t\tvar hiddenNeurons = new List<Neuron>();
\n\t\t\t\tvar lastLayer = Layers.Last();
\n\t\t\t\tfor (int i = 0; i < Topology.HiddenLayers[j]; i++)
\n\t\t\t\t{
\n\t\t\t\t\tvar neuron = new Neuron(lastLayer.Count);
\n\t\t\t\t\thiddenNeurons.Add(neuron);
\n\t\t\t\t}
\n\t\t\t\tvar hiddenLayer = new Layer(hiddenNeurons);
\n\t\t\t\tLayers.Add(hiddenLayer);
\n\t\t\t}
\n\t\t}
\n
\n\t\tprivate void CreateInputLayer()
\n\t\t{
\n\t\t\tvar inputNeurons = new List<Neuron>();
\n\t\t\tfor (int i = 0; i < Topology.InputCount; i++)
\n\t\t\t{
\n\t\t\t\tvar neuron = new Neuron(1, NeuronType.Input);
\n\t\t\t\tinputNeurons.Add(neuron);
\n\t\t\t}
\n\t\t\tvar inputLayer = new Layer(inputNeurons, NeuronType.Input);
\n\t\t\tLayers.Add(inputLayer);
\n\t\t}
\n\t\t
\n\t\tpublic void CacheInDBWeights()
\n\t\t{
\n\t\t\t//serialize weights of all layers
\n\t\t\t//Guid GenNeuralWeightsId = Guid.NewGuid(); // у нас буде один запис який зберігатиме всі коефіцієнти в своєму json
\n\t\t\t 
\n\t\t\tvar weightsJson = \"\"; 
\n\t\t\t// вагові коефіцієти розраховуємо лише для hidden та output layers
\n\t\t\tList<List<List<double>>> weights = new List<List<List<double>>>();
\n\t\t\tforeach (var l in Layers)
\n\t\t\t{
\n\t\t\t\tList<List<double>> layerWeight = new List<List<double>>();
\n\t\t\t\tforeach (var neuro in l.Neurons)
\n\t\t\t\t{
\n\t\t\t\t\tlayerWeight.Add(neuro.Weights);
\n\t\t\t\t}
\n\t\t\t\tweights.Add(layerWeight);
\n\t\t\t\t//weightsJson += JsonConvert.SerializeObject(l.Neurons.SelectMany(neuron => neuron.Weights).ToList());
\n\t\t\t\t//weightsJson += \",\";
\n\t\t\t}
\n\t\t\tweightsJson = JsonConvert.SerializeObject(weights);
\n        \t//weightsJson = weightsJson.TrimEnd(','); // видаляємо останню кому
\n\t\t\t//наш json зберігає масив масивів вагових коефіцієнтів типу [[2.6,1.3,54.6,1.6,31.3,-534.6],[2.6,1.3,54.6,1.6,31.3,-534.6] ...]
\n\t\t\t//weightsJson = \"[\" + weightsJson + \"]\";
\n\t\t\tInsert insertWeights = new Insert(UserConnection)
\n\t\t\t.Into(\"GenNeuralWeights\")
\n\t\t\t//.Set(\"Id\", Column.Parameter(WeightId)) //обов'язковий для вставки лише Id
\n\t\t\t.Set(\"GenWeightsJson\", Column.Parameter(weightsJson))//значення які ми отримаємо при навчанні
\n\t\t\t\t
\n\t\t\t.Set(\"CreatedOn\", Column.Parameter(DateTime.UtcNow))
\n\t\t\t.Set(\"CreatedById\", Column.Parameter(UserConnection.CurrentUser.ContactId))
\n\t\t\t.Set(\"ModifiedById\", Column.Parameter(UserConnection.CurrentUser.ContactId))
\n\t\t\t
\n\t\t\t.Set(\"ModifiedOn\", Column.Parameter(DateTime.UtcNow))
\n\t\t\tas Insert;
\n\t\t\t\t\t\t
\n\t\t\tinsertWeights.Execute();\t
\n\t\t}
\n\t\t
\n\t\tpublic void LoadFromDBWeights()
\n\t\t{
\n\t\t\t//read weights of all layers
\n\t\t\tSelect selectWeights = new Select(UserConnection)
\n        \t.Column(\"GenWeightsJson\").Top(1)
\n        \t.From(\"GenNeuralWeights\")
\n\t\t\t.OrderByDesc(\"ModifiedOn\")
\n\t\t\tas Select;
\n\t\t\t
\n\t\t\tusing (var dbExecutor = UserConnection.EnsureDBConnection())
\n\t\t\t{
\n\t\t\t\tusing (var reader = selectWeights.ExecuteReader(dbExecutor))
\n\t\t\t\t{
\n\t\t\t\t\tif (reader.Read())
\n\t\t\t\t\t{
\n\t\t\t\t\t\tstring jWeights = (reader.GetValue(0) != System.DBNull.Value) ? (string)reader.GetValue(0) : \"\";
\n\t\t\t\t\t\tif (!string.IsNullOrEmpty(jWeights))
\n\t\t\t\t\t\t{
\n\t\t\t\t\t\t\tList<List<List<double>>>  weights = JsonConvert.DeserializeObject<List<List<List<double>>>>(jWeights);
\n
\n\t\t\t\t\t\t\tif (Layers.Count == weights.Count)
\n\t\t\t\t\t\t\t{
\n\t\t\t\t\t\t\t\tfor (int l = 0; l < weights.Count; l++)
\n\t\t\t\t\t\t\t\t{
\n\t\t\t\t\t\t\t\t\tList<List<double>> neuro = weights[l];
\n\t\t\t\t\t\t\t\t\tvar layer = Layers[l];
\n\t\t\t\t\t\t\t\t\tif (neuro.Count == layer.Neurons.Count)
\n\t\t\t\t\t\t\t\t\t{
\n\t\t\t\t\t\t\t\t\t\tfor (var i = 0; i < neuro.Count; i++)
\n\t\t\t\t\t\t\t\t\t\t{
\n\t\t\t\t\t\t\t\t\t\t\tif (layer.Neurons[i].Weights.Count == neuro[i].Count)
\n\t\t\t\t\t\t\t\t\t\t\t\tlayer.Neurons[i].Weights = neuro[i];
\n\t\t\t\t\t\t\t\t\t\t}
\n\t\t\t\t\t\t\t\t\t}
\n\t\t\t\t\t\t\t\t}
\n\t\t\t\t\t\t\t}
\n\t\t\t\t\t\t\t
\n\t\t\t\t\t\t\t/*
\n\t\t\t\t\t\t\tforeach(var layer in Layers)
\n\t\t\t\t\t\t\t{
\n\t\t\t\t\t\t\t\tif(layer.Type == NeuronType.Output)
\n\t\t\t\t\t\t\t\t{
\n\t\t\t\t\t\t\t\t\tif (weights.Count >= 3)
\n\t\t\t\t\t\t\t\t\t{
\n\t\t\t\t\t\t\t\t\t\tList<List<double>> neuro = weights[2];
\n\t\t\t\t\t\t\t\t\t\tif (neuro.Count == layer.Neurons.Count)
\n\t\t\t\t\t\t\t\t\t\t{
\n\t\t\t\t\t\t\t\t\t\t\tfor (var i = 0; i< neuro.Count; i++)
\n\t\t\t\t\t\t\t\t\t\t\t{
\n\t\t\t\t\t\t\t\t\t\t\t\tif (layer.Neurons[i].Weights.Count == neuro[i].Count)
\n\t\t\t\t\t\t\t\t\t\t\t\t\tlayer.Neurons[i].Weights = neuro[i];
\n\t\t\t\t\t\t\t\t\t\t\t}
\n\t\t\t\t\t\t\t\t\t\t}
\n\t\t\t\t\t\t\t\t\t}
\n\t\t\t\t\t\t\t\t\t//layer.Neurons = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Neuron>>(\"GenWeightsJson\"); // треба зчитати останній масив 
\n\t\t\t\t\t\t\t\t}
\n\t\t\t\t\t\t\t\tif(layer.Type == NeuronType.Normal)
\n\t\t\t\t\t\t\t\t{
\n\t\t\t\t\t\t\t\t\tif (weights.Count >= 3)
\n\t\t\t\t\t\t\t\t\t{
\n\t\t\t\t\t\t\t\t\t\tList<List<double>> neuro = weights[1];
\n\t\t\t\t\t\t\t\t\t\tif (neuro.Count == layer.Neurons.Count)
\n\t\t\t\t\t\t\t\t\t\t{
\n\t\t\t\t\t\t\t\t\t\t\tfor (var i = 0; i< neuro.Count; i++)
\n\t\t\t\t\t\t\t\t\t\t\t{
\n\t\t\t\t\t\t\t\t\t\t\t\tif (layer.Neurons[i].Weights.Count == neuro[i].Count)
\n\t\t\t\t\t\t\t\t\t\t\t\t\tlayer.Neurons[i].Weights = neuro[i];
\n\t\t\t\t\t\t\t\t\t\t\t}
\n\t\t\t\t\t\t\t\t\t\t}
\n\t\t\t\t\t\t\t\t\t}
\n\t\t\t\t\t\t\t\t\t//layer.Neurons = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Neuron>>(\"GenWeightsJson\");//зчитуємо всі крім останнього 
\n\t\t\t\t\t\t\t\t}
\n\t\t\t\t\t\t\t\tif(layer.Type == NeuronType.Input)
\n\t\t\t\t\t\t\t\t{
\n\t\t\t\t\t\t\t\t\tif (weights.Count >= 3)
\n\t\t\t\t\t\t\t\t\t{
\n\t\t\t\t\t\t\t\t\t\tList<List<double>> neuro = weights[0];
\n\t\t\t\t\t\t\t\t\t\tif (neuro.Count == layer.Neurons.Count)
\n\t\t\t\t\t\t\t\t\t\t{
\n\t\t\t\t\t\t\t\t\t\t\tfor (var i = 0; i< neuro.Count; i++)
\n\t\t\t\t\t\t\t\t\t\t\t{
\n\t\t\t\t\t\t\t\t\t\t\t\tif (layer.Neurons[i].Weights.Count == neuro[i].Count)
\n\t\t\t\t\t\t\t\t\t\t\t\t\tlayer.Neurons[i].Weights = neuro[i];
\n\t\t\t\t\t\t\t\t\t\t\t}
\n\t\t\t\t\t\t\t\t\t\t}
\n\t\t\t\t\t\t\t\t\t}
\n\t\t\t\t\t\t\t\t\t//layer.Neurons = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Neuron>>(\"GenWeightsJson\"); // треба зчитати останній масив 
\n\t\t\t\t\t\t\t\t}
\n\t\t\t\t\t\t\t\t//layer.Neurons = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Neuron>>(\"GenWeightsJson\");
\n\t\t\t\t\t\t\t}
\n\t\t\t\t\t\t\t*/
\n\t\t\t\t\t\t}
\n\t\t\t\t\t}
\n\t\t\t\t}
\n\t\t\t}
\n\t\t}
\n\t}
\n
\n}",
  "MetaData": "{\r
\n  \"MetaData\": {\r
\n    \"Schema\": {\r
\n      \"ManagerName\": \"SourceCodeSchemaManager\",\r
\n      \"UId\": \"d3947c7a-8bae-47a6-8b5f-5ffc804e9745\",\r
\n      \"A2\": \"GenNeuralNetwork\",\r
\n      \"A5\": \"0bd87f3b-859d-4a49-9e89-ba853bfeb6e6\",\r
\n      \"B1\": [],\r
\n      \"B2\": [],\r
\n      \"B3\": [],\r
\n      \"B6\": \"92cea9ed-4580-cf30-a59e-c5bde6193519\",\r
\n      \"B8\": \"8.0.8.4807\",\r
\n      \"HD1\": \"50e3acc0-26fc-4237-a095-849a1d534bd3\"\r
\n    }\r
\n  }\r
\n}",
  "LocalizableValues": [
    {
      "Culture": "ru-RU",
      "ResourceType": "String",
      "Key": "Caption",
      "Value": "NeuralNetwork",
      "ImageData": ""
    },
    {
      "Culture": "en-US",
      "ResourceType": "String",
      "Key": "Caption",
      "Value": "NeuralNetwork",
      "ImageData": ""
    },
    {
      "Culture": "en-US",
      "ResourceType": "String",
      "Key": "Description",
      "Value": "Нейронна мережа для підбору транспорту до запиту ",
      "ImageData": ""
    },
    {
      "Culture": "uk-UA",
      "ResourceType": "String",
      "Key": "Caption",
      "Value": "NeuralNetwork",
      "ImageData": ""
    },
    {
      "Culture": "uk-UA",
      "ResourceType": "String",
      "Key": "Description",
      "Value": "Нейронна мережа для підбору транспорту до запиту ",
      "ImageData": ""
    }
  ],
  "Properties": [
    {
      "Name": "CreatedInVersion",
      "Value": "8.0.8.4807"
    }
  ]
}
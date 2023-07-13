namespace MachineLearning
{
    public static class Utils
    {
        public static double Sigmoid(double x)
        {
            return 1 / (1 + Math.Exp(-x));
        }

        public static double GenerateRandomValue()
        {
            var rnd = new Random();
            //var minValue = 0.00000000000001;
            //return minValue + rnd.NextDouble() * (1 - minValue);

            return rnd.NextDouble();
        }

        public static double DerivedSigmoid(double x)
        {
            var sigmoid = Sigmoid(x);
            return sigmoid * (1 - sigmoid);
        }
    }
}

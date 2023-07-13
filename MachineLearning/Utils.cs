namespace MachineLearning
{
    public static class Utils
    {
        public static double Sigmoid(double x)
        {
            return 1 / (1 + Math.Exp(-x));
        }

        public static double DerivedSigmoid(double x)
        {
            var sigmoid = Sigmoid(x);
            return sigmoid * (1 - sigmoid);
        }
    }
}

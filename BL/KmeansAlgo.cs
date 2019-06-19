using Accord.MachineLearning;
using System.Collections.Generic;


namespace BL
{
    class KmeansAlgo
    {

        private readonly int _centerAmount;
        private readonly double[][] _observations;

        public KmeansAlgo(double[][] observations, int centerAmount)
        {
            _centerAmount = centerAmount;
            _observations = observations;

            if (centerAmount > observations.Length)
                _centerAmount = observations.Length;
        }

        public List<object[]> Run()
        {
            KMeans kmeans = new KMeans(_centerAmount);
            var clusters = kmeans.Learn(_observations);
            int[] labels = clusters.Decide(_observations);


            int centerNumber = 1;
            List<object[]> result = new List<object[]>(); // object[centerNumber] = pushpin data = {latitude, longitude, name, centerNumber}
            foreach (var center in clusters.Centroids)
            {
                result.Add(new object[] { center[0], center[1], "c" + centerNumber.ToString(), centerNumber });

                int j = 0;
                foreach (var point in _observations)
                {
                    if (labels[j++] == centerNumber - 1)
                        result.Add(new object[] { point[0], point[1], "", centerNumber });
                }
                centerNumber++;
            }

            return result;
        }
    }
}



using Accord.MachineLearning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    class KmeansAlgo
    {
        /*
         
        #region tt

        public int _centerAmount;

        public KmeansAlgo(int centerAmount, )
        {
            _centerAmount = centerAmount;
        }

        public void Run()
        {

            
            int reportAmount = 5, i = 0;
            var lastResults = new double[_centerAmount][];
            double[][] observations = new double[reportAmount][];


            // adding the points to the k means algo

            foreach (var report in reports)
            {
                observations[i++] = new double[latitude, longitude];
            }



            // running kmeans algo

            KMeans kmeans = new KMeans(_centerAmount);
            var clusters = kmeans.Learn(observations);
            int[] labels = clusters.Decide(observations);

            i = 1;
            foreach (var center in clusters.Centroids)
            {
                int j = 0;
                Pin.Invoke(this, new PropertyChangedEventArgs(center[0].ToString() + "," + center[1], ToString() + ",c" + i));
                foreach (var report in reports)
                {
                    if (labels[j++] == i - 1)
                        Pin.Invoke(this, new PropertyChangedEventArgs(report.longtitude.tostring() + "," + report.latitude.tostring()));// maybe need to switch between long and lat
                }
                lastResults[i - 1] = new double[] { center[0], center[1] };
                i++;
            }
            database.setKmeansResults(lastResults);
        }


        // set the color of a pin by label
        public ConsoleColor GetLabelColor(int label)
        {
            switch(label)
            {
                case 0:
                    return ConsoleColor.Red;

                case 1:
                    return ConsoleColor.White;

                case 2:
                    return ConsoleColor.Green;

                case 3:
                    return ConsoleColor.Blue;

                case 4:
                    return ConsoleColor.Black;

                default:
                    return ConsoleColor.Red;

            }

    
        }

        #endregion

        */
    }

}

using System;
using System.IO;

using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace ACE
{
    class ACEMethod
    {
        //============================CONSTANTS===============================
        // maximum value for color channel
        private const int _channelMax = 255;
        // minimum value for color channel
        private const int _channelMin = 0;
        // 3x3 subset of pixels
        private static int s_subsetSize = 3;
        // slope for Saturation function r(*)
        private const int _slopeR = 20;
        private const string _nameFirst = "Original image";
        private const string _nameSecond = "Oc";
        //==============================MAIN==================================
        public static void Run(string PATH)
        {
            // stopwatch to time the method
            System.Diagnostics.Stopwatch stopWatch = new();
            stopWatch.Start();

            var img2 = CvInvoke.Imread(PATH, ImreadModes.Grayscale);
            var img = (byte[,])img2.GetData();
            s_subsetSize = (img.GetLength(0) > img.GetLength(1) ?
                        img.GetLength(0) : img.GetLength(1));

            //------------first step: count Rc for the whole image------------
            // array for r(*) values of subset
            var rArr = new float[s_subsetSize, s_subsetSize];

            var newImg = new float[img.GetLength(0), img.GetLength(1)];
            float minR = 0.0F;
            float maxR = 0.0F;

            for (int i = 0; i < img.GetLength(0); i++)
            {
                for (int j = 0; j < img.GetLength(1); j++)
                {
                    var rMax = s_countSubset(rArr, img, i, j);
                    newImg[i, j] = s_funcR(rArr, rMax);
                    if (i == 0 && j == 0)
                        minR = newImg[i, j];
                    if (minR > newImg[i, j])
                        minR = newImg[i, j];
                    if (maxR < newImg[i, j])
                        maxR = newImg[i, j];
                }
            }
            /*
                after the first step we get an array of
                float values that will be scaled
                from 0 to 255 in the second step
            */

            //----------------second step: count Oc for all Rc----------------
            // Oc = round[ slope_O*(Rc(p) - min_R) ]
            var resImg = new byte[img.GetLength(0), img.GetLength(1)];

            float slopeO = 255.0F / (maxR - minR);
            for (int i = 0; i < newImg.GetLength(0); i++)
            {
                for (int j = 0; j < newImg.GetLength(1); j++)
                {
                    // round in python: int(A + (0.5 if A > 0 else -0.5))
                    var tmp = slopeO * (newImg[i, j] - minR);
                    if (tmp > 0)
                        tmp += 0.5F;
                    else
                        tmp -= 0.5F;
                    var res = (int)tmp;
                    resImg[i, j] = (byte)res;
                    if (res < 0)
                        Console.WriteLine(".....how?");
                }
            }
            //----------------------------------------------------------------
            Console.WriteLine(slopeO);

            //=====================THIS NEEDS TO BE FIXED=====================
            byte[] tmpArr = new byte[img.GetLength(0) * img.GetLength(1)];
            int k = 0;
            for (int i = 0; i < img.GetLength(0); i++)
            {
                for (int j = 0; j < img.GetLength(1); j++)
                {
                    tmpArr[k] = img[i, j];
                    k++;
                }
            }
            var imgNew = img2.Clone();
            imgNew.SetTo<byte>(tmpArr);
            tmpArr = new byte[resImg.GetLength(0) * resImg.GetLength(1)];
            k = 0;
            for (int i = 0; i < resImg.GetLength(0); i++)
            {
                for (int j = 0; j < resImg.GetLength(1); j++)
                {
                    tmpArr[k] = resImg[i, j];
                    k++;
                }
            }
            var resImgNew = img2.Clone();
            resImgNew.SetTo<byte>(tmpArr);
            //================================================================

            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            // Format and display the TimeSpan value.
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            Console.WriteLine("ACE RunTime - " + elapsedTime);

            CvInvoke.Imshow(_nameFirst, imgNew);    //imgNew is redundant!!!
            CvInvoke.WaitKey(0);
            //CvInvoke.DestroyWindow(_nameFirst);
            CvInvoke.Imshow(_nameSecond, resImgNew);
            CvInvoke.WaitKey(0);
            //CvInvoke.Imwrite("Oc.jpg", resImg);
            //cv2.destroyWindow(name_second)

            CvInvoke.DestroyAllWindows();
            //----------------------------------------------------------------
            //var img = CvInvoke.Imread(PATH, ImreadModes.Grayscale);
            //s_subsetSize = (img.Cols > img.Rows ? img.Cols : img.Rows);
        }

        //============================FUNCTIONS===============================
        // r(*) is a Saturation function
        /*
            Saturation function r(x):
            | a <= x <= b->k * x
            | a > x->a
            | x > b->b
            where k-slope
        */
        private static float s_funcR(float x)
        {
            float ans = 0.0F;
            if ((_channelMin <= x) && (x <= _channelMax))
            {
                ans = _slopeR * x;
            }
            else if (x > _channelMax)
            {
                ans = _channelMax;
            }
            else
            {
                ans = _channelMin;
            }
            return ans;
        }

        // count r(*) for subset
        // (to, from, i, j)
        private static float s_countSubset(float[,] arr, byte[,] pix,
                                int i_p = 0, int j_p = 0)
        {
            float tmp = 0.0F;
            int size = (int)(s_subsetSize / 2);
            float max = 0.0F;
            for (int i = 0; i < s_subsetSize; i++)
            {
                for (int j = 0; j < s_subsetSize; j++)
                {
                    int a = i_p - size + i;
                    int b = j_p - size + j;
                    if (!((0 <= a) && (a < pix.GetLength(0))) ||
                        !((0 <= b) && (b < pix.GetLength(1))))
                    {
                        tmp = 0;
                    }
                    else
                    {
                        tmp = (int)pix[i_p, j_p] - (int)pix[a, b];
                    }
                    arr[i, j] = s_funcR(tmp);
                    if (arr[i, j] >= max)
                        max = arr[i, j];
                }
            }
            return max;
        }

        // d(p, q) is Euclidian distance: sqrt(sum_i((p_i -q_i)^2))
        /*
            in this program p, q are considered to be
            duplexes of coordinates of two pixels
            (tuples with 2 elements)
        */
        private static float s_funcD((int, int) p, (int, int) q) {
            return MathF.Sqrt(MathF.Pow((p.Item1 - q.Item1), 2)
                    + MathF.Pow((p.Item2 - q.Item2), 2));
        }

        // Rc is a first step of ACE method
        /*
            because of using square area around a pixel(p)
            as subset (3x3, 5x5, etc), I presume that
            while counting d(p, q) coordinates of p
                will be center of said subset
        */
        private static float s_funcR(float[,] r, float r_max)
        {
            float ans = 0.0F;
            float numer = 0.0F;     // numerator
            float denom = 0.0F;     // denominator
            int center = (int)(s_subsetSize / 2);
            for (int i = 0; i < r.GetLength(0); i++)
            {
                for (int j = 0; j < r.GetLength(1); j++)
                {
                    float tmp = s_funcD((center, i), (center, j));
                    if (tmp == 0)
                        continue;
                    numer += r[i, j] / tmp;
                    //print("numer", numer)       // DEBUG
                    denom += r_max / tmp;
                    //print("denom", denom)       // DEBUG
                }
            }
            if (denom != 0)
                ans = numer / denom;
            return ans;
        }
    }
}
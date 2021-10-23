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
            var img2 = CvInvoke.Imread(PATH, ImreadModes.Grayscale);
            var img = img2.GetData();
            s_subsetSize = (img.GetLength(0) > img.GetLength(1) ?
                        img.GetLength(0) : img.GetLength(1));

            //------------first step: count Rc for the whole image------------
            // array for r(*) values of subset
            var r_arr = new float[s_subsetSize, s_subsetSize];

            var new_img = new float[img.GetLength(0), img.GetLength(1)];
            float min_R = 0.0F;
            float max_R = 0.0F;
            /*
            for i in range(0, img.shape[0]):
                for j in range(0, img.shape[1]):
                    r_max = count_subset(r_arr, img, i, j)
                    new_img[i][j] = func_R(r_arr, r_max)
                    if i == 0 and j== 0:
                        min_R = new_img[i][j]
                    if min_R > new_img[i][j]:
                        min_R = new_img[i][j]
                    if max_R < new_img[i][j]:
                        max_R = new_img[i][j]
            /*
                after the first step we get an array of
                float values that will be scaled
                from 0 to 255 in the second step
            
            //----------------second step: count Oc for all Rc----------------
            // Oc = round[ slope_O*(Rc(p) - min_R) ]
            res_img = np.zeros(img.shape, np.uint8)

            //print(new_img[0][98], new_img[255][255], new_img[600][1000])                   #DEBUG

            float slope_O = 255.0F / (max_R - min_R);
            for i in range(0, new_img.shape[0]):
                for j in range(0, new_img.shape[1]):
                    #round in python: int(A + (0.5 if A > 0 else -0.5))
                    tmp = slope_O * (new_img[i][j] - min_R)
                    res = int(tmp + (0.5 if tmp > 0 else -0.5))
                    res_img[i][j] = res
                    if res < 0:
                        print(".....how?")

            //print(res_img[0][98], res_img[255][255], res_img[600][1000])                   #DEBUG
            //----------------------------------------------------------------
            //print(img.shape)
            //print(min_R, max_R)
            Console.WriteLine(slope_O);

            //print("--- %s seconds ---" % (time.time() - start_time))

            CvInvoke.Imshow(_nameFirst, img);
            cv2.waitKey(0)
            //cv2.destroyWindow(name_first)
            cv2.imshow(name_second, res_img)
            cv2.waitKey(0)
            cv2.imwrite("Oc.jpg", res_img)
            //cv2.destroyWindow(name_second)

            cv2.destroyAllWindows()
            */
            //----------------------------------------------------------------
            String win1 = "Test Window";
            //var img = CvInvoke.Imread(PATH, ImreadModes.Grayscale);
            //s_subsetSize = (img.Cols > img.Rows? img.Cols : img.Rows);
            Console.WriteLine(s_subsetSize);
            CvInvoke.NamedWindow(win1);
            CvInvoke.Imshow(win1, img2);
            CvInvoke.WaitKey(0);
            CvInvoke.DestroyWindow(win1);
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
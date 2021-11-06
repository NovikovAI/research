using System;
using System.IO;

using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace ACE
{
    class Program
    {
        static void Main(string[] args)
        {

            string PATH = Directory.GetCurrentDirectory();
            PATH += "//..//..//..//";
            PATH += "..//images//test.jpg";
            //start_time = time.time()
            //============================MAIN================================
            {
                /*
                img = cv2.imread("test.jpg", cv2.IMREAD_GRAYSCALE)

                subset_size = (img.shape[0] if img.shape[0] > img.shape[1] else img.shape[1])

                //-----first step: count Rc for the whole image-----
                //array for r(*) values of subset
                r_arr = np.zeros((subset_size, subset_size), float)

                new_img = np.zeros(img.shape, float)
                min_R = float(0)
                max_R = float(0)

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
                '''
                    after the first step we get an array of
                    float values that will be scaled
                    from 0 to 255 in the second step
                '''
                //---------second step: count Oc for all Rc---------
                //Oc = round[ slope_O*(Rc(p) - min_R) ]
                res_img = np.zeros(img.shape, np.uint8)

                //print(new_img[0][98], new_img[255][255], new_img[600][1000])                   #DEBUG

                slope_O = float(255) / (max_R - min_R)
                for i in range(0, new_img.shape[0]):
                    for j in range(0, new_img.shape[1]):
                        #round in python: int(A + (0.5 if A > 0 else -0.5))
                        tmp = slope_O * (new_img[i][j] - min_R)
                        res = int(tmp + (0.5 if tmp > 0 else -0.5))
                        res_img[i][j] = res
                        if res < 0:
                            print(".....how?")

                //print(res_img[0][98], res_img[255][255], res_img[600][1000])                   #DEBUG
                //--------------------------------------------------
                //print(img.shape)
                //print(min_R, max_R)
                print(slope_O)

                print("--- %s seconds ---" % (time.time() - start_time))

                cv2.imshow(name_first, img)
                cv2.waitKey(0)
                //cv2.destroyWindow(name_first)
                cv2.imshow(name_second, res_img)
                cv2.waitKey(0)
                cv2.imwrite("Oc.jpg", res_img)
                //cv2.destroyWindow(name_second)

                cv2.destroyAllWindows()
                */
            }
            /*
            String win1 = "Test Window";
            var img = CvInvoke.Imread(PATH, ImreadModes.Grayscale);
            CvInvoke.NamedWindow(win1);
            CvInvoke.Imshow(win1, img);
            CvInvoke.WaitKey(0);
            CvInvoke.DestroyWindow(win1);
            
            Console.WriteLine(img.GetType());
            var img2 = (byte[,])img.GetData();       // I WILL BE USING THIS!!
            //Console.WriteLine(img2.GetType());
            */

            /*
             нужно ппереписать метод ACE так, чтобы он получал объект Mat,
             а не путь к изображению, чтобы можно было потом добавлять
             методы, которые будут вызываться до или после ACE
            */

            System.Diagnostics.Stopwatch stopWatch = new();
            stopWatch.Start();
            ACEMethod.Run(PATH);      //I guess now it will need img2 OR img
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;

            // Format and display the TimeSpan value.
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            Console.WriteLine("RunTime " + elapsedTime);
            /*
            //===========from byte[][] to byte[]===========
            byte[] tmp = new byte[img.Rows * img.Cols];
            int k = 0;
            for (int i = 0; i < img.Rows; i++)
            {
                for (int j = 0; j < img.Cols; j++)
                {
                    tmp[k] = img2[i, j];
                    k++;
                }
            }
            img.SetTo<byte>(tmp);
            //=============================================
            CvInvoke.Imshow(win1, img);
            CvInvoke.WaitKey(0);
            */
            CvInvoke.DestroyAllWindows();
        }
    }
}
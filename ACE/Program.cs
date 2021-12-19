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
            var imgPath = PATH + "..//images//parrot.png";
            string imgSaveName = "";        //if it's empty it won't save
            //============================MAIN================================
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

            var img = CvInvoke.Imread(imgPath, ImreadModes.Unchanged);

            var tmp = img.GetData();
            //int i = tmp.GetLength(0);
            //int j = tmp.GetLength(1);
            //int k = tmp.GetLength(2);
            Type byte2 = typeof(Byte[,]);
            Type byte3 = typeof(Byte[,,]);

            Console.WriteLine(tmp.GetType().Equals(byte3));
            /*
               now based on this I can make a unified method for
               all types of images
               also using the tmp.GetLength(2) method to
               get number of channels
            */

            //var resImg = ACEMethod.s_run(img, imgSaveName);

            CvInvoke.DestroyAllWindows();
        }
    }
}
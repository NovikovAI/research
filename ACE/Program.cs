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

            // I think I should change the input to img itself
            // and add an option whether to save an image 
            // and under what name
            ACEMethod.Run(PATH);

            CvInvoke.DestroyAllWindows();
        }
    }
}
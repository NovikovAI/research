using System;

namespace ACE
{
    class NewProgram
    {
        public static void Examples()
        {//code examples
            string name = "Tom";
            int age = 34;
            //int age = Convert.ToInt32(Console.ReadLine());
            double height = 1.7;
            Console.WriteLine($"Имя: {name}  Возраст: {age}  Рост: {height}м");
            //Console.WriteLine("Имя: {0}  Возраст: {2}  Рост: {1}м", name, height, age);

            int a = 3;
            int b = 5;
            int c = 40;
            int d = c-- - b * a;    // a=3  b=5  c=39  d=25
            Console.WriteLine($"a={a}  b={b}  c={c}  d={d}");
            //одномерные массивы
            int[] nums2 = new int[4] { 1, 2, 3, 5 };
            int[] nums3 = new int[] { 1, 2, 3, 5 };
            int[] nums4 = new[] { 1, 2, 3, 5 };
            int[] nums5 = { 1, 2, 3, 5 };
            //двухмерные массивы
            int[,] nums21;
            int[,] nums22 = new int[2, 3];
            int[,] nums23 = new int[2, 3] { { 0, 1, 2 }, { 3, 4, 5 } };
            int[,] nums24 = new int[,] { { 0, 1, 2 }, { 3, 4, 5 } };
            int[,] nums25 = new [,]{ { 0, 1, 2 }, { 3, 4, 5 } };
            int[,] nums26 = { { 0, 1, 2 }, { 3, 4, 5 } };
            //трехмерный
            int[,,] nums33 = new int[2, 3, 4];
            //пример с массивом
            {
                int[,] mas = { { 1, 2, 3 }, { 4, 5, 6 }, { 7, 8, 9 }, { 10, 11, 12 } };
                int rows = mas.GetUpperBound(0) + 1;
                int columns = mas.Length / rows;
                // или так
                // int columns = mas.GetUpperBound(1) + 1;

                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < columns; j++)
                    {
                        Console.Write($"{mas[i, j]} \t");
                    }
                    Console.WriteLine();
                }
            }
            //"зубчатый" массив(массив массивов)
            int[][] nums = new int[3][];
            nums[0] = new int[2] { 1, 2 };          // выделяем память для первого подмассива
            nums[1] = new int[3] { 1, 2, 3 };       // выделяем память для второго подмассива
            nums[2] = new int[5] { 1, 2, 3, 4, 5 }; // выделяем память для третьего подмассива

            Console.WriteLine("{0} \t {1}", nums25.GetLength(0), nums25.GetUpperBound(0));

            //Console.ReadKey();
        }
    }
}
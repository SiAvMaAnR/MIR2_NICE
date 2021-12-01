using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SecondLaba
{
    class Program
    {
        static object locker = new object();
        static Random random = new Random();
        static List<(int, string, int)> Results = new List<(int, string, int)>();

        static void Main(string[] args)
        {
            //string text = Console.ReadLine();
            //Console.WriteLine(levenshtein(text, pattern));
            try
            {
                List<string> words = new List<string>();
                for (int i = 0; i < 100; i++)
                {
                    words.Add(new string(GenerationString(10, 20, null, "ABCDEFGHIJKLMNOPQRSTUVWXYZ")));
                }

                Console.ForegroundColor = ConsoleColor.Green;
                foreach (var item in words)
                {
                    Console.WriteLine($" {words.IndexOf(item) + 1} ==> {item}");
                }
                Console.ResetColor();

                Console.Write("Введите стоимость добавления: ");
                int insertCost = int.Parse(Console.ReadLine());
                Console.Write("Введите стоимость удаления: ");
                int deleteCost = int.Parse(Console.ReadLine());
                Console.Write("Введите стоимость замены: ");
                int replaceCost = int.Parse(Console.ReadLine());
                Console.Write("Введите k возможных различий: ");
                int k = int.Parse(Console.ReadLine());

                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("Введите искомое словосочетание: ");
                string pattern = Console.ReadLine();

                Console.ResetColor();
                Console.Write("Совпадения:\n");
                foreach (var item in words)
                {
                    int length = MethodWF(item, pattern, insertCost, deleteCost, replaceCost);
                    if (length <= k)
                        Results.Add((length, item, words.IndexOf(item) + 1));
                }
                foreach (var searchString in Results.OrderBy(x => x.Item1))
                {
                    Console.WriteLine($" {searchString.Item3} ==> {searchString.Item2} ({searchString.Item1})");
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                System.Console.WriteLine(ex.Message);
                Console.ForegroundColor = ConsoleColor.Red;
                System.Console.WriteLine(ex);
                Console.ResetColor();
            }
            Console.ReadKey();
        }

        //Метод Вагнера-Фишера
        static int MethodWF(string s1, string s2, int InsertCost = 1, int DeleteCost = 1, int ReplaceCost = 1)
        {
            s1 = s1.ToUpper();
            s2 = s2.ToUpper();

            if (string.IsNullOrEmpty(s1))
            {
                return (!string.IsNullOrEmpty(s2)) ? s2.Length : 0;
            }
            if (string.IsNullOrEmpty(s2))
            {
                return (!string.IsNullOrEmpty(s1)) ? s1.Length : 0;
            }

            int[,] D = new int[s1.Length + 1, s2.Length + 1];
            int M = D.GetUpperBound(0);
            int N = D.GetUpperBound(1);
            D[0, 0] = 0;
            for (int i = 1; i <= M; i++)
            {
                D[i, 0] = D[i - 1, 0] + DeleteCost;
            }
            for (int j = 1; j <= N; j++)
            {
                D[0, j] = D[0, j - 1] + InsertCost;
            }
            for (int i = 1; i <= M; i++)
            {
                for (int j = 1; j <= N; j++)
                {
                    if (s1[i - 1] != s2[j - 1])
                        D[i, j] = Math.Min(Math.Min(D[i - 1, j] + DeleteCost, D[i, j - 1] + InsertCost), D[i - 1, j - 1] + ReplaceCost);
                    else
                        D[i, j] = D[i - 1, j - 1];
                }
            }
            return D[M, N];
        }

        static char[] GenerationString(int n, int m, char[] text = default, string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz")
        {
            if (alphabet == null)
            {
                alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz ";
            }

            int length = random.Next(n, m + 1);

            if (text == null)
            {
                text = new char[length];
                if (Parallel.For(0, length, x =>
                {
                    int index;
                    lock (locker)
                    {
                        index = random.Next(0, alphabet.Length);
                    }
                    text[x] = alphabet[index];
                }).IsCompleted)
                {
                    return text;
                }
            }
            return text;
        }
    }
}

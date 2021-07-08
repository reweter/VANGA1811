using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Vanga
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to algorithm 'Vanga'!");
            Console.WriteLine($"StartTime: {DateTime.Now}");
            Vanga.GenerateBlock(1073741824);
            Console.WriteLine($"EndTime: {DateTime.Now}");
            Console.WriteLine("End algorithm work!");
            Console.ReadKey();
        }
    }

    //VANGA1811 (VANGA1152)
    public static class Vanga
    {
        public static char[] Letters = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'q', 'a', 'z', 'w', 's', 'x', 'e', 'd', 'c', 'r', 'f', 'v', 't', 'g', 'b', 'y', 'h', 'n', 'u', 'j', 'm', 'i', 'k', 'o', 'l', 'p' };

        public static void GenerateBlock(long BlockSize)
        {
            //Generates a random GenesisHash based on the parameters: DateTime.Now, Random(char[10])
            string GetGenesisHash()
            {
                Random rand = new Random();
                char[] sims = new char[10];

                for (int i = 0; i < 10; i++)
                    sims[i] = Letters[rand.Next(0, Letters.Length)];

                StringBuilder predHashData = new StringBuilder();

                for (int i = 9; i >= 5; i--)
                    predHashData.Append(sims[i]);

                predHashData.Append(DateTime.Now.ToString());

                for (int i = 4; i >= 0; i--)
                    predHashData.Append(sims[i]);

                return GetSHA256(predHashData.ToString());
            }

            //Combining arrays
            string[] Unify(string[] a, string[] b)
            {
                string[] c = new string[a.Length + b.Length];
                for (int i = 0; i < a.Length; i++)
                    c[i] = a[i];
                for (int j = 0; j < b.Length; j++)
                    c[a.Length + j] = b[j];
                return c;
            }

            //Combining three arrays
            string[] Unify3(string[] a, string[] b, string[] c)
            {
                string[] d = new string[a.Length + b.Length];
                for (int i = 0; i < a.Length; i++)
                    d[i] = a[i];
                for (int j = 0; j < b.Length; j++)
                    d[a.Length + j] = b[j];
                for (int k = 0; k < c.Length; k++)
                    d[b.Length + k] = c[k];
                return d;
            }

            //Combines array elements
            string[] UnifyElements(string[] a, string[] b)
            {
                string[] c = new string[a.Length];

                for (int i = 0; i < a.Length; i++)
                    c[i] = a[i] + b[i];

                return c;
            }

            //Converts a hash to an array of numbers and then adds them up
            long HashNumberOper(string hash)
            {
                int[] nums = new int[hash.Length];

                for (int i = 0; i < hash.Length; i++)
                    nums[i] = Convert.ToInt32(hash[i]);

                long value = 0;

                for (int i = 0; i < nums.Length; i++)
                    value += nums[i];

                return value;
            }

            //Splitting a string into a certain number of chunks
            IEnumerable<string> Split(string s, int chunkSize)
            {
                int chunkCount = s.Length / chunkSize;
                for (int i = 0; i < chunkCount; i++)
                    yield return s.Substring(i * chunkSize, chunkSize);

                if (chunkSize * chunkCount < s.Length)
                    yield return s.Substring(chunkSize * chunkCount);
            }

            //Get 16 words of 4 characters from a hash
            string[] GetHash16Words(string hash)
            {
                string[] words = Split(hash, 4).ToArray();
                return words;
            }

            //Adds a value to the end of each element of the array
            string[] AddToMassEnd(string[] mass, string toEnd)
            {
                string[] mas = new string[mass.Length];

                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < mass.Length; i++)
                {
                    sb.Append(mass[i]);
                    sb.Append(toEnd);
                    mas[i] = sb.ToString();
                    sb.Clear();
                }

                return mas;
            }

            string[] GetOneMass(string[][] mass)
            {
                List<string> elements = new List<string>();

                for (int i = 0; i < mass.Length; i++)
                {
                    for (int e = 0; e < mass[i].Length; e++)
                        elements.Add(mass[i][e]);
                }

                return elements.ToArray();
            }

            string GenesisHash = GetGenesisHash();

            //Get GenesisWords from GenesisHash
            string[] GetGenesisWords(string GenesisHash)
            {
                string[] GenesisWords = new string[32];

                StringBuilder sb = new StringBuilder();

                for (int i = 0, c = 0; i < 64; c++)
                {
                    sb.Append(GenesisHash[i]);
                    sb.Append(GenesisHash[i + 1]);
                    GenesisWords[c] = sb.ToString();
                    sb.Clear();
                    if (i + 2 <= GenesisHash.Length + 3)
                        i += 2;
                    else
                        break;
                }

                return GenesisWords;
            }

            string[] GenesisWords = GetGenesisWords(GenesisHash);

            //Swap array
            Array.Reverse(GenesisWords);

            //Turns a list of words into a hash (SHA256)
            string[] WordsForHash256(string[] Words)
            {
                string[] hash = new string[Words.Length];

                for (int i = 0; i < Words.Length; i++)
                    hash[i] = GetSHA256(Words[i]);

                return hash;
            }

            //Turns a list of words into a hash (SHA512)
            string[] WordsForHash512(string[] Words)
            {
                string[] hash = new string[Words.Length];

                for (int i = 0; i < Words.Length; i++)
                    hash[i] = GetSHA512(Words[i]);

                return hash;
            }

            //Turns a list of words into a hash (SHA384)
            string[] WordsForHash384(string[] Words)
            {
                string[] hash = new string[Words.Length];

                for (int i = 0; i < Words.Length; i++)
                    hash[i] = GetSHA384(Words[i]);

                return hash;
            }

            //32 words
            string[] GenesisWordsHash = WordsForHash256(GenesisWords);

            //half 0 or 1
            //Paraleling
            string[] GetHalfHashWords(uint half, string[] hashWords)
            {
                string[] halfPart = new string[32];

                StringBuilder sb = new StringBuilder();

                if (half == 0)
                {
                    for (int s = 0; s < hashWords.Length; s++)
                    {
                        for (int i = 0; i < 31 + 1; i++)
                        {
                            sb.Append(hashWords[s][i]);
                        }
                        halfPart[s] = sb.ToString();
                        sb.Clear();
                    }
                }
                else if (half == 1)
                {
                    for (int s = 0; s < hashWords.Length; s++)
                    {
                        for (int i = 32; i < 64; i++)
                        {
                            sb.Append(hashWords[s][i]);
                        }
                        halfPart[s] = sb.ToString();
                        sb.Clear();
                    }
                }

                halfPart = WordsForHash256(halfPart);

                return halfPart;
            }

            //Hash Words 0 (32 hashs)
            string[] hw0x0 = GetHalfHashWords(0, GenesisWordsHash);
            //Hash Words 1 (32 hashs)
            string[] hw0x1 = GetHalfHashWords(1, GenesisWordsHash);

            //0x01 STAGE Complete. Db (DataBlocks): 2, Total: 64 hash

            string[] hw0x0S = new string[hw0x0.Length];
            string[] hw0x1S = new string[hw0x1.Length];

            hw0x0.CopyTo(hw0x0S, 0);
            hw0x1.CopyTo(hw0x1S, 0);

            Array.Reverse(hw0x0S);
            Array.Reverse(hw0x1S);

            hw0x0S = WordsForHash256(hw0x0S);
            hw0x1S = WordsForHash256(hw0x1S);

            //0x02 STAGE Complete. Db: 4, Total: 128

            string[] hw0x0and0x1U = Unify(hw0x0, hw0x1);
            string[] hw0x0Sand0x1SU = Unify(hw0x0S, hw0x1S);

            //Hash Words 3 (128 hashs)
            string[] hw0x3 = Unify(hw0x0and0x1U, hw0x0Sand0x1SU);

            //Hash Words 3 (128 hashs) Clone
            string[] hw0x3C = new string[hw0x3.Length];

            hw0x3.CopyTo(hw0x3C, 0);

            Array.Reverse(hw0x3C);

            //Hash Words 4 (256 hashs)
            string[] hw0x4 = Unify(hw0x3, hw0x3C);

            //0x03 STAGE Complete. Db: 1, Total: 256

            string[] nm0x0 = new string[128];
            string[] nm0x1 = new string[128];

            Array.Copy(hw0x4, 0, nm0x0, 0, 128);
            Array.Copy(hw0x4, 129, nm0x1, 0, 127);
            //Bug fix
            nm0x1[127] = hw0x4[254];
            nm0x1[127] = hw0x4[255];

            //SHA384
            string[] nm0x0SHA384 = WordsForHash384(nm0x0);
            //SHA512
            string[] nm0x1SHA512 = WordsForHash512(nm0x1);

            //Minor gets [0], [1], [2], [126], [127]
            //Major gets [101], [94], [115], [26], [4]
            string[] nm0x0_Minor = new string[] { nm0x0[0], nm0x0[1], nm0x0[2], nm0x0[126], nm0x0[127] };
            string[] nm0x0_Major = new string[] { nm0x0[101], nm0x0[94], nm0x0[115], nm0x0[26], nm0x0[4] };
            string[] nm0x1_Minor = new string[] { nm0x1[0], nm0x1[1], nm0x1[2], nm0x1[126], nm0x1[127] };
            string[] nm0x1_Major = new string[] { nm0x1[101], nm0x1[94], nm0x1[115], nm0x1[26], nm0x1[4] };

            long nm0x0_Minor_Sum = 0;
            long nm0x0_Major_Sum = 0;
            long nm0x1_Minor_Sum = 0;
            long nm0x1_Major_Sum = 0;

            for (int i = 0; i < nm0x0_Minor.Length; i++)
                nm0x0_Minor_Sum += HashNumberOper(nm0x0_Minor[i]);

            for (int i = 0; i < nm0x0_Major.Length; i++)
                nm0x0_Major_Sum += HashNumberOper(nm0x0_Major[i]);

            for (int i = 0; i < nm0x1_Minor.Length; i++)
                nm0x1_Minor_Sum += HashNumberOper(nm0x1_Minor[i]);

            for (int i = 0; i < nm0x1_Major.Length; i++)
                nm0x1_Major_Sum += HashNumberOper(nm0x1_Major[i]);

            nm0x0_Minor_Sum *= -1;
            nm0x1_Minor_Sum *= -1;

            long Minor_Sum = nm0x0_Minor_Sum += nm0x1_Minor_Sum;
            long Major_Sum = nm0x0_Major_Sum += nm0x1_Major_Sum;

            long UniqueValue = Minor_Sum += Major_Sum;

            if (UniqueValue < 0)
                UniqueValue *= -1;
            else if (UniqueValue == 0)
                UniqueValue += 1;

            //0x04 STAGE Complete. Db: 4, Total: 512

            string[] S3 = new string[100];

            for (int i = 0; i < S3.Length; i++)
                S3[i] = nm0x0SHA384[i][0].ToString() + hw0x4[i][0].ToString() + nm0x1SHA512[i][0].ToString();

            string[] hw0x5 = new string[S3.Length];

            for (int i = 0; i < hw0x5.Length; i++)
                hw0x5[i] = GetSHA256(S3[i]);

            string[] hw0x6 = new string[300];

            string[] hw0x6_0 = new string[100];
            string[] hw0x6_1 = new string[100];
            string[] hw0x6_2 = new string[100];

            for (int i = 0; i < hw0x6_0.Length; i++)
                hw0x6_0[i] = hw0x5[i][0].ToString() + hw0x5[i][1].ToString() + hw0x5[i][2].ToString();

            for (int i = 0; i < hw0x6_1.Length; i++)
                hw0x6_1[i] = hw0x5[i][3].ToString() + hw0x5[i][4].ToString() + hw0x5[i][5].ToString();

            for (int i = 0; i < hw0x6_2.Length; i++)
                hw0x6_2[i] = hw0x5[i][6].ToString() + hw0x5[i][7].ToString() + hw0x5[i][8].ToString();

            string[] hw0x6S = Unify(hw0x6_0, hw0x6_1);

            hw0x6 = Unify(hw0x6S, hw0x6_2);

            //0x05 STAGE Complete. Db: 5, Total: 912

            string[] hw0x7 = new string[hw0x4.Length + hw0x5.Length + hw0x6.Length];

            Array.Copy(hw0x4, 0, hw0x7, 0, hw0x4.Length);
            Array.Copy(hw0x5, 0, hw0x7, hw0x4.Length, hw0x5.Length);
            Array.Copy(hw0x6, 0, hw0x7, hw0x5.Length, hw0x6.Length);

            //0x06 STAGE Complete. Db: 3, Total: 912

            string[] nm0x0SHA384U = AddToMassEnd(nm0x0SHA384, UniqueValue.ToString());
            string[] nm0x1SHA512U = AddToMassEnd(nm0x1SHA512, UniqueValue.ToString());

            nm0x0SHA384U = WordsForHash384(nm0x0SHA384U);
            nm0x1SHA512U = WordsForHash512(nm0x1SHA512U);

            //0x07 STAGE Complete. Db: 5, Total: 1168

            string[] nm0x0SHA384UP = UnifyElements(nm0x0SHA384, nm0x0SHA384U);
            string[] nm0x1SHA512UP = UnifyElements(nm0x1SHA512, nm0x1SHA512U);

            nm0x0SHA384UP = WordsForHash256(nm0x0SHA384UP);
            nm0x1SHA512UP = WordsForHash256(nm0x1SHA512UP);

            //0x08 STAGE Complete. Db: 7, Total: 1424

            string[] SHAX1 = Unify3(nm0x0SHA384, nm0x0SHA384U, nm0x0SHA384UP);
            string[] SHAX2 = Unify3(nm0x1SHA512, nm0x1SHA512U, nm0x1SHA512UP);

            string[] SHAX1P2 = UnifyElements(SHAX1, SHAX2);

            SHAX1P2 = WordsForHash256(SHAX1P2);

            string[] SHAX = Unify3(SHAX1, SHAX2, SHAX1P2);
            
            //0x09 STAGE Complete. Db: 2, Total: 1811

            SHAX = WordsForHash512(SHAX);
            SHAX = WordsForHash384(SHAX);
            SHAX = WordsForHash256(SHAX);

            //0x10 STAGE Complete. Db: 2, Total: 1811

            string[][] SHAX16W = new string[SHAX.Length][];

            for (int i = 0; i < SHAX16W.Length; i++)
                SHAX16W[i] = GetHash16Words(SHAX[i]);

            for (int i = 0; i < SHAX16W.Length; i++)
            {
                for (int w = 0; w < 16; w++)
                {
                    SHAX16W[i][w] = GetSHA384(SHAX16W[i][w]);
                    SHAX16W[i][w] = GetSHA512(SHAX16W[i][w]);
                    SHAX16W[i][w] = GetSHA256(SHAX16W[i][w]);
                }
            }

            string[] SHAXX = Unify(SHAX, GetOneMass(SHAX16W));

            SHAXX = WordsForHash384(SHAXX);
            SHAXX = WordsForHash512(SHAXX);
            SHAXX = WordsForHash256(SHAXX);
        }

        public static string GetSHA256(string input)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(input);
            SHA256Managed hashstring = new SHA256Managed();
            byte[] hash = hashstring.ComputeHash(bytes);
            string hashString = string.Empty;
            hashString = HashString(hash, hashString);
            return hashString;
        }

        public static string GetSHA512(string input)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(input);
            HMACSHA512 hashstring = new HMACSHA512();
            byte[] hash = hashstring.ComputeHash(bytes);
            string hashString = string.Empty;
            hashString = HashString(hash, hashString);
            return hashString;
        }

        public static string GetSHA384(string input)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(input);
            SHA384Managed hashstring = new SHA384Managed();
            byte[] hash = hashstring.ComputeHash(bytes);
            string hashString = string.Empty;
            hashString = HashString(hash, hashString);
            return hashString;
        }

        public static string HashString(byte[] hash, string hashString)
        {
            foreach (byte x in hash)
            {
                hashString += string.Format("{0:x2}", x);
            }
            return hashString;
        }

        public enum Polarities : uint
        {
            Minor,
            Major
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;

namespace BIGsort.Lib
{
    public class BigFiles
    {
        private readonly string _path;
        private readonly string _nameMain;
        private string _name1 = "B.txt";
        private string _name2 = "C.txt";

        public BigFiles(string path, string nameMain)
        {
            _path = path;
            _nameMain = nameMain;
        }

        public void CreatedInitialFile()
        {
            Random random = new Random();
            int max = int.MaxValue;
            using StreamWriter streamWriter = new StreamWriter(new FileStream(_path + _nameMain, FileMode.Create));
            for (int i = 0; i < 100_000_000; i++)
            {
                streamWriter.Write(random.Next(max));
                streamWriter.Write(" ");
            }

            //streamWriter.Write(" ");
        }

        private void WriteListToFile(List<int> list, int i)
        {
            using StreamWriter streamWriter =
                new StreamWriter(new FileStream(_path + @"SortedParts\" + i, FileMode.Create));
            foreach (int sortedNumber in list)
            {
                streamWriter.Write(sortedNumber + ",");
            }
        }

        private void SplitToSortedFiles()
        {
            long maxRamNumber = 100_000;
            List<int> ints = new List<int>();
            using StreamReader streamReader = new StreamReader(new FileStream(_path + _nameMain, FileMode.Open));
            int i = 0;
            long numbers = 0;
            String number = "";
            char c1 = (char) streamReader.Read();
            while (!char.IsWhiteSpace(c1))
            {
                while (c1 != ',' && c1 != ' ')
                {
                    number += c1;
                    c1 = (char) streamReader.Read();
                }

                if (c1 == ',')
                {
                    c1 = (char) streamReader.Read();
                }

                ints.Add(int.Parse(number));
                numbers++;
                number = "";
                if (numbers > maxRamNumber)
                {
                    numbers = 0;
                    ints.Sort();
                    WriteListToFile(ints, i);
                    ints = new List<int>();
                    i++;
                }
            }

            ints.Sort();
            WriteListToFile(ints, i);
        }

        private void UniteAuxiliaryFiles()
        {
            String[] files = Directory.GetFiles(_path + @"SortedParts\");

            using (StreamWriter streamWriter =
                new StreamWriter(new FileStream(_path + _nameMain, FileMode.Create)))
            {
                foreach (var t in files)
                {
                    using StreamReader streamReader = new StreamReader(new FileStream(t, FileMode.Open));
                    streamWriter.Write(streamReader.ReadLine());
                }

                streamWriter.Write(" ");
            }
        }

        private void DivideMainFile()
        {
            using StreamReader streamReader = new StreamReader(new FileStream(_path + _nameMain, FileMode.Open));
            using StreamWriter streamWriter1 =
                new StreamWriter(new FileStream(_path + _name1, FileMode.Create));
            using StreamWriter streamWriter2 =
                new StreamWriter(new FileStream(_path + _name2, FileMode.Create));
            char c = (char) streamReader.Read();
            int i = 0;
            String number = "";
            int previousNumber = int.MinValue;
            while (!char.IsWhiteSpace(c))
            {
                if (c != ',')
                {
                    number += c;
                }
                else
                {
                    if (int.Parse(number) < previousNumber)
                    {
                        i++;
                    }

                    if (i % 2 == 1)
                    {
                        streamWriter1.Write(number);
                        streamWriter1.Write(',');
                    }
                    else
                    {
                        streamWriter2.Write(number);
                        streamWriter2.Write(',');
                    }


                    previousNumber = int.Parse(number);
                    number = "";
                }

                c = (char) streamReader.Read();
            }

            streamWriter1.Write(" ");
            streamWriter2.Write(" ");
        }

        private bool UniteFilesBack(bool finished)
        {
            using StreamWriter streamWriter = new StreamWriter(new FileStream(_path + _nameMain, FileMode.Create));
            using StreamReader streamReader1 = new StreamReader(new FileStream(_path + _name1, FileMode.Open));
            using StreamReader streamReader2 =
                new StreamReader(new FileStream(_path + _name2, FileMode.Open));
            bool n1formed = false;
            bool n2formed = false;
            char char1 = (char) streamReader1.Read();
            char char2 = (char) streamReader2.Read();
            if (char1 == ' ' || char2 == ' ')
            {
                finished = true;
            }

            String number1 = "";
            String number2 = "";
            while (char1 != ' ' || char2 != ' ')
            {
                if (!n1formed)
                {
                    if (char1 != ' ')
                    {
                        number1 += char1;
                        char1 = (char) streamReader1.Read();
                    }
                    else
                    {
                        n1formed = true;
                    }
                }

                if (!n2formed)
                {
                    if (char2 != ' ')
                    {
                        number2 += char2;
                        char2 = (char) streamReader2.Read();
                    }
                    else
                    {
                        n2formed = true;
                    }
                }

                if (char1 == ',')
                {
                    char1 = (char) streamReader1.Read();
                    n1formed = true;
                }

                if (char2 == ',')
                {
                    char2 = (char) streamReader2.Read();
                    n2formed = true;
                }

                if (n1formed && n2formed)
                {
                    if (number1 == "")
                    {
                        streamWriter.Write(number2);
                        streamWriter.Write(',');
                        number2 = "";
                        n2formed = false;
                    }
                    else if (number2 == "")
                    {
                        streamWriter.Write(number1);
                        streamWriter.Write(',');
                        number1 = "";
                        n1formed = false;
                    }
                    else
                    {
                        if (int.Parse(number1) <= int.Parse(number2))
                        {
                            streamWriter.Write(number1);
                            streamWriter.Write(',');
                            number1 = "";
                            n1formed = false;
                        }
                        else
                        {
                            streamWriter.Write(number2);
                            streamWriter.Write(',');
                            number2 = "";
                            n2formed = false;
                        }
                    }
                }
            }

            if (number1 != "")
            {
                streamWriter.Write(number1);
                streamWriter.Write(',');
            }

            if (number2 != "")
            {
                streamWriter.Write(number2);
                streamWriter.Write(',');
            }

            streamWriter.Write(" ");

            return finished;
        }

        public void NaturalSort()
        {
            SplitToSortedFiles();
            
            UniteAuxiliaryFiles();

            bool finished = false;
            while (!finished)
            {
                DivideMainFile();
                finished = UniteFilesBack(finished);
            }
        }
    }
}
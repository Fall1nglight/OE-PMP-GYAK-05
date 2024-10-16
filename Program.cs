﻿namespace OE_PMP_GYAK_05
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // 1. feladat
            #region
            Console.WriteLine("1. feladat");

            List<String> coloremIpsum = new List<String>(File.ReadAllLines("colorem_ipsum.txt"));

            foreach (string line in coloremIpsum)
            {
                string[] tmpArr = line.Split('#');
                string color = tmpArr[0];
                string text = tmpArr[1];

                ConsoleColor tmpColor;
                bool isValidColor = Enum.TryParse(color, true, out tmpColor);

                // ha sikeres volt az átalakítás, beállítjuk a kapott értéket
                // ellenkező esetben egy alapértelmezett (fehér) színre állítjuk
                Console.ForegroundColor = isValidColor ? tmpColor : ConsoleColor.White;

                Console.ForegroundColor = tmpColor;
                Console.WriteLine(text);

                // alternatív megoldás:
                // ConsoleColor tmpColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), color);
            }

            Console.WriteLine();
            #endregion

            // 2. feladat
            #region
            Console.WriteLine("2. feladat");

            DateTime dateTime = DateTime.Now;
            Random rand = new Random();

            string userInput = "y";
            while (userInput.CompareTo("y") == 0 && userInput.CompareTo("n") != 0)
            {
                string date = dateTime.GetDateTimeFormats().First();
                string numbers = string.Empty;
                HashSet<int> randInt = new HashSet<int>();

                while (randInt.Count != 5)
                    randInt.Add(rand.Next(1, 91));

                foreach (int n in randInt)
                    numbers += $"{n} ";

                Console.WriteLine($"On {date} numbers were: {numbers}");
                File.WriteAllText(
                    $"{date}.txt",
                    $"On {date} numbers were: {numbers}",
                    System.Text.Encoding.UTF8
                );

                Console.Write("Another week? [y/n] => ");
                userInput = Console.ReadLine()!;
                dateTime = dateTime.AddDays(7);
            }

            Console.WriteLine("");
            #endregion

            // 3. feladat
            #region
            Console.WriteLine("\n3. feladat");

            List<string> instructions = new List<string>(
                File.ReadAllText("ant_instructions.txt").Trim().Split('\n')
            );

            int x = 0;
            int y = 0;
            int direction = 0;

            // első sor feldolgozása külön
            string[] details = instructions.First().Split(' ');
            x = int.Parse(details[0]);
            y = int.Parse(details[1]);
            direction = int.Parse(details[2]);

            Console.WriteLine($"Kezdeti pozíció: (x: {x}, y: {y}), irány: {direction}°");

            // további sorok feldolgozása
            for (int i = 1; i < instructions.Count; i++)
                HandleCommand(instructions.ElementAt(i));

            Console.WriteLine($"Végső pozíció: (x: {x}, y: {y}), irány: {direction}°");

            void HandleCommand(string inst)
            {
                string[] parts = inst.Split(' ');
                string type = parts[0];
                int value = int.Parse(parts[1]);

                switch (type)
                {
                    case "go":
                    {
                        Move();
                        break;
                    }

                    case "right":
                    {
                        direction = (direction + value) % 360;
                        break;
                    }

                    case "left":
                    {
                        direction = (360 + (direction - value)) % 360;
                        break;
                    }
                }

                void Move()
                {
                    switch (direction)
                    {
                        // 0° -> y-tengely +
                        case 0:
                        {
                            y += value;
                            break;
                        }

                        // 360° -> y-tengely +
                        case 360:
                        {
                            y += value;
                            break;
                        }

                        // 90° -> x-tengely +
                        case 90:
                        {
                            x += value;
                            break;
                        }

                        // 180° -> y-tengely -
                        case 180:
                        {
                            y -= value;
                            break;
                        }

                        // 270° -> x-tengely -
                        case 270:
                        {
                            x -= value;
                            break;
                        }
                    }
                }
            }

            #endregion

            // 4. feladat
            #region
            Console.WriteLine("4. feladat");

            string[] nhanesArr = File.ReadAllText("NHANES_1999-2018.csv").Trim().Split('\n');
            int[] SEQN = new int[nhanesArr.Length - 1];
            string[] SURVEY = new string[nhanesArr.Length - 1];
            double[] RIAGENDR = new double[nhanesArr.Length - 1];
            double[] RIDAGEYR = new double[nhanesArr.Length - 1];
            double[] BMXBMI = new double[nhanesArr.Length - 1];
            double[] LBDGLUSI = new double[nhanesArr.Length - 1];

            for (int i = 1; i < nhanesArr.Length; i++)
            {
                int idx = i - 1;
                string[] nhanesDetails = nhanesArr[i].Split(',');
                SEQN[idx] = int.Parse(nhanesDetails[0]);
                SURVEY[idx] = nhanesDetails[1];
                RIAGENDR[idx] = double.Parse(nhanesDetails[2].Replace('.', ','));
                RIDAGEYR[idx] = double.Parse(nhanesDetails[3].Replace('.', ','));
                BMXBMI[idx] = double.Parse(nhanesDetails[4].Replace('.', ','));
                LBDGLUSI[idx] = double.Parse(nhanesDetails[5].Replace('.', ','));
            }

            string choosenSurvey = SURVEY[0];

            double sumOfMaleBMI = 0;
            int numOfMales = 0;
            double sumOfFemaleBMI = 0;
            int numOfFemales = 0;

            for (int i = 0; i < BMXBMI.Length; i++)
            {
                if (SURVEY[i].CompareTo(choosenSurvey) == 0)
                {
                    if (RIAGENDR[i] == 1)
                    {
                        sumOfMaleBMI += BMXBMI[i];
                        numOfMales++;
                    }

                    if (RIAGENDR[i] == 2)
                    {
                        sumOfFemaleBMI += BMXBMI[i];
                        numOfFemales++;
                    }
                }
            }

            double avgMaleBMI = (double)sumOfMaleBMI / numOfMales;
            double avgFemaleBMI = (double)sumOfFemaleBMI / numOfFemales;

            Console.WriteLine(
                "\t1. Egy adott felmérésben mennyi volt a nők és a férfiak átlagos testtömegindexe?"
            );
            Console.WriteLine(
                $"\tA {choosenSurvey} felmérésben a férfiak átlagos BMI-je: {avgMaleBMI}, a nőké pedig: {avgFemaleBMI}"
            );

            int numOfPeople = 0;
            int numOfHighBloodSugarPeople = 0;

            for (int i = 0; i < LBDGLUSI.Length; i++)
            {
                if (SURVEY[i].CompareTo(choosenSurvey) == 0)
                {
                    numOfPeople++;

                    if (LBDGLUSI[i] > 5.6)
                        numOfHighBloodSugarPeople++;
                }
            }

            double percentage = (double)numOfHighBloodSugarPeople / numOfPeople * 100;

            Console.WriteLine(
                "\t2. Egy adott felmérésben az alanyok hány százalékának volt 5.6-nál magasabb a vércukorszintje?"
            );
            Console.WriteLine(
                $"\tA {choosenSurvey} felmérésben az alanyok {Math.Round(percentage, 2)}%-a magas vércukorszinttel rendelkezik."
            );

            double maxBMI = 0.0;
            int maxBMIIdx = 0;

            for (int i = 0; i < BMXBMI.Length; i++)
            {
                if (SURVEY[i].CompareTo(choosenSurvey) == 0)
                {
                    if (BMXBMI[i] > maxBMI)
                    {
                        maxBMI = BMXBMI[i];
                        maxBMIIdx = i;
                    }
                }
            }

            Console.WriteLine(
                "\tEgy maximális BMI-vel rendelkező alanynak mennyi a vércukorszintje?"
            );
            Console.WriteLine(
                $"\tA {choosenSurvey} felmérésben a legmagasabb BMI: {maxBMI}, a hozzá tartozó vércukorszint: {LBDGLUSI[maxBMIIdx]}"
            );

            int numOfFatPeople = 0;
            int sumOfAge = 0;

            for (int i = 0; i < BMXBMI.Length; i++)
            {
                if (BMXBMI[i] > 30)
                {
                    numOfFatPeople++;
                    sumOfAge += (int)RIDAGEYR[i];
                }
            }

            double avgAge = (double)sumOfAge / numOfFatPeople;

            Console.WriteLine(
                "\tA teljes adathalmazban mi a túlsúlyos (legalább 30.0-as BMI) személyek átlagos életkora?"
            );
            Console.WriteLine(
                $"\tA teljes adathalmazban a túlsúlyos személyek átlagos életkora: {Math.Round(avgAge, 2)} év."
            );

            #endregion
        }
    }
}

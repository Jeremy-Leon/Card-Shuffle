using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

/* 
 * This program is a card shuffling simulation to show the amount of time/itterations it takes to achieve the same card shuffle twice given n cards.
 *
 * The program will prompt the user to select how many cards they would like to shuffle (1 to 52 cards).
 * The program will then shuffle the cards and add the shuffle to a dictionary.
 * The program will continue until it finds the same shuffle twice in the dictionary,
 * or until the system runs out of memory.
 * 
 * If the program does find a duplicate shuffle the program will end with stats on how many itterations and how much time it took.
 */
namespace Card_Shuffle
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<Key,int> shuffles = new Dictionary<Key, int>();
            Random rng = new Random();

            // Notation: "CoS" where C = card value and S = suit. e.g. Ace of spades = AoS
            string[] cards = new string[]
                {
                    "AoC", "AoD", "AoH", "AoS", // Aces
                    "2oC", "2oD", "2oH", "2oS", // Twos
                    "3oC", "3oD", "3oH", "3oS", // Threes
                    "4oC", "4oD", "4oH", "4oS", // Fours
                    "5oC", "5oD", "5oH", "5oS", // Fives
                    "6oC", "6oD", "6oH", "6oS", // Sixs
                    "7oC", "7oD", "7oH", "7oS", // Sevens
                    "8oC", "8oD", "8oH", "8oS", // Eights
                    "9oC", "9oD", "9oH", "9oS", // Nines
                    "10oC","10oD","10oH","10oS", // Tens
                    "JoC", "JoD", "JoH", "JoD", // Jacks
                    "QoC", "QoD", "QoH", "QoS", // Queens
                    "KoC", "KoD", "KoH", "KoS", // Kings
                };

            Console.WriteLine("How many cards would you like to shuffle? (1 - 52)");
            try
            {
                int numCards = Int32.Parse(Console.ReadLine());
                if (numCards > 52)
                {
                    Console.WriteLine("Invalid number entered");
                    return;
                }
                Array.Resize(ref cards, numCards); // Resize card array based off user input
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid number entered");
                return;
            }

            int i = 0;
            Key a = new Key(shuffleCards(cards,rng));
            var elapsed = TimeAction(() =>
            {
                for (i = 0; !shuffles.ContainsKey(a); i++)
                {
                    shuffles.Add(a, i);
                    a = new Key(shuffleCards(cards,rng));
                    printProgress(i, 100000);
                }
            });

            Console.WriteLine("\nYou found a mathching shuffle!");
            Console.WriteLine("Itterations: {0}", i);
            Console.WriteLine("Time elapsed: {0} (hours/minutes/seconds)", elapsed);
        }

        // Shuffle cards by randomizing the array (Thanks to https://stackoverflow.com/a/4262134)
        public static string shuffleCards(string[] cards, Random rng)
        {
            string[] shuffeledCards = cards.OrderBy(a => rng.Next()).ToArray();
            return string.Join("", shuffeledCards); // Return string because Key class is set up for strings
        }

        // Function to time a block of code (Thanks to https://stackoverflow.com/a/7726435)
        public static TimeSpan TimeAction(Action blockingAction)
        {
            Stopwatch stopWatch = System.Diagnostics.Stopwatch.StartNew();
            blockingAction();
            stopWatch.Stop();
            return stopWatch.Elapsed;
        }

        // Print a dot every x itterations to show progress
        static void printProgress(int itteration, int interval)
        {
            if(itteration == 0)
            {
                Console.Write("Shuffling");
            }
            else if (itteration % interval == 0)
            {
                Console.Write(".");
            }
        }

        // Override GetHashCode and Equals so that .ContainsKey()
        // compares values and not references (Thanks to https://stackoverflow.com/a/3014962)
        public class Key
        {
            string name;
            public Key(string n) { name = n; }

            public override int GetHashCode()
            {
                if (name == null) return 0;
                return name.GetHashCode();
            }

            public override bool Equals(object obj)
            {
                Key other = obj as Key;
                return other != null && other.name == this.name;
            }
        } 
    }
}

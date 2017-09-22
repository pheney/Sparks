using Sparks.Models.DataModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Sparks.Models.WorkModel
{
    public static class Utilities
    {
        public static Random random = new Random();

        public static T SelectRandom<T>(this DbSet<T> source, int? seed = null) where T : BaseModel
        {
            seed = seed ?? Utilities.random.Next();
            return source.Where(x=>!x.Name.Equals("--")).OrderBy(s => s.ID ^ seed).First();
        }

        public static T SelectRandom<T>(this IQueryable<T> source)
        {
            int index = Utilities.random.Next(source.Count());
            return source.OrderBy(x=>true).Skip(index).First();
        }

        public static T SelectRandom<T>(this List<T> source)
        {
            return source[random.Next(source.Count)];
        }
        
        public static T SelectRandom<T>(this T[] source)
        {
            return source[random.Next(source.Length)];
        }

        public static bool RandomBool ()
        {
            return random.Next(2) == 0;
        }

        public static bool OneChanceIn(int number)
        {
            if (number <= 1) return true;
            return random.Next(number) == 0;
        }

        private static string vowels = "aeiou";
        public static string ToPrep (this string source)
        {
            if (vowels.IndexOf(source.ToLower()[0]) == -1)
            {
                return "a";
            } else
            {
                return "an";
            }
        }
    }
}
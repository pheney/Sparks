using Sparks.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web;

namespace Sparks.Models.WorkModel
{
    /// <summary>
    /// Probably should have been called 'Mimic' because it uses statistical modelling to mimic
    /// language and word construction. A "language" is defined as follows:
    ///     1) the exponent describing the probability curve
    ///     2) the seed for a random number generator
    /// 
    /// Using Zipf's law, the following data is created:
    ///     1) first letter probabilities,
    ///     2) letter pair probabilities
    ///
    /// From this, any number of words and sentences can be generated which adhere to the 
    /// implicit linguistic 'rules' of the "language"
    /// </summary>
    public static class Polyglot
    {
        public enum WordType { Normal, Easy, Vowel };
        private static List<string> VOWELS = new List<string>() { "a", "e", "i", "o", "u" };
        private static float[] lettersPb = new float[26];
        private static float[] letterComboPb = new float[26 * 26];

        #region Data
        //////////////
        //  data    //
        //////////////
        
        private partial class PolyglotDbContext : DbContext
        {
            //  constructor
            public PolyglotDbContext() : base("name=JabberwockDb")
            {
                Database.SetInitializer<PolyglotDbContext>(null);
            }

            protected override void OnModelCreating(DbModelBuilder modelBuilder)
            {
                modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            }

            public virtual DbSet<WordLengthPb> WordLength { get; set; }
            public virtual DbSet<StartLetterPb> FirstLetter { get; set; }
            public virtual DbSet<VSecondLetter> SecondLetter { get; set; }
            public virtual DbSet<VNextLetter> NextLetter { get; set; }
            public virtual DbSet<Dialect> Language { get; set; }
        }

        [Table("WordLengthPb")]
        private class WordLengthPb
        {
            [Key]
            public int ID { get; set; }
            public int Length { get; set; }        
            public decimal Weight { get; set; }
        }

        [Table("StartLetterPb")]
        private class StartLetterPb
        {
            [Key]
            public int ID { get; set; }            
            public string Letter { get; set; }            
            public decimal Weight { get; set; }
        }

        [Table("vSecondLetter")]
        private class VSecondLetter
        {
            [Key]
            public int ID { get; set; }            
            public string StartLetter { get; set; }            
            public string SecondLetter { get; set; }            
            public decimal SecondLetterPb { get; set; }
        }
        
        [Table("vNextLetter")]
        private class VNextLetter
        {
            [Key]
            public int ID { get; set; }            
            public string PrevLetter { get; set; }            
            public string NextLetter { get; set; }            
            public decimal NextLetterPb { get; set; }
        }

        [Table("Dialect")]
        private class Dialect
        {
            [Key]
            public int ID { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public int Seed { get; set; }
            public decimal Power { get; set; }
            public int Weight { get; set; }
        }

        #endregion
        #region Data Extensions
        /////////////////////////
        //  Data Extensions    //
        /////////////////////////

        private static string GetWeightedRandom (this List<StartLetterPb> source)
        {
            decimal max = source.Sum(n => n.Weight);
            decimal r = (Decimal)Utilities.random.NextDouble() * max;
            foreach (var l in source)
            {
                r -= l.Weight;
                if (r <= 0) return l.Letter;
            }
            return "*";
        }

        private static string GetWeightedRandom (this List<VSecondLetter> source)
        {
            decimal max = source.Sum(n => n.SecondLetterPb);
            decimal r = (Decimal)Utilities.random.NextDouble() * max;
            foreach (var l in source)
            {
                r -= l.SecondLetterPb;
                if (r <= 0) return l.SecondLetter;
            }
            return "*";
        }

        private static string GetWeightedRandom (this List<VNextLetter> source)
        {
            decimal max = source.Sum(n => n.NextLetterPb);
            decimal r = (Decimal)Utilities.random.NextDouble() * max;
            foreach (var l in source)
            {
                r -= l.NextLetterPb;
                if (r <= 0) return l.NextLetter;
            }
            return "*";
        }

        private static int GetWeightedRandom (this List<WordLengthPb> source)
        {
            decimal max = source.Sum(n => n.Weight);
            decimal r = (Decimal)Utilities.random.NextDouble() * max;
            foreach (var l in source)
            {
                r -= l.Weight;
                if (r <= 0) return l.Length;
            }
            return '*';
        }

        #endregion
        #region Utilities
        ///////////////////
        //  Utilities    //
        ///////////////////

        private static string AnyFirstLetter (int languageID)
        {
            var db = new PolyglotDbContext();
            var letters = db.FirstLetter.OrderBy(n => n.ID).ToList();

            //  When there is a valid languageID
            //  overwrite the default American-English letter probabilities with data
            //  generated based on the provided language
            if (languageID > 0)
            {
                var language = db.Language.Where(n => n.ID == languageID).First();
                var languageSeed = language.Seed;
                var power = (double)language.Power;

                //  assign probabilities based on the language
                Random r = new Random(languageSeed);
                foreach (var letter in letters)
                {
                    letter.Weight = (decimal)Math.Pow(r.NextDouble(), power);
                }
            }

            //  return a random letter based on the new probabilitiy
            return letters.GetWeightedRandom();
        }

        private static string AnySecondLetter (string firstLetter, int languageID)
        {
            var db = new PolyglotDbContext();
            var letters = db.SecondLetter.Where(n => n.StartLetter.Equals(firstLetter)).OrderBy(n => n.ID).ToList();

            //  When there is a valid languageID
            //  overwrite the default American-English letter probabilities with data
            //  generated based on the provided language
            if (languageID > 0)
            {
                var language = db.Language.Where(n => n.ID == languageID).First();
                var languageSeed = language.Seed;
                var power = (double)language.Power;

                //  assign probabilities based on the language
                //  * probabilities must be different than the 'first letter' Pb
                //  * probabilities must be different for each firstLetter parameter
                int offset = char.ToUpper(firstLetter.ToCharArray()[0]) - 'A';
                Random r = new Random(languageSeed + 1 + offset);
                foreach (var pair in letters)
                {
                    pair.SecondLetterPb = (decimal)Math.Pow(r.NextDouble(), power);
                }
            }

            //  return a random second letter
            return letters.GetWeightedRandom();
        }

        private static string AnyNextLetter (string previousLetter, int languageID)
        {

            var db = new PolyglotDbContext();
            var letters = db.NextLetter.Where(n => n.PrevLetter.Equals(previousLetter)).OrderBy(n => n.ID).ToList();

            //  When there is a valid languageID
            //  overwrite the default American-English letter probabilities with data
            //  generated based on the provided language
            if (languageID > 0)
            {
                var language = db.Language.Where(n => n.ID == languageID).First();
                var languageSeed = language.Seed;
                var power = (double)language.Power;

                //  assign probabilities based on the language
                //  * probabilities must be different than the 'first letter' Pb
                //  * probabilities must be different for each firstLetter parameter
                int offset = char.ToUpper(previousLetter.ToCharArray()[0]) - 'A';
                Random r = new Random(languageSeed + 2 + offset);
                foreach (var pair in letters)
                {
                    pair.NextLetterPb = (decimal)Math.Pow(r.NextDouble(), power);
                }
            }

            //  return a random next letter
            return letters.GetWeightedRandom();
        }

        private static string AnyNextVowel(string previousLetter, int languageID)
        {
            var db = new PolyglotDbContext();
            var letters = db.NextLetter.Where(n => VOWELS.Contains(n.NextLetter)).OrderBy(n => n.ID).ToList();

            //  When there is a valid languageID
            //  overwrite the default American-English letter probabilities with data
            //  generated based on the provided language
            if (languageID > 0)
            {
                var language = db.Language.Where(n => n.ID == languageID).First();
                var languageSeed = language.Seed;
                var power = (double)language.Power;

                //  assign probabilities based on the language
                //  * probabilities must be different than the 'first letter' Pb
                //  * probabilities must be different for each firstLetter parameter
                int offset = char.ToUpper(previousLetter.ToCharArray()[0]) - 'A';
                Random r = new Random(languageSeed + 3 + offset);
                foreach (var pair in letters)
                {
                    pair.NextLetterPb = (decimal)Math.Pow(r.NextDouble(), power);
                }
            }

            return letters.GetWeightedRandom();
        }

        private static int AnyLength ()
        {
            var db = new PolyglotDbContext();
            var wl = db.WordLength;
            var ll = wl.ToList();
            var wr = ll.GetWeightedRandom();
            return db.WordLength.ToList().GetWeightedRandom();
        }

        #endregion
        #region Public Utilities
        //////////////////////////
        //  Public Utilities    //
        //////////////////////////

        public static string NormalWord (string startingLetter, int length, int languageID = 1)
        {
            string word = startingLetter.ToString();

            while (word.Length < length +2)
            {
                switch (word.Length)
                {
                    case 1: //  second letter
                        word += AnySecondLetter(word.LastLetter(), languageID);
                        break;
                    default:
                        //  remaining letters
                        word += AnyNextLetter(word.LastLetter(), languageID);
                        break;
                }
            }
            return word;
        }

        public static string EasyWord (string startingLetter, int length, int languageID = 1)
        {
            string word = startingLetter;
            word += AnySecondLetter(startingLetter, languageID);
            string lastTwo = word;
            
            while (word.Length < length +3)
            {
                string c;
                if (lastTwo.HasVowels())
                {
                    c = AnyNextLetter(word.LastLetter(), languageID);
                }
                else
                {
                    c = AnyNextVowel(word.LastLetter(), languageID);
                }

                word += c;
                lastTwo = word.Substring(word.Length - 2);
            }
            return word;
        }

        public static string VowelWord (string startingLetter, int length, int languageID = 1)
        {
            string word = startingLetter;
            word += AnySecondLetter(startingLetter, languageID);
            int wordLength = length < 4 ? 4 : length > 8 ? length - 1 : length;
            while (word.Length < wordLength)
            {
                word += AnyNextVowel(word.LastLetter(), languageID);
            }

            return word;
        }

        public static List<VmGeneral> GetLanguages ()
        {
            var db = new PolyglotDbContext();
            List<VmGeneral> lang = new List<VmGeneral>();
            foreach (var L in db.Language.ToList())
            {
                //  Must explicitly pass the parameters instead of using 
                //  the view-model's constructor, because the constructor automatically
                //  converts all text to lower-case.
                VmGeneral language = new VmGeneral();
                language.ID = L.ID;
                language.Name = L.Name;
                language.Description = L.Description;
                lang.Add(language);
            }
            return lang;
        }

        #endregion
        #region General String Extensions
        //////////////////////////////////
        //  General String Extensions   //
        //////////////////////////////////

        //  general string extension
        public static string LastLetter(this string source)
        {
            switch (source.Length)
            {
                case 0: //  empty string
                    return "-";
                default:
                    return source.Substring(source.Length - 1, 1);
            }
        }

        //  general string extension
        public static string FirstLetter(this string source)
        {
            switch (source.Length)
            {
                case 0: //  empty string
                    return "-";
                default:
                    return source.Substring(0, 1);
            }
        }
        
        /// <summary>
        /// Capitalizes the first letter of a string.
        /// </summary>
        public static string ToProper(this string source)
        {
            if (source.Length == 0) return "";
            return source.FirstLetter().ToUpper() + source.Substring(1).ToLower();
        }

        /// <summary>
        /// Capitalizes every word in the string. "words"
        /// are defined as any character that follows a blank space.
        /// </summary>
        public static string ToTitle(this string source)
        {
            StringBuilder w = new StringBuilder();
            string[] words = source.Split(' ');
            int length = words.Length;

            for (int i = 0; i < length; i++)
            {
                w.Append(words[i].ToProper());
                w.Append(' ');
            }

            return w.ToString().TrimEnd(' ');
        }

        //  general string extension
        private static bool HasVowels(this string source)
        {
            foreach (char c in source.ToCharArray())
            {
                if (VOWELS.Contains(c.ToString().ToLower())) return true;
            }
            return false;
        }

        #endregion
        #region Public Methods
        //////////////////////
        //  Public Methods  //
        //////////////////////

        public static string AnyWord(int languageID = 1)
        {
            return AnyWord(AnyLength(), (WordType)Utilities.random.Next(WordType.GetValues(typeof(WordType)).Length), languageID);
        }

        public static string AnyWord(WordType wordType, int languageID)
        {
            return AnyWord(AnyLength(), wordType, languageID);
        }

        public static string AnyWord(string startingLetter, WordType wordType = WordType.Normal, int languageID = 1)
        {
            return AnyWord(startingLetter, AnyLength(), wordType, languageID);
        }

        public static string AnyWord(int length, WordType wordType = WordType.Normal, int languageID = 1)
        {
            return AnyWord(AnyFirstLetter(languageID), length, wordType, languageID);
        }

        public static string AnyWord(string startingLetter, int length, WordType wordType = WordType.Normal, int languageID = 1)
        {
            string word = "";

            switch (wordType)
            {
                case WordType.Vowel:
                    //  Alien words have mostly vowels
                    word = VowelWord(startingLetter, length, languageID);
                    break;
                case WordType.Easy:
                    //  Prevents words from having more than 2 consonants in a row
                    word = EasyWord(startingLetter, length, languageID);
                    break;
                case WordType.Normal:
                default:
                    word = NormalWord(startingLetter, length, languageID);
                    break;
            }
            return word.ToProper();
        }

        #endregion

    }
}
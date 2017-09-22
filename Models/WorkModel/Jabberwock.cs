using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace Sparks.Models.WorkModel
{
    /// <summary>
    /// Uses letter and letter-pair probabilities from the English language to build jibberish
    /// words that follow the implicit language rules of English.
    /// 
    /// There are a few options that can be passed that for the jibberish to adhere to additional
    /// rules.
    ///     1) "Jabber" -- this is the default generator
    ///     2) "Song" -- this guarantees there are no 3-letter sequences of all consonants
    ///     3) "Alien" -- this generates only vowels for the 2nd-nth letter
    /// </summary>
    public static class Jabberwock 
    {
        private static List<string> VOWELS = new List<string>() { "a", "e", "i", "o", "u" };
        public enum Language { Jabber, Song, Alien };

        #region Data
        //////////////
        //  data    //
        //////////////
        
        private partial class JabberwockDbContext : DbContext
        {
            //  constructor
            public JabberwockDbContext() : base("name=JabberwockDb")
            {
                Database.SetInitializer<JabberwockDbContext>(null);
            }

            protected override void OnModelCreating(DbModelBuilder modelBuilder)
            {
                modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            }

            public virtual DbSet<WordLengthPb> WordLength { get; set; }
            public virtual DbSet<StartLetterPb> FirstLetter { get; set; }
            public virtual DbSet<VSecondLetter> SecondLetter { get; set; }
            public virtual DbSet<VNextLetter> NextLetter { get; set; }
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

        #endregion
        #region Data Extensions

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

        private static string AnyFirstLetter()
        {
            var db = new JabberwockDbContext();
            var letters = db.FirstLetter.ToList();
            return letters.GetWeightedRandom();
        }

        private static string AnySecondLetter (string firstLetter)
        {
            var db = new JabberwockDbContext();
            var letters = db.SecondLetter.Where(n => n.StartLetter.Equals(firstLetter)).ToList();
            return letters.GetWeightedRandom();
        }

        private static string AnyNextLetter (string previousLetter)
        {
            var db = new JabberwockDbContext();
            var letters = db.NextLetter.Where(n => n.PrevLetter.Equals(previousLetter)).ToList();
            return letters.GetWeightedRandom();
        }

        private static string AnyNextVowel (string previousLetter)
        {
            var db = new JabberwockDbContext();
            var letters = db.NextLetter.Where(n => VOWELS.Contains(n.NextLetter)).ToList();
            return letters.GetWeightedRandom();
        }

        private static int AnyLength ()
        {
            var db = new JabberwockDbContext();
            var wl = db.WordLength;
            var ll = wl.ToList();
            var wr = ll.GetWeightedRandom();
            return wr;
            //return db.WordLength.ToList().GetWeightedRandom();
        }
        
        public static string XLastLetter (this string source)
        {
            switch (source.Length)
            {
                case 0: //  empty string
                    return "-";
                default:
                    return source.Substring(source.Length - 1, 1);
            }
        }

        public static string XFirstLetter (this string source)
        {
            switch (source.Length)
            {
                case 0: //  empty string
                    return "-";
                default:
                    return source.Substring(0, 1);
            }
        }

        public static string XToProper(this string source)
        {
            if (source.Length == 0) return "";
            return source.FirstLetter().ToUpper() + source.Substring(1).ToLower();
        }

        public static string XToTitle(this string source)
        {
            string w = "";
            foreach(string s in source.Split(' '))
            {
                w += s.ToProper() + ' ';
            }

            return w.TrimEnd(' ');
        }

        private static bool HasVowels (this string source)
        {
            foreach (char c in source.ToCharArray())
            {
                if (VOWELS.Contains(c.ToString().ToLower())) return true;
            }
            return false;
        }

        private static string JabberWord (string startingLetter, int length)
        {
            string word = startingLetter.ToString();

            while (word.Length < length +2)
            {
                switch (word.Length)
                {
                    case 1: //  second letter
                        word += AnySecondLetter(word.LastLetter());
                        break;
                    default:
                        //  remaining letters
                        word += AnyNextLetter(word.LastLetter());
                        break;
                }
            }
            return word;
        }

        public static string SongWord (string startingLetter, int length)
        {
            string word = startingLetter;
            word += AnySecondLetter(startingLetter);
            string lastTwo = word;
            
            while (word.Length < length +3)
            {
                string c;
                if (lastTwo.HasVowels())
                {
                    c = AnyNextLetter(word.LastLetter());
                }
                else
                {
                    c = AnyNextVowel(word.LastLetter());
                }

                word += c;
                lastTwo = word.Substring(word.Length - 2);
            }
            return word;
        }

        public static string AlienWord (string startingLetter, int length)
        {
            string word = startingLetter;
            word += AnySecondLetter(startingLetter);
            int wordLength = length < 4 ? 4 : length > 8 ? length - 1 : length;
            while (word.Length < wordLength)
            {
                word += AnyNextVowel(word.LastLetter());
            }

            return word;
        }

        #endregion
        #region Public Methods
        //////////////////////
        //  Public Methods  //
        //////////////////////

        public static string AnyWord()
        {
            return AnyWord(AnyLength(), (Language)Utilities.random.Next(Language.GetValues(typeof(Language)).Length));
        }

        public static string AnyWord(Language dialect)
        {
            return AnyWord(AnyLength(), dialect);
        }

        public static string AnyWord(string startingLetter, Language dialect = Language.Jabber)
        {
            return AnyWord(startingLetter, AnyLength(), dialect);
        }

        public static string AnyWord(int length, Language dialect = Language.Jabber)
        {
            return AnyWord(AnyFirstLetter(), length, dialect);
        }

        public static string AnyWord(string startingLetter, int length, Language dialect = Language.Jabber)
        {
            string word = "";

            switch (dialect)
            {
                case Language.Alien:
                    //  Alien words have mostly vowels
                    word = AlienWord(startingLetter, length);
                    break;
                case Language.Song:
                    //  Prevents words from having more than 2 consonants in a row
                    word = SongWord(startingLetter, length);
                    break;
                case Language.Jabber:
                default:
                    word = JabberWord(startingLetter, length);
                    break;
            }
            return word.ToProper();
        }

        #endregion

    }
}
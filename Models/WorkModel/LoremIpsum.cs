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
    /// Uses letter and letter-pair probabilities to build lorem-ipsum style text  that 
    /// mimics the implicit language rules of LATIN. The probabilities are computed after 
    /// loading the entire work of Cicero's "De finibus bonorum et malorum" which is what
    /// actual Lorem Ipsum filler text is sourced from.
    /// 
    /// Can be used to select actual words from the text, based on their relative frequency
    /// of appearance (which follow's Zipf's law).
    /// 
    /// Can be used to randomly create words from letter and letter-pair probabilities computed
    /// from the latin source text (which also follow Zipf's law).
    /// 
    /// Adhoc rules:
    ///     1) no 3-letter consonant sequences
    ///     2) no 3-letter vowel sequences
    ///     3) no all consonant 2-letter words
    ///     4) no all vowel 2-letter words
    /// </summary>
    public class LoremIpsum
    {
        private static string vowels = "aeiou";

        /// <summary>
        /// Perform the analysis of the table data and store it for reference.
        /// </summary>
        #region Singleton definition
        //  constructor
        //  prevent instantiation outside of the class itself
        private LoremIpsum ()
        {
            var db = new LatinDbContext();
            Debug.WriteLine(this.GetHashCode() + " ---------------------- " + DateTime.Now.ToLocalTime());

            //  load ~75,000 words from Cicero's text "De finibus bonorum et malorum"
            var words = db.LatinWords.ToArray();
            Debug.WriteLine("{0}, Loaded {1} words to memory", this.GetHashCode(), words.Count());
                
            //  crate the data for word probability
            MakeWordPbs(words);
            Debug.WriteLine("{0}, Counted {1} unique words", this.GetHashCode(), wordPbs.Count());

            //  create the data for letter probability as a first letter
            MakeFirstLetterPbs(words);
            Debug.WriteLine("{0}, Found {1} unique letters", this.GetHashCode(), firstLetterPbs.Count());

            //  create the data for letter pair probability
            MakeLetterPairPbs(words);
            Debug.WriteLine("{0}, Found {1} unique pairs", this.GetHashCode(), letterPairPbs.Count());
        }

        //  store an instance of this object
        private static LoremIpsum instance;

        //  accessor for this object
        //  returns the only instance of this object
        public static LoremIpsum Instance
        {
            get
            {
                if (LoremIpsum.instance == null) LoremIpsum.instance = new LoremIpsum();
                return LoremIpsum.instance;
            }
        }
        
        #endregion             
        #region Analysis probability construction

        private List<WordPb> wordPbs;
        private void MakeWordPbs(LatinWord[] words)
        {
            wordPbs = new List<WordPb>();
            int totalWords = 0;
            foreach (var word in words)
            {
                bool match = false;
                foreach (var existing in wordPbs)
                {
                    if (word.Word.Equals(existing.Word))
                    {
                        existing.Pb++;
                        match = true;
                    }
                }
                if (!match)
                {
                    wordPbs.Add(new WordPb(word.Word));
                }
                totalWords++;
            }

            //  compute word probabilities
            foreach (var word in wordPbs)
            {
                word.Pb /= totalWords;
                word.Pb *= word.Pb;
            }
        }

        private List<FirstLetterPb> firstLetterPbs;
        private void MakeFirstLetterPbs (LatinWord[] words)
        {
            firstLetterPbs = new List<FirstLetterPb>();
            int totalLetterCount = 0;
            foreach (var sourceWord in words)
            {
                //  source letter
                char firstLetter = sourceWord.Word.Substring(0, 1).ToLower().ToCharArray()[0];
                
                //  check against existing letters
                bool match = false;
                foreach (var countedLetter in firstLetterPbs)
                {
                    if (countedLetter.Letter.Equals(firstLetter))
                    {
                        match = true;
                        countedLetter.Pb++;
                    }
                }

                //  add a new counted letter if there was no match
                if (!match)
                {
                    firstLetterPbs.Add(new FirstLetterPb(firstLetter));
                }

                //  count the number of letters analyzed
                totalLetterCount++;
            }

            //  compute the probabilities
            foreach (var letter in firstLetterPbs)
            {
                letter.Pb /= totalLetterCount;
                letter.Pb *= letter.Pb;
            }
        }

        private List<LetterPairPb> letterPairPbs;
        private void MakeLetterPairPbs (LatinWord[] words)
        {
            letterPairPbs = new List<LetterPairPb>();
            int totalPairCount = 0;

            foreach (var sourceWord in words)
            {
                //  get the letter pairs from the source word
                List<string> pairsFromWord = GetPairsFromWord(sourceWord.Word);

                //  itterate the pairs and add to the index
                foreach (string letterPair in pairsFromWord)
                {
                    bool match = false;
                    foreach (var countedPair in letterPairPbs)
                    {
                        if (letterPair.Equals(countedPair.Pair))
                        {
                            countedPair.Pb++;
                            match = true;
                        }
                    }
                    if (!match)
                    {
                        letterPairPbs.Add(new LetterPairPb(letterPair));
                    }
                    totalPairCount++;
                }
            }

            //  compute the final probabilities
            foreach (var pair in letterPairPbs)
            {
                pair.Pb /= totalPairCount;
                pair.Pb *= pair.Pb;
            }
        }

        //  Pb construction helper
        private List<string> GetPairsFromWord(string word)
        {
            List<string> results = new List<string>();

            if (word.Length < 2) return results;

            for (int i = word.Length-2; i > 0; i--)
            {
                string pair = word.Substring(i, 2).ToLower();
                results.Add(pair);
            }
            return results;
        }
          
        //  Pb data classes  
        private class FirstLetterPb
        {
            public char Letter;
            public double Pb;
            public FirstLetterPb(char letter)
            {
                Letter = letter;
                Pb = 1;
            }
        }

        //  Pb data classes
        private class LetterPairPb
        {
            public string Pair;
            public double Pb;
            public LetterPairPb (string pair)
            {
                Pair = pair;
                Pb = 1;
            }
        }

        //  Pb data classes
        private class WordPb
        {
            public string Word;
            public double Pb;
            public WordPb (string word)
            {
                Word = word;
                Pb = 1;
            }
        }

        #endregion
        #region Database access

        //  data context
        private partial class LatinDbContext : DbContext
        {
            //  constructor
            public LatinDbContext() : base("name=JabberwockDb")
            {
                Database.SetInitializer<LatinDbContext>(null);
            }

            protected override void OnModelCreating(DbModelBuilder modelBuilder)
            {
                modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            }

            public virtual DbSet<LatinWord> LatinWords { get; set; }
        }

        [Table("LatinWords")]
        private class LatinWord
        {
            [Key]
            public string Word { get; set; }
        }

        #endregion
        #region Random selection

        private static char SelectRandomByPb(List<FirstLetterPb> source)
        {
            double total = 0;
            foreach (var letter in source) total += letter.Pb;

            double choice = Utilities.random.NextDouble() * total;

            foreach (var letter in source)
            {
                choice -= letter.Pb;
                if (choice <= 0)
                {
                    return letter.Letter;
                }
            }
            return '?';
        }

        private static char SelectRandomByPb(List<LetterPairPb> source, char letter)
        {
            //  build a list of pairs that start with the provided letter
            List<LetterPairPb> pairs = new List<LetterPairPb>();
            foreach (var pair in source)
            {
                if (letter.Equals(pair.Pair.Substring(0, 1).ToCharArray()[0]))
                {
                    pairs.Add(pair);
                }
            }

            //  find the total weights of all the relevant pairs
            double maxPb = 0;
            foreach (var pair in pairs) maxPb += pair.Pb;

            //  randomly select the second letter from the list of pairs
            double choice = Utilities.random.NextDouble() * maxPb;
            foreach (var pair in pairs)
            {
                choice -= pair.Pb;
                if (choice <= 0)
                {
                    return pair.Pair.Substring(1, 1).ToCharArray()[0];
                }
            }
            return '*';
        }

        private static string SelectRandomByPb(List<WordPb> source)
        {
            double total = 0;
            foreach (var word in source) total += word.Pb;

            double choice = Utilities.random.NextDouble() * total;

            foreach (var word in source)
            {
                choice -= word.Pb;
                if (choice <= 0)
                {
                    return word.Word;
                }
            }
            return "!@#";
        }

        #endregion
        #region Methods for getting letters

        private static char AnyFirstLetter()
        {
            var a = LoremIpsum.Instance;
            return SelectRandomByPb(a.firstLetterPbs);
        }

        private static char AnyNextLetter(char letter)
        {
            var a = LoremIpsum.Instance;
            return SelectRandomByPb(a.letterPairPbs, letter);
        }

        private static bool IsVowel(string letter)
        {
            return vowels.Contains(letter.ToLower());
        }

        private static bool IsConsonant(string letter)
        {
            return !IsVowel(letter.ToLower());
        }

        private static bool IsVowelSequence (string letters, int length)
        {
            bool allVowels = true;
            foreach (var letter in letters.Substring(letters.Length - length).ToCharArray())
            {
                allVowels &= IsVowel(letter.ToString());
            }
            return allVowels;
        }

        private static bool IsConsonantSequence(string letters, int length)
        {
            bool allConsonant = true;
            foreach (var letter in letters.Substring(letters.Length - length).ToCharArray())
            {
                allConsonant &= IsConsonant(letter.ToString());
            }
            return allConsonant;
        }
        
        #endregion
        #region Public Methods

        //  return a random selected word from ciciro's work
        public static string AnyLatinWord ()
        {
            var a = LoremIpsum.Instance;
            return SelectRandomByPb(a.wordPbs);
        }

        //  return a newly created word using Pb's based on latin
        public static string AnyIpsumWord ()
        {
            int length = 1;
            double lengthPb = Math.Pow(Utilities.random.NextDouble(), 2) * 12; // 0-11
            length += (int)Math.Floor(lengthPb);

            string result = "";
            result += LoremIpsum.AnyFirstLetter().ToString();

            while (result.Length < length)
            {
                string candidateLetter = LoremIpsum.AnyNextLetter(result[result.Length - 1]).ToString();
                string candiateWord = result + candidateLetter;
                bool valid = true;

                //  ensure 2-letter words aren't all-consonant or all-vowel
                //  ensure 3+ letter words don't have triple-consonant or triple-vowel series
                int sequenceLength = candiateWord.Length == 2 ? 2 : 3;

                if (IsVowelSequence(candiateWord, sequenceLength)
                    || IsConsonantSequence(candiateWord, sequenceLength))
                {
                    valid = false;
                    Debug.WriteLine("Rejected sequence '" + candiateWord.Substring(candiateWord.Length - sequenceLength) + "'");
                }

                if (valid) result = candiateWord;
            }
            return result;
        }

        //  return a paragraph of the indicated length. Paragraph will
        //  be either ipsum lorem, or fake.
        public static string Paragraph (int wordCount)
        {
            StringBuilder result = new StringBuilder();

            if (Utilities.RandomBool())
            {
                for (var i = 0; i < wordCount; i++)
                {
                    result.Append(LoremIpsum.AnyLatinWord());
                    result.Append(' ');
                }
            }
            else
            {
                for (var i = 0; i < wordCount; i++)
                {
                    result.Append(LoremIpsum.AnyIpsumWord());
                    result.Append(' ');
                }
            }

            return result.ToString();
        }
        
        #endregion
    }
}
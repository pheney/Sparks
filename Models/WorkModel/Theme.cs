using Sparks.Models.DataModel;
using Sparks.Models.ViewModel;
using System.Collections.Generic;
using System.Linq;

namespace Sparks.Models.WorkModel
{
    /// <summary>
    /// Story Theme is made from:
    ///     Artistic Concept (something from nothing, hope, the color blue)
    ///     Theme (cyberpunk, vampires in the city)
    ///     Mood (happy, sad, exciting, scary...)
    ///     
    /// Game Theme is made from:
    ///     Category (pirates, ships, space-combat)
    ///     Subcategory (FPS, Racer, 4X)
    /// </summary>
    public static class Theme
    {
        public static VmTheme AnyStoryDrivenTheme()
        {
            var vm = new VmTheme();
            vm.ArtisticConcept= Theme.ArtisticConcept();
            vm.LiterarySetting= Theme.StoryTheme();
            vm.EvocativeMood= Theme.StoryMood();                        
            return vm;
        }

        /// <summary>
        /// Returns a literary setting, game-type and a unique location,
        /// fantasy RPG in lost spaceship
        /// </summary>
        public static VmGeneral AnyMechanicDrivenTheme()
        {
            var Setting = Location.UniqueLocation();
            var Category = Theme.GameplayCategory();
            var Subcategory = Theme.GameplaySubcategory();

            var vm = new VmGeneral();
            vm.Name = Category.NamePrep + " " + Category.Name + " " + Subcategory.Name
                + " game set in " + Setting.NamePrep + " " + Setting.Name + ".";
            return vm;
        }

        #region Story-driven theme
        
        /// <summary>
        /// TODO
        /// Ongoing. Add more ideas to this db table. Consider reorganizing the
        /// table into a list of single ideas, and then have this class randomly
        /// combine pairs, instead of listing fixed pairs in the db.
        /// 
        /// Design Theme is the driving idea behind the game; the artistic
        /// concept the game conveys, such as 'hope' or 'something from nothing'
        /// </summary>
        public static VmGeneral ArtisticConcept()
        {
            var db = new SparksDbContext();
            List<Game_DesignTheme> concepts = db.GameDesignTheme.ToList();
            int firstIndex = Utilities.random.Next(0, concepts.Count);
            int secondIndex = firstIndex;
            do
            {
                secondIndex = Utilities.random.Next(0, concepts.Count);
            } while (firstIndex == secondIndex);

            string first = concepts[firstIndex].Name;
            string second = concepts[secondIndex].Name;
            //  phrased as a complext noun, e.g., so it fits
            //  the sentence "the theme is about ..."
            string[] templates =
            {
                "the evolution of {1} from {0}",
                "how '{0}' is the beginning, but '{1}' is the end",
                "the similarity of {0} and {1}",
                "{0} leading to the sacrifice of {1}",
                "giving up {0} to gain {1}",
                "{0} and {1}",
                "going from {0} to {1}",
                "finding {0} from {1}",
                "{0} becoming {1}",
                "exchanging {0} for {1}",
                "discovering {0} in {1}",
                "bringing {0} together with {1}",
                "the commonality between {0} and {1}",
                "the duality of {0} and {1}",
                "bridging {0} and {1}",
                "unifying {0} and {1}",
                "separating {0} from {1}",
                "the difference between {0} and {1}",
                "parallels between {0} and {1}"
            };

            VmGeneral result = new VmGeneral();

            string selected = templates[Utilities.random.Next(0, templates.Length)];
            result.Name = selected.Replace("{0}", first).Replace("{1}", second);
            return result;
        
        }
        
        /// <summary>
        /// The story genre, e.g., cyberpunk, high fantasy, modern warfare, gothic,
        /// medieval horror, etc.
        /// 
        /// TODO
        /// Revise db so all entries read logically in the sentence "this is
        /// a *cyberpunk* story."
        /// </summary>
        public static VmGeneral StoryTheme()
        {
            var db = new SparksDbContext();

            //  Literary Genre is the theme of the story setting, such as 'cyberpunk',
            //  or 'vampires in the modern city' or 'alien invasion'
            return db.GameLiteraryGenre.SelectRandom().ToViewModel();
        }

        /// <summary>
        /// Emotional mood, e.g., happy, sad, exciting, scary...
        /// 
        /// TODO
        /// Revise GameMood data in db so words are conjugated to answer 
        /// "what kind of story? a *happy* story"
        /// </summary>
        public static VmGeneral StoryMood()
        {
            var db = new SparksDbContext();
        
            //  Mood is how the game approaches it's story, such as 'light hearterd'
            //  or 'serious' or 'black comedy'
            return db.GameMood.SelectRandom().ToViewModel();
        }

        #endregion
        #region Mechanics-driven theme

        /// <summary>
        /// This is the thematic gameplay category, such as "pirates" or "space combat".
        /// This is not RPG, FPS, 4X, etc.
        /// 
        /// Randomizes the parenthetical information for GameGenres
        /// from the Business tables.
        /// </summary>
        public static VmGeneral GameplayCategory()
        {
            var db = new SparksDbContext();

            //  Genre is the specific game theme
            VmGeneral result= db.GameGenre.SelectRandom().ToViewModel();

            result.Name = result.Name.ToLower();
            
            switch (result.ID)
            {
                case 1:
                case 9:
                case 16:
                case 21:
                case 33:
                case 36:
                case 43:
                case 55:
                case 50:
                    result.Name = result.Name.Replace("{data}", result.Description.Split(',').SelectRandom().Trim());
                    break;
            }

            if (result.ID == 50)
            {
                //  pick a sport
                var sport = "sportball";    //  RealSport.AnySport().Name;
                result.Name = result.Name.Replace("{sport}", sport.Trim());
            }

            return result;
        }

        /// <summary>
        /// This is the "Gameplay" category of the game, e.g., FPS, RTS, 4X,
        /// racing, platformer, etc. This is purely mechanical and independant
        /// of world setting, artistic theme, etc.
        /// 
        /// TODO
        /// Review db. Ensure there are only primary gameplay mechanics, such 
        /// as "racer", "4X" or "FPS"
        /// </summary>
        public static VmGeneral GameplaySubcategory()
        {
            var db = new SparksDbContext();

            //  Type is the sub-category of game-play, such as Kart Racing,
            //  Genetics Puzzle, or Coloniztion
            return db.GameGameplay.SelectRandom().ToViewModel();
        }

        public static VmGeneral AnyBusiness()
        {
            var db = new SparksDbContext();
            return db.Businesses.SelectRandom().ToViewModel();
        }

        #endregion
    }

}
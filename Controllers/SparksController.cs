using Newtonsoft.Json;
using Sparks.Models.ViewModel;
using Sparks.Models.WorkModel;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;

namespace Sparks.Controllers
{
    public class SparksController : Controller
    {
        #region Development

        [HttpGet]
        public ActionResult Dev(string viewName)
        {
            return View(viewName);
        }


        [HttpGet]
        public ActionResult Proto()
        {
            return View("prototypePage");
        }

        #endregion
        #region Primary navigation

        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.Jabber = Jabberwock.AnyWord(Jabberwock.Language.Jabber);
            return View();
        }

        #endregion
        #region API
        #region Language

        [HttpGet]
        public ActionResult PokeJabberwocky(int count, int dialect = 0)
        {
            var j = Json(GetJabberwockyWords(count, dialect));
            j.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return j;
        }

        private List<string> GetJabberwockyWords(int count, int dialect = 0)
        {
            List<string> jabber = new List<string>();
            for (int i = 0; i < count; i++)
            {
                jabber.Add(Jabberwock.AnyWord((Jabberwock.Language)dialect));
            }
            return jabber;
        }

        [HttpGet]
        public ActionResult WordByLanguageID(int count, int languageID, int dialect = 0)
        {
            var j = Json(GetWordsByLanguageID(count, languageID, dialect));
            j.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return j;
        }

        /// <summary>
        /// Returns a number of words equal to [count]. These words are generated using
        /// [languageID] to indicate engine/algorithm to use. [dialect] indicates the
        /// type of word (normal, consonant, vowel).
        /// </summary>
        private List<string> GetWordsByLanguageID(int count, int languageID, int dialect = 0)
        {
            if (languageID == 0) return GetJabberwockyWords(count, dialect);

            List<string> words = new List<string>();
            for (int i = 0; i < count; i++)
            {
                words.Add(Polyglot.AnyWord((Polyglot.WordType)dialect, languageID));
            }
            return words;
        }

        [HttpGet]
        public ActionResult SentenceByLanguageID(int count, int languageID, int dialect = 0)
        {
            var j = Json(GetSentenceByLanguageID(count, languageID, dialect));
            j.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return j;
        }

        /// <summary>
        /// Returns complete sentences in Proper case (first letter capitalized, period
        /// at the end). The number of sentences returned is [count]. [languageID] determines
        /// the word-generation algorithm used. [dialect] indicates the type of word
        /// (normal, consonant, vowel). Sentences are 5-25 words long.
        /// </summary>
        private List<string> GetSentenceByLanguageID(int count, int languageID, int dialect = 0)
        {
            List<string> sentences = new List<string>();
            for (int i = 0; i < count; i++)
            {
                //  generate each sentence
                StringBuilder sentence = new StringBuilder();
                int words = Utilities.random.Next(20) + 5;
                for (int k = 0; k < words; k++)
                {
                    if (languageID == 0)
                    {
                        //  Jabberwocky
                        sentence.Append(Jabberwock.AnyWord((Jabberwock.Language)dialect));
                    }
                    else
                    {
                        //  Polyglot
                        sentence.Append(Polyglot.AnyWord((Polyglot.WordType)dialect, languageID));
                    }
                    sentence.Append(" ");
                }
                sentences.Add(sentence.ToString().TrimEnd(' ').ToProper() + ".");
            }
            return sentences;
        }

        [HttpGet]
        public ActionResult ParagraphByLanguageID(int count, int languageID, int dialect = 0)
        {
            var j = Json(GetParagraphByLanguageID(count, languageID, Utilities.random.Next(50) + 50, dialect));
            j.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return j;
        }

        /// <summary>
        /// Returns a number of paragraphs equal to [count]. Each paragraph is comprised of
        /// sentences. The sentences are in proper case. The paragraph word count is [wordCount].
        /// [languageID] determines the word-generation engine used. [dialect] determines
        /// the types of words used (normal, consonant, vowel).
        /// </summary>
        private List<string> GetParagraphByLanguageID(int count, int languageID, int wordCount, int dialect = 0)
        {
            List<string> paragraphs = new List<string>();

            for (int i = 0; i < count; i++)
            {
                ;
                List<string> sentences = new List<string>();
                int words = 0;

                #region get sentences

                while (words < wordCount)
                {
                    int maxLength = wordCount - words;
                    string sentence = GetSentenceByLanguageID(1, languageID, dialect)[0];
                    int sentenceLength = sentence.Split(' ').Length;

                    if (maxLength < 4)
                    {
                        #region extend previous sentence

                        //  Not enough room for a new sentence.
                        //  Insert a few extra words at the end of the previous sentence

                        //  Get the previous sentence and remove the ending period                        
                        string lastSentence = sentences[sentences.Count - 1];
                        lastSentence.Replace('.', ' ');

                        //  The last sentence is going to be modified and replace, so 
                        //  remove the original.
                        sentences.RemoveAt(sentences.Count - 1);
                        words -= lastSentence.Split(' ').Length;

                        //  add however many words are left (1-3)
                        for (int j = 0; j < maxLength; j++)
                        {
                            lastSentence += GetWordsByLanguageID(1, languageID, dialect);
                            lastSentence += " ";
                        }
                        lastSentence.TrimEnd(' ');

                        //  set the 'current' sentence
                        sentence = lastSentence +".";

                        #endregion
                    }
                    if (sentenceLength > maxLength)
                    {
                        #region truncate current sentence

                        //  sentence length will be limited
                        StringBuilder newSentence = new StringBuilder();
                        string[] originalWords = sentence.Replace('.', ' ').Split(' ');
                        for (int j = 0; j < maxLength; j++)
                        {
                            newSentence.Append(originalWords[j]);
                            newSentence.Append(" ");
                        }
                        sentence = newSentence.ToString().TrimEnd(' ') + ".";

                        #endregion
                    }
                    else
                    {
                        //  The current sentence will do.
                        //  No special action required.
                    }

                    sentences.Add(sentence);
                    words += sentence.Split(' ').Length;
                }

                #endregion

                //  Build the paragraph object from the sentences
                StringBuilder paragraph = new StringBuilder();
                foreach (string sentence in sentences)
                {
                    paragraph.Append(sentence);
                    paragraph.Append(" ");
                }
                paragraphs.Add(paragraph.ToString());
            }

            return paragraphs;
        }

        [HttpGet]
        public ActionResult Languages()
        {
            var j = Json(GetLanguages());
            j.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return j;
        }

        private List<VmGeneral> GetLanguages()
        {
            VmGeneral jabberwocky = new VmGeneral();
            jabberwocky.Name = "Jabberwocky";

            List<VmGeneral> languages = new List<VmGeneral>();
            languages.Add(jabberwocky);
            languages.AddRange(Polyglot.GetLanguages());
            return languages;
        }

        #endregion
        #region PC/NPC

        /// <summary>
        /// Returns a complete character concept.
        /// </summary>
        [HttpGet]
        public ActionResult Character(int count)
        {
            List<VmCharacter> characterList = new List<VmCharacter>();
            for (int i = 0; i < count; i++)
            {
                characterList.Add(Models.WorkModel.Character.AnyCharacter());
            }
            var j = Json(characterList);
            j.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            return j;
        }

        /// <summary>
        /// Returns a personal weapon or magic item
        /// </summary>
        [HttpGet]
        public ActionResult Weapon(int count, string types = null, string mods=null)
        {
            int[] weaponTypes = null;
            if (types != null)
            {
                weaponTypes = JsonConvert.DeserializeObject<int[]>(types);
            }

            int[] modTypes = null;
            if (mods != null)
            {
                modTypes = JsonConvert.DeserializeObject<int[]>(mods);
            }

            List<VmWeapon> weaponList = new List<VmWeapon>();

            for (int i = 0; i < count; i++)
            {
                weaponList.Add(Models.WorkModel.Weapon.AnyWeapon(weaponTypes, modTypes));
            }

            var j = Json(weaponList);
            j.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            return j;
        }

        /// <summary>
        /// Returns a number of species equal to the parameter.
        /// Species are returned as a list of VmGeneral objects.
        /// The name is generated using Jabberwocky. The 
        /// description resembles Flying Turtle, Skeleton Dinosaur, 
        /// Crystal Humanoid, etc. GameEffect is empty.
        /// </summary>
        [HttpGet]
        public ActionResult Species(int count)
        {
            List<VmGeneral> speciesList = new List<VmGeneral>();
            for (int i = 0; i < count; i++)
            {
                speciesList.Add(Models.WorkModel.Species.AnySpecies());
            }
            var j = Json(speciesList);
            j.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            return j;
        }

        /// <summary>
        /// Returns a title, archetype, profession, etc
        /// </summary>
        [HttpGet]
        public ActionResult Title(int count, string titleTypes)
        {
            int[] validOptions = null;
            if (titleTypes != null)
            {
                validOptions = JsonConvert.DeserializeObject<int[]>(titleTypes);
            }

            List<VmTitle> titleList = new List<VmTitle>();

            for (int i = 0; i < count; i++)
            {
                titleList.Add(Models.WorkModel.Title.AnyTitle(validOptions));
            }
            var j = Json(titleList);
            j.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            return j;
        }

        /// <summary>
        /// Returns a random non-weapon object
        /// </summary>
        [HttpGet]
        public ActionResult InterestingItem(int count)
        {
            List<VmWeapon> itemList = new List<VmWeapon>();
            for (int i = 0; i < count; i++)
            {
                itemList.Add(Models.WorkModel.Weapon.AnyItem());
            }
            var j = Json(itemList);
            j.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            return j;
        }

        #endregion
        #region Vehicle

        [HttpGet]
        public ActionResult Vehicle(int count, int attrCount = 1)
        {
            List<VmVehicle> vehicleList = new List<VmVehicle>();
            for (int i = 0; i < count; i++)
            {
                vehicleList.Add(Models.WorkModel.Vehicle.AnyVehicle(attrCount));
            }
            var j = Json(vehicleList);
            j.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            return j;
        }

        /// <summary>
        /// Returns a list of vehicles that are defined by how they move,
        /// e.g., four-legged car, hover cycle, etc
        /// </summary>
        [HttpGet]
        public ActionResult VehicleByMovement(int count, int attrCount = 1, string zones = null)
        {
            int[] validOptions = null;
            if (zones != null)
            {
                validOptions = JsonConvert.DeserializeObject<int[]>(zones);
            }

            List<VmVehicle> vehicleList = new List<VmVehicle>();

            for (int i = 0; i < count; i++)
            {
                vehicleList.Add(Models.WorkModel.Vehicle.VehicleByMovement(validOptions));
            }
            var j = Json(vehicleList);
            j.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return j;
        }

        /// <summary>
        /// Returns a list of vehicles that are defined by their purpose,
        /// e.g., hover police car, ancient ambulance, destroyed construction vehicle
        /// </summary>
        [HttpGet]
        public ActionResult VehicleByPurpose(int count)
        {
            List<VmVehicle> vehicleList = new List<VmVehicle>();
            for (int i = 0; i < count; i++)
            {
                vehicleList.Add(Models.WorkModel.Vehicle.VehicleByPurpose());
            }
            var j = Json(vehicleList);
            j.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return j;
        }

        /// <summary>
        /// Returns a list of tactical vehicles
        /// e.g., tanks, APCs, fighter jet, helicopter
        /// </summary>
        [HttpGet]
        public ActionResult VehicleTactical(int count, int attrCount = 1)
        {
            List<VmVehicle> vehicleList = new List<VmVehicle>();
            for (int i = 0; i < count; i++)
            {
                vehicleList.Add(Models.WorkModel.Vehicle.CombatVehicle(attrCount));
            }
            var j = Json(vehicleList);
            j.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return j;
        }

        /// <summary>
        /// Returns a list of capital warships,
        /// e.g., frigate, battleship, cruiser
        /// </summary>
        [HttpGet]
        public ActionResult VehicleWarship(int count, int attrCount = 1)
        {
            List<VmVehicle> vehicleList = new List<VmVehicle>();
            for (int i = 0; i < count; i++)
            {
                vehicleList.Add(Models.WorkModel.Vehicle.Warship(attrCount));
            }
            var j = Json(vehicleList);
            j.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return j;
        }

        #endregion
        #region World

        [HttpGet]
        public ActionResult World(int count)
        {
            List<VmWorld> WorldList = new List<VmWorld>();
            for (int i = 0; i < count; i++)
            {
                WorldList.Add(Models.WorkModel.World.AnyWorld());
            }
            var j = Json(WorldList);
            j.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            return j;
        }

        [HttpGet]
        public ActionResult Star(int count)
        {
            List<VmGeneral> StarList = new List<VmGeneral>();
            for (int i = 0; i < count; i++)
            {
                VmGeneral vm = Models.WorkModel.World.Star();
                vm.Description = vm.Name;
                vm.Name = Jabberwock.AnyWord();
                StarList.Add(vm);
            }
            var j = Json(StarList);
            j.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            return j;
        }

        [HttpGet]
        public ActionResult Planet(int count)
        {
            List<VmGeneral> PlanetList = new List<VmGeneral>();
            for (int i = 0; i < count; i++)
            {
                VmGeneral vm = Models.WorkModel.World.Planet();
                vm.Description = vm.Name;
                vm.Name = Jabberwock.AnyWord();
                PlanetList.Add(vm);
            }
            var j = Json(PlanetList);
            j.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            return j;
        }

        [HttpGet]
        public ActionResult Landmass(int count)
        {
            List<VmGeneral> LandList = new List<VmGeneral>();
            for (int i = 0; i < count; i++)
            {
                VmGeneral vm = Models.WorkModel.World.Landmass();
                vm.Description = vm.Name;
                vm.Name = Jabberwock.AnyWord();
                LandList.Add(vm);
            }
            var j = Json(LandList);
            j.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            return j;
        }

        [HttpGet]
        public ActionResult Waterbody(int count)
        {
            List<VmGeneral> WaterList = new List<VmGeneral>();
            for (int i = 0; i < count; i++)
            {
                VmGeneral vm = Models.WorkModel.World.Waterbody();
                vm.Description = vm.Name;
                vm.Name = Jabberwock.AnyWord();
                WaterList.Add(vm);
            }
            var j = Json(WaterList);
            j.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            return j;
        }

        [HttpGet]
        public ActionResult Ecology(int count)
        {
            List<VmGeneral> EcologyList = new List<VmGeneral>();
            for (int i = 0; i < count; i++)
            {
                VmGeneral vm = Models.WorkModel.World.Ecology();
                EcologyList.Add(vm);
            }
            var j = Json(EcologyList);
            j.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            return j;
        }

        #endregion
        #region Other

        [HttpGet]
        public ActionResult Location(int count)
        {
            List<VmGeneral> LocationList = new List<VmGeneral>();
            for (int i = 0; i < count; i++)
            {
                LocationList.Add(Models.WorkModel.Location.UniqueLocation());
            }
            var j = Json(LocationList);
            j.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            return j;
        }

        [HttpGet]
        public ActionResult Society(int count, float attrChance)
        {
            List<VmSociety> societyList = new List<VmSociety>();
            for (int i = 0; i < count; i++)
            {
                societyList.Add(Models.WorkModel.Society.AnySociety(attrChance));
            }
            var j = Json(societyList);
            j.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            return j;
        }

        #endregion
        #region Genre, Theme, Mechanics

        [HttpGet]
        public ActionResult Genre(int count)
        {
            List<VmGeneral> genreList = new List<VmGeneral>();
            for (int i = 0; i < count; i++)
            {
                genreList.Add(Models.WorkModel.Theme.AnyMechanicDrivenTheme());
            }
            var j = Json(genreList);
            j.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            return j;
        }

        [HttpGet]
        public ActionResult Mechanics(int count)
        {
            List<VmMechanics> mechanicsList = new List<VmMechanics>();
            for (int i = 0; i < count; i++)
            {
                mechanicsList.Add(Models.WorkModel.Mechanics.AnyMechanics());
            }
            var j = Json(mechanicsList);
            j.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            return j;
        }

        [HttpGet]
        public ActionResult Theme(int count)
        {
            List<VmTheme> themeList = new List<VmTheme>();
            for (int i = 0; i < count; i++)
            {
                themeList.Add(Models.WorkModel.Theme.AnyStoryDrivenTheme());
            }
            var j = Json(themeList);
            j.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            return j;
        }

        #endregion
        #region Sparks

        [HttpGet]
        public ActionResult Traditional(int count)
        {
            List<VmIdeaTraditional> traditionalList = new List<VmIdeaTraditional>();
            for (int i = 0; i < count; i++)
            {
                traditionalList.Add(Models.WorkModel.Game.AnyTraditional());
            }
            var j = Json(traditionalList);
            j.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            return j;
        }

        [HttpGet]
        public ActionResult Digital(int count)
        {
            List<VmIdeaTraditional> digitalList = new List<VmIdeaTraditional>();
            for (int i = 0; i < count; i++)
            {
                digitalList.Add(Models.WorkModel.Game.AnyDigital());
            }
            var j = Json(digitalList);
            j.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            return j;
        }

        #endregion
        #endregion
    }
}
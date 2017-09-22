using System.Data.Entity;

namespace Sparks.Models.DataModel
{
    public partial class SparksDbContext : DbContext
    {
        //  constructor
        public SparksDbContext() : base("name=SparksDb") {

            /*
            Ref: http://stackoverflow.com/questions/3552000/entity-framework-code-only-error-the-model-backing-the-context-has-changed-sinc
            Here is what is going on and what to do about it:

            When a model is first created, we run a DatabaseInitializer to do things like create the database
            if it's not there or add seed data. The default DatabaseInitializer tries to compare the database 
            schema needed to use the model with a hash of the schema stored in an EdmMetadata table that is 
            created with a database (when Code First is the one creating the database). Existing databases 
            won’t have the EdmMetadata table and so won’t have the hash…and the implementation today will 
            throw if that table is missing. We'll work on changing this behavior before we ship the fial 
            version since it is the default. Until then, existing databases do not generally need any 
            database initializer so it can be turned off for your context type by calling:
            */
            Database.SetInitializer<SparksDbContext>(null);
        }

        public virtual DbSet<Actor_Abilities> ActorAbilities { get; set; }
        public virtual DbSet<Actor_Collective> ActorCollective { get; set; }
        public virtual DbSet<Actor_Composition> ActorComposition { get; set; }
        public virtual DbSet<Actor_Distribution> ActorDistribution { get; set; }
        public virtual DbSet<Actor_Mutation> ActorMutation { get; set; }
        public virtual DbSet<Actor_Uniformity> ActorUniformity { get; set; }
        public virtual DbSet<Animal_Amphibian> AnimalAmphibian { get; set; }
        public virtual DbSet<Animal_Bear> AnimalBear { get; set; }
        public virtual DbSet<Animal_BirdOfPrey> AnimalBirdOfPrey { get; set; }
        public virtual DbSet<Animal_Cat> AnimalCat { get; set; }
        public virtual DbSet<Animal_Diet> AnimalDiet { get; set; }
        public virtual DbSet<Animal_Dinosaur> AnimalDinosaur { get; set; }
        public virtual DbSet<Animal_Dog> AnimalDog { get; set; }
        public virtual DbSet<Animal_Insect> AnimalInsect { get; set; }
        public virtual DbSet<Animal_Invertebrate> AnimalInvertebrate { get; set; }
        public virtual DbSet<Animal_Lizard> AnimalLizard { get; set; }
        public virtual DbSet<Animal_Mammal> AnimalMammal { get; set; }
        public virtual DbSet<Animal_Mythical> AnimalMythical { get; set; }
        public virtual DbSet<Animal_Shark> AnimalShark { get; set; }
        public virtual DbSet<Animal_Snake> AnimalSnake { get; set; }
        public virtual DbSet<Animal_Turtle> AnimalTurtle { get; set; }
        public virtual DbSet<Animal_Type> AnimalType { get; set; }
        public virtual DbSet<Animal_Undead> AnimalUndead { get; set; }

        public virtual DbSet<Character_Activity> CharacterActivity { get; set; }
        public virtual DbSet<Character_Age> CharacterAge { get; set; }
        public virtual DbSet<Character_Appearance> CharacterAppearance { get; set; }
        public virtual DbSet<Character_CombatRole> CharacterCombatRole { get; set; }
        public virtual DbSet<Character_Experience> CharacterExperience { get; set; }
        public virtual DbSet<Character_Personality> CharacterPersonality { get; set; }
        public virtual DbSet<Character_TitleType> CharacterTitleType { get; set; }
        public virtual DbSet<Species> Species { get; set; }
        public virtual DbSet<Species_Template> SpeciesTemplate { get; set; }

        public virtual DbSet<Game_Component> GameComponent { get; set; }
        public virtual DbSet<Game_Commitment> GameCommitment { get; set; }
        public virtual DbSet<Game_Control> GameControl { get; set; }
        public virtual DbSet<Game_Extension> GameExtension { get; set; }
        public virtual DbSet<Game_Gameplay> GameGameplay { get; set; }
        public virtual DbSet<Game_Genre> GameGenre { get; set; }
        public virtual DbSet<Game_LiteraryGenre> GameLiteraryGenre { get; set; }
        public virtual DbSet<Game_Mood> GameMood { get; set; }
        public virtual DbSet<Game_System> GameSystem { get; set; }
        public virtual DbSet<Game_TargetMarket> GameTargetMarket { get; set; }
        public virtual DbSet<Game_DesignTheme> GameDesignTheme { get; set; }

        public virtual DbSet<GameMechanics_Arcade> GameMechanicsArcade { get; set; }
        public virtual DbSet<GameMechanics_Contest> GameMechanicsContest { get; set; }
        public virtual DbSet<GameMechanics_Cooperation> GameMechanicsCooperation { get; set; }
        public virtual DbSet<GameMechanics_Movement> GameMechanicsMovement { get; set; }
        public virtual DbSet<GameMechanics_Obstacle> GameMechanicsObstacle { get; set; }
        public virtual DbSet<GameMechanics_Primary> GameMechanicsPrimary { get; set; }
        public virtual DbSet<GameMechanics_WinCondition> GameMechanicsWinCondition { get; set; }

        public virtual DbSet<Item_Magic> ItemMagic { get; set; }
        public virtual DbSet<Item_Miscellaneous> ItemMiscellaneous { get; set; }
        public virtual DbSet<Item_Tool> ItemTool { get; set; }

        public virtual DbSet<Movement_AirshipType> MovementAirshipType { get; set; }
        public virtual DbSet<Movement_FlightType> MovementFlightType { get; set; }
        public virtual DbSet<Movement_MultiSystemType> MovementMultiSystemType { get; set; }
        public virtual DbSet<Movement_SailType> MovementSailType { get; set; }
        public virtual DbSet<Movement_SpaceType> MovementSpaceType { get; set; }
        public virtual DbSet<Movement_WaterType> MovementWaterType { get; set; }

        public virtual DbSet<PowerSystem_Container> PowerSystemContainer { get; set; }
        public virtual DbSet<PowerSystem_Energy> PowerSystemEnergy { get; set; }

        public virtual DbSet<Data_Engineer> DataEngineer { get; set; }
        public virtual DbSet<Data_Science> DataScience { get; set; }
        public virtual DbSet<Data_Science_Medical> DataScienceMedical { get; set; }
        public virtual DbSet<Data_Color> DataColor { get; set; }
        public virtual DbSet<Sensor_Bands> SensorBands { get; set; }
        public virtual DbSet<Data_General_Size> GeneralSize { get; set; }
        public virtual DbSet<Data_General_Quantity> GeneralQuantity { get; set; }
        public virtual DbSet<Data_General_Range> GeneralRange { get; set; }

        public virtual DbSet<Profession_Type> ProfessionType { get; set; }
        public virtual DbSet<Profession_Misc> ProfessionMisc { get; set; }
        public virtual DbSet<Profession_Technologist> ProfessionTechnologist { get; set; }
        public virtual DbSet<Profession_TechnologyField> ProfessionTechnologyField { get; set; }
        public virtual DbSet<Role_FacilityStaff> RoleFacilityStaff { get; set; }
        public virtual DbSet<Role_NauticalCrew> RoleNauticalCrew { get; set; }
        public virtual DbSet<Role_ProductionCrew> RoleProductionCrew { get; set; }

        public virtual DbSet<Business> Businesses { get; set; }
        public virtual DbSet<Business_Type> BusinessType { get; set; }

        public virtual DbSet<Sport> Sports { get; set; }
        public virtual DbSet<Sport_Type> SportTypes { get; set; }

        public virtual DbSet<Title_Political> TitlePolitical { get; set; }
        public virtual DbSet<Title_Heroic> TitleHeroic { get; set; }
        public virtual DbSet<Title_Magic> TitleMagic { get; set; }
        public virtual DbSet<Title_Melee> TitleMelee { get; set; }
        public virtual DbSet<Title_Savage> TitleSavage { get; set; }
        public virtual DbSet<Title_Shooter> TitleShooter { get; set; }

        public virtual DbSet<Vehicle_Base> BaseVehicle { get; set; }
        public virtual DbSet<Vehicle_Armor> VehicleArmor { get; set; }
        public virtual DbSet<Vehicle_CargoCapacity> VehicleCargoCapacity { get; set; }
        public virtual DbSet<Vehicle_Characteristic> VehicleCharacteristic { get; set; }
        public virtual DbSet<Vehicle_Condition> VehicleCondition { get; set; }
        public virtual DbSet<Vehicle_Defenses> VehicleDefenses { get; set; }
        public virtual DbSet<Vehicle_Material> VehicleMaterial { get; set; }
        public virtual DbSet<Vehicle_Movement> VehicleMovement { get; set; }
        public virtual DbSet<Vehicle_Offenses> VehicleOffenses { get; set; }
        public virtual DbSet<Vehicle_Size> VehicleSize { get; set; }
        public virtual DbSet<Vehicle_Power> VehiclePower { get; set; }
        public virtual DbSet<Vehicle_Purpose> VehiclePurpose { get; set; }
        public virtual DbSet<Vehicle_Range> VehicleRange { get; set; }
        public virtual DbSet<Vehicle_Speed> VehicleSpeed { get; set; }
        public virtual DbSet<Vehicle_Technology> VehicleTechnology { get; set; }
        public virtual DbSet<Vehicle_Warship> VehicleWarship { get; set; }
        public virtual DbSet<Vehicle_Military> VehicleMilitary { get; set; }

        public virtual DbSet<Weapon_Class> WeaponClass { get; set; }
        public virtual DbSet<Weapon_Magic> WeaponMagic { get; set; }
        public virtual DbSet<Weapon_Melee> WeaponMelee { get; set; }
        public virtual DbSet<Weapon_ModernRanged> WeaponModernRanged { get; set; }
        public virtual DbSet<Weapon_PrimativeRanged> WeaponPrimativeRanged { get; set; }
        public virtual DbSet<Weapon_Siege> WeaponSiege { get; set; }
        public virtual DbSet<Weapon_Size> WeaponSize { get; set; }
        public virtual DbSet<Weapon_Type> WeaponType { get; set; }
        public virtual DbSet<Weapon_Capital> WeaponCapital { get; set; }
        public virtual DbSet<WeaponMod_Aesthetic> WeaponModAesthetic { get; set; }
        public virtual DbSet<WeaponMod_MagicAugment> WeaponModMagicAugment { get; set; }
        public virtual DbSet<WeaponMod_MagicEnergy> WeaponModMagicEnergy { get; set; }
        public virtual DbSet<WeaponMod_NonEnergy> WeaponModNonEnergy { get; set; }
        public virtual DbSet<WeaponMod_TechEnergy> WeaponModTechEnergy { get; set; }
        public virtual DbSet<WeaponMod_Type> WeaponModType { get; set; }

        public virtual DbSet<World_Climate> WorldClimate { get; set; }
        public virtual DbSet<World_Ecosystem> WorldEcosystem { get; set; }
        public virtual DbSet<World_LandMass> WorldLandMass { get; set; }
        public virtual DbSet<World_Planet> WorldPlanet { get; set; }
        public virtual DbSet<World_Star> WorldStar { get; set; }
        public virtual DbSet<World_WaterBody> WorldWaterBody { get; set; }
        
        public virtual DbSet<World_CommonEnergy> WorldCommonEnergy { get; set; }
        public virtual DbSet<World_Ekistic> WorldEkistic { get; set; }
        public virtual DbSet<World_Epoch> WorldEpoch { get; set; }
        public virtual DbSet<World_MacroCulture> WorldMacroCulture { get; set; }
        public virtual DbSet<World_SettingAdjective> WorldSettingAdjective { get; set; }
        public virtual DbSet<World_SettingScale> WorldSettingScale { get; set; }
        public virtual DbSet<World_Society> WorldSociety { get; set; }
        public virtual DbSet<World_Technology> WorldTechnology { get; set; }
        
        public virtual DbSet<Society_CivilDefenseInfrastructure> SocietyCivilDefenseInf { get; set; }
        public virtual DbSet<Society_CulturalInfrastructure> SocietyCulturalInf { get; set; }
        public virtual DbSet<Society_EconomicInfrastructure> SocietyEconomicInf { get; set; }
        public virtual DbSet<Society_EducationInfrastructure> SocietyEducationInf { get; set; }
        public virtual DbSet<Society_EmergencyServicesInfrastructure> SocietyEmergencyServicesInf { get; set; }
        public virtual DbSet<Society_HealthCareInfrastructure> SocietyHealthCareInf { get; set; }
        public virtual DbSet<Society_IndustrialInfrastructure> SocietyIndustrialInf { get; set; }
        public virtual DbSet<Society_LawEnforcementInfrastructure> SocietyLawEnforcementInf { get; set; }
        public virtual DbSet<Society_SocialInfrastructure> SocietySocialInf { get; set; }
        public virtual DbSet<Society_Government> SocietyGovernment { get; set; }

        public virtual DbSet<Setting_CitySize> SettingCitySize { get; set; }
        public virtual DbSet<Setting_Location> SettingLocation { get; set; }
    }
}
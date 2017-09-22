using Sparks.Models.WorkModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Script.Serialization;

/*
    Society Type:
        pre-industrial
        - hunting and gathering
        - pastoral -- domesticated herd animals
        - horticultural -- fruit and vegetable farms
        - agrarian -- use technology to farm large areas of land
        - feudal -- social status based on the ownership of land
        industrial
        post-industrial
        - information
        - knowledge
            
    Economic Infrastructure:
    barter-based economy -- goods and services are exchanged on a small scale in local markets to meet individual needs
    market-based economy -- goods and services are exhcanged using a medium of exchange (currency) to meet supply and demand
    command-based economy -- central policital agent commands what is produced and how it is sold and sitributed. shortages are common problems.
    green economy -- low-carbon, resource efficient and socially inclusive.
    subsistence agriculture -- self-sufficiency farming
    slave traders -- society built on slave labor and trading
    industrial economy -- lessen role of subsistence farming. extensive mono-cultural forms of agriculture. commerce becomes more significant. economic groth in mining, construction and manufacturing.
    consumer society -- growing part played by service, finance and technology
    knowlege economy -- value on technology, information and processing power
    education economy -- value on scholarly endeavors, pedantic pursuits. largely post-scarcity society with most people have a lot of free time and available resources
    social economy -- value on social interaction, networking and politics
    science economy -- value on scientific knowlege and application, education is a commodety, which implies it is common to a degree, but high levels are rare. trading on scientific knowlege leads to massive technology gaps and equal focus on scientific research as well as espionage
    
*/
namespace Sparks.Models.ViewModel
{
    public class VmSociety
    {
        public string Name;
        public VmGeneral Government;
        public List<VmGeneral> Influences;
        public VmGeneral Descriptor, Ekistic, TechnologicalEra, Energy;
        public VmGeneral Culture;

        public VmGeneral SocialInfrastructure, EducationInfrastructure, HealthCareInfrastructure, 
            LawEnforcementInfrastructure, EmergencyServicesInfrastructure, CivilDefenseInfrastructure, 
            EconomicInfrastructure, IndustrialInfrastructure, CulturalInfrastructure;

        /*

            This nation is DESCRIPTOR.

            Economic Infrastructrue
        The economic infrastructure of the nation is made up of the ABC communication systems, the national TRANSPORTATION[A] and TRANSPORTATION[B]
        and distribution systems, the energy infrastructure and the financial institutions that fund everything.

            Social Infrastructure
        The nation's social infrastructure is comprised of the interplace of the education, law enforcement and health care infrastructures.
        Over time, the focus of the nation's financial and research endeviors shifts among these three areas in reaction to both
        political influences among the population and the reality of society and the current problems-of-the-day.
    
        The central unit of community in the nation is the EKISTIC UNIT, combined with the nation's COMMUNICATION infrastructure and
        TRANSPORTATION network.

        The society's value system revolves around the following primary IDEAS. (polygamy, highly ritualistic, etc)

        The society is comprised of different social echelons. Where an individual falls in class depends on CLASS-REASONS.

    class-based society, the elite are: none, purely random, the wealthiest, the best educated,
    the highest religious leaders, those with rare genetics (albinos), born of a specific 
    ethnicity (whites, blacks), born in a specific regional, the most corrupt, the highest skilled
    in a given field, the most combative, placed via a test of some sort
                        
        The DESCRIPTOR society is a result of the interplace of many aspects. It is a LANGUAGE nation, that believes in MARRIAGE-TYPE
        for MARRIAGE-DURATION. The RITUAL BASED society is strongly RELIGIOUS although formal religion has a NEGLIGIBLE effect on 
        daily life or politics.
        
    language: single pervasive language, bilingual, polyglot, regional dialects
    marriage: no such thing, gender dominant, monogomous, polygomous
    marriage duration: contractual, short term, long term, permanent
    artistic expression: none, taboo, limited, common, extensive, artisan / everything
    rituals: none, limited (few holidays, celebrations), common (holiday, celebrations, rituals), excessive
    religion (none, single dominant, government mandated, multiple belief systems, monotheistic, polytheistic
    religious influence on life: none, minor, moderate, strong, dominant
    technologies for cooking shelter and clothing: ?

        Artistic expression takes the form of EXPRESSION and is FORBIDDEN. 

    cultural expression: philosophical, combative, materialistic, ritualistic, artistic, pragmatic, dogmatic

        The society as a whole currently has a sharp interest in technology and science in the field of TECHNOLOGY.

            
            Education Infrastructure
        The goal of the national education system is the FAIR, IMPARTIAL and SYSTEMATIC provision of KNOWLEDGE, INFORMATION and 
        TECHNOLOGY to the entire nation's population for the PURPOSE (of the advancement of innovation and technology, social enrichment,
        and preservation of historical knowlege).
        
        The nation's education system is comprised of a variety of places for children to learn. This facilities vary from individual
        teach laboratory's, to classrooms, to lecture halls, to laboratories where students learn by performing hands-on experiements
        under the watchful eye of a professor.

        The education system has three components. The first is the primary and secondary education of children as they become adults. This
        includes education in basic sciences, art, history. It is a well-rounded education program that encourages curiosity, hard work,
        competition, achievement and initiative.

        The secondary component consists of formal education beyond secondary school. This is available to any adult, regardless of youth
        or age and enables them to continue their intelectual development through exposure to new technology and advancements in their field,
        as well as the opportunity to acquire additional advanced credentialling which can be politically advantageous when seeking 
        improved employment conditions.

        The final component consisits of of informal education that is available everywhere to all citizens. The level of initiative and
        autonomy required for this education system depends entirely on the nature of the subject and the method the student has chosen
        to employ in seeking the education. These opportunities can take the form of structured classes available for free at universities,
        libraries, and most large trade facilities, e.g., factories, infrastructure maintenance, service training, etc, as well as the
        opportunity to puruse academic education and keep up with research and trends in any chosen field.

            Health Care Infrastructure
        THe nation's HealthCare system is made up of PRIVATE ORGANIZATIONS that are subsidized by THE GOVERNMENT. THe intentent is to provide
        a BASIC LEVEL of healthcare to ALL CITIZENS, but due to REASONS (corruption), often the CLASS go untreated and the OTHER CLASS get 
        the best treatment.

            Law Enforcement Infrastructure
        Peace is maintained across the nation through [a combination of overlapping branches of law enforcement at all levels, from
        national police agency, down to local municipal law enforcement officials.] Jurisdiction on crime varies by [agency, nature of the crime
        and geographical location.]

            Emergency Services Infrastructure
        Emergency Services are a combination of [privitization] and [federally] supported [hopsitals] and [rescue organizations]. The two systems
        are complimentary. The federal agencies have greater reach and capability to help large numbers of people in all areas, while the
        private agencies are able to provide higher levels of advanced rescue and recovery technology taylored to specific individuals and 
        situations.

            Civil Defense Infrastructure
        Civil Defense is handled by the combined efforts of several distinct military branches, each with their own specialized area of warfare.
        By focusing on specific facets of warfare, each branch is able to focus their training and technology to address their given theater,
        however it means there is a gap in areas that fall outside of any single facet of combat.

            Industrial Infrastructure
        The nation's Industrial infrastructure was created over a period of 200-2000 years. It consists of a national highway system designed to
        enable the nation's population freedom of travel from any region to another. The highway system is maintained by a combination of national
        and local governments, and the level of disrepair is often coorelated to the depth in the support chain any particular road falls, with
        municipal and small town roads being in the worst states, and the national interstates being in the best repair.

        The highway system is suplemented by a rail system and airport network that facilitates moving large amounts of resources across the 
        nation. Thwo two systems are complimentary. The rail system can move greater amounts of heavier cargo, while the aircargo network
        facilitates rapid transit among cities, primarly for the puposes of enabling the population to travel quickly.

        The nation's energy is produced primarly from ENERGY-A sources, and suplemented with ENERGY-B sources in areas where geography or politics
        make ENERGY-A unavailable or impractical. THere is a growing movement to switch to ENERGY-C or ENERGY-D in an effort to allieviate
        the negative effects of usiung ENERGY-A.

            Cultural Infrastructure
        There are a number of significant factors that combine to shape the nation's cultural infrastructure.
        Event A created a shift in popular thinking about topic B among much of the citizenship. This shift lead to art projects and music
        significantly different from the past.
        
        The architectural style was influence by A and B, along with several other famous historical influencers of the past. The influence is
        regional and a visitor can easily tell what part of the country they are in, just from observing the structures alone.
        Also significant was the great calamety ABC of year 21012. This secondary effects of this effect rippled through society for several
        decades and manifest in a number of changes over time. roads and bridges are now built ABC. schools, libraries and other institutions of
        public funded learning are now DEFG.
        
        A growing movement for change in the existing traditions regarding spirituality, religion and traditions reached a tipping point
        as a result of political situation ABDE.
        */
        public string Description
        {
            get
            {
                return "The " + this.Name.ToProper() + " nation is" + (Descriptor != null ? " " + Descriptor.NamePrep + " " +
                    Descriptor.Name : "") + " " + Culture.Name + " society built upon" +
                    " the" + (Ekistic != null ? " " + Ekistic.Name : "") + " social unit. " +
                    "The " + Energy.Name + " powered society is otherwise typical of " +TechnologicalEra.NamePrep +
                    " " + TechnologicalEra.Name + " era.";
            }
        }

        public bool IsEmpty()
        {
            return Energy == null && Ekistic == null
                && Culture == null && Descriptor == null
                && TechnologicalEra == null;
        }

        public bool HasMinFieldCount(int requiredFieldCount)
        {
            int count = 0;
            count += Energy == null ? 0 : 1;
            count += Ekistic == null ? 0 : 1;
            count += Culture == null ? 0 : 1;
            count += Descriptor == null ? 0 : 1;
            count += TechnologicalEra == null ? 0 : 1;
            return count >= requiredFieldCount;
        }
    }
}
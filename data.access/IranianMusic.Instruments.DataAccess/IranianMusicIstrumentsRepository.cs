using System;
using System.Collections.Generic;

namespace IranianMusic.Instruments.DataAccess
{
    public static class IranianMusicIstrumentsRepository
    {
        private static readonly Dictionary<string, string> FormalInstruments
            = new Dictionary<string, string>();

        private static  List<string> _secoundaryInstruments;
        static IranianMusicIstrumentsRepository()
        {
            FillSecoundaryInstruments();
            FillFormalInstruments();
        }

        private static  void FillSecoundaryInstruments()
        {
            _secoundaryInstruments = new List<string>
            {
                "Oud",
                "Daf",
                "Robbab",
                "BamTar",
                "Qeijak"
            };
        }

        private static void FillFormalInstruments()
        {
            FormalInstruments["Tar"] = @"Tar (Persian: تار‎; Azerbaijani: tar) is an Iranian long-necked, waisted" +
                                        "instrument, shared by many cultures and countries including Iran, Azerbaijan, Armenia, Georgia," +
                                        "and others near the Caucasus region.[1][2][5] The word tār means string in Persian, " +
                                        "and is also related to the names of the guitar, sitar, setar (سه‌تار, \"three strings\") " +
                                        "and dutar (دوتار, \"two strings\")." +
                                        "It was invented in the 18th century and has since become one of the most important musical instruments" +
                                        "in Iran and the Caucasus, particularly in Persian classical music, and the favoured instrument for radifs." +
                                        "In 2012, the craftsmanship and performance art of the Azerbaijani tar was added to the UNESCO's " +
                                        "List of the Intangible Cultural Heritage of Humanity";
            FormalInstruments["Setar"] =
                @"The Setar (Persian: سه‌تار‎, from seh, meaning /""three/"" and tār, meaning /""string/"") 
                                        is an Iranian musical instrument. It is a member of the lute family, 
                                    which is played with the index finger of the right hand. Two and a half centuries ago, 
                                    a fourth string was added to the setar. It has 25–27 moveable frets which are usually
                                     made of animal intestines or silk. It originated in Persia before the spread of Islam.";
            FormalInstruments["Santur"] =
                @"The santur was invented and developed in the area of Iran, Kuwait, Syria and Turkey,
                                     and parts of Mesopotamia (modern-day Iraq). 
                                     This instrument was traded and traveled to different parts of the middle east 
                                     and each country customized and designed their own versions to adapt to their musical 
                                     scales and tunings.
                                     The original santur was made with tree bark, stones and stringed with goat intestines.
                                     The Mesopotamian santur is also the father of the harp, the Chinese yangqin, the harpsichord,
                                     the qanun, the cimbalom and the American and European hammered dulcimers.";
            FormalInstruments["Ney"] = @"The ney (Persian: نی / نای‎), is an end-blown flute that figures prominently in 
                                    Middle Eastern music. In some of these musical traditions, it is the only wind instrument used. 
                                    The ney has been played continuously for 4,500–5,000 years, making it one of the oldest musical instruments still in use.
                                    The Persian ney consists of a hollow cylinder with finger-holes. Sometimes a brass, horn, 
                                    or plastic mouthpiece is placed at the top to protect the wood from damage,
                                     and to provide a sharper and more durable edge to blow at. The ney consists of a piece of hollow cane or giant reed with
                                     five or six finger holes and one thumb hole. Modern neys may be made instead of metal or plastic tubing. 
                                    The pitch of the ney varies depending on the region and the finger arrangement. 
                                    A highly skilled ney player, called neyzen, can reach more than three octaves, though it is more common 
                                    to have several /""helper/"" neys to cover different pitch ranges or to facilitate playing 
                                    technically difficult passages in other dastgahs or maqams.
                                    
                                    In Romanian, the word nai[1] is also applied to a curved pan flute while an end-blown 
                                    flute resembling the Arab ney is referred to as caval.[2]";
            FormalInstruments["Kamancheh"] =
                @"The kamancheh (also kamānche or kamāncha) (Persian: کمانچه‎) is an Iranian bowed 
                                        string instrument, used also in Armenian, Azerbaijani, Turkish and Kurdish music and related 
                                        to the rebab, the historical ancestor of the kamancheh and also to the bowed Byzantine lyra, 
                                        ancestor of the European violin family. The strings are played with a variable-tension bow.
                                         It is widely used in the classical music of Iran, Armenia, Azerbaijan, Uzbekistan, 
                                        Turkmenistan and Kurdistan Regions with slight variations in the structure of the instrument.
                                        
                                        In 2017, the art of crafting and playing with Kamantcheh/Kamancha was included into the UNESCO 
                                        Intangible Cultural Heritage Lists.";
            FormalInstruments["Tonbak"] = @"The tompak (official Persian name) (تنپک, تنبک, دنبک، تمپک), also tombak, donbak,
                                     dombak or zarb (ضَرب or ضرب) in Afghanistan zer baghali (زیر بغلی ), is a goblet drum from Persia (ancient Iran).
                                     It is considered the principal percussion instrument of Persian music. 
                                    The tonbak is normally positioned diagonally across the torso while the player uses one or more 
                                    fingers and/or the palm(s) of the hand(s) on the drumhead, often (for a ringing timbre) 
                                    near the drumhead's edge. Sometimes tonbak players wear metal finger rings for an 
                                    extra-percussive /""click/"" on the drum's shell. Tonbak virtuosi perform solos 
                                    lasting ten minutes or more. The tompak had been used to create a goblet drum.";
        }

        public static bool HasFormalInstrument(string name)
        {
            return FormalInstruments.TryGetValue(name, out string _);
        }
        public static  bool HasSecoundaryInstrument(string name)
        {
            return _secoundaryInstruments.Contains(name);
        }
        public static string GetFormalInstrumentDescription(string instrumentName)
        {
            return FormalInstruments.TryGetValue(instrumentName, out var desc) ? desc : "";
        }

        public static  Dictionary<string, string> GetAllFormal()
        {
            return FormalInstruments;
        }
    }
}
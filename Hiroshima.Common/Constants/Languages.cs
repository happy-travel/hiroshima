using System;

namespace Hiroshima.Common.Constants
{
    public static class Languages
    {
        public static string GetLanguageCode(DcLanguages dcLanguage)
        {
            var languageCode = dcLanguage switch
            {
                DcLanguages.Arabic => ArabicLanguageCode,
                DcLanguages.Bulgarian => BulgarianLanguageCode,
                DcLanguages.German => GermanLanguageCode,
                DcLanguages.Greek => GreekLanguageCode,
                DcLanguages.English => EnglishLanguageCode,
                DcLanguages.Spanish => SpanishLanguageCode,
                DcLanguages.French => FrenchLanguageCode,
                DcLanguages.Italian => ItalianLanguageCode,
                DcLanguages.Hungarian => HungarianLanguageCode,
                DcLanguages.Polish => PolishLanguageCode,
                DcLanguages.Portuguese => PortugueseLanguageCode,
                DcLanguages.Romanian => RomanianLanguageCode,
                DcLanguages.Russian => RussianLanguageCode,
                DcLanguages.Serbian => SerbianLanguageCode,
                DcLanguages.Turkish => TurkishLanguageCode,
                DcLanguages.Unknown => throw new ArgumentException("Language is unknown"),
                _ => throw new ArgumentOutOfRangeException(nameof(dcLanguage), dcLanguage, null)
            };
            
            return languageCode.ToLowerInvariant();
        }


        public static bool TryGetLanguage(string languageCode, out DcLanguages language)
        {
            language = languageCode.ToUpperInvariant() switch
            {
                ArabicLanguageCode => DcLanguages.Arabic,
                BulgarianLanguageCode => DcLanguages.Bulgarian,
                GermanLanguageCode => DcLanguages.German,
                GreekLanguageCode => DcLanguages.Greek,
                EnglishLanguageCode => DcLanguages.English,
                SpanishLanguageCode => DcLanguages.Spanish,
                FrenchLanguageCode => DcLanguages.French,
                ItalianLanguageCode => DcLanguages.Italian,
                HungarianLanguageCode => DcLanguages.Hungarian,
                PolishLanguageCode => DcLanguages.Polish,
                PortugueseLanguageCode => DcLanguages.Portuguese,
                RomanianLanguageCode => DcLanguages.Romanian,
                RussianLanguageCode => DcLanguages.Russian,
                SerbianLanguageCode => DcLanguages.Serbian,
                TurkishLanguageCode => DcLanguages.Turkish,
                _ => DcLanguages.Unknown
            };

            return language != DcLanguages.Unknown;
        }

        
        public const DcLanguages DefaultLanguage = DcLanguages.English; 
        
        private const string ArabicLanguageCode = "ar";
        private const string BulgarianLanguageCode = "bg";
        private const string GermanLanguageCode = "de";
        private const string GreekLanguageCode = "el";
        private const string EnglishLanguageCode = "en";
        private const string SpanishLanguageCode = "es";
        private const string FrenchLanguageCode = "fr";
        private const string ItalianLanguageCode = "it";
        private const string HungarianLanguageCode = "hu";
        private const string PolishLanguageCode = "pl";
        private const string PortugueseLanguageCode = "pt";
        private const string RomanianLanguageCode = "ro";
        private const string RussianLanguageCode = "ru";
        private const string SerbianLanguageCode = "sr";
        private const string TurkishLanguageCode = "tr";
    }
}
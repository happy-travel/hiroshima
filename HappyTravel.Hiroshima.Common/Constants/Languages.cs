using System;

namespace HappyTravel.Hiroshima.Common.Constants
{
    public static class Languages
    {
        public static string GetLanguageCode(DcLanguages dcLanguage)
        {
            var languageCode = dcLanguage switch
            {
                DcLanguages.Arabic => ArabicLanguageCode,
                DcLanguages.English => EnglishLanguageCode,
                DcLanguages.Russian => RussianLanguageCode,
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
                EnglishLanguageCode => DcLanguages.English,
                RussianLanguageCode => DcLanguages.Russian,
                _ => DcLanguages.Unknown
            };

            return language != DcLanguages.Unknown;
        }

        
        public const DcLanguages DefaultLanguage = DcLanguages.English; 
        
        private const string ArabicLanguageCode = "AR";
        private const string EnglishLanguageCode = "EN";
        private const string RussianLanguageCode = "RU";
    }
}
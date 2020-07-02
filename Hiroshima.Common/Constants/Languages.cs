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
        
        private const string ArabicLanguageCode = "ar";
        private const string EnglishLanguageCode = "en";
        private const string RussianLanguageCode = "ru";
    }
}
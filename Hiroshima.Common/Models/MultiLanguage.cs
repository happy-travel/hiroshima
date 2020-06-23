using Hiroshima.Common.Constants;

namespace Hiroshima.Common.Models
{
    public class MultiLanguage<T>
    {
        public T Ar { get; set; }
        public T Bg { get; set; }
        public T De { get; set; }
        public T El { get; set; }
        public T En { get; set; }
        public T Es { get; set; }
        public T Fr { get; set; }
        public T It { get; set; }
        public T Hu { get; set; }
        public T Pl { get; set; }
        public T Pt { get; set; }
        public T Ro { get; set; }
        public T Ru { get; set; }
        public T Sr { get; set; }
        public T Tr { get; set; }


        public bool TryGetValue(string languageCode, out T value)
        {
            switch (languageCode.ToUpperInvariant())
            {
                case "AR":
                    value = Ar;
                    break;
                case "BG":
                    value = Bg;
                    break;
                case "DE":
                    value = De;
                    break;
                case "El":
                    value = El;
                    break;
                case "EN":
                    value = En;
                    break;
                case "ES":
                    value = Es;
                    break;
                case "FR":
                    value = Fr;
                    break;
                case "IT":
                    value = It;
                    break;
                case "HU":
                    value = Hu;
                    break;
                case "PL":
                    value = Pl;
                    break;
                case "PT":
                    value = Pt;
                    break;
                case "RO":
                    value = Ro;
                    break;
                case "RU":
                    value = Ru;
                    break;
                case "SR":
                    value = Sr;
                    break;
                case "TR":
                    value = Tr;
                    break;
                default:
                    value = default;
                    return false;
            }
            
            return value != null;
        }


        public bool TryGetValueOrDefault(string languageCode, out T value)
        {
            if (TryGetValue(languageCode, out value))
                return true;
            
            var defaultLanguageCode = Languages.GetLanguageCode(Languages.DefaultLanguage);
            
            return TryGetValue(defaultLanguageCode, out value);
        }
        
        
        public bool TryAddValue(string languageCode, T value)
        {
            if (!Languages.TryGetLanguage(languageCode, out var language)) 
                return false;
            
            return TryAddValue(language, value);
        }
        
        
        public bool TryAddValue(DcLanguages etgLanguage, T value)
        {
            switch (etgLanguage)
            {
                case DcLanguages.Arabic:
                    Bg = value;
                    return true;
                case DcLanguages.Bulgarian:
                    Bg = value;
                    return true;
                case DcLanguages.German:
                    De = value;
                    return true;
                case DcLanguages.Greek:
                    El = value;
                    return true;
                case DcLanguages.English:
                    En = value;
                    return true;
                case DcLanguages.Spanish:
                    Es = value;
                    return true;
                case DcLanguages.French:
                    Fr = value;
                    return true;
                case DcLanguages.Italian:
                    It = value;
                    return true;
                case DcLanguages.Hungarian:
                    Hu = value;
                    return true;
                case DcLanguages.Polish:
                    Pl = value;
                    return true;
                case DcLanguages.Portuguese:
                    Pt = value;
                    return true;
                case DcLanguages.Romanian:
                    Ro = value;
                    return true;
                case DcLanguages.Russian:
                    Ru = value;
                    return true;
                case DcLanguages.Serbian:
                    Sr = value;
                    return true;
                case DcLanguages.Turkish:
                    Tr = value;
                    return true;
                default:
                    return false;
            }
        }
    }
}
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace HappyTravel.Hiroshima.Common.Models.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    [Flags]
    public enum InCompanyPermissions
    {
        None = 1,
        ManagerInvitation = 2,
        ChangeManagerPermissions = 4,
        // All = 01111111111111111111111111111110. First bit is 0 because it is reserved for sign, last bit is 0, because "All" does not include "None"
        All = 2147483646
    }
}
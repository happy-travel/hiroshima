using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace HappyTravel.Hiroshima.Common.Models.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    [Flags]
    public enum ManagerPermissions
    {
        ManagerInvitation = 1,
        ChangeManagerPermissions = 2,
        // All = 01111111111111111111111111111111. First bit is 0 because it is reserved for sign
        All = 2147483647
    }
}
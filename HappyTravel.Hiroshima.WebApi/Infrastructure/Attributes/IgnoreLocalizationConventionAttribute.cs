using System;

namespace HappyTravel.Hiroshima.WebApi.Infrastructure.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class IgnoreLocalizationConventionAttribute: Attribute
    { }
}
using System.Globalization;
using Microsoft.AspNetCore.Mvc;

namespace HappyTravel.Hiroshima.Api.Controllers
{
    public abstract class BaseController : ControllerBase
    {
        protected string LanguageCode => CultureInfo.CurrentCulture.Name;
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using HappyTravel.EdoContracts.Accommodations.Enums;
using Hiroshima.Common.Models;
using Hiroshima.Common.Models.Enums;
using Hiroshima.DbData;
using Hiroshima.DbData.Models;
using Hiroshima.DbData.Models.Accommodation;
using Hiroshima.DbData.Models.Rooms;
using Hiroshima.DbData.Models.Rooms.Occupancy;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using Newtonsoft.Json;
using Location = Hiroshima.DbData.Models.Location;
using PropertyTypes = Hiroshima.Common.Models.Enums.PropertyTypes;

namespace Hiroshima.DirectContractsDataSeeder
{
    internal static class DataSeeder
    {
        internal static void AddData(DirectContractsDbContext dbContext)
        {
            AddLocations(dbContext);
            AddOneAndOnlyContract(dbContext);
            AddJumeriahContract(dbContext);
        }

        private static void AddLocations(DirectContractsDbContext dbContext)
        {
            #region AddLocation
            dbContext.Locations.AddRange(
                new Location.Location
                {
                    Id = 1,
                    Name = new MultiLanguage<string>
                    {
                        En = "The United Arab Emirates",
                        Ru = "Объединенные Арабские Эмираты",
                        Ar = "الإمارات العربية المتحدة"
                    },
                    CountryCode = "AE",
                    Type = Location.LocationTypes.Country,
                    ParentId = 0
                },
                new Location.Location
                {
                    Id = 2,
                    Name = new MultiLanguage<string>
                    {
                        Ar = "دبي", 
                        En = "Dubai",
                        Ru = "Дубай"
                    },
                    Type = Location.LocationTypes.City,
                    CountryCode = "AE",
                    ParentId = 1
                });
            #endregion
        }
        
        
        private static void AddOneAndOnlyContract(DirectContractsDbContext dbContext)
        {
            var accommodation = dbContext.Accommodations.FirstOrDefault(a => a.Name.En.Equals("ONE&ONLY ROYAL MIRAGE"));
            if (accommodation == null)
            {
                var hotelId = 1;

                #region AddAccommodation
                dbContext.Accommodations.Add(new Accommodation
                {
                    Id = hotelId,
                    Rating = AccommodationRating.FiveStars,
                    PropertyType = PropertyTypes.Hotel,
                    Name = new MultiLanguage<string>
                        {
                            Ar = "ون آند اونلي رويال ميراج",
                            En = "ONE&ONLY ROYAL MIRAGE",
                            Ru = "ONE&ONLY ROYAL MIRAGE"
                        },
                    TextualDescription =
                        new List<TextualDescription>
                        {
                            new TextualDescription
                            {
                                Description = new MultiLanguage<string>
                                {
                                    Ar =
                                        "65 فدان من الحدائق الغناء وكيلومتر من الشواطئ الخاصة تمثل أنموذجاً للسلاموالرخاء منقطعا النظير.",
                                    En =
                                        "Set in 65 acres of lush gardens and a kilometer of private beach, peaceful lives in remarkable opulence.",
                                    Ru =
                                        "26 гектаров ландшафтных садов, собственный пляж протяженностью в один километр, умиротворенная обстановка и роскошное окружение.",
                                }
                            }
                        },
                    Address = new MultiLanguage<string>
                    {
                        Ar = "شارع الملك سلمان بن عبدالعزيز آل سعود - دبي",
                        En = "King Salman Bin Abdulaziz Al Saud St - Dubai",
                        Ru = "King Salman Bin Abdulaziz Al Saud St - Dubai - ОАЭ"
                    },
                    Contacts = new Contacts {Email = "info@oneandonlythepalm.com", Phone = "+ 971 4 440 1010"},
                    AccommodationAmenities = new List<MultiLanguage<string>>
                    {
                        new MultiLanguage<string>
                        {
                            Ar =
                                @"يعتبر تناول الطعام في ون آند أونلي رويال ميراج متعة بحد ذاتها بفضل ثمانية مطاعم مختلفة في ذا بالاس والردهة العربية والإقامة والسبا. ويقدم كل منها تجربة فريدة لاكتشاف أشهى المأكولات في أجواء مميزة ومبتكرة. ويوفر ذا بالاس أربعة مطاعم مختلفة؛ حيث تُقدَّم القوائم المتوسطية في أوليفز، والمأكولات المغربية الاستثنائية في طاجين، والمأكولات العالمية الكلاسيكية في مطعم المشاهير، والسلطات الطازجة وثمار البحر في بار ومطعم الشاطئ للمشويات. ويمكن للضيوف اكتشاف أشهى النكهات الهندو-أوروبية في نينا، أو تذوق أصنافهم المفضلة طوال اليوم من أطباق الشرق الأوسط وشمال أوروبا في ذا روتيسيري، أو تناول الطعام في مجلس عائم مطل على الخليج العربي في مطعم أوزون الذي يقدم المأكولات العالمية بلمسة آسيوية. وتعرض قاعة الطعام قوائم ملهمة ومبتكرة حصريًا لضيوف المساكن والسبا.",
                            En =
                                @"Dining at One&Only Royal Mirage is a unique journey within eight different restaurants between The Palace, Arabian Court and Residence & Spa. Each venue presents an authentic culinary experience in a distinctive environment. The Palace offers four different restaurants, including Mediterranean flavours at Olives, exceptional Moroccan fare at Tagine, an elegant dining on European cuisine at Celebrities, and fresh grilled seafood and bright salads at The Beach Bar & Grill. Within Arabian Court, guests may discover seductive Indo-European flavours at Nina, savour all-day dining on traditional recipes from the Middle East and northern Europe at The Rotisserie, or experience the floating majlis overlooking the Arabian Gulf at Eauzone featuring international cuisine with an Asian twist. Exclusive to guests at Residence & Spa, The Dining Room showcases inspired menus of fresh creativity.",
                            Ru =
                                @"Ужин или обед на курорте One&Only Royal Mirage — это уникальное путешествие, в котором вас ждут восемь ресторанов, расположенных в корпусах The Palace, Arabian Court и Residence & Spa. Каждый из ресторанов дарит гостям уникальный гастрономический опыт в неповторимой обстановке. В корпусе The Palace расположены четыре ресторана, включая ресторан средиземноморской кухни Olives, ресторан марокканской кухни Tagine, ресторан Celebrities с элегантным интерьером и европейской кухней, а также гриль-бар Beach Bar & Grill, где вам предложат блюда из свежих морепродуктов на гриле и великолепные салаты. В корпусе Arabian Court гостей ждут соблазнительные блюда индоевропейской кухни ресторана Nina, а в ресторане The Rotisserie в течение всего дня можно оценить блюда, приготовленные по традиционным рецептам кухни Ближнего Востока и Северной Европы. В ресторане Eauzone можно удобно устроиться в плавающем меджлисе с видом на Персидский залив и заказать блюда международной кухни с азиатскими нотками. Ресторан Dining Room, обслуживающий исключительно гостей корпуса Residence & Spa, отличается творческим подходом к составлению меню и приготовлению блюд."
                        },
                        new MultiLanguage<string>
                        {
                            Ar =
                                @"تضم دبي 10 ملاعب جولف ضمن مسافة قريبة من ون آند أونلي رويال ميراج، ولا يبعد نادي الإمارات للجولف ونادي مونتغمري إلا بضع دقائق.",
                            En =
                                @"Dubai boasts 10 spectacular golf courses within easy reach of One&Only Royal Mirage, with Emirates Golf Club and Montgomerie just minutes away.",
                            Ru =
                                @"В Дубае, недалеко от курорта One&Only Royal Mirage, находится 10 роскошных полей для гольфа, а гольф-клубы Emirates Golf Club и Montgomerie также расположены в непосредственной близости."
                        },
                        new MultiLanguage<string>
                        {
                            Ar =
                                "ترتقي روحك وتسمو بينما تنغمس في ملذات التجارب الشاملة المصممة لاستعادة العقل والبدن والروح في منتجع ون آند أونلي سبا. ادخل الحمام الشرقي ودع بدنك يرتاح ببطء عبر الحرارة والبخار المتصاعد بينما تبث أصوات الماء المترقرقة الاسترخاء في العقل.",
                            En =
                                @"Spirits elevate as you indulge in a range of holistic experiences designed to restore mind, body and soul at One&Only Spa. At the Traditional Oriental Hammam, let the body ease slowly into the rising heat and steam whilst the sounds of rippling water relaxes the mind.",
                            Ru =
                                @"Процедуры для восстановления равновесия разума, тела и души в спа-центре One&Only обновят вашу жизненную энергию. Позвольте теплым парам традиционного восточного хаммама расслабить ваше тело, пока струящаяся вода успокаивает мысли."
                        }
                    },
                    Pictures = new List<Picture>
                    {
                        new Picture
                        {
                            Caption = new MultiLanguage<string>
                            {
                                Ar = "ون آند اونلي رويال ميراج", En = "ONE&ONLY ROYAL MIRAGE"
                            },
                            Source =
                                "https://encrypted-tbn0.gstatic.com/images?q=tbn%3AANd9GcQ6HkA4WClkmu3oOM0iuENG66UC6iKZNUrefe0iJ__MX5ZbValF"
                        }
                    },
                    CheckInTime = "13:00",
                    CheckOutTime = "11:00",
                    Coordinates = new Point(55.153219,25.097596),
                    LocationId = 2
                });
                #endregion
                dbContext.SaveChanges();
                #region AddRooms

                dbContext.Rooms.AddRange(
                    new Room
                    {
                        Id = 20,
                        AccommodationId = 1,
                        Name = new MultiLanguage<string>
                        {
                            En = "Palace Superior Deluxe Room",
                            Ru = "",
                            Ar = ""
                        },
                        Description = new MultiLanguage<string>
                        {
                            En = "",
                            Ru = "",
                            Ar = ""
                        },
                        Amenities = new List<MultiLanguage<string>>
                        {
                            new MultiLanguage<string>
                            {
                                En = "",
                                Ru = "",
                                Ar = "",
                            },
                            new MultiLanguage<string>
                            {
                                En = "",
                                Ru = "",
                                Ar = "",
                            }
                        },
                        #region PermittedOccupancies
                        PermittedOccupancies = new PermittedOccupancies
                        {
                            RoomOccupancies = new List<List<RoomOccupancy>>
                            {
                                new List<RoomOccupancy>
                                {
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 18,
                                            UpperBound = 190,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 2
                                    },
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 12,
                                            UpperBound = 16,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 1
                                    }
                                },
                                new List<RoomOccupancy>
                                {
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 18,
                                            UpperBound = 190,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 2
                                    },
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 3,
                                            UpperBound = 12,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = false
                                        },
                                        NumberOfPersons = 1
                                    },
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 0,
                                            UpperBound = 3,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = false
                                        },
                                        NumberOfPersons = 1
                                    }
                                }
                            }
                        }
                        #endregion
                    },
                    new Room
                    {
                        Id = 21,
                        AccommodationId = 1,
                        Name = new MultiLanguage<string>
                        {
                            En = "Palace Gold Club Room",
                            Ru = "",
                            Ar = ""
                        },
                        Description = new MultiLanguage<string>
                        {
                            En = "",
                            Ru = "",
                            Ar = ""
                        },
                        Amenities = new List<MultiLanguage<string>>
                        {
                            new MultiLanguage<string>
                            {
                                En = "",
                                Ru = "",
                                Ar = "",
                            },
                            new MultiLanguage<string>
                            {
                                En = "",
                                Ru = "",
                                Ar = "",
                            }
                        },
                         #region PermittedOccupancies
                        PermittedOccupancies = new PermittedOccupancies
                        {
                            RoomOccupancies = new List<List<RoomOccupancy>>
                            {
                                new List<RoomOccupancy>
                                {
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 18,
                                            UpperBound = 190,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 2
                                    },
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 12,
                                            UpperBound = 16,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 1
                                    }
                                },
                                new List<RoomOccupancy>
                                {
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 18,
                                            UpperBound = 190,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 2
                                    },
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 3,
                                            UpperBound = 12,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = false
                                        },
                                        NumberOfPersons = 1
                                    },
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 0,
                                            UpperBound = 3,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = false
                                        },
                                        NumberOfPersons = 1
                                    }
                                }
                            }
                        }
                        #endregion
                    },
                    new Room
                    {
                        Id = 22,
                        AccommodationId = 1,
                        Name = new MultiLanguage<string>
                        {
                            En = "Palace One Bedroom Executive Suite",
                            Ru = "",
                            Ar = ""
                        },
                        Description = new MultiLanguage<string>
                        {
                            En = "",
                            Ru = "",
                            Ar = ""
                        },
                        Amenities = new List<MultiLanguage<string>>
                        {
                            new MultiLanguage<string>
                            {
                                En = "",
                                Ru = "",
                                Ar = "",
                            },
                            new MultiLanguage<string>
                            {
                                En = "",
                                Ru = "",
                                Ar = "",
                            }
                        },
                         #region PermittedOccupancies
                        PermittedOccupancies = new PermittedOccupancies
                        {
                            RoomOccupancies = new List<List<RoomOccupancy>>
                            {
                                new List<RoomOccupancy>
                                {
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 18,
                                            UpperBound = 190,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 2
                                    },
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 12,
                                            UpperBound = 16,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 1
                                    }
                                },
                                new List<RoomOccupancy>
                                {
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 18,
                                            UpperBound = 190,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 2
                                    },
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 3,
                                            UpperBound = 12,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = false
                                        },
                                        NumberOfPersons = 1
                                    },
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 0,
                                            UpperBound = 3,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = false
                                        },
                                        NumberOfPersons = 1
                                    }
                                }
                            }
                        }
                        #endregion
                    }, new Room
                    {
                        Id = 23,
                        AccommodationId = 1,
                        Name = new MultiLanguage<string>
                        {
                            En = "Palace One Bedroom Gold Club Suite",
                            Ru = "",
                            Ar = ""
                        },
                        Description = new MultiLanguage<string>
                        {
                            En = "",
                            Ru = "",
                            Ar = ""
                        },
                        Amenities = new List<MultiLanguage<string>>
                        {
                            new MultiLanguage<string>
                            {
                                En = "",
                                Ru = "",
                                Ar = "",
                            },
                            new MultiLanguage<string>
                            {
                                En = "",
                                Ru = "",
                                Ar = "",
                            }
                        },
                         #region PermittedOccupancies
                        PermittedOccupancies = new PermittedOccupancies
                        {
                            RoomOccupancies = new List<List<RoomOccupancy>>
                            {
                                new List<RoomOccupancy>
                                {
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 18,
                                            UpperBound = 190,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 2
                                    },
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 12,
                                            UpperBound = 16,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 1
                                    }
                                },
                                new List<RoomOccupancy>
                                {
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 18,
                                            UpperBound = 190,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 2
                                    },
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 3,
                                            UpperBound = 12,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = false
                                        },
                                        NumberOfPersons = 1
                                    },
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 0,
                                            UpperBound = 3,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = false
                                        },
                                        NumberOfPersons = 1
                                    }
                                }
                            }
                        }
                        #endregion
                    },
                    new Room
                    {
                        Id = 24,
                        AccommodationId = 1,
                        Name = new MultiLanguage<string>
                        {
                            En = "Palace Two Bedroom Executive Suite",
                            Ru = "",
                            Ar = ""
                        },
                        Description = new MultiLanguage<string>
                        {
                            En = "",
                            Ru = "",
                            Ar = ""
                        },
                        Amenities = new List<MultiLanguage<string>>
                        {
                            new MultiLanguage<string>
                            {
                                En = "",
                                Ru = "",
                                Ar = "",
                            },
                            new MultiLanguage<string>
                            {
                                En = "",
                                Ru = "",
                                Ar = "",
                            }
                        },
                        #region PermittedOccupancies
                        PermittedOccupancies = new PermittedOccupancies
                        {
                            RoomOccupancies = new List<List<RoomOccupancy>>
                            {
                                new List<RoomOccupancy>
                                {
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 18,
                                            UpperBound = 190,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 4
                                    },
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 0,
                                            UpperBound = 16,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 1
                                    }
                                }
                            }
                        }
                        #endregion
                    },
                    new Room
                    {
                        Id = 25,
                        AccommodationId = 1,
                        Name = new MultiLanguage<string>
                        {
                            En = "Palace Two Bedroom Gold Club Suite",
                            Ru = "",
                            Ar = ""
                        },
                        Description = new MultiLanguage<string>
                        {
                            En = "",
                            Ru = "",
                            Ar = ""
                        },
                        Amenities = new List<MultiLanguage<string>>
                        {
                            new MultiLanguage<string>
                            {
                                En = "",
                                Ru = "",
                                Ar = "",
                            },
                            new MultiLanguage<string>
                            {
                                En = "",
                                Ru = "",
                                Ar = "",
                            }
                        },
                        #region PermittedOccupancies
                        PermittedOccupancies = new PermittedOccupancies
                        {
                            RoomOccupancies = new List<List<RoomOccupancy>>
                            {
                                new List<RoomOccupancy>
                                {
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 18,
                                            UpperBound = 190,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 4
                                    },
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 0,
                                            UpperBound = 16,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 1
                                    }
                                }
                            }
                        }
                        #endregion
                    },
                    new Room
                    {
                        Id = 26,
                        AccommodationId = 1,
                        Name = new MultiLanguage<string>
                        {
                            En = "Palace Two Bedroom Royal Suite",
                            Ru = "",
                            Ar = ""
                        },
                        Description = new MultiLanguage<string>
                        {
                            En = "",
                            Ru = "",
                            Ar = ""
                        },
                        Amenities = new List<MultiLanguage<string>>
                        {
                            new MultiLanguage<string>
                            {
                                En = "",
                                Ru = "",
                                Ar = "",
                            },
                            new MultiLanguage<string>
                            {
                                En = "",
                                Ru = "",
                                Ar = "",
                            }
                        },
                        #region PermittedOccupancies
                        PermittedOccupancies = new PermittedOccupancies
                        {
                            RoomOccupancies = new List<List<RoomOccupancy>>
                            {
                                new List<RoomOccupancy>
                                {
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 18,
                                            UpperBound = 190,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 4
                                    },
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 0,
                                            UpperBound = 16,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 1
                                    }
                                }
                            }
                        }
                        #endregion
                    },
                    new Room
                    {
                        Id = 27,
                        AccommodationId = 1,
                        Name = new MultiLanguage<string>
                        {
                            En = "Arabian Court Deluxe Room",
                            Ru = "",
                            Ar = ""
                        },
                        Description = new MultiLanguage<string>
                        {
                            En = "",
                            Ru = "",
                            Ar = ""
                        },
                        Amenities = new List<MultiLanguage<string>>
                        {
                            new MultiLanguage<string>
                            {
                                En = "",
                                Ru = "",
                                Ar = "",
                            },
                            new MultiLanguage<string>
                            {
                                En = "",
                                Ru = "",
                                Ar = "",
                            }
                        },
                         #region PermittedOccupancies
                        PermittedOccupancies = new PermittedOccupancies
                        {
                            RoomOccupancies = new List<List<RoomOccupancy>>
                            {
                                new List<RoomOccupancy>
                                {
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 18,
                                            UpperBound = 190,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 2
                                    },
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 12,
                                            UpperBound = 16,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 1
                                    }
                                },
                                new List<RoomOccupancy>
                                {
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 18,
                                            UpperBound = 190,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 2
                                    },
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 3,
                                            UpperBound = 12,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = false
                                        },
                                        NumberOfPersons = 1
                                    },
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 0,
                                            UpperBound = 3,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = false
                                        },
                                        NumberOfPersons = 1
                                    }
                                }
                            }
                        }
                        #endregion
                    },
                    new Room
                    {
                        Id = 28,
                        AccommodationId = 1,
                        Name = new MultiLanguage<string>
                        {
                            En = "Arabian Court Two Deluxe Rooms Family Accommodation",
                            Ru = "",
                            Ar = ""
                        },
                        Description = new MultiLanguage<string>
                        {
                            En = "",
                            Ru = "",
                            Ar = ""
                        },
                        Amenities = new List<MultiLanguage<string>>
                        {
                            new MultiLanguage<string>
                            {
                                En = "",
                                Ru = "",
                                Ar = "",
                            },
                            new MultiLanguage<string>
                            {
                                En = "",
                                Ru = "",
                                Ar = "",
                            }
                        },
                         #region PermittedOccupancies
                        PermittedOccupancies = new PermittedOccupancies
                        {
                            RoomOccupancies = new List<List<RoomOccupancy>>
                            {
                                new List<RoomOccupancy>
                                {
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 18,
                                            UpperBound = 190,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 3
                                    },
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 0,
                                            UpperBound = 12,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = false
                                        },
                                        NumberOfPersons = 3
                                    }
                                }
                            }
                        }
                        #endregion
                    },
                    new Room
                    {
                        Id = 29,
                        AccommodationId = 1,
                        Name = new MultiLanguage<string>
                        {
                            En = "Arabian Court One Bedroom Executive Suite",
                            Ru = "",
                            Ar = ""
                        },
                        Description = new MultiLanguage<string>
                        {
                            En = "",
                            Ru = "",
                            Ar = ""
                        },
                        Amenities = new List<MultiLanguage<string>>
                        {
                            new MultiLanguage<string>
                            {
                                En = "",
                                Ru = "",
                                Ar = "",
                            },
                            new MultiLanguage<string>
                            {
                                En = "",
                                Ru = "",
                                Ar = "",
                            }
                        },
                        #region PermittedOccupancies
                        PermittedOccupancies = new PermittedOccupancies
                        {
                            RoomOccupancies = new List<List<RoomOccupancy>>
                            {
                                new List<RoomOccupancy>
                                {
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 18,
                                            UpperBound = 190,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 2
                                    },
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 12,
                                            UpperBound = 16,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 1
                                    }
                                },
                                new List<RoomOccupancy>
                                {
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 18,
                                            UpperBound = 190,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 2
                                    },
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 3,
                                            UpperBound = 12,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = false
                                        },
                                        NumberOfPersons = 1
                                    },
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 0,
                                            UpperBound = 3,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = false
                                        },
                                        NumberOfPersons = 1
                                    }
                                }
                            }
                        }
                        #endregion
                    },
                    new Room
                    {
                        Id = 30,
                        AccommodationId = 1,
                        Name = new MultiLanguage<string>
                        {
                            En = "Arabian Court Two Bedroom Executive Suite",
                            Ru = "",
                            Ar = ""
                        },
                        Description = new MultiLanguage<string>
                        {
                            En = "",
                            Ru = "",
                            Ar = ""
                        },
                        Amenities = new List<MultiLanguage<string>>
                        {
                            new MultiLanguage<string>
                            {
                                En = "",
                                Ru = "",
                                Ar = "",
                            },
                            new MultiLanguage<string>
                            {
                                En = "",
                                Ru = "",
                                Ar = "",
                            }
                        },
                        #region PermittedOccupancies
                        PermittedOccupancies = new PermittedOccupancies
                        {
                            RoomOccupancies = new List<List<RoomOccupancy>>
                            {
                                new List<RoomOccupancy>
                                {
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 18,
                                            UpperBound = 190,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 4
                                    },
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 0,
                                            UpperBound = 16,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 1
                                    }
                                }
                            }
                        }
                        #endregion
                    },
                    new Room
                    {
                        Id = 31,
                        AccommodationId = 1,
                        Name = new MultiLanguage<string>
                        {
                            En = "Arabian Court Prince Suite",
                            Ru = "",
                            Ar = ""
                        },
                        Description = new MultiLanguage<string>
                        {
                            En = "",
                            Ru = "",
                            Ar = ""
                        },
                        Amenities = new List<MultiLanguage<string>>
                        {
                            new MultiLanguage<string>
                            {
                                En = "",
                                Ru = "",
                                Ar = "",
                            },
                            new MultiLanguage<string>
                            {
                                En = "",
                                Ru = "",
                                Ar = "",
                            }
                        },
                         #region PermittedOccupancies
                        PermittedOccupancies = new PermittedOccupancies
                        {
                            RoomOccupancies = new List<List<RoomOccupancy>>
                            {
                                new List<RoomOccupancy>
                                {
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 18,
                                            UpperBound = 190,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 2
                                    },
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 12,
                                            UpperBound = 16,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 1
                                    }
                                },
                                new List<RoomOccupancy>
                                {
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 18,
                                            UpperBound = 190,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 2
                                    },
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 3,
                                            UpperBound = 12,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = false
                                        },
                                        NumberOfPersons = 1
                                    },
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 0,
                                            UpperBound = 3,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = false
                                        },
                                        NumberOfPersons = 1
                                    }
                                }
                            }
                        }
                        #endregion
                    },
                    new Room
                    {
                        Id = 32,
                        AccommodationId = 1,
                        Name = new MultiLanguage<string>
                        {
                            En = "Residence Prestige Room",
                            Ru = "",
                            Ar = ""
                        },
                        Description = new MultiLanguage<string>
                        {
                            En = "",
                            Ru = "",
                            Ar = ""
                        },
                        Amenities = new List<MultiLanguage<string>>
                        {
                            new MultiLanguage<string>
                            {
                                En = "",
                                Ru = "",
                                Ar = "",
                            },
                            new MultiLanguage<string>
                            {
                                En = "",
                                Ru = "",
                                Ar = "",
                            }
                        },
                         #region PermittedOccupancies
                        PermittedOccupancies = new PermittedOccupancies
                        {
                            RoomOccupancies = new List<List<RoomOccupancy>>
                            {
                                new List<RoomOccupancy>
                                {
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 18,
                                            UpperBound = 190,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 2
                                    },
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 12,
                                            UpperBound = 16,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 1
                                    }
                                },
                                new List<RoomOccupancy>
                                {
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 18,
                                            UpperBound = 190,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 2
                                    },
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 3,
                                            UpperBound = 12,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = false
                                        },
                                        NumberOfPersons = 1
                                    },
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 0,
                                            UpperBound = 3,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = false
                                        },
                                        NumberOfPersons = 1
                                    }
                                }
                            }
                        }
                        #endregion
                    },
                    new Room
                    {
                        Id = 33,
                        AccommodationId = 1,
                        Name = new MultiLanguage<string>
                        {
                            En = "Residence Junior Suite",
                            Ru = "",
                            Ar = ""
                        },
                        Description = new MultiLanguage<string>
                        {
                            En = "",
                            Ru = "",
                            Ar = ""
                        },
                        Amenities = new List<MultiLanguage<string>>
                        {
                            new MultiLanguage<string>
                            {
                                En = "",
                                Ru = "",
                                Ar = "",
                            },
                            new MultiLanguage<string>
                            {
                                En = "",
                                Ru = "",
                                Ar = "",
                            }
                        },
                         #region PermittedOccupancies
                        PermittedOccupancies = new PermittedOccupancies
                        {
                            RoomOccupancies = new List<List<RoomOccupancy>>
                            {
                                new List<RoomOccupancy>
                                {
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 18,
                                            UpperBound = 190,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 2
                                    },
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 12,
                                            UpperBound = 16,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 1
                                    }
                                },
                                new List<RoomOccupancy>
                                {
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 18,
                                            UpperBound = 190,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 2
                                    },
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 3,
                                            UpperBound = 12,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = false
                                        },
                                        NumberOfPersons = 1
                                    },
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 0,
                                            UpperBound = 3,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = false
                                        },
                                        NumberOfPersons = 1
                                    }
                                }
                            }
                        }
                        #endregion
                    },
                    new Room
                    {
                        Id = 34,
                        AccommodationId = 1,
                        Name = new MultiLanguage<string>
                        {
                            En = "Residence Executive Suite",
                            Ru = "",
                            Ar = ""
                        },
                        Description = new MultiLanguage<string>
                        {
                            En = "",
                            Ru = "",
                            Ar = ""
                        },
                        Amenities = new List<MultiLanguage<string>>
                        {
                            new MultiLanguage<string>
                            {
                                En = "",
                                Ru = "",
                                Ar = "",
                            },
                            new MultiLanguage<string>
                            {
                                En = "",
                                Ru = "",
                                Ar = "",
                            }
                        },
                         #region PermittedOccupancies
                        PermittedOccupancies = new PermittedOccupancies
                        {
                            RoomOccupancies = new List<List<RoomOccupancy>>
                            {
                                new List<RoomOccupancy>
                                {
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 18,
                                            UpperBound = 190,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 2
                                    },
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 12,
                                            UpperBound = 16,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 1
                                    }
                                },
                                new List<RoomOccupancy>
                                {
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 18,
                                            UpperBound = 190,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 2
                                    },
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 3,
                                            UpperBound = 12,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = false
                                        },
                                        NumberOfPersons = 1
                                    },
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 0,
                                            UpperBound = 3,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = false
                                        },
                                        NumberOfPersons = 1
                                    }
                                }
                            }
                        }
                        #endregion
                    },
                    new Room
                    {
                        Id = 35,
                        AccommodationId = 1,
                        Name = new MultiLanguage<string>
                        {
                            En = "Residence Garden Beach Villa",
                            Ru = "",
                            Ar = ""
                        },
                        Description = new MultiLanguage<string>
                        {
                            En = "",
                            Ru = "",
                            Ar = ""
                        },
                        Amenities = new List<MultiLanguage<string>>
                        {
                            new MultiLanguage<string>
                            {
                                En = "",
                                Ru = "",
                                Ar = "",
                            },
                            new MultiLanguage<string>
                            {
                                En = "",
                                Ru = "",
                                Ar = "",
                            }
                        },
                         #region PermittedOccupancies
                        PermittedOccupancies = new PermittedOccupancies
                        {
                            RoomOccupancies = new List<List<RoomOccupancy>>
                            {
                                new List<RoomOccupancy>
                                {
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 18,
                                            UpperBound = 190,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 4
                                    },
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 0,
                                            UpperBound = 16,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = false
                                        },
                                        NumberOfPersons = 1
                                    }
                                }
                            }
                        }
                        #endregion
                    });

                #endregion
                dbContext.SaveChanges();
                #region AddRates
                FillRates(
                    dbContext,
                    new(DateTime , DateTime)[] { 
                        (new DateTime(2020,01,8), new DateTime(2020,01,14)),
                        (new DateTime(2020,01,25), new DateTime(2020,02,7)),
                        (new DateTime(2020,02,22), new DateTime(2020,03,20)),
                        (new DateTime(2020,04,19), new DateTime(2020,05,03))
                    },
                    new List<(int, decimal)>
                    {
                        (20, 2465),
                        (21, 3150),
                        (22, 5700),
                        (23, 6700),
                        (24, 8130),
                        (25, 9850),
                        (26, 2230),
                        (27, 2465),
                        (28, 4390),
                        (29, 6750),
                        (30, 9180),
                        (31, 9100),
                        (32, 3755),
                        (33, 6090),
                        (34, 8460),
                        (35, 30000)});
                FillRates(
                    dbContext,
                    new[] {  
                        (new DateTime(2020,01,15), new DateTime(2020,01,24)),
                        (new DateTime(2020,09,26), new DateTime(2020,10,09)),
                        (new DateTime(2020,12,5), new DateTime(2020,12,18)) 
                    },
                    new List<(int, decimal)>
                    {
                        (20, 2100),
                        (21, 2700),
                        (22, 4800),
                        (23, 5800),
                        (24, 6870),
                        (25, 8500),
                        (26, 20700),
                        (27, 2100),
                        (28, 4000),
                        (29, 5750),
                        (30, 7820),
                        (31, 7475),
                        (32, 3150),
                        (33, 5250),
                        (34, 6900),
                        (35, 29000)});
                FillRates(
                    dbContext,
                    new[]
                    {
                        (new DateTime(2020,02,8), new DateTime(2020,02,21)),
                        (new DateTime(2020,03,21), new DateTime(2020,03,27)),
                        (new DateTime(2020,04,13), new DateTime(2020,04,18)),
                        (new DateTime(2020,10,17), new DateTime(2020,10,23))
                    },
                    new List<(int, decimal)>
                    {
                        (20, 3170),
                        (21, 3780),
                        (22, 7000),
                        (23, 8450),
                        (24, 10200),
                        (25, 12230),
                        (26, 23100),
                        (27, 3170),
                        (28, 6000),
                        (29, 8450),
                        (30, 11600),
                        (31, 10000),
                        (32, 4300),
                        (33, 7000),
                        (34, 9800),
                        (35, 30000)});
                FillRates(
                    dbContext,
                    //new[] { 17, 26 },
                    new[] { 
                        (new DateTime(2020,03,28), new DateTime(2020,04,12)), 
                        (new DateTime(2020,10,24), new DateTime(2020,11,06))
                    },
                    new List<(int, decimal)>
                    {
                        (20, 3780),
                        (21, 4590),
                        (22, 8800),
                        (23, 10500),
                        (24, 12600),
                        (25, 15100),
                        (26, 26450),
                        (27, 3780),
                        (28, 7560),
                        (29, 10700),
                        (30, 14500),
                        (31, 12850),
                        (32, 5245),
                        (33, 8780),
                        (34, 11780),
                        (35, 36000)});
                FillRates(
                    dbContext,
                    //new[] { 20, 22 },
                    new [] 
                    {
                        (new DateTime(2020,05,04), new DateTime(2020,05,31)),
                        (new DateTime(2020,09,05), new DateTime(2020,09,25))
                    },
                    new List<(int, decimal)>
                    {
                        (20, 1830),
                        (21, 2405),
                        (22, 4200),
                        (23, 5200),
                        (24, 6000),
                        (25, 7600),
                        (26, 16600),
                        (27, 1830),
                        (28, 3650),
                        (29, 5000),
                        (30, 6800),
                        (31, 6500),
                        (32, 2835),
                        (33, 4700),
                        (34, 5400),
                        (35, 24000)});
                FillRates(
                    dbContext,
                    new [] 
                    {
                        (new DateTime(2020,06,1), new DateTime(2020,09,04))
                    },
                    new List<(int, decimal)>
                    {
                        (20, 1560),
                        (21, 2070),
                        (22, 3500),
                        (23, 4200),
                        (24, 5000),
                        (25, 6270),
                        (26, 16600),
                        (27, 1560),
                        (28, 3000),
                        (29, 4175),
                        (30, 5680),
                        (31, 5550),
                        (32, 2300),
                        (33, 3540),
                        (34, 4900),
                        (35, 24000)});
                FillRates(
                    dbContext,
                    //new[] { 24, 27, 29 },
                    new [] 
                    {
                        (new DateTime(2020,10,10), new DateTime(2020,10,16)),
                        (new DateTime(2020,11,07), new DateTime(2020,12,04)),
                        (new DateTime(2020,12,19), new DateTime(2020,12,25))
                    },
                    new List<(int, decimal)>
                    {
                        (20, 1560),
                        (21, 2070),
                        (22, 3500),
                        (23, 4200),
                        (24, 5000),
                        (25, 6270),
                        (26, 16600),
                        (27, 1560),
                        (28, 3000),
                        (29, 4175),
                        (30, 5680),
                        (31, 5550),
                        (32, 2300),
                        (33, 3540),
                        (34, 4900),
                        (35, 24000)});
                FillRates(
                    dbContext,
                    new [] 
                    {
                        (new DateTime(2020,12,26), new DateTime(2021,01,03))
                    },
                    new List<(int, decimal)>
                    {
                        (20, 4600),
                        (21, 5620),
                        (22, 9350),
                        (23, 11000),
                        (24, 13850),
                        (25, 16620),
                        (26, 28700),
                        (27, 4830),
                        (28, 9200),
                        (29, 11750),
                        (30, 16600),
                        (31, 14100),
                        (32, 6350),
                        (33, 9300),
                        (34, 11750),
                        (35, 38000)});
                FillRates(
                    dbContext,
                    //new[] { 31, 33, 35 },
                    new [] 
                    {
                        (new DateTime(2021,01,4), new DateTime(2021,01,10)),
                        (new DateTime(2021, 02,06), new DateTime(2021,02,19)),
                        (new DateTime(2021,03,20), new DateTime(2021,03,26))
                    },
                    new List<(int, decimal)>
                    {
                        (20, 3170),
                        (21, 3780),
                        (22, 7000),
                        (23, 8450),
                        (24, 10200),
                        (25, 12230),
                        (26, 23100),
                        (27, 3170),
                        (28, 6000),
                        (29, 8450),
                        (30, 11600),
                        (31, 10000),
                        (32, 4300),
                        (33, 7000),
                        (34, 9800),
                        (35, 3000)});
                FillRates(
                    dbContext,
                    //new[] { 32, 34 },
                    new [] 
                    {
                        (new DateTime(2021,01,11), new DateTime(2021,02,05)),
                        (new DateTime(2021, 02,20), new DateTime(2021,03,19))
                    },
                    new List<(int, decimal)>
                    {
                        (20, 2465),
                        (21, 3150),
                        (22, 5700),
                        (23, 6700),
                        (24, 8130),
                        (25, 9850),
                        (26, 22300),
                        (27, 2465),
                        (28, 4930),
                        (29, 6750),
                        (30, 9180),
                        (31, 9100),
                        (32, 3755),
                        (33, 6090),
                        (34, 8460),
                        (35, 30000)});
                FillRates(
                    dbContext,
                    new [] 
                    {
                        (new DateTime(2021,03,27), new DateTime(2021,04,10))
                    },
                    new List<(int, decimal)>
                    {
                        (20, 3780),
                        (21, 4590),
                        (22, 8800),
                        (23, 10500),
                        (24, 12600),
                        (25, 15100),
                        (26, 26450),
                        (27, 3780),
                        (28, 7560),
                        (29, 10700),
                        (30, 14500),
                        (31, 12850),
                        (32, 5245),
                        (33, 8780),
                        (34, 11780),
                        (35, 36000)});
                FillRates(
                    dbContext,
                    new [] 
                    {
                        (new DateTime(2021,04,11), new DateTime(2021,05,07))
                    },
                    new List<(int, decimal)>
                    {
                        (20, 2465),
                        (21, 3150),
                        (22, 5700),
                        (23, 6700),
                        (24, 8130),
                        (25, 9850),
                        (26, 22300),
                        (27, 2465),
                        (28, 4930),
                        (29, 6750),
                        (30, 9180),
                        (31, 9100),
                        (32, 3755),
                        (33, 6090),
                        (34, 8460),
                        (35, 30000)});
                #endregion
                dbContext.SaveChanges();
                #region AddPromotionalOffers
                var promotionalOffers = new[]
                {
                    new RoomPromotionalOffer
                    {
                        DiscountPercent = 25,
                        BookByDate = new DateTime(2019, 12, 13),
                        ValidFromDate = new DateTime(2020, 01, 08),
                        ValidToDate = new DateTime(2020, 02, 07),
                        BookingCode = "25% PROMO",
                        Details = new MultiLanguage<string>
                        {
                            En =
                                "Book on or before Friday, DECEMBER 13, 2019, receive 25% discount, combinable with complimentary dinner dine-around offer on applicable dates"
                        }
                    },
                    new RoomPromotionalOffer
                    {
                        DiscountPercent = 20,
                        BookByDate = new DateTime(2019, 12, 13),
                        ValidFromDate = new DateTime(2020, 02, 08),
                        ValidToDate = new DateTime(2020, 03, 20),
                        BookingCode = "20% PROMO",
                        Details = new MultiLanguage<string>
                        {
                            En =
                                "Book on or before Friday, DECEMBER 13, 2019, receive 20% discount, combinable with complimentary dinner dine-around offer on applicable dates"
                        }
                    },
                    new RoomPromotionalOffer
                    {
                        DiscountPercent = 25,
                        BookByDate = new DateTime(2019, 12, 13),
                        ValidFromDate = new DateTime(2020, 02, 08),
                        ValidToDate = new DateTime(2020, 03, 20),
                        BookingCode = "25% PROMO",
                        Details = new MultiLanguage<string>
                        {
                            En =
                                "Book on or before Friday, DECEMBER 13, 2019, receive 25% discount, combinable with complimentary dinner dine-around offer on applicable dates"
                        }
                    },
                    new RoomPromotionalOffer
                    {
                        DiscountPercent = 20,
                        BookByDate = new DateTime(2020, 02, 28),
                        ValidFromDate = new DateTime(2020, 03, 28),
                        ValidToDate = new DateTime(2020, 04, 03),
                        BookingCode = "20% PROMO",
                        Details = new MultiLanguage<string>
                        {
                            En =
                                "Book on or before Friday, FEBRUARY 28, 2020, receive 20% discount, combinable with complimentary dinner dine-around offer on applicable dates"
                        }
                    },
                    new RoomPromotionalOffer
                    {
                        DiscountPercent = 15,
                        BookByDate = new DateTime(2020, 02, 28),
                        ValidFromDate = new DateTime(2020, 04, 04),
                        ValidToDate = new DateTime(2020, 04, 12),
                        BookingCode = "15% PROMO",
                        Details = new MultiLanguage<string>
                        {
                            En =
                                "Book on or before Friday, FEBRUARY 28, 2020, receive 15% discount, combinable with complimentary dinner dine-around offer on applicable dates"
                        }
                    },
                    new RoomPromotionalOffer
                    {
                        DiscountPercent = 25,
                        BookByDate = new DateTime(2020, 02, 28),
                        ValidFromDate = new DateTime(2020, 04, 13),
                        ValidToDate = new DateTime(2020, 04, 18),
                        BookingCode = "25% PROMO",
                        Details = new MultiLanguage<string>
                        {
                            En =
                                "Book on or before Friday, FEBRUARY 28, 2020, receive 25% discount, combinable with complimentary dinner dine-around offer on applicable dates"
                        }
                    },
                    new RoomPromotionalOffer
                    {
                        DiscountPercent = 20,
                        BookByDate = new DateTime(2020, 02, 28),
                        ValidFromDate = new DateTime(2020, 04, 19),
                        ValidToDate = new DateTime(2020, 05, 03),
                        BookingCode = "20% PROMO",
                        Details = new MultiLanguage<string>
                        {
                            En =
                                "Book on or before Friday, FEBRUARY 28, 2020, receive 20% discount, combinable with complimentary dinner dine-around offer on applicable dates"
                        }
                    },
                    new RoomPromotionalOffer
                    {
                        DiscountPercent = 30,
                        BookByDate = new DateTime(2020, 03, 03),
                        ValidFromDate = new DateTime(2020, 05, 04),
                        ValidToDate = new DateTime(2020, 05, 31),
                        BookingCode = "30% PROMO",
                        Details = new MultiLanguage<string>
                        {
                            En =
                                "Book on or before Friday, APRIL 03, 2020, receive 30% discount, combinable with complimentary dinner dine-around offer on applicable dates"
                        }
                    },
                    new RoomPromotionalOffer
                    {
                        DiscountPercent = 30,
                        BookByDate = new DateTime(2020, 05, 08),
                        ValidFromDate = new DateTime(2020, 06, 01),
                        ValidToDate = new DateTime(2020, 09, 04),
                        BookingCode = "30% PROMO",
                        Details = new MultiLanguage<string>
                        {
                            En =
                                "Book on or before Friday, MAY 08, 2020, receive 30% discount, combinable with complimentary dinner dine-around offer on applicable dates"
                        }
                    },
                    new RoomPromotionalOffer
                    {
                        DiscountPercent = 30,
                        BookByDate = new DateTime(2020, 08, 14),
                        ValidFromDate = new DateTime(2020, 09, 05),
                        ValidToDate = new DateTime(2020, 09, 20),
                        BookingCode = "30% PROMO",
                        Details = new MultiLanguage<string>
                        {
                            En =
                                "Book on or before Friday, AUGUST 14, 2020, receive 30% discount, combinable with complimentary dinner dine-around offer on applicable dates"
                        }
                    },
                    new RoomPromotionalOffer
                    {
                        DiscountPercent = 10,
                        BookByDate = new DateTime(2020, 08, 14),
                        ValidFromDate = new DateTime(2020, 10, 17),
                        ValidToDate = new DateTime(2020, 11, 06),
                        BookingCode = "10% PROMO",
                        Details = new MultiLanguage<string>
                        {
                            En =
                                "Book on or before Friday, AUGUST 14, 2020, receive 10% discount, combinable with complimentary dinner dine-around offer on applicable dates"
                        }
                    },
                    new RoomPromotionalOffer
                    {
                        DiscountPercent = 20,
                        BookByDate = new DateTime(2020, 08, 14),
                        ValidFromDate = new DateTime(2020, 11, 07),
                        ValidToDate = new DateTime(2020, 12, 04),
                        BookingCode = "20% PROMO",
                        Details = new MultiLanguage<string>
                        {
                            En =
                                "Book on or before Friday, AUGUST 14, 2020, receive 20% discount, combinable with complimentary dinner dine-around offer on applicable dates"
                        }
                    },
                    new RoomPromotionalOffer
                    {
                        DiscountPercent = 30,
                        BookByDate = new DateTime(2020, 10, 02),
                        ValidFromDate = new DateTime(2020, 12, 05),
                        ValidToDate = new DateTime(2020, 12, 18),
                        BookingCode = "30% PROMO",
                        Details = new MultiLanguage<string>
                        {
                            En =
                                "Book on or before Friday, OCTOBER 02, 2020, receive 30% discount, combinable with complimentary dinner dine-around offer on applicable dates"
                        }
                    },
                    new RoomPromotionalOffer
                    {
                        DiscountPercent = 20,
                        BookByDate = new DateTime(2020, 11, 01),
                        ValidFromDate = new DateTime(2020, 12, 19),
                        ValidToDate = new DateTime(2020, 12, 25),
                        BookingCode = "20% PROMO",
                        Details = new MultiLanguage<string>
                        {
                            En =
                                "Book on or before Friday, NOVEMBER 01, 2020, receive 20% discount, combinable with complimentary dinner dine-around offer on applicable dates"
                        }
                    },
                    new RoomPromotionalOffer
                    {
                        DiscountPercent = 10,
                        BookByDate = new DateTime(2020, 08, 14),
                        ValidFromDate = new DateTime(2020, 12, 26),
                        ValidToDate = new DateTime(2021, 01, 03),
                        BookingCode = "10% PROMO",
                        Details = new MultiLanguage<string>
                        {
                            En =
                                "Book on or before Friday, AUGUST 14, 2020, receive 10% discount, combinable with complimentary dinner dine-around offer on applicable dates"
                        }
                    },
                    new RoomPromotionalOffer
                    {
                        DiscountPercent = 20,
                        BookByDate = new DateTime(2020, 11, 01),
                        ValidFromDate = new DateTime(2021, 01, 24),
                        ValidToDate = new DateTime(2021, 03, 26),
                        BookingCode = "20% PROMO",
                        Details = new MultiLanguage<string>
                        {
                            En =
                                "Book on or before Friday, NOVEMBER 01, 2020, receive 20% discount, combinable with complimentary dinner dine-around offer on applicable dates"
                        }
                    },
                    new RoomPromotionalOffer
                    {
                        DiscountPercent = 10,
                        BookByDate = new DateTime(2020, 11, 01),
                        ValidFromDate = new DateTime(2021, 03, 27),
                        ValidToDate = new DateTime(2021, 04, 10),
                        BookingCode = "10% PROMO",
                        Details = new MultiLanguage<string>
                        {
                            En =
                                "Book on or before Friday, NOVEMBER 01, 2020, receive 10% discount, combinable with complimentary dinner dine-around offer on applicable dates"
                        }
                    },
                    new RoomPromotionalOffer
                    {
                        DiscountPercent = 20,
                        BookByDate = new DateTime(2021, 02, 26),
                        ValidFromDate = new DateTime(2021, 04, 11),
                        ValidToDate = new DateTime(2021, 05, 07),
                        BookingCode = "10% PROMO",
                        Details = new MultiLanguage<string>
                        {
                            En =
                                "Book on or before Friday, FEBRUARY 26, 2021, receive 10% discount, combinable with complimentary dinner dine-around offer on applicable dates"
                        }
                    }
                };
                AddPromotionalOffers(dbContext, new[] {32, 33, 34, 35}, promotionalOffers);

                promotionalOffers = new[]
                {
                    new RoomPromotionalOffer
                    {
                        DiscountPercent = 30,
                        BookByDate = new DateTime(2019, 12, 13),
                        ValidFromDate = new DateTime(2020, 01, 08),
                        ValidToDate = new DateTime(2020, 02, 21),
                        BookingCode = "30% PROMO",
                        Details = new MultiLanguage<string>
                        {
                            En =
                                "Book on or before Friday, DECEMBER 13, 2019, receive 30% discount, combinable with complimentary dinner dine-around offer on applicable dates"
                        }
                    },
                    new RoomPromotionalOffer
                    {
                        DiscountPercent = 25,
                        BookByDate = new DateTime(2019, 12, 13),
                        ValidFromDate = new DateTime(2020, 02, 22),
                        ValidToDate = new DateTime(2020, 03, 20),
                        BookingCode = "25% PROMO",
                        Details = new MultiLanguage<string>
                        {
                            En =
                                "Book on or before Friday, DECEMBER 13, 2019, receive 25% discount, combinable with complimentary dinner dine-around offer on applicable dates"
                        }
                    },
                    new RoomPromotionalOffer
                    {
                        DiscountPercent = 35,
                        BookByDate = new DateTime(2019, 12, 13),
                        ValidFromDate = new DateTime(2020, 03, 21),
                        ValidToDate = new DateTime(2020, 03, 27),
                        BookingCode = "35% PROMO",
                        Details = new MultiLanguage<string>
                        {
                            En =
                                "Book on or before Friday, DECEMBER 13, 2019, receive 35% discount, combinable with complimentary dinner dine-around offer on applicable dates"
                        }
                    },
                    new RoomPromotionalOffer
                    {
                        DiscountPercent = 35,
                        BookByDate = new DateTime(2020, 02, 28),
                        ValidFromDate = new DateTime(2020, 03, 28),
                        ValidToDate = new DateTime(2020, 04, 03),
                        BookingCode = "35% PROMO",
                        Details = new MultiLanguage<string>
                        {
                            En =
                                "Book on or before Friday, FEBRUARY 28, 2020, receive 35% discount, combinable with complimentary dinner dine-around offer on applicable dates"
                        }
                    },
                    new RoomPromotionalOffer
                    {
                        DiscountPercent = 25,
                        BookByDate = new DateTime(2020, 02, 28),
                        ValidFromDate = new DateTime(2020, 04, 04),
                        ValidToDate = new DateTime(2020, 04, 12),
                        BookingCode = "25% PROMO",
                        Details = new MultiLanguage<string>
                        {
                            En =
                                "Book on or before Friday, FEBRUARY 28, 2020, receive 25% discount, combinable with complimentary dinner dine-around offer on applicable dates"
                        }
                    },
                    new RoomPromotionalOffer
                    {
                        DiscountPercent = 35,
                        BookByDate = new DateTime(2020, 02, 28),
                        ValidFromDate = new DateTime(2020, 04, 13),
                        ValidToDate = new DateTime(2020, 04, 18),
                        BookingCode = "35% PROMO",
                        Details = new MultiLanguage<string>
                        {
                            En =
                                "Book on or before Friday, FEBRUARY 28, 2020, receive 35% discount, combinable with complimentary dinner dine-around offer on applicable dates"
                        }
                    },
                    new RoomPromotionalOffer
                    {
                        DiscountPercent = 30,
                        BookByDate = new DateTime(2020, 02, 28),
                        ValidFromDate = new DateTime(2020, 04, 19),
                        ValidToDate = new DateTime(2020, 05, 03),
                        BookingCode = "30% PROMO",
                        Details = new MultiLanguage<string>
                        {
                            En =
                                "Book on or before Friday, FEBRUARY 28, 2020, receive 30% discount, combinable with complimentary dinner dine-around offer on applicable dates"
                        }
                    },
                    new RoomPromotionalOffer
                    {
                        DiscountPercent = 35,
                        BookByDate = new DateTime(2020, 04, 03),
                        ValidFromDate = new DateTime(2020, 05, 04),
                        ValidToDate = new DateTime(2020, 05, 31),
                        BookingCode = "35% PROMO",
                        Details = new MultiLanguage<string>
                        {
                            En =
                                "Book on or before Friday, APRIL 03, 2020, receive 35% discount, combinable with complimentary dinner dine-around offer on applicable dates"
                        }
                    },
                    new RoomPromotionalOffer
                    {
                        DiscountPercent = 30,
                        BookByDate = new DateTime(2020, 05, 08),
                        ValidFromDate = new DateTime(2020, 06, 01),
                        ValidToDate = new DateTime(2020, 09, 04),
                        BookingCode = "30% PROMO",
                        Details = new MultiLanguage<string>
                        {
                            En =
                                "Book on or before Friday, MAY 08, 2020, receive 30% discount, combinable with complimentary dinner dine-around offer on applicable dates"
                        }
                    },
                    new RoomPromotionalOffer
                    {
                        DiscountPercent = 30,
                        BookByDate = new DateTime(2020, 08, 14),
                        ValidFromDate = new DateTime(2020, 09, 05),
                        ValidToDate = new DateTime(2020, 09, 25),
                        BookingCode = "30% PROMO",
                        Details = new MultiLanguage<string>
                        {
                            En =
                                "Book on or before Friday, AUGUST 14, 2020, receive 30% discount, combinable with complimentary dinner dine-around offer on applicable dates"
                        }
                    },
                    new RoomPromotionalOffer
                    {
                        DiscountPercent = 30,
                        BookByDate = new DateTime(2020, 08, 14),
                        ValidFromDate = new DateTime(2020, 09, 26),
                        ValidToDate = new DateTime(2020, 10, 16),
                        BookingCode = "30% PROMO",
                        Details = new MultiLanguage<string>
                        {
                            En =
                                "Book on or before Friday, AUGUST 14, 2020, receive 30% discount, combinable with complimentary dinner dine-around offer on applicable dates"
                        }
                    },
                    new RoomPromotionalOffer
                    {
                        DiscountPercent = 20,
                        BookByDate = new DateTime(2020, 08, 14),
                        ValidFromDate = new DateTime(2020, 10, 17),
                        ValidToDate = new DateTime(2020, 12, 04),
                        BookingCode = "20% PROMO",
                        Details = new MultiLanguage<string>
                        {
                            En =
                                "Book on or before Friday, AUGUST 14, 2020, receive 20% discount, combinable with complimentary dinner dine-around offer on applicable dates"
                        }
                    },
                    new RoomPromotionalOffer
                    {
                        DiscountPercent = 30,
                        BookByDate = new DateTime(2020, 10, 02),
                        ValidFromDate = new DateTime(2020, 12, 05),
                        ValidToDate = new DateTime(2020, 12, 18),
                        BookingCode = "30% PROMO",
                        Details = new MultiLanguage<string>
                        {
                            En =
                                "Book on or before Friday, OCTOBER 02, 2020, receive 30% discount, combinable with complimentary dinner dine-around offer on applicable dates"
                        }
                    },
                    new RoomPromotionalOffer
                    {
                        DiscountPercent = 20,
                        BookByDate = new DateTime(2020, 11, 01),
                        ValidFromDate = new DateTime(2020, 12, 19),
                        ValidToDate = new DateTime(2020, 12, 25),
                        BookingCode = "20% PROMO",
                        Details = new MultiLanguage<string>
                        {
                            En =
                                "Book on or before Friday, NOVEMBER 01, 2020, receive 20% discount, combinable with complimentary dinner dine-around offer on applicable dates"
                        }
                    },
                    new RoomPromotionalOffer
                    {
                        DiscountPercent = 10,
                        BookByDate = new DateTime(2020, 11, 01),
                        ValidFromDate = new DateTime(2020, 12, 26),
                        ValidToDate = new DateTime(2021, 01, 03),
                        BookingCode = "10% PROMO",
                        Details = new MultiLanguage<string>
                        {
                            En =
                                "Book on or before Friday, NOVEMBER 01, 2020, receive 10% discount, combinable with complimentary dinner dine-around offer on applicable dates"
                        }
                    },
                    new RoomPromotionalOffer
                    {
                        DiscountPercent = 20,
                        BookByDate = new DateTime(2020, 11, 01),
                        ValidFromDate = new DateTime(2021, 01, 04),
                        ValidToDate = new DateTime(2021, 04, 10),
                        BookingCode = "20% PROMO",
                        Details = new MultiLanguage<string>
                        {
                            En =
                                "Book on or before Friday, NOVEMBER 01, 2020, receive 20% discount, combinable with complimentary dinner dine-around offer on applicable dates"
                        }
                    },
                    new RoomPromotionalOffer
                    {
                        DiscountPercent = 20,
                        BookByDate = new DateTime(2021, 02, 26),
                        ValidFromDate = new DateTime(2021, 04, 11),
                        ValidToDate = new DateTime(2021, 05, 07),
                        BookingCode = "20% PROMO",
                        Details = new MultiLanguage<string>
                        {
                            En =
                                "Book on or before Friday, FEBRUARY 26, 2020, receive 20% discount, combinable with complimentary dinner dine-around offer on applicable dates"
                        }
                    }
                };
                AddPromotionalOffers(dbContext, new[] {27, 28, 29, 30, 31}, promotionalOffers);

                promotionalOffers = new[]
                {
                    new RoomPromotionalOffer
                    {
                        DiscountPercent = 25,
                        BookByDate = new DateTime(2019, 12, 13),
                        ValidFromDate = new DateTime(2020, 01, 08),
                        ValidToDate = new DateTime(2020, 02, 21),
                        BookingCode = "25% PROMO",
                        Details = new MultiLanguage<string>
                        {
                            En =
                                "Book on or before Friday, DECEMBER 13, 2019, receive 25% discount, combinable with complimentary dinner dine-around offer on applicable dates"
                        }
                    },
                    new RoomPromotionalOffer
                    {
                        DiscountPercent = 20,
                        BookByDate = new DateTime(2019, 12, 13),
                        ValidFromDate = new DateTime(2020, 02, 22),
                        ValidToDate = new DateTime(2020, 03, 20),
                        BookingCode = "20% PROMO",
                        Details = new MultiLanguage<string>
                        {
                            En =
                                "Book on or before Friday, DECEMBER 13, 2019, receive 20% discount, combinable with complimentary dinner dine-around offer on applicable dates"
                        }
                    },
                    new RoomPromotionalOffer
                    {
                        DiscountPercent = 30,
                        BookByDate = new DateTime(2019, 12, 13),
                        ValidFromDate = new DateTime(2020, 03, 21),
                        ValidToDate = new DateTime(2020, 03, 27),
                        BookingCode = "30% PROMO",
                        Details = new MultiLanguage<string>
                        {
                            En =
                                "Book on or before Friday, DECEMBER 13, 2019, receive 30% discount, combinable with complimentary dinner dine-around offer on applicable dates"
                        }
                    },
                    new RoomPromotionalOffer
                    {
                        DiscountPercent = 20,
                        BookByDate = new DateTime(2020, 02, 28),
                        ValidFromDate = new DateTime(2020, 03, 28),
                        ValidToDate = new DateTime(2020, 04, 03),
                        BookingCode = "20% PROMO",
                        Details = new MultiLanguage<string>
                        {
                            En =
                                "Book on or before Friday, FEBRUARY 28, 2020, receive 20% discount, combinable with complimentary dinner dine-around offer on applicable dates"
                        }
                    },
                    new RoomPromotionalOffer
                    {
                        DiscountPercent = 20,
                        BookByDate = new DateTime(2020, 02, 28),
                        ValidFromDate = new DateTime(2020, 04, 04),
                        ValidToDate = new DateTime(2020, 04, 12),
                        BookingCode = "20% PROMO",
                        Details = new MultiLanguage<string>
                        {
                            En =
                                "Book on or before Friday, FEBRUARY 28, 2020, receive 20% discount, combinable with complimentary dinner dine-around offer on applicable dates"
                        }
                    },
                    new RoomPromotionalOffer
                    {
                        DiscountPercent = 30,
                        BookByDate = new DateTime(2020, 02, 28),
                        ValidFromDate = new DateTime(2020, 04, 13),
                        ValidToDate = new DateTime(2020, 04, 18),
                        BookingCode = "30% PROMO",
                        Details = new MultiLanguage<string>
                        {
                            En =
                                "Book on or before Friday, FEBRUARY 28, 2020, receive 30% discount, combinable with complimentary dinner dine-around offer on applicable dates"
                        }
                    },
                    new RoomPromotionalOffer
                    {
                        DiscountPercent = 25,
                        BookByDate = new DateTime(2020, 02, 28),
                        ValidFromDate = new DateTime(2020, 04, 19),
                        ValidToDate = new DateTime(2020, 05, 03),
                        BookingCode = "25% PROMO",
                        Details = new MultiLanguage<string>
                        {
                            En =
                                "Book on or before Friday, FEBRUARY 28, 2020, receive 25% discount, combinable with complimentary dinner dine-around offer on applicable dates"
                        }
                    },
                    new RoomPromotionalOffer
                    {
                        DiscountPercent = 30,
                        BookByDate = new DateTime(2020, 04, 03),
                        ValidFromDate = new DateTime(2020, 05, 04),
                        ValidToDate = new DateTime(2020, 05, 31),
                        BookingCode = "30% PROMO",
                        Details = new MultiLanguage<string>
                        {
                            En =
                                "Book on or before Friday, APRIL 04, 2020, receive 30% discount, combinable with complimentary dinner dine-around offer on applicable dates"
                        }
                    },
                    new RoomPromotionalOffer
                    {
                        DiscountPercent = 30,
                        BookByDate = new DateTime(2020, 04, 03),
                        ValidFromDate = new DateTime(2020, 06, 01),
                        ValidToDate = new DateTime(2020, 09, 04),
                        BookingCode = "30% PROMO",
                        Details = new MultiLanguage<string>
                        {
                            En =
                                "Book on or before Friday, MAY 08, 2020, receive 30% discount, combinable with complimentary dinner dine-around offer on applicable dates"
                        }
                    },
                    new RoomPromotionalOffer
                    {
                        DiscountPercent = 40,
                        BookByDate = new DateTime(2020, 08, 14),
                        ValidFromDate = new DateTime(2020, 09, 05),
                        ValidToDate = new DateTime(2020, 09, 25),
                        BookingCode = "40% PROMO",
                        Details = new MultiLanguage<string>
                        {
                            En =
                                "Book on or before Friday, AUGUST 14, 2020, receive 40% discount, combinable with complimentary dinner dine-around offer on applicable dates"
                        }
                    },
                    new RoomPromotionalOffer
                    {
                        DiscountPercent = 25,
                        BookByDate = new DateTime(2020, 08, 14),
                        ValidFromDate = new DateTime(2020, 09, 26),
                        ValidToDate = new DateTime(2020, 10, 16),
                        BookingCode = "25% PROMO",
                        Details = new MultiLanguage<string>
                        {
                            En =
                                "Book on or before Friday, AUGUST 14, 2020, receive 25% discount, combinable with complimentary dinner dine-around offer on applicable dates"
                        }
                    },
                    new RoomPromotionalOffer
                    {
                        DiscountPercent = 20,
                        BookByDate = new DateTime(2020, 08, 14),
                        ValidFromDate = new DateTime(2020, 10, 17),
                        ValidToDate = new DateTime(2020, 12, 04),
                        BookingCode = "20% PROMO",
                        Details = new MultiLanguage<string>
                        {
                            En =
                                "Book on or before Friday, AUGUST 14, 2020, receive 20% discount, combinable with complimentary dinner dine-around offer on applicable dates"
                        }
                    },
                    new RoomPromotionalOffer
                    {
                        DiscountPercent = 30,
                        BookByDate = new DateTime(2020, 10, 02),
                        ValidFromDate = new DateTime(2020, 12, 05),
                        ValidToDate = new DateTime(2020, 12, 18),
                        BookingCode = "30% PROMO",
                        Details = new MultiLanguage<string>
                        {
                            En =
                                "Book on or before Friday, OCTOBER 02, 2020, receive 30% discount, combinable with complimentary dinner dine-around offer on applicable dates"
                        }
                    },
                    new RoomPromotionalOffer
                    {
                        DiscountPercent = 20,
                        BookByDate = new DateTime(2020, 11, 01),
                        ValidFromDate = new DateTime(2020, 12, 19),
                        ValidToDate = new DateTime(2020, 12, 25),
                        BookingCode = "20% PROMO",
                        Details = new MultiLanguage<string>
                        {
                            En =
                                "Book on or before Friday, NOVEMBER 01, 2020, receive 20% discount, combinable with complimentary dinner dine-around offer on applicable dates"
                        }
                    },
                    new RoomPromotionalOffer
                    {
                        DiscountPercent = 10,
                        BookByDate = new DateTime(2020, 11, 01),
                        ValidFromDate = new DateTime(2020, 12, 26),
                        ValidToDate = new DateTime(2021, 01, 03),
                        BookingCode = "10% PROMO",
                        Details = new MultiLanguage<string>
                        {
                            En =
                                "Book on or before Friday, NOVEMBER 01, 2020, receive 10% discount, combinable with complimentary dinner dine-around offer on applicable dates"
                        }
                    },
                    new RoomPromotionalOffer
                    {
                        DiscountPercent = 20,
                        BookByDate = new DateTime(2020, 11, 01),
                        ValidFromDate = new DateTime(2021, 01, 04),
                        ValidToDate = new DateTime(2021, 04, 10),
                        BookingCode = "20% PROMO",
                        Details = new MultiLanguage<string>
                        {
                            En =
                                "Book on or before Friday, NOVEMBER 01, 2020, receive 20% discount, combinable with complimentary dinner dine-around offer on applicable dates"
                        }
                    },
                    new RoomPromotionalOffer
                    {
                        DiscountPercent = 20,
                        BookByDate = new DateTime(2021, 02, 26),
                        ValidFromDate = new DateTime(2021, 04, 11),
                        ValidToDate = new DateTime(2021, 05, 07),
                        BookingCode = "20% PROMO",
                        Details = new MultiLanguage<string>
                        {
                            En =
                                "Book on or before Friday, FEBRUARY 26, 2021, receive 20% discount, combinable with complimentary dinner dine-around offer on applicable dates"
                        }
                    }
                };
                AddPromotionalOffers(dbContext, new[] {20, 21, 22, 23, 24, 25, 26}, promotionalOffers);
                #endregion
                dbContext.SaveChanges();
                #region AddRoomAvailabilityRestrictions //Test data
                dbContext.RoomAvailabilityRestrictions.AddRange(new RoomAvailabilityRestrictions()
                {
                    RoomId = 20,
                    StartsFromDate = new DateTime(2020, 4, 1),
                    EndsToDate = new DateTime(2020, 4, 10),
                    Restrictions = SaleRestrictions.StopSale
                }, new RoomAvailabilityRestrictions
                {
                    RoomId = 20,
                    StartsFromDate = new DateTime(2020, 7, 10),
                    EndsToDate = new DateTime(2020, 7, 13)
                }, new RoomAvailabilityRestrictions
                {
                    RoomId = 20,
                    StartsFromDate = new DateTime(2020, 10, 15),
                    EndsToDate = new DateTime(2020, 10, 18)
                }, new RoomAvailabilityRestrictions
                {
                    RoomId = 20,
                    StartsFromDate = new DateTime(2021, 1, 10),
                    EndsToDate = new DateTime(2021, 1, 12)
                }, new RoomAvailabilityRestrictions
                {
                    RoomId = 21,
                    StartsFromDate = new DateTime(2020, 2, 5),
                    EndsToDate = new DateTime(2020, 2, 7)
                }, new RoomAvailabilityRestrictions
                {
                    RoomId = 21,
                    StartsFromDate = new DateTime(2020, 10, 15),
                    EndsToDate = new DateTime(2020, 10, 18)
                }, new RoomAvailabilityRestrictions
                {
                    RoomId = 21,
                    StartsFromDate = new DateTime(2020, 12, 28),
                    EndsToDate = new DateTime(2021, 1, 2)
                }, new RoomAvailabilityRestrictions
                {
                    RoomId = 22,
                    StartsFromDate = new DateTime(2020, 2, 5),
                    EndsToDate = new DateTime(2020, 2, 7)
                }, new RoomAvailabilityRestrictions
                {
                    RoomId = 22,
                    StartsFromDate = new DateTime(2020, 10, 15),
                    EndsToDate = new DateTime(2020, 10, 18)
                }, new RoomAvailabilityRestrictions
                {
                    RoomId = 22,
                    StartsFromDate = new DateTime(2020, 12, 28),
                    EndsToDate = new DateTime(2020, 12, 29)
                }, new RoomAvailabilityRestrictions
                {
                    RoomId = 23,
                    StartsFromDate = new DateTime(2020, 2, 5),
                    EndsToDate = new DateTime(2020, 2, 10)
                }, new RoomAvailabilityRestrictions
                {
                    RoomId = 23,
                    StartsFromDate = new DateTime(2020, 11, 1),
                    EndsToDate = new DateTime(2020, 11, 3)
                }, new RoomAvailabilityRestrictions
                {
                    RoomId = 23,
                    StartsFromDate = new DateTime(2020, 8, 7),
                    EndsToDate = new DateTime(2020, 8, 10)
                }, new RoomAvailabilityRestrictions
                {
                    RoomId = 24,
                    StartsFromDate = new DateTime(2020, 3, 8),
                    EndsToDate = new DateTime(2020, 3, 10)
                }, new RoomAvailabilityRestrictions
                {
                    RoomId = 24,
                    StartsFromDate = new DateTime(2020, 7, 1),
                    EndsToDate = new DateTime(2020, 7, 3)
                }, new RoomAvailabilityRestrictions
                {
                    RoomId = 24,
                    StartsFromDate = new DateTime(2020, 9, 3),
                    EndsToDate = new DateTime(2020, 9, 5)
                }, new RoomAvailabilityRestrictions
                {
                    RoomId = 25,
                    StartsFromDate = new DateTime(2020, 1, 29),
                    EndsToDate = new DateTime(2020, 2, 1)
                }, new RoomAvailabilityRestrictions
                {
                    RoomId = 25,
                    StartsFromDate = new DateTime(2020, 9, 15),
                    EndsToDate = new DateTime(2020, 9, 16)
                }, new RoomAvailabilityRestrictions
                {
                    RoomId = 25,
                    StartsFromDate = new DateTime(2020, 12, 30),
                    EndsToDate = new DateTime(2021, 1, 2)
                },
                new RoomAvailabilityRestrictions
                {
                    RoomId = 26,
                    StartsFromDate = new DateTime(2020, 1, 10),
                    EndsToDate = new DateTime(2020, 1, 12)
                }, new RoomAvailabilityRestrictions
                {
                    RoomId = 26,
                    StartsFromDate = new DateTime(2020, 3, 4),
                    EndsToDate = new DateTime(2020, 3, 7)
                }, new RoomAvailabilityRestrictions
                {
                    RoomId = 26,
                    StartsFromDate = new DateTime(2020, 6, 9),
                    EndsToDate = new DateTime(2020, 6, 11)
                }, new RoomAvailabilityRestrictions
                {
                    RoomId = 27,
                    StartsFromDate = new DateTime(2020, 1, 18),
                    EndsToDate = new DateTime(2020, 1, 19)
                }, new RoomAvailabilityRestrictions
                {
                    RoomId = 27,
                    StartsFromDate = new DateTime(2020, 2, 27),
                    EndsToDate = new DateTime(2020, 3, 1)
                }, new RoomAvailabilityRestrictions
                {
                    RoomId = 27,
                    StartsFromDate = new DateTime(2020, 12, 26),
                    EndsToDate = new DateTime(2020, 12, 27)
                }, new RoomAvailabilityRestrictions
                {
                    RoomId = 28,
                    StartsFromDate = new DateTime(2020, 4, 5),
                    EndsToDate = new DateTime(2020, 4, 7)
                }, new RoomAvailabilityRestrictions
                {
                    RoomId = 28,
                    StartsFromDate = new DateTime(2020, 11, 3),
                    EndsToDate = new DateTime(2020, 11, 5)
                }, new RoomAvailabilityRestrictions
                {
                    RoomId = 28,
                    StartsFromDate = new DateTime(2020, 12, 31),
                    EndsToDate = new DateTime(2021, 1, 1)
                }, new RoomAvailabilityRestrictions
                {
                    RoomId = 29,
                    StartsFromDate = new DateTime(2020, 2, 7),
                    EndsToDate = new DateTime(2020, 2, 10)
                }, new RoomAvailabilityRestrictions
                {
                    RoomId = 29,
                    StartsFromDate = new DateTime(2020, 4, 3),
                    EndsToDate = new DateTime(2020, 4, 5)
                }, new RoomAvailabilityRestrictions
                {
                    RoomId = 29,
                    StartsFromDate = new DateTime(2020, 12, 1),
                    EndsToDate = new DateTime(2021, 1, 1)
                }, new RoomAvailabilityRestrictions
                {
                    RoomId = 30,
                    StartsFromDate = new DateTime(2020, 6, 1),
                    EndsToDate = new DateTime(2020, 9, 1)
                }, new RoomAvailabilityRestrictions
                {
                    RoomId = 30,
                    StartsFromDate = new DateTime(2020, 9, 2),
                    EndsToDate = new DateTime(2020, 9, 4)
                }, new RoomAvailabilityRestrictions
                {
                    RoomId = 30,
                    StartsFromDate = new DateTime(2020, 12, 1),
                    EndsToDate = new DateTime(2021, 12, 2)
                }, new RoomAvailabilityRestrictions
                {
                    RoomId = 31,
                    StartsFromDate = new DateTime(2020, 1, 6),
                    EndsToDate = new DateTime(2020, 1, 8)
                }, new RoomAvailabilityRestrictions
                {
                    RoomId = 31,
                    StartsFromDate = new DateTime(2020, 2, 14),
                    EndsToDate = new DateTime(2020, 2, 16)
                }, new RoomAvailabilityRestrictions
                {
                    RoomId = 31,
                    StartsFromDate = new DateTime(2020, 7, 5),
                    EndsToDate = new DateTime(2021, 7, 8)
                }, new RoomAvailabilityRestrictions
                {
                    RoomId = 32,
                    StartsFromDate = new DateTime(2020, 9, 12),
                    EndsToDate = new DateTime(2020, 9, 15)
                }, new RoomAvailabilityRestrictions
                {
                    RoomId = 32,
                    StartsFromDate = new DateTime(2020, 10, 2),
                    EndsToDate = new DateTime(2020, 10, 4)
                }, new RoomAvailabilityRestrictions
                {
                    RoomId = 32,
                    StartsFromDate = new DateTime(2019, 12, 3),
                    EndsToDate = new DateTime(2019, 12, 4)
                }, new RoomAvailabilityRestrictions
                {
                    RoomId = 33,
                    StartsFromDate = new DateTime(2020, 3, 6),
                    EndsToDate = new DateTime(2020, 3, 8)
                }, new RoomAvailabilityRestrictions
                {
                    RoomId = 33,
                    StartsFromDate = new DateTime(2020, 4, 10),
                    EndsToDate = new DateTime(2020, 4, 13)
                }, new RoomAvailabilityRestrictions
                {
                    RoomId = 33,
                    StartsFromDate = new DateTime(2020, 7, 1),
                    EndsToDate = new DateTime(2020, 8, 1)
                },
                new RoomAvailabilityRestrictions
                {
                    RoomId = 34,
                    StartsFromDate = new DateTime(2020, 3, 3),
                    EndsToDate = new DateTime(2020, 3, 5)
                }, new RoomAvailabilityRestrictions
                {
                    RoomId = 34,
                    StartsFromDate = new DateTime(2020, 4, 16),
                    EndsToDate = new DateTime(2020, 4, 19)
                }, new RoomAvailabilityRestrictions
                {
                    RoomId = 34,
                    StartsFromDate = new DateTime(2020, 11, 23),
                    EndsToDate = new DateTime(2020, 11, 26)
                }, new RoomAvailabilityRestrictions
                {
                    RoomId = 35,
                    StartsFromDate = new DateTime(2020, 5, 1),
                    EndsToDate = new DateTime(2020, 5, 6)
                }, new RoomAvailabilityRestrictions
                {
                    RoomId = 35,
                    StartsFromDate = new DateTime(2020, 7, 19),
                    EndsToDate = new DateTime(2020, 7, 21)
                }, new RoomAvailabilityRestrictions
                {
                    RoomId = 35,
                    StartsFromDate = new DateTime(2020, 11, 28),
                    EndsToDate = new DateTime(2020, 12, 3)
                });
                #endregion
                dbContext.SaveChanges();
                #region AddAllocationRequirements
                var roomIds = new []{ 20, 21, 22, 23, 24, 25, 27, 28, 29, 30, 31, 32, 33, 34}; 
                var roomAllocationRequirements = new[]
                {
                    new RoomAllocationRequirement
                    {
                        ReleasePeriod = new ReleasePeriod { Days = 21 },
                        StartsFromDate = new DateTime(2020, 01, 08),
                        EndsToDate = new DateTime(2020, 01, 14)
                    },
                    new RoomAllocationRequirement
                    {
                        ReleasePeriod = new ReleasePeriod { Days = 21 },
                        StartsFromDate = new DateTime(2020, 01, 25),
                        EndsToDate = new DateTime(2020, 02, 07)
                    },
                    new RoomAllocationRequirement
                    {
                        ReleasePeriod = new ReleasePeriod { Days = 21 },
                        StartsFromDate = new DateTime(2020, 02, 22),
                        EndsToDate = new DateTime(2020, 03, 20)
                    },
                    new RoomAllocationRequirement
                    {
                        ReleasePeriod = new ReleasePeriod { Days = 21 },
                        StartsFromDate = new DateTime(2020, 04, 19),
                        EndsToDate = new DateTime(2020, 05, 03)
                    },
                    new RoomAllocationRequirement
                    {
                        ReleasePeriod = new ReleasePeriod { Days = 21 },
                        StartsFromDate = new DateTime(2020, 01, 15),
                        EndsToDate = new DateTime(2020, 01, 24)
                    },
                    new RoomAllocationRequirement
                    {
                        ReleasePeriod = new ReleasePeriod { Days = 21 },
                        StartsFromDate = new DateTime(2020, 09, 26),
                        EndsToDate = new DateTime(2020, 10, 09)
                    },
                    new RoomAllocationRequirement
                    {
                        ReleasePeriod = new ReleasePeriod { Days = 21 },
                        StartsFromDate = new DateTime(2020, 09, 26),
                        EndsToDate = new DateTime(2020, 10, 09)
                    }
                };
                AddRoomAllocationRequirements(dbContext, roomIds, roomAllocationRequirements);
                
                roomIds = new []{ 20, 21, 22, 23, 24, 25, 27, 28, 29, 30, 31};
                roomAllocationRequirements = new[]
                {
                    new RoomAllocationRequirement
                    {
                        ReleasePeriod = new ReleasePeriod { Days = 21 },
                        StartsFromDate = new DateTime(2020, 02, 08),
                        EndsToDate = new DateTime(2020, 02, 21)
                    },
                    new RoomAllocationRequirement
                    {
                        ReleasePeriod = new ReleasePeriod { Days = 21 },
                        StartsFromDate = new DateTime(2020, 03, 21),
                        EndsToDate = new DateTime(2020, 03, 27)
                    },
                    new RoomAllocationRequirement
                    {
                        ReleasePeriod = new ReleasePeriod { Days = 21 },
                        StartsFromDate = new DateTime(2020, 04, 13),
                        EndsToDate = new DateTime(2020, 04, 18)
                    }
                };
                AddRoomAllocationRequirements(dbContext, roomIds, roomAllocationRequirements);
                
                roomIds = new []{32, 33, 34};
                roomAllocationRequirements = new[]
                {
                    new RoomAllocationRequirement
                    {
                        ReleasePeriod = new ReleasePeriod { Days = 21 },
                        MinimumStayNights = 5,
                        StartsFromDate = new DateTime(2020, 02, 08),
                        EndsToDate = new DateTime(2020, 02, 21)
                    },
                    new RoomAllocationRequirement
                    {
                        ReleasePeriod = new ReleasePeriod { Days = 21 },
                        MinimumStayNights = 5,
                        StartsFromDate = new DateTime(2020, 03, 21),
                        EndsToDate = new DateTime(2020, 03, 27)
                    },
                    new RoomAllocationRequirement
                    {
                        ReleasePeriod = new ReleasePeriod { Days = 21 },
                        MinimumStayNights = 5,
                        StartsFromDate = new DateTime(2020, 04, 13),
                        EndsToDate = new DateTime(2020, 04, 18)
                    }
                };
                AddRoomAllocationRequirements(dbContext, roomIds, roomAllocationRequirements);
                
                roomIds = new []{20, 21, 22, 23, 24, 25, 27, 28, 29, 30, 31, 32, 33, 34};
                roomAllocationRequirements = new[]
                {
                    new RoomAllocationRequirement
                    {
                        ReleasePeriod = new ReleasePeriod { Days = 28 },
                        MinimumStayNights = 5,
                        StartsFromDate = new DateTime(2020, 03, 28),
                        EndsToDate = new DateTime(2020, 04, 12)
                    }
                };
                AddRoomAllocationRequirements(dbContext, roomIds, roomAllocationRequirements);
                
                roomIds = new []{20, 21, 22, 23, 24, 25, 27, 28, 29, 30, 31, 32, 33, 34};
                roomAllocationRequirements = new[]
                {
                    new RoomAllocationRequirement
                    {
                        ReleasePeriod = new ReleasePeriod { Days = 14 },
                        StartsFromDate = new DateTime(2020, 05, 04),
                        EndsToDate = new DateTime(2020, 05, 31)
                    },
                    new RoomAllocationRequirement
                    {
                        ReleasePeriod = new ReleasePeriod { Days = 14 },
                        StartsFromDate = new DateTime(2020, 09, 16),
                        EndsToDate = new DateTime(2020, 09, 25)
                    }
                };
                AddRoomAllocationRequirements(dbContext, roomIds, roomAllocationRequirements);
                
                roomIds = new []{20, 21, 22, 23, 24, 25, 27, 28, 29, 30, 31, 32, 33, 34};
                roomAllocationRequirements = new[]
                {
                    new RoomAllocationRequirement
                    {
                        ReleasePeriod = new ReleasePeriod { Days = 7 },
                        StartsFromDate = new DateTime(2020, 06, 01),
                        EndsToDate = new DateTime(2020, 06, 14)
                    }
                };
                AddRoomAllocationRequirements(dbContext, roomIds, roomAllocationRequirements);
                
                roomIds = new []{20, 21, 22, 23, 24, 25, 32, 33, 34};
                roomAllocationRequirements = new[]
                {
                    new RoomAllocationRequirement
                    {
                        ReleasePeriod = new ReleasePeriod { Days = 7 },
                        StartsFromDate = new DateTime(2020, 06, 15),
                        EndsToDate = new DateTime(2020, 09, 04)
                    }
                };
                AddRoomAllocationRequirements(dbContext, roomIds, roomAllocationRequirements);
                
                roomIds = new []{20, 21, 22, 23, 24, 25, 32, 33, 34};
                roomAllocationRequirements = new[]
                {
                    new RoomAllocationRequirement
                    {
                        ReleasePeriod = new ReleasePeriod { Days = 7 },
                        StartsFromDate = new DateTime(2020, 09, 05),
                        EndsToDate = new DateTime(2020, 09, 15)
                    }
                };
                AddRoomAllocationRequirements(dbContext, roomIds, roomAllocationRequirements);
                
                roomIds = new []{20, 21, 22, 23, 24, 25, 27, 28, 29, 30, 31, 32, 33, 34};
                roomAllocationRequirements = new[]
                {
                    new RoomAllocationRequirement
                    {
                        ReleasePeriod = new ReleasePeriod { Days = 7 },
                        StartsFromDate = new DateTime(2020, 10, 10),
                        EndsToDate = new DateTime(2020, 10, 16)
                    }
                };
                AddRoomAllocationRequirements(dbContext, roomIds, roomAllocationRequirements);
                
                roomAllocationRequirements = new[]
                {
                    new RoomAllocationRequirement
                    {
                        ReleasePeriod = new ReleasePeriod { Days = 7 },
                        MinimumStayNights = 3,
                        StartsFromDate = new DateTime(2020, 10, 17),
                        EndsToDate = new DateTime(2020, 10, 23)
                    }
                };
                AddRoomAllocationRequirements(dbContext, roomIds, roomAllocationRequirements);
                
                roomAllocationRequirements = new[]
                {
                    new RoomAllocationRequirement
                    {
                        ReleasePeriod = new ReleasePeriod { Days = 35 },
                        MinimumStayNights = 5,
                        StartsFromDate = new DateTime(2020, 10, 24),
                        EndsToDate = new DateTime(2020, 11, 06)
                    }
                };
                AddRoomAllocationRequirements(dbContext, roomIds, roomAllocationRequirements);
                
                roomAllocationRequirements = new[]
                {
                    new RoomAllocationRequirement
                    {
                        ReleasePeriod = new ReleasePeriod { Days = 28 },
                        MinimumStayNights = 3,
                        StartsFromDate = new DateTime(2020, 11, 07),
                        EndsToDate = new DateTime(2020, 12, 04)
                    },
                    new RoomAllocationRequirement
                    {
                        ReleasePeriod = new ReleasePeriod { Days = 28 },
                        MinimumStayNights = 3,
                        StartsFromDate = new DateTime(2020, 12, 19),
                        EndsToDate = new DateTime(2020, 12, 25)
                    }
                };
                AddRoomAllocationRequirements(dbContext, roomIds, roomAllocationRequirements);
                
                roomAllocationRequirements = new[]
                {
                    new RoomAllocationRequirement
                    {
                        ReleasePeriod = new ReleasePeriod { Days = 21 },
                        MinimumStayNights = 3,
                        StartsFromDate = new DateTime(2020, 12, 05),
                        EndsToDate = new DateTime(2020, 12, 18)
                    }
                };
                AddRoomAllocationRequirements(dbContext, roomIds, roomAllocationRequirements);
                
                roomAllocationRequirements = new[]
                {
                    new RoomAllocationRequirement
                    {
                        ReleasePeriod = new ReleasePeriod { Date = new DateTime(2020,11,01) },
                        MinimumStayNights = 7,
                        StartsFromDate = new DateTime(2020, 12, 26),
                        EndsToDate = new DateTime(2021, 01, 03)
                    }
                };
                AddRoomAllocationRequirements(dbContext, roomIds, roomAllocationRequirements);

                roomIds = new[] {20, 21, 22, 23, 24, 25, 27, 28, 29, 30, 31};
                roomAllocationRequirements = new[]
                {
                    new RoomAllocationRequirement
                    {
                        ReleasePeriod = new ReleasePeriod { Days = 28 },
                        MinimumStayNights = 3,
                        StartsFromDate = new DateTime(2021, 01, 04),
                        EndsToDate = new DateTime(2021, 01, 10)
                    },
                    new RoomAllocationRequirement
                    {
                        ReleasePeriod = new ReleasePeriod { Days = 28 },
                        MinimumStayNights = 3,
                        StartsFromDate = new DateTime(2021, 02, 06),
                        EndsToDate = new DateTime(2021, 02, 19)
                    },
                    new RoomAllocationRequirement
                    {
                        ReleasePeriod = new ReleasePeriod { Days = 28 },
                        MinimumStayNights = 3,
                        StartsFromDate = new DateTime(2021, 03, 21),
                        EndsToDate = new DateTime(2021, 03, 26)
                    }
                };
                AddRoomAllocationRequirements(dbContext, roomIds, roomAllocationRequirements);
                
                roomIds = new[] {32, 33, 34};
                roomAllocationRequirements = new[]
                {
                    new RoomAllocationRequirement
                    {
                        ReleasePeriod = new ReleasePeriod { Days = 28 },
                        MinimumStayNights = 5,
                        StartsFromDate = new DateTime(2021, 01, 04),
                        EndsToDate = new DateTime(2021, 01, 10)
                    },
                    new RoomAllocationRequirement
                    {
                        ReleasePeriod = new ReleasePeriod { Days = 28 },
                        MinimumStayNights = 5,
                        StartsFromDate = new DateTime(2021, 02, 06),
                        EndsToDate = new DateTime(2021, 02, 19)
                    },
                    new RoomAllocationRequirement
                    {
                        ReleasePeriod = new ReleasePeriod { Days = 28 },
                        MinimumStayNights = 5,
                        StartsFromDate = new DateTime(2021, 03, 21),
                        EndsToDate = new DateTime(2021, 03, 26)
                    }
                };
                AddRoomAllocationRequirements(dbContext, roomIds, roomAllocationRequirements);
                
                roomIds = new []{20, 21, 22, 23, 24, 25, 27, 28, 29, 30, 31, 32, 33, 34};
                roomAllocationRequirements = new[]
                {
                    new RoomAllocationRequirement
                    {
                        ReleasePeriod = new ReleasePeriod { Days = 28 },
                        MinimumStayNights = 3,
                        StartsFromDate = new DateTime(2021, 01, 11),
                        EndsToDate = new DateTime(2021, 02, 05)
                    },
                    new RoomAllocationRequirement
                    {
                        ReleasePeriod = new ReleasePeriod { Days = 28 },
                        MinimumStayNights = 3,
                        StartsFromDate = new DateTime(2021, 02, 20),
                        EndsToDate = new DateTime(2021, 03, 19)
                    }
                };
                AddRoomAllocationRequirements(dbContext, roomIds, roomAllocationRequirements);
                
                roomIds = new []{20, 21, 22, 23, 24, 25, 27, 28, 29, 30, 31, 32, 33, 34};
                roomAllocationRequirements = new[]
                {
                    new RoomAllocationRequirement
                    {
                        ReleasePeriod = new ReleasePeriod { Days = 35 },
                        MinimumStayNights = 5,
                        StartsFromDate = new DateTime(2021, 03, 27),
                        EndsToDate = new DateTime(2021, 04, 10)
                    }
                };
                AddRoomAllocationRequirements(dbContext, roomIds, roomAllocationRequirements);
                
                roomIds = new []{20, 21, 22, 23, 24, 25, 27, 28, 29, 30, 31, 32, 33, 34};
                roomAllocationRequirements = new[]
                {
                    new RoomAllocationRequirement
                    {
                        ReleasePeriod = new ReleasePeriod { Days = 21 },
                        StartsFromDate = new DateTime(2021, 04, 11),
                        EndsToDate = new DateTime(2021, 05, 07)
                    }
                };
                
                AddRoomAllocationRequirements(dbContext, roomIds, roomAllocationRequirements);
                #endregion
                dbContext.SaveChanges();
            }
        }

        
        private static void AddJumeriahContract(DirectContractsDbContext dbContext)
        {
            var accommodation = dbContext.Accommodations.FirstOrDefault(a => a.Name.En.Equals("Burj Al Arab Jumeirah"));
            if (accommodation == null)
            {
                var hotelId = 2;

                #region AddAccommodation
                dbContext.Accommodations.Add(new Accommodation
                {
                    Id = hotelId,
                    Rating = AccommodationRating.FiveStars,
                    PropertyType = PropertyTypes.Hotel,
                    Name = new MultiLanguage<string>
                    {
                        Ar = "برج العرب جميرا",
                        En = "Burj Al Arab Jumeirah",
                        Ru = "Burj Al Arab Jumeirah"
                    },
                    TextualDescription =
                        new List<TextualDescription>
                        {
                            new TextualDescription
                            {
                                Description = new MultiLanguage<string>
                                {
                                    Ar =
                                        "يقف فندق برج العرب جميرا الشهير شامخًا على شكل شراع وكأنه منارة دبي الحديثة، ويتسم بأجود وأرقى الضيافات التي يمكن أن تمر بها على الإطلاق.",
                                    En =
                                        "The iconic sail-shaped silhoutte of Burj Al Arab Jumeirah stands tall as a beacon of modern Dubai, characterized by the finest hospitality you can ever experience.",
                                    Ru =
                                        "Легендарный отель Burj Al Arab Jumeirah известен своим непревзойденным уровнем обслуживания и гостеприимства, а его высокий силуэт в форме паруса служит маяком современного Дубая.",
                                },
                                Type = TextualDescriptionTypes.General
                            }
                        },
                    Address = new MultiLanguage<string>
                    {
                        Ar = "شارع الملك سلمان بن عبدالعزيز آل سعود - دبي",
                        En = "King Salman Bin Abdulaziz Al Saud St - Dubai",
                        Ru = "King Salman Bin Abdulaziz Al Saud St - Dubai - ОАЭ"
                    },
                    Contacts = new Contacts
                    {
                        Email = "info@jumeirah.com",
                        Phone = "+971 4 3665000"
                    },
                    AccommodationAmenities = new List<MultiLanguage<string>>
                    {
                        new MultiLanguage<string>
                        {
                            Ar =
                                @"201 جناح دوبلكس فخم",
                            En =
                                @"201 luxurious duplex suites",
                            Ru =
                                @"201 роскошный двухэтажный номер люкс"
                        },
                        new MultiLanguage<string>
                        {
                            Ar =
                                @"تسعة مقاهي ومطاعم عالمية",
                            En =
                                @"Nine world-class restaurants and bars",
                            Ru =
                                @"Девять ресторанов и баров мирового класса"
                        },
                        new MultiLanguage<string>
                        {
                            Ar =
                                "خمسة مسابح (ثلاثة خارجيون، اثنان داخليان) وشاطئ خاص",
                            En =
                                @"Five swimming pools (three outdoor, two indoor) and a private beach",
                            Ru =
                                @"Пять плавательных бассейнов (три открытых и два крытых) и частный пляж"
                        }
                    },
                    Pictures = new List<Picture>
                    {
                        new Picture
                        {
                            Caption = new MultiLanguage<string>
                            {
                                Ar = "برج العرب جميرا", En = "Burj Al Arab Jumeirah"
                            },
                            Source =
                                "https://mediastream.jumeirah.com/webimage/image1152x648//globalassets/global/hotels-and-resorts/dubai/burj-al-arab/homepage-audit/burj-al-arab-jumeirah-terrace-hero.jpg"
                        }
                    },
                    CheckInTime = "14:00",
                    CheckOutTime = "12:00",
                    Coordinates = new Point(55.153219, 25.097596),
                    LocationId = 2
                });
                #endregion
                dbContext.SaveChanges();
                #region AddRooms
                dbContext.Rooms.AddRange(
                    new Room
                    {
                        Id = 71,
                        AccommodationId = 2,
                        Name = new MultiLanguage<string>
                        {
                            En = "One Bedroom Deluxe Suite",
                            Ru = "",
                            Ar = ""
                        },
                        Description = new MultiLanguage<string>
                        {
                            En = "",
                            Ru = "",
                            Ar = ""
                        },
                        Amenities = new List<MultiLanguage<string>>
                        {
                            new MultiLanguage<string>
                            {
                                En = "",
                                Ru = "",
                                Ar = "",
                            },
                            new MultiLanguage<string>
                            {
                                En = "",
                                Ru = "",
                                Ar = "",
                            }
                        },
                       
                        PermittedOccupancies = new PermittedOccupancies
                        {
                            RoomOccupancies = new List<List<RoomOccupancy>>
                            {
                                new List<RoomOccupancy>
                                {
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 12,
                                            UpperBound = 190,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 1
                                    }
                                },
                                new List<RoomOccupancy>
                                {
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 12,
                                            UpperBound = 190,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 2
                                    }
                                },
                                new List<RoomOccupancy>
                                {
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 12,
                                            UpperBound = 190,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 1
                                    },
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 4,
                                            UpperBound = 11,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 1
                                    }
                                },
                                new List<RoomOccupancy>
                                {
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 12,
                                            UpperBound = 190,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 2
                                    },
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 4,
                                            UpperBound = 11,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 1
                                    }
                                },
                                new List<RoomOccupancy>
                                {
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 12,
                                            UpperBound = 190,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 3
                                    }
                                },
                                new List<RoomOccupancy>
                                {
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 12,
                                            UpperBound = 190,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 1
                                    },
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 4,
                                            UpperBound = 11,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 2
                                    }
                                },
                                new List<RoomOccupancy>
                                {
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 12,
                                            UpperBound = 190,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 3
                                    },
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 4,
                                            UpperBound = 11,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 1
                                    }
                                },
                                new List<RoomOccupancy>
                                {
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 12,
                                            UpperBound = 190,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 2
                                    },
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 4,
                                            UpperBound = 11,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 2
                                    }
                                }
                            }
                        }
                    },
                    new Room
                    {
                        Id = 72,
                        AccommodationId = 2,
                        Name = new MultiLanguage<string>
                        {
                            En = "Panoramic One Bedroom Suite",
                            Ru = "",
                            Ar = ""
                        },
                        Description = new MultiLanguage<string>
                        {
                            En = "",
                            Ru = "",
                            Ar = ""
                        },
                        Amenities = new List<MultiLanguage<string>>
                        {
                            new MultiLanguage<string>
                            {
                                En = "",
                                Ru = "",
                                Ar = "",
                            },
                            new MultiLanguage<string>
                            {
                                En = "",
                                Ru = "",
                                Ar = "",
                            }
                        },
                       
                       PermittedOccupancies = new PermittedOccupancies
                        {
                            RoomOccupancies = new List<List<RoomOccupancy>>
                            {
                                new List<RoomOccupancy>
                                {
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 12,
                                            UpperBound = 190,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 1
                                    }
                                },
                                new List<RoomOccupancy>
                                {
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 12,
                                            UpperBound = 190,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 2
                                    }
                                },
                                new List<RoomOccupancy>
                                {
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 12,
                                            UpperBound = 190,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 1
                                    },
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 4,
                                            UpperBound = 11,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 1
                                    }
                                },
                                new List<RoomOccupancy>
                                {
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 12,
                                            UpperBound = 190,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 2
                                    },
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 4,
                                            UpperBound = 11,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 1
                                    }
                                },
                                new List<RoomOccupancy>
                                {
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 12,
                                            UpperBound = 190,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 3
                                    }
                                },
                                new List<RoomOccupancy>
                                {
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 12,
                                            UpperBound = 190,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 1
                                    },
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 4,
                                            UpperBound = 11,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 2
                                    }
                                },
                                new List<RoomOccupancy>
                                {
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 12,
                                            UpperBound = 190,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 3
                                    },
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 4,
                                            UpperBound = 11,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 1
                                    }
                                },
                                new List<RoomOccupancy>
                                {
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 12,
                                            UpperBound = 190,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 2
                                    },
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 4,
                                            UpperBound = 11,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 2
                                    }
                                }
                            }
                        }
                    },
                    new Room
                    {
                        Id = 73,
                        AccommodationId = 2,
                        Name = new MultiLanguage<string>
                        {
                            En = "Two Bedroom Delux Suite",
                            Ru = "",
                            Ar = ""
                        },
                        Description = new MultiLanguage<string>
                        {
                            En = "",
                            Ru = "",
                            Ar = ""
                        },
                        Amenities = new List<MultiLanguage<string>>
                        {
                            new MultiLanguage<string>
                            {
                                En = "",
                                Ru = "",
                                Ar = "",
                            },
                            new MultiLanguage<string>
                            {
                                En = "",
                                Ru = "",
                                Ar = "",
                            }
                        },
                       
                        PermittedOccupancies = new PermittedOccupancies
                        {
                            RoomOccupancies = new List<List<RoomOccupancy>>
                            {
                                new List<RoomOccupancy>
                                {
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 12,
                                            UpperBound = 190,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 1
                                    }
                                },
                                new List<RoomOccupancy>
                                {
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 12,
                                            UpperBound = 190,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 2
                                    }
                                },
                                new List<RoomOccupancy>
                                {
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 12,
                                            UpperBound = 190,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 1
                                    },
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 4,
                                            UpperBound = 11,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 1
                                    }
                                },
                                new List<RoomOccupancy>
                                {
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 12,
                                            UpperBound = 190,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 2
                                    },
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 4,
                                            UpperBound = 11,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 1
                                    }
                                },
                                new List<RoomOccupancy>
                                {
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 12,
                                            UpperBound = 190,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 3
                                    }
                                },
                                new List<RoomOccupancy>
                                {
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 12,
                                            UpperBound = 190,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 1
                                    },
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 4,
                                            UpperBound = 11,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 2
                                    }
                                },
                                new List<RoomOccupancy>
                                {
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 12,
                                            UpperBound = 190,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 4
                                    }
                                },
                                new List<RoomOccupancy>
                                {
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 12,
                                            UpperBound = 190,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 3
                                    },
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 4,
                                            UpperBound = 11,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 1
                                    }
                                },
                                new List<RoomOccupancy>
                                {
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 12,
                                            UpperBound = 190,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 2
                                    },
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 4,
                                            UpperBound = 11,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 2
                                    }
                                },
                                new List<RoomOccupancy>
                                {
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 12,
                                            UpperBound = 190,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 1
                                    },
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 4,
                                            UpperBound = 11,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 3
                                    }
                                },
                                new List<RoomOccupancy>
                                {
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 12,
                                            UpperBound = 190,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 5
                                    }
                                },
                                new List<RoomOccupancy>
                                {
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 12,
                                            UpperBound = 190,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 4
                                    },
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 4,
                                            UpperBound = 11,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 1
                                    }
                                },
                                new List<RoomOccupancy>
                                {
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 12,
                                            UpperBound = 190,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 3
                                    },
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 4,
                                            UpperBound = 11,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 2
                                    }
                                },
                                new List<RoomOccupancy>
                                {
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 12,
                                            UpperBound = 190,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 2
                                    },
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 4,
                                            UpperBound = 11,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 3
                                    }
                                },
                                new List<RoomOccupancy>
                                {
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 12,
                                            UpperBound = 190,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 1
                                    },
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 4,
                                            UpperBound = 11,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 4
                                    }
                                },
                                new List<RoomOccupancy>
                                {
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 12,
                                            UpperBound = 190,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 5
                                    },
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 4,
                                            UpperBound = 11,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 1
                                    }
                                },
                                new List<RoomOccupancy>
                                {
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 12,
                                            UpperBound = 190,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 4
                                    },
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 4,
                                            UpperBound = 11,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 2
                                    }
                                },
                                new List<RoomOccupancy>
                                {
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 12,
                                            UpperBound = 190,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 3
                                    },
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 4,
                                            UpperBound = 11,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 3
                                    }
                                },
                                new List<RoomOccupancy>
                                {
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 12,
                                            UpperBound = 190,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 2
                                    },
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 4,
                                            UpperBound = 11,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 4
                                    }
                                },
                                new List<RoomOccupancy>
                                {
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 12,
                                            UpperBound = 190,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 1
                                    },
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 4,
                                            UpperBound = 11,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 5
                                    }
                                }
                            }
                        }
                    },
                    new Room
                    {
                        Id = 74,
                        AccommodationId = 2,
                        Name = new MultiLanguage<string>
                        {
                            En = "Diplomatic Three Bedroom Suite",
                            Ru = "",
                            Ar = ""
                        },
                        Description = new MultiLanguage<string>
                        {
                            En = "",
                            Ru = "",
                            Ar = ""
                        },
                        Amenities = new List<MultiLanguage<string>>
                        {
                            new MultiLanguage<string>
                            {
                                En = "",
                                Ru = "",
                                Ar = ""
                            },
                            new MultiLanguage<string>
                            {
                                En = "",
                                Ru = "",
                                Ar = ""
                            }
                        },
                       
                        PermittedOccupancies = new PermittedOccupancies
                        {
                            RoomOccupancies = new List<List<RoomOccupancy>>
                            {
                                new List<RoomOccupancy>
                                {
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 12,
                                            UpperBound = 190,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 1
                                    }
                                },
                                new List<RoomOccupancy>
                                {
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 12,
                                            UpperBound = 190,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 2
                                    }
                                },
                                new List<RoomOccupancy>
                                {
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 12,
                                            UpperBound = 190,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 1
                                    },
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 4,
                                            UpperBound = 11,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 1
                                    }
                                },
                                new List<RoomOccupancy>
                                {
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 12,
                                            UpperBound = 190,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 2
                                    },
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 4,
                                            UpperBound = 11,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 1
                                    }
                                },
                                new List<RoomOccupancy>
                                {
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 12,
                                            UpperBound = 190,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 3
                                    }
                                },
                                new List<RoomOccupancy>
                                {
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 12,
                                            UpperBound = 190,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 1
                                    },
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 4,
                                            UpperBound = 11,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 2
                                    }
                                },
                                new List<RoomOccupancy>
                                {
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 12,
                                            UpperBound = 190,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 4
                                    }
                                },
                                new List<RoomOccupancy>
                                {
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 12,
                                            UpperBound = 190,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 3
                                    },
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 4,
                                            UpperBound = 11,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 1
                                    }
                                },
                                new List<RoomOccupancy>
                                {
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 12,
                                            UpperBound = 190,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 2
                                    },
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 4,
                                            UpperBound = 11,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 2
                                    }
                                },
                                new List<RoomOccupancy>
                                {
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 12,
                                            UpperBound = 190,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 1
                                    },
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 4,
                                            UpperBound = 11,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 3
                                    }
                                },
                                new List<RoomOccupancy>
                                {
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 12,
                                            UpperBound = 190,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 5
                                    }
                                },
                                new List<RoomOccupancy>
                                {
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 12,
                                            UpperBound = 190,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 4
                                    },
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 4,
                                            UpperBound = 11,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 1
                                    }
                                },
                                new List<RoomOccupancy>
                                {
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 12,
                                            UpperBound = 190,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 3
                                    },
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 4,
                                            UpperBound = 11,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 2
                                    }
                                },
                                new List<RoomOccupancy>
                                {
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 12,
                                            UpperBound = 190,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 2
                                    },
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 4,
                                            UpperBound = 11,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 3
                                    }
                                },
                                new List<RoomOccupancy>
                                {
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 12,
                                            UpperBound = 190,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 1
                                    },
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 4,
                                            UpperBound = 11,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 4
                                    }
                                },
                                new List<RoomOccupancy>
                                {
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 12,
                                            UpperBound = 190,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 6
                                    }
                                },
                                new List<RoomOccupancy>
                                {
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 12,
                                            UpperBound = 190,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 5
                                    },
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 4,
                                            UpperBound = 11,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 1
                                    }
                                },
                                new List<RoomOccupancy>
                                {
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 12,
                                            UpperBound = 190,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 4
                                    },
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 4,
                                            UpperBound = 11,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 2
                                    }
                                },
                                new List<RoomOccupancy>
                                {
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 12,
                                            UpperBound = 190,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 3
                                    },
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 4,
                                            UpperBound = 11,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 3
                                    }
                                },
                                new List<RoomOccupancy>
                                {
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 12,
                                            UpperBound = 190,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 2
                                    },
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 4,
                                            UpperBound = 11,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 4
                                    }
                                },
                                new List<RoomOccupancy>
                                {
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 12,
                                            UpperBound = 190,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 1
                                    },
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 4,
                                            UpperBound = 11,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 5
                                    }
                                },
                                new List<RoomOccupancy>
                                {
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 12,
                                            UpperBound = 190,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 7
                                    }
                                },
                                new List<RoomOccupancy>
                                {
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 12,
                                            UpperBound = 190,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 6
                                    },
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 4,
                                            UpperBound = 11,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 1
                                    }
                                },
                                new List<RoomOccupancy>
                                {
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 12,
                                            UpperBound = 190,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 5
                                    },
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 4,
                                            UpperBound = 11,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 2
                                    }
                                },
                                new List<RoomOccupancy>
                                {
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 12,
                                            UpperBound = 190,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 4
                                    },
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 4,
                                            UpperBound = 11,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 3
                                    }
                                },
                                new List<RoomOccupancy>
                                {
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 12,
                                            UpperBound = 190,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 3
                                    },
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 4,
                                            UpperBound = 11,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 4
                                    }
                                },
                                new List<RoomOccupancy>
                                {
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 12,
                                            UpperBound = 190,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 2
                                    },
                                    new RoomOccupancy
                                    {
                                        AgeRange = new AgeRange
                                        {
                                            LowerBound = 4,
                                            UpperBound = 11,
                                            LowerBoundInclusive = true,
                                            UpperBoundInclusive = true
                                        },
                                        NumberOfPersons = 5
                                    }
                                }
                            }
                        }
                    });

                #endregion
                dbContext.SaveChanges();
                #region AddRoomAllocationRequirements //Test data

                //Panoramic Suite
                dbContext.RoomAllocationRequirements.AddRange(new RoomAllocationRequirement
                    {
                        RoomId = 72,
                        StartsFromDate = new DateTime(2019, 9, 28),
                        EndsToDate = new DateTime(2019, 9, 28)
                    }, new RoomAllocationRequirement
                    {
                        RoomId = 72,
                        StartsFromDate = new DateTime(2019, 9, 30),
                        EndsToDate = new DateTime(2019, 9, 30)
                    },
                    new RoomAllocationRequirement
                    {
                        RoomId = 72,
                        StartsFromDate = new DateTime(2019, 10, 1),
                        EndsToDate = new DateTime(2019, 10, 1)
                    },
                    new RoomAllocationRequirement
                    {
                        RoomId = 73,
                        StartsFromDate = new DateTime(2019, 10, 11),
                        EndsToDate = new DateTime(2019, 10, 12)
                    },
                    new RoomAllocationRequirement
                    {
                        RoomId = 73,
                        StartsFromDate = new DateTime(2020, 10, 19),
                        EndsToDate = new DateTime(2020, 10, 19)
                    },
                    new RoomAllocationRequirement
                    {
                        RoomId = 73,
                        StartsFromDate = new DateTime(2020, 10, 26),
                        EndsToDate = new DateTime(2020, 10, 27)
                    });

                #endregion
                dbContext.SaveChanges();
                #region AddRates
                FillRates(
                    dbContext,
                    new[] {(new DateTime(2019,01,14), new DateTime(2019, 01, 31) )},
                    new List<(int, decimal)>
                    {
                        (71, 7321),
                        (72, 7809),
                        (73, 10981),
                        (74, 18302)
                    });

                FillRates(
                    dbContext,
                    new[] {(new DateTime(2019,02,01), new DateTime(2019, 02, 04) )},
                    new List<(int, decimal)>
                    {
                        (71, 9273),
                        (72, 10005),
                        (73, 13909),
                        (74, 23182)
                    });

                FillRates(
                    dbContext,
                    new[] {(new DateTime(2019,02,05), new DateTime(2019, 02, 11) )},
                    new List<(int, decimal)>
                    {
                        (71, 11225),
                        (72, 12201),
                        (73, 22450),
                        (74, 33675)
                    });

                FillRates(
                    dbContext,
                    new[] {(new DateTime(2019,02,12), new DateTime(2019, 03, 27) )},
                    new List<(int, decimal)>
                    {
                        (71, 9273),
                        (72, 10005),
                        (73, 13909),
                        (74, 23182)
                    });

                FillRates(
                    dbContext,
                    new[] {(new DateTime(2019,03,28), new DateTime(2019, 04, 21) )},
                    new List<(int, decimal)>
                    {
                        (71, 11225),
                        (72, 12201),
                        (73, 22450),
                        (74, 33675)
                    });

                FillRates(
                    dbContext,
                    new[] {(new DateTime(2019,04,22), new DateTime(2019, 05, 05) )},
                    new List<(int, decimal)>
                    {
                        (71, 9273),
                        (72, 10005),
                        (73, 13909),
                        (74, 23182)
                    });

                FillRates(
                    dbContext,
                    new[] {(new DateTime(2019,05,06), new DateTime(2019, 08, 31) )},
                    new List<(int, decimal)>
                    {
                        (71, 6345),
                        (72, 6833),
                        (73, 9517),
                        (74, 15862)
                    });

                FillRates(
                    dbContext,
                    new[] {(new DateTime(2019,09,01), new DateTime(2019, 10, 12) )},
                    new List<(int, decimal)>
                    {
                        (71, 7321),
                        (72, 7809),
                        (73, 10981),
                        (74, 18302)
                    });

                FillRates(
                    dbContext,
                    new[] {(new DateTime(2019,10,13), new DateTime(2019, 10, 19) )},
                    new List<(int, decimal)>
                    {
                        (71, 9273),
                        (72, 10005),
                        (73, 13909),
                        (74, 23182)
                    });

                FillRates(
                    dbContext,
                    new[] {(new DateTime(2019,10,20), new DateTime(2019, 11, 09) )},
                    new List<(int, decimal)>
                    {
                        (71, 11225),
                        (72, 12201),
                        (73, 22450),
                        (74, 33675)
                    });

                FillRates(
                    dbContext,
                    new[] {(new DateTime(2019,11,10), new DateTime(2019, 12, 01) )},
                    new List<(int, decimal)>
                    {
                        (71, 9273),
                        (72, 10005),
                        (73, 13909),
                        (74, 23182)
                    });

                FillRates(
                    dbContext,
                    new[] {(new DateTime(2019,12,02), new DateTime(2019, 12, 21) )},
                    new List<(int, decimal)>
                    {
                        (71, 7321),
                        (72, 7809),
                        (73, 10981),
                        (74, 18302)
                    });

                FillRates(
                    dbContext,
                    new[] {(new DateTime(2019,12,22), new DateTime(2019, 12, 26) )},
                    new List<(int, decimal)>
                    {
                        (71, 9273),
                        (72, 10005),
                        (73, 13909),
                        (74, 23182)
                    });

                FillRates(
                    dbContext,
                    new[] {(new DateTime(2019,12,27), new DateTime(2020, 01, 04) )},
                    new List<(int, decimal)>
                    {
                        (71, 13177),
                        (72, 14153),
                        (73, 26355),
                        (74, 39532)
                    });

                FillRates(
                    dbContext,
                    new[] {(new DateTime(2020,01,05), new DateTime(2020, 01, 13) )},
                    new List<(int, decimal)>
                    {
                        (71, 9273),
                        (72, 10005),
                        (73, 13909),
                        (74, 23182)
                    });
                #endregion
                dbContext.SaveChanges();
                #region AddPromotionalOffers
                var promotionalOffers = new[]
                {
                    new RoomPromotionalOffer
                    {
                        DiscountPercent = 20,
                        BookByDate = new DateTime(2018, 11, 29),
                        ValidFromDate = new DateTime(2019, 01, 08),
                        ValidToDate = new DateTime(2019, 01, 31),
                        BookingCode = "WWHL600",
                        Details = new MultiLanguage<string> {En = ""}
                    }
                };
                AddPromotionalOffers(dbContext, new[] {71, 72, 73, 74}, promotionalOffers);
                promotionalOffers = new[]
                {
                    new RoomPromotionalOffer
                    {
                        DiscountPercent = 25,
                        BookByDate = new DateTime(2018, 11, 29),
                        ValidFromDate = new DateTime(2019, 02, 01),
                        ValidToDate = new DateTime(2019, 02, 04),
                        BookingCode = "WWHL601",
                        Details = new MultiLanguage<string> {En = ""}
                    }
                };
                AddPromotionalOffers(dbContext, new[] {71, 72, 73, 74}, promotionalOffers);
                
                promotionalOffers = new[]
                {
                    new RoomPromotionalOffer
                    {
                        DiscountPercent = 20,
                        BookByDate = new DateTime(2018, 11, 29),
                        ValidFromDate = new DateTime(2019, 02, 05),
                        ValidToDate = new DateTime(2019, 02, 11),
                        BookingCode = "WWHL602",
                        Details = new MultiLanguage<string> {En = ""}
                    }
                };
                AddPromotionalOffers(dbContext, new[] {71, 72, 73, 74}, promotionalOffers);
                
                promotionalOffers = new[]
                {
                    new RoomPromotionalOffer
                    {
                        DiscountPercent = 30,
                        BookByDate = new DateTime(2018, 11, 29),
                        ValidFromDate = new DateTime(2019, 02, 12),
                        ValidToDate = new DateTime(2019, 03, 27),
                        BookingCode = "WWHL603",
                        Details = new MultiLanguage<string> {En = ""}
                    }
                };
                AddPromotionalOffers(dbContext, new[] {71, 72, 73, 74}, promotionalOffers);
                
                promotionalOffers = new[]
                {
                    new RoomPromotionalOffer
                    {
                        DiscountPercent = 30,
                        BookByDate = new DateTime(2019, 01, 31),
                        ValidFromDate = new DateTime(2019, 03, 28),
                        ValidToDate = new DateTime(2019, 04, 21),
                        BookingCode = "WWHL604",
                        Details = new MultiLanguage<string> {En = ""}
                    }
                };
                AddPromotionalOffers(dbContext, new[] {71, 72, 73, 74}, promotionalOffers);
                
                promotionalOffers = new[]
                {
                    new RoomPromotionalOffer
                    {
                        DiscountPercent = 30,
                        BookByDate = new DateTime(2019, 01, 31),
                        ValidFromDate = new DateTime(2019, 03, 22),
                        ValidToDate = new DateTime(2019, 05, 05),
                        BookingCode = "WWHL605",
                        Details = new MultiLanguage<string> {En = ""}
                    }
                };
                AddPromotionalOffers(dbContext, new[] {71, 72, 73, 74}, promotionalOffers);
                
                promotionalOffers = new[]
                {
                    new RoomPromotionalOffer
                    {
                        DiscountPercent = 30,
                        BookByDate = new DateTime(2019, 02, 28),
                        ValidFromDate = new DateTime(2019, 05, 06),
                        ValidToDate = new DateTime(2019, 08, 31),
                        BookingCode = "WWHL606",
                        Details = new MultiLanguage<string> {En = ""}
                    }
                };
                AddPromotionalOffers(dbContext, new[] {71, 72, 73, 74}, promotionalOffers);
                
                promotionalOffers = new[]
                {
                    new RoomPromotionalOffer
                    {
                        DiscountPercent = 25,
                        BookByDate = new DateTime(2019, 02, 28),
                        ValidFromDate = new DateTime(2019, 09, 01),
                        ValidToDate = new DateTime(2019, 10, 12),
                        BookingCode = "WWHL607",
                        Details = new MultiLanguage<string> {En = ""}
                    }
                };
                AddPromotionalOffers(dbContext, new[] {71, 72, 73, 74}, promotionalOffers);
                
                promotionalOffers = new[]
                {
                    new RoomPromotionalOffer
                    {
                        DiscountPercent = 25,
                        BookByDate = new DateTime(2019, 05, 30),
                        ValidFromDate = new DateTime(2019, 10, 13),
                        ValidToDate = new DateTime(2019, 10, 19),
                        BookingCode = "WWHL608",
                        Details = new MultiLanguage<string> {En = ""}
                    }
                };
                AddPromotionalOffers(dbContext, new[] {71, 72, 73, 74}, promotionalOffers);
                
                promotionalOffers = new[]
                {
                    new RoomPromotionalOffer
                    {
                        DiscountPercent = 30,
                        BookByDate = new DateTime(2019, 07, 31),
                        ValidFromDate = new DateTime(2019, 10, 20),
                        ValidToDate = new DateTime(2019, 11, 09),
                        BookingCode = "WWHL609",
                        Details = new MultiLanguage<string> {En = ""}
                    }
                };
                AddPromotionalOffers(dbContext, new[] {71, 72, 73, 74}, promotionalOffers);
                
                promotionalOffers = new[]
                {
                    new RoomPromotionalOffer
                    {
                        DiscountPercent = 25,
                        BookByDate = new DateTime(2019, 07, 31),
                        ValidFromDate = new DateTime(2019, 11, 10),
                        ValidToDate = new DateTime(2019, 12, 01),
                        BookingCode = "WWHL610",
                        Details = new MultiLanguage<string> {En = ""}
                    }
                };
                AddPromotionalOffers(dbContext, new[] {71, 72, 73, 74}, promotionalOffers);
                
                promotionalOffers = new[]
                {
                    new RoomPromotionalOffer
                    {
                        DiscountPercent = 25,
                        BookByDate = new DateTime(2019, 09, 30),
                        ValidFromDate = new DateTime(2019, 12, 02),
                        ValidToDate = new DateTime(2019, 12, 21),
                        BookingCode = "WWHL611",
                        Details = new MultiLanguage<string> {En = ""}
                    }
                };
                AddPromotionalOffers(dbContext, new[] {71, 72, 73, 74}, promotionalOffers);
                
                promotionalOffers = new[]
                {
                    new RoomPromotionalOffer
                    {
                        DiscountPercent = 15,
                        BookByDate = new DateTime(2019, 09, 30),
                        ValidFromDate = new DateTime(2019, 12, 22),
                        ValidToDate = new DateTime(2019, 12, 26),
                        BookingCode = "WWHL612",
                        Details = new MultiLanguage<string> {En = ""}
                    }
                };
                AddPromotionalOffers(dbContext, new[] {71, 72, 73, 74}, promotionalOffers);
                
                promotionalOffers = new[]
                {
                    new RoomPromotionalOffer
                    {
                        DiscountPercent = 20,
                        BookByDate = new DateTime(2019, 09, 30),
                        ValidFromDate = new DateTime(2019, 12, 27),
                        ValidToDate = new DateTime(2020, 01, 04),
                        BookingCode = "WWHL613",
                        Details = new MultiLanguage<string> {En = ""}
                    }
                };
                AddPromotionalOffers(dbContext, new[] {71, 72, 73, 74}, promotionalOffers);
                
                promotionalOffers = new[]
                {
                    new RoomPromotionalOffer
                    {
                        DiscountPercent = 20,
                        BookByDate = new DateTime(2019, 09, 30),
                        ValidFromDate = new DateTime(2020, 01, 05),
                        ValidToDate = new DateTime(2020, 01, 13),
                        BookingCode = "WWHL614",
                        Details = new MultiLanguage<string> {En = ""}
                    }
                };
                AddPromotionalOffers(dbContext, new[] {71, 72, 73, 74}, promotionalOffers);
                #endregion
                dbContext.SaveChanges();
                #region AddAllocationRequirements
                var roomIds = new []{ 71, 72, 73, 74};
                var roomAllocationRequirements = new[]
                {
                    new RoomAllocationRequirement
                    {
                        ReleasePeriod = new ReleasePeriod {Days = 14},
                        StartsFromDate = new DateTime(2019, 01, 14),
                        EndsToDate = new DateTime(2019, 01, 31)
                    }
                };
                AddRoomAllocationRequirements(dbContext, roomIds, roomAllocationRequirements);
                
                 roomIds = new []{ 71, 72, 73, 74};
                 roomAllocationRequirements = new[]
                {
                    new RoomAllocationRequirement
                    {
                        ReleasePeriod = new ReleasePeriod {Days = 21},
                        StartsFromDate = new DateTime(2019, 02, 01),
                        EndsToDate = new DateTime(2019, 02, 04)
                    }
                };
                AddRoomAllocationRequirements(dbContext, roomIds, roomAllocationRequirements);
                
                roomIds = new []{ 71, 72, 73, 74};
                roomAllocationRequirements = new[]
                {
                    new RoomAllocationRequirement
                    {
                        ReleasePeriod = new ReleasePeriod {Days = 35},
                        StartsFromDate = new DateTime(2019, 02, 05),
                        EndsToDate = new DateTime(2019, 02, 11)
                    }
                };
                AddRoomAllocationRequirements(dbContext, roomIds, roomAllocationRequirements);
                
                roomIds = new []{ 71, 72, 73, 74};
                roomAllocationRequirements = new[]
                {
                    new RoomAllocationRequirement
                    {
                        ReleasePeriod = new ReleasePeriod {Days = 21},
                        StartsFromDate = new DateTime(2019, 02, 12),
                        EndsToDate = new DateTime(2019, 03, 27)
                    }
                };
                AddRoomAllocationRequirements(dbContext, roomIds, roomAllocationRequirements);
                
                roomIds = new []{ 71, 72, 73, 74};
                roomAllocationRequirements = new[]
                {
                    new RoomAllocationRequirement
                    {
                        ReleasePeriod = new ReleasePeriod {Days = 35},
                        StartsFromDate = new DateTime(2019, 03, 28),
                        EndsToDate = new DateTime(2019, 04, 21)
                    }
                };
                AddRoomAllocationRequirements(dbContext, roomIds, roomAllocationRequirements);
                
                roomIds = new []{ 71, 72, 73, 74};
                roomAllocationRequirements = new[]
                {
                    new RoomAllocationRequirement
                    {
                        ReleasePeriod = new ReleasePeriod {Days = 21},
                        StartsFromDate = new DateTime(2019, 04, 22),
                        EndsToDate = new DateTime(2019, 05, 05)
                    }
                };
                AddRoomAllocationRequirements(dbContext, roomIds, roomAllocationRequirements);
                
                roomIds = new []{ 71, 72, 73, 74};
                roomAllocationRequirements = new[]
                {
                    new RoomAllocationRequirement
                    {
                        ReleasePeriod = new ReleasePeriod {Days = 7},
                        StartsFromDate = new DateTime(2019, 05, 06),
                        EndsToDate = new DateTime(2019, 08, 31)
                    }
                };
                AddRoomAllocationRequirements(dbContext, roomIds, roomAllocationRequirements);
                
                roomIds = new []{ 71, 72, 73, 74};
                roomAllocationRequirements = new[]
                {
                    new RoomAllocationRequirement
                    {
                        ReleasePeriod = new ReleasePeriod {Days = 14},
                        StartsFromDate = new DateTime(2019, 09, 01),
                        EndsToDate = new DateTime(2019, 10, 12)
                    }
                };
                AddRoomAllocationRequirements(dbContext, roomIds, roomAllocationRequirements);
                
                roomIds = new []{ 71, 72, 73, 74};
                roomAllocationRequirements = new[]
                {
                    new RoomAllocationRequirement
                    {
                        ReleasePeriod = new ReleasePeriod {Days = 21},
                        StartsFromDate = new DateTime(2019, 10, 13),
                        EndsToDate = new DateTime(2019, 10, 19)
                    }
                };
                AddRoomAllocationRequirements(dbContext, roomIds, roomAllocationRequirements);
                
                roomIds = new []{ 71, 72, 73, 74};
                roomAllocationRequirements = new[]
                {
                    new RoomAllocationRequirement
                    {
                        ReleasePeriod = new ReleasePeriod {Days = 35},
                        StartsFromDate = new DateTime(2019, 10, 20),
                        EndsToDate = new DateTime(2019, 11, 09)
                    }
                };
                AddRoomAllocationRequirements(dbContext, roomIds, roomAllocationRequirements);
                
                roomIds = new []{ 71, 72, 73, 74};
                roomAllocationRequirements = new[]
                {
                    new RoomAllocationRequirement
                    {
                        ReleasePeriod = new ReleasePeriod {Days = 21},
                        StartsFromDate = new DateTime(2019, 11, 10),
                        EndsToDate = new DateTime(2019, 12, 01)
                    }
                };
                AddRoomAllocationRequirements(dbContext, roomIds, roomAllocationRequirements);
                
                roomIds = new []{ 71, 72, 73, 74};
                roomAllocationRequirements = new[]
                {
                    new RoomAllocationRequirement
                    {
                        ReleasePeriod = new ReleasePeriod {Days = 14},
                        StartsFromDate = new DateTime(2019, 12, 02),
                        EndsToDate = new DateTime(2019, 12, 21)
                    }
                };
                AddRoomAllocationRequirements(dbContext, roomIds, roomAllocationRequirements);
                
                roomIds = new []{ 71, 72, 73, 74};
                roomAllocationRequirements = new[]
                {
                    new RoomAllocationRequirement
                    {
                        ReleasePeriod = new ReleasePeriod {Days = 21},
                        StartsFromDate = new DateTime(2019, 12, 22),
                        EndsToDate = new DateTime(2019, 12, 26)
                    }
                };
                AddRoomAllocationRequirements(dbContext, roomIds, roomAllocationRequirements);
                
                roomIds = new []{ 71, 72, 73, 74};
                roomAllocationRequirements = new[]
                {
                    new RoomAllocationRequirement
                    {
                        ReleasePeriod = new ReleasePeriod {Days = 35},
                        StartsFromDate = new DateTime(2019, 12, 27),
                        EndsToDate = new DateTime(2020, 01, 04)
                    }
                };
                AddRoomAllocationRequirements(dbContext, roomIds, roomAllocationRequirements);
                
                roomIds = new []{ 71, 72, 73, 74};
                roomAllocationRequirements = new[]
                {
                    new RoomAllocationRequirement
                    {
                        ReleasePeriod = new ReleasePeriod {Days = 21},
                        StartsFromDate = new DateTime(2020, 01, 05),
                        EndsToDate = new DateTime(2020, 01, 13)
                    }
                };
                AddRoomAllocationRequirements(dbContext, roomIds, roomAllocationRequirements);
                
                #endregion
                dbContext.SaveChanges();
/*
                          
                #region AddRooms

                dbContext.Rooms.AddRange(new[]
                {
                    new Room
                    {
                        Id = 71,
                        AccommodationId = hotelId,
                        Name = new MultiLanguage<string>
                        {
                            En = "One Bedroom Deluxe Suite"
                        }
                    },
                    new Room
                    {
                        Id = 72,
                        AccommodationId = hotelId,
                        Name = new MultiLanguage<string>
                        {
                            En = "Panoramic One Bedroom Suite"
                        }
                    },
                    new Room
                    {
                        Id = 73,
                        AccommodationId = hotelId,
                        Name = new MultiLanguage<string>
                        {
                            En = "Two Bedroom Delux Suite"
                        }
                    },
                    new Room
                    {
                        Id = 74,
                        AccommodationId = hotelId,
                        Name = new MultiLanguage<string>
                        {
                            En = "Diplomatic Three Bedroom Suite"
                        }
                    }
                });

                #endregion


                #region AddPrices

                FillRates(
                    dbContext,
                    new[] {51},
                    new List<(int, decimal)>
                    {
                        (71, 7321),
                        (72, 7809),
                        (73, 10981),
                        (74, 18302)
                    });

                FillRates(
                    dbContext,
                    new[] {52},
                    new List<(int, decimal)>
                    {
                        (71, 9273),
                        (72, 10005),
                        (73, 13909),
                        (74, 23182)
                    });

                FillRates(
                    dbContext,
                    new[] {53},
                    new List<(int, decimal)>
                    {
                        (71, 11225),
                        (72, 12201),
                        (73, 22450),
                        (74, 33675)
                    });

                FillRates(
                    dbContext,
                    new[] {54},
                    new List<(int, decimal)>
                    {
                        (71, 9273),
                        (72, 10005),
                        (73, 13909),
                        (74, 23182)
                    });

                FillRates(
                    dbContext,
                    new[] {55},
                    new List<(int, decimal)>
                    {
                        (71, 11225),
                        (72, 12201),
                        (73, 22450),
                        (74, 33675)
                    });

                FillRates(
                    dbContext,
                    new[] {56},
                    new List<(int, decimal)>
                    {
                        (71, 9273),
                        (72, 10005),
                        (73, 13909),
                        (74, 23182)
                    });

                FillRates(
                    dbContext,
                    new[] {57},
                    new List<(int, decimal)>
                    {
                        (71, 6345),
                        (72, 6833),
                        (73, 9517),
                        (74, 15862)
                    });

                FillRates(
                    dbContext,
                    new[] {58},
                    new List<(int, decimal)>
                    {
                        (71, 7321),
                        (72, 7809),
                        (73, 10981),
                        (74, 18302)
                    });

                FillRates(
                    dbContext,
                    new[] {59},
                    new List<(int, decimal)>
                    {
                        (71, 9273),
                        (72, 10005),
                        (73, 13909),
                        (74, 23182)
                    });

                FillRates(
                    dbContext,
                    new[] {60},
                    new List<(int, decimal)>
                    {
                        (71, 11225),
                        (72, 12201),
                        (73, 22450),
                        (74, 33675)
                    });

                FillRates(
                    dbContext,
                    new[] {61},
                    new List<(int, decimal)>
                    {
                        (71, 9273),
                        (72, 10005),
                        (73, 13909),
                        (74, 23182)
                    });

                FillRates(
                    dbContext,
                    new[] {62},
                    new List<(int, decimal)>
                    {
                        (71, 7321),
                        (72, 7809),
                        (73, 10981),
                        (74, 18302)
                    });

                FillRates(
                    dbContext,
                    new[] {63},
                    new List<(int, decimal)>
                    {
                        (71, 9273),
                        (72, 10005),
                        (73, 13909),
                        (74, 23182)
                    });

                FillRates(
                    dbContext,
                    new[] {64},
                    new List<(int, decimal)>
                    {
                        (71, 13177),
                        (72, 14153),
                        (73, 26355),
                        (74, 39532)
                    });

                FillRates(
                    dbContext,
                    new[] {65},
                    new List<(int, decimal)>
                    {
                        (71, 9273),
                        (72, 10005),
                        (73, 13909),
                        (74, 23182)
                    });

                #endregion

                #region AddStopSaleDates //Test data

                //Panoramic Suite
                dbContext.StopSaleDates.AddRange(new StopSaleDate
                {
                    RoomId = 72,
                    StartDate = new DateTime(2019, 9, 28),
                    EndDate = new DateTime(2019, 9, 28)
                }, new StopSaleDate
                {
                    RoomId = 72,
                    StartDate = new DateTime(2019, 9, 30),
                    EndDate = new DateTime(2019, 9, 30)
                },
                new StopSaleDate
                {
                    RoomId = 72,
                    StartDate = new DateTime(2019, 10, 1),
                    EndDate = new DateTime(2019, 10, 1)
                },
                new StopSaleDate
                {
                    RoomId = 73,
                    StartDate = new DateTime(2019, 10, 11),
                    EndDate = new DateTime(2019, 10, 12)
                },
                new StopSaleDate
                {
                    RoomId = 73,
                    StartDate = new DateTime(2020, 10, 19),
                    EndDate = new DateTime(2020, 10, 19)
                },
                new StopSaleDate
                {
                    RoomId = 73,
                    StartDate = new DateTime(2020, 10, 26),
                    EndDate = new DateTime(2020, 10, 27)
                });

                #endregion

                #region Permitted Occupancies

                dbContext.PermittedOccupancies.AddRange(
                    new PermittedOccupancy
                    {
                        RoomId = 71,
                        AdultsNumber = 1,
                        ChildrenNumber = 0,
                    },
                    new PermittedOccupancy
                    {
                        RoomId = 71,
                        AdultsNumber = 2,
                        ChildrenNumber = 0,
                    },
                    new PermittedOccupancy
                    {
                        RoomId = 71,
                        AdultsNumber = 1,
                        ChildrenNumber = 1,
                    },
                    new PermittedOccupancy
                    {
                        RoomId = 71,
                        AdultsNumber = 2,
                        ChildrenNumber = 1,
                    },
                    new PermittedOccupancy
                    {
                        RoomId = 71,
                        AdultsNumber = 3,
                        ChildrenNumber = 0,
                    },
                    new PermittedOccupancy
                    {
                        RoomId = 71,
                        AdultsNumber = 1,
                        ChildrenNumber = 2,
                    },
                    new PermittedOccupancy
                    {
                        RoomId = 71,
                        AdultsNumber = 3,
                        ChildrenNumber = 1,
                    },
                    new PermittedOccupancy
                    {
                        RoomId = 71,
                        AdultsNumber = 2,
                        ChildrenNumber = 2,
                    },
                    new PermittedOccupancy
                    {
                        RoomId = 72,
                        AdultsNumber = 1,
                        ChildrenNumber = 0,
                    },
                    new PermittedOccupancy
                    {
                        RoomId = 72,
                        AdultsNumber = 2,
                        ChildrenNumber = 0
                    },
                    new PermittedOccupancy
                    {
                        RoomId = 72,
                        AdultsNumber = 1,
                        ChildrenNumber = 1
                    },
                    new PermittedOccupancy
                    {
                        RoomId = 72,
                        AdultsNumber = 3,
                        ChildrenNumber = 0
                    },
                    new PermittedOccupancy
                    {
                        RoomId = 72,
                        AdultsNumber = 1,
                        ChildrenNumber = 2
                    },
                    new PermittedOccupancy
                    {
                        RoomId = 72,
                        AdultsNumber = 3,
                        ChildrenNumber = 1
                    },
                    new PermittedOccupancy
                    {
                        RoomId = 72,
                        AdultsNumber = 2,
                        ChildrenNumber = 2
                    },

                    new PermittedOccupancy
                    {
                        RoomId = 73,
                        AdultsNumber = 1,
                        ChildrenNumber = 0
                    },
                    new PermittedOccupancy
                    {
                        RoomId = 73,
                        AdultsNumber = 2,
                        ChildrenNumber = 0
                    },
                    new PermittedOccupancy
                    {
                        RoomId = 73,
                        AdultsNumber = 1,
                        ChildrenNumber = 1
                    },
                    new PermittedOccupancy
                    {
                        RoomId = 73,
                        AdultsNumber = 2,
                        ChildrenNumber = 1
                    },
                    new PermittedOccupancy
                    {
                        RoomId = 73,
                        AdultsNumber = 3,
                        ChildrenNumber = 0
                    },
                    new PermittedOccupancy
                    {
                        RoomId = 73,
                        AdultsNumber = 1,
                        ChildrenNumber = 2
                    },
                    new PermittedOccupancy
                    {
                        RoomId = 73,
                        AdultsNumber = 4,
                        ChildrenNumber = 0
                    },
                    new PermittedOccupancy
                    {
                        RoomId = 73,
                        AdultsNumber = 3,
                        ChildrenNumber = 1
                    },
                    new PermittedOccupancy
                    {
                        RoomId = 73,
                        AdultsNumber = 2,
                        ChildrenNumber = 2
                    },
                    new PermittedOccupancy
                    {
                        RoomId = 73,
                        AdultsNumber = 1,
                        ChildrenNumber = 3
                    },
                    new PermittedOccupancy
                    {
                        RoomId = 73,
                        AdultsNumber = 5,
                        ChildrenNumber = 0
                    },
                    new PermittedOccupancy
                    {
                        RoomId = 73,
                        AdultsNumber = 4,
                        ChildrenNumber = 1
                    },
                    new PermittedOccupancy
                    {
                        RoomId = 73,
                        AdultsNumber = 3,
                        ChildrenNumber = 2
                    },
                    new PermittedOccupancy
                    {
                        RoomId = 73,
                        AdultsNumber = 2,
                        ChildrenNumber = 3
                    },
                    new PermittedOccupancy
                    {
                        RoomId = 73,
                        AdultsNumber = 1,
                        ChildrenNumber = 4
                    },
                    new PermittedOccupancy
                    {
                        RoomId = 73,
                        AdultsNumber = 5,
                        ChildrenNumber = 1
                    },
                    new PermittedOccupancy
                    {
                        RoomId = 73,
                        AdultsNumber = 4,
                        ChildrenNumber = 2
                    },
                    new PermittedOccupancy
                    {
                        RoomId = 73,
                        AdultsNumber = 3,
                        ChildrenNumber = 3
                    },
                    new PermittedOccupancy
                    {
                        RoomId = 73,
                        AdultsNumber = 2,
                        ChildrenNumber = 4
                    },
                    new PermittedOccupancy
                    {
                        RoomId = 73,
                        AdultsNumber = 1,
                        ChildrenNumber = 5
                    },
                    new PermittedOccupancy
                    {
                        RoomId = 73,
                        AdultsNumber = 1,
                        ChildrenNumber = 5
                    },
                    new PermittedOccupancy
                    {
                        RoomId = 74,
                        AdultsNumber = 1,
                        ChildrenNumber = 0
                    },
                    new PermittedOccupancy
                    {
                        RoomId = 74,
                        AdultsNumber = 2,
                        ChildrenNumber = 0
                    },
                    new PermittedOccupancy
                    {
                        RoomId = 74,
                        AdultsNumber = 1,
                        ChildrenNumber = 1
                    },
                    new PermittedOccupancy
                    {
                        RoomId = 74,
                        AdultsNumber = 2,
                        ChildrenNumber = 1
                    },
                    new PermittedOccupancy
                    {
                        RoomId = 74,
                        AdultsNumber = 3,
                        ChildrenNumber = 0
                    },
                    new PermittedOccupancy
                    {
                        RoomId = 74,
                        AdultsNumber = 1,
                        ChildrenNumber = 2
                    },
                    new PermittedOccupancy
                    {
                        RoomId = 74,
                        AdultsNumber = 4,
                        ChildrenNumber = 0
                    },
                    new PermittedOccupancy
                    {
                        RoomId = 74,
                        AdultsNumber = 3,
                        ChildrenNumber = 1
                    },
                    new PermittedOccupancy
                    {
                        RoomId = 74,
                        AdultsNumber = 2,
                        ChildrenNumber = 2
                    },
                    new PermittedOccupancy
                    {
                        RoomId = 74,
                        AdultsNumber = 1,
                        ChildrenNumber = 3
                    },
                    new PermittedOccupancy
                    {
                        RoomId = 74,
                        AdultsNumber = 5,
                        ChildrenNumber = 0
                    },
                    new PermittedOccupancy
                    {
                        RoomId = 74,
                        AdultsNumber = 4,
                        ChildrenNumber = 1
                    },
                    new PermittedOccupancy
                    {
                        RoomId = 74,
                        AdultsNumber = 3,
                        ChildrenNumber = 2
                    },
                    new PermittedOccupancy
                    {
                        RoomId = 74,
                        AdultsNumber = 2,
                        ChildrenNumber = 3
                    },
                    new PermittedOccupancy
                    {
                        RoomId = 74,
                        AdultsNumber = 1,
                        ChildrenNumber = 4
                    },
                    new PermittedOccupancy
                    {
                        RoomId = 74,
                        AdultsNumber = 6,
                        ChildrenNumber = 0
                    },
                    new PermittedOccupancy
                    {
                        RoomId = 74,
                        AdultsNumber = 5,
                        ChildrenNumber = 1
                    },
                    new PermittedOccupancy
                    {
                        RoomId = 74,
                        AdultsNumber = 4,
                        ChildrenNumber = 2
                    },
                    new PermittedOccupancy
                    {
                        RoomId = 74,
                        AdultsNumber = 3,
                        ChildrenNumber = 3
                    },
                    new PermittedOccupancy
                    {
                        RoomId = 74,
                        AdultsNumber = 2,
                        ChildrenNumber = 4
                    },
                    new PermittedOccupancy
                    {
                        RoomId = 74,
                        AdultsNumber = 1,
                        ChildrenNumber =5
                    },
                    new PermittedOccupancy
                    {
                        RoomId = 74,
                        AdultsNumber = 7,
                        ChildrenNumber = 0
                    },
                    new PermittedOccupancy
                    {
                        RoomId = 74,
                        AdultsNumber = 6,
                        ChildrenNumber = 1
                    },
                    new PermittedOccupancy
                    {
                        RoomId = 74,
                        AdultsNumber = 5,
                        ChildrenNumber = 2
                    },
                    new PermittedOccupancy
                    {
                        RoomId = 74,
                        AdultsNumber = 4,
                        ChildrenNumber = 3
                    },
                    new PermittedOccupancy
                    {
                        RoomId = 74,
                        AdultsNumber = 3,
                        ChildrenNumber = 4
                    },
                    new PermittedOccupancy
                    {
                        RoomId = 74,
                        AdultsNumber = 2,
                        ChildrenNumber = 5
                    },
                    new PermittedOccupancy
                    {
                        RoomId = 74,
                        AdultsNumber = 1,
                        ChildrenNumber = 6
                    });

                #endregion region

               dbContext.SaveChanges();
               */
            }
        }


        private static void FillRates(DirectContractsDbContext dbContext, (DateTime startDate, DateTime endDate)[] seasonsPeriods, List<(int, decimal)> roomIdsAndPrices)
        {
            foreach (var seasonPeriod in seasonsPeriods)
            {
                foreach (var roomIdsAndPrice in roomIdsAndPrices)
                {
                    var rate = new RoomRate
                    {
                        StartsFromDate = seasonPeriod.startDate,
                        EndsToDate = seasonPeriod.endDate,
                        RoomId = roomIdsAndPrice.Item1,
                        Price = roomIdsAndPrice.Item2,
                        CurrencyCode = "AED"
                    };
                    dbContext.Entry(rate).State = EntityState.Detached;
                    dbContext.RoomRates.Add(rate);
                }
            }
        }

        
        private static void AddPromotionalOffers(DirectContractsDbContext dbContext, int[] roomIds,
            RoomPromotionalOffer[] promotionalOffers)
        {
            foreach (var id in roomIds)
            {
                foreach (var promotionalOffer in promotionalOffers)
                {
                    promotionalOffer.RoomId = id;
                }
                var serialized = JsonConvert.SerializeObject(promotionalOffers, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
                dbContext.RoomPromotionalOffers.AddRange(JsonConvert.DeserializeObject<IEnumerable<RoomPromotionalOffer>>(serialized));
            }
        }


        private static void AddRoomAllocationRequirements(DirectContractsDbContext dbContext, int[] ids,
            RoomAllocationRequirement[] roomAllocationRequirements)
        {
            foreach (var id in ids)
            {
                foreach (var roomAllocationRequirement in roomAllocationRequirements)
                {
                    roomAllocationRequirement.RoomId = id;
                    dbContext.RoomAllocationRequirements.Add(roomAllocationRequirement);
                }
                var serialized = JsonConvert.SerializeObject(roomAllocationRequirements, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
                dbContext.RoomAllocationRequirements.AddRange(JsonConvert.DeserializeObject<IEnumerable<RoomAllocationRequirement>>(serialized));
            }
        }
        
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using HappyTravel.EdoContracts.Accommodations.Enums;
using HappyTravel.Hiroshima.Common.Models;
using HappyTravel.Hiroshima.Common.Models.Accommodations;
using HappyTravel.Hiroshima.Common.Models.Accommodations.Rooms.CancellationPolicies;
using HappyTravel.Hiroshima.Common.Models.Enums;
using HappyTravel.Hiroshima.Data;
using HappyTravel.Hiroshima.Data.Models;
using HappyTravel.Hiroshima.Data.Models.Accommodations;
using HappyTravel.Hiroshima.Data.Models.Location;
using HappyTravel.Hiroshima.Data.Models.Rooms;
using HappyTravel.Money.Enums;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using Newtonsoft.Json;
using JsonDocumentUtilities = HappyTravel.Hiroshima.Common.Infrastructure.Utilities.JsonDocumentUtilities;
using PropertyTypes = HappyTravel.Hiroshima.Common.Models.Accommodations.PropertyTypes;

namespace HappyTravel.Hiroshima.DirectContractsDataSeeder
{
    internal static class DataSeeder
    {
        internal static void AddData(DirectContractsDbContext dbContext)
        {
            AddContractManager(dbContext);
            AddLocations(dbContext);
            AddOneAndOnlyContract(dbContext);
            AddJumeriahContract(dbContext);
        }

        private static void AddContractManager(DirectContractsDbContext dbContext)
        {
            var contractManager = dbContext.ContractManagers.SingleOrDefault(cm => cm.Id == int.MaxValue);
            if (contractManager is null)
            {
                dbContext.ContractManagers.Add(new ContractManager
                {
                    Id = int.MaxValue,
                    IdentityHash = "test",
                    Created = DateTime.UtcNow,
                    Email = "manager@mail.com",
                    Name = "Manager Name",
                    Title = "Manager Title",
                    Description = "Manager Description"
                });
                dbContext.SaveChanges();
            }
        }

        private static void AddLocations(DirectContractsDbContext dbContext)
        {
            #region AddCountries

            var country = new Country {Code = "AE"};

            country.Name = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string>
            {
                En = "The United Arab Emirates",
                Ru = "Объединенные Арабские Эмираты",
                Ar = "الإمارات العربية المتحدة"
            });

            dbContext.Countries.Add(country);

            #endregion

            #region AddLocations

            var location = new HappyTravel.Hiroshima.Data.Models.Location.Location
            {
                Id = 1,
                CountryCode = "AE",
                Locality = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string>
                {
                    Ar = "دبي", En = "Dubai", Ru = "Дубай"
                })
            };

            dbContext.Locations.Add(location);

            #endregion

            dbContext.SaveChanges();
        }

        private static int GetLocationId(DirectContractsDbContext dbContext)
        {
            var location = dbContext.Locations.Where(l =>
                    l.Locality.RootElement.GetProperty("en").GetString() == "Dubai")
                .SingleOrDefault();
            return location.Id;
        }

        private static bool IsAccommodationExist(DirectContractsDbContext dbContext, string accommodationName)
        {
            var accommodation = dbContext.Accommodations
                .Where(a => a.Name.RootElement.GetProperty("en").GetString() == accommodationName)
                .SingleOrDefault();

            return accommodation != null;
        }

        private static void AddOneAndOnlyContract(DirectContractsDbContext dbContext)
        {
            var contractId = int.MaxValue;
            var hotelId = int.MaxValue;
            
            var contract = dbContext.Contracts.SingleOrDefault(c => c.Name == "ONE&ONLY ROYAL MIRAGE contract");
            if (contract != null) return;
            
            dbContext.Contracts.Add(new Contract
            {
                Id = contractId,
                Name = "ONE&ONLY ROYAL MIRAGE contract",
                Description = "ONE&ONLY ROYAL MIRAGE contract",
                ContractManagerId = int.MaxValue,
                ValidFrom = new DateTime(2020, 01, 01),
                ValidTo = new DateTime(2020, 12, 31)
            });
            dbContext.SaveChanges();

            dbContext.ContractAccommodationRelations.Add(new ContractAccommodationRelation()
            {
                AccommodationId = hotelId, 
                ContractId = contractId
            });
            dbContext.SaveChanges();

            var seasons = new List<(int, DateTime, DateTime)>
            {
                (GetSeasonId(11), new DateTime(2020, 01, 08), new DateTime(2020, 01, 14)),
                (GetSeasonId(13), new DateTime(2020, 01, 25), new DateTime(2020, 02, 07)),
                (GetSeasonId(15), new DateTime(2020, 02, 22), new DateTime(2020, 03, 20)),
                (GetSeasonId(19), new DateTime(2020, 04, 19), new DateTime(2020, 05, 03)),
                (GetSeasonId(24), new DateTime(2020, 10, 10), new DateTime(2020, 10, 16)),
                (GetSeasonId(27), new DateTime(2020, 11, 07), new DateTime(2020, 12, 04)),
                (GetSeasonId(29), new DateTime(2020, 12, 19), new DateTime(2020, 12, 25))
            };
            AddSeasons(dbContext, contractId, "HIGH I", seasons);

            seasons = new List<(int, DateTime, DateTime)>
            {
                (GetSeasonId(12), new DateTime(2020, 01, 15), new DateTime(2020, 01, 24)),
                (GetSeasonId(23), new DateTime(2020, 09, 26), new DateTime(2020, 10, 09)),
                (GetSeasonId(28), new DateTime(2020, 12, 05), new DateTime(2020, 12, 18))
            };
            AddSeasons(dbContext, contractId, "HIGH II", seasons);

            seasons = new List<(int, DateTime, DateTime)>
            {
                (GetSeasonId(14), new DateTime(2020, 02, 08), new DateTime(2020, 02, 21)),
                (GetSeasonId(16), new DateTime(2020, 03, 21), new DateTime(2020, 03, 27)),
                (GetSeasonId(18), new DateTime(2020, 04, 13), new DateTime(2020, 04, 18)),
                (GetSeasonId(25), new DateTime(2020, 10, 17), new DateTime(2020, 10, 23))
            };
            AddSeasons(dbContext, contractId, "PEAK I", seasons);

            seasons = new List<(int, DateTime, DateTime)>
            {
                (GetSeasonId(17), new DateTime(2020, 03, 28), new DateTime(2020, 04, 12)),
                (GetSeasonId(26), new DateTime(2020, 10, 24), new DateTime(2020, 11, 06))
            };
            AddSeasons(dbContext, contractId, "PEAK II", seasons);

            seasons = new List<(int, DateTime, DateTime)>
            {
                (GetSeasonId(20), new DateTime(2020, 05, 04), new DateTime(2020, 05, 31)),
                (GetSeasonId(22), new DateTime(2020, 09, 05), new DateTime(2020, 09, 25))
            };
            AddSeasons(dbContext, contractId, "SHOULDER", seasons);

            seasons = new List<(int, DateTime, DateTime)>
            {
                (GetSeasonId(21), new DateTime(2020, 06, 01), new DateTime(2020, 09, 04))
            };
            AddSeasons(dbContext, contractId, "LOW", seasons);

            seasons = new List<(int, DateTime, DateTime)>
            {
                (GetSeasonId(30), new DateTime(2020, 12, 26), new DateTime(2020, 12, 03))
            };
            AddSeasons(dbContext, contractId, "FESTIVE", seasons);

            seasons = new List<(int, DateTime, DateTime)>
            {
                (GetSeasonId(31), new DateTime(2021, 01, 04), new DateTime(2021, 01, 10)),
                (GetSeasonId(33), new DateTime(2021, 02, 06), new DateTime(2021, 02, 19)),
                (GetSeasonId(35), new DateTime(2021, 03, 20), new DateTime(2021, 03, 26))
            };
            AddSeasons(dbContext, contractId, "PEAK I", seasons);

            seasons = new List<(int, DateTime, DateTime)>
            {
                (GetSeasonId(32), new DateTime(2021, 01, 21), new DateTime(2021, 02, 05)),
                (GetSeasonId(34), new DateTime(2021, 02, 20), new DateTime(2021, 03, 19)),
                (GetSeasonId(37), new DateTime(2021, 04, 11), new DateTime(2021, 05, 7))
            };
            AddSeasons(dbContext, contractId, "HIGH I", seasons);

            seasons = new List<(int, DateTime, DateTime)>
            {
                (GetSeasonId(36), new DateTime(2021, 03, 27), new DateTime(2021, 04, 10))
            };
            AddSeasons(dbContext, contractId, "PEAK II", seasons);

            if (IsAccommodationExist(dbContext, "ONE&ONLY ROYAL MIRAGE")) return;

            
            #region AddAccommodation

            var accommodation = new Accommodation
            {
                Id = hotelId,
                ContractManagerId = int.MaxValue,
                Rating = AccommodationRating.FiveStars,
                PropertyType = PropertyTypes.Hotels,
                ContactInfo = new ContactInfo {Email = "info@oneandonlythepalm.com", Phone = "+ 971 4 440 1010"},
                CheckInTime = "13:00",
                CheckOutTime = "11:00",
                Coordinates = new Point(55.153219, 25.097596),
                OccupancyDefinition = new OccupancyDefinition
                {
                    Infant = new AgeRange {LowerBound = 0, UpperBound = 3},
                    Child = new AgeRange {LowerBound = 4, UpperBound = 11},
                    Adult = new AgeRange {LowerBound = 12, UpperBound = 200},
                },
                LocationId = GetLocationId(dbContext)
            };

            accommodation.Name = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string>
            {
                Ar = "ون آند اونلي رويال ميراج", En = "ONE&ONLY ROYAL MIRAGE", Ru = "ONE&ONLY ROYAL MIRAGE"
            });

            accommodation.TextualDescription = JsonDocumentUtilities.CreateJDocument(
                new MultiLanguage<TextualDescription>
                {
                    En = new TextualDescription
                    {
                        Description =
                            "Set in 65 acres of lush gardens and a kilometer of private beach, peaceful lives in remarkable opulence.",
                    },
                    Ar = new TextualDescription
                    {
                        Description =
                            "65 فدان من الحدائق الغناء وكيلومتر من الشواطئ الخاصة تمثل أنموذجاً للسلاموالرخاء منقطعا النظير."
                    },
                    Ru = new TextualDescription
                    {
                        Description =
                            "26 гектаров ландшафтных садов, собственный пляж протяженностью в один километр, умиротворенная обстановка и роскошное окружение."
                    }
                });

            accommodation.AdditionalInfo = JsonDocumentUtilities.CreateJDocument(
                new MultiLanguage<string>
                {
                    En = "Additional Info 1 en",
                    Ar = "Additional Info 2 ar"
                });

            accommodation.Address = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string>
            {
                Ar = "شارع الملك سلمان بن عبدالعزيز آل سعود - دبي",
                En = "King Salman Bin Abdulaziz Al Saud St - Dubai",
                Ru = "King Salman Bin Abdulaziz Al Saud St - Dubai - ОАЭ"
            });

            accommodation.AccommodationAmenities = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<List<string>>
            {
                En =
                    new List<string>
                    {
                        @"Dining at One&Only Royal Mirage is a unique journey within eight different restaurants between The Palace, Arabian Court and Residence & Spa. Each venue presents an authentic culinary experience in a distinctive environment. The Palace offers four different restaurants, including Mediterranean flavours at Olives, exceptional Moroccan fare at Tagine, an elegant dining on European cuisine at Celebrities, and fresh grilled seafood and bright salads at The Beach Bar & Grill. Within Arabian Court, guests may discover seductive Indo-European flavours at Nina, savour all-day dining on traditional recipes from the Middle East and northern Europe at The Rotisserie, or experience the floating majlis overlooking the Arabian Gulf at Eauzone featuring international cuisine with an Asian twist. Exclusive to guests at Residence & Spa, The Dining Room showcases inspired menus of fresh creativity.",
                        @"Dubai boasts 10 spectacular golf courses within easy reach of One&Only Royal Mirage, with Emirates Golf Club and Montgomerie just minutes away.",
                        @"Spirits elevate as you indulge in a range of holistic experiences designed to restore mind, body and soul at One&Only Spa. At the Traditional Oriental Hammam, let the body ease slowly into the rising heat and steam whilst the sounds of rippling water relaxes the mind."
                    },
                Ar =
                    new List<string>
                    {
                        @"يعتبر تناول الطعام في ون آند أونلي رويال ميراج متعة بحد ذاتها بفضل ثمانية مطاعم مختلفة في ذا بالاس والردهة العربية والإقامة والسبا. ويقدم كل منها تجربة فريدة لاكتشاف أشهى المأكولات في أجواء مميزة ومبتكرة. ويوفر ذا بالاس أربعة مطاعم مختلفة؛ حيث تُقدَّم القوائم المتوسطية في أوليفز، والمأكولات المغربية الاستثنائية في طاجين، والمأكولات العالمية الكلاسيكية في مطعم المشاهير، والسلطات الطازجة وثمار البحر في بار ومطعم الشاطئ للمشويات. ويمكن للضيوف اكتشاف أشهى النكهات الهندو-أوروبية في نينا، أو تذوق أصنافهم المفضلة طوال اليوم من أطباق الشرق الأوسط وشمال أوروبا في ذا روتيسيري، أو تناول الطعام في مجلس عائم مطل على الخليج العربي في مطعم أوزون الذي يقدم المأكولات العالمية بلمسة آسيوية. وتعرض قاعة الطعام قوائم ملهمة ومبتكرة حصريًا لضيوف المساكن والسبا.",
                        @"تضم دبي 10 ملاعب جولف ضمن مسافة قريبة من ون آند أونلي رويال ميراج، ولا يبعد نادي الإمارات للجولف ونادي مونتغمري إلا بضع دقائق.",
                        "ترتقي روحك وتسمو بينما تنغمس في ملذات التجارب الشاملة المصممة لاستعادة العقل والبدن والروح في منتجع ون آند أونلي سبا. ادخل الحمام الشرقي ودع بدنك يرتاح ببطء عبر الحرارة والبخار المتصاعد بينما تبث أصوات الماء المترقرقة الاسترخاء في العقل."
                    },
                Ru = new List<string>
                {
                    @"Ужин или обед на курорте One&Only Royal Mirage — это уникальное путешествие, в котором вас ждут восемь ресторанов, расположенных в корпусах The Palace, Arabian Court и Residence & Spa. Каждый из ресторанов дарит гостям уникальный гастрономический опыт в неповторимой обстановке. В корпусе The Palace расположены четыре ресторана, включая ресторан средиземноморской кухни Olives, ресторан марокканской кухни Tagine, ресторан Celebrities с элегантным интерьером и европейской кухней, а также гриль-бар Beach Bar & Grill, где вам предложат блюда из свежих морепродуктов на гриле и великолепные салаты. В корпусе Arabian Court гостей ждут соблазнительные блюда индоевропейской кухни ресторана Nina, а в ресторане The Rotisserie в течение всего дня можно оценить блюда, приготовленные по традиционным рецептам кухни Ближнего Востока и Северной Европы. В ресторане Eauzone можно удобно устроиться в плавающем меджлисе с видом на Персидский залив и заказать блюда международной кухни с азиатскими нотками. Ресторан Dining Room, обслуживающий исключительно гостей корпуса Residence & Spa, отличается творческим подходом к составлению меню и приготовлению блюд.",
                    @"В Дубае, недалеко от курорта One&Only Royal Mirage, находится 10 роскошных полей для гольфа, а гольф-клубы Emirates Golf Club и Montgomerie также расположены в непосредственной близости.",
                    @"Процедуры для восстановления равновесия разума, тела и души в спа-центре One&Only обновят вашу жизненную энергию. Позвольте теплым парам традиционного восточного хаммама расслабить ваше тело, пока струящаяся вода успокаивает мысли."
                }
            });

            accommodation.Pictures = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<List<Picture>>
            {
                En = new List<Picture>
                {
                    new Picture
                    {
                        Source =
                            "https://encrypted-tbn0.gstatic.com/images?q=tbn%3AANd9GcQ6HkA4WClkmu3oOM0iuENG66UC6iKZNUrefe0iJ__MX5ZbValF",
                        Caption = "ONE&ONLY ROYAL MIRAGE"
                    }
                },
                Ar = new List<Picture>
                {
                    new Picture
                    {
                        Source =
                            "https://encrypted-tbn0.gstatic.com/images?q=tbn%3AANd9GcQ6HkA4WClkmu3oOM0iuENG66UC6iKZNUrefe0iJ__MX5ZbValF",
                        Caption = "ون آند اونلي رويال ميراج"
                    }
                },
                Ru = new List<Picture>
                {
                    new Picture
                    {
                        Source =
                            "https://encrypted-tbn0.gstatic.com/images?q=tbn%3AANd9GcQ6HkA4WClkmu3oOM0iuENG66UC6iKZNUrefe0iJ__MX5ZbValF",
                        Caption = "ONE&ONLY ROYAL MIRAGE"
                    }
                }
            });

            dbContext.Accommodations.Add(accommodation);

            #endregion

            dbContext.SaveChanges();

            #region AddRooms

            var room = new Room
            {
                Id = 20,
                AccommodationId = hotelId,
                OccupancyConfigurations = new List<OccupancyConfiguration>
                {
                    new OccupancyConfiguration {Adults = 2, Teenagers = 1},
                    new OccupancyConfiguration {Adults = 2, Children = 1, Infants = 1}
                }
            };

            room.Name = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string>
            {
                En = "Palace Superior Deluxe Room", Ru = "", Ar = ""
            });
            room.Description =
                JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string> {En = "", Ru = "", Ar = ""});
            room.Amenities = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<List<string>>
            {
                En = new List<string>(), Ar = new List<string>(), Ru = new List<string>(),
            });
            room.Pictures = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<List<Picture>>());
            dbContext.Rooms.Add(room);

            room = new Room
            {
                Id = 21,
                AccommodationId = hotelId,
                OccupancyConfigurations = new List<OccupancyConfiguration>
                {
                    new OccupancyConfiguration {Adults = 2, Teenagers = 1},
                    new OccupancyConfiguration {Adults = 2, Children = 1, Infants = 1}
                }
            };
            room.Name = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string>
            {
                En = "Palace Gold Club Room", Ru = "", Ar = ""
            });
            room.Description =
                JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string> {En = "", Ru = "", Ar = ""});
            room.Amenities = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<List<string>>
            {
                En = new List<string>(), Ar = new List<string>(), Ru = new List<string>(),
            });
            room.Pictures = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<List<Picture>>());
            dbContext.Rooms.Add(room);

            room = new Room
            {
                Id = 22,
                AccommodationId = hotelId,
                OccupancyConfigurations = new List<OccupancyConfiguration>
                {
                    new OccupancyConfiguration {Adults = 2, Teenagers = 1},
                    new OccupancyConfiguration {Adults = 2, Children = 1, Infants = 1}
                }
            };
            room.Name = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string>
            {
                En = "Palace One Bedroom Executive Suite", Ru = "", Ar = ""
            });
            room.Description =
                JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string> {En = "", Ru = "", Ar = ""});
            room.Amenities = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<List<string>>
            {
                En = new List<string>(), Ar = new List<string>(), Ru = new List<string>(),
            });
            room.Pictures = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<List<Picture>>());
            dbContext.Rooms.Add(room);

            room = new Room
            {
                Id = 23,
                AccommodationId = hotelId,
                OccupancyConfigurations = new List<OccupancyConfiguration>
                {
                    new OccupancyConfiguration {Adults = 2, Teenagers = 1},
                    new OccupancyConfiguration {Adults = 2, Children = 1, Infants = 1}
                }
            };
            room.Name = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string>
            {
                En = "Palace One Bedroom Gold Club Suite", Ru = "", Ar = ""
            });
            room.Description =
                JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string> {En = "", Ru = "", Ar = ""});
            room.Amenities = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<List<string>>
            {
                En = new List<string>(), Ar = new List<string>(), Ru = new List<string>(),
            });
            room.Pictures = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<List<Picture>>());
            dbContext.Rooms.Add(room);

            room = new Room
            {
                Id = 24,
                AccommodationId = hotelId,
                OccupancyConfigurations = new List<OccupancyConfiguration>
                {
                    new OccupancyConfiguration {Adults = 4, Teenagers = 1},
                    new OccupancyConfiguration {Adults = 4, Children = 1},
                    new OccupancyConfiguration {Adults = 4, Infants = 1}
                }
            };
            room.Name = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string>
            {
                En = "Palace Two Bedroom Executive Suite", Ru = "", Ar = ""
            });
            room.Description =
                JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string> {En = "", Ru = "", Ar = ""});
            room.Amenities = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<List<string>>
            {
                En = new List<string>(), Ar = new List<string>(), Ru = new List<string>(),
            });
            room.Pictures = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<List<Picture>>());
            dbContext.Rooms.Add(room);

            room = new Room
            {
                Id = 25,
                AccommodationId = hotelId,
                OccupancyConfigurations = new List<OccupancyConfiguration>
                {
                    new OccupancyConfiguration {Adults = 4, Teenagers = 1},
                    new OccupancyConfiguration {Adults = 4, Children = 1},
                    new OccupancyConfiguration {Adults = 4, Infants = 1}
                }
            };
            room.Name = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string>
            {
                En = "Palace Two Bedroom Gold Club Suite", Ru = "", Ar = ""
            });
            room.Description =
                JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string> {En = "", Ru = "", Ar = ""});
            room.Amenities = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<List<string>>
            {
                En = new List<string>(), Ar = new List<string>(), Ru = new List<string>(),
            });
            room.Pictures = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<List<Picture>>());
            dbContext.Rooms.Add(room);

            room = new Room
            {
                Id = 26,
                AccommodationId = hotelId,
                OccupancyConfigurations = new List<OccupancyConfiguration>
                {
                    new OccupancyConfiguration {Adults = 4, Teenagers = 1},
                    new OccupancyConfiguration {Adults = 4, Children = 1},
                    new OccupancyConfiguration {Adults = 4, Infants = 1}
                }
            };
            room.Name = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string>
            {
                En = "Palace Two Bedroom Royal Suite", Ru = "", Ar = ""
            });
            room.Description =
                JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string> {En = "", Ru = "", Ar = ""});
            room.Amenities = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<List<string>>
            {
                En = new List<string>(), Ar = new List<string>(), Ru = new List<string>(),
            });
            room.Pictures = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<List<Picture>>());
            dbContext.Rooms.Add(room);

            room = new Room
            {
                Id = 27,
                AccommodationId = hotelId,
                OccupancyConfigurations = new List<OccupancyConfiguration>
                {
                    new OccupancyConfiguration {Adults = 2, Teenagers = 1},
                    new OccupancyConfiguration {Adults = 2, Children = 1, Infants = 1}
                }
            };
            room.Name = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string>
            {
                En = "Arabian Court Deluxe Room", Ru = "", Ar = ""
            });
            room.Description =
                JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string> {En = "", Ru = "", Ar = ""});
            room.Amenities = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<List<string>>
            {
                En = new List<string>(), Ar = new List<string>(), Ru = new List<string>(),
            });
            room.Pictures = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<List<Picture>>());
            dbContext.Rooms.Add(room);

            room = new Room
            {
                Id = 28,
                AccommodationId = hotelId,
                OccupancyConfigurations = new List<OccupancyConfiguration>
                {
                    new OccupancyConfiguration {Adults = 3, Children = 3},
                    new OccupancyConfiguration {Adults = 3, Infants = 3},
                    new OccupancyConfiguration {Adults = 3, Children = 2, Infants = 1},
                    new OccupancyConfiguration {Adults = 3, Children = 1, Infants = 2}
                }
            };
            room.Name = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string>
            {
                En = "Arabian Court Two Deluxe Rooms Family Accommodation", Ru = "", Ar = ""
            });
            room.Description =
                JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string> {En = "", Ru = "", Ar = ""});
            room.Amenities = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<List<string>>
            {
                En = new List<string>(), Ar = new List<string>(), Ru = new List<string>(),
            });
            room.Pictures = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<List<Picture>>());
            dbContext.Rooms.Add(room);

            room = new Room
            {
                Id = 29,
                AccommodationId = hotelId,
                OccupancyConfigurations = new List<OccupancyConfiguration>
                {
                    new OccupancyConfiguration {Adults = 2, Teenagers = 1},
                    new OccupancyConfiguration {Adults = 2, Children = 1, Infants = 1}
                }
            };
            room.Name = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string>
            {
                En = "Arabian Court One Bedroom Executive Suite", Ru = "", Ar = ""
            });
            room.Description =
                JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string> {En = "", Ru = "", Ar = ""});
            room.Amenities = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<List<string>>
            {
                En = new List<string>(), Ar = new List<string>(), Ru = new List<string>(),
            });
            room.Pictures = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<List<Picture>>());
            dbContext.Rooms.Add(room);

            room = new Room
            {
                Id = 30,
                AccommodationId = hotelId,
                OccupancyConfigurations = new List<OccupancyConfiguration>
                {
                    new OccupancyConfiguration {Adults = 4, Teenagers = 1},
                    new OccupancyConfiguration {Adults = 4, Children = 1},
                    new OccupancyConfiguration {Adults = 4, Infants = 1}
                }
            };
            room.Name = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string>
            {
                En = "Arabian Court Two Bedroom Executive Suite", Ru = "", Ar = ""
            });
            room.Description =
                JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string> {En = "", Ru = "", Ar = ""});
            room.Amenities = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<List<string>>
            {
                En = new List<string>(), Ar = new List<string>(), Ru = new List<string>(),
            });
            room.Pictures = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<List<Picture>>());
            dbContext.Rooms.Add(room);

            room = new Room
            {
                Id = 31,
                AccommodationId = hotelId,
                OccupancyConfigurations = new List<OccupancyConfiguration>
                {
                    new OccupancyConfiguration {Adults = 2, Teenagers = 1},
                    new OccupancyConfiguration {Adults = 2, Children = 1, Infants = 1}
                }
            };
            room.Name = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string>
            {
                En = "Arabian Court Prince Suite", Ru = "", Ar = ""
            });
            room.Description =
                JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string> {En = "", Ru = "", Ar = ""});
            room.Amenities = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<List<string>>
            {
                En = new List<string>(), Ar = new List<string>(), Ru = new List<string>(),
            });
            room.Pictures = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<List<Picture>>());
            dbContext.Rooms.Add(room);

            room = new Room
            {
                Id = 32,
                AccommodationId = hotelId,
                OccupancyConfigurations = new List<OccupancyConfiguration>
                {
                    new OccupancyConfiguration {Adults = 2, Teenagers = 1},
                    new OccupancyConfiguration {Adults = 2, Children = 1, Infants = 1}
                }
            };
            room.Name = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string>
            {
                En = "Residence Prestige Room", Ru = "", Ar = ""
            });
            room.Description =
                JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string> {En = "", Ru = "", Ar = ""});
            room.Amenities = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<List<string>>
            {
                En = new List<string>(), Ar = new List<string>(), Ru = new List<string>(),
            });
            room.Pictures = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<List<Picture>>());
            dbContext.Rooms.Add(room);

            room = new Room
            {
                Id = 33,
                AccommodationId = hotelId,
                OccupancyConfigurations = new List<OccupancyConfiguration>
                {
                    new OccupancyConfiguration {Adults = 2, Teenagers = 1},
                    new OccupancyConfiguration {Adults = 2, Children = 1, Infants = 1}
                }
            };
            room.Name = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string>
            {
                En = "Residence Junior Suite", Ru = "", Ar = ""
            });
            room.Description =
                JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string> {En = "", Ru = "", Ar = ""});
            room.Amenities = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<List<string>>
            {
                En = new List<string>(), Ar = new List<string>(), Ru = new List<string>(),
            });
            room.Pictures = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<List<Picture>>());
            dbContext.Rooms.Add(room);

            room = new Room
            {
                Id = 34,
                AccommodationId = hotelId,
                OccupancyConfigurations = new List<OccupancyConfiguration>
                {
                    new OccupancyConfiguration {Adults = 2, Teenagers = 1},
                    new OccupancyConfiguration {Adults = 2, Children = 1, Infants = 1}
                }
            };
            room.Name = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string>
            {
                En = "Residence Executive Suite", Ru = "", Ar = ""
            });
            room.Description =
                JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string> {En = "", Ru = "", Ar = ""});
            room.Amenities = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<List<string>>
            {
                En = new List<string>(), Ar = new List<string>(), Ru = new List<string>(),
            });
            room.Pictures = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<List<Picture>>());
            dbContext.Rooms.Add(room);

            room = new Room
            {
                Id = 35,
                AccommodationId = hotelId,
                OccupancyConfigurations = new List<OccupancyConfiguration>
                {
                    new OccupancyConfiguration {Adults = 4, Teenagers = 1},
                    new OccupancyConfiguration {Adults = 4, Children = 1},
                    new OccupancyConfiguration {Adults = 4, Infants = 1}
                }
            };
            room.Name = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string>
            {
                En = "Residence Garden Beach Villa", Ru = "", Ar = ""
            });
            room.Description =
                JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string> {En = "", Ru = "", Ar = ""});
            room.Amenities = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<List<string>>
            {
                En = new List<string>(), Ar = new List<string>(), Ru = new List<string>(),
            });
            room.Pictures = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<List<Picture>>());
            dbContext.Rooms.Add(room);

            #endregion

            dbContext.SaveChanges();

            #region AddCancellationPolicies

            var roomIds = new[] {20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35};
            var seasonIds = new[] {30};

            var cancellationPolicy = new RoomCancellationPolicy
            {
                Policies = new List<CancellationPolicyItem>
                {
                    new CancellationPolicyItem
                    {
                        DayPriorToArrival = new DayInterval {FromDay = 0, ToDay = 45},
                        PenaltyType = CancellationPenaltyTypes.Percent,
                        PenaltyCharge = 100
                    }
                }
            };
            AddCancellationPolicies(dbContext, seasonIds, roomIds, cancellationPolicy);
            dbContext.SaveChanges();
            
            seasonIds = new[] {17, 26};
            
            cancellationPolicy = new RoomCancellationPolicy
            {
                Policies = new List<CancellationPolicyItem>
                {
                    new CancellationPolicyItem
                    {
                        DayPriorToArrival = new DayInterval {FromDay = 0, ToDay = 35},
                        PenaltyType = CancellationPenaltyTypes.Percent,
                        PenaltyCharge = 100
                    }
                }
            };
            AddCancellationPolicies(dbContext, seasonIds, roomIds, cancellationPolicy);
            dbContext.SaveChanges();
            
            seasonIds = new[] {14, 16, 18, 25, 11, 13, 15, 19, 12, 23, 28};
            cancellationPolicy = new RoomCancellationPolicy
            {
                Policies = new List<CancellationPolicyItem>
                {
                    new CancellationPolicyItem
                    {
                        DayPriorToArrival = new DayInterval {FromDay = 14, ToDay = 28},
                        PenaltyType = CancellationPenaltyTypes.Nights,
                        PenaltyCharge = 4
                    },
                    new CancellationPolicyItem
                    {
                        DayPriorToArrival = new DayInterval {FromDay = 07, ToDay = 13},
                        PenaltyType = CancellationPenaltyTypes.Nights,
                        PenaltyCharge = 7
                    },
                    new CancellationPolicyItem
                    {
                        DayPriorToArrival = new DayInterval {FromDay = 0, ToDay = 6},
                        PenaltyType = CancellationPenaltyTypes.Percent,
                        PenaltyCharge = 100
                    }
                }
            };
            
            AddCancellationPolicies(dbContext, seasonIds, roomIds, cancellationPolicy);
            dbContext.SaveChanges();

            seasonIds = new[] {20, 21, 22};
            cancellationPolicy = new RoomCancellationPolicy
            {
                Policies = new List<CancellationPolicyItem>
                {
                    new CancellationPolicyItem
                    {
                        DayPriorToArrival = new DayInterval {FromDay = 07, ToDay = 13},
                        PenaltyType = CancellationPenaltyTypes.Nights,
                        PenaltyCharge = 03
                    },
                    new CancellationPolicyItem
                    {
                        DayPriorToArrival = new DayInterval {FromDay = 0, ToDay = 06},
                        PenaltyType = CancellationPenaltyTypes.Nights,
                        PenaltyCharge = 05
                    }
                }
            };
            AddCancellationPolicies(dbContext, seasonIds, roomIds, cancellationPolicy);
            dbContext.SaveChanges();

            #endregion

            #region AddRates

            FillRates(dbContext, new[] {GetSeasonId(11), GetSeasonId(13), GetSeasonId(15), GetSeasonId(19)},
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
                    (35, 30000)
                });
            FillRates(dbContext, new[] {GetSeasonId(12), GetSeasonId(23), GetSeasonId(28)},
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
                    (35, 29000)
                });
            FillRates(dbContext, new[] {GetSeasonId(14), GetSeasonId(16), GetSeasonId(18), GetSeasonId(25)},
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
                    (35, 30000)
                });
            FillRates(dbContext, new[] {GetSeasonId(17), GetSeasonId(26)},
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
                    (35, 36000)
                });
            FillRates(dbContext,
                //new[] { 20, 22 },
                new[] {GetSeasonId(20), GetSeasonId(22)}, new List<(int, decimal)>
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
                    (35, 24000)
                });
            FillRates(dbContext, new[] {GetSeasonId(21)}, new List<(int, decimal)>
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
                (35, 24000)
            });
            FillRates(dbContext,
                //new[] { 24, 27, 29 },
                new[] {GetSeasonId(24), GetSeasonId(27), GetSeasonId(29)}, new List<(int, decimal)>
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
                    (35, 24000)
                });
            FillRates(dbContext, new[] {GetSeasonId(30)}, new List<(int, decimal)>
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
                (35, 38000)
            });
            FillRates(dbContext, new[] {GetSeasonId(31), GetSeasonId(33), GetSeasonId(35)},
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
                    (35, 3000)
                });
            FillRates(dbContext, new[] {GetSeasonId(32), GetSeasonId(34)},
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
                    (35, 30000)
                });
            FillRates(dbContext, new[] {GetSeasonId(36)}, new List<(int, decimal)>
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
                (35, 36000)
            });
            FillRates(dbContext, new[] {GetSeasonId(37)}, new List<(int, decimal)>
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
                (35, 30000)
            });

            #endregion

            dbContext.SaveChanges();

            #region AddPromotionalOffers

            var promotionalOffers = new List<RoomPromotionalOffer>();
            var promotionalOffer = new RoomPromotionalOffer
            {
                DiscountPercent = 25,
                BookByDate = new DateTime(2019, 12, 13),
                ValidFromDate = new DateTime(2020, 01, 08),
                ValidToDate = new DateTime(2020, 02, 07),
                BookingCode = "25% PROMO",
                ContractId = contractId
            };
            promotionalOffer.Details = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string>
            {
                En =
                    "Book on or before Friday, DECEMBER 13, 2019, receive 25% discount, combinable with complimentary dinner dine-around offer on applicable dates"
            });
            promotionalOffers.Add(promotionalOffer);

            promotionalOffer = new RoomPromotionalOffer
            {
                DiscountPercent = 20,
                BookByDate = new DateTime(2019, 12, 13),
                ValidFromDate = new DateTime(2020, 02, 08),
                ValidToDate = new DateTime(2020, 03, 20),
                BookingCode = "20% PROMO",
                ContractId = contractId
            };
            promotionalOffer.Details = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string>
            {
                En =
                    "Book on or before Friday, DECEMBER 13, 2019, receive 20% discount, combinable with complimentary dinner dine-around offer on applicable dates"
            });
            promotionalOffers.Add(promotionalOffer);

            promotionalOffer = new RoomPromotionalOffer
            {
                DiscountPercent = 25,
                BookByDate = new DateTime(2019, 12, 13),
                ValidFromDate = new DateTime(2020, 02, 08),
                ValidToDate = new DateTime(2020, 03, 20),
                BookingCode = "25% PROMO",
                ContractId = contractId
            };
            promotionalOffer.Details = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string>
            {
                En =
                    "Book on or before Friday, DECEMBER 13, 2019, receive 25% discount, combinable with complimentary dinner dine-around offer on applicable dates"
            });
            promotionalOffers.Add(promotionalOffer);

            promotionalOffer = new RoomPromotionalOffer
            {
                DiscountPercent = 20,
                BookByDate = new DateTime(2020, 02, 28),
                ValidFromDate = new DateTime(2020, 03, 28),
                ValidToDate = new DateTime(2020, 04, 03),
                BookingCode = "20% PROMO",
                ContractId = contractId
            };
            promotionalOffer.Details = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string>
            {
                En =
                    "Book on or before Friday, FEBRUARY 28, 2020, receive 20% discount, combinable with complimentary dinner dine-around offer on applicable dates"
            });
            promotionalOffers.Add(promotionalOffer);

            promotionalOffer = new RoomPromotionalOffer
            {
                DiscountPercent = 15,
                BookByDate = new DateTime(2020, 02, 28),
                ValidFromDate = new DateTime(2020, 04, 04),
                ValidToDate = new DateTime(2020, 04, 12),
                BookingCode = "15% PROMO",
                ContractId = contractId
            };
            promotionalOffer.Details = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string>
            {
                En =
                    "Book on or before Friday, FEBRUARY 28, 2020, receive 15% discount, combinable with complimentary dinner dine-around offer on applicable dates"
            });
            promotionalOffers.Add(promotionalOffer);

            promotionalOffer = new RoomPromotionalOffer
            {
                DiscountPercent = 25,
                BookByDate = new DateTime(2020, 02, 28),
                ValidFromDate = new DateTime(2020, 04, 13),
                ValidToDate = new DateTime(2020, 04, 18),
                BookingCode = "25% PROMO",
                ContractId = contractId
            };
            promotionalOffer.Details = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string>
            {
                En =
                    "Book on or before Friday, FEBRUARY 28, 2020, receive 25% discount, combinable with complimentary dinner dine-around offer on applicable dates"
            });
            promotionalOffers.Add(promotionalOffer);

            promotionalOffer = new RoomPromotionalOffer
            {
                DiscountPercent = 20,
                BookByDate = new DateTime(2020, 02, 28),
                ValidFromDate = new DateTime(2020, 04, 19),
                ValidToDate = new DateTime(2020, 05, 03),
                BookingCode = "20% PROMO",
                ContractId = contractId
            };
            promotionalOffer.Details = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string>
            {
                En =
                    "Book on or before Friday, FEBRUARY 28, 2020, receive 20% discount, combinable with complimentary dinner dine-around offer on applicable dates"
            });
            promotionalOffers.Add(promotionalOffer);

            promotionalOffer = new RoomPromotionalOffer
            {
                DiscountPercent = 30,
                BookByDate = new DateTime(2020, 03, 03),
                ValidFromDate = new DateTime(2020, 05, 04),
                ValidToDate = new DateTime(2020, 05, 31),
                BookingCode = "30% PROMO",
                ContractId = contractId
            };
            promotionalOffer.Details = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string>
            {
                En =
                    "Book on or before Friday, APRIL 03, 2020, receive 30% discount, combinable with complimentary dinner dine-around offer on applicable dates"
            });

            promotionalOffer = new RoomPromotionalOffer
            {
                DiscountPercent = 30,
                BookByDate = new DateTime(2020, 05, 08),
                ValidFromDate = new DateTime(2020, 06, 01),
                ValidToDate = new DateTime(2020, 09, 04),
                BookingCode = "30% PROMO",
                ContractId = contractId
            };
            promotionalOffer.Details = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string>
            {
                En =
                    "Book on or before Friday, MAY 08, 2020, receive 30% discount, combinable with complimentary dinner dine-around offer on applicable dates"
            });
            promotionalOffers.Add(promotionalOffer);

            promotionalOffer = new RoomPromotionalOffer
            {
                DiscountPercent = 30,
                BookByDate = new DateTime(2020, 08, 14),
                ValidFromDate = new DateTime(2020, 09, 05),
                ValidToDate = new DateTime(2020, 09, 20),
                BookingCode = "30% PROMO",
                ContractId = contractId
            };
            promotionalOffer.Details = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string>
            {
                En =
                    "Book on or before Friday, AUGUST 14, 2020, receive 30% discount, combinable with complimentary dinner dine-around offer on applicable dates"
            });
            promotionalOffers.Add(promotionalOffer);

            promotionalOffer = new RoomPromotionalOffer
            {
                DiscountPercent = 10,
                BookByDate = new DateTime(2020, 08, 14),
                ValidFromDate = new DateTime(2020, 10, 17),
                ValidToDate = new DateTime(2020, 11, 06),
                BookingCode = "10% PROMO",
                ContractId = contractId
            };

            promotionalOffer.Details = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string>
            {
                En =
                    "Book on or before Friday, AUGUST 14, 2020, receive 10% discount, combinable with complimentary dinner dine-around offer on applicable dates"
            });
            promotionalOffers.Add(promotionalOffer);

            promotionalOffer = new RoomPromotionalOffer
            {
                DiscountPercent = 20,
                BookByDate = new DateTime(2020, 08, 14),
                ValidFromDate = new DateTime(2020, 11, 07),
                ValidToDate = new DateTime(2020, 12, 04),
                BookingCode = "20% PROMO",
                ContractId = contractId
            };

            promotionalOffer.Details = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string>
            {
                En =
                    "Book on or before Friday, AUGUST 14, 2020, receive 20% discount, combinable with complimentary dinner dine-around offer on applicable dates"
            });
            promotionalOffers.Add(promotionalOffer);

            promotionalOffer = new RoomPromotionalOffer
            {
                DiscountPercent = 30,
                BookByDate = new DateTime(2020, 10, 02),
                ValidFromDate = new DateTime(2020, 12, 05),
                ValidToDate = new DateTime(2020, 12, 18),
                BookingCode = "30% PROMO",
                ContractId = contractId
            };
            promotionalOffer.Details = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string>
            {
                En =
                    "Book on or before Friday, OCTOBER 02, 2020, receive 30% discount, combinable with complimentary dinner dine-around offer on applicable dates"
            });

            promotionalOffers.Add(promotionalOffer);

            promotionalOffer = new RoomPromotionalOffer
            {
                DiscountPercent = 20,
                BookByDate = new DateTime(2020, 11, 01),
                ValidFromDate = new DateTime(2020, 12, 19),
                ValidToDate = new DateTime(2020, 12, 25),
                BookingCode = "20% PROMO",
                ContractId = contractId
            };

            promotionalOffer.Details = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string>
            {
                En =
                    "Book on or before Friday, NOVEMBER 01, 2020, receive 20% discount, combinable with complimentary dinner dine-around offer on applicable dates"
            });
            promotionalOffers.Add(promotionalOffer);

            promotionalOffer = new RoomPromotionalOffer
            {
                DiscountPercent = 10,
                BookByDate = new DateTime(2020, 08, 14),
                ValidFromDate = new DateTime(2020, 12, 26),
                ValidToDate = new DateTime(2021, 01, 03),
                BookingCode = "10% PROMO",
                ContractId = contractId
            };
            promotionalOffer.Details = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string>
            {
                En =
                    "Book on or before Friday, AUGUST 14, 2020, receive 10% discount, combinable with complimentary dinner dine-around offer on applicable dates"
            });
            promotionalOffers.Add(promotionalOffer);

            promotionalOffer = new RoomPromotionalOffer
            {
                DiscountPercent = 20,
                BookByDate = new DateTime(2020, 11, 01),
                ValidFromDate = new DateTime(2021, 01, 24),
                ValidToDate = new DateTime(2021, 03, 26),
                BookingCode = "20% PROMO",
                ContractId = contractId
            };
            promotionalOffer.Details = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string>
            {
                En =
                    "Book on or before Friday, NOVEMBER 01, 2020, receive 20% discount, combinable with complimentary dinner dine-around offer on applicable dates"
            });
            promotionalOffers.Add(promotionalOffer);

            promotionalOffer = new RoomPromotionalOffer
            {
                DiscountPercent = 10,
                BookByDate = new DateTime(2020, 11, 01),
                ValidFromDate = new DateTime(2021, 03, 27),
                ValidToDate = new DateTime(2021, 04, 10),
                BookingCode = "10% PROMO",
                ContractId = contractId
            };
            promotionalOffer.Details = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string>
            {
                En =
                    "Book on or before Friday, NOVEMBER 01, 2020, receive 10% discount, combinable with complimentary dinner dine-around offer on applicable dates"
            });
            promotionalOffers.Add(promotionalOffer);

            promotionalOffer = new RoomPromotionalOffer
            {
                DiscountPercent = 20,
                BookByDate = new DateTime(2021, 02, 26),
                ValidFromDate = new DateTime(2021, 04, 11),
                ValidToDate = new DateTime(2021, 05, 07),
                BookingCode = "10% PROMO",
                ContractId = contractId
            };
            promotionalOffer.Details = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string>
            {
                En =
                    "Book on or before Friday, FEBRUARY 26, 2021, receive 10% discount, combinable with complimentary dinner dine-around offer on applicable dates"
            });
            promotionalOffers.Add(promotionalOffer);

            AddPromotionalOffers(dbContext, new[] {32, 33, 34, 35}, promotionalOffers);

            promotionalOffers = new List<RoomPromotionalOffer>();

            promotionalOffer = new RoomPromotionalOffer
            {
                DiscountPercent = 30,
                BookByDate = new DateTime(2019, 12, 13),
                ValidFromDate = new DateTime(2020, 01, 08),
                ValidToDate = new DateTime(2020, 02, 21),
                BookingCode = "30% PROMO",
                ContractId = contractId
            };
            promotionalOffer.Details = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string>
            {
                En =
                    "Book on or before Friday, DECEMBER 13, 2019, receive 30% discount, combinable with complimentary dinner dine-around offer on applicable dates"
            });
            promotionalOffers.Add(promotionalOffer);

            promotionalOffer = new RoomPromotionalOffer
            {
                DiscountPercent = 25,
                BookByDate = new DateTime(2019, 12, 13),
                ValidFromDate = new DateTime(2020, 02, 22),
                ValidToDate = new DateTime(2020, 03, 20),
                BookingCode = "25% PROMO",
                ContractId = contractId
            };
            promotionalOffer.Details = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string>
            {
                En =
                    "Book on or before Friday, DECEMBER 13, 2019, receive 25% discount, combinable with complimentary dinner dine-around offer on applicable dates"
            });

            promotionalOffers.Add(promotionalOffer);

            promotionalOffer = new RoomPromotionalOffer
            {
                DiscountPercent = 35,
                BookByDate = new DateTime(2019, 12, 13),
                ValidFromDate = new DateTime(2020, 03, 21),
                ValidToDate = new DateTime(2020, 03, 27),
                BookingCode = "35% PROMO",
                ContractId = contractId
            };

            promotionalOffer.Details = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string>
            {
                En =
                    "Book on or before Friday, DECEMBER 13, 2019, receive 35% discount, combinable with complimentary dinner dine-around offer on applicable dates"
            });
            promotionalOffers.Add(promotionalOffer);

            promotionalOffer = new RoomPromotionalOffer
            {
                DiscountPercent = 35,
                BookByDate = new DateTime(2020, 02, 28),
                ValidFromDate = new DateTime(2020, 03, 28),
                ValidToDate = new DateTime(2020, 04, 03),
                BookingCode = "35% PROMO",
                ContractId = contractId
            };

            promotionalOffer.Details = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string>
            {
                En =
                    "Book on or before Friday, FEBRUARY 28, 2020, receive 35% discount, combinable with complimentary dinner dine-around offer on applicable dates"
            });
            promotionalOffers.Add(promotionalOffer);

            promotionalOffer = new RoomPromotionalOffer
            {
                DiscountPercent = 25,
                BookByDate = new DateTime(2020, 02, 28),
                ValidFromDate = new DateTime(2020, 04, 04),
                ValidToDate = new DateTime(2020, 04, 12),
                BookingCode = "25% PROMO",
                ContractId = contractId
            };
            promotionalOffer.Details = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string>
            {
                En =
                    "Book on or before Friday, FEBRUARY 28, 2020, receive 25% discount, combinable with complimentary dinner dine-around offer on applicable dates"
            });

            promotionalOffers.Add(promotionalOffer);

            promotionalOffer = new RoomPromotionalOffer
            {
                DiscountPercent = 35,
                BookByDate = new DateTime(2020, 02, 28),
                ValidFromDate = new DateTime(2020, 04, 13),
                ValidToDate = new DateTime(2020, 04, 18),
                BookingCode = "35% PROMO",
                ContractId = contractId
            };
            promotionalOffer.Details = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string>
            {
                En =
                    "Book on or before Friday, FEBRUARY 28, 2020, receive 35% discount, combinable with complimentary dinner dine-around offer on applicable dates"
            });

            promotionalOffers.Add(promotionalOffer);

            promotionalOffer = new RoomPromotionalOffer
            {
                DiscountPercent = 30,
                BookByDate = new DateTime(2020, 02, 28),
                ValidFromDate = new DateTime(2020, 04, 19),
                ValidToDate = new DateTime(2020, 05, 03),
                BookingCode = "30% PROMO",
                ContractId = contractId
            };
            promotionalOffer.Details = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string>
            {
                En =
                    "Book on or before Friday, FEBRUARY 28, 2020, receive 30% discount, combinable with complimentary dinner dine-around offer on applicable dates"
            });
            promotionalOffers.Add(promotionalOffer);

            promotionalOffer = new RoomPromotionalOffer
            {
                DiscountPercent = 35,
                BookByDate = new DateTime(2020, 04, 03),
                ValidFromDate = new DateTime(2020, 05, 04),
                ValidToDate = new DateTime(2020, 05, 31),
                BookingCode = "35% PROMO",
                ContractId = contractId
            };
            promotionalOffer.Details = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string>
            {
                En =
                    "Book on or before Friday, APRIL 03, 2020, receive 35% discount, combinable with complimentary dinner dine-around offer on applicable dates"
            });
            promotionalOffers.Add(promotionalOffer);

            promotionalOffer = new RoomPromotionalOffer
            {
                DiscountPercent = 30,
                BookByDate = new DateTime(2020, 05, 08),
                ValidFromDate = new DateTime(2020, 06, 01),
                ValidToDate = new DateTime(2020, 09, 04),
                BookingCode = "30% PROMO",
                ContractId = contractId
            };
            promotionalOffer.Details = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string>
            {
                En =
                    "Book on or before Friday, MAY 08, 2020, receive 30% discount, combinable with complimentary dinner dine-around offer on applicable dates"
            });
            promotionalOffers.Add(promotionalOffer);

            promotionalOffer = new RoomPromotionalOffer
            {
                DiscountPercent = 30,
                BookByDate = new DateTime(2020, 08, 14),
                ValidFromDate = new DateTime(2020, 09, 05),
                ValidToDate = new DateTime(2020, 09, 25),
                BookingCode = "30% PROMO",
                ContractId = contractId
            };
            promotionalOffer.Details = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string>
            {
                En =
                    "Book on or before Friday, AUGUST 14, 2020, receive 30% discount, combinable with complimentary dinner dine-around offer on applicable dates"
            });
            promotionalOffers.Add(promotionalOffer);

            promotionalOffer = new RoomPromotionalOffer
            {
                DiscountPercent = 30,
                BookByDate = new DateTime(2020, 08, 14),
                ValidFromDate = new DateTime(2020, 09, 26),
                ValidToDate = new DateTime(2020, 10, 16),
                BookingCode = "30% PROMO",
                ContractId = contractId
            };
            promotionalOffer.Details = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string>
            {
                En =
                    "Book on or before Friday, AUGUST 14, 2020, receive 30% discount, combinable with complimentary dinner dine-around offer on applicable dates"
            });
            promotionalOffers.Add(promotionalOffer);

            promotionalOffer = new RoomPromotionalOffer
            {
                DiscountPercent = 20,
                BookByDate = new DateTime(2020, 08, 14),
                ValidFromDate = new DateTime(2020, 10, 17),
                ValidToDate = new DateTime(2020, 12, 04),
                BookingCode = "20% PROMO",
                ContractId = contractId
            };
            promotionalOffer.Details = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string>
            {
                En =
                    "Book on or before Friday, AUGUST 14, 2020, receive 20% discount, combinable with complimentary dinner dine-around offer on applicable dates"
            });
            promotionalOffers.Add(promotionalOffer);

            promotionalOffer = new RoomPromotionalOffer
            {
                DiscountPercent = 30,
                BookByDate = new DateTime(2020, 10, 02),
                ValidFromDate = new DateTime(2020, 12, 05),
                ValidToDate = new DateTime(2020, 12, 18),
                BookingCode = "30% PROMO",
                ContractId = contractId
            };
            promotionalOffer.Details = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string>
            {
                En =
                    "Book on or before Friday, OCTOBER 02, 2020, receive 30% discount, combinable with complimentary dinner dine-around offer on applicable dates"
            });
            promotionalOffers.Add(promotionalOffer);

            promotionalOffer = new RoomPromotionalOffer
            {
                DiscountPercent = 20,
                BookByDate = new DateTime(2020, 11, 01),
                ValidFromDate = new DateTime(2020, 12, 19),
                ValidToDate = new DateTime(2020, 12, 25),
                BookingCode = "20% PROMO",
                ContractId = contractId
            };
            promotionalOffer.Details = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string>
            {
                En =
                    "Book on or before Friday, NOVEMBER 01, 2020, receive 20% discount, combinable with complimentary dinner dine-around offer on applicable dates"
            });
            promotionalOffers.Add(promotionalOffer);

            promotionalOffer = new RoomPromotionalOffer
            {
                DiscountPercent = 10,
                BookByDate = new DateTime(2020, 11, 01),
                ValidFromDate = new DateTime(2020, 12, 26),
                ValidToDate = new DateTime(2021, 01, 03),
                BookingCode = "10% PROMO",
                ContractId = contractId
            };
            promotionalOffer.Details = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string>
            {
                En =
                    "Book on or before Friday, NOVEMBER 01, 2020, receive 10% discount, combinable with complimentary dinner dine-around offer on applicable dates"
            });
            promotionalOffers.Add(promotionalOffer);

            promotionalOffer = new RoomPromotionalOffer
            {
                DiscountPercent = 20,
                BookByDate = new DateTime(2020, 11, 01),
                ValidFromDate = new DateTime(2021, 01, 04),
                ValidToDate = new DateTime(2021, 04, 10),
                BookingCode = "20% PROMO",
                ContractId = contractId
            };

            promotionalOffer.Details = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string>
            {
                En =
                    "Book on or before Friday, NOVEMBER 01, 2020, receive 20% discount, combinable with complimentary dinner dine-around offer on applicable dates"
            });
            promotionalOffers.Add(promotionalOffer);

            promotionalOffer = new RoomPromotionalOffer
            {
                DiscountPercent = 20,
                BookByDate = new DateTime(2021, 02, 26),
                ValidFromDate = new DateTime(2021, 04, 11),
                ValidToDate = new DateTime(2021, 05, 07),
                BookingCode = "20% PROMO",
                ContractId = contractId
            };

            promotionalOffer.Details = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string>
            {
                En =
                    "Book on or before Friday, FEBRUARY 26, 2020, receive 20% discount, combinable with complimentary dinner dine-around offer on applicable dates"
            });
            promotionalOffers.Add(promotionalOffer);

            AddPromotionalOffers(dbContext, new[] {27, 28, 29, 30, 31}, promotionalOffers);

            promotionalOffers = new List<RoomPromotionalOffer>();

            promotionalOffer = new RoomPromotionalOffer
            {
                DiscountPercent = 25,
                BookByDate = new DateTime(2019, 12, 13),
                ValidFromDate = new DateTime(2020, 01, 08),
                ValidToDate = new DateTime(2020, 02, 21),
                BookingCode = "25% PROMO",
                ContractId = contractId
            };

            promotionalOffer.Details = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string>
            {
                En =
                    "Book on or before Friday, DECEMBER 13, 2019, receive 25% discount, combinable with complimentary dinner dine-around offer on applicable dates"
            });
            promotionalOffers.Add(promotionalOffer);

            promotionalOffer = new RoomPromotionalOffer
            {
                DiscountPercent = 20,
                BookByDate = new DateTime(2019, 12, 13),
                ValidFromDate = new DateTime(2020, 02, 22),
                ValidToDate = new DateTime(2020, 03, 20),
                BookingCode = "20% PROMO",
                ContractId = contractId
            };
            promotionalOffer.Details = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string>
            {
                En =
                    "Book on or before Friday, DECEMBER 13, 2019, receive 20% discount, combinable with complimentary dinner dine-around offer on applicable dates"
            });
            promotionalOffers.Add(promotionalOffer);

            promotionalOffer = new RoomPromotionalOffer
            {
                DiscountPercent = 30,
                BookByDate = new DateTime(2019, 12, 13),
                ValidFromDate = new DateTime(2020, 03, 21),
                ValidToDate = new DateTime(2020, 03, 27),
                BookingCode = "30% PROMO",
                ContractId = contractId
            };

            promotionalOffer.Details = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string>
            {
                En =
                    "Book on or before Friday, DECEMBER 13, 2019, receive 30% discount, combinable with complimentary dinner dine-around offer on applicable dates"
            });
            promotionalOffers.Add(promotionalOffer);

            promotionalOffer = new RoomPromotionalOffer
            {
                DiscountPercent = 20,
                BookByDate = new DateTime(2020, 02, 28),
                ValidFromDate = new DateTime(2020, 03, 28),
                ValidToDate = new DateTime(2020, 04, 03),
                BookingCode = "20% PROMO",
                ContractId = contractId
            };

            promotionalOffer.Details = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string>
            {
                En =
                    "Book on or before Friday, FEBRUARY 28, 2020, receive 20% discount, combinable with complimentary dinner dine-around offer on applicable dates"
            });
            promotionalOffers.Add(promotionalOffer);

            promotionalOffer = new RoomPromotionalOffer
            {
                DiscountPercent = 20,
                BookByDate = new DateTime(2020, 02, 28),
                ValidFromDate = new DateTime(2020, 04, 04),
                ValidToDate = new DateTime(2020, 04, 12),
                BookingCode = "20% PROMO",
                ContractId = contractId
            };
            promotionalOffer.Details = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string>
            {
                En =
                    "Book on or before Friday, FEBRUARY 28, 2020, receive 20% discount, combinable with complimentary dinner dine-around offer on applicable dates"
            });
            promotionalOffers.Add(promotionalOffer);

            promotionalOffer = new RoomPromotionalOffer
            {
                DiscountPercent = 30,
                BookByDate = new DateTime(2020, 02, 28),
                ValidFromDate = new DateTime(2020, 04, 13),
                ValidToDate = new DateTime(2020, 04, 18),
                BookingCode = "30% PROMO",
                ContractId = contractId
            };
            promotionalOffer.Details = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string>
            {
                En =
                    "Book on or before Friday, FEBRUARY 28, 2020, receive 30% discount, combinable with complimentary dinner dine-around offer on applicable dates"
            });
            promotionalOffers.Add(promotionalOffer);

            promotionalOffer = new RoomPromotionalOffer
            {
                DiscountPercent = 25,
                BookByDate = new DateTime(2020, 02, 28),
                ValidFromDate = new DateTime(2020, 04, 19),
                ValidToDate = new DateTime(2020, 05, 03),
                BookingCode = "25% PROMO",
                ContractId = contractId
            };
            promotionalOffer.Details = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string>
            {
                En =
                    "Book on or before Friday, FEBRUARY 28, 2020, receive 25% discount, combinable with complimentary dinner dine-around offer on applicable dates"
            });
            promotionalOffers.Add(promotionalOffer);

            promotionalOffer = new RoomPromotionalOffer
            {
                DiscountPercent = 30,
                BookByDate = new DateTime(2020, 04, 03),
                ValidFromDate = new DateTime(2020, 05, 04),
                ValidToDate = new DateTime(2020, 05, 31),
                BookingCode = "30% PROMO",
                ContractId = contractId
            };

            promotionalOffer.Details = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string>
            {
                En =
                    "Book on or before Friday, APRIL 04, 2020, receive 30% discount, combinable with complimentary dinner dine-around offer on applicable dates"
            });
            promotionalOffers.Add(promotionalOffer);

            promotionalOffer = new RoomPromotionalOffer
            {
                DiscountPercent = 30,
                BookByDate = new DateTime(2020, 04, 03),
                ValidFromDate = new DateTime(2020, 06, 01),
                ValidToDate = new DateTime(2020, 09, 04),
                BookingCode = "30% PROMO",
                ContractId = contractId
            };
            promotionalOffer.Details = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string>
            {
                En =
                    "Book on or before Friday, MAY 08, 2020, receive 30% discount, combinable with complimentary dinner dine-around offer on applicable dates"
            });
            promotionalOffers.Add(promotionalOffer);

            promotionalOffer = new RoomPromotionalOffer
            {
                DiscountPercent = 40,
                BookByDate = new DateTime(2020, 08, 14),
                ValidFromDate = new DateTime(2020, 09, 05),
                ValidToDate = new DateTime(2020, 09, 25),
                BookingCode = "40% PROMO",
                ContractId = contractId
            };

            promotionalOffer.Details = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string>
            {
                En =
                    "Book on or before Friday, AUGUST 14, 2020, receive 40% discount, combinable with complimentary dinner dine-around offer on applicable dates"
            });
            promotionalOffers.Add(promotionalOffer);

            promotionalOffer = new RoomPromotionalOffer
            {
                DiscountPercent = 25,
                BookByDate = new DateTime(2020, 08, 14),
                ValidFromDate = new DateTime(2020, 09, 26),
                ValidToDate = new DateTime(2020, 10, 16),
                BookingCode = "25% PROMO",
                ContractId = contractId
            };
            promotionalOffer.Details = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string>
            {
                En =
                    "Book on or before Friday, AUGUST 14, 2020, receive 25% discount, combinable with complimentary dinner dine-around offer on applicable dates"
            });
            promotionalOffers.Add(promotionalOffer);

            promotionalOffer = new RoomPromotionalOffer
            {
                DiscountPercent = 20,
                BookByDate = new DateTime(2020, 08, 14),
                ValidFromDate = new DateTime(2020, 10, 17),
                ValidToDate = new DateTime(2020, 12, 04),
                BookingCode = "20% PROMO",
                ContractId = contractId
            };
            promotionalOffer.Details = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string>
            {
                En =
                    "Book on or before Friday, AUGUST 14, 2020, receive 20% discount, combinable with complimentary dinner dine-around offer on applicable dates"
            });
            promotionalOffers.Add(promotionalOffer);

            promotionalOffer = new RoomPromotionalOffer
            {
                DiscountPercent = 30,
                BookByDate = new DateTime(2020, 10, 02),
                ValidFromDate = new DateTime(2020, 12, 05),
                ValidToDate = new DateTime(2020, 12, 18),
                BookingCode = "30% PROMO"
            };

            promotionalOffer.Details = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string>
            {
                En =
                    "Book on or before Friday, OCTOBER 02, 2020, receive 30% discount, combinable with complimentary dinner dine-around offer on applicable dates"
            });
            promotionalOffers.Add(promotionalOffer);

            promotionalOffer = new RoomPromotionalOffer
            {
                DiscountPercent = 20,
                BookByDate = new DateTime(2020, 11, 01),
                ValidFromDate = new DateTime(2020, 12, 19),
                ValidToDate = new DateTime(2020, 12, 25),
                BookingCode = "20% PROMO",
                ContractId = contractId
            };
            promotionalOffer.Details = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string>
            {
                En =
                    "Book on or before Friday, NOVEMBER 01, 2020, receive 20% discount, combinable with complimentary dinner dine-around offer on applicable dates"
            });
            promotionalOffers.Add(promotionalOffer);

            promotionalOffer = new RoomPromotionalOffer
            {
                DiscountPercent = 10,
                BookByDate = new DateTime(2020, 11, 01),
                ValidFromDate = new DateTime(2020, 12, 26),
                ValidToDate = new DateTime(2021, 01, 03),
                BookingCode = "10% PROMO",
                ContractId = contractId
            };

            promotionalOffer.Details = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string>
            {
                En =
                    "Book on or before Friday, NOVEMBER 01, 2020, receive 10% discount, combinable with complimentary dinner dine-around offer on applicable dates"
            });
            promotionalOffers.Add(promotionalOffer);

            promotionalOffer = new RoomPromotionalOffer
            {
                DiscountPercent = 20,
                BookByDate = new DateTime(2020, 11, 01),
                ValidFromDate = new DateTime(2021, 01, 04),
                ValidToDate = new DateTime(2021, 04, 10),
                BookingCode = "20% PROMO",
                ContractId = contractId
            };
            promotionalOffer.Details = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string>
            {
                En =
                    "Book on or before Friday, NOVEMBER 01, 2020, receive 20% discount, combinable with complimentary dinner dine-around offer on applicable dates"
            });
            promotionalOffers.Add(promotionalOffer);

            promotionalOffer = new RoomPromotionalOffer
            {
                DiscountPercent = 20,
                BookByDate = new DateTime(2021, 02, 26),
                ValidFromDate = new DateTime(2021, 04, 11),
                ValidToDate = new DateTime(2021, 05, 07),
                BookingCode = "20% PROMO",
                ContractId = contractId
            };
            promotionalOffer.Details = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string>
            {
                En =
                    "Book on or before Friday, FEBRUARY 26, 2021, receive 20% discount, combinable with complimentary dinner dine-around offer on applicable dates"
            });
            promotionalOffers.Add(promotionalOffer);
            AddPromotionalOffers(dbContext, new[] {20, 21, 22, 23, 24, 25, 26}, promotionalOffers);

            #endregion

            dbContext.SaveChanges();

            #region AddRoomAvailabilityRestrictions //Test data

            dbContext.RoomAvailabilityRestrictions.AddRange(
                new RoomAvailabilityRestrictions()
                {
                    RoomId = 20,
                    StartDate = new DateTime(2020, 4, 1),
                    EndDate = new DateTime(2020, 4, 10),
                    Restrictions = SaleRestrictions.StopSale
                },
                new RoomAvailabilityRestrictions
                {
                    RoomId = 20, StartDate = new DateTime(2020, 7, 10), EndDate = new DateTime(2020, 7, 13)
                },
                new RoomAvailabilityRestrictions
                {
                    RoomId = 20, StartDate = new DateTime(2020, 10, 15), EndDate = new DateTime(2020, 10, 18)
                },
                new RoomAvailabilityRestrictions
                {
                    RoomId = 20, StartDate = new DateTime(2021, 1, 10), EndDate = new DateTime(2021, 1, 12)
                },
                new RoomAvailabilityRestrictions
                {
                    RoomId = 21, StartDate = new DateTime(2020, 2, 5), EndDate = new DateTime(2020, 2, 7)
                },
                new RoomAvailabilityRestrictions
                {
                    RoomId = 21, StartDate = new DateTime(2020, 10, 15), EndDate = new DateTime(2020, 10, 18)
                },
                new RoomAvailabilityRestrictions
                {
                    RoomId = 21, StartDate = new DateTime(2020, 12, 28), EndDate = new DateTime(2021, 1, 2)
                },
                new RoomAvailabilityRestrictions
                {
                    RoomId = 22, StartDate = new DateTime(2020, 2, 5), EndDate = new DateTime(2020, 2, 7)
                },
                new RoomAvailabilityRestrictions
                {
                    RoomId = 22, StartDate = new DateTime(2020, 10, 15), EndDate = new DateTime(2020, 10, 18)
                },
                new RoomAvailabilityRestrictions
                {
                    RoomId = 22, StartDate = new DateTime(2020, 12, 28), EndDate = new DateTime(2020, 12, 29)
                },
                new RoomAvailabilityRestrictions
                {
                    RoomId = 23, StartDate = new DateTime(2020, 2, 5), EndDate = new DateTime(2020, 2, 10)
                },
                new RoomAvailabilityRestrictions
                {
                    RoomId = 23, StartDate = new DateTime(2020, 11, 1), EndDate = new DateTime(2020, 11, 3)
                },
                new RoomAvailabilityRestrictions
                {
                    RoomId = 23, StartDate = new DateTime(2020, 8, 7), EndDate = new DateTime(2020, 8, 10)
                },
                new RoomAvailabilityRestrictions
                {
                    RoomId = 24, StartDate = new DateTime(2020, 3, 8), EndDate = new DateTime(2020, 3, 10)
                },
                new RoomAvailabilityRestrictions
                {
                    RoomId = 24, StartDate = new DateTime(2020, 7, 1), EndDate = new DateTime(2020, 7, 3)
                },
                new RoomAvailabilityRestrictions
                {
                    RoomId = 24, StartDate = new DateTime(2020, 9, 3), EndDate = new DateTime(2020, 9, 5)
                },
                new RoomAvailabilityRestrictions
                {
                    RoomId = 25, StartDate = new DateTime(2020, 1, 29), EndDate = new DateTime(2020, 2, 1)
                },
                new RoomAvailabilityRestrictions
                {
                    RoomId = 25, StartDate = new DateTime(2020, 9, 15), EndDate = new DateTime(2020, 9, 16)
                },
                new RoomAvailabilityRestrictions
                {
                    RoomId = 25, StartDate = new DateTime(2020, 12, 30), EndDate = new DateTime(2021, 1, 2)
                },
                new RoomAvailabilityRestrictions
                {
                    RoomId = 26, StartDate = new DateTime(2020, 1, 10), EndDate = new DateTime(2020, 1, 12)
                },
                new RoomAvailabilityRestrictions
                {
                    RoomId = 26, StartDate = new DateTime(2020, 3, 4), EndDate = new DateTime(2020, 3, 7)
                },
                new RoomAvailabilityRestrictions
                {
                    RoomId = 26, StartDate = new DateTime(2020, 6, 9), EndDate = new DateTime(2020, 6, 11)
                },
                new RoomAvailabilityRestrictions
                {
                    RoomId = 27, StartDate = new DateTime(2020, 1, 18), EndDate = new DateTime(2020, 1, 19)
                },
                new RoomAvailabilityRestrictions
                {
                    RoomId = 27, StartDate = new DateTime(2020, 2, 27), EndDate = new DateTime(2020, 3, 1)
                },
                new RoomAvailabilityRestrictions
                {
                    RoomId = 27, StartDate = new DateTime(2020, 12, 26), EndDate = new DateTime(2020, 12, 27)
                },
                new RoomAvailabilityRestrictions
                {
                    RoomId = 28, StartDate = new DateTime(2020, 4, 5), EndDate = new DateTime(2020, 4, 7)
                },
                new RoomAvailabilityRestrictions
                {
                    RoomId = 28, StartDate = new DateTime(2020, 11, 3), EndDate = new DateTime(2020, 11, 5)
                },
                new RoomAvailabilityRestrictions
                {
                    RoomId = 28, StartDate = new DateTime(2020, 12, 31), EndDate = new DateTime(2021, 1, 1)
                },
                new RoomAvailabilityRestrictions
                {
                    RoomId = 29, StartDate = new DateTime(2020, 2, 7), EndDate = new DateTime(2020, 2, 10)
                },
                new RoomAvailabilityRestrictions
                {
                    RoomId = 29, StartDate = new DateTime(2020, 4, 3), EndDate = new DateTime(2020, 4, 5)
                },
                new RoomAvailabilityRestrictions
                {
                    RoomId = 29, StartDate = new DateTime(2020, 12, 1), EndDate = new DateTime(2021, 1, 1)
                },
                new RoomAvailabilityRestrictions
                {
                    RoomId = 30, StartDate = new DateTime(2020, 6, 1), EndDate = new DateTime(2020, 9, 1)
                },
                new RoomAvailabilityRestrictions
                {
                    RoomId = 30, StartDate = new DateTime(2020, 9, 2), EndDate = new DateTime(2020, 9, 4)
                },
                new RoomAvailabilityRestrictions
                {
                    RoomId = 30, StartDate = new DateTime(2020, 12, 1), EndDate = new DateTime(2021, 12, 2)
                },
                new RoomAvailabilityRestrictions
                {
                    RoomId = 31, StartDate = new DateTime(2020, 1, 6), EndDate = new DateTime(2020, 1, 8)
                },
                new RoomAvailabilityRestrictions
                {
                    RoomId = 31, StartDate = new DateTime(2020, 2, 14), EndDate = new DateTime(2020, 2, 16)
                },
                new RoomAvailabilityRestrictions
                {
                    RoomId = 31, StartDate = new DateTime(2020, 7, 5), EndDate = new DateTime(2021, 7, 8)
                },
                new RoomAvailabilityRestrictions
                {
                    RoomId = 32, StartDate = new DateTime(2020, 9, 12), EndDate = new DateTime(2020, 9, 15)
                },
                new RoomAvailabilityRestrictions
                {
                    RoomId = 32, StartDate = new DateTime(2020, 10, 2), EndDate = new DateTime(2020, 10, 4)
                },
                new RoomAvailabilityRestrictions
                {
                    RoomId = 32, StartDate = new DateTime(2019, 12, 3), EndDate = new DateTime(2019, 12, 4)
                },
                new RoomAvailabilityRestrictions
                {
                    RoomId = 33, StartDate = new DateTime(2020, 3, 6), EndDate = new DateTime(2020, 3, 8)
                },
                new RoomAvailabilityRestrictions
                {
                    RoomId = 33, StartDate = new DateTime(2020, 4, 10), EndDate = new DateTime(2020, 4, 13)
                },
                new RoomAvailabilityRestrictions
                {
                    RoomId = 33, StartDate = new DateTime(2020, 7, 1), EndDate = new DateTime(2020, 8, 1)
                },
                new RoomAvailabilityRestrictions
                {
                    RoomId = 34, StartDate = new DateTime(2020, 3, 3), EndDate = new DateTime(2020, 3, 5)
                },
                new RoomAvailabilityRestrictions
                {
                    RoomId = 34, StartDate = new DateTime(2020, 4, 16), EndDate = new DateTime(2020, 4, 19)
                },
                new RoomAvailabilityRestrictions
                {
                    RoomId = 34, StartDate = new DateTime(2020, 11, 23), EndDate = new DateTime(2020, 11, 26)
                },
                new RoomAvailabilityRestrictions
                {
                    RoomId = 35, StartDate = new DateTime(2020, 5, 1), EndDate = new DateTime(2020, 5, 6)
                },
                new RoomAvailabilityRestrictions
                {
                    RoomId = 35, StartDate = new DateTime(2020, 7, 19), EndDate = new DateTime(2020, 7, 21)
                },
                new RoomAvailabilityRestrictions
                {
                    RoomId = 35, StartDate = new DateTime(2020, 11, 28), EndDate = new DateTime(2020, 12, 3)
                });

            #endregion

            dbContext.SaveChanges();

            #region AddAllocationRequirements

            roomIds = new[] {20, 21, 22, 23, 24, 25, 27, 28, 29, 30, 31, 32, 33, 34};
            var roomAllocationRequirements = new[]
            {
                new RoomAllocationRequirement
                {
                    ReleasePeriod = new ReleasePeriod {Days = 21},
                    StartDate = new DateTime(2020, 01, 08),
                    EndDate = new DateTime(2020, 01, 14),
                },
                new RoomAllocationRequirement
                {
                    ReleasePeriod = new ReleasePeriod {Days = 21},
                    StartDate = new DateTime(2020, 01, 25),
                    EndDate = new DateTime(2020, 02, 07)
                },
                new RoomAllocationRequirement
                {
                    ReleasePeriod = new ReleasePeriod {Days = 21},
                    StartDate = new DateTime(2020, 02, 22),
                    EndDate = new DateTime(2020, 03, 20)
                },
                new RoomAllocationRequirement
                {
                    ReleasePeriod = new ReleasePeriod {Days = 21},
                    StartDate = new DateTime(2020, 04, 19),
                    EndDate = new DateTime(2020, 05, 03)
                },
                new RoomAllocationRequirement
                {
                    ReleasePeriod = new ReleasePeriod {Days = 21},
                    StartDate = new DateTime(2020, 01, 15),
                    EndDate = new DateTime(2020, 01, 24)
                },
                new RoomAllocationRequirement
                {
                    ReleasePeriod = new ReleasePeriod {Days = 21},
                    StartDate = new DateTime(2020, 09, 26),
                    EndDate = new DateTime(2020, 10, 09)
                }
            };
            AddRoomAllocationRequirements(dbContext, roomIds, roomAllocationRequirements);

            roomIds = new[] {20, 21, 22, 23, 24, 25, 27, 28, 29, 30, 31};
            roomAllocationRequirements = new[]
            {
                new RoomAllocationRequirement
                {
                    ReleasePeriod = new ReleasePeriod {Days = 21},
                    StartDate = new DateTime(2020, 02, 08),
                    EndDate = new DateTime(2020, 02, 21)
                },
                new RoomAllocationRequirement
                {
                    ReleasePeriod = new ReleasePeriod {Days = 21},
                    StartDate = new DateTime(2020, 03, 21),
                    EndDate = new DateTime(2020, 03, 27)
                },
                new RoomAllocationRequirement
                {
                    ReleasePeriod = new ReleasePeriod {Days = 21},
                    StartDate = new DateTime(2020, 04, 13),
                    EndDate = new DateTime(2020, 04, 18)
                }
            };
            AddRoomAllocationRequirements(dbContext, roomIds, roomAllocationRequirements);

            roomIds = new[] {32, 33, 34};
            roomAllocationRequirements = new[]
            {
                new RoomAllocationRequirement
                {
                    ReleasePeriod = new ReleasePeriod {Days = 21},
                    MinimumStayNights = 5,
                    StartDate = new DateTime(2020, 02, 08),
                    EndDate = new DateTime(2020, 02, 21)
                },
                new RoomAllocationRequirement
                {
                    ReleasePeriod = new ReleasePeriod {Days = 21},
                    MinimumStayNights = 5,
                    StartDate = new DateTime(2020, 03, 21),
                    EndDate = new DateTime(2020, 03, 27)
                },
                new RoomAllocationRequirement
                {
                    ReleasePeriod = new ReleasePeriod {Days = 21},
                    MinimumStayNights = 5,
                    StartDate = new DateTime(2020, 04, 13),
                    EndDate = new DateTime(2020, 04, 18)
                }
            };
            AddRoomAllocationRequirements(dbContext, roomIds, roomAllocationRequirements);

            roomIds = new[] {20, 21, 22, 23, 24, 25, 27, 28, 29, 30, 31, 32, 33, 34};
            roomAllocationRequirements = new[]
            {
                new RoomAllocationRequirement
                {
                    ReleasePeriod = new ReleasePeriod {Days = 28},
                    MinimumStayNights = 5,
                    StartDate = new DateTime(2020, 03, 28),
                    EndDate = new DateTime(2020, 04, 12)
                }
            };
            AddRoomAllocationRequirements(dbContext, roomIds, roomAllocationRequirements);

            roomIds = new[] {20, 21, 22, 23, 24, 25, 27, 28, 29, 30, 31, 32, 33, 34};
            roomAllocationRequirements = new[]
            {
                new RoomAllocationRequirement
                {
                    ReleasePeriod = new ReleasePeriod {Days = 14},
                    StartDate = new DateTime(2020, 05, 04),
                    EndDate = new DateTime(2020, 05, 31)
                },
                new RoomAllocationRequirement
                {
                    ReleasePeriod = new ReleasePeriod {Days = 14},
                    StartDate = new DateTime(2020, 09, 16),
                    EndDate = new DateTime(2020, 09, 25)
                }
            };
            AddRoomAllocationRequirements(dbContext, roomIds, roomAllocationRequirements);

            roomIds = new[] {20, 21, 22, 23, 24, 25, 27, 28, 29, 30, 31, 32, 33, 34};
            roomAllocationRequirements = new[]
            {
                new RoomAllocationRequirement
                {
                    ReleasePeriod = new ReleasePeriod {Days = 7},
                    StartDate = new DateTime(2020, 06, 01),
                    EndDate = new DateTime(2020, 06, 14)
                }
            };
            AddRoomAllocationRequirements(dbContext, roomIds, roomAllocationRequirements);

            roomIds = new[] {20, 21, 22, 23, 24, 25, 32, 33, 34};
            roomAllocationRequirements = new[]
            {
                new RoomAllocationRequirement
                {
                    ReleasePeriod = new ReleasePeriod {Days = 7},
                    StartDate = new DateTime(2020, 06, 15),
                    EndDate = new DateTime(2020, 09, 04)
                }
            };
            AddRoomAllocationRequirements(dbContext, roomIds, roomAllocationRequirements);

            roomIds = new[] {20, 21, 22, 23, 24, 25, 32, 33, 34};
            roomAllocationRequirements = new[]
            {
                new RoomAllocationRequirement
                {
                    ReleasePeriod = new ReleasePeriod {Days = 7},
                    StartDate = new DateTime(2020, 09, 05),
                    EndDate = new DateTime(2020, 09, 15)
                }
            };
            AddRoomAllocationRequirements(dbContext, roomIds, roomAllocationRequirements);

            roomIds = new[] {20, 21, 22, 23, 24, 25, 27, 28, 29, 30, 31, 32, 33, 34};
            roomAllocationRequirements = new[]
            {
                new RoomAllocationRequirement
                {
                    ReleasePeriod = new ReleasePeriod {Days = 7},
                    StartDate = new DateTime(2020, 10, 10),
                    EndDate = new DateTime(2020, 10, 16)
                }
            };
            AddRoomAllocationRequirements(dbContext, roomIds, roomAllocationRequirements);

            roomAllocationRequirements = new[]
            {
                new RoomAllocationRequirement
                {
                    ReleasePeriod = new ReleasePeriod {Days = 7},
                    MinimumStayNights = 3,
                    StartDate = new DateTime(2020, 10, 17),
                    EndDate = new DateTime(2020, 10, 23)
                }
            };
            AddRoomAllocationRequirements(dbContext, roomIds, roomAllocationRequirements);

            roomAllocationRequirements = new[]
            {
                new RoomAllocationRequirement
                {
                    ReleasePeriod = new ReleasePeriod {Days = 35},
                    MinimumStayNights = 5,
                    StartDate = new DateTime(2020, 10, 24),
                    EndDate = new DateTime(2020, 11, 06)
                }
            };
            AddRoomAllocationRequirements(dbContext, roomIds, roomAllocationRequirements);

            roomAllocationRequirements = new[]
            {
                new RoomAllocationRequirement
                {
                    ReleasePeriod = new ReleasePeriod {Days = 28},
                    MinimumStayNights = 3,
                    StartDate = new DateTime(2020, 11, 07),
                    EndDate = new DateTime(2020, 12, 04)
                },
                new RoomAllocationRequirement
                {
                    ReleasePeriod = new ReleasePeriod {Days = 28},
                    MinimumStayNights = 3,
                    StartDate = new DateTime(2020, 12, 19),
                    EndDate = new DateTime(2020, 12, 25)
                }
            };
            AddRoomAllocationRequirements(dbContext, roomIds, roomAllocationRequirements);

            roomAllocationRequirements = new[]
            {
                new RoomAllocationRequirement
                {
                    ReleasePeriod = new ReleasePeriod {Days = 21},
                    MinimumStayNights = 3,
                    StartDate = new DateTime(2020, 12, 05),
                    EndDate = new DateTime(2020, 12, 18)
                }
            };
            AddRoomAllocationRequirements(dbContext, roomIds, roomAllocationRequirements);

            roomAllocationRequirements = new[]
            {
                new RoomAllocationRequirement
                {
                    ReleasePeriod = new ReleasePeriod {Date = new DateTime(2020, 11, 01)},
                    MinimumStayNights = 7,
                    StartDate = new DateTime(2020, 12, 26),
                    EndDate = new DateTime(2021, 01, 03)
                }
            };
            AddRoomAllocationRequirements(dbContext, roomIds, roomAllocationRequirements);

            roomIds = new[] {20, 21, 22, 23, 24, 25, 27, 28, 29, 30, 31};
            roomAllocationRequirements = new[]
            {
                new RoomAllocationRequirement
                {
                    ReleasePeriod = new ReleasePeriod {Days = 28},
                    MinimumStayNights = 3,
                    StartDate = new DateTime(2021, 01, 04),
                    EndDate = new DateTime(2021, 01, 10)
                },
                new RoomAllocationRequirement
                {
                    ReleasePeriod = new ReleasePeriod {Days = 28},
                    MinimumStayNights = 3,
                    StartDate = new DateTime(2021, 02, 06),
                    EndDate = new DateTime(2021, 02, 19)
                },
                new RoomAllocationRequirement
                {
                    ReleasePeriod = new ReleasePeriod {Days = 28},
                    MinimumStayNights = 3,
                    StartDate = new DateTime(2021, 03, 21),
                    EndDate = new DateTime(2021, 03, 26)
                }
            };
            AddRoomAllocationRequirements(dbContext, roomIds, roomAllocationRequirements);

            roomIds = new[] {32, 33, 34};
            roomAllocationRequirements = new[]
            {
                new RoomAllocationRequirement
                {
                    ReleasePeriod = new ReleasePeriod {Days = 28},
                    MinimumStayNights = 5,
                    StartDate = new DateTime(2021, 01, 04),
                    EndDate = new DateTime(2021, 01, 10)
                },
                new RoomAllocationRequirement
                {
                    ReleasePeriod = new ReleasePeriod {Days = 28},
                    MinimumStayNights = 5,
                    StartDate = new DateTime(2021, 02, 06),
                    EndDate = new DateTime(2021, 02, 19)
                },
                new RoomAllocationRequirement
                {
                    ReleasePeriod = new ReleasePeriod {Days = 28},
                    MinimumStayNights = 5,
                    StartDate = new DateTime(2021, 03, 21),
                    EndDate = new DateTime(2021, 03, 26)
                }
            };
            AddRoomAllocationRequirements(dbContext, roomIds, roomAllocationRequirements);

            roomIds = new[] {20, 21, 22, 23, 24, 25, 27, 28, 29, 30, 31, 32, 33, 34};
            roomAllocationRequirements = new[]
            {
                new RoomAllocationRequirement
                {
                    ReleasePeriod = new ReleasePeriod {Days = 28},
                    MinimumStayNights = 3,
                    StartDate = new DateTime(2021, 01, 11),
                    EndDate = new DateTime(2021, 02, 05)
                },
                new RoomAllocationRequirement
                {
                    ReleasePeriod = new ReleasePeriod {Days = 28},
                    MinimumStayNights = 3,
                    StartDate = new DateTime(2021, 02, 20),
                    EndDate = new DateTime(2021, 03, 19)
                }
            };
            AddRoomAllocationRequirements(dbContext, roomIds, roomAllocationRequirements);

            roomIds = new[] {20, 21, 22, 23, 24, 25, 27, 28, 29, 30, 31, 32, 33, 34};
            roomAllocationRequirements = new[]
            {
                new RoomAllocationRequirement
                {
                    ReleasePeriod = new ReleasePeriod {Days = 35},
                    MinimumStayNights = 5,
                    StartDate = new DateTime(2021, 03, 27),
                    EndDate = new DateTime(2021, 04, 10)
                }
            };
            AddRoomAllocationRequirements(dbContext, roomIds, roomAllocationRequirements);

            roomIds = new[] {20, 21, 22, 23, 24, 25, 27, 28, 29, 30, 31, 32, 33, 34};
            roomAllocationRequirements = new[]
            {
                new RoomAllocationRequirement
                {
                    ReleasePeriod = new ReleasePeriod {Days = 21},
                    StartDate = new DateTime(2021, 04, 11),
                    EndDate = new DateTime(2021, 05, 07)
                }
            };

            AddRoomAllocationRequirements(dbContext, roomIds, roomAllocationRequirements);

            #endregion

            dbContext.SaveChanges();
        }

        private static void AddJumeriahContract(DirectContractsDbContext dbContext)
        {
            var hotelId = int.MaxValue - 1;
            var contractId = int.MaxValue - 1;
            var contract = dbContext.Contracts.SingleOrDefault(c => c.Name == "Burj Al Arab Jumeirah contract");
            if (contract != null) return;

            dbContext.Contracts.Add(new Contract
            {
                Id = contractId,
                Name = "Burj Al Arab Jumeirah contract",
                Description = "Burj Al Arab Jumeirah contract",
                ContractManagerId = int.MaxValue,
                ValidFrom = new DateTime(2020, 01, 01),
                ValidTo = new DateTime(2020, 12, 31)
            });
            dbContext.SaveChanges();

            dbContext.ContractAccommodationRelations.Add(new ContractAccommodationRelation()
            {
                AccommodationId = hotelId, ContractId = contractId
            });
            dbContext.SaveChanges();

            var seasons = new List<(int, DateTime, DateTime)>
            {
                (GetSeasonId(51), new DateTime(2020, 01, 14), new DateTime(2020, 01, 31)),
                (GetSeasonId(58), new DateTime(2020, 09, 01), new DateTime(2020, 10, 12)),
                (GetSeasonId(62), new DateTime(2020, 12, 02), new DateTime(2020, 12, 21))
            };
            AddSeasons(dbContext, contractId, "SHOULDER", seasons);

            seasons = new List<(int, DateTime, DateTime)>
            {
                (GetSeasonId(52), new DateTime(2020, 02, 01), new DateTime(2020, 02, 04)),
                (GetSeasonId(54), new DateTime(2020, 02, 12), new DateTime(2020, 03, 27)),
                (GetSeasonId(56), new DateTime(2020, 04, 22), new DateTime(2020, 05, 05)),
                (GetSeasonId(59), new DateTime(2020, 10, 13), new DateTime(2020, 10, 19)),
                (GetSeasonId(61), new DateTime(2020, 11, 10), new DateTime(2020, 12, 01)),
                (GetSeasonId(63), new DateTime(2020, 12, 22), new DateTime(2020, 12, 26)),
                (GetSeasonId(65), new DateTime(2021, 01, 05), new DateTime(2021, 01, 13))
            };
            AddSeasons(dbContext, contractId, "HIGH", seasons);

            seasons = new List<(int, DateTime, DateTime)>
            {
                (GetSeasonId(53), new DateTime(2020, 02, 05), new DateTime(2020, 02, 11)),
                (GetSeasonId(55), new DateTime(2020, 03, 28), new DateTime(2020, 04, 21)),
                (GetSeasonId(60), new DateTime(2020, 10, 20), new DateTime(2020, 11, 09)),
            };
            AddSeasons(dbContext, contractId, "PEAK", seasons);

            seasons = new List<(int, DateTime, DateTime)>
            {
                (GetSeasonId(57), new DateTime(2020, 05, 06), new DateTime(2020, 08, 31)),
            };
            AddSeasons(dbContext, contractId, "LOW", seasons);

            seasons = new List<(int, DateTime, DateTime)>
            {
                (GetSeasonId(64), new DateTime(2020, 12, 27), new DateTime(2021, 01, 04))
            };
            AddSeasons(dbContext, contractId, "FESTIVE", seasons);

            if (IsAccommodationExist(dbContext, "Burj Al Arab Jumeirah")) return;

            #region AddAccommodation

            var accommodation = new Accommodation
            {
                Id = hotelId,
                ContractManagerId = int.MaxValue,
                Rating = AccommodationRating.FiveStars,
                PropertyType = PropertyTypes.Hotels,
                ContactInfo = new ContactInfo {Email = "info@jumeirah.com", Phone = "+971 4 3665000"},
                CheckInTime = "14:00",
                CheckOutTime = "12:00",
                Coordinates = new Point(55.153219, 25.097596),
                OccupancyDefinition = new OccupancyDefinition
                {
                    Infant = new AgeRange {LowerBound = 0, UpperBound = 3},
                    Child = new AgeRange {LowerBound = 4, UpperBound = 11},
                    Teenager = new AgeRange {LowerBound = 12, UpperBound = 16},
                    Adult = new AgeRange {LowerBound = 17, UpperBound = 200},
                },
                LocationId = GetLocationId(dbContext)
            };

            accommodation.Name = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string>
            {
                Ar = "برج العرب جميرا", En = "Burj Al Arab Jumeirah", Ru = "Burj Al Arab Jumeirah"
            });

            accommodation.TextualDescription = JsonDocumentUtilities.CreateJDocument(
                new MultiLanguage<TextualDescription>
                {
                    En = new TextualDescription
                    {
                        Description =
                            "The iconic sail-shaped silhoutte of Burj Al Arab Jumeirah stands tall as a beacon of modern Dubai, characterized by the finest hospitality you can ever experience."
                    },
                    Ar = new TextualDescription
                    {
                        Description =
                            "يقف فندق برج العرب جميرا الشهير شامخًا على شكل شراع وكأنه منارة دبي الحديثة، ويتسم بأجود وأرقى الضيافات التي يمكن أن تمر بها على الإطلاق."
                    },
                    Ru = new TextualDescription
                    {
                        Description =
                            "Легендарный отель Burj Al Arab Jumeirah известен своим непревзойденным уровнем обслуживания и гостеприимства, а его высокий силуэт в форме паруса служит маяком современного Дубая."
                    }
                });

            accommodation.Address = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string>
            {
                Ar = "شارع الملك سلمان بن عبدالعزيز آل سعود - دبي",
                En = "King Salman Bin Abdulaziz Al Saud St - Dubai",
                Ru = "King Salman Bin Abdulaziz Al Saud St - Dubai - ОАЭ"
            });

            accommodation.AdditionalInfo = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string>
            {
                En = "Additional Info 1 en",
                Ar = "Additional Info 2 ar",
                Ru = "Additional Info 2 ru"
            });
            
            accommodation.AccommodationAmenities = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<List<string>>
            {
                En = new List<string>
                {
                    @"201 luxurious duplex suites",
                    @"Nine world-class restaurants and bars",
                    @"Five swimming pools (three outdoor, two indoor) and a private beach"
                },
                Ar = new List<string>
                {
                    @"201 جناح دوبلكس فخم",
                    @"تسعة مقاهي ومطاعم عالمية",
                    "خمسة مسابح (ثلاثة خارجيون، اثنان داخليان) وشاطئ خاص",
                },
                Ru = new List<string>
                {
                    @"201 роскошный двухэтажный номер люкс",
                    @"Девять ресторанов и баров мирового класса",
                    @"Пять плавательных бассейнов (три открытых и два крытых) и частный пляж"
                }
            });

            accommodation.Pictures = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<List<Picture>>
            {
                En = new List<Picture>
                {
                    new Picture
                    {
                        Caption = "Burj Al Arab Jumeirah",
                        Source =
                            "https://mediastream.jumeirah.com/webimage/image1152x648//globalassets/global/hotels-and-resorts/dubai/burj-al-arab/homepage-audit/burj-al-arab-jumeirah-terrace-hero.jpg"
                    }
                },
                Ru = new List<Picture>
                {
                    new Picture
                    {
                        Caption = "برج العرب جميرا",
                        Source =
                            "https://mediastream.jumeirah.com/webimage/image1152x648//globalassets/global/hotels-and-resorts/dubai/burj-al-arab/homepage-audit/burj-al-arab-jumeirah-terrace-hero.jpg"
                    }
                }
            });

            dbContext.Accommodations.Add(accommodation);

            #endregion

            dbContext.SaveChanges();

            #region AddRooms

            var room = new Room
            {
                Id = 71,
                AccommodationId = hotelId,
                OccupancyConfigurations = new List<OccupancyConfiguration>
                {
                    new OccupancyConfiguration {Adults = 1},
                    new OccupancyConfiguration {Adults = 2},
                    new OccupancyConfiguration {Adults = 1, Children = 1},
                    new OccupancyConfiguration {Adults = 2, Children = 1},
                    new OccupancyConfiguration {Adults = 3},
                    new OccupancyConfiguration {Adults = 1, Children = 2},
                    new OccupancyConfiguration {Adults = 3, Children = 1},
                    new OccupancyConfiguration {Adults = 2, Children = 2}
                }
            };
            room.Name = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string>
            {
                En = "One Bedroom Deluxe Suite", Ru = "", Ar = ""
            });
            room.Description =
                JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string> {En = "", Ru = "", Ar = ""});
            room.Amenities = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<List<string>>
            {
                En = new List<string>(), Ar = new List<string>(), Ru = new List<string>(),
            });
            room.Pictures = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<List<Picture>>());
            dbContext.Rooms.Add(room);

            room = new Room
            {
                Id = 72,
                AccommodationId = hotelId,
                OccupancyConfigurations = new List<OccupancyConfiguration>
                {
                    new OccupancyConfiguration {Adults = 1},
                    new OccupancyConfiguration {Adults = 2},
                    new OccupancyConfiguration {Adults = 1, Children = 1},
                    new OccupancyConfiguration {Adults = 2, Children = 1},
                    new OccupancyConfiguration {Adults = 3},
                    new OccupancyConfiguration {Adults = 1, Children = 2},
                    new OccupancyConfiguration {Adults = 3, Children = 1},
                    new OccupancyConfiguration {Adults = 2, Children = 2}
                }
            };
            room.Name = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string>
            {
                En = "Panoramic One Bedroom Suite", Ru = "", Ar = ""
            });
            room.Description =
                JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string> {En = "", Ru = "", Ar = ""});
            room.Amenities = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<List<string>>
            {
                En = new List<string>(), Ar = new List<string>(), Ru = new List<string>(),
            });
            room.Pictures = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<List<Picture>>());
            dbContext.Rooms.Add(room);

            room = new Room
            {
                Id = 73,
                AccommodationId = hotelId,
                OccupancyConfigurations = new List<OccupancyConfiguration>
                {
                    new OccupancyConfiguration {Adults = 1},
                    new OccupancyConfiguration {Adults = 2},
                    new OccupancyConfiguration {Adults = 1, Children = 1},
                    new OccupancyConfiguration {Adults = 2, Children = 1},
                    new OccupancyConfiguration {Adults = 3},
                    new OccupancyConfiguration {Adults = 3, Children = 1},
                    new OccupancyConfiguration {Adults = 2, Children = 2},
                    new OccupancyConfiguration {Adults = 1, Children = 3},
                    new OccupancyConfiguration {Adults = 5},
                    new OccupancyConfiguration {Adults = 4, Children = 1},
                    new OccupancyConfiguration {Adults = 3, Children = 2},
                    new OccupancyConfiguration {Adults = 2, Children = 3},
                    new OccupancyConfiguration {Adults = 1, Children = 4},
                    new OccupancyConfiguration {Adults = 5, Children = 1},
                    new OccupancyConfiguration {Adults = 4, Children = 2},
                    new OccupancyConfiguration {Adults = 3, Children = 3},
                    new OccupancyConfiguration {Adults = 2, Children = 4},
                    new OccupancyConfiguration {Adults = 1, Children = 5}
                }
            };
            room.Name = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string>
            {
                En = "Two Bedroom Delux Suite", Ru = "", Ar = ""
            });
            room.Description =
                JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string> {En = "", Ru = "", Ar = ""});
            room.Amenities = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<List<string>>
            {
                En = new List<string>(), Ar = new List<string>(), Ru = new List<string>(),
            });
            room.Pictures = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<List<Picture>>());
            dbContext.Rooms.Add(room);

            room = new Room
            {
                Id = 74,
                AccommodationId = hotelId,
                OccupancyConfigurations = new List<OccupancyConfiguration>
                {
                    new OccupancyConfiguration {Adults = 1},
                    new OccupancyConfiguration {Adults = 2},
                    new OccupancyConfiguration {Adults = 1},
                    new OccupancyConfiguration {Adults = 1, Children = 1},
                    new OccupancyConfiguration {Adults = 2, Children = 1},
                    new OccupancyConfiguration {Adults = 3},
                    new OccupancyConfiguration {Adults = 1, Children = 2},
                    new OccupancyConfiguration {Adults = 4, Children = 1},
                    new OccupancyConfiguration {Adults = 3, Children = 1},
                    new OccupancyConfiguration {Adults = 2, Children = 2},
                    new OccupancyConfiguration {Adults = 1, Children = 3},
                    new OccupancyConfiguration {Adults = 5},
                    new OccupancyConfiguration {Adults = 3, Children = 2},
                    new OccupancyConfiguration {Adults = 2, Children = 3},
                    new OccupancyConfiguration {Adults = 1, Children = 4},
                    new OccupancyConfiguration {Adults = 6},
                    new OccupancyConfiguration {Adults = 5, Children = 1},
                    new OccupancyConfiguration {Adults = 4, Children = 2},
                    new OccupancyConfiguration {Adults = 3, Children = 3},
                    new OccupancyConfiguration {Adults = 2, Children = 4},
                    new OccupancyConfiguration {Adults = 1, Children = 5},
                    new OccupancyConfiguration {Adults = 7},
                    new OccupancyConfiguration {Adults = 6, Children = 1},
                    new OccupancyConfiguration {Adults = 5, Children = 2},
                    new OccupancyConfiguration {Adults = 4, Children = 3},
                    new OccupancyConfiguration {Adults = 3, Children = 4},
                    new OccupancyConfiguration {Adults = 2, Children = 5}
                }
            };
            room.Name = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string>
            {
                En = "Diplomatic Three Bedroom Suite", Ru = "", Ar = ""
            });
            room.Description =
                JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string> {En = "", Ru = "", Ar = ""});
            room.Amenities = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<List<string>>
            {
                En = new List<string>(), Ar = new List<string>(), Ru = new List<string>(),
            });
            room.Pictures = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<List<Picture>>());
            dbContext.Rooms.Add(room);
            dbContext.SaveChanges();
            #endregion
            
            #region AddCanellationPolicies

            var roomIds = new[] {71, 72, 73, 74};

            var seasonIds = new[] {GetSeasonId(57)};
            var cancellationPolicy = new RoomCancellationPolicy
            {
                Policies = new List<CancellationPolicyItem>
                {
                    new CancellationPolicyItem
                    {
                        DayPriorToArrival = new DayInterval {FromDay = 0, ToDay = 7},
                        PenaltyType = CancellationPenaltyTypes.Percent,
                        PenaltyCharge = 100
                    }
                }
            };
            
            AddCancellationPolicies(dbContext, seasonIds, roomIds, cancellationPolicy);
            dbContext.SaveChanges();
            
            seasonIds = new[] { GetSeasonId(51), GetSeasonId(58), GetSeasonId(62)};
            cancellationPolicy = new RoomCancellationPolicy
            {
                Policies = new List<CancellationPolicyItem>
                {
                    new CancellationPolicyItem
                    {
                        DayPriorToArrival = new DayInterval {FromDay = 0, ToDay = 14},
                        PenaltyType = CancellationPenaltyTypes.Percent,
                        PenaltyCharge = 100
                    }
                }
            };
            
            AddCancellationPolicies(dbContext, seasonIds, roomIds, cancellationPolicy);
            dbContext.SaveChanges();
            
            seasonIds = new[] {GetSeasonId(52), GetSeasonId(54), GetSeasonId(56), GetSeasonId(59), GetSeasonId(61), GetSeasonId(63), GetSeasonId(65)};
            cancellationPolicy = new RoomCancellationPolicy
            {
                Policies = new List<CancellationPolicyItem>
                {
                    new CancellationPolicyItem
                    {
                        DayPriorToArrival = new DayInterval {FromDay = 0, ToDay = 21},
                        PenaltyType = CancellationPenaltyTypes.Percent,
                        PenaltyCharge = 100
                    }
                }
            };

            AddCancellationPolicies(dbContext, seasonIds, roomIds, cancellationPolicy);
            dbContext.SaveChanges();
            
            seasonIds = new[] {GetSeasonId(53), GetSeasonId(55), GetSeasonId(60)};
            cancellationPolicy = new RoomCancellationPolicy
            {
                Policies = new List<CancellationPolicyItem>
                {
                    new CancellationPolicyItem
                    {
                        DayPriorToArrival = new DayInterval {FromDay = 0, ToDay = 35},
                        PenaltyType = CancellationPenaltyTypes.Percent,
                        PenaltyCharge = 100
                    }
                }
            };
            AddCancellationPolicies(dbContext, seasonIds, roomIds, cancellationPolicy);
            dbContext.SaveChanges();
            
            seasonIds = new[] {GetSeasonId(64)};
            cancellationPolicy = new RoomCancellationPolicy
            {
                Policies = new List<CancellationPolicyItem>
                {
                    new CancellationPolicyItem
                    {
                        DayPriorToArrival = new DayInterval {FromDay = 0, ToDay = 35},
                        PenaltyType = CancellationPenaltyTypes.Percent,
                        PenaltyCharge = 100
                    }
                }
            };
            AddCancellationPolicies(dbContext, seasonIds, roomIds, cancellationPolicy);
            dbContext.SaveChanges();
            #endregion

            dbContext.SaveChanges();

            #region AddRates

            FillRates(dbContext, new[] {GetSeasonId(51)},
                new List<(int, decimal)> {(71, 7321), (72, 7809), (73, 10981), (74, 18302)});

            FillRates(dbContext, new[] {GetSeasonId(52)},
                new List<(int, decimal)> {(71, 9273), (72, 10005), (73, 13909), (74, 23182)});

            FillRates(dbContext, new[] {GetSeasonId(53)},
                new List<(int, decimal)> {(71, 11225), (72, 12201), (73, 22450), (74, 33675)});

            FillRates(dbContext, new[] {GetSeasonId(54)},
                new List<(int, decimal)> {(71, 9273), (72, 10005), (73, 13909), (74, 23182)});

            FillRates(dbContext, new[] {GetSeasonId(55)},
                new List<(int, decimal)> {(71, 11225), (72, 12201), (73, 22450), (74, 33675)});

            FillRates(dbContext, new[] {GetSeasonId(56)},
                new List<(int, decimal)> {(71, 9273), (72, 10005), (73, 13909), (74, 23182)});

            FillRates(dbContext, new[] {GetSeasonId(57)},
                new List<(int, decimal)> {(71, 6345), (72, 6833), (73, 9517), (74, 15862)});

            FillRates(dbContext, new[] {GetSeasonId(58)},
                new List<(int, decimal)> {(71, 7321), (72, 7809), (73, 10981), (74, 18302)});

            FillRates(dbContext, new[] {GetSeasonId(59)},
                new List<(int, decimal)> {(71, 9273), (72, 10005), (73, 13909), (74, 23182)});

            FillRates(dbContext, new[] {GetSeasonId(60)},
                new List<(int, decimal)> {(71, 11225), (72, 12201), (73, 22450), (74, 33675)});

            FillRates(dbContext, new[] {GetSeasonId(61)},
                new List<(int, decimal)> {(71, 9273), (72, 10005), (73, 13909), (74, 23182)});

            FillRates(dbContext, new[] {GetSeasonId(62)},
                new List<(int, decimal)> {(71, 7321), (72, 7809), (73, 10981), (74, 18302)});

            FillRates(dbContext, new[] {GetSeasonId(63)},
                new List<(int, decimal)> {(71, 9273), (72, 10005), (73, 13909), (74, 23182)});

            FillRates(dbContext, new[] {GetSeasonId(64)},
                new List<(int, decimal)> {(71, 13177), (72, 14153), (73, 26355), (74, 39532)});

            FillRates(dbContext, new[] {GetSeasonId(65)},
                new List<(int, decimal)> {(71, 9273), (72, 10005), (73, 13909), (74, 23182)});

            #endregion

            dbContext.SaveChanges();

            #region AddPromotionalOffers

            var promotionalOffers = new List<RoomPromotionalOffer>
            {
                new RoomPromotionalOffer
                {
                    DiscountPercent = 20,
                    BookByDate = new DateTime(2018, 11, 29),
                    ValidFromDate = new DateTime(2019, 01, 08),
                    ValidToDate = new DateTime(2019, 01, 31),
                    BookingCode = "WWHL600",
                    ContractId = contractId,
                    Details = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string>())
                }
            };
            AddPromotionalOffers(dbContext, new[] {71, 72, 73, 74}, promotionalOffers);
            promotionalOffers = new List<RoomPromotionalOffer>
            {
                new RoomPromotionalOffer
                {
                    DiscountPercent = 25,
                    BookByDate = new DateTime(2018, 11, 29),
                    ValidFromDate = new DateTime(2019, 02, 01),
                    ValidToDate = new DateTime(2019, 02, 04),
                    BookingCode = "WWHL601",
                    ContractId = contractId,
                    Details = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string>())
                }
            };
            AddPromotionalOffers(dbContext, new[] {71, 72, 73, 74}, promotionalOffers);

            promotionalOffers = new List<RoomPromotionalOffer>
            {
                new RoomPromotionalOffer
                {
                    DiscountPercent = 20,
                    BookByDate = new DateTime(2018, 11, 29),
                    ValidFromDate = new DateTime(2019, 02, 05),
                    ValidToDate = new DateTime(2019, 02, 11),
                    BookingCode = "WWHL602",
                    ContractId = contractId,
                    Details = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string>())
                }
            };
            AddPromotionalOffers(dbContext, new[] {71, 72, 73, 74}, promotionalOffers);

            promotionalOffers = new List<RoomPromotionalOffer>
            {
                new RoomPromotionalOffer
                {
                    DiscountPercent = 30,
                    BookByDate = new DateTime(2018, 11, 29),
                    ValidFromDate = new DateTime(2019, 02, 12),
                    ValidToDate = new DateTime(2019, 03, 27),
                    BookingCode = "WWHL603",
                    Details = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string>())
                }
            };
            AddPromotionalOffers(dbContext, new[] {71, 72, 73, 74}, promotionalOffers);

            promotionalOffers = new List<RoomPromotionalOffer>
            {
                new RoomPromotionalOffer
                {
                    DiscountPercent = 30,
                    BookByDate = new DateTime(2019, 01, 31),
                    ValidFromDate = new DateTime(2019, 03, 28),
                    ValidToDate = new DateTime(2019, 04, 21),
                    BookingCode = "WWHL604",
                    ContractId = contractId,
                    Details = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string>())
                }
            };
            AddPromotionalOffers(dbContext, new[] {71, 72, 73, 74}, promotionalOffers);

            promotionalOffers = new List<RoomPromotionalOffer>
            {
                new RoomPromotionalOffer
                {
                    DiscountPercent = 30,
                    BookByDate = new DateTime(2019, 01, 31),
                    ValidFromDate = new DateTime(2019, 03, 22),
                    ValidToDate = new DateTime(2019, 05, 05),
                    BookingCode = "WWHL605",
                    ContractId = contractId,
                    Details = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string>())
                }
            };
            AddPromotionalOffers(dbContext, new[] {71, 72, 73, 74}, promotionalOffers);

            promotionalOffers = new List<RoomPromotionalOffer>
            {
                new RoomPromotionalOffer
                {
                    DiscountPercent = 30,
                    BookByDate = new DateTime(2019, 02, 28),
                    ValidFromDate = new DateTime(2019, 05, 06),
                    ValidToDate = new DateTime(2019, 08, 31),
                    BookingCode = "WWHL606",
                    ContractId = contractId,
                    Details = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string>())
                }
            };
            AddPromotionalOffers(dbContext, new[] {71, 72, 73, 74}, promotionalOffers);

            promotionalOffers = new List<RoomPromotionalOffer>
            {
                new RoomPromotionalOffer
                {
                    DiscountPercent = 25,
                    BookByDate = new DateTime(2019, 02, 28),
                    ValidFromDate = new DateTime(2019, 09, 01),
                    ValidToDate = new DateTime(2019, 10, 12),
                    BookingCode = "WWHL607",
                    ContractId = contractId,
                    Details = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string>())
                }
            };
            AddPromotionalOffers(dbContext, new[] {71, 72, 73, 74}, promotionalOffers);

            promotionalOffers = new List<RoomPromotionalOffer>
            {
                new RoomPromotionalOffer
                {
                    DiscountPercent = 25,
                    BookByDate = new DateTime(2019, 05, 30),
                    ValidFromDate = new DateTime(2019, 10, 13),
                    ValidToDate = new DateTime(2019, 10, 19),
                    BookingCode = "WWHL608",
                    ContractId = contractId,
                    Details = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string>())
                }
            };
            AddPromotionalOffers(dbContext, new[] {71, 72, 73, 74}, promotionalOffers);

            promotionalOffers = new List<RoomPromotionalOffer>
            {
                new RoomPromotionalOffer
                {
                    DiscountPercent = 30,
                    BookByDate = new DateTime(2019, 07, 31),
                    ValidFromDate = new DateTime(2019, 10, 20),
                    ValidToDate = new DateTime(2019, 11, 09),
                    BookingCode = "WWHL609",
                    ContractId = contractId,
                    Details = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string>())
                }
            };
            AddPromotionalOffers(dbContext, new[] {71, 72, 73, 74}, promotionalOffers);

            promotionalOffers = new List<RoomPromotionalOffer>
            {
                new RoomPromotionalOffer
                {
                    DiscountPercent = 25,
                    BookByDate = new DateTime(2019, 07, 31),
                    ValidFromDate = new DateTime(2019, 11, 10),
                    ValidToDate = new DateTime(2019, 12, 01),
                    BookingCode = "WWHL610",
                    ContractId = contractId,
                    Details = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string>())
                }
            };
            AddPromotionalOffers(dbContext, new[] {71, 72, 73, 74}, promotionalOffers);

            promotionalOffers = new List<RoomPromotionalOffer>
            {
                new RoomPromotionalOffer
                {
                    DiscountPercent = 25,
                    BookByDate = new DateTime(2019, 09, 30),
                    ValidFromDate = new DateTime(2019, 12, 02),
                    ValidToDate = new DateTime(2019, 12, 21),
                    BookingCode = "WWHL611",
                    ContractId = contractId,
                    Details = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string>())
                }
            };
            AddPromotionalOffers(dbContext, new[] {71, 72, 73, 74}, promotionalOffers);

            promotionalOffers = new List<RoomPromotionalOffer>
            {
                new RoomPromotionalOffer
                {
                    DiscountPercent = 15,
                    BookByDate = new DateTime(2019, 09, 30),
                    ValidFromDate = new DateTime(2019, 12, 22),
                    ValidToDate = new DateTime(2019, 12, 26),
                    BookingCode = "WWHL612",
                    ContractId = contractId,
                    Details = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string>())
                }
            };
            AddPromotionalOffers(dbContext, new[] {71, 72, 73, 74}, promotionalOffers);

            promotionalOffers = new List<RoomPromotionalOffer>
            {
                new RoomPromotionalOffer
                {
                    DiscountPercent = 20,
                    BookByDate = new DateTime(2019, 09, 30),
                    ValidFromDate = new DateTime(2019, 12, 27),
                    ValidToDate = new DateTime(2020, 01, 04),
                    BookingCode = "WWHL613",
                    ContractId = contractId,
                    Details = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string>())
                }
            };
            AddPromotionalOffers(dbContext, new[] {71, 72, 73, 74}, promotionalOffers);

            promotionalOffers = new List<RoomPromotionalOffer>
            {
                new RoomPromotionalOffer
                {
                    DiscountPercent = 20,
                    BookByDate = new DateTime(2019, 09, 30),
                    ValidFromDate = new DateTime(2020, 01, 05),
                    ValidToDate = new DateTime(2020, 01, 13),
                    BookingCode = "WWHL614",
                    ContractId = contractId,
                    Details = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string>())
                }
            };
            AddPromotionalOffers(dbContext, new[] {71, 72, 73, 74}, promotionalOffers);

            #endregion

            dbContext.SaveChanges();

            #region AddAllocationRequirements

            roomIds = new[] {71, 72, 73, 74};
            var roomAllocationRequirements = new[]
            {
                new RoomAllocationRequirement
                {
                    ReleasePeriod = new ReleasePeriod {Days = 14},
                    StartDate = new DateTime(2019, 01, 14),
                    EndDate = new DateTime(2019, 01, 31)
                }
            };
            AddRoomAllocationRequirements(dbContext, roomIds, roomAllocationRequirements);

            roomIds = new[] {71, 72, 73, 74};
            roomAllocationRequirements = new[]
            {
                new RoomAllocationRequirement
                {
                    ReleasePeriod = new ReleasePeriod {Days = 21},
                    StartDate = new DateTime(2019, 02, 01),
                    EndDate = new DateTime(2019, 02, 04)
                }
            };
            AddRoomAllocationRequirements(dbContext, roomIds, roomAllocationRequirements);

            roomIds = new[] {71, 72, 73, 74};
            roomAllocationRequirements = new[]
            {
                new RoomAllocationRequirement
                {
                    ReleasePeriod = new ReleasePeriod {Days = 35},
                    StartDate = new DateTime(2019, 02, 05),
                    EndDate = new DateTime(2019, 02, 11)
                }
            };
            AddRoomAllocationRequirements(dbContext, roomIds, roomAllocationRequirements);

            roomIds = new[] {71, 72, 73, 74};
            roomAllocationRequirements = new[]
            {
                new RoomAllocationRequirement
                {
                    ReleasePeriod = new ReleasePeriod {Days = 21},
                    StartDate = new DateTime(2019, 02, 12),
                    EndDate = new DateTime(2019, 03, 27)
                }
            };
            AddRoomAllocationRequirements(dbContext, roomIds, roomAllocationRequirements);

            roomIds = new[] {71, 72, 73, 74};
            roomAllocationRequirements = new[]
            {
                new RoomAllocationRequirement
                {
                    ReleasePeriod = new ReleasePeriod {Days = 35},
                    StartDate = new DateTime(2019, 03, 28),
                    EndDate = new DateTime(2019, 04, 21)
                }
            };
            AddRoomAllocationRequirements(dbContext, roomIds, roomAllocationRequirements);

            roomIds = new[] {71, 72, 73, 74};
            roomAllocationRequirements = new[]
            {
                new RoomAllocationRequirement
                {
                    ReleasePeriod = new ReleasePeriod {Days = 21},
                    StartDate = new DateTime(2019, 04, 22),
                    EndDate = new DateTime(2019, 05, 05)
                }
            };
            AddRoomAllocationRequirements(dbContext, roomIds, roomAllocationRequirements);

            roomIds = new[] {71, 72, 73, 74};
            roomAllocationRequirements = new[]
            {
                new RoomAllocationRequirement
                {
                    ReleasePeriod = new ReleasePeriod {Days = 7},
                    StartDate = new DateTime(2019, 05, 06),
                    EndDate = new DateTime(2019, 08, 31)
                }
            };
            AddRoomAllocationRequirements(dbContext, roomIds, roomAllocationRequirements);

            roomIds = new[] {71, 72, 73, 74};
            roomAllocationRequirements = new[]
            {
                new RoomAllocationRequirement
                {
                    ReleasePeriod = new ReleasePeriod {Days = 14},
                    StartDate = new DateTime(2019, 09, 01),
                    EndDate = new DateTime(2019, 10, 12)
                }
            };
            AddRoomAllocationRequirements(dbContext, roomIds, roomAllocationRequirements);

            roomIds = new[] {71, 72, 73, 74};
            roomAllocationRequirements = new[]
            {
                new RoomAllocationRequirement
                {
                    ReleasePeriod = new ReleasePeriod {Days = 21},
                    StartDate = new DateTime(2019, 10, 13),
                    EndDate = new DateTime(2019, 10, 19)
                }
            };
            AddRoomAllocationRequirements(dbContext, roomIds, roomAllocationRequirements);

            roomIds = new[] {71, 72, 73, 74};
            roomAllocationRequirements = new[]
            {
                new RoomAllocationRequirement
                {
                    ReleasePeriod = new ReleasePeriod {Days = 35},
                    StartDate = new DateTime(2019, 10, 20),
                    EndDate = new DateTime(2019, 11, 09)
                }
            };
            AddRoomAllocationRequirements(dbContext, roomIds, roomAllocationRequirements);

            roomIds = new[] {71, 72, 73, 74};
            roomAllocationRequirements = new[]
            {
                new RoomAllocationRequirement
                {
                    ReleasePeriod = new ReleasePeriod {Days = 21},
                    StartDate = new DateTime(2019, 11, 10),
                    EndDate = new DateTime(2019, 12, 01)
                }
            };
            AddRoomAllocationRequirements(dbContext, roomIds, roomAllocationRequirements);

            roomIds = new[] {71, 72, 73, 74};
            roomAllocationRequirements = new[]
            {
                new RoomAllocationRequirement
                {
                    ReleasePeriod = new ReleasePeriod {Days = 14},
                    StartDate = new DateTime(2019, 12, 02),
                    EndDate = new DateTime(2019, 12, 21)
                }
            };
            AddRoomAllocationRequirements(dbContext, roomIds, roomAllocationRequirements);

            roomIds = new[] {71, 72, 73, 74};
            roomAllocationRequirements = new[]
            {
                new RoomAllocationRequirement
                {
                    ReleasePeriod = new ReleasePeriod {Days = 21},
                    StartDate = new DateTime(2019, 12, 22),
                    EndDate = new DateTime(2019, 12, 26)
                }
            };
            AddRoomAllocationRequirements(dbContext, roomIds, roomAllocationRequirements);

            roomIds = new[] {71, 72, 73, 74};
            roomAllocationRequirements = new[]
            {
                new RoomAllocationRequirement
                {
                    ReleasePeriod = new ReleasePeriod {Days = 35},
                    StartDate = new DateTime(2019, 12, 27),
                    EndDate = new DateTime(2020, 01, 04)
                }
            };
            AddRoomAllocationRequirements(dbContext, roomIds, roomAllocationRequirements);

            roomIds = new[] {71, 72, 73, 74};
            roomAllocationRequirements = new[]
            {
                new RoomAllocationRequirement
                {
                    ReleasePeriod = new ReleasePeriod {Days = 21},
                    StartDate = new DateTime(2020, 01, 05),
                    EndDate = new DateTime(2020, 01, 13)
                }
            };
            AddRoomAllocationRequirements(dbContext, roomIds, roomAllocationRequirements);

            #endregion

            dbContext.SaveChanges();
        }

        private static void FillRates(DirectContractsDbContext dbContext, int[] seasonIds,
            List<(int, decimal)> roomIdsAndPrices)
        {
            foreach (var seasonId in seasonIds)
            {
                foreach (var roomIdsAndPrice in roomIdsAndPrices)
                {
                    var rate = new RoomRate
                    {
                        SeasonId = seasonId,
                        RoomId = roomIdsAndPrice.Item1,
                        Price = roomIdsAndPrice.Item2,
                        Currency = Currencies.AED,
                        Details = JsonDocumentUtilities.CreateJDocument(new MultiLanguage<string>()),
                        BoardBasis = BoardBasisTypes.NotSpecified,
                        MealPlan = "no meal"
                    };
                    dbContext.Entry(rate).State = EntityState.Detached;
                    dbContext.RoomRates.Add(rate);
                }
            }
        }

        private static void AddPromotionalOffers(DirectContractsDbContext dbContext, int[] roomIds,
            List<RoomPromotionalOffer> promotionalOffers)
        {
            foreach (var roomId in roomIds)
            {
                var promotionalOffersWithRoomIds = new List<RoomPromotionalOffer>();
                foreach (var promotionalOffer in promotionalOffers)
                {
                    promotionalOffersWithRoomIds.Add(new RoomPromotionalOffer
                    {
                        Details = promotionalOffer.Details,
                        BookingCode = promotionalOffer.BookingCode,
                        DiscountPercent = promotionalOffer.DiscountPercent,
                        RoomId = roomId,
                        BookByDate = promotionalOffer.BookByDate,
                        ValidFromDate = promotionalOffer.ValidFromDate,
                        ValidToDate = promotionalOffer.ValidToDate
                    });
                }

                dbContext.RoomPromotionalOffers.AddRange(promotionalOffersWithRoomIds);
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
                }

                var serialized = JsonConvert.SerializeObject(roomAllocationRequirements,
                    new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore});
                dbContext.RoomAllocationRequirements.AddRange(
                    JsonConvert.DeserializeObject<IEnumerable<RoomAllocationRequirement>>(serialized));
            }
        }

        private static void AddCancellationPolicies(DirectContractsDbContext dbContext, IEnumerable<int> seasonIds,
            IEnumerable<int> roomIds, RoomCancellationPolicy roomCancellationPolicy)
        {
            foreach (var seasonId in seasonIds)
            {
                foreach (var roomId in roomIds)
                {
                    roomCancellationPolicy.RoomId = roomId;
                    roomCancellationPolicy.SeasonId = seasonId;
                    var serialized = JsonConvert.SerializeObject(roomCancellationPolicy);
                    dbContext.RoomCancellationPolicies.Add(
                        JsonConvert.DeserializeObject<RoomCancellationPolicy>(serialized));
                }
            }
        }

        private static void AddSeasons(DirectContractsDbContext dbContext, int contractId, string seasonName,
            List<(int id, DateTime start, DateTime end)> seasonDates)
        {
            foreach (var (id, startDate, endDate) in seasonDates)
            {
                dbContext.Add(new Season
                {
                    Id = id,
                    ContractId = contractId,
                    StartDate = startDate,
                    EndDate = endDate,
                    Name = seasonName
                });
            }

            dbContext.SaveChanges();
        }


        private static int GetSeasonId(int id) => int.MaxValue - id;
    }
}
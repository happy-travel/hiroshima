using System;
using System.Collections.Generic;
using System.Linq;
using Hiroshima.DbData;
using Hiroshima.DbData.Models;
using Hiroshima.DbData.Models.Enums;
using NetTopologySuite.Geometries;
using NodaTime;
using Location = Hiroshima.DbData.Models.Location;
using Price = Hiroshima.DbData.Models.Price;

namespace Hiroshima.DirectContractsDataSeeder
{
    internal static class DataSeeder
    {
        internal static void AddData(DcDbContext dbContext)
        {
            AddOneAndOnlyContract(dbContext);
            AddJumeriahContract(dbContext);
        }

        private static void AddOneAndOnlyContract(DcDbContext dbContext)
        {
            var accommodation = dbContext.Accommodations.FirstOrDefault(a => a.Name.En.Equals("ONE&ONLY ROYAL MIRAGE"));
            if (accommodation == null)
            {
                var hotelId = 1;
                #region AddAccommodation
                dbContext.Accommodations.Add(
                
                    new Accommodation
                    {
                        Id = hotelId,
                        Rating = HotelRating.FiveStars,
                        PropertyType = PropertyType.Hotels,
                        Name = new MultiLanguage<string>
                        {
                            Ar = "ون آند اونلي رويال ميراج",
                            En = "ONE&ONLY ROYAL MIRAGE",
                            Ru = "ONE&ONLY ROYAL MIRAGE"
                        },
                        Contacts = new DbData.Models.Contacts
                        {
                            Email = "info@oneandonlythepalm.com",
                            Phone = "+ 971 4 440 1010"
                        },
                        Schedule = new Schedule
                        {
                            CheckInTime = new TimeSpan(13, 00,0,0),
                            CheckOutTime = new TimeSpan(11, 00,0),
                            PortersStartTime = new TimeSpan(11, 00,0),
                            PortersEndTime = new TimeSpan(16, 00,0)
                        },
                        Description = new MultiLanguage<string>
                        {
                            Ar =
                                "65 فدان من الحدائق الغناء وكيلومتر من الشواطئ الخاصة تمثل أنموذجاً للسلاموالرخاء منقطعا النظير.",
                            En =
                                "Set in 65 acres of lush gardens and a kilometer of private beach, peaceful lives in remarkable opulence.",
                            Ru =
                                "26 гектаров ландшафтных садов, собственный пляж протяженностью в один километр, умиротворенная обстановка и роскошное окружение.",
                        },
                        Amenities = new List<MultiLanguage<string>>
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
                        }
                });
                #endregion
                #region AddLocation
                dbContext.Locations.AddRange(new[]{new Location()
            {
                Coordinates = new Point(55.153219,25.097596),
                AccommodationId = hotelId,
                Address = new MultiLanguage<string>
                {
                    Ar = "شارع الملك سلمان بن عبدالعزيز آل سعود - دبي",
                    En = "King Salman Bin Abdulaziz Al Saud St - Dubai",
                    Ru = "King Salman Bin Abdulaziz Al Saud St - Dubai - ОАЭ"
                },
                LocalityId = 1

            }});
                #endregion
                #region AddSeasons
                dbContext.Seasons.AddRange(
                new Season
                {
                    AccommodationId = hotelId,
                    Name = "HIGH I",
                    StartDate = new DateTime(2020, 1, 8),
                    EndDate = new DateTime(2020, 1, 14)
                },
                new Season
                {
                    AccommodationId = hotelId,
                    Name = "HIGH I",
                    StartDate = new DateTime(2020, 1, 25),
                    EndDate = new DateTime(2020, 2, 7)
                },
                new Season
                {
                    AccommodationId = hotelId,
                    Name = "HIGH I",
                    StartDate = new DateTime(2020, 2, 22),
                    EndDate = new DateTime(2020, 3, 20)
                },
                new Season
                {
                    AccommodationId = hotelId,
                    Name = "HIGH I",
                    StartDate = new DateTime(2020, 4, 19),
                    EndDate = new DateTime(2020, 5, 3)
                },
                new Season
                {
                    AccommodationId = hotelId,
                    Name = "HIGH I",
                    StartDate = new DateTime(2020, 10, 10),
                    EndDate = new DateTime(2020, 10, 16)
                },
                new Season
                {
                    AccommodationId = hotelId,
                    Name = "HIGH I",
                    StartDate = new DateTime(2020, 11, 7),
                    EndDate = new DateTime(2020, 12, 4)
                },
                new Season
                {
                    AccommodationId = hotelId,
                    Name = "HIGH I",
                    StartDate = new DateTime(2020, 12, 19),
                    EndDate = new DateTime(2020, 12, 25)
                },
                new Season
                {
                    AccommodationId = hotelId,
                    Name = "HIGH II",
                    StartDate = new DateTime(2020, 1, 15),
                    EndDate = new DateTime(2020, 1, 24)
                },
                new Season
                {
                    AccommodationId = hotelId,
                    Name = "HIGH II",
                    StartDate = new DateTime(2020, 9, 26),
                    EndDate = new DateTime(2020, 10, 9)
                },
                new Season
                {
                    AccommodationId = hotelId,
                    Name = "HIGH II",
                    StartDate = new DateTime(2020, 12, 5),
                    EndDate = new DateTime(2020, 12, 18)
                },
                new Season
                {
                    AccommodationId = hotelId,
                    Name = "PEAK I",
                    StartDate = new DateTime(2020, 2, 8),
                    EndDate = new DateTime(2020, 2, 21)
                },
                new Season
                {
                    AccommodationId = hotelId,
                    Name = "PEAK I",
                    StartDate = new DateTime(2020, 3, 21),
                    EndDate = new DateTime(2020, 3, 27)
                },
                new Season
                {
                    AccommodationId = hotelId,
                    Name = "PEAK I",
                    StartDate = new DateTime(2020, 4, 13),
                    EndDate = new DateTime(2020, 4, 18)
                },
                new Season
                {
                    AccommodationId = hotelId,
                    Name = "PEAK I",
                    StartDate = new DateTime(2020, 10, 17),
                    EndDate = new DateTime(2020, 10, 23)
                },
                new Season
                {
                    AccommodationId = hotelId,
                    Name = "PEAK II",
                    StartDate = new DateTime(2020, 3, 28),
                    EndDate = new DateTime(2020, 4, 12)
                },
                new Season
                {
                    AccommodationId = hotelId,
                    Name = "PEAK II",
                    StartDate = new DateTime(2020, 10, 24),
                    EndDate = new DateTime(2020, 11, 6)
                },
                new Season
                {
                    AccommodationId = hotelId,
                    Name = "SHOULDER",
                    StartDate = new DateTime(2020, 5, 4),
                    EndDate = new DateTime(2020, 5, 31)
                },
                new Season
                {
                    AccommodationId = hotelId,
                    Name = "SHOULDER",
                    StartDate = new DateTime(2020, 9, 5),
                    EndDate = new DateTime(2020, 9, 25)
                },
                new Season
                {
                    AccommodationId = hotelId,
                    Name = "LOW",
                    StartDate = new DateTime(2020, 7, 1),
                    EndDate = new DateTime(2020, 7, 4)
                },
                new Season
                {
                    AccommodationId = hotelId,
                    Name = "FESTIVE",
                    StartDate = new DateTime(2020, 12, 26),
                    EndDate = new DateTime(2021, 1, 3)
                });
                #endregion
                #region AddRooms
                dbContext.Rooms.AddRange(new[]{new Room
                {
                    AccommodationId = hotelId,
                    Name = new MultiLanguage<string>
                    {
                        En = "Palace Superior Deluxe Room"
                    }
                },new Room
                {
                    AccommodationId = hotelId,
                    Name = new MultiLanguage<string>
                    {
                        En = "Palace Gold Club Room"
                    }
                },new Room
                {
                    AccommodationId = hotelId,
                    Name = new MultiLanguage<string>
                    {
                        En = "Palace One Bedroom Executive Suite"
                    }
                },new Room
                {
                    AccommodationId = hotelId,
                    Name = new MultiLanguage<string>
                    {
                        En = "Palace One Bedroom Golf Club Suite"
                    }
                },new Room
                {
                    AccommodationId = hotelId,
                    Name = new MultiLanguage<string>
                    {
                        En = "Palace One Bedroom Royal Suite"
                    }
                },new Room
                {
                    AccommodationId = hotelId,
                    Name = new MultiLanguage<string>
                    {
                        En = "Arabian Court Delux Room"
                    }
                },new Room
                {
                    AccommodationId = hotelId,
                    Name = new MultiLanguage<string>
                    {
                        En = "Arabian Court Two Deluxe Room Family Accommodation"
                    }
                },new Room
                {
                    AccommodationId = hotelId,
                    Name = new MultiLanguage<string>
                    {
                        En = "Arabian Court Prince Suite"
                    }
                },new Room
                {
                    AccommodationId = hotelId,
                    Name = new MultiLanguage<string>
                    {
                        En = "Residence Prestige Room"
                    }
                },new Room
                {
                    AccommodationId = hotelId,
                    Name = new MultiLanguage<string>
                    {
                        En = "Residence Junior Room"
                    }
                },new Room
                {
                    AccommodationId = hotelId,
                    Name = new MultiLanguage<string>
                    {
                        En = "Residence Executive Suite"
                    }
                },new Room
                {
                    AccommodationId = hotelId,
                    Name = new MultiLanguage<string>
                    {
                        En = "Residence Garden Beach Villa"
                    }
                } });
                #endregion
                dbContext.SaveChanges();
                var accommodationsIds = dbContext.Rooms.Where(a => a.AccommodationId == hotelId).Select(a => a.Id).ToList();
                var seasonsIds = dbContext.Seasons.Where(s => s.AccommodationId == hotelId).Select(s => s.Id).ToArray();
                #region AddAgreements
                AddAgreements(dbContext, accommodationsIds, seasonsIds);
                #endregion
                #region AddStopSaleDate
                AddStopSaleDates(dbContext, accommodationsIds);
                #endregion
                #region AddPermittedOccupancy
                AddPermittedOccupancy(dbContext, accommodationsIds);
                #endregion
                dbContext.SaveChanges();
            }
        }
        private static void AddJumeriahContract(DcDbContext dbContext)
        {
            var accommodation = dbContext.Accommodations.FirstOrDefault(a => a.Name.En.Equals("Burj Al Arab Jumeirah"));
            if (accommodation == null)
            {
                var hotelId = 2;
                #region AddAccommodation

                dbContext.Accommodations.AddRange(new[]
                {
                    new Accommodation()
                    {
                        Id = hotelId,
                        Rating = HotelRating.FiveStars,
                        PropertyType = PropertyType.Hotels,
                        Name = new MultiLanguage<string>
                        {
                            Ar = "برج العرب جميرا",
                            En = "Burj Al Arab Jumeirah",
                            Ru = "Burj Al Arab Jumeirah"
                        },
                        Contacts = new DbData.Models.Contacts
                        {
                            Email = "info@jumeirah.com",
                            Phone = "+971 4 3665000"
                        },
                        Schedule = new Schedule
                        {
                            CheckInTime = new TimeSpan(14, 00,0,0),
                            CheckOutTime = new TimeSpan(12, 00,0,0),
                            PortersStartTime = new TimeSpan(10, 00,0,0),
                            PortersEndTime = new TimeSpan(15, 00,0,0)
                        },
                        Description = new MultiLanguage<string>
                        {
                            Ar =
                                "يقف فندق برج العرب جميرا الشهير شامخًا على شكل شراع وكأنه منارة دبي الحديثة، ويتسم بأجود وأرقى الضيافات التي يمكن أن تمر بها على الإطلاق.",
                            En =
                                "The iconic sail-shaped silhoutte of Burj Al Arab Jumeirah stands tall as a beacon of modern Dubai, characterized by the finest hospitality you can ever experience.",
                            Ru =
                                "Легендарный отель Burj Al Arab Jumeirah известен своим непревзойденным уровнем обслуживания и гостеприимства, а его высокий силуэт в форме паруса служит маяком современного Дубая.",
                        },
                        Amenities = new List<MultiLanguage<string>>
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
                    }
                });
                #endregion
                #region AddLocation
                dbContext.Locations.AddRange(new Location
                {
                    Coordinates = new Point(55.1850, 25.1385),
                    AccommodationId = hotelId,
                    Address = new MultiLanguage<string>
                    {
                        Ar = "طريق المدينة الأكاديمية ص.ب. 214159 دبي، الإمارات العربية المتحدة",
                        En = "Level 5, Building 5 Dubai Design District PO Box 73137 Dubai,UAE",
                        Ru = "Level 5, Building 5 Dubai Design District PO Box 73137 Dubai,UAE"
                    },
                    LocalityId = 1

                });
                #endregion
                #region AddSeasons
                dbContext.Seasons.AddRange(
                new Season
                {
                    AccommodationId = hotelId,
                    Name = "Shoulder",
                    StartDate = new DateTime(2019, 1, 14),
                    EndDate = new DateTime(2020, 1, 31)
                },
                new Season
                {
                    AccommodationId = hotelId,
                    Name = "High",
                    StartDate = new DateTime(2019, 2, 1),
                    EndDate = new DateTime(2019, 2, 4)
                },
                new Season
                {
                    AccommodationId = hotelId,
                    Name = "Peak",
                    StartDate = new DateTime(2019, 2, 5),
                    EndDate = new DateTime(2019, 2, 11)
                },
                new Season
                {
                    AccommodationId = hotelId,
                    Name = "High",
                    StartDate = new DateTime(2019, 2, 12),
                    EndDate = new DateTime(2019, 3, 27)
                },
                new Season
                {
                    AccommodationId = hotelId,
                    Name = "Peak",
                    StartDate = new DateTime(2019, 3, 28),
                    EndDate = new DateTime(2019, 4, 21)
                },
                new Season
                {
                    AccommodationId = hotelId,
                    Name = "High",
                    StartDate = new DateTime(2019, 4, 22),
                    EndDate = new DateTime(2019, 5, 5)
                },
                new Season
                {
                    AccommodationId = hotelId,
                    Name = "Low",
                    StartDate = new DateTime(2019, 5, 6),
                    EndDate = new DateTime(2019, 8, 31)
                },
                new Season
                {
                    AccommodationId = hotelId,
                    Name = "Shoulder",
                    StartDate = new DateTime(2019, 9, 1),
                    EndDate = new DateTime(2019, 10, 12)
                },
                new Season
                {
                    AccommodationId = hotelId,
                    Name = "High",
                    StartDate = new DateTime(2019, 10, 13),
                    EndDate = new DateTime(2019, 10, 19)
                },
                new Season
                {
                    AccommodationId = hotelId,
                    Name = "Peak",
                    StartDate = new DateTime(2019, 10, 20),
                    EndDate = new DateTime(2019, 11, 9)
                },
                new Season
                {
                    AccommodationId = hotelId,
                    Name = "High",
                    StartDate = new DateTime(2019, 11, 10),
                    EndDate = new DateTime(2019, 12, 1)
                },
                new Season
                {
                    AccommodationId = hotelId,
                    Name = "Shoulder",
                    StartDate = new DateTime(2019, 12, 2),
                    EndDate = new DateTime(2019, 12, 21)
                },
                new Season
                {
                    AccommodationId = hotelId,
                    Name = "High",
                    StartDate = new DateTime(2019, 12, 22),
                    EndDate = new DateTime(2019, 12, 26)
                },
                new Season
                {
                    AccommodationId = hotelId,
                    Name = "Festive",
                    StartDate = new DateTime(2019, 12, 27),
                    EndDate = new DateTime(2020, 1, 4)
                },
                new Season
                {
                    AccommodationId = hotelId,
                    Name = "High",
                    StartDate = new DateTime(2020, 1, 5),
                    EndDate = new DateTime(2020, 1, 13)
                });
                #endregion
                #region AddRooms
                dbContext.Rooms.AddRange(new[]{new Room
                {
                    AccommodationId = hotelId,
                    Name = new MultiLanguage<string>
                    {
                        En = "One Bedroom Deluxe Suite"
                    }
                },new Room
                {
                    AccommodationId = hotelId,
                    Name = new MultiLanguage<string>
                    {
                        En = "Panoramic One Bedroom Delux Suite"
                    }
                },new Room
                {
                        AccommodationId = hotelId,
                        Name = new MultiLanguage<string>
                        {
                            En = "Two Bedroom Delux Suite"
                        }
                    },new Room
                {
                    AccommodationId = hotelId,
                    Name = new MultiLanguage<string>
                    {
                        En = "Diplomatic Three Bedroom Suite"
                    }
                } });
                #endregion
                dbContext.SaveChanges();
                var accommodationsIds = dbContext.Rooms.Where(a => a.AccommodationId == hotelId).Select(a => a.Id).ToList();
                var seasonsIds = dbContext.Seasons.Where(s => s.AccommodationId == hotelId).Select(s => s.Id).ToArray();
                #region AddAgreements
                AddAgreements(dbContext, accommodationsIds, seasonsIds);
                #endregion
                #region AddStopSaleDate
                AddStopSaleDates(dbContext, accommodationsIds);
                #endregion
                dbContext.SaveChanges();
            }
        }
        
        private static void AddAgreements(DcDbContext dbContext, IEnumerable<int> accommodationsIds, IEnumerable<int> seasonsIds)
        {
            var random = new Random();
            var sglPriceMin = 5000;
            var sglPriceMax = 7000;
            
            foreach (var accommodationId in accommodationsIds)
            {
                foreach (var seasonId in seasonsIds)
                {
                    var sglPrice = random.Next(sglPriceMin, sglPriceMax);
                    var dblPrice = random.Next(sglPrice, sglPrice + 1000);
                    dbContext.Rates.Add(new Rate
                    {
                        RoomId = accommodationId,
                        Price = new Price
                        {
                            SglPrice = sglPrice,
                            DblPrice = dblPrice
                        },
                        SeasonId = seasonId
                    });
                }
            }
        }
        private static void AddStopSaleDates(DcDbContext dbContext, IEnumerable<int> accommodationsIds)
        {
            var random = new Random();
            foreach (var accommodationId in accommodationsIds)
            {
                var fromMonth = random.Next(1, 6);
                var fromDay = random.Next(1, 20);

                dbContext.StopSaleDates.Add(new StopSaleDate
                {
                    RoomId = accommodationId,
                    StartDate = new DateTime(2019, fromMonth, fromDay),
                    EndDate = new DateTime(2019, fromMonth + random.Next(1, 6), fromDay + random.Next(1, 8))
                });
            }
        }
        private static void AddPermittedOccupancy(DcDbContext dbContext, IEnumerable<int> accommodationsIds)
        {
            var random = new Random();
            foreach (var accommodationId in accommodationsIds)
            {
                dbContext.PermittedOccupancies.Add(new PermittedOccupancy()
                {
                    RoomId = accommodationId,
                    AdultsCount = random.Next(1, 4),
                    ChildrenCount = random.Next(1, 3)
                });
            }
        }

    }
}

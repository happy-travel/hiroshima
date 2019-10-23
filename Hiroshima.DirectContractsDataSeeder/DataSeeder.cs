using System;
using System.Collections.Generic;
using System.Linq;
using Hiroshima.Common.Models;
using Hiroshima.Common.Models.Accommodation;
using Hiroshima.Common.Models.Enums;
using Hiroshima.DbData;
using Hiroshima.DbData.Models;
using Hiroshima.DbData.Models.Accommodation;
using Hiroshima.DbData.Models.Rates;
using Hiroshima.DbData.Models.Rooms;
using NetTopologySuite.Geometries;
using Location = Hiroshima.DbData.Models.Location;
using PermittedOccupancy = Hiroshima.DbData.Models.Rooms.PermittedOccupancy;

namespace Hiroshima.DirectContractsDataSeeder
{
    internal static class DataSeeder
    {
        internal static void AddData(DirectContractsDbContext dbContext)
        {
            AddOneAndOnlyContract(dbContext);
            //AddJumeriahContract(dbContext);
            dbContext.SaveChanges();
        }

        private static void AddOneAndOnlyContract(DirectContractsDbContext dbContext)
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
                        Rating = AccommodationRating.FiveStars,
                        PropertyType = PropertyTypes.Hotels,
                        Name = new MultiLanguage<string>
                        {
                            Ar = "ون آند اونلي رويال ميراج",
                            En = "ONE&ONLY ROYAL MIRAGE",
                            Ru = "ONE&ONLY ROYAL MIRAGE"
                        },
                        Contacts = new Contacts
                        {
                            Email = "info@oneandonlythepalm.com",
                            Phone = "+ 971 4 440 1010"
                        },
                        Schedule = new Schedule
                        {
                            CheckInTime = "13:00",
                            CheckOutTime = "11:00",
                            PortersStartTime = "11:00",
                            PortersEndTime = "16:00"
                        },
                        TextualDescription = new TextualDescription
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
                        },
                        Picture = new Picture
                        {
                            Caption = new MultiLanguage<string>
                            {
                                Ar = "ون آند اونلي رويال ميراج",
                                En = "ONE&ONLY ROYAL MIRAGE"
                            },
                            Source = "https://encrypted-tbn0.gstatic.com/images?q=tbn%3AANd9GcQ6HkA4WClkmu3oOM0iuENG66UC6iKZNUrefe0iJ__MX5ZbValF"
                        }
                    });
                #endregion

                #region AddLocation
                dbContext.Locations.AddRange(new[]{new Location.Location()
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
                    Id = 11,
                    AccommodationId = hotelId,
                    Name = "HIGH I",
                    StartDate = new DateTime(2020, 1, 8),
                    EndDate = new DateTime(2020, 1, 14)
                },
                new Season
                {
                    Id = 12,
                    AccommodationId = hotelId,
                    Name = "HIGH II",
                    StartDate = new DateTime(2020, 1, 15),
                    EndDate = new DateTime(2020, 1, 24)
                },
                new Season
                {
                    Id = 13,
                    AccommodationId = hotelId,
                    Name = "HIGH I",
                    StartDate = new DateTime(2020, 1, 25),
                    EndDate = new DateTime(2020, 2, 7)
                },
                new Season
                {
                    Id = 14,
                    AccommodationId = hotelId,
                    Name = "PEAK I",
                    StartDate = new DateTime(2020, 2, 8),
                    EndDate = new DateTime(2020, 2, 21)
                },
                new Season
                {
                    Id = 15,
                    AccommodationId = hotelId,
                    Name = "HIGH I",
                    StartDate = new DateTime(2020, 2, 22),
                    EndDate = new DateTime(2020, 3, 20)
                },
                new Season
                {
                    Id = 16,
                    AccommodationId = hotelId,
                    Name = "PEAK I",
                    StartDate = new DateTime(2020, 3, 21),
                    EndDate = new DateTime(2020, 3, 27)
                },
                new Season
                {
                    Id = 17,
                    AccommodationId = hotelId,
                    Name = "PEAK II",
                    StartDate = new DateTime(2020, 3, 28),
                    EndDate = new DateTime(2020, 4, 12)
                },
                new Season
                {
                    Id = 18,
                    AccommodationId = hotelId,
                    Name = "PEAK I",
                    StartDate = new DateTime(2020, 4, 13),
                    EndDate = new DateTime(2020, 4, 18)
                },
                new Season
                {
                    Id = 19,
                    AccommodationId = hotelId,
                    Name = "HIGH I",
                    StartDate = new DateTime(2020, 4, 19),
                    EndDate = new DateTime(2020, 5, 3)
                },
                new Season
                {
                    Id = 20,
                    AccommodationId = hotelId,
                    Name = "SHOULDER",
                    StartDate = new DateTime(2020, 5, 4),
                    EndDate = new DateTime(2020, 5, 31)
                },
                new Season
                {
                    Id = 21,
                    AccommodationId = hotelId,
                    Name = "LOW",
                    StartDate = new DateTime(2020, 7, 1),
                    EndDate = new DateTime(2020, 9, 4)
                },
                new Season
                {
                    Id = 22,
                    AccommodationId = hotelId,
                    Name = "SHOULDER",
                    StartDate = new DateTime(2020, 9, 5),
                    EndDate = new DateTime(2020, 9, 25)
                },
                new Season
                {
                    Id = 23,
                    AccommodationId = hotelId,
                    Name = "HIGH II",
                    StartDate = new DateTime(2020, 9, 26),
                    EndDate = new DateTime(2020, 10, 9)
                },
                new Season
                {
                    Id = 24,
                    AccommodationId = hotelId,
                    Name = "HIGH I",
                    StartDate = new DateTime(2020, 10, 10),
                    EndDate = new DateTime(2020, 10, 16)
                },
                new Season
                {
                    Id = 25,
                    AccommodationId = hotelId,
                    Name = "PEAK I",
                    StartDate = new DateTime(2020, 10, 17),
                    EndDate = new DateTime(2020, 10, 23)
                },
                new Season
                {
                    Id = 26,
                    AccommodationId = hotelId,
                    Name = "PEAK II",
                    StartDate = new DateTime(2020, 10, 24),
                    EndDate = new DateTime(2020, 11, 6)
                },
                new Season
                {
                    Id = 27,
                    AccommodationId = hotelId,
                    Name = "HIGH I",
                    StartDate = new DateTime(2020, 11, 7),
                    EndDate = new DateTime(2020, 12, 4)
                },
                new Season
                {
                    Id = 28,
                    AccommodationId = hotelId,
                    Name = "HIGH II",
                    StartDate = new DateTime(2020, 12, 5),
                    EndDate = new DateTime(2020, 12, 18)
                },
                new Season
                {
                    Id = 29,
                    AccommodationId = hotelId,
                    Name = "HIGH I",
                    StartDate = new DateTime(2020, 12, 19),
                    EndDate = new DateTime(2020, 12, 25)
                },
                new Season
                {
                    Id = 30,
                    AccommodationId = hotelId,
                    Name = "FESTIVE",
                    StartDate = new DateTime(2020, 12, 26),
                    EndDate = new DateTime(2021, 1, 3)
                },
                new Season
                {
                    Id = 31,
                    AccommodationId = hotelId,
                    Name = "PEAK I",
                    StartDate = new DateTime(2021, 1, 4),
                    EndDate = new DateTime(2021, 1, 10)
                }, new Season
                {
                    Id = 32,
                    AccommodationId = hotelId,
                    Name = "HIGH I",
                    StartDate = new DateTime(2021, 1, 11),
                    EndDate = new DateTime(2021, 2, 5)
                }, new Season
                {
                    Id = 33,
                    AccommodationId = hotelId,
                    Name = "PEAK I",
                    StartDate = new DateTime(2021, 2, 6),
                    EndDate = new DateTime(2021, 2, 19)
                }, new Season
                {
                    Id = 34,
                    AccommodationId = hotelId,
                    Name = "HIGH I",
                    StartDate = new DateTime(2021, 2, 20),
                    EndDate = new DateTime(2021, 3, 19)
                }, new Season
                {
                    Id = 35,
                    AccommodationId = hotelId,
                    Name = "PEAK I",
                    StartDate = new DateTime(2021, 3, 20),
                    EndDate = new DateTime(2021, 3, 26)
                }, new Season
                {
                    Id = 36,
                    AccommodationId = hotelId,
                    Name = "PEAK II",
                    StartDate = new DateTime(2021, 3, 27),
                    EndDate = new DateTime(2021, 4, 10)
                },
                new Season
                {
                    Id = 37,
                    AccommodationId = hotelId,
                    Name = "HIGH I",
                    StartDate = new DateTime(2021, 4, 11),
                    EndDate = new DateTime(2021, 5, 7)
                }
                );
                #endregion

                #region AddRooms
                dbContext.Rooms.AddRange(new[]{new Room
                {
                    Id = 20,
                    AccommodationId = hotelId,
                    Name = new MultiLanguage<string>
                    {
                        En = "Palace Superior Deluxe Room"
                    }
                },new Room
                {
                    Id = 21,
                    AccommodationId = hotelId,
                    Name = new MultiLanguage<string>
                    {
                        En = "Palace Gold Club Room"
                    }
                },new Room
                {
                    Id = 22,
                    AccommodationId = hotelId,
                    Name = new MultiLanguage<string>
                    {
                        En = "Palace One Bedroom Executive Suite"
                    }
                },new Room
                {
                    Id = 23,
                    AccommodationId = hotelId,
                    Name = new MultiLanguage<string>
                    {
                        En = "Palace One Bedroom Golf Club Suite"
                    }
                },new Room
                {
                    Id = 24,
                    AccommodationId = hotelId,
                    Name = new MultiLanguage<string>
                    {
                        En = "Palace Two Bedroom Executive Suite"
                    }
                },new Room
                {
                    Id = 25,
                    AccommodationId = hotelId,
                    Name = new MultiLanguage<string>
                    {
                        En = "Palace Two Bedroom Golf Club Suite"
                    }
                },new Room
                {
                    Id = 26,
                    AccommodationId = hotelId,
                    Name = new MultiLanguage<string>
                    {
                        En = "Palace Two Bedroom Royal Suite"
                    }
                },new Room
                {
                    Id = 27,
                    AccommodationId = hotelId,
                    Name = new MultiLanguage<string>
                    {
                        En = "Arabian Court Delux Room"
                    }
                },new Room
                {
                    Id = 28,
                    AccommodationId = hotelId,
                    Name = new MultiLanguage<string>
                    {
                        En = "Arabian Court Two Deluxe Rooms Family Accommodation"
                    }
                },new Room
                {
                    Id = 29,
                    AccommodationId = hotelId,
                    Name = new MultiLanguage<string>
                    {
                        En = "Arabian Court One Bedroom Executive Suite"
                    }
                },new Room
                {
                    Id = 30,
                    AccommodationId = hotelId,
                    Name = new MultiLanguage<string>
                    {
                        En = "Arabian Court Two Bedroom Executive Suite"
                    }
                },new Room
                {
                    Id = 31,
                    AccommodationId = hotelId,
                    Name = new MultiLanguage<string>
                    {
                        En = "Arabian Court Prince Suite"
                    }
                },new Room
                {
                    Id = 32,
                    AccommodationId = hotelId,
                    Name = new MultiLanguage<string>
                    {
                        En = "Residence Prestige Room"
                    }
                },new Room
                {
                    Id = 33,
                    AccommodationId = hotelId,
                    Name = new MultiLanguage<string>
                    {
                        En = "Residence Junior Room"
                    }
                },new Room
                {
                    Id = 34,
                    AccommodationId = hotelId,
                    Name = new MultiLanguage<string>
                    {
                        En = "Residence Executive Suite"
                    }
                },new Room
                {
                    Id = 35,
                    AccommodationId = hotelId,
                    Name = new MultiLanguage<string>
                    {
                        En = "Residence Garden Beach Villa"
                    }
                } });
                #endregion

                #region AddRates
                FillRates(
                    dbContext,
                    new[] { 11, 13, 15, 19 },
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
                    new[] { 12, 23, 28 },
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
                    new[] { 14, 16, 18, 25 },
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
                    new[] { 17, 26 },
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
                    new[] { 20, 22 },
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
                    new[] { 21 },
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
                    new[] { 24, 27, 29 },
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
                    new[] { 30 },
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
                    new[] { 31, 33, 35 },
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
                    new[] { 32, 34 },
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
                    new[] { 36 },
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
                    new[] { 37 },
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

                #region AddStopSaleDate
                dbContext.StopSaleDates.AddRange(new StopSaleDate
                {
                    RoomId = 20,
                    StartDate = new DateTime(2020, 4, 1),
                    EndDate = new DateTime(2020, 4, 10)
                }, new StopSaleDate
                {
                    RoomId = 20,
                    StartDate = new DateTime(2020, 7, 10),
                    EndDate = new DateTime(2020, 7, 13)
                }, new StopSaleDate
                {
                    RoomId = 20,
                    StartDate = new DateTime(2020, 10, 15),
                    EndDate = new DateTime(2020, 10, 18)
                }, new StopSaleDate
                {
                    RoomId = 20,
                    StartDate = new DateTime(2021, 1, 10),
                    EndDate = new DateTime(2021, 1, 12)
                }, new StopSaleDate
                {
                    RoomId = 21,
                    StartDate = new DateTime(2020, 2, 5),
                    EndDate = new DateTime(2020, 2, 7)
                }, new StopSaleDate
                {
                    RoomId = 21,
                    StartDate = new DateTime(2020, 10, 15),
                    EndDate = new DateTime(2020, 10, 18)
                }, new StopSaleDate
                {
                    RoomId = 21,
                    StartDate = new DateTime(2020, 12, 28),
                    EndDate = new DateTime(2021, 1, 2)
                }, new StopSaleDate
                {
                    RoomId = 22,
                    StartDate = new DateTime(2020, 2, 5),
                    EndDate = new DateTime(2020, 2, 7)
                }, new StopSaleDate
                {
                    RoomId = 22,
                    StartDate = new DateTime(2020, 10, 15),
                    EndDate = new DateTime(2020, 10, 18)
                }, new StopSaleDate
                {
                    RoomId = 22,
                    StartDate = new DateTime(2020, 12, 28),
                    EndDate = new DateTime(2020, 12, 29)
                }, new StopSaleDate
                {
                    RoomId = 23,
                    StartDate = new DateTime(2020, 2, 5),
                    EndDate = new DateTime(2020, 2, 10)
                }, new StopSaleDate
                {
                    RoomId = 23,
                    StartDate = new DateTime(2020, 11, 1),
                    EndDate = new DateTime(2020, 11, 3)
                }, new StopSaleDate
                {
                    RoomId = 23,
                    StartDate = new DateTime(2020, 8, 7),
                    EndDate = new DateTime(2020, 8, 10)
                }, new StopSaleDate
                {
                    RoomId = 24,
                    StartDate = new DateTime(2020, 3, 8),
                    EndDate = new DateTime(2020, 3, 10)
                }, new StopSaleDate
                {
                    RoomId = 24,
                    StartDate = new DateTime(2020, 7, 1),
                    EndDate = new DateTime(2020, 7, 3)
                }, new StopSaleDate
                {
                    RoomId = 24,
                    StartDate = new DateTime(2020, 9, 3),
                    EndDate = new DateTime(2020, 9, 5)
                }, new StopSaleDate
                {
                    RoomId = 25,
                    StartDate = new DateTime(2020, 1, 29),
                    EndDate = new DateTime(2020, 2, 1)
                }, new StopSaleDate
                {
                    RoomId = 25,
                    StartDate = new DateTime(2020, 9, 15),
                    EndDate = new DateTime(2020, 9, 16)
                }, new StopSaleDate
                {
                    RoomId = 25,
                    StartDate = new DateTime(2020, 12, 30),
                    EndDate = new DateTime(2021, 1, 2)
                },
                new StopSaleDate
                {
                    RoomId = 26,
                    StartDate = new DateTime(2020, 1, 10),
                    EndDate = new DateTime(2020, 1, 12)
                }, new StopSaleDate
                {
                    RoomId = 26,
                    StartDate = new DateTime(2020, 3, 4),
                    EndDate = new DateTime(2020, 3, 7)
                }, new StopSaleDate
                {
                    RoomId = 26,
                    StartDate = new DateTime(2020, 6, 9),
                    EndDate = new DateTime(2020, 6, 11)
                }, new StopSaleDate
                {
                    RoomId = 27,
                    StartDate = new DateTime(2020, 1, 18),
                    EndDate = new DateTime(2020, 1, 19)
                }, new StopSaleDate
                {
                    RoomId = 27,
                    StartDate = new DateTime(2020, 2, 27),
                    EndDate = new DateTime(2020, 3, 1)
                }, new StopSaleDate
                {
                    RoomId = 27,
                    StartDate = new DateTime(2020, 12, 26),
                    EndDate = new DateTime(2020, 12, 27)
                }, new StopSaleDate
                {
                    RoomId = 28,
                    StartDate = new DateTime(2020, 4, 5),
                    EndDate = new DateTime(2020, 4, 7)
                }, new StopSaleDate
                {
                    RoomId = 28,
                    StartDate = new DateTime(2020, 11, 3),
                    EndDate = new DateTime(2020, 11, 5)
                }, new StopSaleDate
                {
                    RoomId = 28,
                    StartDate = new DateTime(2020, 12, 31),
                    EndDate = new DateTime(2021, 1, 1)
                }, new StopSaleDate
                {
                    RoomId = 29,
                    StartDate = new DateTime(2020, 2, 7),
                    EndDate = new DateTime(2020, 2, 10)
                }, new StopSaleDate
                {
                    RoomId = 29,
                    StartDate = new DateTime(2020, 4, 3),
                    EndDate = new DateTime(2020, 4, 5)
                }, new StopSaleDate
                {
                    RoomId = 29,
                    StartDate = new DateTime(2020, 12, 1),
                    EndDate = new DateTime(2021, 1, 1)
                }, new StopSaleDate
                {
                    RoomId = 30,
                    StartDate = new DateTime(2020, 6, 1),
                    EndDate = new DateTime(2020, 9, 1)
                }, new StopSaleDate
                {
                    RoomId = 30,
                    StartDate = new DateTime(2020, 9, 2),
                    EndDate = new DateTime(2020, 9, 4)
                }, new StopSaleDate
                {
                    RoomId = 30,
                    StartDate = new DateTime(2020, 12, 1),
                    EndDate = new DateTime(2021, 12, 2)
                }, new StopSaleDate
                {
                    RoomId = 31,
                    StartDate = new DateTime(2020, 1, 6),
                    EndDate = new DateTime(2020, 1, 8)
                }, new StopSaleDate
                {
                    RoomId = 31,
                    StartDate = new DateTime(2020, 2, 14),
                    EndDate = new DateTime(2020, 2, 16)
                }, new StopSaleDate
                {
                    RoomId = 31,
                    StartDate = new DateTime(2020, 7, 5),
                    EndDate = new DateTime(2021, 7, 8)
                }, new StopSaleDate
                {
                    RoomId = 32,
                    StartDate = new DateTime(2020, 9, 12),
                    EndDate = new DateTime(2020, 9, 15)
                }, new StopSaleDate
                {
                    RoomId = 32,
                    StartDate = new DateTime(2020, 10, 2),
                    EndDate = new DateTime(2020, 10, 4)
                }, new StopSaleDate
                {
                    RoomId = 32,
                    StartDate = new DateTime(2019, 12, 3),
                    EndDate = new DateTime(2019, 12, 4)
                }, new StopSaleDate
                {
                    RoomId = 33,
                    StartDate = new DateTime(2020, 3, 6),
                    EndDate = new DateTime(2020, 3, 8)
                }, new StopSaleDate
                {
                    RoomId = 33,
                    StartDate = new DateTime(2020, 4, 10),
                    EndDate = new DateTime(2020, 4, 13)
                }, new StopSaleDate
                {
                    RoomId = 33,
                    StartDate = new DateTime(2020, 7, 1),
                    EndDate = new DateTime(2020, 8, 1)
                },
                new StopSaleDate
                {
                    RoomId = 34,
                    StartDate = new DateTime(2020, 3, 3),
                    EndDate = new DateTime(2020, 3, 5)
                }, new StopSaleDate
                {
                    RoomId = 34,
                    StartDate = new DateTime(2020, 4, 16),
                    EndDate = new DateTime(2020, 4, 19)
                }, new StopSaleDate
                {
                    RoomId = 34,
                    StartDate = new DateTime(2020, 11, 23),
                    EndDate = new DateTime(2020, 11, 26)
                }, new StopSaleDate
                {
                    RoomId = 35,
                    StartDate = new DateTime(2020, 5, 1),
                    EndDate = new DateTime(2020, 5, 6)
                }, new StopSaleDate
                {
                    RoomId = 35,
                    StartDate = new DateTime(2020, 7, 19),
                    EndDate = new DateTime(2020, 7, 21)
                }, new StopSaleDate
                {
                    RoomId = 35,
                    StartDate = new DateTime(2020, 11, 28),
                    EndDate = new DateTime(2020, 12, 3)
                });
                #endregion

                #region AddPermittedOccupancy
                dbContext.PermittedOccupancies.AddRange(
                    new PermittedOccupancy
                    {
                        RoomId = 20,
                        AdultsNumber = 2,
                        ChildrenNumber = 1,
                    }, new PermittedOccupancy
                    {
                        RoomId = 21,
                        AdultsNumber = 2,
                        ChildrenNumber = 1
                    }, new PermittedOccupancy
                    {
                        RoomId = 22,
                        AdultsNumber = 2,
                        ChildrenNumber = 1
                    }, new PermittedOccupancy
                    {
                        RoomId = 23,
                        AdultsNumber = 2,
                        ChildrenNumber = 1
                    }, new PermittedOccupancy
                    {
                        RoomId = 24,
                        AdultsNumber = 4,
                        ChildrenNumber = 1
                    }, new PermittedOccupancy
                    {
                        RoomId = 25,
                        AdultsNumber = 4,
                        ChildrenNumber = 1
                    }, new PermittedOccupancy
                    {
                        RoomId = 26,
                        AdultsNumber = 4,
                        ChildrenNumber = 1
                    }, new PermittedOccupancy
                    {
                        RoomId = 27,
                        AdultsNumber = 2,
                        ChildrenNumber = 1
                    }, new PermittedOccupancy
                    {
                        RoomId = 28,
                        AdultsNumber = 3,
                        ChildrenNumber = 3
                    }, new PermittedOccupancy
                    {
                        RoomId = 29,
                        AdultsNumber = 3,
                        ChildrenNumber = 1,
                    }, new PermittedOccupancy
                    {
                        RoomId = 30,
                        AdultsNumber = 4,
                        ChildrenNumber = 1
                    }, new PermittedOccupancy
                    {
                        RoomId = 31,
                        AdultsNumber = 2,
                        ChildrenNumber = 1
                    }, new PermittedOccupancy
                    {
                        RoomId = 32,
                        AdultsNumber = 4,
                        ChildrenNumber = 1
                    }, new PermittedOccupancy
                    {
                        RoomId = 33,
                        AdultsNumber = 2,
                        ChildrenNumber = 1
                    }, new PermittedOccupancy
                    {
                        RoomId = 34,
                        AdultsNumber = 2,
                        ChildrenNumber = 1
                    }, new PermittedOccupancy
                    {
                        RoomId = 35,
                        AdultsNumber = 4,
                        ChildrenNumber = 1
                    }
                    );
                #endregion

            }
        }

        private static void AddJumeriahContract(DirectContractsDbContext dbContext)
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
                        Rating = AccommodationRating.FiveStars,
                        PropertyType = PropertyTypes.Hotels,
                        Name = new MultiLanguage<string>
                        {
                            Ar = "برج العرب جميرا",
                            En = "Burj Al Arab Jumeirah",
                            Ru = "Burj Al Arab Jumeirah"
                        },
                        Contacts = new Contacts
                        {
                            Email = "info@jumeirah.com",
                            Phone = "+971 4 3665000"
                        },
                        Schedule = new Schedule
                        {
                            CheckInTime = "14:00",
                            CheckOutTime = "12:00",
                            PortersStartTime = "10:00",
                            PortersEndTime = "15:00"
                        },
                        TextualDescription = new TextualDescription
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
                dbContext.Locations.AddRange(new Location.Location
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

                #endregion
                #region AddStopSaleDate

                #endregion
                dbContext.SaveChanges();
            }
        }

        private static void FillRates(DirectContractsDbContext dbContext, int[] seasonsIds, List<(int, decimal)> roomIdsAndPrices)
        {
            foreach (var seasonId in seasonsIds)
            {
                foreach (var roomIdsAndPrice in roomIdsAndPrices)
                {
                    dbContext.Rates.Add(new ContractRate
                    {
                        SeasonId = seasonId,
                        RoomId = roomIdsAndPrice.Item1,
                        SeasonPrice = roomIdsAndPrice.Item2,
                        CurrencyCode = "AED"
                    });
                }
            }
        }

    }
}

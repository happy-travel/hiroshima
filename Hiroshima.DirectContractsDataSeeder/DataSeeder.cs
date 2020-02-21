using System;
using System.Collections.Generic;
using System.Linq;
using HappyTravel.EdoContracts.Accommodations.Enums;
using Hiroshima.Common.Models;
using Hiroshima.Common.Models.Accommodation;
using Hiroshima.DbData;
using Hiroshima.DbData.Models.Accommodation;
using Hiroshima.DbData.Models.Booking;
using Hiroshima.DbData.Models.Location;
using Hiroshima.DbData.Models.Rates;
using Hiroshima.DbData.Models.Rooms;
using NetTopologySuite.Geometries;
using NpgsqlTypes;
using Location = Hiroshima.DbData.Models.Location.Location;

namespace Hiroshima.DirectContractsDataSeeder
{
    internal static class DataSeeder
    {
        internal static void AddData(DirectContractsDbContext dbContext)
        {
            AddOneAndOnlyContract(dbContext);
            AddOneAndOnlyTestContract(dbContext);
            AddJumeriahContract(dbContext);
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
                        Rating = AccommodationRatings.FiveStars,
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
                            CheckInTime = "15:00",
                            CheckOutTime = "10:00",
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
                                    "26 гектаров ландшафтных садов, собственный пляж протяженностью в один километр, умиротворенная обстановка и роскошное окружение."
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

                dbContext.Locations.AddRange(new Location
                {
                    Coordinates = new Point(55.153219, 25.097596),
                    AccommodationId = hotelId,
                    Address = new MultiLanguage<string>
                    {
                        Ar = "شارع الملك سلمان بن عبدالعزيز آل سعود - دبي",
                        En = "King Salman Bin Abdulaziz Al Saud St - Dubai",
                        Ru = "King Salman Bin Abdulaziz Al Saud St - Dubai - ОАЭ"
                    },
                    LocalityId = 1
                });

                #endregion

                #region AddCancelationPolicies

                dbContext.CancelationPolicies.Add(new CancelationPolicy
                {
                    Id = 21,
                    CancelationPolicyDetails = new List<CancelationPolicyDetails>
                    {
                        new CancelationPolicyDetails
                        {
                            FromDays = 45,
                            ToDays = 0,
                            Details = new MultiLanguage<string>
                                {En = "New Year Peak Period: Full stay of tour operator rates if cancelled within 45 days of arrival"}
                        }
                    }
                });

                dbContext.CancelationPolicies.Add(new CancelationPolicy
                {
                    Id = 22,
                    CancelationPolicyDetails = new List<CancelationPolicyDetails>
                    {
                        new CancelationPolicyDetails
                        {
                            FromDays = 35,
                            ToDays = 0,
                            Details = new MultiLanguage<string>
                                {En = "Full stay of tour operator rates if cancelled within 45 days of arrival"}
                        }
                    }
                });

                dbContext.CancelationPolicies.Add(new CancelationPolicy
                {
                    Id = 23,
                    CancelationPolicyDetails = new List<CancelationPolicyDetails>
                    {
                        new CancelationPolicyDetails
                        {
                            FromDays = 28,
                            ToDays = 14,
                            Details = new MultiLanguage<string>
                            {
                                En =
                                    "28-14 days prior to arrival: Cancellation penalty charge of 4 nights (or full stay if less than 4 nights) of full rate applicable"
                            }
                        },
                        new CancelationPolicyDetails
                        {
                            FromDays = 13,
                            ToDays = 7,
                            Details = new MultiLanguage<string>
                            {
                                En =
                                    "13-7 days prior to arrival: Cancellation penalty charge of 7 nights (or full stay if less than 7 nights) of full rate applicable"
                            }
                        },
                        new CancelationPolicyDetails
                        {
                            FromDays = 6,
                            ToDays = 0,
                            Details = new MultiLanguage<string>
                                {En = "6 days-No show: Tour operator will be charged full length of booking"}
                        }
                    }
                });

                dbContext.CancelationPolicies.Add(new CancelationPolicy
                {
                    Id = 24,
                    CancelationPolicyDetails = new List<CancelationPolicyDetails>
                    {
                        new CancelationPolicyDetails
                        {
                            FromDays = 13,
                            ToDays = 7,
                            Details = new MultiLanguage<string>
                            {
                                En =
                                    "13-07 days before guest arrival: Cancellation penalty charge of 3 nights (or full stay if less than 3 nights) of full rate applicable"
                            }
                        },
                        new CancelationPolicyDetails
                        {
                            FromDays = 6,
                            ToDays = 0,
                            Details = new MultiLanguage<string>
                            {
                                En =
                                    "6 days-Ni show: Cancellation penalty charge of 5 nights at full tour operator rate (or full stay if less than 5 nights) of full rate applicable"
                            }
                        }
                    }
                });

                #endregion

                #region AddSeasons

                dbContext.Seasons.AddRange(
                    new Season
                    {
                        Id = 11,
                        AccommodationId = hotelId,
                        Name = "HIGH I",
                        StartDate = new DateTime(2020, 1, 8),
                        EndDate = new DateTime(2020, 1, 14),
                        CancelationPolicyId = 23
                    },
                    new Season
                    {
                        Id = 12,
                        AccommodationId = hotelId,
                        Name = "HIGH II",
                        StartDate = new DateTime(2020, 1, 15),
                        EndDate = new DateTime(2020, 1, 24),
                        CancelationPolicyId = 23
                    },
                    new Season
                    {
                        Id = 13,
                        AccommodationId = hotelId,
                        Name = "HIGH I",
                        StartDate = new DateTime(2020, 1, 25),
                        EndDate = new DateTime(2020, 2, 7),
                        CancelationPolicyId = 23
                    },
                    new Season
                    {
                        Id = 14,
                        AccommodationId = hotelId,
                        Name = "PEAK I",
                        StartDate = new DateTime(2020, 2, 8),
                        EndDate = new DateTime(2020, 2, 21),
                        CancelationPolicyId = 23
                    },
                    new Season
                    {
                        Id = 15,
                        AccommodationId = hotelId,
                        Name = "HIGH I",
                        StartDate = new DateTime(2020, 2, 22),
                        EndDate = new DateTime(2020, 3, 20),
                        CancelationPolicyId = 23
                    },
                    new Season
                    {
                        Id = 16,
                        AccommodationId = hotelId,
                        Name = "PEAK I",
                        StartDate = new DateTime(2020, 3, 21),
                        EndDate = new DateTime(2020, 3, 27),
                        CancelationPolicyId = 23
                    },
                    new Season
                    {
                        Id = 17,
                        AccommodationId = hotelId,
                        Name = "PEAK II",
                        StartDate = new DateTime(2020, 3, 28),
                        EndDate = new DateTime(2020, 4, 12),
                        CancelationPolicyId = 22
                    },
                    new Season
                    {
                        Id = 18,
                        AccommodationId = hotelId,
                        Name = "PEAK I",
                        StartDate = new DateTime(2020, 4, 13),
                        EndDate = new DateTime(2020, 4, 18),
                        CancelationPolicyId = 23
                    },
                    new Season
                    {
                        Id = 19,
                        AccommodationId = hotelId,
                        Name = "HIGH I",
                        StartDate = new DateTime(2020, 4, 19),
                        EndDate = new DateTime(2020, 5, 3),
                        CancelationPolicyId = 23
                    },
                    new Season
                    {
                        Id = 20,
                        AccommodationId = hotelId,
                        Name = "SHOULDER",
                        StartDate = new DateTime(2020, 5, 4),
                        EndDate = new DateTime(2020, 5, 31),
                        CancelationPolicyId = 24
                    },
                    new Season
                    {
                        Id = 21,
                        AccommodationId = hotelId,
                        Name = "LOW",
                        StartDate = new DateTime(2020, 7, 1),
                        EndDate = new DateTime(2020, 9, 4),
                        CancelationPolicyId = 24
                    },
                    new Season
                    {
                        Id = 22,
                        AccommodationId = hotelId,
                        Name = "SHOULDER",
                        StartDate = new DateTime(2020, 9, 5),
                        EndDate = new DateTime(2020, 9, 25),
                        CancelationPolicyId = 24
                    },
                    new Season
                    {
                        Id = 23,
                        AccommodationId = hotelId,
                        Name = "HIGH II",
                        StartDate = new DateTime(2020, 9, 26),
                        EndDate = new DateTime(2020, 10, 9),
                        CancelationPolicyId = 23
                    },
                    new Season
                    {
                        Id = 24,
                        AccommodationId = hotelId,
                        Name = "HIGH I",
                        StartDate = new DateTime(2020, 10, 10),
                        EndDate = new DateTime(2020, 10, 16),
                        CancelationPolicyId = 23
                    },
                    new Season
                    {
                        Id = 25,
                        AccommodationId = hotelId,
                        Name = "PEAK I",
                        StartDate = new DateTime(2020, 10, 17),
                        EndDate = new DateTime(2020, 10, 23),
                        CancelationPolicyId = 23
                    },
                    new Season
                    {
                        Id = 26,
                        AccommodationId = hotelId,
                        Name = "PEAK II",
                        StartDate = new DateTime(2020, 10, 24),
                        EndDate = new DateTime(2020, 11, 6),
                        CancelationPolicyId = 22
                    },
                    new Season
                    {
                        Id = 27,
                        AccommodationId = hotelId,
                        Name = "HIGH I",
                        StartDate = new DateTime(2020, 11, 7),
                        EndDate = new DateTime(2020, 12, 4),
                        CancelationPolicyId = 23
                    },
                    new Season
                    {
                        Id = 28,
                        AccommodationId = hotelId,
                        Name = "HIGH II",
                        StartDate = new DateTime(2020, 12, 5),
                        EndDate = new DateTime(2020, 12, 18),
                        CancelationPolicyId = 23
                    },
                    new Season
                    {
                        Id = 29,
                        AccommodationId = hotelId,
                        Name = "HIGH I",
                        StartDate = new DateTime(2020, 12, 19),
                        EndDate = new DateTime(2020, 12, 25),
                        CancelationPolicyId = 23
                    },
                    new Season
                    {
                        Id = 30,
                        AccommodationId = hotelId,
                        Name = "FESTIVE",
                        StartDate = new DateTime(2020, 12, 26),
                        EndDate = new DateTime(2021, 1, 3),
                        CancelationPolicyId = 21
                    },
                    new Season
                    {
                        Id = 31,
                        AccommodationId = hotelId,
                        Name = "PEAK I",
                        StartDate = new DateTime(2021, 1, 4),
                        EndDate = new DateTime(2021, 1, 10),
                        CancelationPolicyId = 23
                    }, new Season
                    {
                        Id = 32,
                        AccommodationId = hotelId,
                        Name = "HIGH I",
                        StartDate = new DateTime(2021, 1, 11),
                        EndDate = new DateTime(2021, 2, 5),
                        CancelationPolicyId = 23
                    }, new Season
                    {
                        Id = 33,
                        AccommodationId = hotelId,
                        Name = "PEAK I",
                        StartDate = new DateTime(2021, 2, 6),
                        EndDate = new DateTime(2021, 2, 19),
                        CancelationPolicyId = 23
                    }, new Season
                    {
                        Id = 34,
                        AccommodationId = hotelId,
                        Name = "HIGH I",
                        StartDate = new DateTime(2021, 2, 20),
                        EndDate = new DateTime(2021, 3, 19),
                        CancelationPolicyId = 23
                    }, new Season
                    {
                        Id = 35,
                        AccommodationId = hotelId,
                        Name = "PEAK I",
                        StartDate = new DateTime(2021, 3, 20),
                        EndDate = new DateTime(2021, 3, 26),
                        CancelationPolicyId = 23
                    }, new Season
                    {
                        Id = 36,
                        AccommodationId = hotelId,
                        Name = "PEAK II",
                        StartDate = new DateTime(2021, 3, 27),
                        EndDate = new DateTime(2021, 4, 10),
                        CancelationPolicyId = 23
                    },
                    new Season
                    {
                        Id = 37,
                        AccommodationId = hotelId,
                        Name = "HIGH I",
                        StartDate = new DateTime(2021, 4, 11),
                        EndDate = new DateTime(2021, 5, 7),
                        CancelationPolicyId = 23
                    }
                );

                #endregion

                #region AddRooms

                dbContext.Rooms.AddRange(new Room
                {
                    Id = 20,
                    AccommodationId = hotelId,
                    Name = new MultiLanguage<string>
                    {
                        En = "Palace Superior Deluxe Room"
                    }
                }, new Room
                {
                    Id = 21,
                    AccommodationId = hotelId,
                    Name = new MultiLanguage<string>
                    {
                        En = "Palace Gold Club Room"
                    }
                }, new Room
                {
                    Id = 22,
                    AccommodationId = hotelId,
                    Name = new MultiLanguage<string>
                    {
                        En = "Palace One Bedroom Executive Suite"
                    }
                }, new Room
                {
                    Id = 23,
                    AccommodationId = hotelId,
                    Name = new MultiLanguage<string>
                    {
                        En = "Palace One Bedroom Golf Club Suite"
                    }
                }, new Room
                {
                    Id = 24,
                    AccommodationId = hotelId,
                    Name = new MultiLanguage<string>
                    {
                        En = "Palace Two Bedroom Executive Suite"
                    }
                }, new Room
                {
                    Id = 25,
                    AccommodationId = hotelId,
                    Name = new MultiLanguage<string>
                    {
                        En = "Palace Two Bedroom Golf Club Suite"
                    }
                }, new Room
                {
                    Id = 26,
                    AccommodationId = hotelId,
                    Name = new MultiLanguage<string>
                    {
                        En = "Palace Two Bedroom Royal Suite"
                    }
                }, new Room
                {
                    Id = 27,
                    AccommodationId = hotelId,
                    Name = new MultiLanguage<string>
                    {
                        En = "Arabian Court Delux Room"
                    }
                }, new Room
                {
                    Id = 28,
                    AccommodationId = hotelId,
                    Name = new MultiLanguage<string>
                    {
                        En = "Arabian Court Two Deluxe Rooms Family Accommodation"
                    }
                }, new Room
                {
                    Id = 29,
                    AccommodationId = hotelId,
                    Name = new MultiLanguage<string>
                    {
                        En = "Arabian Court One Bedroom Executive Suite"
                    }
                }, new Room
                {
                    Id = 30,
                    AccommodationId = hotelId,
                    Name = new MultiLanguage<string>
                    {
                        En = "Arabian Court Two Bedroom Executive Suite"
                    }
                }, new Room
                {
                    Id = 31,
                    AccommodationId = hotelId,
                    Name = new MultiLanguage<string>
                    {
                        En = "Arabian Court Prince Suite"
                    }
                }, new Room
                {
                    Id = 32,
                    AccommodationId = hotelId,
                    Name = new MultiLanguage<string>
                    {
                        En = "Residence Prestige Room"
                    }
                }, new Room
                {
                    Id = 33,
                    AccommodationId = hotelId,
                    Name = new MultiLanguage<string>
                    {
                        En = "Residence Junior Room"
                    }
                }, new Room
                {
                    Id = 34,
                    AccommodationId = hotelId,
                    Name = new MultiLanguage<string>
                    {
                        En = "Residence Executive Suite"
                    }
                }, new Room
                {
                    Id = 35,
                    AccommodationId = hotelId,
                    Name = new MultiLanguage<string>
                    {
                        En = "Residence Garden Beach Villa"
                    }
                });

                #endregion

                #region AddRates

                FillRates(
                    dbContext,
                    new[] {11, 13, 15, 19},
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
                FillRates(
                    dbContext,
                    new[] {12, 23, 28},
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
                FillRates(
                    dbContext,
                    new[] {14, 16, 18, 25},
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
                FillRates(
                    dbContext,
                    new[] {17, 26},
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
                FillRates(
                    dbContext,
                    new[] {20, 22},
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
                        (35, 24000)
                    });
                FillRates(
                    dbContext,
                    new[] {21},
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
                        (35, 24000)
                    });
                FillRates(
                    dbContext,
                    new[] {24, 27, 29},
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
                        (35, 24000)
                    });
                FillRates(
                    dbContext,
                    new[] {30},
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
                        (35, 38000)
                    });
                FillRates(
                    dbContext,
                    new[] {31, 33, 35},
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
                FillRates(
                    dbContext,
                    new[] {32, 34},
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
                FillRates(
                    dbContext,
                    new[] {36},
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
                FillRates(
                    dbContext,
                    new[] {37},
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

                #endregion

                #region DiscountRates

                //palace
                var offers =
                    new (int discountPct, DateTime bookBy, DateTime validFrom, DateTime validTo, string bookingCode,
                        MultiLanguage<string> details)[]
                        {
                            (25, new DateTime(2019, 12, 13), new DateTime(2020, 1, 8), new DateTime(2020, 2, 21), "25% PROMO",
                                new MultiLanguage<string> {En = "Book on or before Friday, DECEMBER 13, 2019 receive a 25% discount"}),
                            (20, new DateTime(2019, 12, 13), new DateTime(2020, 2, 22), new DateTime(2020, 3, 20), "20% PROMO",
                                new MultiLanguage<string> {En = "Book on or before Friday, DECEMBER 13, 2019 receive a 20% discount"}),
                            (30, new DateTime(2019, 12, 13), new DateTime(2020, 3, 21), new DateTime(2020, 3, 27), "30% PROMO",
                                new MultiLanguage<string> {En = "Book on or before Friday, DECEMBER 13, 2019 receive a 30% discount"}),
                            (35, new DateTime(2020, 2, 28), new DateTime(2020, 3, 28), new DateTime(2020, 4, 3), "35% PROMO",
                                new MultiLanguage<string> {En = "Book on or before Friday, FEBRUARY 28, 2020 receive a 35% discount"}),
                            (20, new DateTime(2020, 2, 28), new DateTime(2020, 4, 4), new DateTime(2020, 4, 12), "20% PROMO",
                                new MultiLanguage<string> {En = "Book on or before Friday, FEBRUARY 28, 2020 receive a 20% discount"}),

                            (30, new DateTime(2020, 2, 28), new DateTime(2020, 4, 13), new DateTime(2020, 4, 18), "30% PROMO",
                                new MultiLanguage<string> {En = "Book on or before Friday, FEBRUARY 28, 2020 receive a 30% discount"}),

                            (25, new DateTime(2020, 2, 28), new DateTime(2020, 4, 19), new DateTime(2020, 5, 3), "25% PROMO",
                                new MultiLanguage<string> {En = "Book on or before Friday, FEBRUARY 28, 2020 receive a 25% discount"}),

                            (30, new DateTime(2020, 4, 3), new DateTime(2020, 5, 4), new DateTime(2020, 5, 31), "30% PROMO",
                                new MultiLanguage<string> {En = "Book on or before Friday, APRIL 28, 2020 receive a 30% discount"}),

                            (30, new DateTime(2020, 5, 8), new DateTime(2020, 6, 1), new DateTime(2020, 9, 4), "30% PROMO",
                                new MultiLanguage<string> {En = "Book on or before Friday, MAY 8, 2020 receive a 30% discount"}),

                            (40, new DateTime(2020, 8, 14), new DateTime(2020, 9, 5), new DateTime(2020, 9, 25), "40% PROMO",
                                new MultiLanguage<string> {En = "Book on or before Friday, AUGUST 14, 2020 receive a 40% discount"}),

                            (25, new DateTime(2020, 8, 14), new DateTime(2020, 9, 26), new DateTime(2020, 10, 16), "25% PROMO",
                                new MultiLanguage<string> {En = "Book on or before Friday, AUGUST 14, 2020 receive a 25% discount"}),

                            (20, new DateTime(2020, 8, 14), new DateTime(2020, 10, 17), new DateTime(2020, 12, 4), "20% PROMO",
                                new MultiLanguage<string> {En = "Book on or before Friday, AUGUST 14, 2020 receive a 20% discount"}),

                            (30, new DateTime(2020, 10, 2), new DateTime(2020, 12, 5), new DateTime(2020, 12, 18), "30% PROMO",
                                new MultiLanguage<string> {En = "Book on or before Friday, OCTOBER 2, 2020 receive a 30% discount"}),

                            (20, new DateTime(2020, 11, 1), new DateTime(2020, 12, 19), new DateTime(2020, 12, 25), "20% PROMO",
                                new MultiLanguage<string> {En = "Book on or before Friday, NOVEMBER 1, 2020 receive a 20% discount"}),

                            (10, new DateTime(2020, 11, 1), new DateTime(2020, 12, 26), new DateTime(2021, 1, 3), "10% PROMO",
                                new MultiLanguage<string> {En = "Book on or before Friday, NOVEMBER 1, 2020 receive a 10% discount"}),

                            (20, new DateTime(2020, 11, 1), new DateTime(2021, 1, 4), new DateTime(2021, 4, 10), "20% PROMO",
                                new MultiLanguage<string> {En = "Book on or before Friday, NOVEMBER 1, 2020 receive a 20% discount"}),

                            (20, new DateTime(2021, 2, 26), new DateTime(2021, 4, 11), new DateTime(2021, 5, 7), "20% PROMO",
                                new MultiLanguage<string> {En = "Book on or before Friday, FEBRUARY 28, 2021 receive a 20% discount"})
                        };
                FillDiscountRates(dbContext, new[] {20, 21, 22, 23, 24, 25, 26}, offers);

                //arabian court
                offers =
                    new (int discountPct, DateTime bookBy, DateTime validFrom, DateTime validTo, string bookingCode,
                        MultiLanguage<string> details)[]
                        {
                            (30, new DateTime(2019, 12, 13), new DateTime(2020, 1, 8), new DateTime(2020, 2, 21), "30% PROMO",
                                new MultiLanguage<string> {En = "Book on or before Friday, DECEMBER 13, 2019 receive a 30% discount"}),
                            (25, new DateTime(2019, 12, 13), new DateTime(2020, 2, 22), new DateTime(2020, 3, 20), "25% PROMO",
                                new MultiLanguage<string> {En = "Book on or before Friday, DECEMBER 13, 2019 receive a 25% discount"}),
                            (35, new DateTime(2019, 12, 13), new DateTime(2020, 3, 21), new DateTime(2020, 3, 27), "35% PROMO",
                                new MultiLanguage<string> {En = "Book on or before Friday, DECEMBER 13, 2019 receive a 35% discount"}),
                            (35, new DateTime(2020, 2, 28), new DateTime(2020, 3, 28), new DateTime(2020, 4, 3), "35% PROMO",
                                new MultiLanguage<string> {En = "Book on or before Friday, FEBRUARY 28, 2020 receive a 35% discount"}),
                            (25, new DateTime(2020, 2, 28), new DateTime(2020, 4, 4), new DateTime(2020, 4, 12), "25% PROMO",
                                new MultiLanguage<string> {En = "Book on or before Friday, FEBRUARY 28, 2020 receive a 25% discount"}),
                            (35, new DateTime(2020, 2, 28), new DateTime(2020, 4, 13), new DateTime(2020, 4, 18), "35% PROMO",
                                new MultiLanguage<string> {En = "Book on or before Friday, FEBRUARY 28, 2020 receive a 35% discount"}),
                            (30, new DateTime(2020, 2, 28), new DateTime(2020, 4, 19), new DateTime(2020, 5, 3), "30% PROMO",
                                new MultiLanguage<string> {En = "Book on or before Friday, FEBRUARY 28, 2020 receive a 30% discount"}),
                            (35, new DateTime(2020, 4, 3), new DateTime(2020, 5, 4), new DateTime(2020, 5, 31), "35% PROMO",
                                new MultiLanguage<string> {En = "Book on or before Friday, APRIL 28, 2020 receive a 35% discount"}),
                            (30, new DateTime(2020, 5, 8), new DateTime(2020, 6, 1), new DateTime(2020, 9, 4), "30% PROMO",
                                new MultiLanguage<string> {En = "Book on or before Friday, MAY 8, 2020 receive a 30% discount"}),
                            (30, new DateTime(2020, 8, 14), new DateTime(2020, 9, 5), new DateTime(2020, 9, 25), "30% PROMO",
                                new MultiLanguage<string> {En = "Book on or before Friday, AUGUST 14, 2020 receive a 30% discount"}),
                            (30, new DateTime(2020, 8, 14), new DateTime(2020, 9, 26), new DateTime(2020, 10, 16), "30% PROMO",
                                new MultiLanguage<string> {En = "Book on or before Friday, AUGUST 14, 2020 receive a 30% discount"}),
                            (20, new DateTime(2020, 8, 14), new DateTime(2020, 10, 17), new DateTime(2020, 12, 4), "20% PROMO",
                                new MultiLanguage<string> {En = "Book on or before Friday, AUGUST 14, 2020 receive a 20% discount"}),
                            (30, new DateTime(2020, 10, 2), new DateTime(2020, 12, 5), new DateTime(2020, 12, 18), "30% PROMO",
                                new MultiLanguage<string> {En = "Book on or before Friday, OCTOBER 2, 2020 receive a 30% discount"}),
                            (20, new DateTime(2020, 11, 1), new DateTime(2020, 12, 19), new DateTime(2020, 12, 25), "20% PROMO",
                                new MultiLanguage<string> {En = "Book on or before Friday, NOVEMBER 1, 2020 receive a 20% discount"}),
                            (10, new DateTime(2020, 11, 1), new DateTime(2020, 12, 26), new DateTime(2021, 1, 3), "10% PROMO",
                                new MultiLanguage<string> {En = "Book on or before Friday, NOVEMBER 1, 2020 receive a 10% discount"}),
                            (20, new DateTime(2020, 11, 1), new DateTime(2021, 1, 4), new DateTime(2021, 3, 10), "20% PROMO",
                                new MultiLanguage<string> {En = "Book on or before Friday, NOVEMBER 1, 2020 receive a 20% discount"}),
                            (20, new DateTime(2021, 2, 26), new DateTime(2021, 3, 11), new DateTime(2021, 5, 7), "20% PROMO",
                                new MultiLanguage<string> {En = "Book on or before Friday, FEBRUARY 26, 2021 receive a 20% discount"})
                        };
                FillDiscountRates(dbContext, new[] {27, 28, 29, 30, 31}, offers);

                //residence and spa
                offers =
                    new (int discountPct, DateTime bookBy, DateTime validFrom, DateTime validTo, string bookingCode,
                        MultiLanguage<string> details)[]
                        {
                            (30, new DateTime(2019, 12, 13), new DateTime(2020, 1, 8), new DateTime(2020, 2, 21), "30% PROMO",
                                new MultiLanguage<string> {En = "Book on or before Friday, DECEMBER 13, 2019 receive a 30% discount"}),
                            (25, new DateTime(2019, 12, 13), new DateTime(2020, 2, 22), new DateTime(2020, 3, 20), "25% PROMO",
                                new MultiLanguage<string> {En = "Book on or before Friday, DECEMBER 13, 2019 receive a 25% discount"}),
                            (35, new DateTime(2019, 12, 13), new DateTime(2020, 3, 21), new DateTime(2020, 3, 27), "35% PROMO",
                                new MultiLanguage<string> {En = "Book on or before Friday, DECEMBER 13, 2019 receive a 35% discount"}),
                            (35, new DateTime(2020, 2, 28), new DateTime(2020, 3, 28), new DateTime(2020, 4, 3), "35% PROMO",
                                new MultiLanguage<string> {En = "Book on or before Friday, FEBRUARY 28, 2020 receive a 35% discount"}),
                            (25, new DateTime(2020, 2, 28), new DateTime(2020, 4, 4), new DateTime(2020, 4, 12), "25% PROMO",
                                new MultiLanguage<string> {En = "Book on or before Friday, FEBRUARY 28, 2020 receive a 25% discount"}),
                            (35, new DateTime(2020, 2, 28), new DateTime(2020, 4, 13), new DateTime(2020, 4, 18), "35% PROMO",
                                new MultiLanguage<string> {En = "Book on or before Friday, FEBRUARY 28, 2020 receive a 35% discount"}),
                            (30, new DateTime(2020, 2, 28), new DateTime(2020, 4, 19), new DateTime(2020, 5, 3), "30% PROMO",
                                new MultiLanguage<string> {En = "Book on or before Friday, FEBRUARY 28, 2020 receive a 30% discount"}),
                            (35, new DateTime(2020, 4, 3), new DateTime(2020, 5, 4), new DateTime(2020, 5, 31), "35% PROMO",
                                new MultiLanguage<string> {En = "Book on or before Friday, APRIL 28, 2020 receive a 35% discount"}),
                            (30, new DateTime(2020, 5, 8), new DateTime(2020, 6, 1), new DateTime(2020, 9, 4), "30% PROMO",
                                new MultiLanguage<string> {En = "Book on or before Friday, MAY 8, 2020 receive a 30% discount"}),
                            (30, new DateTime(2020, 8, 14), new DateTime(2020, 9, 5), new DateTime(2020, 9, 25), "30% PROMO",
                                new MultiLanguage<string> {En = "Book on or before Friday, AUGUST 14, 2020 receive a 30% discount"}),
                            (30, new DateTime(2020, 8, 14), new DateTime(2020, 9, 26), new DateTime(2020, 10, 16), "30% PROMO",
                                new MultiLanguage<string> {En = "Book on or before Friday, AUGUST 14, 2020 receive a 30% discount"}),
                            (20, new DateTime(2020, 8, 14), new DateTime(2020, 10, 17), new DateTime(2020, 12, 4), "20% PROMO",
                                new MultiLanguage<string> {En = "Book on or before Friday, AUGUST 14, 2020 receive a 20% discount"}),
                            (30, new DateTime(2020, 10, 2), new DateTime(2020, 12, 5), new DateTime(2020, 12, 18), "30% PROMO",
                                new MultiLanguage<string> {En = "Book on or before Friday, OCTOBER 2, 2020 receive a 30% discount"}),
                            (20, new DateTime(2020, 11, 1), new DateTime(2020, 12, 19), new DateTime(2020, 12, 25), "20% PROMO",
                                new MultiLanguage<string> {En = "Book on or before Friday, NOVEMBER 1, 2020 receive a 20% discount"}),
                            (10, new DateTime(2020, 11, 1), new DateTime(2020, 12, 26), new DateTime(2021, 1, 3), "10% PROMO",
                                new MultiLanguage<string> {En = "Book on or before Friday, NOVEMBER 1, 2020 receive a 10% discount"}),
                            (20, new DateTime(2020, 11, 1), new DateTime(2021, 1, 4), new DateTime(2021, 3, 10), "20% PROMO",
                                new MultiLanguage<string> {En = "Book on or before Friday, NOVEMBER 1, 2020 receive a 20% discount"}),
                            (20, new DateTime(2021, 2, 26), new DateTime(2021, 3, 11), new DateTime(2021, 5, 7), "20% PROMO",
                                new MultiLanguage<string> {En = "Book on or before Friday, FEBRUARY 28, 2021 receive a 20% discount"})
                        };
                FillDiscountRates(dbContext, new[] {32, 33, 34, 35}, offers);

                #endregion

                #region AddStopSaleDate //Test data

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

                #region AddRoomDetails

                dbContext.RoomDetails.AddRange(
                    //20
                    new RoomDetails
                    {
                        RoomId = 20,
                        AdultsNumber = 2,
                        ChildrenNumber = 1,
                        ChildrenAges = new NpgsqlRange<int>(12, true, 16, true),
                        InfantsNumber = 0
                    }
                    , new RoomDetails
                    {
                        RoomId = 20,
                        AdultsNumber = 2,
                        ChildrenNumber = 1,
                        ChildrenAges = new NpgsqlRange<int>(4, true, 11, true),
                        InfantsNumber = 1
                    },
                    new RoomDetails
                    {
                        RoomId = 20,
                        AdultsNumber = 1,
                        ChildrenNumber = 0,
                        InfantsNumber = 0
                    },
                    new RoomDetails
                    {
                        RoomId = 20,
                        AdultsNumber = 2,
                        ChildrenNumber = 0,
                        InfantsNumber = 0
                    },
                    //21
                    new RoomDetails
                    {
                        RoomId = 21,
                        AdultsNumber = 2,
                        ChildrenNumber = 1,
                        ChildrenAges = new NpgsqlRange<int>(4, true, 11, true),
                        InfantsNumber = 1
                    }, new RoomDetails
                    {
                        RoomId = 21,
                        AdultsNumber = 2,
                        ChildrenNumber = 1,
                        ChildrenAges = new NpgsqlRange<int>(12, true, 16, true),
                        InfantsNumber = 0
                    },
                    new RoomDetails
                    {
                        RoomId = 21,
                        AdultsNumber = 1,
                        ChildrenNumber = 0,
                        InfantsNumber = 0
                    },
                    new RoomDetails
                    {
                        RoomId = 21,
                        AdultsNumber = 2,
                        ChildrenNumber = 0,
                        InfantsNumber = 0
                    }
                    //22
                    , new RoomDetails
                    {
                        RoomId = 22,
                        AdultsNumber = 2,
                        ChildrenNumber = 1,
                        ChildrenAges = new NpgsqlRange<int>(12, true, 16, true),
                        InfantsNumber = 0
                    }, new RoomDetails
                    {
                        RoomId = 22,
                        AdultsNumber = 2,
                        ChildrenNumber = 1,
                        ChildrenAges = new NpgsqlRange<int>(4, true, 11, true),
                        InfantsNumber = 1
                    },
                    new RoomDetails
                    {
                        RoomId = 22,
                        AdultsNumber = 1,
                        ChildrenNumber = 0,
                        InfantsNumber = 0
                    },
                    new RoomDetails
                    {
                        RoomId = 22,
                        AdultsNumber = 2,
                        ChildrenNumber = 0,
                        InfantsNumber = 0
                    },
                    //23
                    new RoomDetails
                    {
                        RoomId = 23,
                        AdultsNumber = 2,
                        ChildrenNumber = 1,
                        ChildrenAges = new NpgsqlRange<int>(12, true, 16, true),
                        InfantsNumber = 0
                    }, new RoomDetails
                    {
                        RoomId = 23,
                        AdultsNumber = 2,
                        ChildrenNumber = 1,
                        ChildrenAges = new NpgsqlRange<int>(4, true, 11, true),
                        InfantsNumber = 1
                    }, new RoomDetails
                    {
                        RoomId = 23,
                        AdultsNumber = 1,
                        ChildrenNumber = 0,
                        InfantsNumber = 0
                    },
                    new RoomDetails
                    {
                        RoomId = 23,
                        AdultsNumber = 2,
                        ChildrenNumber = 0,
                        InfantsNumber = 0
                    },
                    //24
                    new RoomDetails
                    {
                        RoomId = 24,
                        AdultsNumber = 4,
                        ChildrenNumber = 1,
                        ChildrenAges = new NpgsqlRange<int>(4, true, 16, true),
                        InfantsNumber = 0
                    }, new RoomDetails
                    {
                        RoomId = 24,
                        AdultsNumber = 1,
                        ChildrenNumber = 0,
                        InfantsNumber = 0
                    },
                    new RoomDetails
                    {
                        RoomId = 24,
                        AdultsNumber = 2,
                        ChildrenNumber = 0,
                        InfantsNumber = 0
                    }, new RoomDetails
                    {
                        RoomId = 24,
                        AdultsNumber = 3,
                        ChildrenNumber = 0,
                        InfantsNumber = 0
                    },
                    //25
                    new RoomDetails
                    {
                        RoomId = 25,
                        AdultsNumber = 4,
                        ChildrenNumber = 1,
                        ChildrenAges = new NpgsqlRange<int>(4, true, 16, true),
                        InfantsNumber = 0
                    },
                    new RoomDetails
                    {
                        RoomId = 25,
                        AdultsNumber = 1,
                        ChildrenNumber = 0,
                        InfantsNumber = 0
                    },
                    new RoomDetails
                    {
                        RoomId = 25,
                        AdultsNumber = 2,
                        ChildrenNumber = 0,
                        InfantsNumber = 0
                    }, new RoomDetails
                    {
                        RoomId = 25,
                        AdultsNumber = 3,
                        ChildrenNumber = 0,
                        InfantsNumber = 0
                    },
                    //26
                    new RoomDetails
                    {
                        RoomId = 26,
                        AdultsNumber = 4,
                        ChildrenNumber = 1,
                        ChildrenAges = new NpgsqlRange<int>(4, true, 16, true),
                        InfantsNumber = 0
                    }, new RoomDetails
                    {
                        RoomId = 26,
                        AdultsNumber = 1,
                        ChildrenNumber = 0,
                        InfantsNumber = 0
                    },
                    new RoomDetails
                    {
                        RoomId = 26,
                        AdultsNumber = 2,
                        ChildrenNumber = 0,
                        InfantsNumber = 0
                    }, new RoomDetails
                    {
                        RoomId = 26,
                        AdultsNumber = 3,
                        ChildrenNumber = 0,
                        InfantsNumber = 0
                    }
                    //27
                    , new RoomDetails
                    {
                        RoomId = 27,
                        AdultsNumber = 2,
                        ChildrenNumber = 1,
                        ChildrenAges = new NpgsqlRange<int>(12, true, 16, true),
                        InfantsNumber = 0
                    }, new RoomDetails
                    {
                        RoomId = 27,
                        AdultsNumber = 2,
                        ChildrenNumber = 1,
                        ChildrenAges = new NpgsqlRange<int>(4, true, 11, true),
                        InfantsNumber = 0
                    }, new RoomDetails
                    {
                        RoomId = 27,
                        AdultsNumber = 1,
                        ChildrenNumber = 0,
                        InfantsNumber = 0
                    },
                    new RoomDetails
                    {
                        RoomId = 27,
                        AdultsNumber = 2,
                        ChildrenNumber = 0,
                        InfantsNumber = 0
                    },
                    //28
                    new RoomDetails
                    {
                        RoomId = 28,
                        AdultsNumber = 3,
                        ChildrenNumber = 3,
                        ChildrenAges = new NpgsqlRange<int>(4, true, 11, true)
                    }, new RoomDetails
                    {
                        RoomId = 28,
                        AdultsNumber = 1,
                        ChildrenNumber = 0,
                        InfantsNumber = 0
                    },
                    new RoomDetails
                    {
                        RoomId = 28,
                        AdultsNumber = 2,
                        ChildrenNumber = 0,
                        InfantsNumber = 0
                    },
                    new RoomDetails
                    {
                        RoomId = 28,
                        AdultsNumber = 3,
                        ChildrenNumber = 0,
                        InfantsNumber = 0
                    },
                    //29
                    new RoomDetails
                    {
                        RoomId = 29,
                        AdultsNumber = 2,
                        ChildrenNumber = 1,
                        ChildrenAges = new NpgsqlRange<int>(12, true, 16, true),
                        InfantsNumber = 0
                    }, new RoomDetails
                    {
                        RoomId = 29,
                        AdultsNumber = 2,
                        ChildrenNumber = 1,
                        ChildrenAges = new NpgsqlRange<int>(4, true, 11, true),
                        InfantsNumber = 1
                    }, new RoomDetails
                    {
                        RoomId = 29,
                        AdultsNumber = 1,
                        ChildrenNumber = 0,
                        InfantsNumber = 0
                    }, new RoomDetails
                    {
                        RoomId = 29,
                        AdultsNumber = 2,
                        ChildrenNumber = 0,
                        InfantsNumber = 0
                    },
                    //30
                    new RoomDetails
                    {
                        RoomId = 30,
                        AdultsNumber = 4,
                        ChildrenNumber = 1,
                        ChildrenAges = new NpgsqlRange<int>(4, true, 16, true),
                        InfantsNumber = 0
                    },
                    new RoomDetails
                    {
                        RoomId = 30,
                        AdultsNumber = 3,
                        ChildrenNumber = 0,
                        InfantsNumber = 0
                    },
                    new RoomDetails
                    {
                        RoomId = 30,
                        AdultsNumber = 4,
                        ChildrenNumber = 0,
                        InfantsNumber = 0
                    },
                    //31
                    new RoomDetails
                    {
                        RoomId = 31,
                        AdultsNumber = 2,
                        ChildrenNumber = 1,
                        ChildrenAges = new NpgsqlRange<int>(12, true, 16, true),
                        InfantsNumber = 0
                    }, new RoomDetails
                    {
                        RoomId = 31,
                        AdultsNumber = 2,
                        ChildrenNumber = 1,
                        ChildrenAges = new NpgsqlRange<int>(4, true, 11, true),
                        InfantsNumber = 1
                    }, new RoomDetails
                    {
                        RoomId = 31,
                        AdultsNumber = 1,
                        ChildrenNumber = 0,
                        InfantsNumber = 0
                    }, new RoomDetails
                    {
                        RoomId = 31,
                        AdultsNumber = 2,
                        ChildrenNumber = 0,
                        InfantsNumber = 0
                    },
                    //32
                    new RoomDetails
                    {
                        RoomId = 32,
                        AdultsNumber = 4,
                        ChildrenNumber = 1,
                        ChildrenAges = new NpgsqlRange<int>(12, true, 16, true),
                        InfantsNumber = 0
                    }, new RoomDetails
                    {
                        RoomId = 32,
                        AdultsNumber = 4,
                        ChildrenNumber = 1,
                        ChildrenAges = new NpgsqlRange<int>(4, true, 11, true),
                        InfantsNumber = 1
                    },
                    new RoomDetails
                    {
                        RoomId = 32,
                        AdultsNumber = 1,
                        ChildrenNumber = 0,
                        InfantsNumber = 0
                    }, new RoomDetails
                    {
                        RoomId = 32,
                        AdultsNumber = 2,
                        ChildrenNumber = 0,
                        InfantsNumber = 0
                    }
                    //33
                    , new RoomDetails
                    {
                        RoomId = 33,
                        AdultsNumber = 2,
                        ChildrenNumber = 1,
                        ChildrenAges = new NpgsqlRange<int>(12, true, 16, true),
                        InfantsNumber = 0
                    }, new RoomDetails
                    {
                        RoomId = 33,
                        AdultsNumber = 2,
                        ChildrenNumber = 1,
                        ChildrenAges = new NpgsqlRange<int>(4, true, 11, true),
                        InfantsNumber = 1
                    }, new RoomDetails
                    {
                        RoomId = 33,
                        AdultsNumber = 1,
                        ChildrenNumber = 0,
                        InfantsNumber = 0
                    }, new RoomDetails
                    {
                        RoomId = 33,
                        AdultsNumber = 2,
                        ChildrenNumber = 0,
                        InfantsNumber = 0
                    },
                    //34
                    new RoomDetails
                    {
                        RoomId = 34,
                        AdultsNumber = 2,
                        ChildrenNumber = 1,
                        ChildrenAges = new NpgsqlRange<int>(4, true, 11, true),
                        InfantsNumber = 1
                    }, new RoomDetails
                    {
                        RoomId = 34,
                        AdultsNumber = 2,
                        ChildrenNumber = 1,
                        ChildrenAges = new NpgsqlRange<int>(12, true, 16, true),
                        InfantsNumber = 0
                    }, new RoomDetails
                    {
                        RoomId = 34,
                        AdultsNumber = 1,
                        ChildrenNumber = 0,
                        InfantsNumber = 0
                    }, new RoomDetails
                    {
                        RoomId = 34,
                        AdultsNumber = 2,
                        ChildrenNumber = 0,
                        InfantsNumber = 0
                    },
                    //35
                    new RoomDetails
                    {
                        RoomId = 35,
                        AdultsNumber = 4,
                        ChildrenNumber = 1,
                        ChildrenAges = new NpgsqlRange<int>(4, true, 16, true),
                        InfantsNumber = 0
                    }, new RoomDetails
                    {
                        RoomId = 35,
                        AdultsNumber = 1,
                        ChildrenNumber = 0,
                        InfantsNumber = 0
                    }, new RoomDetails
                    {
                        RoomId = 35,
                        AdultsNumber = 2,
                        ChildrenNumber = 0,
                        InfantsNumber = 0
                    }, new RoomDetails
                    {
                        RoomId = 35,
                        AdultsNumber = 3,
                        ChildrenNumber = 0,
                        InfantsNumber = 0
                    }, new RoomDetails
                    {
                        RoomId = 35,
                        AdultsNumber = 4,
                        ChildrenNumber = 0,
                        InfantsNumber = 0
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

                dbContext.Accommodations.AddRange(new Accommodation
                {
                    Id = hotelId,
                    Rating = AccommodationRatings.FiveStars,
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
                        CheckInTime = "15:00",
                        CheckOutTime = "10:00",
                        PortersStartTime = "09:00",
                        PortersEndTime = "19:00"
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
                                "Легендарный отель Burj Al Arab Jumeirah известен своим непревзойденным уровнем обслуживания и гостеприимства, а его высокий силуэт в форме паруса служит маяком современного Дубая."
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

                #region AddCancelationPolicies

                dbContext.CancelationPolicies.Add(new CancelationPolicy
                {
                    Id = 11,
                    CancelationPolicyDetails = new List<CancelationPolicyDetails>
                    {
                        new CancelationPolicyDetails
                        {
                            FromDays = 7,
                            ToDays = 0,
                            Details = new MultiLanguage<string>
                                {En = "Within seven (7) days or less prior to arrival at 15:00 hours, 100% charge on total booking"}
                        }
                    }
                });

                dbContext.CancelationPolicies.Add(new CancelationPolicy
                {
                    Id = 12,
                    CancelationPolicyDetails = new List<CancelationPolicyDetails>
                    {
                        new CancelationPolicyDetails
                        {
                            FromDays = 14,
                            ToDays = 0,
                            Details = new MultiLanguage<string>
                                {En = "Within fourteen (14) days or less prior to arrival at 15:00 hours, 100% charge on total booking"}
                        }
                    }
                });

                dbContext.CancelationPolicies.Add(new CancelationPolicy
                {
                    Id = 13,
                    CancelationPolicyDetails = new List<CancelationPolicyDetails>
                    {
                        new CancelationPolicyDetails
                        {
                            FromDays = 21,
                            ToDays = 0,
                            Details = new MultiLanguage<string>
                                {En = "Within twenty one (21) days or less prior to arrival at 15:00 hours, 100% charge on total booking"}
                        }
                    }
                });

                dbContext.CancelationPolicies.Add(new CancelationPolicy
                {
                    Id = 14,
                    CancelationPolicyDetails = new List<CancelationPolicyDetails>
                    {
                        new CancelationPolicyDetails
                        {
                            FromDays = 35,
                            ToDays = 0,
                            Details = new MultiLanguage<string>
                                {En = "Within thirty five (35) days or less prior to arrival at 15:00 hours, 100% charge on total booking"}
                        }
                    }
                });

                dbContext.CancelationPolicies.Add(new CancelationPolicy
                {
                    Id = 15,
                    CancelationPolicyDetails = new List<CancelationPolicyDetails>
                    {
                        new CancelationPolicyDetails
                        {
                            FromDays = 35,
                            ToDays = 0,
                            Details = new MultiLanguage<string>
                                {En = "Within thirty five (35) days or less prior to arrival at 15:00 hours, 100% charge on total booking"}
                        }
                    }
                });

                #endregion

                #region AddSeasons

                dbContext.Seasons.AddRange(
                    new Season
                    {
                        Id = 51,
                        AccommodationId = hotelId,
                        Name = "Shoulder",
                        StartDate = new DateTime(2019, 1, 14),
                        EndDate = new DateTime(2019, 1, 31),
                        CancelationPolicyId = 12
                    },
                    new Season
                    {
                        Id = 52,
                        AccommodationId = hotelId,
                        Name = "High",
                        StartDate = new DateTime(2019, 2, 1),
                        EndDate = new DateTime(2019, 2, 4),
                        CancelationPolicyId = 13
                    },
                    new Season
                    {
                        Id = 53,
                        AccommodationId = hotelId,
                        Name = "Peak",
                        StartDate = new DateTime(2019, 2, 5),
                        EndDate = new DateTime(2019, 2, 11),
                        CancelationPolicyId = 14
                    },
                    new Season
                    {
                        Id = 54,
                        AccommodationId = hotelId,
                        Name = "High",
                        StartDate = new DateTime(2019, 2, 12),
                        EndDate = new DateTime(2019, 3, 27),
                        CancelationPolicyId = 13
                    },
                    new Season
                    {
                        Id = 55,
                        AccommodationId = hotelId,
                        Name = "Peak",
                        StartDate = new DateTime(2019, 3, 28),
                        EndDate = new DateTime(2019, 4, 21),
                        CancelationPolicyId = 14
                    },
                    new Season
                    {
                        Id = 56,
                        AccommodationId = hotelId,
                        Name = "High",
                        StartDate = new DateTime(2019, 4, 22),
                        EndDate = new DateTime(2019, 5, 5),
                        CancelationPolicyId = 13
                    },
                    new Season
                    {
                        Id = 57,
                        AccommodationId = hotelId,
                        Name = "Low",
                        StartDate = new DateTime(2019, 5, 6),
                        EndDate = new DateTime(2019, 8, 31),
                        CancelationPolicyId = 11
                    },
                    new Season
                    {
                        Id = 58,
                        AccommodationId = hotelId,
                        Name = "Shoulder",
                        StartDate = new DateTime(2019, 9, 1),
                        EndDate = new DateTime(2019, 10, 12),
                        CancelationPolicyId = 12
                    },
                    new Season
                    {
                        Id = 59,
                        AccommodationId = hotelId,
                        Name = "High",
                        StartDate = new DateTime(2019, 10, 13),
                        EndDate = new DateTime(2019, 10, 19),
                        CancelationPolicyId = 13
                    },
                    new Season
                    {
                        Id = 60,
                        AccommodationId = hotelId,
                        Name = "Peak",
                        StartDate = new DateTime(2019, 10, 20),
                        EndDate = new DateTime(2019, 11, 9),
                        CancelationPolicyId = 14
                    },
                    new Season
                    {
                        Id = 61,
                        AccommodationId = hotelId,
                        Name = "High",
                        StartDate = new DateTime(2019, 11, 10),
                        EndDate = new DateTime(2019, 12, 1),
                        CancelationPolicyId = 13
                    },
                    new Season
                    {
                        Id = 62,
                        AccommodationId = hotelId,
                        Name = "Shoulder",
                        StartDate = new DateTime(2019, 12, 2),
                        EndDate = new DateTime(2019, 12, 21),
                        CancelationPolicyId = 12
                    },
                    new Season
                    {
                        Id = 63,
                        AccommodationId = hotelId,
                        Name = "High",
                        StartDate = new DateTime(2019, 12, 22),
                        EndDate = new DateTime(2019, 12, 26),
                        CancelationPolicyId = 13
                    },
                    new Season
                    {
                        Id = 64,
                        AccommodationId = hotelId,
                        Name = "Festive",
                        StartDate = new DateTime(2019, 12, 27),
                        EndDate = new DateTime(2020, 1, 4),
                        CancelationPolicyId = 15
                    },
                    new Season
                    {
                        Id = 65,
                        AccommodationId = hotelId,
                        Name = "High",
                        StartDate = new DateTime(2020, 1, 5),
                        EndDate = new DateTime(2020, 1, 13),
                        CancelationPolicyId = 13
                    });

                #endregion

                #region AddRooms

                dbContext.Rooms.AddRange(new Room
                {
                    Id = 71,
                    AccommodationId = hotelId,
                    Name = new MultiLanguage<string>
                    {
                        En = "One Bedroom Deluxe Suite"
                    }
                }, new Room
                {
                    Id = 72,
                    AccommodationId = hotelId,
                    Name = new MultiLanguage<string>
                    {
                        En = "Panoramic One Bedroom Suite"
                    }
                }, new Room
                {
                    Id = 73,
                    AccommodationId = hotelId,
                    Name = new MultiLanguage<string>
                    {
                        En = "Two Bedroom Delux Suite"
                    }
                }, new Room
                {
                    Id = 74,
                    AccommodationId = hotelId,
                    Name = new MultiLanguage<string>
                    {
                        En = "Diplomatic Three Bedroom Suite"
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

                #region AddDiscountRates

                var discounts =
                    new (int discountPercent, DateTime bookBy, DateTime validFrom, DateTime validTo, string bookingCode,
                        MultiLanguage<string> details)[]
                        {
                            (20, new DateTime(2018, 11, 29), new DateTime(2019, 1, 14), new DateTime(2019, 1, 31), "WWHL600",
                                new MultiLanguage<string> {En = "Book by NOVEMBER 29, 2018 receive a 20% discount"}),

                            (25, new DateTime(2018, 11, 29), new DateTime(2019, 2, 1), new DateTime(2019, 2, 4), "WWHL601",
                                new MultiLanguage<string> {En = "Book by NOVEMBER 29, 2019 receive a 25% discount"}),

                            (20, new DateTime(2018, 11, 29), new DateTime(2019, 2, 5), new DateTime(2019, 2, 11), "WWHL602",
                                new MultiLanguage<string> {En = "Book by NOVEMBER 29, 2019 receive a 20% discount"}),

                            (30, new DateTime(2018, 11, 29), new DateTime(2019, 2, 12), new DateTime(2019, 3, 27), "WWHL603",
                                new MultiLanguage<string> {En = "Book by NOVEMBER 29, 2019 receive a 30% discount"}),

                            (30, new DateTime(2019, 1, 31), new DateTime(2019, 3, 28), new DateTime(2019, 4, 21), "WWHL604",
                                new MultiLanguage<string> {En = "Book by JANUARY 1, 2019 receive a 30% discount"}),

                            (30, new DateTime(2019, 1, 31), new DateTime(2019, 4, 22), new DateTime(2019, 5, 5), "WWHL605",
                                new MultiLanguage<string> {En = "Book by JANUARY 1, 2019 receive a 30% discount"}),

                            (30, new DateTime(2019, 2, 28), new DateTime(2019, 5, 6), new DateTime(2019, 8, 31), "WWHL606",
                                new MultiLanguage<string> {En = "Book by FEBRUARY 28, 2019 receive a 30% discount"}),

                            (25, new DateTime(2019, 2, 28), new DateTime(2019, 9, 1), new DateTime(2020, 10, 12), "WWHL607",
                                new MultiLanguage<string> {En = "Book by FEBRUARY 28, 2019 receive a 25% discount"}),

                            (25, new DateTime(2019, 5, 30), new DateTime(2019, 10, 13), new DateTime(2019, 10, 19), "WWHL608",
                                new MultiLanguage<string> {En = "Book by MAY 20, 2019 receive a 25% discount"}),

                            (30, new DateTime(2019, 7, 31), new DateTime(2019, 10, 20), new DateTime(2019, 11, 9), "WWHL609",
                                new MultiLanguage<string> {En = "Book by JULY 31, 2019 receive a 30% "}),

                            (25, new DateTime(2019, 7, 31), new DateTime(2019, 11, 10), new DateTime(2019, 12, 1), "WWHL610",
                                new MultiLanguage<string> {En = "Book by JULY 31, 2019 receive a 25% discount"}),

                            (25, new DateTime(2019, 9, 30), new DateTime(2019, 12, 2), new DateTime(2019, 12, 21), "WWHL611",
                                new MultiLanguage<string> {En = "Book by SEPTEMBER 30, 2019 receive a 25% discount"}),

                            (15, new DateTime(2019, 9, 30), new DateTime(2019, 12, 22), new DateTime(2019, 12, 26), "WWHL612",
                                new MultiLanguage<string> {En = "Book by SEPTEMBER 30, 2019 receive a 15% discount"}),

                            (20, new DateTime(2019, 9, 30), new DateTime(2019, 12, 27), new DateTime(2020, 1, 4), "WWHL613",
                                new MultiLanguage<string> {En = "Book by SEPTEMBER 30, 2019 receive a 20% discount"}),

                            (20, new DateTime(2019, 9, 30), new DateTime(2020, 1, 5), new DateTime(2020, 1, 13), "WWHL614",
                                new MultiLanguage<string> {En = "Book by SEPTEMBER 30, 2019 receive a 20% discount"})
                        };
                FillDiscountRates(dbContext, new[] {71, 72, 73, 74}, discounts);

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

                #region RoomDetails

                dbContext.RoomDetails.AddRange(
                    new RoomDetails
                    {
                        RoomId = 71,
                        AdultsNumber = 1,
                        ChildrenNumber = 0,
                        InfantsNumber = 0
                    },
                    new RoomDetails
                    {
                        RoomId = 71,
                        AdultsNumber = 2,
                        ChildrenNumber = 0,
                        InfantsNumber = 0
                    },
                    new RoomDetails
                    {
                        RoomId = 71,
                        AdultsNumber = 1,
                        ChildrenNumber = 1,
                        InfantsNumber = 0,
                        ChildrenAges = new NpgsqlRange<int>(4, true, 11, true)
                    },
                    new RoomDetails
                    {
                        RoomId = 71,
                        AdultsNumber = 2,
                        ChildrenNumber = 1,
                        InfantsNumber = 0,
                        ChildrenAges = new NpgsqlRange<int>(4, true, 11, true)
                    },
                    new RoomDetails
                    {
                        RoomId = 71,
                        AdultsNumber = 3,
                        ChildrenNumber = 0,
                        InfantsNumber = 0
                    },
                    new RoomDetails
                    {
                        RoomId = 71,
                        AdultsNumber = 1,
                        ChildrenNumber = 2,
                        InfantsNumber = 0,
                        ChildrenAges = new NpgsqlRange<int>(4, true, 11, true)
                    },
                    new RoomDetails
                    {
                        RoomId = 71,
                        AdultsNumber = 3,
                        ChildrenNumber = 1,
                        InfantsNumber = 0,
                        ChildrenAges = new NpgsqlRange<int>(4, true, 11, true)
                    },
                    new RoomDetails
                    {
                        RoomId = 71,
                        AdultsNumber = 2,
                        ChildrenNumber = 2,
                        InfantsNumber = 0,
                        ChildrenAges = new NpgsqlRange<int>(4, true, 11, true)
                    },
                    new RoomDetails
                    {
                        RoomId = 72,
                        AdultsNumber = 1,
                        ChildrenNumber = 0,
                        InfantsNumber = 0
                    },
                    new RoomDetails
                    {
                        RoomId = 72,
                        AdultsNumber = 2,
                        ChildrenNumber = 0,
                        InfantsNumber = 0
                    },
                    new RoomDetails
                    {
                        RoomId = 72,
                        AdultsNumber = 1,
                        ChildrenNumber = 1,
                        InfantsNumber = 0,
                        ChildrenAges = new NpgsqlRange<int>(4, true, 11, true)
                    },
                    new RoomDetails
                    {
                        RoomId = 72,
                        AdultsNumber = 3,
                        ChildrenNumber = 0,
                        InfantsNumber = 0
                    },
                    new RoomDetails
                    {
                        RoomId = 72,
                        AdultsNumber = 1,
                        ChildrenNumber = 2,
                        InfantsNumber = 0,
                        ChildrenAges = new NpgsqlRange<int>(4, true, 11, true)
                    },
                    new RoomDetails
                    {
                        RoomId = 72,
                        AdultsNumber = 3,
                        ChildrenNumber = 1,
                        InfantsNumber = 0,
                        ChildrenAges = new NpgsqlRange<int>(4, true, 11, true)
                    },
                    new RoomDetails
                    {
                        RoomId = 72,
                        AdultsNumber = 2,
                        ChildrenNumber = 2,
                        InfantsNumber = 0,
                        ChildrenAges = new NpgsqlRange<int>(4, true, 11, true)
                    },
                    new RoomDetails
                    {
                        RoomId = 73,
                        AdultsNumber = 1,
                        ChildrenNumber = 0,
                        InfantsNumber = 0
                    },
                    new RoomDetails
                    {
                        RoomId = 73,
                        AdultsNumber = 2,
                        ChildrenNumber = 0,
                        InfantsNumber = 0
                    },
                    new RoomDetails
                    {
                        RoomId = 73,
                        AdultsNumber = 1,
                        ChildrenNumber = 1,
                        InfantsNumber = 0,
                        ChildrenAges = new NpgsqlRange<int>(4, true, 11, true)
                    },
                    new RoomDetails
                    {
                        RoomId = 73,
                        AdultsNumber = 2,
                        ChildrenNumber = 1,
                        InfantsNumber = 0,
                        ChildrenAges = new NpgsqlRange<int>(4, true, 11, true)
                    },
                    new RoomDetails
                    {
                        RoomId = 73,
                        AdultsNumber = 3,
                        ChildrenNumber = 0,
                        InfantsNumber = 0
                    },
                    new RoomDetails
                    {
                        RoomId = 73,
                        AdultsNumber = 1,
                        ChildrenNumber = 2,
                        InfantsNumber = 0,
                        ChildrenAges = new NpgsqlRange<int>(4, true, 11, true)
                    },
                    new RoomDetails
                    {
                        RoomId = 73,
                        AdultsNumber = 4,
                        ChildrenNumber = 0,
                        InfantsNumber = 0
                    },
                    new RoomDetails
                    {
                        RoomId = 73,
                        AdultsNumber = 3,
                        ChildrenNumber = 1,
                        InfantsNumber = 0,
                        ChildrenAges = new NpgsqlRange<int>(4, true, 11, true)
                    },
                    new RoomDetails
                    {
                        RoomId = 73,
                        AdultsNumber = 2,
                        ChildrenNumber = 2,
                        InfantsNumber = 0,
                        ChildrenAges = new NpgsqlRange<int>(4, true, 11, true)
                    },
                    new RoomDetails
                    {
                        RoomId = 73,
                        AdultsNumber = 1,
                        ChildrenNumber = 3,
                        InfantsNumber = 0,
                        ChildrenAges = new NpgsqlRange<int>(4, true, 11, true)
                    },
                    new RoomDetails
                    {
                        RoomId = 73,
                        AdultsNumber = 5,
                        ChildrenNumber = 0,
                        InfantsNumber = 0
                    },
                    new RoomDetails
                    {
                        RoomId = 73,
                        AdultsNumber = 4,
                        ChildrenNumber = 1,
                        InfantsNumber = 0,
                        ChildrenAges = new NpgsqlRange<int>(4, true, 11, true)
                    },
                    new RoomDetails
                    {
                        RoomId = 73,
                        AdultsNumber = 3,
                        ChildrenNumber = 2,
                        InfantsNumber = 0,
                        ChildrenAges = new NpgsqlRange<int>(4, true, 11, true)
                    },
                    new RoomDetails
                    {
                        RoomId = 73,
                        AdultsNumber = 2,
                        ChildrenNumber = 3,
                        InfantsNumber = 0,
                        ChildrenAges = new NpgsqlRange<int>(4, true, 11, true)
                    },
                    new RoomDetails
                    {
                        RoomId = 73,
                        AdultsNumber = 1,
                        ChildrenNumber = 4,
                        InfantsNumber = 0,
                        ChildrenAges = new NpgsqlRange<int>(4, true, 11, true)
                    },
                    new RoomDetails
                    {
                        RoomId = 73,
                        AdultsNumber = 5,
                        ChildrenNumber = 1,
                        InfantsNumber = 0,
                        ChildrenAges = new NpgsqlRange<int>(4, true, 11, true)
                    },
                    new RoomDetails
                    {
                        RoomId = 73,
                        AdultsNumber = 4,
                        ChildrenNumber = 2,
                        InfantsNumber = 0,
                        ChildrenAges = new NpgsqlRange<int>(4, true, 11, true)
                    },
                    new RoomDetails
                    {
                        RoomId = 73,
                        AdultsNumber = 3,
                        ChildrenNumber = 3,
                        InfantsNumber = 0,
                        ChildrenAges = new NpgsqlRange<int>(4, true, 11, true)
                    },
                    new RoomDetails
                    {
                        RoomId = 73,
                        AdultsNumber = 2,
                        ChildrenNumber = 4,
                        InfantsNumber = 0,
                        ChildrenAges = new NpgsqlRange<int>(4, true, 11, true)
                    },
                    new RoomDetails
                    {
                        RoomId = 73,
                        AdultsNumber = 1,
                        ChildrenNumber = 5,
                        InfantsNumber = 0,
                        ChildrenAges = new NpgsqlRange<int>(4, true, 11, true)
                    },
                    new RoomDetails
                    {
                        RoomId = 73,
                        AdultsNumber = 1,
                        ChildrenNumber = 5,
                        InfantsNumber = 0,
                        ChildrenAges = new NpgsqlRange<int>(4, true, 11, true)
                    },
                    new RoomDetails
                    {
                        RoomId = 74,
                        AdultsNumber = 1,
                        ChildrenNumber = 0,
                        InfantsNumber = 0,
                        ChildrenAges = new NpgsqlRange<int>(4, true, 11, true)
                    },
                    new RoomDetails
                    {
                        RoomId = 74,
                        AdultsNumber = 2,
                        ChildrenNumber = 0,
                        InfantsNumber = 0
                    },
                    new RoomDetails
                    {
                        RoomId = 74,
                        AdultsNumber = 1,
                        ChildrenNumber = 1,
                        InfantsNumber = 0,
                        ChildrenAges = new NpgsqlRange<int>(4, true, 11, true)
                    },
                    new RoomDetails
                    {
                        RoomId = 74,
                        AdultsNumber = 2,
                        ChildrenNumber = 1,
                        InfantsNumber = 0,
                        ChildrenAges = new NpgsqlRange<int>(4, true, 11, true)
                    },
                    new RoomDetails
                    {
                        RoomId = 74,
                        AdultsNumber = 3,
                        ChildrenNumber = 0,
                        InfantsNumber = 0
                    },
                    new RoomDetails
                    {
                        RoomId = 74,
                        AdultsNumber = 1,
                        ChildrenNumber = 2,
                        InfantsNumber = 0,
                        ChildrenAges = new NpgsqlRange<int>(4, true, 11, true)
                    },
                    new RoomDetails
                    {
                        RoomId = 74,
                        AdultsNumber = 4,
                        ChildrenNumber = 0,
                        InfantsNumber = 0
                    },
                    new RoomDetails
                    {
                        RoomId = 74,
                        AdultsNumber = 3,
                        ChildrenNumber = 1,
                        InfantsNumber = 0,
                        ChildrenAges = new NpgsqlRange<int>(4, true, 11, true)
                    },
                    new RoomDetails
                    {
                        RoomId = 74,
                        AdultsNumber = 2,
                        ChildrenNumber = 2,
                        InfantsNumber = 0,
                        ChildrenAges = new NpgsqlRange<int>(4, true, 11, true)
                    },
                    new RoomDetails
                    {
                        RoomId = 74,
                        AdultsNumber = 1,
                        ChildrenNumber = 3,
                        InfantsNumber = 0,
                        ChildrenAges = new NpgsqlRange<int>(4, true, 11, true)
                    },
                    new RoomDetails
                    {
                        RoomId = 74,
                        AdultsNumber = 5,
                        ChildrenNumber = 0,
                        InfantsNumber = 0
                    },
                    new RoomDetails
                    {
                        RoomId = 74,
                        AdultsNumber = 4,
                        ChildrenNumber = 1,
                        InfantsNumber = 0,
                        ChildrenAges = new NpgsqlRange<int>(4, true, 11, true)
                    },
                    new RoomDetails
                    {
                        RoomId = 74,
                        AdultsNumber = 3,
                        ChildrenNumber = 2,
                        InfantsNumber = 0,
                        ChildrenAges = new NpgsqlRange<int>(4, true, 11, true)
                    },
                    new RoomDetails
                    {
                        RoomId = 74,
                        AdultsNumber = 2,
                        ChildrenNumber = 3,
                        InfantsNumber = 0,
                        ChildrenAges = new NpgsqlRange<int>(4, true, 11, true)
                    },
                    new RoomDetails
                    {
                        RoomId = 74,
                        AdultsNumber = 1,
                        ChildrenNumber = 4,
                        InfantsNumber = 0,
                        ChildrenAges = new NpgsqlRange<int>(4, true, 11, true)
                    },
                    new RoomDetails
                    {
                        RoomId = 74,
                        AdultsNumber = 6,
                        ChildrenNumber = 0,
                        InfantsNumber = 0
                    },
                    new RoomDetails
                    {
                        RoomId = 74,
                        AdultsNumber = 5,
                        ChildrenNumber = 1,
                        ChildrenAges = new NpgsqlRange<int>(4, true, 11, true)
                    },
                    new RoomDetails
                    {
                        RoomId = 74,
                        AdultsNumber = 4,
                        ChildrenNumber = 2,
                        InfantsNumber = 0,
                        ChildrenAges = new NpgsqlRange<int>(4, true, 11, true)
                    },
                    new RoomDetails
                    {
                        RoomId = 74,
                        AdultsNumber = 3,
                        ChildrenNumber = 3,
                        InfantsNumber = 0,
                        ChildrenAges = new NpgsqlRange<int>(4, true, 11, true)
                    },
                    new RoomDetails
                    {
                        RoomId = 74,
                        AdultsNumber = 2,
                        ChildrenNumber = 4,
                        InfantsNumber = 0,
                        ChildrenAges = new NpgsqlRange<int>(4, true, 11, true)
                    },
                    new RoomDetails
                    {
                        RoomId = 74,
                        AdultsNumber = 1,
                        ChildrenNumber = 5,
                        InfantsNumber = 0,
                        ChildrenAges = new NpgsqlRange<int>(4, true, 11, true)
                    },
                    new RoomDetails
                    {
                        RoomId = 74,
                        AdultsNumber = 7,
                        ChildrenNumber = 0,
                        InfantsNumber = 0
                    },
                    new RoomDetails
                    {
                        RoomId = 74,
                        AdultsNumber = 6,
                        ChildrenNumber = 1,
                        InfantsNumber = 0,
                        ChildrenAges = new NpgsqlRange<int>(4, true, 11, true)
                    },
                    new RoomDetails
                    {
                        RoomId = 74,
                        AdultsNumber = 5,
                        ChildrenNumber = 2,
                        InfantsNumber = 0,
                        ChildrenAges = new NpgsqlRange<int>(4, true, 11, true)
                    },
                    new RoomDetails
                    {
                        RoomId = 74,
                        AdultsNumber = 4,
                        ChildrenNumber = 3,
                        InfantsNumber = 0,
                        ChildrenAges = new NpgsqlRange<int>(4, true, 11, true)
                    },
                    new RoomDetails
                    {
                        RoomId = 74,
                        AdultsNumber = 3,
                        ChildrenNumber = 4,
                        InfantsNumber = 0,
                        ChildrenAges = new NpgsqlRange<int>(4, true, 11, true)
                    },
                    new RoomDetails
                    {
                        RoomId = 74,
                        AdultsNumber = 2,
                        ChildrenNumber = 5,
                        InfantsNumber = 0,
                        ChildrenAges = new NpgsqlRange<int>(4, true, 11, true)
                    },
                    new RoomDetails
                    {
                        RoomId = 74,
                        AdultsNumber = 1,
                        ChildrenNumber = 6,
                        InfantsNumber = 0,
                        ChildrenAges = new NpgsqlRange<int>(4, true, 11, true)
                    });

                #endregion region

                dbContext.SaveChanges();
            }
        }


        private static void AddOneAndOnlyTestContract(DirectContractsDbContext dbContext)
        {
            var accommodation = dbContext.Accommodations.FirstOrDefault(a => a.Name.En.Equals("Test ROYAL MIRAGE"));
            if (accommodation == null)
            {
                var hotelId = 3;

                #region AddTestLocation

                dbContext.Localities.Add(new Locality {CountryCode = "IE", Id = 2, Name = new MultiLanguage<string> {En = "Test"}});

                #endregion

                #region AddAccommodation

                dbContext.Accommodations.Add(
                    new Accommodation
                    {
                        Id = hotelId,
                        Rating = AccommodationRatings.FiveStars,
                        PropertyType = PropertyTypes.Hotels,
                        Name = new MultiLanguage<string>
                        {
                            Ar = "ون آند اونلي رويال Test",
                            En = "Test ROYAL MIRAGE",
                            Ru = "Test ROYAL MIRAGE"
                        },
                        Contacts = new Contacts
                        {
                            Email = "Test@oneandonlythepalm.com",
                            Phone = "+ 933 4 440 1010"
                        },
                        Schedule = new Schedule
                        {
                            CheckInTime = "15:00",
                            CheckOutTime = "10:00",
                            PortersStartTime = "11:00",
                            PortersEndTime = "16:00"
                        },
                        TextualDescription = new TextualDescription
                        {
                            Description = new MultiLanguage<string>
                            {
                                Ar =
                                    "65 فدان من الحدائق الغناء وكيلومتر من الشواطئ الخاصة تمثل أنموذجاً للسلاموالرخاء منقطعا النظير. Test",
                                En =
                                    "Test Set in 65 acres of lush gardens and a kilometer of private beach, peaceful lives in remarkable opulence.",
                                Ru =
                                    " Test26 гектаров ландшафтных садов, собственный пляж протяженностью в один километр, умиротворенная обстановка и роскошное окружение."
                            }
                        },
                        Amenities = new List<MultiLanguage<string>>
                        {
                            new MultiLanguage<string>
                            {
                                Ar =
                                    @"يعتبر تناول الطعام في ون آند أونلي رويال ميراج متعة بحد ذاتها بفضل ثمانية مطاعم مختلفة في ذا بالاس والردهة العربية والإقامة والسبا. ويقدم كل منها تجربة فريدة لاكتشاف أشهى المأكولات في أجواء مميزة ومبتكرة. ويوفر ذا بالاس أربعة مطاعم مختلفة؛ حيث تُقدَّم القوائم المتوسطية في أوليفز، والمأكولات المغربية الاستثنائية في طاجين، والمأكولات العالمية الكلاسيكية في مطعم المشاهير، والسلطات الطازجة وثمار البحر في بار ومطعم الشاطئ للمشويات. ويمكن للضيوف اكتشاف أشهى النكهات الهندو-أوروبية في نينا، أو تذوق أصنافهم المفضلة طوال اليوم من أطباق الشرق الأوسط وشمال أوروبا في ذا روتيسيري، أو تناول الطعام في مجلس عائم مطل على الخليج العربي في مطعم أوزون الذي يقدم المأكولات العالمية بلمسة آسيوية. وتعرض قاعة الطعام قوائم ملهمة ومبتكرة حصريًا لضيوف المساكن والسبا. Test",
                                En =
                                    @"Test Dining at One&Only Royal Mirage is a unique journey within eight different restaurants between The Palace, Arabian Court and Residence & Spa. Each venue presents an authentic culinary experience in a distinctive environment. The Palace offers four different restaurants, including Mediterranean flavours at Olives, exceptional Moroccan fare at Tagine, an elegant dining on European cuisine at Celebrities, and fresh grilled seafood and bright salads at The Beach Bar & Grill. Within Arabian Court, guests may discover seductive Indo-European flavours at Nina, savour all-day dining on traditional recipes from the Middle East and northern Europe at The Rotisserie, or experience the floating majlis overlooking the Arabian Gulf at Eauzone featuring international cuisine with an Asian twist. Exclusive to guests at Residence & Spa, The Dining Room showcases inspired menus of fresh creativity.",
                                Ru =
                                    @"Test Ужин или обед на курорте One&Only Royal Mirage — это уникальное путешествие, в котором вас ждут восемь ресторанов, расположенных в корпусах The Palace, Arabian Court и Residence & Spa. Каждый из ресторанов дарит гостям уникальный гастрономический опыт в неповторимой обстановке. В корпусе The Palace расположены четыре ресторана, включая ресторан средиземноморской кухни Olives, ресторан марокканской кухни Tagine, ресторан Celebrities с элегантным интерьером и европейской кухней, а также гриль-бар Beach Bar & Grill, где вам предложат блюда из свежих морепродуктов на гриле и великолепные салаты. В корпусе Arabian Court гостей ждут соблазнительные блюда индоевропейской кухни ресторана Nina, а в ресторане The Rotisserie в течение всего дня можно оценить блюда, приготовленные по традиционным рецептам кухни Ближнего Востока и Северной Европы. В ресторане Eauzone можно удобно устроиться в плавающем меджлисе с видом на Персидский залив и заказать блюда международной кухни с азиатскими нотками. Ресторан Dining Room, обслуживающий исключительно гостей корпуса Residence & Spa, отличается творческим подходом к составлению меню и приготовлению блюд."
                            },
                            new MultiLanguage<string>
                            {
                                Ar =
                                    @"تضم دبي 10 ملاعب جولف ضمن مسافة قريبة من ون آند أونلي رويال ميراج، ولا يبعد نادي الإمارات للجولف ونادي مونتغمري إلا بضع دقائق. Test",
                                En =
                                    @"Test Dubai boasts 10 spectacular golf courses within easy reach of One&Only Royal Mirage, with Emirates Golf Club and Montgomerie just minutes away.",
                                Ru =
                                    @"Test В Дубае, недалеко от курорта One&Only Royal Mirage, находится 10 роскошных полей для гольфа, а гольф-клубы Emirates Golf Club и Montgomerie также расположены в непосредственной близости."
                            },
                            new MultiLanguage<string>
                            {
                                Ar =
                                    "ترتقي روحك وتسمو بينما تنغمس في ملذات التجارب الشاملة المصممة لاستعادة العقل والبدن والروح في منتجع ون آند أونلي سبا. ادخل الحمام الشرقي ودع بدنك يرتاح ببطء عبر الحرارة والبخار المتصاعد بينما تبث أصوات الماء المترقرقة الاسترخاء في العقل.Test",
                                En =
                                    @"Test Spirits elevate as you indulge in a range of holistic experiences designed to restore mind, body and soul at One&Only Spa. At the Traditional Oriental Hammam, let the body ease slowly into the rising heat and steam whilst the sounds of rippling water relaxes the mind.",
                                Ru =
                                    @"Test Процедуры для восстановления равновесия разума, тела и души в спа-центре One&Only обновят вашу жизненную энергию. Позвольте теплым парам традиционного восточного хаммама расслабить ваше тело, пока струящаяся вода успокаивает мысли."
                            }
                        },
                        Picture = new Picture
                        {
                            Caption = new MultiLanguage<string>
                            {
                                Ar = "ون آند اونلي رويال ميراجTest",
                                En = "Test ROYAL MIRAGE"
                            },
                            Source = "https://encrypted-tbn0.gstatic.com/images?q=tbn%3AANd9GcQ6HkA4WClkmu3oOM0iuENG66UC6iKZNUrefe0iJ__MX5ZbValF"
                        }
                    });

                #endregion

                #region AddLocation

                dbContext.Locations.AddRange(new Location
                {
                    Coordinates = new Point(55.151219, 25.091596),
                    AccommodationId = hotelId,
                    Address = new MultiLanguage<string>
                    {
                        Ar = "شارع الملك سلمان بن عبدالعزيز آل سعود - دبيTest",
                        En = "Test King Salman Bin Abdulaziz Al Saud St - Dubai",
                        Ru = "Test King Salman Bin Abdulaziz Al Saud St - Dubai - ОАЭ"
                    },
                    LocalityId = 2
                });

                #endregion

                #region AddCancelationPolicies

                dbContext.CancelationPolicies.Add(new CancelationPolicy
                {
                    Id = 31,
                    CancelationPolicyDetails = new List<CancelationPolicyDetails>
                    {
                        new CancelationPolicyDetails
                        {
                            FromDays = 45,
                            ToDays = 45,
                            Details = new MultiLanguage<string>
                                {En = "Test New Year Peak Period: Full stay of tour operator rates if cancelled within 45 days of arrival"}
                        }
                    }
                });

                dbContext.CancelationPolicies.Add(new CancelationPolicy
                {
                    Id = 32,
                    CancelationPolicyDetails = new List<CancelationPolicyDetails>
                    {
                        new CancelationPolicyDetails
                        {
                            FromDays = 35,
                            ToDays = 35,
                            Details = new MultiLanguage<string>
                                {En = "Test Full stay of tour operator rates if cancelled within 45 days of arrival"}
                        }
                    }
                });

                dbContext.CancelationPolicies.Add(new CancelationPolicy
                {
                    Id = 33,
                    CancelationPolicyDetails = new List<CancelationPolicyDetails>
                    {
                        new CancelationPolicyDetails
                        {
                            FromDays = 28,
                            ToDays = 14,
                            Details = new MultiLanguage<string>
                            {
                                En =
                                    "Test 28-14 days prior to arrival: Cancellation penalty charge of 4 nights (or full stay if less than 4 nights) of full rate applicable"
                            }
                        },
                        new CancelationPolicyDetails
                        {
                            FromDays = 13,
                            ToDays = 7,
                            Details = new MultiLanguage<string>
                            {
                                En =
                                    "Test 13-7 days prior to arrival: Cancellation penalty charge of 7 nights (or full stay if less than 7 nights) of full rate applicable"
                            }
                        },
                        new CancelationPolicyDetails
                        {
                            FromDays = 6,
                            ToDays = 0,
                            Details = new MultiLanguage<string>
                                {En = "Test 6 days-No show: Tour operator will be charged full length of booking"}
                        }
                    }
                });

                dbContext.CancelationPolicies.Add(new CancelationPolicy
                {
                    Id = 34,
                    CancelationPolicyDetails = new List<CancelationPolicyDetails>
                    {
                        new CancelationPolicyDetails
                        {
                            FromDays = 13,
                            ToDays = 7,
                            Details = new MultiLanguage<string>
                            {
                                En =
                                    "Test 13-07 days before guest arrival: Cancellation penalty charge of 3 nights (or full stay if less than 3 nights) of full rate applicable"
                            }
                        },
                        new CancelationPolicyDetails
                        {
                            FromDays = 6,
                            ToDays = 0,
                            Details = new MultiLanguage<string>
                            {
                                En =
                                    "Test 6 days-Ni show: Cancellation penalty charge of 5 nights at full tour operator rate (or full stay if less than 5 nights) of full rate applicable"
                            }
                        }
                    }
                });

                #endregion

                #region AddSeasons

                dbContext.Seasons.AddRange(
                    new Season
                    {
                        Id = 111,
                        AccommodationId = hotelId,
                        Name = "TestHIGH I",
                        StartDate = new DateTime(2020, 1, 8),
                        EndDate = new DateTime(2020, 1, 14),
                        CancelationPolicyId = 23
                    },
                    new Season
                    {
                        Id = 121,
                        AccommodationId = hotelId,
                        Name = "TestHIGH II",
                        StartDate = new DateTime(2020, 1, 15),
                        EndDate = new DateTime(2020, 1, 24),
                        CancelationPolicyId = 23
                    },
                    new Season
                    {
                        Id = 131,
                        AccommodationId = hotelId,
                        Name = "TestHIGH I",
                        StartDate = new DateTime(2020, 1, 25),
                        EndDate = new DateTime(2020, 2, 7),
                        CancelationPolicyId = 23
                    },
                    new Season
                    {
                        Id = 141,
                        AccommodationId = hotelId,
                        Name = "TestPEAK I",
                        StartDate = new DateTime(2020, 2, 8),
                        EndDate = new DateTime(2020, 2, 21),
                        CancelationPolicyId = 23
                    },
                    new Season
                    {
                        Id = 151,
                        AccommodationId = hotelId,
                        Name = "TestHIGH I",
                        StartDate = new DateTime(2020, 2, 22),
                        EndDate = new DateTime(2020, 3, 20),
                        CancelationPolicyId = 23
                    },
                    new Season
                    {
                        Id = 161,
                        AccommodationId = hotelId,
                        Name = "TestPEAK I",
                        StartDate = new DateTime(2020, 3, 21),
                        EndDate = new DateTime(2020, 3, 27),
                        CancelationPolicyId = 23
                    },
                    new Season
                    {
                        Id = 171,
                        AccommodationId = hotelId,
                        Name = "TestPEAK II",
                        StartDate = new DateTime(2020, 3, 28),
                        EndDate = new DateTime(2020, 4, 12),
                        CancelationPolicyId = 22
                    },
                    new Season
                    {
                        Id = 181,
                        AccommodationId = hotelId,
                        Name = "TestPEAK I",
                        StartDate = new DateTime(2020, 4, 13),
                        EndDate = new DateTime(2020, 4, 18),
                        CancelationPolicyId = 23
                    },
                    new Season
                    {
                        Id = 191,
                        AccommodationId = hotelId,
                        Name = "TestHIGH I",
                        StartDate = new DateTime(2020, 4, 19),
                        EndDate = new DateTime(2020, 5, 3),
                        CancelationPolicyId = 23
                    },
                    new Season
                    {
                        Id = 201,
                        AccommodationId = hotelId,
                        Name = "TestSHOULDER",
                        StartDate = new DateTime(2020, 5, 4),
                        EndDate = new DateTime(2020, 5, 31),
                        CancelationPolicyId = 24
                    },
                    new Season
                    {
                        Id = 211,
                        AccommodationId = hotelId,
                        Name = "TestLOW",
                        StartDate = new DateTime(2020, 7, 1),
                        EndDate = new DateTime(2020, 9, 4),
                        CancelationPolicyId = 24
                    },
                    new Season
                    {
                        Id = 221,
                        AccommodationId = hotelId,
                        Name = "TestSHOULDER",
                        StartDate = new DateTime(2020, 9, 5),
                        EndDate = new DateTime(2020, 9, 25),
                        CancelationPolicyId = 24
                    },
                    new Season
                    {
                        Id = 231,
                        AccommodationId = hotelId,
                        Name = "TestHIGH II",
                        StartDate = new DateTime(2020, 9, 26),
                        EndDate = new DateTime(2020, 10, 9),
                        CancelationPolicyId = 23
                    },
                    new Season
                    {
                        Id = 241,
                        AccommodationId = hotelId,
                        Name = "TestHIGH I",
                        StartDate = new DateTime(2020, 10, 10),
                        EndDate = new DateTime(2020, 10, 16),
                        CancelationPolicyId = 23
                    },
                    new Season
                    {
                        Id = 251,
                        AccommodationId = hotelId,
                        Name = "TestPEAK I",
                        StartDate = new DateTime(2020, 10, 17),
                        EndDate = new DateTime(2020, 10, 23),
                        CancelationPolicyId = 23
                    },
                    new Season
                    {
                        Id = 261,
                        AccommodationId = hotelId,
                        Name = "TestPEAK II",
                        StartDate = new DateTime(2020, 10, 24),
                        EndDate = new DateTime(2020, 11, 6),
                        CancelationPolicyId = 22
                    },
                    new Season
                    {
                        Id = 271,
                        AccommodationId = hotelId,
                        Name = "TestHIGH I",
                        StartDate = new DateTime(2020, 11, 7),
                        EndDate = new DateTime(2020, 12, 4),
                        CancelationPolicyId = 23
                    },
                    new Season
                    {
                        Id = 281,
                        AccommodationId = hotelId,
                        Name = "TestHIGH II",
                        StartDate = new DateTime(2020, 12, 5),
                        EndDate = new DateTime(2020, 12, 18),
                        CancelationPolicyId = 23
                    },
                    new Season
                    {
                        Id = 291,
                        AccommodationId = hotelId,
                        Name = "TestHIGH I",
                        StartDate = new DateTime(2020, 12, 19),
                        EndDate = new DateTime(2020, 12, 25),
                        CancelationPolicyId = 23
                    },
                    new Season
                    {
                        Id = 301,
                        AccommodationId = hotelId,
                        Name = "TestFESTIVE",
                        StartDate = new DateTime(2020, 12, 26),
                        EndDate = new DateTime(2021, 1, 3),
                        CancelationPolicyId = 21
                    },
                    new Season
                    {
                        Id = 311,
                        AccommodationId = hotelId,
                        Name = "TestPEAK I",
                        StartDate = new DateTime(2021, 1, 4),
                        EndDate = new DateTime(2021, 1, 10),
                        CancelationPolicyId = 23
                    }, new Season
                    {
                        Id = 321,
                        AccommodationId = hotelId,
                        Name = "TestHIGH I",
                        StartDate = new DateTime(2021, 1, 11),
                        EndDate = new DateTime(2021, 2, 5),
                        CancelationPolicyId = 23
                    }, new Season
                    {
                        Id = 331,
                        AccommodationId = hotelId,
                        Name = "TestPEAK I",
                        StartDate = new DateTime(2021, 2, 6),
                        EndDate = new DateTime(2021, 2, 19),
                        CancelationPolicyId = 23
                    }, new Season
                    {
                        Id = 341,
                        AccommodationId = hotelId,
                        Name = "TestHIGH I",
                        StartDate = new DateTime(2021, 2, 20),
                        EndDate = new DateTime(2021, 3, 19),
                        CancelationPolicyId = 23
                    }, new Season
                    {
                        Id = 351,
                        AccommodationId = hotelId,
                        Name = "TestPEAK I",
                        StartDate = new DateTime(2021, 3, 20),
                        EndDate = new DateTime(2021, 3, 26),
                        CancelationPolicyId = 23
                    }, new Season
                    {
                        Id = 361,
                        AccommodationId = hotelId,
                        Name = "TestPEAK II",
                        StartDate = new DateTime(2021, 3, 27),
                        EndDate = new DateTime(2021, 4, 10),
                        CancelationPolicyId = 22
                    },
                    new Season
                    {
                        Id = 371,
                        AccommodationId = hotelId,
                        Name = "TestHIGH I",
                        StartDate = new DateTime(2021, 4, 11),
                        EndDate = new DateTime(2021, 5, 7),
                        CancelationPolicyId = 23
                    }
                );

                #endregion

                #region AddRooms

                dbContext.Rooms.AddRange(new Room
                {
                    Id = 201,
                    AccommodationId = hotelId,
                    Name = new MultiLanguage<string>
                    {
                        En = "Test Palace Superior Deluxe Room"
                    }
                }, new Room
                {
                    Id = 211,
                    AccommodationId = hotelId,
                    Name = new MultiLanguage<string>
                    {
                        En = "Test Palace Gold Club Room"
                    }
                }, new Room
                {
                    Id = 221,
                    AccommodationId = hotelId,
                    Name = new MultiLanguage<string>
                    {
                        En = "Test Palace One Bedroom Executive Suite"
                    }
                }, new Room
                {
                    Id = 231,
                    AccommodationId = hotelId,
                    Name = new MultiLanguage<string>
                    {
                        En = "Test Palace One Bedroom Golf Club Suite"
                    }
                }, new Room
                {
                    Id = 241,
                    AccommodationId = hotelId,
                    Name = new MultiLanguage<string>
                    {
                        En = "Test Palace Two Bedroom Executive Suite"
                    }
                }, new Room
                {
                    Id = 251,
                    AccommodationId = hotelId,
                    Name = new MultiLanguage<string>
                    {
                        En = "Test Palace Two Bedroom Golf Club Suite"
                    }
                }, new Room
                {
                    Id = 261,
                    AccommodationId = hotelId,
                    Name = new MultiLanguage<string>
                    {
                        En = "Test Palace Two Bedroom Royal Suite"
                    }
                }, new Room
                {
                    Id = 271,
                    AccommodationId = hotelId,
                    Name = new MultiLanguage<string>
                    {
                        En = "Test Arabian Court Delux Room"
                    }
                }, new Room
                {
                    Id = 281,
                    AccommodationId = hotelId,
                    Name = new MultiLanguage<string>
                    {
                        En = "Test Arabian Court Two Deluxe Rooms Family Accommodation"
                    }
                }, new Room
                {
                    Id = 291,
                    AccommodationId = hotelId,
                    Name = new MultiLanguage<string>
                    {
                        En = "Test Arabian Court One Bedroom Executive Suite"
                    }
                }, new Room
                {
                    Id = 301,
                    AccommodationId = hotelId,
                    Name = new MultiLanguage<string>
                    {
                        En = "Test Arabian Court Two Bedroom Executive Suite"
                    }
                }, new Room
                {
                    Id = 311,
                    AccommodationId = hotelId,
                    Name = new MultiLanguage<string>
                    {
                        En = "Test Arabian Court Prince Suite"
                    }
                }, new Room
                {
                    Id = 321,
                    AccommodationId = hotelId,
                    Name = new MultiLanguage<string>
                    {
                        En = "Test Residence Prestige Room"
                    }
                }, new Room
                {
                    Id = 331,
                    AccommodationId = hotelId,
                    Name = new MultiLanguage<string>
                    {
                        En = "Test Residence Junior Room"
                    }
                }, new Room
                {
                    Id = 341,
                    AccommodationId = hotelId,
                    Name = new MultiLanguage<string>
                    {
                        En = "Test Residence Executive Suite"
                    }
                }, new Room
                {
                    Id = 351,
                    AccommodationId = hotelId,
                    Name = new MultiLanguage<string>
                    {
                        En = "Test Residence Garden Beach Villa"
                    }
                });

                #endregion

                #region AddRates

                FillRates(
                    dbContext,
                    new[] {111, 131, 151, 191},
                    new List<(int, decimal)>
                    {
                        (201, 2465),
                        (211, 3150),
                        (221, 5700),
                        (231, 6700),
                        (241, 8130),
                        (251, 9850),
                        (261, 2230),
                        (271, 2465),
                        (281, 4390),
                        (291, 6750),
                        (301, 9180),
                        (311, 9100),
                        (321, 3755),
                        (331, 6090),
                        (341, 8460),
                        (351, 30000)
                    });
                FillRates(
                    dbContext,
                    new[] {121, 231, 281},
                    new List<(int, decimal)>
                    {
                        (201, 2100),
                        (211, 2700),
                        (221, 4800),
                        (231, 5800),
                        (241, 6870),
                        (251, 8500),
                        (261, 20700),
                        (271, 2100),
                        (281, 4000),
                        (291, 5750),
                        (301, 7820),
                        (311, 7475),
                        (321, 3150),
                        (331, 5250),
                        (341, 6900),
                        (351, 29000)
                    });
                FillRates(
                    dbContext,
                    new[] {141, 161, 181, 251},
                    new List<(int, decimal)>
                    {
                        (201, 3170),
                        (211, 3780),
                        (221, 7000),
                        (231, 8450),
                        (241, 10200),
                        (251, 12230),
                        (261, 23100),
                        (271, 3170),
                        (281, 6000),
                        (291, 8450),
                        (301, 11600),
                        (311, 10000),
                        (321, 4300),
                        (331, 7000),
                        (341, 9800),
                        (351, 30000)
                    });
                FillRates(
                    dbContext,
                    new[] {171, 261},
                    new List<(int, decimal)>
                    {
                        (201, 3780),
                        (211, 4590),
                        (221, 8800),
                        (231, 10500),
                        (241, 12600),
                        (251, 15100),
                        (261, 26450),
                        (271, 3780),
                        (281, 7560),
                        (291, 10700),
                        (301, 14500),
                        (311, 12850),
                        (321, 5245),
                        (331, 8780),
                        (341, 11780),
                        (351, 36000)
                    });
                FillRates(
                    dbContext,
                    new[] {201, 221},
                    new List<(int, decimal)>
                    {
                        (201, 1830),
                        (211, 2405),
                        (221, 4200),
                        (231, 5200),
                        (241, 6000),
                        (251, 7600),
                        (261, 16600),
                        (271, 1830),
                        (281, 3650),
                        (291, 5000),
                        (301, 6800),
                        (311, 6500),
                        (321, 2835),
                        (331, 4700),
                        (341, 5400),
                        (351, 24000)
                    });
                FillRates(
                    dbContext,
                    new[] {211},
                    new List<(int, decimal)>
                    {
                        (201, 1560),
                        (211, 2070),
                        (221, 3500),
                        (231, 4200),
                        (241, 5000),
                        (251, 6270),
                        (261, 16600),
                        (271, 1560),
                        (281, 3000),
                        (291, 4175),
                        (301, 5680),
                        (311, 5550),
                        (321, 2300),
                        (331, 3540),
                        (341, 4900),
                        (351, 24000)
                    });
                FillRates(
                    dbContext,
                    new[] {241, 271, 291},
                    new List<(int, decimal)>
                    {
                        (201, 1560),
                        (211, 2070),
                        (221, 3500),
                        (231, 4200),
                        (241, 5000),
                        (251, 6270),
                        (261, 16600),
                        (271, 1560),
                        (281, 3000),
                        (291, 4175),
                        (301, 5680),
                        (311, 5550),
                        (321, 2300),
                        (331, 3540),
                        (341, 4900),
                        (351, 24000)
                    });
                FillRates(
                    dbContext,
                    new[] {301},
                    new List<(int, decimal)>
                    {
                        (201, 4600),
                        (211, 5620),
                        (221, 9350),
                        (231, 11000),
                        (241, 13850),
                        (251, 16620),
                        (261, 28700),
                        (271, 4830),
                        (281, 9200),
                        (291, 11750),
                        (301, 16600),
                        (311, 14100),
                        (321, 6350),
                        (331, 9300),
                        (341, 11750),
                        (351, 38000)
                    });
                FillRates(
                    dbContext,
                    new[] {311, 331, 351},
                    new List<(int, decimal)>
                    {
                        (201, 3170),
                        (211, 3780),
                        (221, 7000),
                        (231, 8450),
                        (241, 10200),
                        (251, 12230),
                        (261, 23100),
                        (271, 3170),
                        (281, 6000),
                        (291, 8450),
                        (301, 11600),
                        (311, 10000),
                        (321, 4300),
                        (331, 7000),
                        (341, 9800),
                        (351, 3000)
                    });
                FillRates(
                    dbContext,
                    new[] {321, 341},
                    new List<(int, decimal)>
                    {
                        (201, 2465),
                        (211, 3150),
                        (221, 5700),
                        (231, 6700),
                        (241, 8130),
                        (251, 9850),
                        (261, 22300),
                        (271, 2465),
                        (281, 4930),
                        (291, 6750),
                        (301, 9180),
                        (311, 9100),
                        (321, 3755),
                        (331, 6090),
                        (341, 8460),
                        (351, 30000)
                    });
                FillRates(
                    dbContext,
                    new[] {361},
                    new List<(int, decimal)>
                    {
                        (201, 3780),
                        (211, 4590),
                        (221, 8800),
                        (231, 10500),
                        (241, 12600),
                        (251, 15100),
                        (261, 26450),
                        (271, 3780),
                        (281, 7560),
                        (291, 10700),
                        (301, 14500),
                        (311, 12850),
                        (321, 5245),
                        (331, 8780),
                        (341, 11780),
                        (351, 36000)
                    });
                FillRates(
                    dbContext,
                    new[] {371},
                    new List<(int, decimal)>
                    {
                        (201, 2465),
                        (211, 3150),
                        (221, 5700),
                        (231, 6700),
                        (241, 8130),
                        (251, 9850),
                        (261, 22300),
                        (271, 2465),
                        (281, 4930),
                        (291, 6750),
                        (301, 9180),
                        (311, 9100),
                        (321, 3755),
                        (331, 6090),
                        (341, 8460),
                        (351, 30000)
                    });

                #endregion

                #region DiscountRates

                //palace
                var offers =
                    new (int discountPct, DateTime bookBy, DateTime validFrom, DateTime validTo, string bookingCode,
                        MultiLanguage<string> details)[]
                        {
                            (25, new DateTime(2019, 12, 13), new DateTime(2020, 1, 8), new DateTime(2020, 2, 21), "25% PROMO",
                                new MultiLanguage<string> {En = "Book on or before Friday, DECEMBER 13, 2019 receive a 25% discount"}),
                            (20, new DateTime(2019, 12, 13), new DateTime(2020, 2, 22), new DateTime(2020, 3, 20), "20% PROMO",
                                new MultiLanguage<string> {En = "Book on or before Friday, DECEMBER 13, 2019 receive a 20% discount"}),
                            (30, new DateTime(2019, 12, 13), new DateTime(2020, 3, 21), new DateTime(2020, 3, 27), "30% PROMO",
                                new MultiLanguage<string> {En = "Book on or before Friday, DECEMBER 13, 2019 receive a 30% discount"}),
                            (20, new DateTime(2020, 2, 28), new DateTime(2020, 3, 28), new DateTime(2020, 4, 3), "20% PROMO",
                                new MultiLanguage<string> {En = "Book on or before Friday, FEBRUARY 28, 2020 receive a 35% discount"}),
                            (20, new DateTime(2020, 2, 28), new DateTime(2020, 4, 4), new DateTime(2020, 4, 12), "20% PROMO",
                                new MultiLanguage<string> {En = "Book on or before Friday, FEBRUARY 28, 2020 receive a 20% discount"}),

                            (30, new DateTime(2020, 2, 28), new DateTime(2020, 4, 13), new DateTime(2020, 4, 18), "30% PROMO",
                                new MultiLanguage<string> {En = "Book on or before Friday, FEBRUARY 28, 2020 receive a 30% discount"}),

                            (25, new DateTime(2020, 2, 28), new DateTime(2020, 4, 19), new DateTime(2020, 5, 3), "25% PROMO",
                                new MultiLanguage<string> {En = "Book on or before Friday, FEBRUARY 28, 2020 receive a 25% discount"}),

                            (30, new DateTime(2020, 4, 3), new DateTime(2020, 5, 4), new DateTime(2020, 5, 31), "30% PROMO",
                                new MultiLanguage<string> {En = "Book on or before Friday, APRIL 28, 2020 receive a 30% discount"}),

                            (30, new DateTime(2020, 5, 8), new DateTime(2020, 6, 1), new DateTime(2020, 9, 4), "30% PROMO",
                                new MultiLanguage<string> {En = "Book on or before Friday, MAY 8, 2020 receive a 30% discount"}),

                            (40, new DateTime(2020, 8, 14), new DateTime(2020, 9, 5), new DateTime(2020, 9, 25), "40% PROMO",
                                new MultiLanguage<string> {En = "Book on or before Friday, AUGUST 14, 2020 receive a 40% discount"}),

                            (25, new DateTime(2020, 8, 14), new DateTime(2020, 9, 26), new DateTime(2020, 10, 16), "25% PROMO",
                                new MultiLanguage<string> {En = "Book on or before Friday, AUGUST 14, 2020 receive a 25% discount"}),

                            (20, new DateTime(2020, 8, 14), new DateTime(2020, 10, 17), new DateTime(2020, 12, 4), "20% PROMO",
                                new MultiLanguage<string> {En = "Book on or before Friday, AUGUST 14, 2020 receive a 20% discount"}),

                            (30, new DateTime(2020, 10, 2), new DateTime(2020, 12, 5), new DateTime(2020, 12, 18), "30% PROMO",
                                new MultiLanguage<string> {En = "Book on or before Friday, OCTOBER 2, 2020 receive a 30% discount"}),

                            (20, new DateTime(2020, 11, 1), new DateTime(2020, 12, 19), new DateTime(2020, 12, 25), "20% PROMO",
                                new MultiLanguage<string> {En = "Book on or before Friday, NOVEMBER 1, 2020 receive a 20% discount"}),

                            (10, new DateTime(2020, 11, 1), new DateTime(2020, 12, 26), new DateTime(2021, 1, 3), "10% PROMO",
                                new MultiLanguage<string> {En = "Book on or before Friday, NOVEMBER 1, 2020 receive a 10% discount"}),

                            (20, new DateTime(2020, 11, 1), new DateTime(2021, 1, 4), new DateTime(2021, 4, 10), "20% PROMO",
                                new MultiLanguage<string> {En = "Book on or before Friday, NOVEMBER 1, 2020 receive a 20% discount"}),

                            (20, new DateTime(2021, 2, 26), new DateTime(2021, 4, 11), new DateTime(2021, 5, 7), "20% PROMO",
                                new MultiLanguage<string> {En = "Book on or before Friday, FEBRUARY 28, 2021 receive a 20% discount"})
                        };
                FillDiscountRates(dbContext, new[] {201, 211, 221, 231, 241, 251, 261}, offers);

                //arabian court
                offers =
                    new (int discountPct, DateTime bookBy, DateTime validFrom, DateTime validTo, string bookingCode,
                        MultiLanguage<string> details)[]
                        {
                            (30, new DateTime(2019, 12, 13), new DateTime(2020, 1, 8), new DateTime(2020, 2, 21), "30% PROMO",
                                new MultiLanguage<string> {En = "Book on or before Friday, DECEMBER 13, 2019 receive a 30% discount"}),
                            (25, new DateTime(2019, 12, 13), new DateTime(2020, 2, 22), new DateTime(2020, 3, 20), "25% PROMO",
                                new MultiLanguage<string> {En = "Book on or before Friday, DECEMBER 13, 2019 receive a 25% discount"}),
                            (35, new DateTime(2019, 12, 13), new DateTime(2020, 3, 21), new DateTime(2020, 3, 27), "35% PROMO",
                                new MultiLanguage<string> {En = "Book on or before Friday, DECEMBER 13, 2019 receive a 35% discount"}),
                            (35, new DateTime(2020, 2, 28), new DateTime(2020, 3, 28), new DateTime(2020, 4, 3), "35% PROMO",
                                new MultiLanguage<string> {En = "Book on or before Friday, FEBRUARY 28, 2020 receive a 35% discount"}),
                            (25, new DateTime(2020, 2, 28), new DateTime(2020, 4, 4), new DateTime(2020, 4, 12), "25% PROMO",
                                new MultiLanguage<string> {En = "Book on or before Friday, FEBRUARY 28, 2020 receive a 25% discount"}),
                            (35, new DateTime(2020, 2, 28), new DateTime(2020, 4, 13), new DateTime(2020, 4, 18), "35% PROMO",
                                new MultiLanguage<string> {En = "Book on or before Friday, FEBRUARY 28, 2020 receive a 35% discount"}),
                            (30, new DateTime(2020, 2, 28), new DateTime(2020, 4, 19), new DateTime(2020, 5, 3), "30% PROMO",
                                new MultiLanguage<string> {En = "Book on or before Friday, FEBRUARY 28, 2020 receive a 30% discount"}),
                            (35, new DateTime(2020, 4, 3), new DateTime(2020, 5, 4), new DateTime(2020, 5, 31), "35% PROMO",
                                new MultiLanguage<string> {En = "Book on or before Friday, APRIL 28, 2020 receive a 35% discount"}),
                            (30, new DateTime(2020, 5, 8), new DateTime(2020, 6, 1), new DateTime(2020, 9, 4), "30% PROMO",
                                new MultiLanguage<string> {En = "Book on or before Friday, MAY 8, 2020 receive a 30% discount"}),
                            (30, new DateTime(2020, 8, 14), new DateTime(2020, 9, 5), new DateTime(2020, 9, 25), "30% PROMO",
                                new MultiLanguage<string> {En = "Book on or before Friday, AUGUST 14, 2020 receive a 30% discount"}),
                            (30, new DateTime(2020, 8, 14), new DateTime(2020, 9, 26), new DateTime(2020, 10, 16), "30% PROMO",
                                new MultiLanguage<string> {En = "Book on or before Friday, AUGUST 14, 2020 receive a 30% discount"}),
                            (20, new DateTime(2020, 8, 14), new DateTime(2020, 10, 17), new DateTime(2020, 12, 4), "20% PROMO",
                                new MultiLanguage<string> {En = "Book on or before Friday, AUGUST 14, 2020 receive a 20% discount"}),
                            (30, new DateTime(2020, 10, 2), new DateTime(2020, 12, 5), new DateTime(2020, 12, 18), "30% PROMO",
                                new MultiLanguage<string> {En = "Book on or before Friday, OCTOBER 2, 2020 receive a 30% discount"}),
                            (20, new DateTime(2020, 11, 1), new DateTime(2020, 12, 19), new DateTime(2020, 12, 25), "20% PROMO",
                                new MultiLanguage<string> {En = "Book on or before Friday, NOVEMBER 1, 2020 receive a 20% discount"}),
                            (10, new DateTime(2020, 11, 1), new DateTime(2020, 12, 26), new DateTime(2021, 1, 3), "10% PROMO",
                                new MultiLanguage<string> {En = "Book on or before Friday, NOVEMBER 1, 2020 receive a 10% discount"}),
                            (20, new DateTime(2020, 11, 1), new DateTime(2021, 1, 4), new DateTime(2021, 3, 10), "20% PROMO",
                                new MultiLanguage<string> {En = "Book on or before Friday, NOVEMBER 1, 2020 receive a 20% discount"}),
                            (20, new DateTime(2021, 2, 26), new DateTime(2021, 3, 11), new DateTime(2021, 5, 7), "20% PROMO",
                                new MultiLanguage<string> {En = "Book on or before Friday, FEBRUARY 26, 2021 receive a 20% discount"})
                        };
                FillDiscountRates(dbContext, new[] {271, 281, 291, 301, 311}, offers);

                //residence and spa
                offers =
                    new (int discountPct, DateTime bookBy, DateTime validFrom, DateTime validTo, string bookingCode,
                        MultiLanguage<string> details)[]
                        {
                            (30, new DateTime(2019, 12, 13), new DateTime(2020, 1, 8), new DateTime(2020, 2, 21), "30% PROMO",
                                new MultiLanguage<string> {En = "Book on or before Friday, DECEMBER 13, 2019 receive a 30% discount"}),
                            (25, new DateTime(2019, 12, 13), new DateTime(2020, 2, 22), new DateTime(2020, 3, 20), "25% PROMO",
                                new MultiLanguage<string> {En = "Book on or before Friday, DECEMBER 13, 2019 receive a 25% discount"}),
                            (35, new DateTime(2019, 12, 13), new DateTime(2020, 3, 21), new DateTime(2020, 3, 27), "35% PROMO",
                                new MultiLanguage<string> {En = "Book on or before Friday, DECEMBER 13, 2019 receive a 35% discount"}),
                            (35, new DateTime(2020, 2, 28), new DateTime(2020, 3, 28), new DateTime(2020, 4, 3), "35% PROMO",
                                new MultiLanguage<string> {En = "Book on or before Friday, FEBRUARY 28, 2020 receive a 35% discount"}),
                            (25, new DateTime(2020, 2, 28), new DateTime(2020, 4, 4), new DateTime(2020, 4, 12), "25% PROMO",
                                new MultiLanguage<string> {En = "Book on or before Friday, FEBRUARY 28, 2020 receive a 25% discount"}),
                            (35, new DateTime(2020, 2, 28), new DateTime(2020, 4, 13), new DateTime(2020, 4, 18), "35% PROMO",
                                new MultiLanguage<string> {En = "Book on or before Friday, FEBRUARY 28, 2020 receive a 35% discount"}),
                            (30, new DateTime(2020, 2, 28), new DateTime(2020, 4, 19), new DateTime(2020, 5, 3), "30% PROMO",
                                new MultiLanguage<string> {En = "Book on or before Friday, FEBRUARY 28, 2020 receive a 30% discount"}),
                            (35, new DateTime(2020, 4, 3), new DateTime(2020, 5, 4), new DateTime(2020, 5, 31), "35% PROMO",
                                new MultiLanguage<string> {En = "Book on or before Friday, APRIL 28, 2020 receive a 35% discount"}),
                            (30, new DateTime(2020, 5, 8), new DateTime(2020, 6, 1), new DateTime(2020, 9, 4), "30% PROMO",
                                new MultiLanguage<string> {En = "Book on or before Friday, MAY 8, 2020 receive a 30% discount"}),
                            (30, new DateTime(2020, 8, 14), new DateTime(2020, 9, 5), new DateTime(2020, 9, 25), "30% PROMO",
                                new MultiLanguage<string> {En = "Book on or before Friday, AUGUST 14, 2020 receive a 30% discount"}),
                            (30, new DateTime(2020, 8, 14), new DateTime(2020, 9, 26), new DateTime(2020, 10, 16), "30% PROMO",
                                new MultiLanguage<string> {En = "Book on or before Friday, AUGUST 14, 2020 receive a 30% discount"}),
                            (20, new DateTime(2020, 8, 14), new DateTime(2020, 10, 17), new DateTime(2020, 12, 4), "20% PROMO",
                                new MultiLanguage<string> {En = "Book on or before Friday, AUGUST 14, 2020 receive a 20% discount"}),
                            (30, new DateTime(2020, 10, 2), new DateTime(2020, 12, 5), new DateTime(2020, 12, 18), "30% PROMO",
                                new MultiLanguage<string> {En = "Book on or before Friday, OCTOBER 2, 2020 receive a 30% discount"}),
                            (20, new DateTime(2020, 11, 1), new DateTime(2020, 12, 19), new DateTime(2020, 12, 25), "20% PROMO",
                                new MultiLanguage<string> {En = "Book on or before Friday, NOVEMBER 1, 2020 receive a 20% discount"}),
                            (10, new DateTime(2020, 11, 1), new DateTime(2020, 12, 26), new DateTime(2021, 1, 3), "10% PROMO",
                                new MultiLanguage<string> {En = "Book on or before Friday, NOVEMBER 1, 2020 receive a 10% discount"}),
                            (20, new DateTime(2020, 11, 1), new DateTime(2021, 1, 4), new DateTime(2021, 3, 10), "20% PROMO",
                                new MultiLanguage<string> {En = "Book on or before Friday, NOVEMBER 1, 2020 receive a 20% discount"}),
                            (20, new DateTime(2021, 2, 26), new DateTime(2021, 3, 11), new DateTime(2021, 5, 7), "20% PROMO",
                                new MultiLanguage<string> {En = "Book on or before Friday, FEBRUARY 28, 2021 receive a 20% discount"})
                        };
                FillDiscountRates(dbContext, new[] {321, 331, 341, 351}, offers);

                #endregion

                #region AddStopSaleDate

                dbContext.StopSaleDates.AddRange(new StopSaleDate
                    {
                        RoomId = 201,
                        StartDate = new DateTime(2020, 4, 1),
                        EndDate = new DateTime(2020, 4, 10)
                    }, new StopSaleDate
                    {
                        RoomId = 201,
                        StartDate = new DateTime(2020, 7, 10),
                        EndDate = new DateTime(2020, 7, 13)
                    }, new StopSaleDate
                    {
                        RoomId = 201,
                        StartDate = new DateTime(2020, 10, 15),
                        EndDate = new DateTime(2020, 10, 18)
                    }, new StopSaleDate
                    {
                        RoomId = 201,
                        StartDate = new DateTime(2021, 1, 10),
                        EndDate = new DateTime(2021, 1, 12)
                    }, new StopSaleDate
                    {
                        RoomId = 211,
                        StartDate = new DateTime(2020, 2, 5),
                        EndDate = new DateTime(2020, 2, 7)
                    }, new StopSaleDate
                    {
                        RoomId = 211,
                        StartDate = new DateTime(2020, 10, 15),
                        EndDate = new DateTime(2020, 10, 18)
                    }, new StopSaleDate
                    {
                        RoomId = 211,
                        StartDate = new DateTime(2020, 12, 28),
                        EndDate = new DateTime(2021, 1, 2)
                    }, new StopSaleDate
                    {
                        RoomId = 221,
                        StartDate = new DateTime(2020, 2, 5),
                        EndDate = new DateTime(2020, 2, 7)
                    }, new StopSaleDate
                    {
                        RoomId = 221,
                        StartDate = new DateTime(2020, 10, 15),
                        EndDate = new DateTime(2020, 10, 18)
                    }, new StopSaleDate
                    {
                        RoomId = 221,
                        StartDate = new DateTime(2020, 12, 28),
                        EndDate = new DateTime(2020, 12, 29)
                    }, new StopSaleDate
                    {
                        RoomId = 231,
                        StartDate = new DateTime(2020, 2, 5),
                        EndDate = new DateTime(2020, 2, 10)
                    }, new StopSaleDate
                    {
                        RoomId = 231,
                        StartDate = new DateTime(2020, 11, 1),
                        EndDate = new DateTime(2020, 11, 3)
                    }, new StopSaleDate
                    {
                        RoomId = 231,
                        StartDate = new DateTime(2020, 8, 7),
                        EndDate = new DateTime(2020, 8, 10)
                    }, new StopSaleDate
                    {
                        RoomId = 241,
                        StartDate = new DateTime(2020, 3, 8),
                        EndDate = new DateTime(2020, 3, 10)
                    }, new StopSaleDate
                    {
                        RoomId = 241,
                        StartDate = new DateTime(2020, 7, 1),
                        EndDate = new DateTime(2020, 7, 3)
                    }, new StopSaleDate
                    {
                        RoomId = 241,
                        StartDate = new DateTime(2020, 9, 3),
                        EndDate = new DateTime(2020, 9, 5)
                    }, new StopSaleDate
                    {
                        RoomId = 251,
                        StartDate = new DateTime(2020, 1, 29),
                        EndDate = new DateTime(2020, 2, 1)
                    }, new StopSaleDate
                    {
                        RoomId = 251,
                        StartDate = new DateTime(2020, 9, 15),
                        EndDate = new DateTime(2020, 9, 16)
                    }, new StopSaleDate
                    {
                        RoomId = 251,
                        StartDate = new DateTime(2020, 12, 30),
                        EndDate = new DateTime(2021, 1, 2)
                    },
                    new StopSaleDate
                    {
                        RoomId = 261,
                        StartDate = new DateTime(2020, 1, 10),
                        EndDate = new DateTime(2020, 1, 12)
                    }, new StopSaleDate
                    {
                        RoomId = 261,
                        StartDate = new DateTime(2020, 3, 4),
                        EndDate = new DateTime(2020, 3, 7)
                    }, new StopSaleDate
                    {
                        RoomId = 261,
                        StartDate = new DateTime(2020, 6, 9),
                        EndDate = new DateTime(2020, 6, 11)
                    }, new StopSaleDate
                    {
                        RoomId = 271,
                        StartDate = new DateTime(2020, 1, 18),
                        EndDate = new DateTime(2020, 1, 19)
                    }, new StopSaleDate
                    {
                        RoomId = 271,
                        StartDate = new DateTime(2020, 2, 27),
                        EndDate = new DateTime(2020, 3, 1)
                    }, new StopSaleDate
                    {
                        RoomId = 271,
                        StartDate = new DateTime(2020, 12, 26),
                        EndDate = new DateTime(2020, 12, 27)
                    }, new StopSaleDate
                    {
                        RoomId = 281,
                        StartDate = new DateTime(2020, 4, 5),
                        EndDate = new DateTime(2020, 4, 7)
                    }, new StopSaleDate
                    {
                        RoomId = 281,
                        StartDate = new DateTime(2020, 11, 3),
                        EndDate = new DateTime(2020, 11, 5)
                    }, new StopSaleDate
                    {
                        RoomId = 281,
                        StartDate = new DateTime(2020, 12, 31),
                        EndDate = new DateTime(2021, 1, 1)
                    }, new StopSaleDate
                    {
                        RoomId = 291,
                        StartDate = new DateTime(2020, 2, 7),
                        EndDate = new DateTime(2020, 2, 10)
                    }, new StopSaleDate
                    {
                        RoomId = 291,
                        StartDate = new DateTime(2020, 4, 3),
                        EndDate = new DateTime(2020, 4, 5)
                    }, new StopSaleDate
                    {
                        RoomId = 291,
                        StartDate = new DateTime(2020, 12, 1),
                        EndDate = new DateTime(2021, 1, 1)
                    }, new StopSaleDate
                    {
                        RoomId = 301,
                        StartDate = new DateTime(2020, 6, 1),
                        EndDate = new DateTime(2020, 9, 1)
                    }, new StopSaleDate
                    {
                        RoomId = 301,
                        StartDate = new DateTime(2020, 9, 2),
                        EndDate = new DateTime(2020, 9, 4)
                    }, new StopSaleDate
                    {
                        RoomId = 301,
                        StartDate = new DateTime(2020, 12, 1),
                        EndDate = new DateTime(2021, 12, 2)
                    }, new StopSaleDate
                    {
                        RoomId = 311,
                        StartDate = new DateTime(2020, 1, 6),
                        EndDate = new DateTime(2020, 1, 8)
                    }, new StopSaleDate
                    {
                        RoomId = 311,
                        StartDate = new DateTime(2020, 2, 14),
                        EndDate = new DateTime(2020, 2, 16)
                    }, new StopSaleDate
                    {
                        RoomId = 311,
                        StartDate = new DateTime(2020, 7, 5),
                        EndDate = new DateTime(2021, 7, 8)
                    }, new StopSaleDate
                    {
                        RoomId = 321,
                        StartDate = new DateTime(2020, 9, 12),
                        EndDate = new DateTime(2020, 9, 15)
                    }, new StopSaleDate
                    {
                        RoomId = 321,
                        StartDate = new DateTime(2020, 10, 2),
                        EndDate = new DateTime(2020, 10, 4)
                    }, new StopSaleDate
                    {
                        RoomId = 321,
                        StartDate = new DateTime(2019, 12, 3),
                        EndDate = new DateTime(2019, 12, 4)
                    }, new StopSaleDate
                    {
                        RoomId = 331,
                        StartDate = new DateTime(2020, 3, 6),
                        EndDate = new DateTime(2020, 3, 8)
                    }, new StopSaleDate
                    {
                        RoomId = 331,
                        StartDate = new DateTime(2020, 4, 10),
                        EndDate = new DateTime(2020, 4, 13)
                    }, new StopSaleDate
                    {
                        RoomId = 331,
                        StartDate = new DateTime(2020, 7, 1),
                        EndDate = new DateTime(2020, 8, 1)
                    },
                    new StopSaleDate
                    {
                        RoomId = 341,
                        StartDate = new DateTime(2020, 3, 3),
                        EndDate = new DateTime(2020, 3, 5)
                    }, new StopSaleDate
                    {
                        RoomId = 341,
                        StartDate = new DateTime(2020, 4, 16),
                        EndDate = new DateTime(2020, 4, 19)
                    }, new StopSaleDate
                    {
                        RoomId = 341,
                        StartDate = new DateTime(2020, 11, 23),
                        EndDate = new DateTime(2020, 11, 26)
                    }, new StopSaleDate
                    {
                        RoomId = 351,
                        StartDate = new DateTime(2020, 5, 1),
                        EndDate = new DateTime(2020, 5, 6)
                    }, new StopSaleDate
                    {
                        RoomId = 351,
                        StartDate = new DateTime(2020, 7, 19),
                        EndDate = new DateTime(2020, 7, 21)
                    }, new StopSaleDate
                    {
                        RoomId = 351,
                        StartDate = new DateTime(2020, 11, 28),
                        EndDate = new DateTime(2020, 12, 3)
                    });

                #endregion

                #region AddPermittedOccupancy

                dbContext.RoomDetails.AddRange(
                    //201
                    new RoomDetails
                    {
                        RoomId = 201,
                        AdultsNumber = 2,
                        ChildrenNumber = 1,
                        ChildrenAges = new NpgsqlRange<int>(12, true, 16, true),
                        InfantsNumber = 0
                    }
                    , new RoomDetails
                    {
                        RoomId = 201,
                        AdultsNumber = 2,
                        ChildrenNumber = 1,
                        ChildrenAges = new NpgsqlRange<int>(4, true, 11, true),
                        InfantsNumber = 1
                    },
                    new RoomDetails
                    {
                        RoomId = 201,
                        AdultsNumber = 1,
                        ChildrenNumber = 0,
                        InfantsNumber = 0
                    },
                    new RoomDetails
                    {
                        RoomId = 201,
                        AdultsNumber = 2,
                        ChildrenNumber = 0,
                        InfantsNumber = 0
                    },
                    //211
                    new RoomDetails
                    {
                        RoomId = 211,
                        AdultsNumber = 2,
                        ChildrenNumber = 1,
                        ChildrenAges = new NpgsqlRange<int>(12, true, 16, true),
                        InfantsNumber = 0
                    },
                    new RoomDetails
                    {
                        RoomId = 211,
                        AdultsNumber = 2,
                        ChildrenNumber = 1,
                        ChildrenAges = new NpgsqlRange<int>(4, true, 11, true),
                        InfantsNumber = 1
                    },
                    new RoomDetails
                    {
                        RoomId = 211,
                        AdultsNumber = 1,
                        ChildrenNumber = 0,
                        InfantsNumber = 0
                    },
                    new RoomDetails
                    {
                        RoomId = 211,
                        AdultsNumber = 2,
                        ChildrenNumber = 0,
                        InfantsNumber = 0
                    }
                    //221
                    , new RoomDetails
                    {
                        RoomId = 221,
                        AdultsNumber = 2,
                        ChildrenNumber = 1,
                        ChildrenAges = new NpgsqlRange<int>(12, true, 16, true),
                        InfantsNumber = 0
                    }, new RoomDetails
                    {
                        RoomId = 221,
                        AdultsNumber = 2,
                        ChildrenNumber = 1,
                        ChildrenAges = new NpgsqlRange<int>(4, true, 11, true),
                        InfantsNumber = 1
                    },
                    new RoomDetails
                    {
                        RoomId = 221,
                        AdultsNumber = 1,
                        ChildrenNumber = 0,
                        InfantsNumber = 0
                    },
                    new RoomDetails
                    {
                        RoomId = 221,
                        AdultsNumber = 2,
                        ChildrenNumber = 0,
                        InfantsNumber = 0
                    },
                    //231
                    new RoomDetails
                    {
                        RoomId = 231,
                        AdultsNumber = 2,
                        ChildrenNumber = 1,
                        ChildrenAges = new NpgsqlRange<int>(12, true, 16, true),
                        InfantsNumber = 0
                    }, new RoomDetails
                    {
                        RoomId = 231,
                        AdultsNumber = 2,
                        ChildrenNumber = 1,
                        ChildrenAges = new NpgsqlRange<int>(4, true, 11, true),
                        InfantsNumber = 1
                    }, new RoomDetails
                    {
                        RoomId = 231,
                        AdultsNumber = 1,
                        ChildrenNumber = 0,
                        InfantsNumber = 0
                    },
                    new RoomDetails
                    {
                        RoomId = 231,
                        AdultsNumber = 2,
                        ChildrenNumber = 0,
                        InfantsNumber = 0
                    },
                    //241
                    new RoomDetails
                    {
                        RoomId = 241,
                        AdultsNumber = 4,
                        ChildrenNumber = 1,
                        ChildrenAges = new NpgsqlRange<int>(4, true, 16, true),
                        InfantsNumber = 0
                    }, new RoomDetails
                    {
                        RoomId = 241,
                        AdultsNumber = 1,
                        ChildrenNumber = 0,
                        InfantsNumber = 0
                    },
                    new RoomDetails
                    {
                        RoomId = 241,
                        AdultsNumber = 2,
                        ChildrenNumber = 0,
                        InfantsNumber = 0
                    }, new RoomDetails
                    {
                        RoomId = 241,
                        AdultsNumber = 3,
                        ChildrenNumber = 0,
                        InfantsNumber = 0
                    },
                    //251
                    new RoomDetails
                    {
                        RoomId = 251,
                        AdultsNumber = 4,
                        ChildrenNumber = 1,
                        ChildrenAges = new NpgsqlRange<int>(4, true, 16, true),
                        InfantsNumber = 0
                    },
                    new RoomDetails
                    {
                        RoomId = 251,
                        AdultsNumber = 1,
                        ChildrenNumber = 0,
                        InfantsNumber = 0
                    },
                    new RoomDetails
                    {
                        RoomId = 251,
                        AdultsNumber = 2,
                        ChildrenNumber = 0,
                        InfantsNumber = 0
                    }, new RoomDetails
                    {
                        RoomId = 251,
                        AdultsNumber = 3,
                        ChildrenNumber = 0,
                        InfantsNumber = 0
                    },
                    //261
                    new RoomDetails
                    {
                        RoomId = 261,
                        AdultsNumber = 4,
                        ChildrenNumber = 1,
                        ChildrenAges = new NpgsqlRange<int>(4, true, 16, true),
                        InfantsNumber = 0
                    }, new RoomDetails
                    {
                        RoomId = 261,
                        AdultsNumber = 1,
                        ChildrenNumber = 0,
                        InfantsNumber = 0
                    },
                    new RoomDetails
                    {
                        RoomId = 261,
                        AdultsNumber = 2,
                        ChildrenNumber = 0,
                        InfantsNumber = 0
                    }, new RoomDetails
                    {
                        RoomId = 261,
                        AdultsNumber = 3,
                        ChildrenNumber = 0,
                        InfantsNumber = 0
                    }
                    //271
                    , new RoomDetails
                    {
                        RoomId = 271,
                        AdultsNumber = 2,
                        ChildrenNumber = 1,
                        ChildrenAges = new NpgsqlRange<int>(12, true, 16, true),
                        InfantsNumber = 0
                    }, new RoomDetails
                    {
                        RoomId = 271,
                        AdultsNumber = 2,
                        ChildrenNumber = 1,
                        ChildrenAges = new NpgsqlRange<int>(4, true, 11, true),
                        InfantsNumber = 0
                    }, new RoomDetails
                    {
                        RoomId = 271,
                        AdultsNumber = 1,
                        ChildrenNumber = 0,
                        InfantsNumber = 0
                    },
                    new RoomDetails
                    {
                        RoomId = 271,
                        AdultsNumber = 2,
                        ChildrenNumber = 0,
                        InfantsNumber = 0
                    },
                    //281
                    new RoomDetails
                    {
                        RoomId = 281,
                        AdultsNumber = 3,
                        ChildrenNumber = 3,
                        ChildrenAges = new NpgsqlRange<int>(4, true, 11, true)
                    }, new RoomDetails
                    {
                        RoomId = 281,
                        AdultsNumber = 1,
                        ChildrenNumber = 0,
                        InfantsNumber = 0
                    },
                    new RoomDetails
                    {
                        RoomId = 281,
                        AdultsNumber = 2,
                        ChildrenNumber = 0,
                        InfantsNumber = 0
                    },
                    new RoomDetails
                    {
                        RoomId = 281,
                        AdultsNumber = 3,
                        ChildrenNumber = 0,
                        InfantsNumber = 0
                    },
                    //291
                    new RoomDetails
                    {
                        RoomId = 291,
                        AdultsNumber = 2,
                        ChildrenNumber = 1,
                        ChildrenAges = new NpgsqlRange<int>(12, true, 16, true),
                        InfantsNumber = 0
                    }, new RoomDetails
                    {
                        RoomId = 291,
                        AdultsNumber = 2,
                        ChildrenNumber = 1,
                        ChildrenAges = new NpgsqlRange<int>(4, true, 11, true),
                        InfantsNumber = 1
                    }, new RoomDetails
                    {
                        RoomId = 291,
                        AdultsNumber = 1,
                        ChildrenNumber = 0,
                        InfantsNumber = 0
                    }, new RoomDetails
                    {
                        RoomId = 291,
                        AdultsNumber = 2,
                        ChildrenNumber = 0,
                        InfantsNumber = 0
                    },
                    //301
                    new RoomDetails
                    {
                        RoomId = 301,
                        AdultsNumber = 4,
                        ChildrenNumber = 1,
                        ChildrenAges = new NpgsqlRange<int>(4, true, 16, true),
                        InfantsNumber = 0
                    },
                    new RoomDetails
                    {
                        RoomId = 301,
                        AdultsNumber = 3,
                        ChildrenNumber = 0,
                        InfantsNumber = 0
                    },
                    new RoomDetails
                    {
                        RoomId = 301,
                        AdultsNumber = 4,
                        ChildrenNumber = 0,
                        InfantsNumber = 0
                    },
                    //31
                    new RoomDetails
                    {
                        RoomId = 311,
                        AdultsNumber = 2,
                        ChildrenNumber = 1,
                        ChildrenAges = new NpgsqlRange<int>(12, true, 16, true),
                        InfantsNumber = 0
                    }, new RoomDetails
                    {
                        RoomId = 311,
                        AdultsNumber = 2,
                        ChildrenNumber = 1,
                        ChildrenAges = new NpgsqlRange<int>(4, true, 11, true),
                        InfantsNumber = 1
                    }, new RoomDetails
                    {
                        RoomId = 311,
                        AdultsNumber = 1,
                        ChildrenNumber = 0,
                        InfantsNumber = 0
                    }, new RoomDetails
                    {
                        RoomId = 311,
                        AdultsNumber = 2,
                        ChildrenNumber = 0,
                        InfantsNumber = 0
                    },
                    //321
                    new RoomDetails
                    {
                        RoomId = 321,
                        AdultsNumber = 4,
                        ChildrenNumber = 1,
                        ChildrenAges = new NpgsqlRange<int>(12, true, 16, true),
                        InfantsNumber = 0
                    }, new RoomDetails
                    {
                        RoomId = 321,
                        AdultsNumber = 4,
                        ChildrenNumber = 1,
                        ChildrenAges = new NpgsqlRange<int>(4, true, 11, true),
                        InfantsNumber = 1
                    },
                    new RoomDetails
                    {
                        RoomId = 321,
                        AdultsNumber = 1,
                        ChildrenNumber = 0,
                        InfantsNumber = 0
                    }, new RoomDetails
                    {
                        RoomId = 321,
                        AdultsNumber = 2,
                        ChildrenNumber = 0,
                        InfantsNumber = 0
                    }
                    //331
                    , new RoomDetails
                    {
                        RoomId = 331,
                        AdultsNumber = 2,
                        ChildrenNumber = 1,
                        ChildrenAges = new NpgsqlRange<int>(12, true, 16, true),
                        InfantsNumber = 0
                    }, new RoomDetails
                    {
                        RoomId = 331,
                        AdultsNumber = 2,
                        ChildrenNumber = 1,
                        ChildrenAges = new NpgsqlRange<int>(4, true, 11, true),
                        InfantsNumber = 1
                    }, new RoomDetails
                    {
                        RoomId = 331,
                        AdultsNumber = 1,
                        ChildrenNumber = 0,
                        InfantsNumber = 0
                    }, new RoomDetails
                    {
                        RoomId = 331,
                        AdultsNumber = 2,
                        ChildrenNumber = 0,
                        InfantsNumber = 0
                    },
                    //341
                    new RoomDetails
                    {
                        RoomId = 341,
                        AdultsNumber = 2,
                        ChildrenNumber = 1,
                        ChildrenAges = new NpgsqlRange<int>(4, true, 11, true),
                        InfantsNumber = 1
                    }, new RoomDetails
                    {
                        RoomId = 341,
                        AdultsNumber = 2,
                        ChildrenNumber = 1,
                        ChildrenAges = new NpgsqlRange<int>(12, true, 16, true),
                        InfantsNumber = 0
                    }, new RoomDetails
                    {
                        RoomId = 341,
                        AdultsNumber = 1,
                        ChildrenNumber = 0,
                        InfantsNumber = 0
                    }, new RoomDetails
                    {
                        RoomId = 341,
                        AdultsNumber = 2,
                        ChildrenNumber = 0,
                        InfantsNumber = 0
                    },
                    //351
                    new RoomDetails
                    {
                        RoomId = 351,
                        AdultsNumber = 4,
                        ChildrenNumber = 1,
                        ChildrenAges = new NpgsqlRange<int>(4, true, 16, true),
                        InfantsNumber = 0
                    }, new RoomDetails
                    {
                        RoomId = 351,
                        AdultsNumber = 1,
                        ChildrenNumber = 0,
                        InfantsNumber = 0
                    }, new RoomDetails
                    {
                        RoomId = 351,
                        AdultsNumber = 2,
                        ChildrenNumber = 0,
                        InfantsNumber = 0
                    }, new RoomDetails
                    {
                        RoomId = 351,
                        AdultsNumber = 3,
                        ChildrenNumber = 0,
                        InfantsNumber = 0
                    }, new RoomDetails
                    {
                        RoomId = 351,
                        AdultsNumber = 4,
                        ChildrenNumber = 0,
                        InfantsNumber = 0
                    }
                );

                #endregion
            }
        }


        private static void FillRates(DirectContractsDbContext dbContext, int[] seasonsIds, List<(int, decimal)> roomIdsAndPrices)
        {
            foreach (var seasonId in seasonsIds)
            foreach (var roomIdsAndPrice in roomIdsAndPrices)
                dbContext.Rates.Add(new ContractedRate
                {
                    SeasonId = seasonId,
                    RoomId = roomIdsAndPrice.Item1,
                    SeasonPrice = roomIdsAndPrice.Item2,
                    CurrencyCode = "AED"
                });
        }


        private static void FillDiscountRates(DirectContractsDbContext dbContext, int[] roomIds,
            (int discountPercent, DateTime bookBy, DateTime validFrom, DateTime validTo, string bookingCode, MultiLanguage<string> details)[] offers)
        {
            foreach (var roomId in roomIds)
            foreach (var offer in offers)
                dbContext.DiscountRates.Add(new DiscountRate
                {
                    RoomId = roomId,
                    DiscountPercent = offer.discountPercent,
                    BookBy = offer.bookBy,
                    ValidFrom = offer.validFrom,
                    ValidTo = offer.validTo,
                    BookingCode = offer.bookingCode,
                    Details = offer.details
                });
        }
    }
}
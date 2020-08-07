using Microsoft.EntityFrameworkCore.Migrations;

namespace HappyTravel.Hiroshima.Data.Migrations
{
    public partial class AddCountries : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql =
                @"INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('AF', '{""ar"": ""أفغانستان"", ""cn"": ""阿富汗"", ""en"": ""Afghanistan"", ""es"": ""Afganistán"", ""fr"": ""Afghanistan"", ""ru"": ""Афганистан""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('AZ', '{""ar"": ""أذربيجان"", ""cn"": ""阿塞拜疆"", ""en"": ""Azerbaijan"", ""es"": ""Azerbaiyán"", ""fr"": ""Azerbaïdjan"", ""ru"": ""Азербайджан""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('IN', '{""ar"": ""الهند"", ""cn"": ""印度"", ""en"": ""India"", ""es"": ""India"", ""fr"": ""Inde"", ""ru"": ""Индия""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('KP', '{""ar"": ""جمهورية كوريا الشعبية الديمقراطية"", ""cn"": ""朝鲜民主主义人民共和国"", ""en"": ""Democratic People''s Republic of Korea"", ""es"": ""República Popular Democrática de Corea"", ""fr"": ""République populaire démocratique de Corée"", ""ru"": ""Корейская Народно-Демократическая Республика""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('KY', '{""ar"": ""جزر كايمان"", ""cn"": ""开曼群岛"", ""en"": ""Cayman Islands"", ""es"": ""Islas Caimán"", ""fr"": ""Îles Caïmanes"", ""ru"": ""Кайман острова""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('KZ', '{""ar"": ""كازاخستان"", ""cn"": ""哈萨克斯坦"", ""en"": ""Kazakhstan"", ""es"": ""Kazajstán"", ""fr"": ""Kazakhstan"", ""ru"": ""Казахстан""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('LA', '{""ar"": ""جمهورية لاو الديمقراطية الشعبية"", ""cn"": ""老挝人民民主共和国"", ""en"": ""Lao People''s Democratic Republic"", ""es"": ""República Democrática Popular Lao"", ""fr"": ""République démocratique populaire lao"", ""ru"": ""Лаосская Народно-Демократическая Республика""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('LC', '{""ar"": ""سانت لوسيا"", ""cn"": ""圣卢西亚"", ""en"": ""Saint Lucia"", ""es"": ""Santa Lucía"", ""fr"": ""Sainte-Lucie"", ""ru"": ""Сент-Люсия""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('LK', '{""ar"": ""سري لانكا"", ""cn"": ""斯里兰卡"", ""en"": ""Sri Lanka"", ""es"": ""Sri Lanka"", ""fr"": ""Sri Lanka"", ""ru"": ""Шри-Ланка""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('MM', '{""ar"": ""ميانمار"", ""cn"": ""缅甸"", ""en"": ""Myanmar"", ""es"": ""Myanmar"", ""fr"": ""Myanmar"", ""ru"": ""Мьянма""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('MV', '{""ar"": ""ملديف"", ""cn"": ""马尔代夫"", ""en"": ""Maldives"", ""es"": ""Maldivas"", ""fr"": ""Maldives"", ""ru"": ""Мальдивские Острова""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('MY', '{""ar"": ""ماليزيا"", ""cn"": ""马来西亚"", ""en"": ""Malaysia"", ""es"": ""Malasia"", ""fr"": ""Malaisie"", ""ru"": ""Малайзия""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('NP', '{""ar"": ""نيبال"", ""cn"": ""尼泊尔"", ""en"": ""Nepal"", ""es"": ""Nepal"", ""fr"": ""Népal"", ""ru"": ""Непал""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('PH', '{""ar"": ""الفلبين"", ""cn"": ""菲律宾"", ""en"": ""Philippines"", ""es"": ""Filipinas"", ""fr"": ""Philippines"", ""ru"": ""Филиппины""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('PK', '{""ar"": ""باكستان"", ""cn"": ""巴基斯坦"", ""en"": ""Pakistan"", ""es"": ""Pakistán"", ""fr"": ""Pakistan"", ""ru"": ""Пакистан""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('SG', '{""ar"": ""سنغافورة"", ""cn"": ""新加坡"", ""en"": ""Singapore"", ""es"": ""Singapur"", ""fr"": ""Singapour"", ""ru"": ""Сингапур""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('TH', '{""ar"": ""تايلند"", ""cn"": ""泰国"", ""en"": ""Thailand"", ""es"": ""Tailandia"", ""fr"": ""Thaïlande"", ""ru"": ""Таиланд""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('TJ', '{""ar"": ""طاجيكستان"", ""cn"": ""塔吉克斯坦"", ""en"": ""Tajikistan"", ""es"": ""Tayikistán"", ""fr"": ""Tadjikistan"", ""ru"": ""Таджикистан""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('TL', '{""ar"": ""تيمور - ليشتي"", ""cn"": ""东帝汶"", ""en"": ""Timor-Leste"", ""es"": ""Timor-Leste"", ""fr"": ""Timor-Leste"", ""ru"": ""Тимор-Лешти""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('TM', '{""ar"": ""تركمانستان"", ""cn"": ""土库曼斯坦"", ""en"": ""Turkmenistan"", ""es"": ""Turkmenistán"", ""fr"": ""Turkménistan"", ""ru"": ""Туркменистан""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('UZ', '{""ar"": ""أوزبكستان"", ""cn"": ""乌兹别克斯坦"", ""en"": ""Uzbekistan"", ""es"": ""Uzbekistán"", ""fr"": ""Ouzbékistan"", ""ru"": ""Узбекистан""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('VN', '{""ar"": ""فييت نام"", ""cn"": ""越南"", ""en"": ""Viet Nam"", ""es"": ""Viet Nam"", ""fr"": ""Viet Nam"", ""ru"": ""Вьетнам""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('EE', '{""ar"": ""إستونيا"", ""cn"": ""爱沙尼亚"", ""en"": ""Estonia"", ""es"": ""Estonia"", ""fr"": ""Estonie"", ""ru"": ""Эстония""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('FO', '{""ar"": ""جزر فايرو"", ""cn"": ""法罗群岛"", ""en"": ""Faroe Islands"", ""es"": ""Islas Feroe"", ""fr"": ""Îles Féroé"", ""ru"": ""Фарерские острова""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('HK', '{""ar"": ""الصين، منطقة هونغ كونغ الإدارية الخاصة"", ""cn"": ""中国香港特别行政区"", ""en"": ""China, Hong Kong Special Administrative Region"", ""es"": ""China, región administrativa especial de Hong Kong"", ""fr"": ""Chine, région administrative spéciale de Hong Kong"", ""ru"": ""Китай, Специальный административный район Гонконг""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('IE', '{""ar"": ""آيرلندا"", ""cn"": ""爱尔兰"", ""en"": ""Ireland"", ""es"": ""Irlanda"", ""fr"": ""Irlande"", ""ru"": ""Ирландия""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('IM', '{""ar"": ""جزيرة مان"", ""cn"": ""马恩岛"", ""en"": ""Isle of Man"", ""es"": ""Isla de Man"", ""fr"": ""Île de Man"", ""ru"": ""Остров Мэн""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('IS', '{""ar"": ""آيسلندا"", ""cn"": ""冰岛"", ""en"": ""Iceland"", ""es"": ""Islandia"", ""fr"": ""Islande"", ""ru"": ""Исландия""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('JO', '{""ar"": ""الأردن"", ""cn"": ""约旦"", ""en"": ""Jordan"", ""es"": ""Jordania"", ""fr"": ""Jordanie"", ""ru"": ""Иордания""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('JP', '{""ar"": ""اليابان"", ""cn"": ""日本"", ""en"": ""Japan"", ""es"": ""Japón"", ""fr"": ""Japon"", ""ru"": ""Япония""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('KR', '{""ar"": ""جمهورية كوريا"", ""cn"": ""大韩民国"", ""en"": ""Republic of Korea"", ""es"": ""República de Corea"", ""fr"": ""République de Corée"", ""ru"": ""Республика Корея""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('KW', '{""ar"": ""الكويت"", ""cn"": ""科威特"", ""en"": ""Kuwait"", ""es"": ""Kuwait"", ""fr"": ""Koweït"", ""ru"": ""Кувейт""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('LB', '{""ar"": ""لبنان"", ""cn"": ""黎巴嫩"", ""en"": ""Lebanon"", ""es"": ""Líbano"", ""fr"": ""Liban"", ""ru"": ""Ливан""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('LT', '{""ar"": ""ليتوانيا"", ""cn"": ""立陶宛"", ""en"": ""Lithuania"", ""es"": ""Lituania"", ""fr"": ""Lituanie"", ""ru"": ""Литва""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('LV', '{""ar"": ""لاتفيا"", ""cn"": ""拉脱维亚"", ""en"": ""Latvia"", ""es"": ""Letonia"", ""fr"": ""Lettonie"", ""ru"": ""Латвия""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('MF', '{""ar"": ""سانت مارتن (الجزء الفرنسي)"", ""cn"": ""圣马丁（法属）"", ""en"": ""Saint Martin (French Part)"", ""es"": ""San Martín (parte francesa)"", ""fr"": ""Saint-Martin (partie française)"", ""ru"": ""Сен-Мартен (французская часть)""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('MN', '{""ar"": ""منغوليا"", ""cn"": ""蒙古"", ""en"": ""Mongolia"", ""es"": ""Mongolia"", ""fr"": ""Mongolie"", ""ru"": ""Монголия""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('BH', '{""ar"": ""البحرين"", ""cn"": ""巴林"", ""en"": ""Bahrain"", ""es"": ""Bahrein"", ""fr"": ""Bahreïn"", ""ru"": ""Бахрейн""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('BT', '{""ar"": ""بوتان"", ""cn"": ""不丹"", ""en"": ""Bhutan"", ""es"": ""Bhután"", ""fr"": ""Bhoutan"", ""ru"": ""Бутан""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('CN', '{""ar"": ""الصين"", ""cn"": ""中国"", ""en"": ""China"", ""es"": ""China"", ""fr"": ""Chine"", ""ru"": ""Китай""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('DK', '{""ar"": ""الدانمرك"", ""cn"": ""丹麦"", ""en"": ""Denmark"", ""es"": ""Dinamarca"", ""fr"": ""Danemark"", ""ru"": ""Дания""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('OM', '{""ar"": ""عمان"", ""cn"": ""阿曼"", ""en"": ""Oman"", ""es"": ""Omán"", ""fr"": ""Oman"", ""ru"": ""Оман""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('MO', '{""ar"": ""الصين، منطقة ماكاو الإدارية الخاصة"", ""cn"": ""中国澳门特别行政区"", ""en"": ""China, Macao Special Administrative Region"", ""es"": ""China, región administrativa especial de Macao"", ""fr"": ""Chine, région administrative spéciale de Macao"", ""ru"": ""Китай, Специальный административный район Макао""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('MQ', '{""ar"": ""مارتينيك"", ""cn"": ""马提尼克"", ""en"": ""Martinique"", ""es"": ""Martinica"", ""fr"": ""Martinique"", ""ru"": ""Мартиника""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('MS', '{""ar"": ""مونتسيرات"", ""cn"": ""蒙特塞拉特"", ""en"": ""Montserrat"", ""es"": ""Montserrat"", ""fr"": ""Montserrat"", ""ru"": ""Монтсеррат""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('NO', '{""ar"": ""النرويج"", ""cn"": ""挪威"", ""en"": ""Norway"", ""es"": ""Noruega"", ""fr"": ""Norvège"", ""ru"": ""Норвегия""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('PR', '{""ar"": ""بورتوريكو"", ""cn"": ""波多黎各"", ""en"": ""Puerto Rico"", ""es"": ""Puerto Rico"", ""fr"": ""Porto Rico"", ""ru"": ""Пуэрто-Рико""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('RO', '{""ar"": ""رومانيا"", ""cn"": ""罗马尼亚"", ""en"": ""Romania"", ""es"": ""Rumania"", ""fr"": ""Roumanie"", ""ru"": ""Румыния""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('SK', '{""ar"": ""سلوفاكيا"", ""cn"": ""斯洛伐克"", ""en"": ""Slovakia"", ""es"": ""Eslovaquia"", ""fr"": ""Slovaquie"", ""ru"": ""Словакия""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('SX', '{""ar"": ""سانت مارتن (الجزء الهولندي)"", ""cn"": ""圣马丁（荷属）"", ""en"": ""Sint Maarten (Dutch part)"", ""es"": ""San Martín (parte Holandesa)"", ""fr"": ""Saint-Martin (partie néerlandaise)"", ""ru"": ""Синт-Мартен (нидерландская часть)""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('TC', '{""ar"": ""جزر تركس وكايكوس"", ""cn"": ""特克斯和凯科斯群岛"", ""en"": ""Turks and Caicos Islands"", ""es"": ""Islas Turcas y Caicos"", ""fr"": ""Îles Turques-et-Caïques"", ""ru"": ""Острова Теркс и Кайкос""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('TT', '{""ar"": ""ترينيداد وتوباغو"", ""cn"": ""特立尼达和多巴哥"", ""en"": ""Trinidad and Tobago"", ""es"": ""Trinidad y Tabago"", ""fr"": ""Trinité-et-Tobago"", ""ru"": ""Тринидад и Тобаго""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('UA', '{""ar"": ""أوكرانيا"", ""cn"": ""乌克兰"", ""en"": ""Ukraine"", ""es"": ""Ucrania"", ""fr"": ""Ukraine"", ""ru"": ""Украина""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('VC', '{""ar"": ""سانت فنسنت وجزر غرينادين"", ""cn"": ""圣文森特和格林纳丁斯"", ""en"": ""Saint Vincent and the Grenadines"", ""es"": ""San Vicente y las Granadinas"", ""fr"": ""Saint-Vincent-et-les Grenadines"", ""ru"": ""Сент-Винсент и Гренадины""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('VG', '{""ar"": ""جزر فرجن البريطانية"", ""cn"": ""英属维尔京群岛"", ""en"": ""British Virgin Islands"", ""es"": ""Islas Vírgenes Británicas"", ""fr"": ""Îles Vierges britanniques"", ""ru"": ""Британские Виргинские острова""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('VI', '{""ar"": ""جزر فرجن التابعة للولايات المتحدة"", ""cn"": ""美属维尔京群岛"", ""en"": ""United States Virgin Islands"", ""es"": ""Islas Vírgenes de los Estados Unidos"", ""fr"": ""Îles Vierges américaines"", ""ru"": ""Виргинские острова Соединенных Штатов""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('AX', '{""ar"": ""جزر ألاند"", ""cn"": ""奥兰群岛"", ""en"": ""Åland Islands"", ""es"": ""Islas Åland"", ""fr"": ""Îles d’Åland"", ""ru"": ""Аландских островов""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('BA', '{""ar"": ""البوسنة والهرسك"", ""cn"": ""波斯尼亚和黑塞哥维那"", ""en"": ""Bosnia and Herzegovina"", ""es"": ""Bosnia y Herzegovina"", ""fr"": ""Bosnie-Herzégovine"", ""ru"": ""Босния и Герцеговина""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('BE', '{""ar"": ""بلجيكا"", ""cn"": ""比利时"", ""en"": ""Belgium"", ""es"": ""Bélgica"", ""fr"": ""Belgique"", ""ru"": ""Бельгия""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('BG', '{""ar"": ""بلغاريا"", ""cn"": ""保加利亚"", ""en"": ""Bulgaria"", ""es"": ""Bulgaria"", ""fr"": ""Bulgarie"", ""ru"": ""Болгария""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('BY', '{""ar"": ""بيلاروس"", ""cn"": ""白俄罗斯"", ""en"": ""Belarus"", ""es"": ""Belarús"", ""fr"": ""Bélarus"", ""ru"": ""Беларусь""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('CH', '{""ar"": ""سويسرا"", ""cn"": ""瑞士"", ""en"": ""Switzerland"", ""es"": ""Suiza"", ""fr"": ""Suisse"", ""ru"": ""Швейцария""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('CU', '{""ar"": ""كوبا"", ""cn"": ""古巴"", ""en"": ""Cuba"", ""es"": ""Cuba"", ""fr"": ""Cuba"", ""ru"": ""Куба""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('CZ', '{""ar"": ""تشيكيا"", ""cn"": ""捷克"", ""en"": ""Czechia"", ""es"": ""Chequia"", ""fr"": ""Tchéquie"", ""ru"": ""Чехия""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('DE', '{""ar"": ""ألمانيا"", ""cn"": ""德国"", ""en"": ""Germany"", ""es"": ""Alemania"", ""fr"": ""Allemagne"", ""ru"": ""Германия""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('ES', '{""ar"": ""إسبانيا"", ""cn"": ""西班牙"", ""en"": ""Spain"", ""es"": ""España"", ""fr"": ""Espagne"", ""ru"": ""Испания""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('FR', '{""ar"": ""فرنسا"", ""cn"": ""法国"", ""en"": ""France"", ""es"": ""Francia"", ""fr"": ""France"", ""ru"": ""Франция""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('GG', '{""ar"": ""غيرنسي"", ""cn"": ""格恩西"", ""en"": ""Guernsey"", ""es"": ""Guernsey"", ""fr"": ""Guernesey"", ""ru"": ""Гернси""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('GI', '{""ar"": ""جبل طارق"", ""cn"": ""直布罗陀"", ""en"": ""Gibraltar"", ""es"": ""Gibraltar"", ""fr"": ""Gibraltar"", ""ru"": ""Гибралтар""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('GR', '{""ar"": ""اليونان"", ""cn"": ""希腊"", ""en"": ""Greece"", ""es"": ""Grecia"", ""fr"": ""Grèce"", ""ru"": ""Греция""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('HR', '{""ar"": ""كرواتيا"", ""cn"": ""克罗地亚"", ""en"": ""Croatia"", ""es"": ""Croacia"", ""fr"": ""Croatie"", ""ru"": ""Хорватия""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('HU', '{""ar"": ""هنغاريا"", ""cn"": ""匈牙利"", ""en"": ""Hungary"", ""es"": ""Hungría"", ""fr"": ""Hongrie"", ""ru"": ""Венгрия""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('IT', '{""ar"": ""إيطاليا"", ""cn"": ""意大利"", ""en"": ""Italy"", ""es"": ""Italia"", ""fr"": ""Italie"", ""ru"": ""Италия""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('JE', '{""ar"": ""جيرسي"", ""cn"": ""泽西"", ""en"": ""Jersey"", ""es"": ""Jersey"", ""fr"": ""Jersey"", ""ru"": ""Джерси""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('LI', '{""ar"": ""ليختنشتاين"", ""cn"": ""列支敦士登"", ""en"": ""Liechtenstein"", ""es"": ""Liechtenstein"", ""fr"": ""Liechtenstein"", ""ru"": ""Лихтенштейн""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('AD', '{""ar"": ""أندورا"", ""cn"": ""安道尔"", ""en"": ""Andorra"", ""es"": ""Andorra"", ""fr"": ""Andorre"", ""ru"": ""Андорра""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('AT', '{""ar"": ""النمسا"", ""cn"": ""奥地利"", ""en"": ""Austria"", ""es"": ""Austria"", ""fr"": ""Autriche"", ""ru"": ""Австрия""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('LU', '{""ar"": ""لكسمبرغ"", ""cn"": ""卢森堡"", ""en"": ""Luxembourg"", ""es"": ""Luxemburgo"", ""fr"": ""Luxembourg"", ""ru"": ""Люксембург""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('MC', '{""ar"": ""موناكو"", ""cn"": ""摩纳哥"", ""en"": ""Monaco"", ""es"": ""Mónaco"", ""fr"": ""Monaco"", ""ru"": ""Монако""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('ME', '{""ar"": ""الجبل الأسود"", ""cn"": ""黑山"", ""en"": ""Montenegro"", ""es"": ""Montenegro"", ""fr"": ""Monténégro"", ""ru"": ""Черногория""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('MK', '{""ar"": ""مقدونيا الشمالية"", ""cn"": ""北马其顿"", ""en"": ""North Macedonia"", ""es"": ""Macedonia del Norte"", ""fr"": ""Macédoine du Nord"", ""ru"": ""Северная Македония""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('MT', '{""ar"": ""مالطة"", ""cn"": ""马耳他"", ""en"": ""Malta"", ""es"": ""Malta"", ""fr"": ""Malte"", ""ru"": ""Мальта""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('NL', '{""ar"": ""هولندا"", ""cn"": ""荷兰"", ""en"": ""Netherlands"", ""es"": ""Países Bajos"", ""fr"": ""Pays-Bas"", ""ru"": ""Нидерланды""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('PL', '{""ar"": ""بولندا"", ""cn"": ""波兰"", ""en"": ""Poland"", ""es"": ""Polonia"", ""fr"": ""Pologne"", ""ru"": ""Польша""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('PS', '{""ar"": ""دولة فلسطين"", ""cn"": ""巴勒斯坦国"", ""en"": ""State of Palestine"", ""es"": ""Estado de Palestina"", ""fr"": ""État de Palestine"", ""ru"": ""Государство Палестина""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('PT', '{""ar"": ""البرتغال"", ""cn"": ""葡萄牙"", ""en"": ""Portugal"", ""es"": ""Portugal"", ""fr"": ""Portugal"", ""ru"": ""Португалия""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('QA', '{""ar"": ""قطر"", ""cn"": ""卡塔尔"", ""en"": ""Qatar"", ""es"": ""Qatar"", ""fr"": ""Qatar"", ""ru"": ""Катар""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('RS', '{""ar"": ""صربيا"", ""cn"": ""塞尔维亚"", ""en"": ""Serbia"", ""es"": ""Serbia"", ""fr"": ""Serbie"", ""ru"": ""Сербия""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('SA', '{""ar"": ""المملكة العربية السعودية"", ""cn"": ""沙特阿拉伯"", ""en"": ""Saudi Arabia"", ""es"": ""Arabia Saudita"", ""fr"": ""Arabie saoudite"", ""ru"": ""Саудовская Аравия""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('SE', '{""ar"": ""السويد"", ""cn"": ""瑞典"", ""en"": ""Sweden"", ""es"": ""Suecia"", ""fr"": ""Suède"", ""ru"": ""Швеция""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('SI', '{""ar"": ""سلوفينيا"", ""cn"": ""斯洛文尼亚"", ""en"": ""Slovenia"", ""es"": ""Eslovenia"", ""fr"": ""Slovénie"", ""ru"": ""Словения""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('SJ', '{""ar"": ""جزيرتي سفالبارد وجان مايِن"", ""cn"": ""斯瓦尔巴群岛和扬马延岛"", ""en"": ""Svalbard and Jan Mayen Islands"", ""es"": ""Islas Svalbard y Jan Mayen"", ""fr"": ""Îles Svalbard-et-Jan Mayen"", ""ru"": ""Острова Свальбард и Ян-Майен""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('SM', '{""ar"": ""سان مارينو"", ""cn"": ""圣马力诺"", ""en"": ""San Marino"", ""es"": ""San Marino"", ""fr"": ""Saint-Marin"", ""ru"": ""Сан-Марино""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('SY', '{""ar"": ""الجمهورية العربية السورية"", ""cn"": ""阿拉伯叙利亚共和国"", ""en"": ""Syrian Arab Republic"", ""es"": ""República Árabe Siria"", ""fr"": ""République arabe syrienne"", ""ru"": ""Сирийская Арабская Республика""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('TR', '{""ar"": ""تركيا"", ""cn"": ""土耳其"", ""en"": ""Turkey"", ""es"": ""Turquía"", ""fr"": ""Turquie"", ""ru"": ""Турция""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('VA', '{""ar"": ""الكرسي الرسولي"", ""cn"": ""教廷"", ""en"": ""Holy See"", ""es"": ""Santa Sede"", ""fr"": ""Saint-Siège"", ""ru"": ""Святой Престол""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('YE', '{""ar"": ""اليمن"", ""cn"": ""也门"", ""en"": ""Yemen"", ""es"": ""Yemen"", ""fr"": ""Yémen"", ""ru"": ""Йемен""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('BW', '{""ar"": ""بوتسوانا"", ""cn"": ""博茨瓦纳"", ""en"": ""Botswana"", ""es"": ""Botswana"", ""fr"": ""Botswana"", ""ru"": ""Ботсвана""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('CF', '{""ar"": ""جمهورية أفريقيا الوسطى"", ""cn"": ""中非共和国"", ""en"": ""Central African Republic"", ""es"": ""República Centroafricana"", ""fr"": ""République centrafricaine"", ""ru"": ""Центральноафриканская Республика""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('CG', '{""ar"": ""الكونغو"", ""cn"": ""刚果"", ""en"": ""Congo"", ""es"": ""Congo"", ""fr"": ""Congo"", ""ru"": ""Конго""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('CI', '{""ar"": ""كوت ديفوار"", ""cn"": ""科特迪瓦"", ""en"": ""Côte d’Ivoire"", ""es"": ""Côte d’Ivoire"", ""fr"": ""Côte d’Ivoire"", ""ru"": ""Кот-д''Ивуар""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('CM', '{""ar"": ""الكاميرون"", ""cn"": ""喀麦隆"", ""en"": ""Cameroon"", ""es"": ""Camerún"", ""fr"": ""Cameroun"", ""ru"": ""Камерун""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('CV', '{""ar"": ""كابو فيردي"", ""cn"": ""佛得角"", ""en"": ""Cabo Verde"", ""es"": ""Cabo Verde"", ""fr"": ""Cabo Verde"", ""ru"": ""Кабо-Верде""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('CW', '{""ar"": ""كوراساو"", ""cn"": ""库拉索"", ""en"": ""Curaçao"", ""es"": ""Curazao"", ""fr"": ""Curaçao"", ""ru"": ""Кюрасао""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('DZ', '{""ar"": ""الجزائر"", ""cn"": ""阿尔及利亚"", ""en"": ""Algeria"", ""es"": ""Argelia"", ""fr"": ""Algérie"", ""ru"": ""Алжир""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('EG', '{""ar"": ""مصر"", ""cn"": ""埃及"", ""en"": ""Egypt"", ""es"": ""Egipto"", ""fr"": ""Égypte"", ""ru"": ""Египет""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('EH', '{""ar"": ""الصحراء الغربية"", ""cn"": ""西撒哈拉"", ""en"": ""Western Sahara"", ""es"": ""Sáhara Occidental"", ""fr"": ""Sahara occidental"", ""ru"": ""Западная Сахара""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('GA', '{""ar"": ""غابون"", ""cn"": ""加蓬"", ""en"": ""Gabon"", ""es"": ""Gabón"", ""fr"": ""Gabon"", ""ru"": ""Габон""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('GH', '{""ar"": ""غانا"", ""cn"": ""加纳"", ""en"": ""Ghana"", ""es"": ""Ghana"", ""fr"": ""Ghana"", ""ru"": ""Гана""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('GM', '{""ar"": ""غامبيا"", ""cn"": ""冈比亚"", ""en"": ""Gambia"", ""es"": ""Gambia"", ""fr"": ""Gambie"", ""ru"": ""Гамбия""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('GN', '{""ar"": ""غينيا"", ""cn"": ""几内亚"", ""en"": ""Guinea"", ""es"": ""Guinea"", ""fr"": ""Guinée"", ""ru"": ""Гвинея""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('GQ', '{""ar"": ""غينيا الاستوائية"", ""cn"": ""赤道几内亚"", ""en"": ""Equatorial Guinea"", ""es"": ""Guinea Ecuatorial"", ""fr"": ""Guinée équatoriale"", ""ru"": ""Экваториальная Гвинея""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('GW', '{""ar"": ""غينيا - بيساو"", ""cn"": ""几内亚比绍"", ""en"": ""Guinea-Bissau"", ""es"": ""Guinea-Bissau"", ""fr"": ""Guinée-Bissau"", ""ru"": ""Гвинея-Бисау""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('LR', '{""ar"": ""ليبريا"", ""cn"": ""利比里亚"", ""en"": ""Liberia"", ""es"": ""Liberia"", ""fr"": ""Libéria"", ""ru"": ""Либерия""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('LS', '{""ar"": ""ليسوتو"", ""cn"": ""莱索托"", ""en"": ""Lesotho"", ""es"": ""Lesotho"", ""fr"": ""Lesotho"", ""ru"": ""Лесото""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('BJ', '{""ar"": ""بنن"", ""cn"": ""贝宁"", ""en"": ""Benin"", ""es"": ""Benin"", ""fr"": ""Bénin"", ""ru"": ""Бенин""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('BS', '{""ar"": ""جزر البهاما"", ""cn"": ""巴哈马"", ""en"": ""Bahamas"", ""es"": ""Bahamas"", ""fr"": ""Bahamas"", ""ru"": ""Багамские Острова""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('LY', '{""ar"": ""ليبيا"", ""cn"": ""利比亚"", ""en"": ""Libya"", ""es"": ""Libia"", ""fr"": ""Libye"", ""ru"": ""Ливия""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('MA', '{""ar"": ""المغرب"", ""cn"": ""摩洛哥"", ""en"": ""Morocco"", ""es"": ""Marruecos"", ""fr"": ""Maroc"", ""ru"": ""Марокко""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('ML', '{""ar"": ""مالي"", ""cn"": ""马里"", ""en"": ""Mali"", ""es"": ""Malí"", ""fr"": ""Mali"", ""ru"": ""Мали""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('MR', '{""ar"": ""موريتانيا"", ""cn"": ""毛里塔尼亚"", ""en"": ""Mauritania"", ""es"": ""Mauritania"", ""fr"": ""Mauritanie"", ""ru"": ""Мавритания""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('MW', '{""ar"": ""ملاوي"", ""cn"": ""马拉维"", ""en"": ""Malawi"", ""es"": ""Malawi"", ""fr"": ""Malawi"", ""ru"": ""Малави""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('NA', '{""ar"": ""ناميبيا"", ""cn"": ""纳米比亚"", ""en"": ""Namibia"", ""es"": ""Namibia"", ""fr"": ""Namibie"", ""ru"": ""Намибия""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('NE', '{""ar"": ""النيجر"", ""cn"": ""尼日尔"", ""en"": ""Niger"", ""es"": ""Níger"", ""fr"": ""Niger"", ""ru"": ""Нигер""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('RE', '{""ar"": ""ريونيون"", ""cn"": ""留尼汪"", ""en"": ""Réunion"", ""es"": ""Reunión"", ""fr"": ""Réunion"", ""ru"": ""Реюньон""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('RW', '{""ar"": ""رواندا"", ""cn"": ""卢旺达"", ""en"": ""Rwanda"", ""es"": ""Rwanda"", ""fr"": ""Rwanda"", ""ru"": ""Руанда""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('SC', '{""ar"": ""سيشيل"", ""cn"": ""塞舌尔"", ""en"": ""Seychelles"", ""es"": ""Seychelles"", ""fr"": ""Seychelles"", ""ru"": ""Сейшельские Острова""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('SD', '{""ar"": ""السودان"", ""cn"": ""苏丹"", ""en"": ""Sudan"", ""es"": ""Sudán"", ""fr"": ""Soudan"", ""ru"": ""Судан""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('SO', '{""ar"": ""الصومال"", ""cn"": ""索马里"", ""en"": ""Somalia"", ""es"": ""Somalia"", ""fr"": ""Somalie"", ""ru"": ""Сомали""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('SS', '{""ar"": ""جنوب السودان"", ""cn"": ""南苏丹"", ""en"": ""South Sudan"", ""es"": ""Sudán del Sur"", ""fr"": ""Soudan du Sud"", ""ru"": ""Южный Судан""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('ST', '{""ar"": ""سان تومي وبرينسيبي"", ""cn"": ""圣多美和普林西比"", ""en"": ""Sao Tome and Principe"", ""es"": ""Santo Tomé y Príncipe"", ""fr"": ""Sao Tomé-et-Principe"", ""ru"": ""Сан-Томе и Принсипи""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('SZ', '{""ar"": ""إسواتيني"", ""cn"": ""斯威士兰"", ""en"": ""Eswatini"", ""es"": ""Eswatini"", ""fr"": ""Eswatini"", ""ru"": ""Эсватини""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('TD', '{""ar"": ""تشاد"", ""cn"": ""乍得"", ""en"": ""Chad"", ""es"": ""Chad"", ""fr"": ""Tchad"", ""ru"": ""Чад""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('TN', '{""ar"": ""تونس"", ""cn"": ""突尼斯"", ""en"": ""Tunisia"", ""es"": ""Túnez"", ""fr"": ""Tunisie"", ""ru"": ""Тунис""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('TZ', '{""ar"": ""جمهورية تنزانيا المتحدة"", ""cn"": ""坦桑尼亚联合共和国"", ""en"": ""United Republic of Tanzania"", ""es"": ""República Unida de Tanzanía"", ""fr"": ""République-Unie de Tanzanie"", ""ru"": ""Объединенная Республика Танзания""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('UG', '{""ar"": ""أوغندا"", ""cn"": ""乌干达"", ""en"": ""Uganda"", ""es"": ""Uganda"", ""fr"": ""Ouganda"", ""ru"": ""Уганда""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('YT', '{""ar"": ""مايوت"", ""cn"": ""马约特"", ""en"": ""Mayotte"", ""es"": ""Mayotte"", ""fr"": ""Mayotte"", ""ru"": ""Остров Майотта""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('ZM', '{""ar"": ""زامبيا"", ""cn"": ""赞比亚"", ""en"": ""Zambia"", ""es"": ""Zambia"", ""fr"": ""Zambie"", ""ru"": ""Замбия""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('ZW', '{""ar"": ""زمبابوي"", ""cn"": ""津巴布韦"", ""en"": ""Zimbabwe"", ""es"": ""Zimbabwe"", ""fr"": ""Zimbabwe"", ""ru"": ""Зимбабве""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('AU', '{""ar"": ""أستراليا"", ""cn"": ""澳大利亚"", ""en"": ""Australia"", ""es"": ""Australia"", ""fr"": ""Australie"", ""ru"": ""Австралия""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('BF', '{""ar"": ""بوركينا فاسو"", ""cn"": ""布基纳法索"", ""en"": ""Burkina Faso"", ""es"": ""Burkina Faso"", ""fr"": ""Burkina Faso"", ""ru"": ""Буркина-Фасо""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('BL', '{""ar"": ""سان بارتليمي"", ""cn"": ""圣巴泰勒米"", ""en"": ""Saint Barthélemy"", ""es"": ""San Barthélemy"", ""fr"": ""Saint-Barthélemy"", ""ru"": ""Сен-Бартелеми""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('CR', '{""ar"": ""كوستاريكا"", ""cn"": ""哥斯达黎加"", ""en"": ""Costa Rica"", ""es"": ""Costa Rica"", ""fr"": ""Costa Rica"", ""ru"": ""Коста-Рика""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('CX', '{""ar"": ""جزيرة عيد الميلاد"", ""cn"": ""圣诞岛"", ""en"": ""Christmas Island"", ""es"": ""Isla Christmas"", ""fr"": ""Île Christmas"", ""ru"": ""остров Рождества""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('DJ', '{""ar"": ""جيبوتي"", ""cn"": ""吉布提"", ""en"": ""Djibouti"", ""es"": ""Djibouti"", ""fr"": ""Djibouti"", ""ru"": ""Джибути""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('ER', '{""ar"": ""إريتريا"", ""cn"": ""厄立特里亚"", ""en"": ""Eritrea"", ""es"": ""Eritrea"", ""fr"": ""Érythrée"", ""ru"": ""Эритрея""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('ET', '{""ar"": ""إثيوبيا"", ""cn"": ""埃塞俄比亚"", ""en"": ""Ethiopia"", ""es"": ""Etiopía"", ""fr"": ""Éthiopie"", ""ru"": ""Эфиопия""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('FK', '{""ar"": ""جزر فوكلاند (مالفيناس)"", ""cn"": ""福克兰群岛（马尔维纳斯）"", ""en"": ""Falkland Islands (Malvinas)"", ""es"": ""Islas Malvinas (Falkland)"", ""fr"": ""Îles Falkland (Malvinas)"", ""ru"": ""Фолклендские (Мальвинские) острова""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('GF', '{""ar"": ""غيانا الفرنسية"", ""cn"": ""法属圭亚那"", ""en"": ""French Guiana"", ""es"": ""Guayana Francesa"", ""fr"": ""Guyane française"", ""ru"": ""Французская Гвиана""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('GS', '{""ar"": ""جورجيا الجنوبية وجزر ساندويتش الجنوبية"", ""cn"": ""南乔治亚岛和南桑德韦奇岛"", ""en"": ""South Georgia and the South Sandwich Islands"", ""es"": ""Georgia del Sur y las Islas Sandwich del Sur"", ""fr"": ""Géorgie du Sud-et-les Îles Sandwich du Sud"", ""ru"": ""Южная Джорджия и Южные Сандвичевы острова""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('GT', '{""ar"": ""غواتيمالا"", ""cn"": ""危地马拉"", ""en"": ""Guatemala"", ""es"": ""Guatemala"", ""fr"": ""Guatemala"", ""ru"": ""Гватемала""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('GY', '{""ar"": ""غيانا"", ""cn"": ""圭亚那"", ""en"": ""Guyana"", ""es"": ""Guyana"", ""fr"": ""Guyana"", ""ru"": ""Гайана""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('HN', '{""ar"": ""هندوراس"", ""cn"": ""洪都拉斯"", ""en"": ""Honduras"", ""es"": ""Honduras"", ""fr"": ""Honduras"", ""ru"": ""Гондурас""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('IO', '{""ar"": ""المحيط الهندي الإقليم البريطاني في"", ""cn"": ""英属印度洋领土"", ""en"": ""British Indian Ocean Territory"", ""es"": ""Territorio Británico del Océano Índico"", ""fr"": ""Territoire britannique de l''océan Indien"", ""ru"": ""Британская территория в Индийском океане""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('AS', '{""ar"": ""ساموا الأمريكية"", ""cn"": ""美属萨摩亚"", ""en"": ""American Samoa"", ""es"": ""Samoa Americana"", ""fr"": ""Samoa américaines"", ""ru"": ""Американское Самоа""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('BZ', '{""ar"": ""بليز"", ""cn"": ""伯利兹"", ""en"": ""Belize"", ""es"": ""Belice"", ""fr"": ""Belize"", ""ru"": ""Белиз""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('KE', '{""ar"": ""كينيا"", ""cn"": ""肯尼亚"", ""en"": ""Kenya"", ""es"": ""Kenya"", ""fr"": ""Kenya"", ""ru"": ""Кения""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('KM', '{""ar"": ""جزر القمر"", ""cn"": ""科摩罗"", ""en"": ""Comoros"", ""es"": ""Comoras"", ""fr"": ""Comores"", ""ru"": ""Коморские Острова""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('MG', '{""ar"": ""مدغشقر"", ""cn"": ""马达加斯加"", ""en"": ""Madagascar"", ""es"": ""Madagascar"", ""fr"": ""Madagascar"", ""ru"": ""Мадагаскар""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('MU', '{""ar"": ""موريشيوس"", ""cn"": ""毛里求斯"", ""en"": ""Mauritius"", ""es"": ""Mauricio"", ""fr"": ""Maurice"", ""ru"": ""Маврикий""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('MX', '{""ar"": ""المكسيك"", ""cn"": ""墨西哥"", ""en"": ""Mexico"", ""es"": ""México"", ""fr"": ""Mexique"", ""ru"": ""Мексика""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('MZ', '{""ar"": ""موزامبيق"", ""cn"": ""莫桑比克"", ""en"": ""Mozambique"", ""es"": ""Mozambique"", ""fr"": ""Mozambique"", ""ru"": ""Мозамбик""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('NG', '{""ar"": ""نيجيريا"", ""cn"": ""尼日利亚"", ""en"": ""Nigeria"", ""es"": ""Nigeria"", ""fr"": ""Nigéria"", ""ru"": ""Нигерия""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('NI', '{""ar"": ""نيكاراغوا"", ""cn"": ""尼加拉瓜"", ""en"": ""Nicaragua"", ""es"": ""Nicaragua"", ""fr"": ""Nicaragua"", ""ru"": ""Никарагуа""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('PA', '{""ar"": ""بنما"", ""cn"": ""巴拿马"", ""en"": ""Panama"", ""es"": ""Panamá"", ""fr"": ""Panama"", ""ru"": ""Панама""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('PE', '{""ar"": ""بيرو"", ""cn"": ""秘鲁"", ""en"": ""Peru"", ""es"": ""Perú"", ""fr"": ""Pérou"", ""ru"": ""Перу""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('PY', '{""ar"": ""باراغواي"", ""cn"": ""巴拉圭"", ""en"": ""Paraguay"", ""es"": ""Paraguay"", ""fr"": ""Paraguay"", ""ru"": ""Парагвай""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('SH', '{""ar"": ""سانت هيلانة"", ""cn"": ""圣赫勒拿"", ""en"": ""Saint Helena"", ""es"": ""Santa Elena"", ""fr"": ""Sainte-Hélène"", ""ru"": ""Остров Святой Елены""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('SL', '{""ar"": ""سيراليون"", ""cn"": ""塞拉利昂"", ""en"": ""Sierra Leone"", ""es"": ""Sierra Leona"", ""fr"": ""Sierra Leone"", ""ru"": ""Сьерра-Леоне""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('SN', '{""ar"": ""السنغال"", ""cn"": ""塞内加尔"", ""en"": ""Senegal"", ""es"": ""Senegal"", ""fr"": ""Sénégal"", ""ru"": ""Сенегал""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('SR', '{""ar"": ""سورينام"", ""cn"": ""苏里南"", ""en"": ""Suriname"", ""es"": ""Suriname"", ""fr"": ""Suriname"", ""ru"": ""Суринам""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('SV', '{""ar"": ""السلفادور"", ""cn"": ""萨尔瓦多"", ""en"": ""El Salvador"", ""es"": ""El Salvador"", ""fr"": ""El Salvador"", ""ru"": ""Сальвадор""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('TF', '{""ar"": ""الأراضي الفرنسية الجنوبية الجنوبية"", ""cn"": ""法属南方领地"", ""en"": ""French Southern Territories"", ""es"": ""Territorio de las Tierras Australes Francesas"", ""fr"": ""Terres australes françaises"", ""ru"": ""Южные земли (французская заморская территория)""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('TG', '{""ar"": ""توغو"", ""cn"": ""多哥"", ""en"": ""Togo"", ""es"": ""Togo"", ""fr"": ""Togo"", ""ru"": ""Того""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('UY', '{""ar"": ""أوروغواي"", ""cn"": ""乌拉圭"", ""en"": ""Uruguay"", ""es"": ""Uruguay"", ""fr"": ""Uruguay"", ""ru"": ""Уругвай""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('VE', '{""ar"": ""فنزويلا (جمهورية - البوليفارية)"", ""cn"": ""委内瑞拉玻利瓦尔共和国"", ""en"": ""Venezuela (Bolivarian Republic of)"", ""es"": ""Venezuela (República Bolivariana de)"", ""fr"": ""Venezuela (République bolivarienne du)"", ""ru"": ""Венесуэла (Боливарианская Республика)""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('BM', '{""ar"": ""برمودا"", ""cn"": ""百慕大"", ""en"": ""Bermuda"", ""es"": ""Bermuda"", ""fr"": ""Bermudes"", ""ru"": ""Бермудские острова""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('BQ', '{""ar"": ""بونير وسانت يوستاشيوس وسابا"", ""cn"": ""博纳尔，圣俄斯塔休斯和萨巴"", ""en"": ""Bonaire, Sint Eustatius and Saba"", ""es"": ""Bonaire, San Eustaquio y Saba"", ""fr"": ""Bonaire, Saint-Eustache et Saba"", ""ru"": ""Бонайре, Синт-Эстатиус и Саба""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('BV', '{""ar"": ""جزيرة بوفيت"", ""cn"": ""布维岛"", ""en"": ""Bouvet Island"", ""es"": ""Isla Bouvet"", ""fr"": ""Île Bouvet"", ""ru"": ""Остров Буве""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('CA', '{""ar"": ""كندا"", ""cn"": ""加拿大"", ""en"": ""Canada"", ""es"": ""Canadá"", ""fr"": ""Canada"", ""ru"": ""Канада""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('CK', '{""ar"": ""جزر كوك"", ""cn"": ""库克群岛"", ""en"": ""Cook Islands"", ""es"": ""Islas Cook"", ""fr"": ""Îles Cook"", ""ru"": ""Острова Кука""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('CL', '{""ar"": ""شيلي"", ""cn"": ""智利"", ""en"": ""Chile"", ""es"": ""Chile"", ""fr"": ""Chili"", ""ru"": ""Чили""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('CO', '{""ar"": ""كولومبيا"", ""cn"": ""哥伦比亚"", ""en"": ""Colombia"", ""es"": ""Colombia"", ""fr"": ""Colombie"", ""ru"": ""Колумбия""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('EC', '{""ar"": ""إكوادور"", ""cn"": ""厄瓜多尔"", ""en"": ""Ecuador"", ""es"": ""Ecuador"", ""fr"": ""Équateur"", ""ru"": ""Эквадор""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('FJ', '{""ar"": ""فيجي"", ""cn"": ""斐济"", ""en"": ""Fiji"", ""es"": ""Fiji"", ""fr"": ""Fidji"", ""ru"": ""Фиджи""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('FM', '{""ar"": ""ميكرونيزيا (ولايات - الموحدة)"", ""cn"": ""密克罗尼西亚联邦"", ""en"": ""Micronesia (Federated States of)"", ""es"": ""Micronesia (Estados Federados de)"", ""fr"": ""Micronésie (États fédérés de)"", ""ru"": ""Микронезия (Федеративные Штаты)""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('GL', '{""ar"": ""غرينلند"", ""cn"": ""格陵兰"", ""en"": ""Greenland"", ""es"": ""Groenlandia"", ""fr"": ""Groenland"", ""ru"": ""Гренландия""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('GU', '{""ar"": ""غوام"", ""cn"": ""关岛"", ""en"": ""Guam"", ""es"": ""Guam"", ""fr"": ""Guam"", ""ru"": ""Гуам""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('HM', '{""ar"": ""جزيرة هيرد وجزر ماكدونالد"", ""cn"": ""赫德岛和麦克唐纳岛"", ""en"": ""Heard Island and McDonald Islands"", ""es"": ""Islas Heard y McDonald"", ""fr"": ""Île Heard-et-Îles MacDonald"", ""ru"": ""Остров Херд и острова Макдональд""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('KI', '{""ar"": ""كيريباس"", ""cn"": ""基里巴斯"", ""en"": ""Kiribati"", ""es"": ""Kiribati"", ""fr"": ""Kiribati"", ""ru"": ""Кирибати""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('MH', '{""ar"": ""جزر مارشال"", ""cn"": ""马绍尔群岛"", ""en"": ""Marshall Islands"", ""es"": ""Islas Marshall"", ""fr"": ""Îles Marshall"", ""ru"": ""Маршалловы Острова""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('MP', '{""ar"": ""جزر ماريانا الشمالية"", ""cn"": ""北马里亚纳群岛"", ""en"": ""Northern Mariana Islands"", ""es"": ""Islas Marianas Septentrionales"", ""fr"": ""Îles Mariannes du Nord"", ""ru"": ""Северные Марианские острова""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('AI', '{""ar"": ""أنغويلا"", ""cn"": ""安圭拉"", ""en"": ""Anguilla"", ""es"": ""Anguila"", ""fr"": ""Anguilla"", ""ru"": ""Ангилья""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('AR', '{""ar"": ""الأرجنتين"", ""cn"": ""阿根廷"", ""en"": ""Argentina"", ""es"": ""Argentina"", ""fr"": ""Argentine"", ""ru"": ""Аргентина""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('AW', '{""ar"": ""أروبا"", ""cn"": ""阿鲁巴"", ""en"": ""Aruba"", ""es"": ""Aruba"", ""fr"": ""Aruba"", ""ru"": ""Аруба""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('NC', '{""ar"": ""كاليدونيا الجديدة"", ""cn"": ""新喀里多尼亚"", ""en"": ""New Caledonia"", ""es"": ""Nueva Caledonia"", ""fr"": ""Nouvelle-Calédonie"", ""ru"": ""Новая Каледония""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('NF', '{""ar"": ""جزيرة نورفولك"", ""cn"": ""诺福克岛"", ""en"": ""Norfolk Island"", ""es"": ""Isla Norfolk"", ""fr"": ""Île Norfolk"", ""ru"": ""Остров Норфолк""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('NR', '{""ar"": ""ناورو"", ""cn"": ""瑙鲁"", ""en"": ""Nauru"", ""es"": ""Nauru"", ""fr"": ""Nauru"", ""ru"": ""Науру""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('NU', '{""ar"": ""نيوي"", ""cn"": ""纽埃"", ""en"": ""Niue"", ""es"": ""Niue"", ""fr"": ""Nioué"", ""ru"": ""Ниуэ""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('NZ', '{""ar"": ""نيوزيلندا"", ""cn"": ""新西兰"", ""en"": ""New Zealand"", ""es"": ""Nueva Zelandia"", ""fr"": ""Nouvelle-Zélande"", ""ru"": ""Новая Зеландия""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('PF', '{""ar"": ""بولينيزيا الفرنسية"", ""cn"": ""法属波利尼西亚"", ""en"": ""French Polynesia"", ""es"": ""Polinesia Francesa"", ""fr"": ""Polynésie française"", ""ru"": ""Французская Полинезия""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('PG', '{""ar"": ""بابوا غينيا الجديدة"", ""cn"": ""巴布亚新几内亚"", ""en"": ""Papua New Guinea"", ""es"": ""Papua Nueva Guinea"", ""fr"": ""Papouasie-Nouvelle-Guinée"", ""ru"": ""Папуа-Новая Гвинея""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('PM', '{""ar"": ""سان بيير وميكلون"", ""cn"": ""圣皮埃尔和密克隆"", ""en"": ""Saint Pierre and Miquelon"", ""es"": ""San Pedro y Miquelón"", ""fr"": ""Saint-Pierre-et-Miquelon"", ""ru"": ""Сен-Пьер и Микелон""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('PN', '{""ar"": ""بيتكرن"", ""cn"": ""皮特凯恩"", ""en"": ""Pitcairn"", ""es"": ""Pitcairn"", ""fr"": ""Pitcairn"", ""ru"": ""Питкэрн""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('PW', '{""ar"": ""بالاو"", ""cn"": ""帕劳"", ""en"": ""Palau"", ""es"": ""Palau"", ""fr"": ""Palaos"", ""ru"": ""Палау""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('SB', '{""ar"": ""جزر سليمان"", ""cn"": ""所罗门群岛"", ""en"": ""Solomon Islands"", ""es"": ""Islas Salomón"", ""fr"": ""Îles Salomon"", ""ru"": ""Соломоновы Острова""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('TO', '{""ar"": ""تونغا"", ""cn"": ""汤加"", ""en"": ""Tonga"", ""es"": ""Tonga"", ""fr"": ""Tonga"", ""ru"": ""Тонга""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('TV', '{""ar"": ""توفالو"", ""cn"": ""图瓦卢"", ""en"": ""Tuvalu"", ""es"": ""Tuvalu"", ""fr"": ""Tuvalu"", ""ru"": ""Тувалу""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('US', '{""ar"": ""الولايات المتحدة الأمريكية"", ""cn"": ""美利坚合众国"", ""en"": ""United States of America"", ""es"": ""Estados Unidos de América"", ""fr"": ""États-Unis d’Amérique"", ""ru"": ""Соединенные Штаты Америки""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('VU', '{""ar"": ""فانواتو"", ""cn"": ""瓦努阿图"", ""en"": ""Vanuatu"", ""es"": ""Vanuatu"", ""fr"": ""Vanuatu"", ""ru"": ""Вануату""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('WF', '{""ar"": ""جزر واليس وفوتونا"", ""cn"": ""瓦利斯群岛和富图纳群岛"", ""en"": ""Wallis and Futuna Islands"", ""es"": ""Islas Wallis y Futuna"", ""fr"": ""Îles Wallis-et-Futuna"", ""ru"": ""Острова Уоллис и Футуна""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('WS', '{""ar"": ""ساموا"", ""cn"": ""萨摩亚"", ""en"": ""Samoa"", ""es"": ""Samoa"", ""fr"": ""Samoa"", ""ru"": ""Самоа""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('AG', '{""ar"": ""أنتيغوا وبربودا"", ""cn"": ""安提瓜和巴布达"", ""en"": ""Antigua and Barbuda"", ""es"": ""Antigua y Barbuda"", ""fr"": ""Antigua-et-Barbuda"", ""ru"": ""Антигуа и Барбуда""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('AM', '{""ar"": ""أرمينيا"", ""cn"": ""亚美尼亚"", ""en"": ""Armenia"", ""es"": ""Armenia"", ""fr"": ""Arménie"", ""ru"": ""Армения""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('AO', '{""ar"": ""أنغولا"", ""cn"": ""安哥拉"", ""en"": ""Angola"", ""es"": ""Angola"", ""fr"": ""Angola"", ""ru"": ""Ангола""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('BB', '{""ar"": ""بربادوس"", ""cn"": ""巴巴多斯"", ""en"": ""Barbados"", ""es"": ""Barbados"", ""fr"": ""Barbade"", ""ru"": ""Барбадос""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('BO', '{""ar"": ""بوليفيا (دولة - المتعددة القوميات)"", ""cn"": ""多民族玻利维亚国"", ""en"": ""Bolivia (Plurinational State of)"", ""es"": ""Bolivia (Estado Plurinacional de)"", ""fr"": ""Bolivie (État plurinational de)"", ""ru"": ""Боливия (Многонациональное Государство)""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('BD', '{""ar"": ""بنغلاديش"", ""cn"": ""孟加拉国"", ""en"": ""Bangladesh"", ""es"": ""Bangladesh"", ""fr"": ""Bangladesh"", ""ru"": ""Бангладеш""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('BI', '{""ar"": ""بوروندي"", ""cn"": ""布隆迪"", ""en"": ""Burundi"", ""es"": ""Burundi"", ""fr"": ""Burundi"", ""ru"": ""Бурунди""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('BN', '{""ar"": ""بروني دار السلام"", ""cn"": ""文莱达鲁萨兰国"", ""en"": ""Brunei Darussalam"", ""es"": ""Brunei Darussalam"", ""fr"": ""Brunéi Darussalam"", ""ru"": ""Бруней-Даруссалам""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('CY', '{""ar"": ""قبرص"", ""cn"": ""塞浦路斯"", ""en"": ""Cyprus"", ""es"": ""Chipre"", ""fr"": ""Chypre"", ""ru"": ""Кипр""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('DM', '{""ar"": ""دومينيكا"", ""cn"": ""多米尼克"", ""en"": ""Dominica"", ""es"": ""Dominica"", ""fr"": ""Dominique"", ""ru"": ""Доминика""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('DO', '{""ar"": ""الجمهورية الدومينيكية"", ""cn"": ""多米尼加"", ""en"": ""Dominican Republic"", ""es"": ""República Dominicana"", ""fr"": ""République dominicaine"", ""ru"": ""Доминиканская Республика""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('GD', '{""ar"": ""غرينادا"", ""cn"": ""格林纳达"", ""en"": ""Grenada"", ""es"": ""Granada"", ""fr"": ""Grenade"", ""ru"": ""Гренада""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('GE', '{""ar"": ""جورجيا"", ""cn"": ""格鲁吉亚"", ""en"": ""Georgia"", ""es"": ""Georgia"", ""fr"": ""Géorgie"", ""ru"": ""Грузия""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('GP', '{""ar"": ""غوادلوب"", ""cn"": ""瓜德罗普"", ""en"": ""Guadeloupe"", ""es"": ""Guadalupe"", ""fr"": ""Guadeloupe"", ""ru"": ""Гваделупа""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('HT', '{""ar"": ""هايتي"", ""cn"": ""海地"", ""en"": ""Haiti"", ""es"": ""Haití"", ""fr"": ""Haïti"", ""ru"": ""Гаити""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('ID', '{""ar"": ""إندونيسيا"", ""cn"": ""印度尼西亚"", ""en"": ""Indonesia"", ""es"": ""Indonesia"", ""fr"": ""Indonésie"", ""ru"": ""Индонезия""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('IL', '{""ar"": ""إسرائيل"", ""cn"": ""以色列"", ""en"": ""Israel"", ""es"": ""Israel"", ""fr"": ""Israël"", ""ru"": ""Израиль""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('IQ', '{""ar"": ""العراق"", ""cn"": ""伊拉克"", ""en"": ""Iraq"", ""es"": ""Iraq"", ""fr"": ""Iraq"", ""ru"": ""Ирак""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('JM', '{""ar"": ""جامايكا"", ""cn"": ""牙买加"", ""en"": ""Jamaica"", ""es"": ""Jamaica"", ""fr"": ""Jamaïque"", ""ru"": ""Ямайка""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('KG', '{""ar"": ""قيرغيزستان"", ""cn"": ""吉尔吉斯斯坦"", ""en"": ""Kyrgyzstan"", ""es"": ""Kirguistán"", ""fr"": ""Kirghizistan"", ""ru"": ""Кыргызстан""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('KH', '{""ar"": ""كمبوديا"", ""cn"": ""柬埔寨"", ""en"": ""Cambodia"", ""es"": ""Camboya"", ""fr"": ""Cambodge"", ""ru"": ""Камбоджа""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('KN', '{""ar"": ""سانت كيتس ونيفس"", ""cn"": ""圣基茨和尼维斯"", ""en"": ""Saint Kitts and Nevis"", ""es"": ""Saint Kitts y Nevis"", ""fr"": ""Saint-Kitts-et-Nevis"", ""ru"": ""Сент-Китс и Невис""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('RU', '{""ar"": ""الاتحاد الروسي"", ""cn"": ""俄罗斯联邦"", ""en"": ""Russian Federation"", ""es"": ""Federación de Rusia"", ""fr"": ""Fédération de Russie"", ""ru"": ""Российская Федерация""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('–', '{""ar"": ""سارك"", ""cn"": ""萨克"", ""en"": ""Sark"", ""es"": ""Sark"", ""fr"": ""Sercq"", ""ru"": ""Сарк""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('AE', '{""ar"": ""الإمارات العربية المتحدة"", ""cn"": ""阿拉伯联合酋长国"", ""en"": ""United Arab Emirates"", ""es"": ""Emiratos Árabes Unidos"", ""fr"": ""Émirats arabes unis"", ""ru"": ""Объединенные Арабские Эмираты""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('AL', '{""ar"": ""ألبانيا"", ""cn"": ""阿尔巴尼亚"", ""en"": ""Albania"", ""es"": ""Albania"", ""fr"": ""Albanie"", ""ru"": ""Албания""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('ZA', '{""ar"": ""جنوب أفريقيا"", ""cn"": ""南非"", ""en"": ""South Africa"", ""es"": ""Sudáfrica"", ""fr"": ""Afrique du Sud"", ""ru"": ""Южная Африка""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('BR', '{""ar"": ""البرازيل"", ""cn"": ""巴西"", ""en"": ""Brazil"", ""es"": ""Brasil"", ""fr"": ""Brésil"", ""ru"": ""Бразилия""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('CC', '{""ar"": ""جزر كوكس (كيلينغ)"", ""cn"": ""科科斯（基林）群岛"", ""en"": ""Cocos (Keeling) Islands"", ""es"": ""Islas Cocos (Keeling)"", ""fr"": ""Îles des Cocos (Keeling)"", ""ru"": ""Кокосовых (Килинг) островов""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('CD', '{""ar"": ""جمهورية الكونغو الديمقراطية"", ""cn"": ""刚果民主共和国"", ""en"": ""Democratic Republic of the Congo"", ""es"": ""República Democrática del Congo"", ""fr"": ""République démocratique du Congo"", ""ru"": ""Демократическая Республика Конго""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('FI', '{""ar"": ""فنلندا"", ""cn"": ""芬兰"", ""en"": ""Finland"", ""es"": ""Finlandia"", ""fr"": ""Finlande"", ""ru"": ""Финляндия""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('GB', '{""ar"": ""المملكة المتحدة لبريطانيا العظمى وآيرلندا الشمالية"", ""cn"": ""大不列颠及北爱尔兰联合王国"", ""en"": ""United Kingdom of Great Britain and Northern Ireland"", ""es"": ""Reino Unido de Gran Bretaña e Irlanda del Norte"", ""fr"": ""Royaume-Uni de Grande-Bretagne et d’Irlande du Nord"", ""ru"": ""Соединенное Королевство Великобритании и Северной Ирландии""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('IR', '{""ar"": ""إيران (جمهورية - الإسلامية)"", ""cn"": ""伊朗伊斯兰共和国"", ""en"": ""Iran (Islamic Republic of)"", ""es"": ""Irán (República Islámica del)"", ""fr"": ""Iran (République islamique d’)"", ""ru"": ""Иран (Исламская Республика)""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('MD', '{""ar"": ""جمهورية مولدوفا"", ""cn"": ""摩尔多瓦共和国"", ""en"": ""Republic of Moldova"", ""es"": ""República de Moldova"", ""fr"": ""République de Moldova"", ""ru"": ""Республика Молдова""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('TK', '{""ar"": ""توكيلاو"", ""cn"": ""托克劳"", ""en"": ""Tokelau"", ""es"": ""Tokelau"", ""fr"": ""Tokélaou"", ""ru"": ""Токелау""}');
                  INSERT INTO public.""Countries"" (""Code"", ""Name"") VALUES ('UM', '{""ar"": ""نائية التابعة للولايات المتحدة"", ""cn"": ""美国本土外小岛屿"", ""en"": ""United States Minor Outlying Islands"", ""es"": ""Islas menores alejadas de Estados Unidos"", ""fr"": ""Îles mineures éloignées des États-Unis"", ""ru"": ""Внешние малые острова Соединенных Штатов""}');";
            migrationBuilder.Sql(sql);
        }


        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var sql = 
                @"DELETE FROM public.""Countries"" WHERE 
                ""Code"" = 'AF' OR
                ""Code"" = 'AR' OR
                ""Code"" = 'AZ' OR
                ""Code"" = 'IN' OR
                ""Code"" = 'KP' OR
                ""Code"" = 'KY' OR
                ""Code"" = 'KZ' OR
                ""Code"" = 'LA' OR
                ""Code"" = 'LC' OR
                ""Code"" = 'LK' OR
                ""Code"" = 'MM' OR
                ""Code"" = 'MV' OR
                ""Code"" = 'MY' OR
                ""Code"" = 'NP' OR
                ""Code"" = 'PH' OR
                ""Code"" = 'PK' OR
                ""Code"" = 'SG' OR
                ""Code"" = 'TH' OR
                ""Code"" = 'TJ' OR
                ""Code"" = 'TL' OR
                ""Code"" = 'TM' OR
                ""Code"" = 'UZ' OR
                ""Code"" = 'VN' OR
                ""Code"" = 'EE' OR
                ""Code"" = 'FO' OR
                ""Code"" = 'HK' OR
                ""Code"" = 'IE' OR
                ""Code"" = 'IM' OR
                ""Code"" = 'IS' OR
                ""Code"" = 'JO' OR
                ""Code"" = 'JP' OR
                ""Code"" = 'KR' OR
                ""Code"" = 'KW' OR
                ""Code"" = 'LB' OR
                ""Code"" = 'LT' OR
                ""Code"" = 'LV' OR
                ""Code"" = 'MF' OR
                ""Code"" = 'MN' OR
                ""Code"" = 'BH' OR
                ""Code"" = 'BT' OR
                ""Code"" = 'CN' OR
                ""Code"" = 'DK' OR
                ""Code"" = 'OM' OR
                ""Code"" = 'MO' OR
                ""Code"" = 'MQ' OR
                ""Code"" = 'MS' OR
                ""Code"" = 'NO' OR
                ""Code"" = 'PR' OR
                ""Code"" = 'RO' OR
                ""Code"" = 'SK' OR
                ""Code"" = 'SX' OR
                ""Code"" = 'TC' OR
                ""Code"" = 'TT' OR
                ""Code"" = 'UA' OR
                ""Code"" = 'VC' OR
                ""Code"" = 'VG' OR
                ""Code"" = 'VI' OR
                ""Code"" = 'AX' OR
                ""Code"" = 'BA' OR
                ""Code"" = 'BE' OR
                ""Code"" = 'BG' OR
                ""Code"" = 'BY' OR
                ""Code"" = 'CH' OR
                ""Code"" = 'CU' OR
                ""Code"" = 'CZ' OR
                ""Code"" = 'DE' OR
                ""Code"" = 'ES' OR
                ""Code"" = 'FR' OR
                ""Code"" = 'GG' OR
                ""Code"" = 'GI' OR
                ""Code"" = 'GR' OR
                ""Code"" = 'HR' OR
                ""Code"" = 'HU' OR
                ""Code"" = 'IT' OR
                ""Code"" = 'JE' OR
                ""Code"" = 'LI' OR
                ""Code"" = 'AD' OR
                ""Code"" = 'AT' OR
                ""Code"" = 'LU' OR
                ""Code"" = 'MC' OR
                ""Code"" = 'ME' OR
                ""Code"" = 'MK' OR
                ""Code"" = 'MT' OR
                ""Code"" = 'NL' OR
                ""Code"" = 'PL' OR
                ""Code"" = 'PS' OR
                ""Code"" = 'PT' OR
                ""Code"" = 'QA' OR
                ""Code"" = 'RS' OR
                ""Code"" = 'SA' OR
                ""Code"" = 'SE' OR
                ""Code"" = 'SI' OR
                ""Code"" = 'SJ' OR
                ""Code"" = 'SM' OR
                ""Code"" = 'SY' OR
                ""Code"" = 'TR' OR
                ""Code"" = 'VA' OR
                ""Code"" = 'YE' OR
                ""Code"" = 'BW' OR
                ""Code"" = 'CF' OR
                ""Code"" = 'CG' OR
                ""Code"" = 'CI' OR
                ""Code"" = 'CM' OR
                ""Code"" = 'CV' OR
                ""Code"" = 'CW' OR
                ""Code"" = 'DZ' OR
                ""Code"" = 'EG' OR
                ""Code"" = 'EH' OR
                ""Code"" = 'GA' OR
                ""Code"" = 'GH' OR
                ""Code"" = 'GM' OR
                ""Code"" = 'GN' OR
                ""Code"" = 'GQ' OR
                ""Code"" = 'GW' OR
                ""Code"" = 'LR' OR
                ""Code"" = 'LS' OR
                ""Code"" = 'BJ' OR
                ""Code"" = 'BS' OR
                ""Code"" = 'LY' OR
                ""Code"" = 'MA' OR
                ""Code"" = 'ML' OR
                ""Code"" = 'MR' OR
                ""Code"" = 'MW' OR
                ""Code"" = 'NA' OR
                ""Code"" = 'NE' OR
                ""Code"" = 'RE' OR
                ""Code"" = 'RW' OR
                ""Code"" = 'SC' OR
                ""Code"" = 'SD' OR
                ""Code"" = 'SO' OR
                ""Code"" = 'SS' OR
                ""Code"" = 'ST' OR
                ""Code"" = 'SZ' OR
                ""Code"" = 'TD' OR
                ""Code"" = 'TN' OR
                ""Code"" = 'TZ' OR
                ""Code"" = 'UG' OR
                ""Code"" = 'YT' OR
                ""Code"" = 'ZM' OR
                ""Code"" = 'ZW' OR
                ""Code"" = 'AU' OR
                ""Code"" = 'BF' OR
                ""Code"" = 'BL' OR
                ""Code"" = 'CR' OR
                ""Code"" = 'CX' OR
                ""Code"" = 'DJ' OR
                ""Code"" = 'ER' OR
                ""Code"" = 'ET' OR
                ""Code"" = 'FK' OR
                ""Code"" = 'GF' OR
                ""Code"" = 'GS' OR
                ""Code"" = 'GT' OR
                ""Code"" = 'GY' OR
                ""Code"" = 'HN' OR
                ""Code"" = 'IO' OR
                ""Code"" = 'AS' OR
                ""Code"" = 'BZ' OR
                ""Code"" = 'KE' OR
                ""Code"" = 'KM' OR
                ""Code"" = 'MG' OR
                ""Code"" = 'MU' OR
                ""Code"" = 'MX' OR
                ""Code"" = 'MZ' OR
                ""Code"" = 'NG' OR
                ""Code"" = 'NI' OR
                ""Code"" = 'PA' OR
                ""Code"" = 'PE' OR
                ""Code"" = 'PY' OR
                ""Code"" = 'SH' OR
                ""Code"" = 'SL' OR
                ""Code"" = 'SN' OR
                ""Code"" = 'SR' OR
                ""Code"" = 'SV' OR
                ""Code"" = 'TF' OR
                ""Code"" = 'TG' OR
                ""Code"" = 'UY' OR
                ""Code"" = 'VE' OR
                ""Code"" = 'BM' OR
                ""Code"" = 'BQ' OR
                ""Code"" = 'BV' OR
                ""Code"" = 'CA' OR
                ""Code"" = 'CK' OR
                ""Code"" = 'CL' OR
                ""Code"" = 'CO' OR
                ""Code"" = 'EC' OR
                ""Code"" = 'FJ' OR
                ""Code"" = 'FM' OR
                ""Code"" = 'GL' OR
                ""Code"" = 'GU' OR
                ""Code"" = 'HM' OR
                ""Code"" = 'KI' OR
                ""Code"" = 'MH' OR
                ""Code"" = 'MP' OR
                ""Code"" = 'AI' OR
                ""Code"" = 'AR' OR
                ""Code"" = 'AW' OR
                ""Code"" = 'NC' OR
                ""Code"" = 'NF' OR
                ""Code"" = 'NR' OR
                ""Code"" = 'NU' OR
                ""Code"" = 'NZ' OR
                ""Code"" = 'PF' OR
                ""Code"" = 'PG' OR
                ""Code"" = 'PM' OR
                ""Code"" = 'PN' OR
                ""Code"" = 'PW' OR
                ""Code"" = 'SB' OR
                ""Code"" = 'TO' OR
                ""Code"" = 'TV' OR
                ""Code"" = 'US' OR
                ""Code"" = 'VU' OR
                ""Code"" = 'WF' OR
                ""Code"" = 'WS' OR
                ""Code"" = 'AG' OR
                ""Code"" = 'AM' OR
                ""Code"" = 'AO' OR
                ""Code"" = 'BB' OR
                ""Code"" = 'BO' OR
                ""Code"" = 'BD' OR
                ""Code"" = 'BI' OR
                ""Code"" = 'BN' OR
                ""Code"" = 'CY' OR
                ""Code"" = 'DM' OR
                ""Code"" = 'DO' OR
                ""Code"" = 'GD' OR
                ""Code"" = 'GE' OR
                ""Code"" = 'GP' OR
                ""Code"" = 'HT' OR
                ""Code"" = 'ID' OR
                ""Code"" = 'IL' OR
                ""Code"" = 'IQ' OR
                ""Code"" = 'JM' OR
                ""Code"" = 'KG' OR
                ""Code"" = 'KH' OR
                ""Code"" = 'KN' OR
                ""Code"" = 'RU' OR
                ""Code"" = '–', OR
                ""Code"" = 'AE' OR
                ""Code"" = 'AL' OR
                ""Code"" = 'ZA' OR
                ""Code"" = 'BR' OR
                ""Code"" = 'CC' OR
                ""Code"" = 'CD' OR
                ""Code"" = 'FI' OR
                ""Code"" = 'GB' OR
                ""Code"" = 'IR' OR
                ""Code"" = 'MD' OR
                ""Code"" = 'TK' OR
                ""Code"" = 'UM' OR";
            migrationBuilder.Sql(sql);
        }
    }
}
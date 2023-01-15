using DB_app.Models;

namespace DB_app.Repository.SQL;

public class DataSeeder
{
    public static void ClearData(SQLContext dbContext)
    {
        // TODO be careful with deletion order...

        dbContext.Orders.RemoveRange(dbContext.Orders);
        dbContext.OrderItems.RemoveRange(dbContext.OrderItems);
        dbContext.Products.RemoveRange(dbContext.Products);
        dbContext.Pharmacies.RemoveRange(dbContext.Pharmacies);
        dbContext.Hospitals.RemoveRange(dbContext.Hospitals);
        dbContext.Medicines.RemoveRange(dbContext.Medicines);
        dbContext.Addresses.RemoveRange(dbContext.Addresses);

        dbContext.SaveChanges();

    }


    public static void Seed(SQLContext dbContext)
    {

        var addresses = new List<Address>()
        {
            new Address( 1, "Москва", "Сельская", "44" ),
            new Address( 2, "Истра" , "бульвар Гоголя", "28" ),
            new Address( 3, "Ступино" , "Косиора", "83" ),
            new Address( 4, "Щёлково" , "въезд Гагарина", "01" ),
            new Address( 5, "Зарайск" , "бульвар Сталина", "77" ),
            new Address( 6, "Ступино" , "пр. Балканская", "32" ),
            new Address( 7, "Дмитров" , "бульвар Балканская", "96" ),
            new Address( 8, "Пушкино" , "пр. Сталина", "63" ),
            new Address( 9, "Дмитров" , "бульвар Гагарина", "40" ),
            new Address( 10, "Егорьевск" , "Бухарестская", "11" ),
            new Address( 11, "Зарайск" , "пр. Космонавтов", "72" ),
            new Address( 12, "Можайск" , "спуск Гоголя", "14" ),
            new Address( 13, "Подольск" , "проезд 1905 года", "16" ),
            new Address( 14, "Мытищи" , "пер. Ленина", "55" ),
            new Address( 15, "Лотошино" , "наб. Бухарестская", "00" ),
            new Address( 16, "Солнечногорск" , "пер. Домодедовская", "38" ),
            new Address( 17, "Луховицы" , "шоссе Домодедовская", "54" ),
            new Address( 18, "Москва" , "пер. Чехова", "46" ),
            new Address( 19, "Егорьевск" , "пер. 1905 года", "15" ),
            new Address( 21, "Луховицы" , "пер. Славы", "10" ),
            new Address( 22, "Шаховская" , "пер. Славы", "12/2" ),
            new Address( 23, "Москва" , "Очаковское шоссе", "дом 2, корпус 4, строение 1" ),
            new Address( 24, "Москва" , "Зенитчиков", "дом 9, строение 2" ),
            new Address( 25, "Москва" , "Тихорецкий бульвар", "дом 21, строение 71" ),
            new Address( 26, "Бердск" , "Микрорайон", "4" ),
            new Address( 27, "Бердск" , "Микрорайон", "48Б" ),
            new Address( 28, "Посёлок Двуречье" , "Молодёжная", "15/1" ),
            new Address( 29, "Шатура" , "спуск Косиора", "89" ),
            new Address( 30, "Клин" , "пр. Славы", "07" ),
            new Address( 31, "Люберцы" , "шоссе Космонавтов", "60" ),
            new Address( 32, "Можайск" , "пер. Будапештсткая", "97" ),
            new Address( 33, "Орехово-Зуево" , "проезд Ладыгина", "28" ),
            new Address( 34, "Щёлково" , "наб. Домодедовская", "47" ),
            new Address( 35, "Наро-Фоминск" , "спуск Чехова", "55" ),
            new Address( 36, "Воскресенск" , "въезд Бухарестская", "58" ),
            new Address( 37, "Серебряные Пруды" , "проезд Косиора", "62" ),
            new Address( 38, "Ногинск" , "спуск Гагарина", "68" ),
            new Address( 39, "Дмитров" , "въезд Ладыгина", "41" ),
            new Address( 40, "Лотошино" , "наб. Ленина", "76" ),
            new Address( 41, "Шатура" , "ул. Домодедовская", "73" ),
            new Address( 42, "Москва" , "бульвар Гоголя", "90" ),
            new Address( 43, "Серпухов" , "въезд Ломоносова", "48" ),
            new Address( 44, "Чехов" , "въезд Чехова", "17" ),
            new Address( 45, "Воскресенск" , "ул. Ладыгина", "47" ),
            new Address( 46, "Озёры" , "наб. 1905 года", "22" ),
            new Address( 47, "Серебряные Пруды" , "пл. 1905 года", "96" ),
            new Address( 48, "Наро-Фоминск" , "шоссе Гагарина", "41" ),
            new Address( 49, "Можайск" , "пл. Ладыгина", "79" ),
            new Address( 50, "Воскресенск" , "пл. Гагарина", "13" ),
            new Address( 51, "Дмитров" , "пр. Гагарина", "22" ),
            new Address( 52, "Луховицы" , "спуск 1905 года", "40" )
        };

        dbContext.Addresses.AddRange(addresses);


        var medicines = new List<Medicine>()
        {
            new Medicine(1, "Эстрапристин", "Капсулы"),
            new Medicine(2, "Налвираганан", "Капсулы"),
            new Medicine(3, "Соматоксептаког", "Таблетки"),
            new Medicine(4, "Пегакефстатин", "Суспензия"),
            new Medicine(5, "Сомгестрикуриум", "Мазь"),
            new Medicine(6, "Винасалтоцин", "Таблетки"),
            new Medicine(7, "Лоратадин", "Таблетки"),
            new Medicine(8, "Лордес", "Таблетки"),
            new Medicine(9, "Эдермик", "Гель"),
            new Medicine(10, "Алерон", "Таблетки"),
            new Medicine(11, "Псило-бальзам", "Гель"),
            new Medicine(12, "Дезрадин", "Таблетки"),
            new Medicine(13, "АлергоМакс", "Сироп"),
            new Medicine(14, "Дезлоратадин-Тева", "Таблетки"),
            new Medicine(15, "Синафлана", "Мазь"),
            new Medicine(16, "Элизиум", "Таблетки"),
            new Medicine(17, "Элизиум", "Раствор оральный"),
            new Medicine(18, "Аллерголик", "Таблетки"),
            new Medicine(19, "Кларитин", "Таблетки"),
            new Medicine(21, "Диазолин СБ-Фарма", "драже"),
            new Medicine(22, "Фенистил", "Капли оральные"),
            new Medicine(23, "Фрибрис", "Таблетки"),
            new Medicine(24, "Тавегил", "Раствор для инъекций"),
            new Medicine(25, "Лорано ОДТ", "Таблетки"),
            new Medicine(26, "Синафлан-Фитофaрм", "Мазь"),
            new Medicine(27, "Псило-Бальзам", "Гель"),
            new Medicine(28, "Эриус", "Таблетки"),
            new Medicine(29, "Гидрокортизон", "Мазь глазная"),
            new Medicine(30, "Ибупрофен", "Суспензия"),
            new Medicine(31, "Инфлюцеин", "Капсулы"),
            new Medicine(32, "Covid test", "Тест")
        };

        dbContext.Medicines.AddRange(medicines);
        

        var pharmacies = new List<Pharmacy>()
        {
            new Pharmacy(1, "Аптека от склада", "892277534495", "1138793867530",
                new(){ addresses[3], addresses[5], addresses[1] }
            ),
            new Pharmacy(2, "Фармкопейка", "2675519818", "2097045775495",
                new(){ addresses[2], addresses[6], addresses[4] }
            ),
            new Pharmacy(3, "Аптека.ру", "2675519818", "2097045775495",
                new(){ addresses[7], addresses[8], addresses[11] }
            ),
            new Pharmacy(4, "Аптека №1", "4093395003", "3032517827724",
                new(){ addresses[7], addresses[8], addresses[10] }
            ),
            new Pharmacy(5, "Нейрон", "3269059863", "3068588117848",
                new(){ addresses[12], addresses[9], addresses[15] }
            ),
            new Pharmacy(6, "Здравсити", "4284981087", "7040229063274",
                new(){ addresses[18], addresses[13], addresses[16] }
            ),
            new Pharmacy(7, "Магнит Аптека", "1236423538", "1125089191260",
                new(){ addresses[23], addresses[20], addresses[21] }
            ),
        };

        dbContext.Pharmacies.AddRange(pharmacies);
        

        var hospitals = new List<Hospital>()
        {
            new Hospital(1, "Копылов", "Вольдемар", "Улебович", "", "", 
                new() {addresses[29], addresses[25], addresses[22] } 
            ),
            new Hospital(2, "Белозёров", "Натан", "Филиппович", "", "", 
                new() {addresses[26], addresses[27], addresses[28] } 
            ),
            new Hospital(3, "Аксёнов", "Абрам", "Геласьевич", "", "", 
                new() {addresses[37], addresses[33], addresses[35] } 
            ),
            new Hospital(4, "Гордеев", "Зиновий", "Витальевич", "", "", 
                new() {addresses[31], addresses[30], addresses[32] } 
            ),
            new Hospital(5, "Князев", "Иван", "Денисович", "", "", 
                new() {addresses[36], addresses[38], addresses[40] } 
            ),
            new Hospital(6, "Мухин", "Тихон", "Кимович", "", "", 
                new() {addresses[46], addresses[41], addresses[43] } 
            ),
            new Hospital(7, "Королёв", "Алан", "Константинович", "", "", 
                new() {addresses[42], addresses[44], addresses[47] } 
            ),
            new Hospital(8, "Жданов", "Ростислав", "Антонович", "", "", 
                new() {addresses[48], addresses[50], addresses[49] } 
            ),
            new Hospital(9, "Карпов", "Моисей", "Геласьевич", "", "", 
                new() {addresses[0], addresses[17], addresses[14] } 
                
            ),
            new Hospital(10, "Морозов", "Рубен", "Федотович", "", "", 
                new() {addresses[19], addresses[24], addresses[34] } 
            ),
        };

        dbContext.Hospitals.AddRange(hospitals);

        var products = new List<Product>()
        {
            new Product(1, medicines[1], pharmacies[3], 789.99, 192),
            new Product(2, medicines[1], pharmacies[6], 789.99, 195),
            new Product(3, medicines[1], pharmacies[6], 401.99, 162),
            new Product(4, medicines[2], pharmacies[5], 789.99, 136),
            new Product(5, medicines[1], pharmacies[5], 389, 955),
            new Product(6, medicines[16], pharmacies[5], 729, 415),
            new Product(7, medicines[7], pharmacies[6], 433, 497),
            new Product(8, medicines[4], pharmacies[6], 732, 689),
            new Product(9, medicines[7], pharmacies[6], 942, 187),
            new Product(10, medicines[12], pharmacies[3], 381, 614),
            new Product(11, medicines[13], pharmacies[3], 915, 758),
            new Product(12, medicines[19], pharmacies[3], 593, 599),
            new Product(13, medicines[1], pharmacies[1], 1730, 480),
            new Product(14, medicines[17], pharmacies[2], 763, 1607),
            new Product(15, medicines[1], pharmacies[3], 1099, 1479),
            new Product(16, medicines[10], pharmacies[3], 2499, 2418),
            new Product(17, medicines[6], pharmacies[5], 1299, 2346),
            new Product(18, medicines[16], pharmacies[4], 295, 1856),
            new Product(19, medicines[1], pharmacies[3], 783, 900),
            new Product(20, medicines[8], pharmacies[5], 1023, 393),
            new Product(21, medicines[1], pharmacies[3], 1599, 2120),
            new Product(22, medicines[1], pharmacies[4], 1853, 1208),
            new Product(23, medicines[27], pharmacies[3], 2783, 248),
            new Product(24, medicines[28], pharmacies[3], 721, 1457),
            new Product(25, medicines[16], pharmacies[5], 499, 370),
            new Product(26, medicines[21], pharmacies[3], 5789.999999, 2183),
            new Product(27, medicines[1], pharmacies[3], 953, 2399),
            new Product(28, medicines[11], pharmacies[1], 743, 1904),
            new Product(29, medicines[29], pharmacies[0], 1239, 1246),
            new Product(30, medicines[24], pharmacies[0], 1100, 1459),
            new Product(31, medicines[1], pharmacies[0], 145, 664),
            new Product(32, medicines[27], pharmacies[3], 99, 2252),
            new Product(33, medicines[20], pharmacies[3], 104, 1845),

        };

        dbContext.Products.AddRange(products);

        dbContext.SaveChanges();
    }
}

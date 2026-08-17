using System.Collections.Generic;

namespace FiloYonetimi;

public static class VehicleCatalog
{
    public static readonly Dictionary<string,string[]> BrandsAndModels = new()
    {
        ["Foton"] = new[] { "Tunland G7", "Tunland G9", "Auman EST", "Auman GTL", "Aumark", "M4", "M5", "M6" },
        ["Alfa Romeo"] = new[] { "Giulia", "Stelvio", "Junior" },
        ["Audi"] = new[] { "A1", "A3", "A4", "A5", "A6", "A7", "A8", "Q2", "Q3", "Q4 e-tron", "Q5", "Q6 e-tron", "Q7", "Q8", "e-tron GT" },
        ["BMW"] = new[] { "1 Serisi", "2 Serisi", "3 Serisi", "4 Serisi", "5 Serisi", "7 Serisi", "8 Serisi", "X1", "X2", "X3", "X4", "X5", "X6", "X7", "i4", "i5", "i7", "iX", "iX1", "iX2" },
        ["BYD"] = new[] { "Atto 3", "Dolphin", "Seal", "Seal U", "Seal U DM-i", "Han", "Tang" },
        ["Chery"] = new[] { "Tiggo 4", "Tiggo 7", "Tiggo 8", "Omoda 5" },
        ["Citroen"] = new[] { "C3", "C3 Aircross", "C4", "C4 X", "C5 Aircross", "Berlingo", "Jumpy", "SpaceTourer", "Jumper" },
        ["Cupra"] = new[] { "Formentor", "Leon", "Born", "Terramar", "Tavascan" },
        ["Dacia"] = new[] { "Sandero", "Sandero Stepway", "Duster", "Jogger", "Spring" },
        ["DAF"] = new[] { "XD", "XF", "XG", "XG+" },
        ["DS Automobiles"] = new[] { "DS 3", "DS 4", "DS 7", "DS 9" },
        ["Fiat"] = new[] { "Egea", "500", "500e", "500X", "Doblo", "Fiorino", "Ducato", "Scudo", "Ulysse" },
        ["Ford"] = new[] { "Fiesta", "Focus", "Puma", "Kuga", "Explorer", "Tourneo Courier", "Transit Courier", "Tourneo Connect", "Transit Custom", "Tourneo Custom", "Transit", "Ranger", "Maverick", "Mustang Mach-E" },
        ["Honda"] = new[] { "Civic", "City", "HR-V", "CR-V", "ZR-V", "e:Ny1" },
        ["Hino"] = new[] { "300", "500", "700" },
        ["Hyundai"] = new[] { "i10", "i20", "i30", "Bayon", "Kona", "Tucson", "Santa Fe", "Staria", "IONIQ 5", "IONIQ 6", "Kona Electric" },
        ["Isuzu"] = new[] { "D-Max", "NPR", "NLR", "NMR", "NQR", "F-Series" },
        ["Iveco"] = new[] { "Daily", "Eurocargo", "S-Way", "X-Way", "T-Way" },
        ["Jaecoo"] = new[] { "J7", "J7 PHEV" },
        ["Jeep"] = new[] { "Avenger", "Renegade", "Compass", "Wrangler", "Grand Cherokee" },
        ["KGM"] = new[] { "Torres", "Torres EVX", "Korando", "Musso", "Musso Grand", "Rexton" },
        ["Kia"] = new[] { "Picanto", "Rio", "Stonic", "Ceed", "XCeed", "Niro", "Niro EV", "Sportage", "Sorento", "EV3", "EV6", "EV9" },
        ["Lancia"] = new[] { "Ypsilon" },
        ["Land Rover"] = new[] { "Defender", "Discovery", "Discovery Sport", "Range Rover", "Range Rover Evoque", "Range Rover Sport", "Range Rover Velar" },
        ["Lexus"] = new[] { "LBX", "UX", "NX", "RX", "RZ", "ES", "LS", "LM" },
        ["MAN"] = new[] { "TGE", "TGL", "TGM", "TGS", "TGX" },
        ["Mercedes-Benz"] = new[] { "A-Serisi", "B-Serisi", "C-Serisi", "E-Serisi", "S-Serisi", "CLA", "CLE", "GLA", "GLB", "GLC", "GLE", "GLS", "EQA", "EQB", "EQE", "EQS", "Sprinter", "Vito", "Vito Tourer", "Citan", "Actros", "Arocs", "Atego" },
        ["MG"] = new[] { "MG3", "MG4", "MG5", "ZS", "ZS EV", "HS", "HS PHEV", "Marvel R", "Cyberster" },
        ["MINI"] = new[] { "Cooper", "Countryman", "Aceman" },
        ["Mitsubishi Fuso"] = new[] { "Canter" },
        ["Nissan"] = new[] { "Micra", "Juke", "Qashqai", "X-Trail", "Ariya", "Townstar", "Interstar", "Navara" },
        ["Omoda"] = new[] { "5", "5 EV" },
        ["Opel"] = new[] { "Corsa", "Astra", "Mokka", "Crossland", "Grandland", "Combo", "Vivaro", "Movano", "Zafira Life" },
        ["Peugeot"] = new[] { "208", "308", "408", "2008", "3008", "5008", "508", "Rifter", "Partner", "Expert", "Boxer" },
        ["Porsche"] = new[] { "Macan", "Cayenne", "Panamera", "Taycan", "911", "718" },
        ["Renault"] = new[] { "Clio", "Captur", "Megane", "Megane E-Tech", "Austral", "Arkana", "Rafale", "Symbioz", "Duster", "Kangoo", "Trafic", "Master" },
        ["Renault Trucks"] = new[] { "E-Tech D", "E-Tech T", "D", "C", "K", "T" },
        ["Scania"] = new[] { "P", "G", "R", "S", "Super", "Touring" },
        ["SEAT"] = new[] { "Ibiza", "Arona", "Leon", "Ateca", "Tarraco" },
        ["Skoda"] = new[] { "Fabia", "Scala", "Octavia", "Superb", "Kamiq", "Karoq", "Kodiaq", "Enyaq" },
        ["Subaru"] = new[] { "Crosstrek", "Forester", "Outback", "Solterra" },
        ["Suzuki"] = new[] { "Swift", "Ignis", "Vitara", "S-Cross", "Jimny" },
        ["Tesla"] = new[] { "Model 3", "Model Y", "Model S", "Model X" },
        ["Togg"] = new[] { "T10X", "T10F" },
        ["Toyota"] = new[] { "Yaris", "Yaris Cross", "Corolla", "Corolla Cross", "C-HR", "Camry", "RAV4", "Land Cruiser", "Hilux", "Proace", "Proace City", "Proace Max" },
        ["Volkswagen"] = new[] { "Polo", "Golf", "Passat", "T-Roc", "T-Cross", "Taigo", "Tiguan", "Touareg", "ID.3", "ID.4", "ID.5", "ID.7", "Caddy", "Transporter", "Crafter", "Amarok" },
        ["Volvo"] = new[] { "EX30", "EX40", "EC40", "XC40", "XC60", "XC90", "S60", "S90", "V60", "V90", "EX90", "FH", "FM", "FMX" },
        ["Yokohama"] = new[] { "Diğer" },
    };

    // Geniş başlangıç lastik ebatı kataloğu. Araç tipi seçimiyle filtrelenir.
    public static readonly Dictionary<string,string[]> TireSizesByType = new()
    {
        ["Otomobil"] = new[] {
            "145/65R15","155/65R14","165/65R14","165/70R14","175/65R14","175/65R15","175/70R14","185/55R15","185/60R15","185/60R16","185/65R15","185/65R16","195/50R15","195/55R15","195/55R16","195/60R15","195/60R16","195/65R15","195/65R16","205/45R16","205/45R17","205/50R16","205/50R17","205/55R16","205/55R17","205/60R16","205/60R17","205/65R15","215/40R17","215/45R16","215/45R17","215/50R17","215/55R16","215/55R17","215/60R16","215/60R17","215/65R16","225/40R18","225/45R17","225/45R18","225/50R17","225/50R18","225/55R16","225/55R17","225/55R18","225/60R16","225/60R17","225/65R17","235/35R19","235/40R18","235/45R17","235/45R18","235/50R18","235/55R17","235/55R18","235/60R18","245/35R19","245/40R18","245/45R18","245/45R19","245/50R18","255/35R19","255/40R19","255/45R18","265/35R20","275/35R20","285/30R20"
        },
        ["SUV"] = new[] {
            "215/65R16","215/70R16","215/75R15","225/55R18","225/60R17","225/60R18","225/65R17","225/65R18","225/70R16","225/75R16","235/55R18","235/55R19","235/60R18","235/60R19","235/65R17","235/65R18","235/70R16","235/75R15","245/45R20","245/50R19","245/55R19","245/60R18","245/65R17","255/50R19","255/55R18","255/55R19","255/60R18","255/60R19","255/65R17","255/70R18","265/50R19","265/55R19","265/60R18","265/65R17","265/70R16","275/40R20","275/45R20","275/50R20","275/55R19","285/45R22","285/50R20","285/60R18","295/40R21","295/45R20","305/45R22"
        },
        ["Pickup"] = new[] {
            "215/70R16","215/75R15","225/70R15","225/75R15","235/70R16","235/75R15","245/70R16","245/75R16","255/65R17","255/70R16","255/70R17","265/60R18","265/65R17","265/70R16","265/70R17","265/75R16","275/55R20","275/60R20","275/65R18","285/55R20","285/60R18","285/60R20","295/60R20"
        },
        ["Kamyonet"] = new[] {
            "155R12C","165R13C","175R13C","175R14C","185R14C","195R14C","195/70R15C","195/75R16C","205/65R16C","205/70R15C","205/75R16C","215/65R16C","215/70R15C","215/75R16C","225/65R16C","225/70R15C","225/75R16C","235/60R17C","235/65R16C","245/70R16C","255/65R16C","265/70R16C","285/65R16C"
        },
        ["Kamyon"] = new[] {
            "8.25R16","9.00R20","10.00R20","11.00R20","12.00R20","12R22.5","13R22.5","295/80R22.5","315/70R22.5","315/80R22.5","385/55R22.5","385/65R22.5","425/65R22.5","445/65R22.5"
        }
    };

    public static readonly string[] TireBrands = new[] {
        "Bridgestone","Michelin","Goodyear","Continental","Pirelli","Lassa","Petlas","Hankook","Yokohama","Dunlop","Falken","Toyo","Maxxis","Kumho","Giti","Nokian","BFGoodrich","Firestone","Fulda","Barum","Semperit","Uniroyal","Sava","Matador","Debica","Cooper","General Tire","Laufenn","Kormoran","Linglong","Goodride","Triangle","Sailun","RoadX","Milestone","Starmaxx","Funtoma","Sumitomo","Dayton","Linglong","Powertrac","Centara","Royal Black","Michelin Agilis","Petlas Fullpower","Lassa Transway","Bridgestone Duravis"
    };

    public static readonly string[] FuelTypes = new[] { "Benzin", "Dizel", "LPG", "Hibrit", "Elektrik", "Benzin + LPG", "Benzin + Elektrik", "Dizel + Elektrik", "MHEV", "HEV", "PHEV" };
}

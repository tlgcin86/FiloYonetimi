using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;

namespace FiloYonetimi;

public static class Database
{
    private static readonly string DbPath = Path.Combine(AppContext.BaseDirectory, "FiloYonetimi.db");
    private static string ConnectionString => $"Data Source={DbPath}";

    public static void Initialize()
    {
        using var c = new SqliteConnection(ConnectionString);
        c.Open();
        using var cmd = c.CreateCommand();
        cmd.CommandText = """
        CREATE TABLE IF NOT EXISTS Vehicles (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Plate TEXT NOT NULL UNIQUE,
            Brand TEXT,
            Model TEXT,
            ModelYear INTEGER,
            Type TEXT,
            Fuel TEXT,
            FuelCompany TEXT,
            ChassisNo TEXT,
            FleetCompany TEXT,
            FleetTaxOffice TEXT,
            GpsId TEXT,
            AdBlueKitNo TEXT,
            TireSize TEXT,
            TireBrand TEXT,
            WashCodes TEXT,
            InspectionDate TEXT,
            TachographInspectionDate TEXT,
            Location TEXT,
            Unit TEXT,
            CreatedAt TEXT NOT NULL,
            UpdatedAt TEXT
        );
        """;
        cmd.ExecuteNonQuery();

        using var migration = c.CreateCommand();
        migration.CommandText = """
        SELECT COUNT(*) FROM pragma_table_info('Vehicles') WHERE name='ModelYear';
        """;
        var hasModelYear = Convert.ToInt32(migration.ExecuteScalar()) > 0;
        if (!hasModelYear)
        {
            migration.CommandText = "ALTER TABLE Vehicles ADD COLUMN ModelYear INTEGER;";
            migration.ExecuteNonQuery();
        }
    }

    public static List<Vehicle> GetAll(string? search = null)
    {
        var list = new List<Vehicle>();
        using var c = new SqliteConnection(ConnectionString);
        c.Open();
        using var cmd = c.CreateCommand();
        cmd.CommandText = """
        SELECT Id, Plate, Brand, Model, ModelYear, Type, Fuel, FuelCompany, ChassisNo,
               FleetCompany, FleetTaxOffice, GpsId, AdBlueKitNo, TireSize,
               TireBrand, WashCodes, InspectionDate, TachographInspectionDate,
               Location, Unit
        FROM Vehicles
        WHERE ($search IS NULL OR $search = '' OR
               Plate LIKE '%' || $search || '%' OR
               Brand LIKE '%' || $search || '%' OR
               Model LIKE '%' || $search || '%' OR
               Location LIKE '%' || $search || '%' OR
               Unit LIKE '%' || $search || '%')
        ORDER BY Plate;
        """;
        cmd.Parameters.AddWithValue("$search", (object?)search ?? DBNull.Value);
        using var r = cmd.ExecuteReader();
        while (r.Read())
        {
            list.Add(new Vehicle
            {
                Id = r.GetInt32(0), Plate = r.GetString(1),
                Brand = r.IsDBNull(2) ? "" : r.GetString(2),
                Model = r.IsDBNull(3) ? "" : r.GetString(3),
                ModelYear = r.IsDBNull(4) ? null : r.GetInt32(4),
                Type = r.IsDBNull(5) ? "" : r.GetString(5),
                Fuel = r.IsDBNull(6) ? "" : r.GetString(6),
                FuelCompany = r.IsDBNull(7) ? "" : r.GetString(7),
                ChassisNo = r.IsDBNull(8) ? "" : r.GetString(8),
                FleetCompany = r.IsDBNull(9) ? "" : r.GetString(9),
                FleetTaxOffice = r.IsDBNull(10) ? "" : r.GetString(10),
                GpsId = r.IsDBNull(11) ? "" : r.GetString(11),
                AdBlueKitNo = r.IsDBNull(12) ? "" : r.GetString(12),
                TireSize = r.IsDBNull(13) ? "" : r.GetString(13),
                TireBrand = r.IsDBNull(14) ? "" : r.GetString(14),
                WashCodes = r.IsDBNull(15) ? "" : r.GetString(15),
                InspectionDate = ParseDate(r, 16),
                TachographInspectionDate = ParseDate(r, 17),
                Location = r.IsDBNull(18) ? "" : r.GetString(18),
                Unit = r.IsDBNull(19) ? "" : r.GetString(19)
            });
        }
        return list;
    }

    public static Vehicle? Get(int id) => GetAll().Find(v => v.Id == id);

    public static void Save(Vehicle v)
    {
        using var c = new SqliteConnection(ConnectionString);
        c.Open();
        using var cmd = c.CreateCommand();
        cmd.CommandText = """
        INSERT INTO Vehicles
        (Plate,Brand,Model,ModelYear,Type,Fuel,FuelCompany,ChassisNo,FleetCompany,FleetTaxOffice,GpsId,
         AdBlueKitNo,TireSize,TireBrand,WashCodes,InspectionDate,TachographInspectionDate,Location,Unit,CreatedAt,UpdatedAt)
        VALUES ($Plate,$Brand,$Model,$ModelYear,$Type,$Fuel,$FuelCompany,$ChassisNo,$FleetCompany,$FleetTaxOffice,$GpsId,
         $AdBlueKitNo,$TireSize,$TireBrand,$WashCodes,$InspectionDate,$TachographInspectionDate,$Location,$Unit,$CreatedAt,$UpdatedAt);
        """;
        AddParams(cmd, v);
        cmd.ExecuteNonQuery();
    }

    public static void Update(Vehicle v)
    {
        using var c = new SqliteConnection(ConnectionString);
        c.Open();
        using var cmd = c.CreateCommand();
        cmd.CommandText = """
        UPDATE Vehicles SET Plate=$Plate,Brand=$Brand,Model=$Model,ModelYear=$ModelYear,Type=$Type,Fuel=$Fuel,FuelCompany=$FuelCompany,
        ChassisNo=$ChassisNo,FleetCompany=$FleetCompany,FleetTaxOffice=$FleetTaxOffice,GpsId=$GpsId,
        AdBlueKitNo=$AdBlueKitNo,TireSize=$TireSize,TireBrand=$TireBrand,WashCodes=$WashCodes,
        InspectionDate=$InspectionDate,TachographInspectionDate=$TachographInspectionDate,Location=$Location,
        Unit=$Unit,UpdatedAt=$UpdatedAt WHERE Id=$Id;
        """;
        AddParams(cmd, v);
        cmd.Parameters.AddWithValue("$Id", v.Id);
        cmd.ExecuteNonQuery();
    }

    public static void Delete(int id)
    {
        using var c = new SqliteConnection(ConnectionString);
        c.Open();
        using var cmd = c.CreateCommand();
        cmd.CommandText = "DELETE FROM Vehicles WHERE Id=$Id;";
        cmd.Parameters.AddWithValue("$Id", id);
        cmd.ExecuteNonQuery();
    }

    public static bool PlateExists(string plate, int excludeId = 0)
    {
        using var c = new SqliteConnection(ConnectionString);
        c.Open();
        using var cmd = c.CreateCommand();
        cmd.CommandText = "SELECT COUNT(*) FROM Vehicles WHERE UPPER(Plate)=UPPER($Plate) AND Id<>$Id;";
        cmd.Parameters.AddWithValue("$Plate", plate.Trim());
        cmd.Parameters.AddWithValue("$Id", excludeId);
        return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
    }

    private static DateTime? ParseDate(SqliteDataReader r, int i)
        => r.IsDBNull(i) ? null : DateTime.TryParse(r.GetString(i), out var d) ? d : null;

    private static void AddParams(SqliteCommand cmd, Vehicle v)
    {
        cmd.Parameters.AddWithValue("$Plate", v.Plate.Trim());
        cmd.Parameters.AddWithValue("$Brand", v.Brand);
        cmd.Parameters.AddWithValue("$Model", v.Model);
        cmd.Parameters.AddWithValue("$ModelYear", (object?)v.ModelYear ?? DBNull.Value);
        cmd.Parameters.AddWithValue("$Type", v.Type);
        cmd.Parameters.AddWithValue("$Fuel", v.Fuel);
        cmd.Parameters.AddWithValue("$FuelCompany", v.FuelCompany);
        cmd.Parameters.AddWithValue("$ChassisNo", v.ChassisNo);
        cmd.Parameters.AddWithValue("$FleetCompany", v.FleetCompany);
        cmd.Parameters.AddWithValue("$FleetTaxOffice", v.FleetTaxOffice);
        cmd.Parameters.AddWithValue("$GpsId", v.GpsId);
        cmd.Parameters.AddWithValue("$AdBlueKitNo", v.AdBlueKitNo);
        cmd.Parameters.AddWithValue("$TireSize", v.TireSize);
        cmd.Parameters.AddWithValue("$TireBrand", v.TireBrand);
        cmd.Parameters.AddWithValue("$WashCodes", v.WashCodes);
        cmd.Parameters.AddWithValue("$InspectionDate", v.InspectionDate?.ToString("yyyy-MM-dd") ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("$TachographInspectionDate", v.TachographInspectionDate?.ToString("yyyy-MM-dd") ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("$Location", v.Location);
        cmd.Parameters.AddWithValue("$Unit", v.Unit);
        cmd.Parameters.AddWithValue("$CreatedAt", DateTime.Now.ToString("s"));
        cmd.Parameters.AddWithValue("$UpdatedAt", DateTime.Now.ToString("s"));
    }
}

using System;

namespace FiloYonetimi;

public class Vehicle
{
    public int Id { get; set; }
    public string Plate { get; set; } = "";
    public string Brand { get; set; } = "";
    public string Model { get; set; } = "";
    public int? ModelYear { get; set; }
    public string Type { get; set; } = "";
    public string Fuel { get; set; } = "";
    public string FuelCompany { get; set; } = "";
    public string ChassisNo { get; set; } = "";
    public string FleetCompany { get; set; } = "";
    public string FleetTaxOffice { get; set; } = "";
    public string GpsId { get; set; } = "";
    public string AdBlueKitNo { get; set; } = "";
    public string TireSize { get; set; } = "";
    public string TireBrand { get; set; } = "";
    public string WashCodes { get; set; } = "";
    public DateTime? InspectionDate { get; set; }
    public DateTime? TachographInspectionDate { get; set; }
    public string Location { get; set; } = "";
    public string Unit { get; set; } = "";
}

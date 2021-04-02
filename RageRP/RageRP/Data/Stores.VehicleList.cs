using RageRP.Server.DAL.DTO;
using System.Collections.Generic;

namespace RageRP.Server.Data
{
    public class VehicleStores
    {
        public static List<DTOVehicleList> PoliceVehicles()
        {
            var _vehicles = new List<DTOVehicleList>();
            _vehicles.Add(new DTOVehicleList() { VehicleName = "PD Crown Vic", VehicleCode = "crownvic", Price = 0 });
            _vehicles.Add(new DTOVehicleList() { VehicleName = "PD Charger", VehicleCode = "sv4", Price = 0 });
            _vehicles.Add(new DTOVehicleList() { VehicleName = "Slicktop Explorer Traffic", VehicleCode = "16exp", Price = 0 });
            _vehicles.Add(new DTOVehicleList() { VehicleName = "Unmarked Silverado", VehicleCode = "17silver", Price = 0 });
            _vehicles.Add(new DTOVehicleList() { VehicleName = "Marked Tahoe", VehicleCode = "16tahoe", Price = 0 });
            //_vehicles.Add(new DTOVehicleList() { VehicleName = "PD Taurus", VehicleCode = "police3", Price = 0 });
            //_vehicles.Add(new DTOVehicleList() { VehicleName = "PD SUV", VehicleCode = "lspdsuv", Price = 0 });
            //_vehicles.Add(new DTOVehicleList() { VehicleName = "PD SUV Hatchback", VehicleCode = "lspdsuv2", Price = 0 });
            //_vehicles.Add(new DTOVehicleList() { VehicleName = "PD SUV Hatchback Supervisor", VehicleCode = "lspdsuv2", Price = 0 });
            //_vehicles.Add(new DTOVehicleList() { VehicleName = "PD Unmarked Crown Vic", VehicleCode = "police4", Price = 0 });
            //_vehicles.Add(new DTOVehicleList() { VehicleName = "PD Unmarked Taurus", VehicleCode = "pinter", Price = 0 });
            //_vehicles.Add(new DTOVehicleList() { VehicleName = "PD Unmarked Charger", VehicleCode = "fbi", Price = 0 });
            //_vehicles.Add(new DTOVehicleList() { VehicleName = "PD Unmarked SUV", VehicleCode = "fbi2", Price = 0 });
            _vehicles.Add(new DTOVehicleList() { VehicleName = "PD Bike", VehicleCode = "policeb", Price = 0 });
            _vehicles.Add(new DTOVehicleList() { VehicleName = "Prisoner Van", VehicleCode = "policet", Price = 0 });
            _vehicles.Add(new DTOVehicleList() { VehicleName = "Sherrif Crown Vic", VehicleCode = "sheriff", Price = 0 });
            _vehicles.Add(new DTOVehicleList() { VehicleName = "Sherrif SUV", VehicleCode = "sherrif2", Price = 0 });
            _vehicles.Add(new DTOVehicleList() { VehicleName = "Park Ranger SUV", VehicleCode = "pranger", Price = 0 });
            _vehicles.Add(new DTOVehicleList() { VehicleName = "Bearcat", VehicleCode = "riot", Price = 0 });
            _vehicles.Add(new DTOVehicleList() { VehicleName = "RIOT Water Truck", VehicleCode = "riot2", Price = 0 });
            _vehicles.Add(new DTOVehicleList() { VehicleName = "Prisoner Bus", VehicleCode = "pbus", Price = 0 });
            return _vehicles;
        }

        public static List<DTOVehicleList> EMSVehicles()
        {
            var _vehicles = new List<DTOVehicleList>();
            _vehicles.Add(new DTOVehicleList() { VehicleName = "Ambulance", VehicleCode = "ambulance", Price = 0 });
            _vehicles.Add(new DTOVehicleList() { VehicleName = "Ambulance Truck", VehicleCode = "namb", Price = 0 });
            return _vehicles;
        }

        public static List<DTOVehicleList> FireVehicles()
        {
            var _vehicles = new List<DTOVehicleList>();
            _vehicles.Add(new DTOVehicleList() { VehicleName = "FireTruck", VehicleCode = "firetruk", Price = 0 });
            _vehicles.Add(new DTOVehicleList() { VehicleName = "Fire SUV", VehicleCode = "emssuv", Price = 0 });
            return _vehicles;
        }

        public static List<DTOVehicleList> PoliceHelicopters()
        {
            var _helicopters = new List<DTOVehicleList>();
            _helicopters.Add(new DTOVehicleList() { VehicleName = "Police Maverick", VehicleCode = "polmav", Price = 0 });
            _helicopters.Add(new DTOVehicleList() { VehicleName = "Buzzard Unarmed", VehicleCode = "buzzard2", Price = 0 });
            return _helicopters;
        }

        public static List<DTOVehicleList> EMSHelicopters()
        {
            var _helicopters = new List<DTOVehicleList>();

            return _helicopters;
        }

        public static List<DTOVehicleList> SportsVehicles()
        {
            var _vehicles = new List<DTOVehicleList>();
            _vehicles.Add(new DTOVehicleList() { VehicleName = "Adder", VehicleCode = "adder", Price = 100000 });
            _vehicles.Add(new DTOVehicleList() { VehicleName = "Vision-E", VehicleCode = "visione", Price = 100000 });
            _vehicles.Add(new DTOVehicleList() { VehicleName = "T20", VehicleCode = "t20", Price = 100000 });
            _vehicles.Add(new DTOVehicleList() { VehicleName = "Zentorno", VehicleCode = "zentorno", Price = 100000 });
            _vehicles.Add(new DTOVehicleList() { VehicleName = "Prototipo", VehicleCode = "prototipo", Price = 100000 });
            _vehicles.Add(new DTOVehicleList() { VehicleName = "Vision-E", VehicleCode = "visione", Price = 100000 });
            _vehicles.Add(new DTOVehicleList() { VehicleName = "Nero", VehicleCode = "nero", Price = 100000 });
            _vehicles.Add(new DTOVehicleList() { VehicleName = "Nero-2", VehicleCode = "nero2", Price = 100000 });
            _vehicles.Add(new DTOVehicleList() { VehicleName = "Schafter", VehicleCode = "schafter2", Price = 100000 });
            _vehicles.Add(new DTOVehicleList() { VehicleName = "Schafter 2", VehicleCode = "schafter3", Price = 100000 });
            _vehicles.Add(new DTOVehicleList() { VehicleName = "Schafter 2 LWB", VehicleCode = "schafter4", Price = 100000 });
            _vehicles.Add(new DTOVehicleList() { VehicleName = "Schlagen", VehicleCode = "schlagen", Price = 100000 });
            _vehicles.Add(new DTOVehicleList() { VehicleName = "Schwarzer", VehicleCode = "schwarzer", Price = 100000 });
            _vehicles.Add(new DTOVehicleList() { VehicleName = "Neon", VehicleCode = "neon", Price = 100000 });
            return _vehicles;
        }
    }
}

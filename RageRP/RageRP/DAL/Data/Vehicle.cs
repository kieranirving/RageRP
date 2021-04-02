using RageRP.DTO;
using RageRP.Server.Helpers;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace RageRP.Server.DAL.Data
{
    public class Vehicle
    {
        private string _connectionString;
        private bool _isDebug;
        public Vehicle(bool isDebug)
        {
            _connectionString = DataAccess.ConnectionString;
            _isDebug = isDebug;
        }

        public async Task<long> InsertVehicle(DTOVehicle dto)
        {
            if (_isDebug)
            {
                Console.WriteLine($"DAL.Player.InsertVehicle {dto.CharacterID}");
            }

            long vehicleID = 0;

            using (var conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("spu_InsertVehicle", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CharacterID", dto.CharacterID);
                    command.Parameters.AddWithValue("@carModel", dto.carModel);
                    command.Parameters.AddWithValue("@colour1", dto.colour1);
                    command.Parameters.AddWithValue("@colour2", dto.colour2);
                    command.Parameters.AddWithValue("@plate", dto.plate);

                    command.Parameters.Add("@VehicleID", SqlDbType.BigInt).Direction = ParameterDirection.Output;

                    try
                    {
                        await conn.OpenAsync();

                        await command.ExecuteNonQueryAsync();

                        vehicleID = long.Parse(command.Parameters["@VehicleID"].Value.ToString());
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine($"DAL.Player.InsertVehicle({dto.CharacterID}) SQLException {ex}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"DAL.Player.InsertVehicle({dto.CharacterID}) Exception {ex}");
                    }
                    finally
                    {
                        if (conn != null)
                        {
                            conn.Close();
                            conn.Dispose();
                        }
                    }
                }
            }
            return vehicleID;
        }

        public async Task<bool> UpdateVehicle(DTOVehicle dto)
        {
            if (_isDebug)
            {
                Console.WriteLine($"DAL.Player.UpdateVehicle {dto.VehicleID} {dto.CharacterID}");
            }

            bool isSaved = false;

            using (var conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("spu_InsertPlayer", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@VehicleID", dto.VehicleID);
                    command.Parameters.AddWithValue("@carModel", dto.carModel);
                    command.Parameters.AddWithValue("@colour1", dto.colour1);
                    command.Parameters.AddWithValue("@colour2", dto.colour2);
                    command.Parameters.AddWithValue("@plate", dto.plate);
                    command.Parameters.AddWithValue("@mods", dto.mods);
                    command.Parameters.AddWithValue("@inGarage", dto.inGarage);
                    command.Parameters.AddWithValue("@trunk", dto.trunk);

                    try
                    {
                        await conn.OpenAsync();

                        await command.ExecuteNonQueryAsync();

                        isSaved = true;
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine($"DAL.Player.UpdateVehicle({dto.VehicleID}) SQLException {ex}");
                        isSaved = false;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"DAL.Player.UpdateVehicle({dto.VehicleID}) Exception {ex}");
                        isSaved = false;
                    }
                    finally
                    {
                        if (conn != null)
                        {
                            conn.Close();
                            conn.Dispose();
                        }
                    }
                }
            }
            return isSaved;
        }

        public async Task<DTOVehicle> GetVehicle(long VehicleID)
        {
            if (_isDebug)
            {
                Console.WriteLine($"DAL.Player.GetVehicle {VehicleID}");
            }

            var dto = new DTOVehicle();
            SqlDataReader rdr = null;

            using (var conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("spu_GetVehicle", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@VehicleID", VehicleID);

                    try
                    {
                        await conn.OpenAsync();

                        rdr = await command.ExecuteReaderAsync();

                        while (rdr.Read())
                        {
                            dto.VehicleID = SqlReaderHelper.GetValue<long>(rdr, "id");
                            dto.carModel = SqlReaderHelper.GetValue<string>(rdr, "carModel");
                            dto.colour1 = SqlReaderHelper.GetValue<int>(rdr, "colour1");
                            dto.colour2 = SqlReaderHelper.GetValue<int>(rdr, "colour2");
                            dto.plate = SqlReaderHelper.GetValue<string>(rdr, "plate");
                            dto.mods = SqlReaderHelper.GetValue<string>(rdr, "mods");
                            dto.inGarage = SqlReaderHelper.GetValue<bool>(rdr, "inGarage");
                            dto.trunk = SqlReaderHelper.GetValue<string>(rdr, "trunk");
                        }
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine($"DAL.Player.GetVehicle({VehicleID}) SQLException {ex}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"DAL.Player.GetVehicle({VehicleID}) Exception {ex}");
                    }
                    finally
                    {
                        if (rdr != null)
                        {
                            rdr.Close();
                        }
                        if (conn != null)
                        {
                            conn.Close();
                            conn.Dispose();
                        }
                    }
                }
            }
            return dto;
        }
    }
}

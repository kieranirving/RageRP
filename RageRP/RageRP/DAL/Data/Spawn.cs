using RageRP.Server.DAL.DTO;
using RageRP.Server.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace RageRP.Server.DAL.Data
{
    public class Spawn
    {
        private string _connectionString;
        private bool _isDebug;
        public Spawn(bool isDebug)
        {
            _connectionString = DataAccess.ConnectionString;
            _isDebug = isDebug;
        }

        public async Task<DTOSpawnLocations> GetSpawnLocationsByCharacterID(long CharacterID)
        {
            if (_isDebug)
            {
                Console.WriteLine($"DAL.GetSpawnLocationsByCharacterID {CharacterID}");
            }

            var dtos = new DTOSpawnLocations();
            dtos.locations = new List<DTOSpawnLocation>();
            SqlDataReader rdr = null;

            using (var conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("spu_GetSpawnLocations", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CharacterID", CharacterID);

                    try
                    {
                        await conn.OpenAsync();

                        rdr = await command.ExecuteReaderAsync();

                        while (await rdr.ReadAsync())
                        {
                            var dto = new DTOSpawnLocation();
                            dto.id = SqlReaderHelper.GetValue<long>(rdr, "id");
                            dto.Name = SqlReaderHelper.GetValue<string>(rdr, "LocationName");
                            dto.camPosX = SqlReaderHelper.GetValue<float>(rdr, "posX");
                            dto.camPosY = SqlReaderHelper.GetValue<float>(rdr, "posY");
                            dto.camPosZ = SqlReaderHelper.GetValue<float>(rdr, "posZ");
                            dto.camLookX = SqlReaderHelper.GetValue<float>(rdr, "lookX");
                            dto.camLookY = SqlReaderHelper.GetValue<float>(rdr, "lookY");
                            dto.camLookZ = SqlReaderHelper.GetValue<float>(rdr, "lookZ");
                            dto.spawnX = SqlReaderHelper.GetValue<float>(rdr, "spawnX");
                            dto.spawnY = SqlReaderHelper.GetValue<float>(rdr, "spawnY");
                            dto.spawnZ = SqlReaderHelper.GetValue<float>(rdr, "spawnZ");
                            dto.heading = SqlReaderHelper.GetValue<float>(rdr, "heading");
                            dtos.locations.Add(dto);
                        }
                        if(dtos.locations.Count >= 1)
                        {
                            dtos.success = true;
                        }
                        else
                        {
                            dtos.hasError = true;
                            dtos.errorMessage = "No Locations Found";
                        }
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine($"DAL.Player.GetSpawnLocationsByCharacterID SQLException {ex}");
                        dtos.success = false;
                        dtos.hasError = true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"DAL.Player.GetSpawnLocationsByCharacterID Exception {ex}");
                        dtos.success = false;
                        dtos.hasError = true;
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

            return dtos;
        }

        public async Task<DTOBackgrounds> GetSpawnCameras()
        {
            if (_isDebug)
            {
                Console.WriteLine($"DAL.GetSpawnCameras");
            }

            var dtos = new DTOBackgrounds();
            dtos.cameras = new List<DTOBackground>();
            SqlDataReader rdr = null;

            using (var conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("spu_GetBackgrounds", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    try
                    {
                        await conn.OpenAsync();

                        rdr = await command.ExecuteReaderAsync();

                        while (await rdr.ReadAsync())
                        {
                            var dto = new DTOBackground();
                            dto.id = SqlReaderHelper.GetValue<long>(rdr, "id");
                            dto.Name = SqlReaderHelper.GetValue<string>(rdr, "LocationName");
                            dto.camPosX = SqlReaderHelper.GetValue<float>(rdr, "posX");
                            dto.camPosY = SqlReaderHelper.GetValue<float>(rdr, "posY");
                            dto.camPosZ = SqlReaderHelper.GetValue<float>(rdr, "posZ");
                            dto.camLookX = SqlReaderHelper.GetValue<float>(rdr, "lookX");
                            dto.camLookY = SqlReaderHelper.GetValue<float>(rdr, "lookY");
                            dto.camLookZ = SqlReaderHelper.GetValue<float>(rdr, "lookZ");
                            dtos.cameras.Add(dto);
                        }
                        if (dtos.cameras.Count >= 1)
                        {
                            dtos.success = true;
                        }
                        else
                        {
                            dtos.hasError = true;
                            dtos.errorMessage = "No Cameras Found";
                        }
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine($"DAL.Player.GetSpawnCameras SQLException {ex}");
                        dtos.success = false;
                        dtos.hasError = true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"DAL.Player.GetSpawnCameras Exception {ex}");
                        dtos.success = false;
                        dtos.hasError = true;
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

            return dtos;
        }
    }
}

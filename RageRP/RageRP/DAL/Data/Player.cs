using RageRP.DTO;
using RageRP.Server.Encryption;
using RageRP.Server.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace RageRP.Server.DAL.Data
{
    public class Player
    {
        private string _connectionString;
        private bool _isDebug;
        public Player(bool isDebug)
        {
            _connectionString = DataAccess.ConnectionString;
            _isDebug = isDebug;
        }

        public async Task<bool> CheckIfWhitelisted(string License)
        {
            if (_isDebug)
            {
                Console.WriteLine($"DAL.Player.CheckIfWhitelisted {License}");
            }

            bool isWhitelisted = false;
            SqlDataReader rdr = null;

            using (var conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("spu_CheckIfWhitelisted", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@License", License);

                    try
                    {
                        await conn.OpenAsync();

                        rdr = await command.ExecuteReaderAsync();

                        while (await rdr.ReadAsync())
                        {
                            isWhitelisted = SqlReaderHelper.GetValue<bool>(rdr, "isWhitelisted");
                        }
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine($"DAL.Player.CheckIfWhitelisted SQLException {ex}");
                        isWhitelisted = false;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"DAL.Player.CheckIfWhitelisted Exception {ex}");
                        isWhitelisted = false;
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
            return isWhitelisted;
        }

        public async Task<DTOPlayer> GetPlayerBySocialClubName(string SocialClubName)
        {
            if (_isDebug)
            {
                Console.WriteLine($"DAL.GetPlayerBySocialClubName {SocialClubName}");
            }

            var dto = new DTOPlayer();
            SqlDataReader rdr = null;

            using (var conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("spu_GetPlayerBySocialClubName", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@PlayerName", SocialClubName);

                    try
                    {
                        await conn.OpenAsync();

                        rdr = await command.ExecuteReaderAsync();

                        while (await rdr.ReadAsync())
                        {
                            dto.success = true;
                            dto.Player_ID = EncryptionService.EncryptID(SqlReaderHelper.GetValue<long>(rdr, "id"));
                            dto.PlayerName = SqlReaderHelper.GetValue<string>(rdr, "PlayerName");
                            dto.License = SqlReaderHelper.GetValue<string>(rdr, "License");
                            dto.DateCreated = SqlReaderHelper.GetValue<DateTime>(rdr, "DateCreated");
                            dto.isAdmin = SqlReaderHelper.GetValue<bool>(rdr, "isAdmin");
                            dto.AdminLevel = SqlReaderHelper.GetValue<int>(rdr, "AdminLevel");
                            dto.Password = SqlReaderHelper.GetValue<string>(rdr, "Password");

                            dto.isPermBanned = SqlReaderHelper.GetValue<bool>(rdr, "isPermBanned");
                            dto.TempBanDate = SqlReaderHelper.GetValue<DateTime>(rdr, "TempBanDate");
                            dto.BanReason = SqlReaderHelper.GetValue<string>(rdr, "BanReason");
                        }
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine($"DAL.Player.GetPlayerBySocialClubName SQLException {ex}");
                        dto.success = false;
                        dto.hasError = true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"DAL.Player.GetPlayerBySocialClubName Exception {ex}");
                        dto.success = false;
                        dto.hasError = true;
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

        public async Task<long> InsertPlayer(string playerName)
        {
            if (_isDebug)
            {
                Console.WriteLine($"DAL.Player.InsertPlayer {playerName}");
            }

            long playerID = 0;

            using (var conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("spu_InsertPlayer", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@PlayerName", playerName);

                    command.Parameters.Add("@RefID", SqlDbType.BigInt).Direction = ParameterDirection.Output;

                    try
                    {
                        await conn.OpenAsync();

                        await command.ExecuteNonQueryAsync();

                        playerID = long.Parse(command.Parameters["@RefID"].Value.ToString());
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine($"DAL.Player.InsertPlayer SQLException {ex}");
                        playerID = -1;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"DAL.Player.InsertPlayer Exception {ex}");
                        playerID = -1;
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
            return playerID;
        }

        public async Task<long> InsertCharacter(DTOCharacter dto)
        {
            if (_isDebug)
            {
                Console.WriteLine($"DAL.Player.InsertCharacter {dto.PlayerID} {dto.CharacterID}");
            }

            long characterID = 0;

            using (var conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("spu_InsertCharacter", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@PlayerID", dto.PlayerID);
                    command.Parameters.AddWithValue("@CharacterName", dto.CharacterName);
                    command.Parameters.AddWithValue("@Gender", dto.Gender);
                    command.Parameters.AddWithValue("@CurrentPed", dto.CurrentPed);
                    command.Parameters.AddWithValue("@Cash", dto.Cash);
                    command.Parameters.AddWithValue("@Bank", dto.Bank);
                    command.Parameters.AddWithValue("@PedString", dto.PedString);

                    command.Parameters.Add("@CharacterID", SqlDbType.BigInt).Direction = ParameterDirection.Output;

                    try
                    {
                        await conn.OpenAsync();

                        await command.ExecuteNonQueryAsync();

                        characterID = long.Parse(command.Parameters["@CharacterID"].Value.ToString());
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine($"DAL.Player.InsertCharacter SQLException {ex}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"DAL.Player.InsertCharacter Exception {ex}");
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
            return characterID;
        }

        public async Task<bool> UpdateCharacter(DTOCharacter dto)
        {
            if (_isDebug)
            {
                Console.WriteLine($"DAL.Player.UpdateCharacter {dto.PlayerID} {dto.CharacterID}");
            }

            bool isSaved = false;
            SqlDataReader rdr = null;

            using (var conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("spu_UpdateCharacter", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CharacterID", dto.CharacterID);
                    //command.Parameters.AddWithValue("@CharacterName", dto.CharacterName);
                    command.Parameters.AddWithValue("@Gender", dto.Gender);
                    command.Parameters.AddWithValue("@CurrentPed", dto.CurrentPed);
                    command.Parameters.AddWithValue("@Cash", dto.Cash);
                    command.Parameters.AddWithValue("@Bank", dto.Bank);
                    command.Parameters.AddWithValue("@PedString", dto.PedString);

                    try
                    {
                        await conn.OpenAsync();

                        rdr = await command.ExecuteReaderAsync();

                        isSaved = true;
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine($"DAL.Player.UpdateCharacter SQLException {ex}");
                        isSaved = false;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"DAL.Player.UpdateCharacter Exception {ex}");
                        isSaved = false;
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
            return isSaved;
        }

        public async Task<bool> UpdateCharacterCash(DTOCharacter dto)
        {
            if (_isDebug)
            {
                Console.WriteLine($"DAL.Player.UpdateCharacterCash {dto.PlayerID} {dto.CharacterID}");
            }

            bool isSaved = false;
            SqlDataReader rdr = null;

            using (var conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("spu_UpdateCharacterCash", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CharacterID", dto.CharacterID);
                    command.Parameters.AddWithValue("@Cash", dto.Cash);

                    try
                    {
                        await conn.OpenAsync();

                        rdr = await command.ExecuteReaderAsync();

                        isSaved = true;
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine($"DAL.Player.UpdateCharacterCash SQLException {ex}");
                        isSaved = false;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"DAL.Player.UpdateCharacterCash Exception {ex}");
                        isSaved = false;
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
            return isSaved;
        }
        
        public async Task<bool> UpdateCharacterFaction(DTOCharacter dto)
        {
            if (_isDebug)
            {
                Console.WriteLine($"DAL.Player.UpdateCharacterFaction {dto.PlayerID} {dto.CharacterID}");
            }

            bool isSaved = false;
            SqlDataReader rdr = null;

            using (var conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("spu_UpdateCharacterFaction", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CharacterID", dto.CharacterID);
                    command.Parameters.AddWithValue("@isEMS", dto.isEMS);
                    command.Parameters.AddWithValue("@isPolice", dto.isPolice);
                    bool inFaction = dto.isEMS || dto.isPolice ? true : false;
                    command.Parameters.AddWithValue("@inFaction", inFaction);
                    if(dto.EMSLevel != 0)
                        command.Parameters.AddWithValue("@Level", dto.EMSLevel);
                    else if (dto.PoliceLevel != 0)
                        command.Parameters.AddWithValue("@Level", dto.PoliceLevel);
                    else
                        command.Parameters.AddWithValue("@Level", 0);
                    command.Parameters.AddWithValue("@Department", dto.Department);

                    try
                    {
                        await conn.OpenAsync();

                        rdr = await command.ExecuteReaderAsync();

                        isSaved = true;
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine($"DAL.Player.UpdateCharacterFaction SQLException {ex}");
                        isSaved = false;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"DAL.Player.UpdateCharacterFaction Exception {ex}");
                        isSaved = false;
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
            return isSaved;
        }

        public async Task<bool> UpdateCharacterBank(DTOCharacter dto)
        {
            if (_isDebug)
            {
                Console.WriteLine($"DAL.Player.UpdateCharacterBank {dto.PlayerID} {dto.CharacterID}");
            }

            bool isSaved = false;
            SqlDataReader rdr = null;

            using (var conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("spu_UpdateCharacterBank", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@CharacterID", dto.CharacterID);
                    command.Parameters.AddWithValue("@Bank", dto.Bank);

                    try
                    {
                        await conn.OpenAsync();

                        rdr = await command.ExecuteReaderAsync();

                        isSaved = true;
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine($"DAL.Player.UpdateCharacterBank SQLException {ex}");
                        isSaved = false;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"DAL.Player.UpdateCharacterBank Exception {ex}");
                        isSaved = false;
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
            return isSaved;
        }

        public async Task<DTOCharacter> GetCharacter(long PlayerID, long CharacterID)
        {
            if (_isDebug)
            {
                Console.WriteLine($"DAL.Player.GetCharacter {PlayerID} {CharacterID}");
            }

            var dto = new DTOCharacter();
            SqlDataReader rdr = null;

            using (var conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("spu_GetCharacter", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@PlayerID", PlayerID);
                    command.Parameters.AddWithValue("@CharacterID", CharacterID);

                    try
                    {
                        await conn.OpenAsync();

                        rdr = await command.ExecuteReaderAsync();

                        while (await rdr.ReadAsync())
                        {
                            dto.Character_ID = EncryptionService.EncryptID(SqlReaderHelper.GetValue<long>(rdr, "id"));
                            dto.CharacterName = SqlReaderHelper.GetValue<string>(rdr, "CharacterName");
                            dto.Gender = SqlReaderHelper.GetValue<int>(rdr, "Gender");
                            dto.CurrentPed = SqlReaderHelper.GetValue<string>(rdr, "CurrentPed");
                            dto.Cash = SqlReaderHelper.GetValue<int>(rdr, "Cash");
                            dto.Bank = SqlReaderHelper.GetValue<int>(rdr, "Bank");
                            dto.PedString = SqlReaderHelper.GetValue<string>(rdr, "PedString");
                            dto.isNewCharacter = SqlReaderHelper.GetValue<bool>(rdr, "isNewCharacter");
                            dto.isPolice = SqlReaderHelper.GetValue<bool>(rdr, "isPolice");
                            dto.PoliceLevel = SqlReaderHelper.GetValue<int>(rdr, "PoliceLevel");
                            dto.isEMS = SqlReaderHelper.GetValue<bool>(rdr, "isEMS");
                            dto.EMSLevel = SqlReaderHelper.GetValue<int>(rdr, "EMSLevel");
                            dto.Department = SqlReaderHelper.GetValue<int>(rdr, "Department");
                            dto.BadgeNumber = SqlReaderHelper.GetValue<string>(rdr, "BadgeNumber");
                        }
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine($"DAL.Player.GetCharacter({CharacterID}) SQLException {ex}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"DAL.Player.GetCharacter({CharacterID}) Exception {ex}");
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

        public async Task<List<DTOCharacter>> GetPlayerCharacters(long PlayerID)
        {
            if (_isDebug)
            {
                Console.WriteLine($"DAL.Player.GetPlayerCharacters {PlayerID}");
            }

            var dtos = new List<DTOCharacter>();
            SqlDataReader rdr = null;

            using (var conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("spu_GetPlayerCharacters", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@PlayerID", PlayerID);

                    try
                    {
                        await conn.OpenAsync();

                        rdr = await command.ExecuteReaderAsync();

                        while (await rdr.ReadAsync())
                        {
                            var dto = new DTOCharacter();
                            dto.Character_ID = EncryptionService.EncryptID(SqlReaderHelper.GetValue<long>(rdr, "id"));
                            dto.CharacterName = SqlReaderHelper.GetValue<string>(rdr, "CharacterName");
                            dto.Gender = SqlReaderHelper.GetValue<int>(rdr, "Gender");
                            dto.CurrentPed = SqlReaderHelper.GetValue<string>(rdr, "CurrentPed");
                            dto.Cash = SqlReaderHelper.GetValue<int>(rdr, "Cash");
                            dto.Bank = SqlReaderHelper.GetValue<int>(rdr, "Bank");
                            //K - Not sure this is really needed at the time of being called, plus it causes issues on the ui that I'm too tired to fix.
                            //dto.PedString = SqlReaderHelper.GetValue<string>(rdr, "PedString");
                            dtos.Add(dto);
                        }
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine($"DAL.Player.GetPlayerCharacters({PlayerID}) SQLException {ex}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"DAL.Player.GetPlayerCharacters({PlayerID}) Exception {ex}");
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

        public async Task<bool> DeleteCharacter(long PlayerID, long CharacterID)
        {
            if (_isDebug)
            {
                Console.WriteLine($"DAL.Player.DeleteCharacter {PlayerID} {CharacterID}");
            }

            bool isSaved = false;
            SqlDataReader rdr = null;

            using (var conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("spu_DeleteCharacter", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@PlayerID", PlayerID);
                    command.Parameters.AddWithValue("@CharacterID", CharacterID);

                    try
                    {
                        await conn.OpenAsync();

                        rdr = await command.ExecuteReaderAsync();

                        isSaved = true;
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine($"DAL.Player.DeleteCharacter SQLException {ex}");
                        isSaved = false;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"DAL.Player.DeleteCharacter Exception {ex}");
                        isSaved = false;
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
            return isSaved;
        }
    }
}

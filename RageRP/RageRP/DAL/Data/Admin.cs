using RageRP.DTO;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace RageRP.Server.DAL.Data
{
    public class Admin
    {
        private string _connectionString;
        private bool _isDebug;
        public Admin(bool isDebug)
        {
            _connectionString = DataAccess.ConnectionString;
            _isDebug = isDebug;
        }

        public async Task<bool> BanPlayer(DTOPlayer dto)
        {
            if (_isDebug)
            {
                Console.WriteLine($"DAL.Admin.BanPlayer {dto.PlayerID} {dto.isPermBanned} {dto.TempBanDate} {dto.BanReason}");
            }

            bool isSaved = false;
            SqlDataReader rdr = null;

            using (var conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("spu_BanPlayer", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@PlayerID", dto.PlayerID);

                    if (dto.isPermBanned)
                    {
                        command.Parameters.AddWithValue("@isPermBan", dto.isPermBanned);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@isTempBan", true);
                        command.Parameters.AddWithValue("@TempBanDate", dto.TempBanDate);
                    }

                    command.Parameters.AddWithValue("@BanReason", dto.BanReason);

                    try
                    {
                        await conn.OpenAsync();

                        rdr = await command.ExecuteReaderAsync();

                        isSaved = true;
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine($"DAL.Admin.BanPlayer {dto.PlayerID} {dto.isPermBanned} {dto.TempBanDate} {dto.BanReason} SQLException {ex}");
                        isSaved = false;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"DAL.Admin.BanPlayer {dto.PlayerID} {dto.isPermBanned} {dto.TempBanDate} {dto.BanReason} Exception {ex}");
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

        public async Task<bool> UpdateAdminLevel(DTOPlayer dto)
        {
            if (_isDebug)
            {
                Console.WriteLine($"DAL.Player.UpdateAdminLevel {dto.PlayerID} {dto.isAdmin} {dto.AdminLevel}");
            }

            bool isSaved = false;
            SqlDataReader rdr = null;

            using (var conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand("spu_UpdateAdminLevel", conn))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@PlayerID", dto.PlayerID);
                    command.Parameters.AddWithValue("@isAdmin", dto.isAdmin);
                    command.Parameters.AddWithValue("@AdminLevel", dto.AdminLevel);

                    try
                    {
                        await conn.OpenAsync();

                        rdr = await command.ExecuteReaderAsync();

                        isSaved = true;
                    }
                    catch (SqlException ex)
                    {
                        Console.WriteLine($"DAL.Player.UpdateAdminLevel SQLException {ex}");
                        isSaved = false;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"DAL.Player.UpdateAdminLevel Exception {ex}");
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

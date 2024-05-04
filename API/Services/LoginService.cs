using System;
using System.Data;
using API.Models;
using Microsoft.Extensions.Configuration;
using API.Services.Interfaces;
using System.Text;
using Microsoft.Data.SqlClient;

namespace API.Services
{
    public class LoginService : ILogin
    {
        private readonly string _connectionString;

        public LoginService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public string GetUser(Login login)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("sp_GetUser", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@User", login.User);
                cmd.Parameters.AddWithValue("@Pass", login.Pass);
                
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        StringBuilder result = new StringBuilder();
                        result.AppendLine("Inicio de sesión exitoso");
                        result.AppendLine($"ID: {reader["id_usuario"]}, Nombre de usuario: {reader["nombre_usuario"]}, Contraseña: {reader["password"]}");
                        result.AppendLine($"Rol: {reader["nombre_perfil"]}, Descripción del rol: {reader["descripcion"]}");
                        return result.ToString();
                    }
                }
            }

            return "Usuario no encontrado";
        }

        public string CreateUser(Login login)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("sp_CreateUser", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@User", login.User);
                cmd.Parameters.AddWithValue("@Pass", login.Pass);
                cmd.Parameters.AddWithValue("@RoleId", login.RoleId);
                
                int rowsAffected = cmd.ExecuteNonQuery();
                conn.Close();

                if (rowsAffected > 0)
                {
                    return "Creación de usuario exitosa";
                }
                else
                {
                    return "Error al crear usuario";
                }
            }
        }
    }
}

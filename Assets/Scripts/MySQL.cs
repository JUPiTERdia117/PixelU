using System;
using System.Data;
using System.IO;
using MySql.Data.MySqlClient;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MySQL : MonoBehaviour
{
    private string server = "localhost";
    private string database = "pixel_universe";
    private string user = "root";
    private string password = "root";
    private string connectionString;
    private MySqlConnection conn;

    void Start()
    {
        connectionString = $"Server={server};Database={database};User ID={user};Password={password};";
        conn = new MySqlConnection(connectionString);
        AbrirConexion();

        List<string> usuarios = ObtenerUsuariosAsignados(0);

        foreach (string usuario in usuarios)
        {
            Debug.Log(usuario);
        }
    }

    private void AbrirConexion()
    {
        if (conn.State != ConnectionState.Open)
        {
            try
            {
                conn.Open();
                Debug.Log("Conexi�n abierta exitosamente con la base de datos local.");
            }
            catch (MySqlException ex)
            {
                Debug.LogError($"Error al abrir la conexi�n: C�digo {ex.Number}, Mensaje: {ex.Message}");
            }
        }
    }

    public void Reconectar()
    {
        // Si la conexi�n no est� abierta, intenta reabrirla
        if (conn == null || conn.State != ConnectionState.Open)
        {
            Debug.LogError("La conexi�n no est� abierta. Reintentando abrir...");
            AbrirConexion();
        }
    }

    void OnDestroy()
    {
        if (conn != null && conn.State == ConnectionState.Open)
        {
            conn.Close();
            conn.Dispose();
            Debug.Log("Conexi�n a la base de datos cerrada y recursos liberados.");
        }
    }

    List<string> ObtenerUsuariosAsignados(int masterId)
    {
        List<string> usuarios = new List<string>();
        AbrirConexion();

        try
        {
            string query = "SELECT id_usuario_jugador FROM jugador WHERE id_master_actual = @masterId;";
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@masterId", masterId);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        usuarios.Add(reader.GetString("id_usuario_jugador"));
                    }
                }
            }
        }
        catch (MySqlException ex)
        {
            Debug.LogError($"Error al obtener usuarios asignados: {ex.Message}");
        }
        

        return usuarios;
    }
}

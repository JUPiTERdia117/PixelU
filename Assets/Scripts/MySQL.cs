using System;
using System.Data;
using System.IO;
using MySql.Data.MySqlClient;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    }

    private void AbrirConexion()
    {
        if (conn.State != ConnectionState.Open)
        {
            try
            {
                conn.Open();
                Debug.Log("Conexión abierta exitosamente con la base de datos local.");
            }
            catch (MySqlException ex)
            {
                Debug.LogError($"Error al abrir la conexión: Código {ex.Number}, Mensaje: {ex.Message}");
            }
        }
    }

    public void Reconectar()
    {
        // Si la conexión no está abierta, intenta reabrirla
        if (conn == null || conn.State != ConnectionState.Open)
        {
            Debug.LogError("La conexión no está abierta. Reintentando abrir...");
            AbrirConexion();
        }
    }

    void OnDestroy()
    {
        if (conn != null && conn.State == ConnectionState.Open)
        {
            conn.Close();
            conn.Dispose();
            Debug.Log("Conexión a la base de datos cerrada y recursos liberados.");
        }
    }
}

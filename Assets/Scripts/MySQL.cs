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
    List<string> usuariosA;

    void Start()
    {
        connectionString = $"Server={server};Database={database};User ID={user};Password={password};";
        conn = new MySqlConnection(connectionString);
        AbrirConexion();

        usuariosA = ObtenerUsuariosAsignados(0);

        foreach (string usuario in usuariosA)
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

    public void ActualizarPuntajes(int scoreRed, int scoreGreen, int scoreBlue, int scoreYellow, int scorePink, int scorePurple, int scoreOrange, int scoreCyan)
    {
        Dictionary<string, int> colorScores = new Dictionary<string, int>
        {
            { "Rojo", scoreRed },
            { "Verde", scoreGreen },
            { "Azul", scoreBlue },
            { "Amarillo", scoreYellow },
            { "Rosa", scorePink },
            { "Morado", scorePurple },
            { "Naranja", scoreOrange },
            { "Cyan", scoreCyan }
        };

        foreach (var colorScore in colorScores)
        {
            try
            {
                string query = "UPDATE jugador SET `Pts_Juego1-New` = @score WHERE Color = @color;";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@score", colorScore.Value);
                    cmd.Parameters.AddWithValue("@color", colorScore.Key);
                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        Debug.Log($"Puntaje actualizado para el color {colorScore.Key}: {colorScore.Value}");
                    }
                    else
                    {
                        Debug.LogWarning($"No se encontró ningún jugador con el color {colorScore.Key}");
                    }
                }

                // Comparar y actualizar Pts_Juego1-Record si es necesario
                string querySelect = "SELECT `Pts_Juego1-Record` FROM jugador WHERE Color = @color;";
                using (MySqlCommand cmdSelect = new MySqlCommand(querySelect, conn))
                {
                    cmdSelect.Parameters.AddWithValue("@color", colorScore.Key);
                    object result = cmdSelect.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        int currentRecord = Convert.ToInt32(result);
                        if (colorScore.Value > currentRecord)
                        {
                            string queryUpdateRecord = "UPDATE jugador SET `Pts_Juego1-Record` = @score WHERE Color = @color;";
                            using (MySqlCommand cmdUpdateRecord = new MySqlCommand(queryUpdateRecord, conn))
                            {
                                cmdUpdateRecord.Parameters.AddWithValue("@score", colorScore.Value);
                                cmdUpdateRecord.Parameters.AddWithValue("@color", colorScore.Key);
                                cmdUpdateRecord.ExecuteNonQuery();
                                Debug.Log($"Nuevo record para el color {colorScore.Key}: {colorScore.Value}");
                            }
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                Debug.LogError($"Error al actualizar puntajes para el color {colorScore.Key}: {ex.Message}");
            }
        }
    }
}

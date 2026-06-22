using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using ChatBotWPF.Models;

namespace ChatBotWPF.Services
{
    public class DatabaseService
    {
        private readonly string connectionString =
            @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=CyberBotDB;Integrated Security=True;";

        public bool TestConnection()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public void InitializeDatabase()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string createTablesQuery = @"
                        IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Tasks' AND xtype='U')
                        BEGIN
                            CREATE TABLE Tasks (
                                Id INT IDENTITY(1,1) PRIMARY KEY,
                                Title NVARCHAR(200) NOT NULL,
                                Description NVARCHAR(MAX) NULL,
                                ReminderDate DATETIME NULL,
                                IsCompleted BIT NOT NULL DEFAULT 0
                            );
                        END
                        
                        IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='ChatMessages' AND xtype='U')
                        BEGIN
                            CREATE TABLE ChatMessages (
                                Id INT IDENTITY(1,1) PRIMARY KEY,
                                UserMessage NVARCHAR(MAX) NOT NULL,
                                BotMessage NVARCHAR(MAX) NOT NULL,
                                Timestamp DATETIME NOT NULL
                            );
                        END";

                    using (SqlCommand cmd = new SqlCommand(createTablesQuery, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Database initialization failed: {ex.Message}");
            }
        }

        // =========================
        // TASK FUNCTIONS
        // =========================

        public void AddTask(TaskItem task)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO Tasks
                                (Title, Description, ReminderDate, IsCompleted)
                                VALUES
                                (@Title, @Description, @ReminderDate, @IsCompleted)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Title", task.Title);
                    cmd.Parameters.AddWithValue("@Description",
                        string.IsNullOrEmpty(task.Description) ? DBNull.Value : (object)task.Description);

                    // Properly handle nullable DateTime
                    if (task.ReminderDate.HasValue)
                        cmd.Parameters.AddWithValue("@ReminderDate", task.ReminderDate.Value);
                    else
                        cmd.Parameters.AddWithValue("@ReminderDate", DBNull.Value);

                    cmd.Parameters.AddWithValue("@IsCompleted", task.IsCompleted);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<TaskItem> GetTasks()
        {
            List<TaskItem> tasks = new List<TaskItem>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Tasks ORDER BY IsCompleted, Id DESC";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            TaskItem task = new TaskItem();
                            task.Id = (int)reader["Id"];
                            task.Title = reader["Title"].ToString();
                            task.Description = reader["Description"] == DBNull.Value ? null : reader["Description"].ToString();

                            // Handle nullable datetime from database
                            if (reader["ReminderDate"] == DBNull.Value)
                                task.ReminderDate = null;
                            else
                                task.ReminderDate = (DateTime)reader["ReminderDate"];

                            task.IsCompleted = (bool)reader["IsCompleted"];
                            tasks.Add(task);
                        }
                    }
                }
            }

            return tasks;
        }

        public void MarkTaskComplete(int taskId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "UPDATE Tasks SET IsCompleted = 1 WHERE Id = @Id";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", taskId);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteTask(int taskId)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM Tasks WHERE Id = @Id";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", taskId);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // =========================
        // CHAT FUNCTIONS
        // =========================

        public void SaveChat(string userMessage, string botMessage)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO ChatMessages
                                (UserMessage, BotMessage, Timestamp)
                                VALUES
                                (@UserMessage, @BotMessage, @Timestamp)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserMessage", userMessage ?? "");
                    cmd.Parameters.AddWithValue("@BotMessage", botMessage ?? "");
                    cmd.Parameters.AddWithValue("@Timestamp", DateTime.Now);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<ChatMessage> GetChatHistory()
        {
            List<ChatMessage> chats = new List<ChatMessage>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM ChatMessages ORDER BY Timestamp DESC";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ChatMessage chat = new ChatMessage();
                            chat.Id = (int)reader["Id"];
                            chat.UserMessage = reader["UserMessage"].ToString();
                            chat.BotMessage = reader["BotMessage"].ToString();
                            chat.Timestamp = (DateTime)reader["Timestamp"];
                            chats.Add(chat);
                        }
                    }
                }
            }

            return chats;
        }
    }
}
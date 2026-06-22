using System;
using System.Collections.Generic;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using ChatBotWPF.Services;
using ChatBotWPF.Models;

namespace ChatBotWPF
{
    public partial class MainWindow : Window
    {
        private readonly Random random = new Random();
        private readonly DatabaseService _db = new DatabaseService();
        private readonly NLPService _nlp = new NLPService();
        private readonly QuizService _quiz = new QuizService();

        private string userName = "User";
        private bool quizActive = false;
        private int currentQuestionIndex = 0;
        private List<ActivityLog> activityLog = new List<ActivityLog>();

        // =========================
        // KNOWLEDGE BASE
        // =========================
        private readonly Dictionary<string, List<string>> responses = new Dictionary<string, List<string>>();

        private void InitializeResponses()
        {
            responses.Add("password", new List<string>
            {
                "Use strong passwords with 12+ characters, numbers, and symbols.",
                "Never reuse passwords across different accounts.",
                "Avoid using personal information in passwords.",
                "Enable 2FA for an extra layer of security."
            });

            responses.Add("phishing", new List<string>
            {
                "Don't click suspicious links in emails or messages.",
                "Always verify the sender's email address carefully.",
                "Never share OTPs or personal information online.",
                "Report phishing attempts to your IT department."
            });

            responses.Add("privacy", new List<string>
            {
                "Limit personal information shared on social media.",
                "Enable two-factor authentication on all accounts.",
                "Review app permissions regularly.",
                "Use a VPN when using public Wi-Fi."
            });

            responses.Add("safe browsing", new List<string>
            {
                "Only use HTTPS websites for sensitive information.",
                "Avoid downloading files from unknown sources.",
                "Keep your browser and extensions updated.",
                "Use ad-blockers to prevent malicious ads."
            });

            responses.Add("2fa", new List<string>
            {
                "Two-factor authentication adds an extra security layer.",
                "Use authenticator apps instead of SMS when possible.",
                "Backup your 2FA recovery codes safely.",
                "Enable 2FA on all accounts that support it."
            });

            responses.Add("malware", new List<string>
            {
                "Install and update antivirus software regularly.",
                "Don't download software from untrusted sources.",
                "Be careful with email attachments.",
                "Keep your operating system updated."
            });
        }

        // =========================
        // INIT
        // =========================
        public MainWindow()
        {
            InitializeComponent();
            InitializeResponses();

            try
            {
                if (!_db.TestConnection())
                {
                    _db.InitializeDatabase();
                    AddBotMessage("Database initialized successfully!");
                }
                else
                {
                    AddBotMessage("Database connected!");
                }
            }
            catch (Exception ex)
            {
                AddBotMessage($"Database error: {ex.Message}");
            }

            PlayGreeting();
            LoadTasks();
            LoadChatHistory();
            LoadRecentActivity();

            AddBotMessage("Welcome to Cybersecurity Awareness Bot!");
            AddBotMessage("Type your name to begin, or try these commands:");
            AddBotMessage("'Help' - Show available commands");
            AddBotMessage("'Start quiz' - Test your cybersecurity knowledge");
            AddBotMessage("'Add task: [title]' - Add a new task");
            AddBotMessage("'Show log' - View recent activity");
            AddBotMessage("Type your name to begin.");
        }

        // =========================
        // INPUT EVENTS
        // =========================
        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            HandleUserInput();
        }

        private void UserInputBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                HandleUserInput();
        }

        private void HandleUserInput()
        {
            string input = UserInputBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(input))
            {
                AddBotMessage("Please enter a message.");
                return;
            }

            AddUserMessage(input);
            UserInputBox.Clear();

            if (userName == "User")
            {
                userName = input;
                AddBotMessage($"Nice to meet you {userName}!");
                AddBotMessage("How can I help you today?");
                LogActivity($"User joined: {userName}");
                return;
            }

            string nlpResponse = _nlp.ProcessInput(input);
            if (!string.IsNullOrEmpty(nlpResponse))
            {
                AddBotMessage(nlpResponse);
                LogActivity($"NLP processed: {input}");
                return;
            }

            RespondToUser(input.ToLower());
        }

        // =========================
        // CHATBOT LOGIC
        // =========================
        private void RespondToUser(string input)
        {
            if (input.Contains("start quiz") || input.Contains("play quiz"))
            {
                StartQuiz();
                return;
            }

            if (input.Contains("show log") || input.Contains("activity log") ||
                input.Contains("what have you done") || input.Contains("summary"))
            {
                ShowActivityLog();
                return;
            }

            if (input.Contains("help") || input.Contains("commands"))
            {
                ShowHelp();
                return;
            }

            if (input.Contains("task") || input.Contains("remind") || input.Contains("reminder"))
            {
                HandleTaskCommand(input);
                return;
            }

            if (input.Contains("password"))
                SendResponse("password", input);
            else if (input.Contains("phishing"))
                SendResponse("phishing", input);
            else if (input.Contains("privacy"))
                SendResponse("privacy", input);
            else if (input.Contains("safe") || input.Contains("browsing"))
                SendResponse("safe browsing", input);
            else if (input.Contains("2fa") || input.Contains("two factor"))
                SendResponse("2fa", input);
            else if (input.Contains("malware") || input.Contains("virus"))
                SendResponse("malware", input);
            else if (input.Contains("how are you"))
                SendSimpleResponse("I'm running perfectly and ready to help!");
            else if (input.Contains("exit") || input.Contains("bye"))
                SendSimpleResponse($"Goodbye {userName}! Stay safe online!");
            else if (input.Contains("thank"))
                SendSimpleResponse("You're welcome! Stay secure!");
            else
                SendSimpleResponse("I'm here to help with cybersecurity topics. Try asking about passwords, phishing, privacy, or 2FA. Type 'Help' for commands.");
        }

        private void HandleTaskCommand(string input)
        {
            string taskTitle = _nlp.ExtractTaskFromInput(input);

            if (!string.IsNullOrEmpty(taskTitle))
            {
                var task = new TaskItem
                {
                    Title = taskTitle,
                    Description = $"Added via chat: {input}",
                    IsCompleted = false
                };

                _db.AddTask(task);
                LoadTasks();
                AddBotMessage($"Task added: '{taskTitle}'");
                LogActivity($"Task added via chat: {taskTitle}");

                if (input.Contains("remind") || input.Contains("reminder"))
                {
                    AddBotMessage("Reminder will be set for this task.");
                }
            }
            else
            {
                AddBotMessage("I can help with tasks. Try saying: 'Add task: Review my passwords'");
            }
        }

        private void SendResponse(string topic, string userInput)
        {
            string response = GetRandomResponse(topic);
            AddBotMessage(response);
            _db.SaveChat(userInput, response);
            LogActivity($"Sent response for topic: {topic}");
        }

        private void SendSimpleResponse(string message)
        {
            AddBotMessage(message);
            _db.SaveChat("", message);
        }

        private string GetRandomResponse(string topic)
        {
            if (!responses.ContainsKey(topic))
                return "No info available on that topic.";

            var list = responses[topic];
            return list[random.Next(list.Count)];
        }

        private void ShowHelp()
        {
            AddBotMessage("Available Commands:");
            AddBotMessage("'Start quiz' - Play cybersecurity quiz");
            AddBotMessage("'Show log' - View recent activity");
            AddBotMessage("'Add task: [title]' - Add a new task");
            AddBotMessage("Ask about: passwords, phishing, privacy, 2FA, malware");
            AddBotMessage("'Help' - Show this menu");
            AddBotMessage("'Exit' - End the conversation");
            LogActivity("User viewed help");
        }

        // =========================
        // TASK SYSTEM
        // =========================
        private void LoadTasks()
        {
            try
            {
                TasksListBox.Items.Clear();
                var tasks = _db.GetTasks();

                foreach (var task in tasks)
                {
                    string description = string.IsNullOrEmpty(task.Description)
                        ? "No description"
                        : task.Description.Length > 30 ? task.Description.Substring(0, 30) + "..." : task.Description;

                    string status = task.IsCompleted ? "Done" : "Pending";
                    TasksListBox.Items.Add($"{task.Id} | {task.Title} | {status}");
                }
            }
            catch (Exception ex)
            {
                AddActivity($"Error loading tasks: {ex.Message}");
            }
        }

        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(TaskTitleBox.Text))
                {
                    AddActivity("Please enter a task title.");
                    return;
                }

                var task = new TaskItem
                {
                    Title = TaskTitleBox.Text.Trim(),
                    Description = TaskDescriptionBox.Text?.Trim(),
                    ReminderDate = ReminderDatePicker.SelectedDate,
                    IsCompleted = false
                };

                _db.AddTask(task);
                LoadTasks();

                string reminderMsg = task.ReminderDate.HasValue ?
                    $" with reminder on {task.ReminderDate.Value.ToShortDateString()}" : "";
                AddActivity($"Task added: {task.Title}{reminderMsg}");
                LogActivity($"Task added: {task.Title}");

                TaskTitleBox.Clear();
                TaskDescriptionBox.Clear();
                ReminderDatePicker.SelectedDate = null;

                AddBotMessage($"Task '{task.Title}' added successfully!{reminderMsg}");
            }
            catch (Exception ex)
            {
                AddActivity($"Error adding task: {ex.Message}");
            }
        }

        private void CompleteTask_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (TasksListBox.SelectedItem == null)
                {
                    AddActivity("Select a task first.");
                    return;
                }

                string selectedItem = TasksListBox.SelectedItem.ToString();
                int id = int.Parse(selectedItem.Split('|')[0].Trim());

                _db.MarkTaskComplete(id);
                LoadTasks();
                AddActivity("Task completed");
                LogActivity($"Task {id} marked as complete");
                AddBotMessage("Task marked as complete!");
            }
            catch (Exception ex)
            {
                AddActivity($"Error completing task: {ex.Message}");
            }
        }

        private void DeleteTask_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (TasksListBox.SelectedItem == null)
                {
                    AddActivity("Select a task first.");
                    return;
                }

                string selectedItem = TasksListBox.SelectedItem.ToString();
                int id = int.Parse(selectedItem.Split('|')[0].Trim());

                _db.DeleteTask(id);
                LoadTasks();
                AddActivity("Task deleted");
                LogActivity($"Task {id} deleted");
                AddBotMessage("Task deleted.");
            }
            catch (Exception ex)
            {
                AddActivity($"Error deleting task: {ex.Message}");
            }
        }

        // =========================
        // QUIZ SYSTEM
        // =========================
        private void StartQuiz_Click(object sender, RoutedEventArgs e)
        {
            StartQuiz();
        }

        private void StartQuiz()
        {
            if (quizActive)
            {
                AddBotMessage("A quiz is already in progress!");
                return;
            }

            quizActive = true;
            currentQuestionIndex = 0;
            _quiz.ResetScore();

            AddBotMessage("Starting Cybersecurity Quiz!");
            AddBotMessage("You'll be asked questions. Let's begin!");
            LogActivity("Quiz started");

            ShowQuestion();
            NextQuestionButton.Visibility = Visibility.Visible;
        }

        private void ShowQuestion()
        {
            if (currentQuestionIndex >= _quiz.Questions.Count)
            {
                EndQuiz();
                return;
            }

            var question = _quiz.Questions[currentQuestionIndex];
            QuizQuestionDisplay.Text = $"Q{currentQuestionIndex + 1}: {question.QuestionText}";
            UpdateQuizScore();

            QuizOptionsPanel.Children.Clear();

            for (int i = 0; i < question.Options.Count; i++)
            {
                var button = new Button
                {
                    Content = question.Options[i],
                    Height = 30,
                    Margin = new Thickness(0, 3, 0, 0),
                    Background = new SolidColorBrush(Color.FromRgb(0x2A, 0x3A, 0x4A)),
                    Foreground = new SolidColorBrush(Colors.White),
                    FontWeight = FontWeights.Normal,
                    Tag = i
                };
                button.Click += QuizOption_Click;
                QuizOptionsPanel.Children.Add(button);
            }

            NextQuestionButton.Visibility = Visibility.Collapsed;
            QuizQuestionDisplay.Visibility = Visibility.Visible;
        }

        private void QuizOption_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            int selectedIndex = (int)button.Tag;
            var question = _quiz.Questions[currentQuestionIndex];

            bool isCorrect = selectedIndex == question.CorrectIndex;

            if (isCorrect)
            {
                _quiz.Score++;
                AddBotMessage($"Correct! {question.Explanation}");
                LogActivity($"Quiz: Correct answer for Q{currentQuestionIndex + 1}");
            }
            else
            {
                AddBotMessage($"Incorrect. {question.Explanation}");
                LogActivity($"Quiz: Incorrect answer for Q{currentQuestionIndex + 1}");
            }

            UpdateQuizScore();

            foreach (var child in QuizOptionsPanel.Children)
            {
                if (child is Button btn)
                    btn.IsEnabled = false;
            }

            currentQuestionIndex++;
            NextQuestionButton.Visibility = Visibility.Visible;
        }

        private void NextQuestion_Click(object sender, RoutedEventArgs e)
        {
            if (_quiz.IsComplete())
            {
                EndQuiz();
            }
            else
            {
                ShowQuestion();
                foreach (var child in QuizOptionsPanel.Children)
                {
                    if (child is Button btn)
                        btn.IsEnabled = true;
                }
            }
        }

        private void EndQuiz()
        {
            quizActive = false;
            NextQuestionButton.Visibility = Visibility.Collapsed;
            QuizQuestionDisplay.Text = "";

            int total = _quiz.Questions.Count;
            int score = _quiz.Score;
            double percentage = (double)score / total * 100;

            string feedback;
            if (percentage >= 90)
                feedback = "Excellent! You're a cybersecurity pro!";
            else if (percentage >= 70)
                feedback = "Good job! Keep learning to stay safe online!";
            else if (percentage >= 50)
                feedback = "Not bad! Review the topics to improve your score.";
            else
                feedback = "Keep practicing! Cybersecurity is important for everyone.";

            AddBotMessage($"Quiz Complete! Score: {score}/{total} ({percentage:F1}%)");
            AddBotMessage(feedback);
            LogActivity($"Quiz completed. Score: {score}/{total}");

            QuizScoreDisplay.Text = $"Final Score: {score}/{total}";
        }

        private void UpdateQuizScore()
        {
            QuizScoreDisplay.Text = $"Score: {_quiz.Score}/{_quiz.Questions.Count}";
        }

        // =========================
        // ACTIVITY LOG
        // =========================
        private void ShowActivityLog_Click(object sender, RoutedEventArgs e)
        {
            ShowActivityLog();
        }

        private void ShowActivityLog()
        {
            if (activityLog.Count == 0)
            {
                AddBotMessage("No activity recorded yet.");
                return;
            }

            AddBotMessage("Recent Activity Log:");
            int count = Math.Min(activityLog.Count, 10);

            for (int i = activityLog.Count - count; i < activityLog.Count; i++)
            {
                var log = activityLog[i];
                AddBotMessage($"* {log.Timestamp:HH:mm} - {log.Action}");
            }

            LogActivity("User viewed activity log");
        }

        private void LogActivity(string action)
        {
            activityLog.Add(new ActivityLog
            {
                Timestamp = DateTime.Now,
                Action = action
            });

            if (activityLog.Count > 100)
                activityLog.RemoveAt(0);

            LoadRecentActivity();
        }

        private void LoadRecentActivity()
        {
            ActivityLogListBox.Items.Clear();
            int count = Math.Min(activityLog.Count, 10);

            for (int i = activityLog.Count - count; i < activityLog.Count; i++)
            {
                var log = activityLog[i];
                ActivityLogListBox.Items.Add($"{log.Timestamp:HH:mm} - {log.Action}");
            }
        }

        // =========================
        // CHAT HISTORY
        // =========================
        private void LoadChatHistory()
        {
            try
            {
                var chats = _db.GetChatHistory();
                foreach (var chat in chats)
                {
                    AddMessage($"You: {chat.UserMessage}\nBot: {chat.BotMessage}", Brushes.Gray);
                }
            }
            catch (Exception ex)
            {
                AddActivity($"Error loading chat history: {ex.Message}");
            }
        }

        // =========================
        // UI HELPERS
        // =========================
        private void AddUserMessage(string message)
        {
            AddMessage($"You: {message}", Brushes.LightSkyBlue);
        }

        private void AddBotMessage(string message)
        {
            AddMessage($"Bot: {message}", Brushes.White);
        }

        private void AddMessage(string message, Brush color)
        {
            Paragraph p = new Paragraph();
            Run run = new Run(message);
            run.Foreground = color;
            p.Inlines.Add(run);
            ChatBox.Document.Blocks.Add(p);
            ChatBox.ScrollToEnd();
        }

        private void AddActivity(string message)
        {
            ActivityLogListBox.Items.Add($"{DateTime.Now:HH:mm} - {message}");
        }

        // =========================
        // SOUND
        // =========================
        private void PlayGreeting()
        {
            try
            {
                SoundPlayer player = new SoundPlayer("welcome.wav.wav");
                player.Play();
            }
            catch
            {
                // Sound not available, continue
            }
        }
    }
}
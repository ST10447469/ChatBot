using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace ChatBotWPF.Services
{
    public class NLPService
    {
        private readonly Dictionary<string, List<string>> intentPatterns = new Dictionary<string, List<string>>();

        public NLPService()
        {
            InitializePatterns();
        }

        private void InitializePatterns()
        {
            intentPatterns.Add("add_task", new List<string>
            {
                "add task", "create task", "new task", "make task",
                "add reminder", "set reminder", "remind me", "task:",
                "add a task", "create a task", "add to-do", "create to-do",
                "add a reminder", "set a reminder", "add todo", "create todo"
            });

            intentPatterns.Add("view_tasks", new List<string>
            {
                "show tasks", "view tasks", "list tasks", "my tasks",
                "what are my tasks", "show my tasks", "display tasks",
                "show me my tasks", "view my tasks"
            });

            intentPatterns.Add("quiz", new List<string>
            {
                "start quiz", "play quiz", "quiz", "take quiz",
                "start game", "play game", "start the quiz", "take the quiz"
            });

            intentPatterns.Add("activity_log", new List<string>
            {
                "show log", "activity log", "what have you done",
                "view log", "recent activity", "show activity",
                "display log", "summary", "show activity log",
                "what have you done for me"
            });

            intentPatterns.Add("help", new List<string>
            {
                "help", "commands", "what can you do",
                "how to use", "help me", "options", "menu"
            });

            intentPatterns.Add("goodbye", new List<string>
            {
                "exit", "bye", "goodbye", "see you", "quit", "end"
            });

            intentPatterns.Add("thank_you", new List<string>
            {
                "thank", "thanks", "thank you", "appreciate it", "good bot"
            });
        }

        public string ProcessInput(string input)
        {
            string lowerInput = input.ToLower().Trim();

            // Check for task creation
            if (ContainsAny(lowerInput, intentPatterns["add_task"]))
            {
                string taskTitle = ExtractTaskFromInput(input);
                if (!string.IsNullOrEmpty(taskTitle))
                {
                    return $" I've added: '{taskTitle}'. The task has been saved to your database!";
                }
                return "I'll help you add a task. Just say 'Add task: [your task title]' or 'Remind me to [task]'";
            }

            // Check for viewing tasks
            if (ContainsAny(lowerInput, intentPatterns["view_tasks"]))
            {
                return "I'll show your tasks in the task list on the right panel. You can also click the 'Add Task' button to create new ones.";
            }

            // Check for quiz
            if (ContainsAny(lowerInput, intentPatterns["quiz"]))
            {
                return " Starting the cybersecurity quiz! Click the 'Start Quiz' button or wait for the first question to appear.";
            }

            // Check for activity log
            if (ContainsAny(lowerInput, intentPatterns["activity_log"]))
            {
                return " Viewing recent activity log... Check the Activity Log section on the right panel for the latest actions.";
            }

            // Check for help
            if (ContainsAny(lowerInput, intentPatterns["help"]))
            {
                return " Here's what I can do:\n" +
                       "• Add and manage cybersecurity tasks\n" +
                       "• Test your knowledge with the Cybersecurity Quiz\n" +
                       "• Answer questions about cybersecurity topics\n" +
                       "• Show your activity log\n" +
                       "• Provide cybersecurity tips and advice\n\n" +
                       "Try asking: 'Add task: Enable 2FA' or 'Start quiz' or 'Show log'";
            }

            // Check for goodbye
            if (ContainsAny(lowerInput, intentPatterns["goodbye"]))
            {
                return " Goodbye! Remember to stay safe online! ";
            }

            // Check for thank you
            if (ContainsAny(lowerInput, intentPatterns["thank_you"]))
            {
                return "You're welcome! Stay secure and keep learning about cybersecurity! ";
            }

            // Check for cybersecurity topics
            if (ContainsAny(lowerInput, new List<string> { "password", "passwords" }))
            {
                return GetRandomResponse(new List<string>
                {
                    " Passwords: Use strong, unique passwords with a mix of characters. Never reuse passwords across accounts!",
                    " A strong password should be at least 12 characters long and include uppercase, lowercase, numbers, and symbols.",
                    " Consider using a password manager to generate and store strong passwords securely.",
                    "Enable Two-Factor Authentication (2FA) on all accounts that support it for extra security."
                });
            }

            if (ContainsAny(lowerInput, new List<string> { "phishing", "phish", "scam" }))
            {
                return GetRandomResponse(new List<string>
                {
                    " Phishing: Never click suspicious links or share personal information. Always verify the sender's email address!",
                    " Be cautious of emails asking for urgent action or personal information. Legitimate companies never ask for passwords via email.",
                    " Check for spelling errors and suspicious email addresses - these are common signs of phishing attempts.",
                    " If you receive a suspicious email, report it to your IT department and delete it immediately."
                });
            }

            if (ContainsAny(lowerInput, new List<string> { "2fa", "two factor", "multi-factor", "mfa" }))
            {
                return GetRandomResponse(new List<string>
                {
                    " Two-Factor Authentication adds an extra security layer. Always enable 2FA on your important accounts!",
                    " Use authenticator apps like Google Authenticator or Microsoft Authenticator instead of SMS for 2FA.",
                    " Backup your 2FA recovery codes in a safe place in case you lose access to your authenticator app.",
                    " 2FA significantly reduces the risk of account compromise even if your password is stolen."
                });
            }

            if (ContainsAny(lowerInput, new List<string> { "malware", "virus", "ransomware", "trojan" }))
            {
                return GetRandomResponse(new List<string>
                {
                    " Protect against malware: Install antivirus software, keep it updated, and avoid suspicious downloads!",
                    " Regular software updates patch security vulnerabilities that malware exploits.",
                    " Be careful with email attachments and downloads from untrusted sources.",
                    " Use Windows Defender or a reputable antivirus and run regular scans."
                });
            }

            if (ContainsAny(lowerInput, new List<string> { "privacy", "private", "data" }))
            {
                return GetRandomResponse(new List<string>
                {
                    " Protect your privacy: Limit personal information shared on social media and review privacy settings regularly.",
                    " Use a VPN when using public Wi-Fi to encrypt your internet traffic.",
                    " Review app permissions and remove access to apps you no longer use.",
                    " Be careful about what you share online - once it's out there, it's hard to take back."
                });
            }

            if (ContainsAny(lowerInput, new List<string> { "browsing", "browser", "safe browsing", "https" }))
            {
                return GetRandomResponse(new List<string>
                {
                    " Safe browsing: Always check for 'https://' and the padlock icon in the address bar.",
                    " Avoid clicking on suspicious ads or pop-ups while browsing.",
                    " Keep your browser and extensions updated to protect against security vulnerabilities.",
                    " Use privacy-focused browsers or extensions to block trackers and malicious content."
                });
            }

            if (ContainsAny(lowerInput, new List<string> { "update", "updates", "patch" }))
            {
                return "Regular updates are crucial for security! Always install the latest updates for your operating system, applications, and antivirus software. Updates patch security vulnerabilities that hackers exploit.";
            }

            if (ContainsAny(lowerInput, new List<string> { "wi-fi", "wifi", "wireless", "public" }))
            {
                return " Public Wi-Fi is risky! Always use a VPN on public networks, avoid accessing sensitive information, and make sure websites use HTTPS. Consider using your phone's hotspot for sensitive activities.";
            }

            return null;
        }

        private bool ContainsAny(string input, List<string> patterns)
        {
            foreach (string pattern in patterns)
            {
                if (input.Contains(pattern))
                    return true;
            }
            return false;
        }

        public string ExtractTaskFromInput(string input)
        {
            string lowerInput = input.ToLower();

            // Check for "task:" pattern
            if (input.Contains("task:"))
            {
                int startIndex = input.IndexOf("task:") + 5;
                string task = input.Substring(startIndex).Trim();
                if (!string.IsNullOrEmpty(task))
                    return CapitalizeFirstLetter(task);
            }

            // Check for "add task" pattern
            if (lowerInput.Contains("add task"))
            {
                int startIndex = input.IndexOf("add task", StringComparison.OrdinalIgnoreCase) + 8;
                string task = input.Substring(startIndex).Trim();
                if (!string.IsNullOrEmpty(task))
                    return CapitalizeFirstLetter(task);
            }

            // Check for "remind me" pattern
            if (lowerInput.Contains("remind me") || lowerInput.Contains("reminder"))
            {
                // Look for "to", "about", "for" to extract the task
                string[] connectors = { " to ", " about ", " for " };
                foreach (string connector in connectors)
                {
                    if (lowerInput.Contains(connector))
                    {
                        int startIndex = lowerInput.IndexOf(connector) + connector.Length;
                        string task = input.Substring(startIndex).Trim();
                        if (!string.IsNullOrEmpty(task) && !task.Contains("day") && !task.Contains("week"))
                            return CapitalizeFirstLetter(task);
                    }
                }

                // If no connector found, try to extract after the keywords
                string[] keywords = { "remind me to", "remind me about", "reminder for", "reminder to" };
                foreach (string keyword in keywords)
                {
                    if (lowerInput.Contains(keyword))
                    {
                        int startIndex = lowerInput.IndexOf(keyword) + keyword.Length;
                        string task = input.Substring(startIndex).Trim();
                        if (!string.IsNullOrEmpty(task))
                            return CapitalizeFirstLetter(task);
                    }
                }
            }

            // Generic extraction - take everything after "to" or "for"
            if (lowerInput.Contains(" to "))
            {
                int startIndex = lowerInput.IndexOf(" to ") + 4;
                string task = input.Substring(startIndex).Trim();
                if (!string.IsNullOrEmpty(task))
                    return CapitalizeFirstLetter(task);
            }

            return null;
        }

        private string GetRandomResponse(List<string> responses)
        {
            Random random = new Random();
            return responses[random.Next(responses.Count)];
        }

        private string CapitalizeFirstLetter(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            text = text.Trim();

            // Remove common starting words
            string[] removeWords = { "the ", "a ", "an " };
            foreach (string word in removeWords)
            {
                if (text.ToLower().StartsWith(word))
                {
                    text = text.Substring(word.Length);
                    break;
                }
            }

            if (text.Length > 0)
            {
                text = char.ToUpper(text[0]) + text.Substring(1);
            }
            return text;
        }
    }
}

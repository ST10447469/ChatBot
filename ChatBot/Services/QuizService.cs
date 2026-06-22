using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ChatBotWPF.Services
{
    public class QuizService
    {
        public List<QuizQuestion> Questions { get; private set; }
        public int Score { get; set; }
        private int currentQuestionIndex = 0;

        public QuizService()
        {
            Questions = new List<QuizQuestion>();
            InitializeQuestions();
            Score = 0;
        }

        private void InitializeQuestions()
        {
            Questions.Add(new QuizQuestion
            {
                QuestionText = "What is phishing?",
                Options = new List<string>
                {
                    "A type of fishing technique",
                    "A scam to steal personal information",
                    "A secure way to send emails",
                    "A type of antivirus software"
                },
                CorrectIndex = 1,
                Explanation = "Phishing is a cyber attack where scammers trick you into revealing personal information like passwords or credit card details."
            });

            Questions.Add(new QuizQuestion
            {
                QuestionText = "What should you do if you receive an email asking for your password?",
                Options = new List<string>
                {
                    "Reply with your password",
                    "Delete the email and ignore it",
                    "Report it as phishing",
                    "Forward it to your friends"
                },
                CorrectIndex = 2,
                Explanation = "Never share your password via email. Report phishing attempts to your IT department or email provider."
            });

            Questions.Add(new QuizQuestion
            {
                QuestionText = "A strong password should contain:",
                Options = new List<string>
                {
                    "Only lowercase letters",
                    "Your birthday",
                    "A mix of uppercase, lowercase, numbers, and symbols",
                    "Only numbers"
                },
                CorrectIndex = 2,
                Explanation = "Strong passwords use a combination of uppercase, lowercase, numbers, and special characters for better security."
            });

            Questions.Add(new QuizQuestion
            {
                QuestionText = "What is two-factor authentication (2FA)?",
                Options = new List<string>
                {
                    "A second password",
                    "A security method requiring two verification steps",
                    "A type of firewall",
                    "A password manager"
                },
                CorrectIndex = 1,
                Explanation = "2FA adds an extra layer of security by requiring two different forms of verification to access your account."
            });

            Questions.Add(new QuizQuestion
            {
                QuestionText = "True or False: It's safe to use the same password for multiple accounts.",
                Options = new List<string>
                {
                    "True",
                    "False"
                },
                CorrectIndex = 1,
                Explanation = "Never reuse passwords across accounts. If one account is compromised, all your accounts become vulnerable."
            });

            Questions.Add(new QuizQuestion
            {
                QuestionText = "What does HTTPS stand for?",
                Options = new List<string>
                {
                    "HyperText Transfer Protocol Secure",
                    "High Tech Transfer Protocol",
                    "Hyper Transfer Secure",
                    "Home Transfer Protocol System"
                },
                CorrectIndex = 0,
                Explanation = "HTTPS ensures encrypted communication between your browser and the website, protecting your data from interception."
            });

            Questions.Add(new QuizQuestion
            {
                QuestionText = "What is social engineering?",
                Options = new List<string>
                {
                    "A type of engineering job",
                    "Building social networks",
                    "Manipulating people to reveal confidential information",
                    "A programming language"
                },
                CorrectIndex = 2,
                Explanation = "Social engineering uses psychological manipulation to trick people into giving up sensitive information or access."
            });

            Questions.Add(new QuizQuestion
            {
                QuestionText = "What is the best way to protect your personal information online?",
                Options = new List<string>
                {
                    "Share it on social media",
                    "Use strong privacy settings and be careful what you share",
                    "Post it on public forums",
                    "Give it to anyone who asks"
                },
                CorrectIndex = 1,
                Explanation = "Limit what you share online and regularly review your privacy settings to protect your personal information."
            });

            Questions.Add(new QuizQuestion
            {
                QuestionText = "What should you do if you receive a suspicious attachment?",
                Options = new List<string>
                {
                    "Open it to check what it is",
                    "Delete the email without opening the attachment",
                    "Forward it to your friends",
                    "Save it to your computer first"
                },
                CorrectIndex = 1,
                Explanation = "Never open suspicious attachments. They may contain malware. Delete the email and report it."
            });

            Questions.Add(new QuizQuestion
            {
                QuestionText = "What is ransomware?",
                Options = new List<string>
                {
                    "A type of antivirus software",
                    "Malware that encrypts your files and demands payment",
                    "A secure backup system",
                    "A password recovery tool"
                },
                CorrectIndex = 1,
                Explanation = "Ransomware is malicious software that locks you out of your files and demands payment to restore access."
            });

            Questions.Add(new QuizQuestion
            {
                QuestionText = "True or False: Public Wi-Fi networks are always safe to use.",
                Options = new List<string>
                {
                    "True",
                    "False"
                },
                CorrectIndex = 1,
                Explanation = "Public Wi-Fi networks are often unsecured and can be easily exploited by hackers. Always use a VPN on public networks."
            });

            Questions.Add(new QuizQuestion
            {
                QuestionText = "What is the best way to create a memorable but strong password?",
                Options = new List<string>
                {
                    "Use a long passphrase with random words",
                    "Use your pet's name",
                    "Use your birthdate",
                    "Use 'password123'"
                },
                CorrectIndex = 0,
                Explanation = "A passphrase like 'BlueSky!Coffee$42' is both strong and memorable. Combine random words with symbols and numbers."
            });

            Questions.Add(new QuizQuestion
            {
                QuestionText = "What should you do before downloading software?",
                Options = new List<string>
                {
                    "Check if it's from a trusted source",
                    "Download it quickly",
                    "Disable your antivirus",
                    "Share the download link"
                },
                CorrectIndex = 0,
                Explanation = "Always download software from official or trusted sources to avoid malware and security risks."
            });

            Questions.Add(new QuizQuestion
            {
                QuestionText = "What is a VPN used for?",
                Options = new List<string>
                {
                    "To speed up your internet",
                    "To encrypt your internet connection and protect privacy",
                    "To download files faster",
                    "To create passwords"
                },
                CorrectIndex = 1,
                Explanation = "A VPN (Virtual Private Network) encrypts your internet traffic and hides your IP address for better privacy and security."
            });

            Questions.Add(new QuizQuestion
            {
                QuestionText = "True or False: You should regularly update your software and operating system.",
                Options = new List<string>
                {
                    "True",
                    "False"
                },
                CorrectIndex = 0,
                Explanation = "Regular updates patch security vulnerabilities and protect your system from known threats."
            });
        }

        public QuizQuestion GetNextQuestion()
        {
            if (currentQuestionIndex < Questions.Count)
            {
                return Questions[currentQuestionIndex];
            }
            return null;
        }

        public void Reset()
        {
            Score = 0;
            currentQuestionIndex = 0;
        }

        public void ResetScore()
        {
            Score = 0;
        }

        public void MoveToNextQuestion()
        {
            currentQuestionIndex++;
        }

        public bool IsComplete()
        {
            return currentQuestionIndex >= Questions.Count;
        }
    }

    public class QuizQuestion
    {
        public string QuestionText { get; set; }
        public List<string> Options { get; set; }
        public int CorrectIndex { get; set; }
        public string Explanation { get; set; }
    }
}

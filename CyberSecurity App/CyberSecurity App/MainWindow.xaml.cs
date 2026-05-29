using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using WpfApp1;

namespace CyberSecurity_App
{
    public partial class MainWindow : Window
    {
        private SpeechSynthesizer? synthesizer;
        private bool isVoiceEnabled = false; // Start with voice disabled to avoid errors
        private bool hasUserName = false;
        private string userName = "";
        private string currentTopic = "";
        private Random random = new Random();

        // Memory storage
        private Dictionary<string, string> userMemory = new Dictionary<string, string>();

        private void PlayWelcomeMessage()
        {
            // New ASCII Art
            string asciiArt = @"
╔══════════════════════════════════════════════════════════════════════════════════════╗
║                                                                                      ║
║     ██████╗ ██╗   ██╗██████╗ ███████╗██████╗     ███████╗███████╗██╗   ██╗██╗  ██╗   ║
║    ██╔════╝╚██╗ ██╔╝██╔══██╗██╔════╝██╔══██╗    ██╔════╝██╔════╝╚██╗ ██╔╝██║  ██║   ║
║    ██║      ╚████╔╝ ██████╔╝█████╗  ██████╔╝    ███████╗█████╗   ╚████╔╝ ███████║   ║
║    ██║       ╚██╔╝  ██╔══██╗██╔══╝  ██╔══██╗    ╚════██║██╔══╝    ╚██╔╝  ██╔══██║   ║
║    ╚██████╗   ██║   ██████╔╝███████╗██║  ██║    ███████║███████╗   ██║   ██║  ██║   ║
║     ╚═════╝   ╚═╝   ╚═════╝ ╚══════╝╚═╝  ╚═╝    ╚══════╝╚══════╝   ╚═╝   ╚═╝  ╚═╝   ║
║                                                                                      ║
║                    🔒   C Y B E R   S E C U R I T Y   B O T   🔒                     ║
║                                                                                      ║
║                        'Your Digital Safety Guardian'                                ║
║                                                                                      ║
╚══════════════════════════════════════════════════════════════════════════════════════╝

   [ SYSTEM ONLINE ]
   > Security protocols active
   > Firewall engaged
   > Ready to assist";

        }

        private void AppendToChat(string v, string asciiArt, object cyan)
        {
            throw new NotImplementedException();
        }

        // Bot responses
        private Dictionary<string, string[]> responses = new Dictionary<string, string[]>()
        {
            { "password", new string[] {
                "🔐 Use strong passwords with uppercase, lowercase, numbers, and symbols!",
                "🔑 Never reuse passwords across different websites!",
                "💡 Create passwords that are at least 12 characters long.",
                "⚠️ Enable two-factor authentication for extra protection!"
            }},
            { "phishing", new string[] {
                "🎣 Never click suspicious links in emails or messages!",
                "📧 Always check the sender's email address carefully.",
                "🔍 Hover over links to see the real URL before clicking.",
                "⚠️ Legitimate companies never ask for passwords via email!"
            }},
            { "malware", new string[] {
                "🦠 Keep your antivirus software updated at all times!",
                "💿 Don't download attachments from unknown senders.",
                "🚫 Avoid clicking on pop-up ads or suspicious links.",
                "🔄 Back up your important files regularly!"
            }},
            { "scam", new string[] {
                "🚨 Never trust calls asking for personal information!",
                "⚠️ Scammers create urgency - 'Your account will be closed!'",
                "💰 If it sounds too good to be true, it's a scam!",
                "📞 Hang up and call official numbers directly."
            }},
            { "privacy", new string[] {
                "🛡️ Don't share personal information publicly online!",
                "📱 Review app permissions on your phone regularly.",
                "🔒 Use privacy settings on all social media accounts.",
                "🌐 Use a VPN when on public Wi-Fi networks."
            }},
            { "2fa", new string[] {
                "🔐 Two-factor authentication adds an extra security layer!",
                "📱 Use authenticator apps instead of SMS when possible.",
                "✅ Enable 2FA on email, banking, and social media.",
                "🔑 Save backup codes in a safe place!"
            }},
            { "safe browsing", new string[] {
                "🌐 Look for 'https://' and the padlock icon!",
                "🔒 Avoid using public Wi-Fi for banking.",
                "📱 Keep your browser updated to the latest version.",
                "🛡️ Use ad-blockers for safer browsing."
            }}
        };

        private string[] topics = { "password", "phishing", "malware", "scam", "privacy", "2fa", "safe browsing" };
        private bool voiceEnabled;

        public MainWindow()
        {
            InitializeComponent();
            InitializeSpeechSafely();
            ShowWelcomeMessage();
        }

        private void InitializeSpeechSafely()
        {
            try
            {
                // Check if running on Windows and speech is available
                if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                {
                    synthesizer = new SpeechSynthesizer();
                    isVoiceEnabled = true;
                    btnVoice.Content = "🔊";
                }
                else
                {
                    isVoiceEnabled = false;
                    btnVoice.Content = "🔇";
                }
            }
            catch (Exception ex)
            {
                // Speech not available - silently disable
                System.Diagnostics.Debug.WriteLine($"Speech not available: {ex.Message}");
                isVoiceEnabled = false;
                btnVoice.Content = "🔇";
                synthesizer = null;
            }
        }

        private void ShowWelcomeMessage()
        {
            string welcome = "👋 Hello! I'm your Cybersecurity Assistant.\n\nI can help you with:\n• Password safety\n• Phishing detection\n• Malware protection\n• Scam detection\n• Privacy protection\n• Two-factor authentication\n• Safe browsing\n\nWhat's your name?";
            AddBotMessage(welcome);
        }

        private void AddBotMessage(string message)
        {
            Dispatcher.Invoke(() =>
            {
                var bubble = CreateMessageBubble(message, false);
                chatPanel.Children.Add(bubble);
                ScrollToBottom();
                SaveToHistory("Bot", message);

                // Safely speak the message
                if (isVoiceEnabled && synthesizer != null)
                {
                    try
                    {
                        synthesizer.SpeakAsync(message);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Speech failed: {ex.Message}");
                        // Disable voice on error to prevent further issues
                        isVoiceEnabled = false;
                        btnVoice.Content = "🔇";
                    }
                }
            });
        }

        private void AddUserMessage(string message)
        {
            Dispatcher.Invoke(() =>
            {
                var bubble = CreateMessageBubble(message, true);
                chatPanel.Children.Add(bubble);
                ScrollToBottom();
                SaveToHistory("User", message);
            });
        }

        private Border CreateMessageBubble(string text, bool isUser)
        {
            var outerBorder = new Border
            {
                Margin = new Thickness(isUser ? 40 : 10, 5, isUser ? 10 : 40, 5),
                HorizontalAlignment = isUser ? HorizontalAlignment.Right : HorizontalAlignment.Left,
                MaxWidth = 280
            };

            var innerBorder = new Border
            {
                CornerRadius = new CornerRadius(15),
                Background = isUser ? new SolidColorBrush(Color.FromRgb(220, 248, 198)) : new SolidColorBrush(Color.FromRgb(255, 255, 255)),
                Padding = new Thickness(12, 8, 12, 8)
            };

            var stack = new StackPanel();

            var textBlock = new TextBlock
            {
                Text = text,
                Foreground = new SolidColorBrush(Color.FromRgb(17, 27, 33)),
                FontSize = 14,
                TextWrapping = TextWrapping.Wrap
            };

            var timeBlock = new TextBlock
            {
                Text = DateTime.Now.ToString("HH:mm"),
                FontSize = 9,
                Foreground = new SolidColorBrush(Color.FromRgb(102, 119, 129)),
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(0, 5, 0, 0)
            };

            stack.Children.Add(textBlock);
            stack.Children.Add(timeBlock);
            innerBorder.Child = stack;
            outerBorder.Child = innerBorder;

            return outerBorder;
        }

        private void AddTypingIndicator()
        {
            Dispatcher.Invoke(() =>
            {
                var outerBorder = new Border
                {
                    Margin = new Thickness(10, 5, 40, 5),
                    HorizontalAlignment = HorizontalAlignment.Left
                };

                var innerBorder = new Border
                {
                    CornerRadius = new CornerRadius(15),
                    Background = new SolidColorBrush(Color.FromRgb(230, 230, 230)),
                    Padding = new Thickness(12, 8, 12, 8)
                };

                var textBlock = new TextBlock
                {
                    Text = "Bot is typing...",
                    Foreground = new SolidColorBrush(Color.FromRgb(102, 102, 102)),
                    FontSize = 12,
                    FontStyle = FontStyles.Italic
                };

                innerBorder.Child = textBlock;
                outerBorder.Child = innerBorder;
                outerBorder.Tag = "typing";
                chatPanel.Children.Add(outerBorder);
                ScrollToBottom();
            });
        }

        private void RemoveTypingIndicator()
        {
            Dispatcher.Invoke(() =>
            {
                for (int i = chatPanel.Children.Count - 1; i >= 0; i--)
                {
                    var item = chatPanel.Children[i] as Border;
                    if (item != null && item.Tag?.ToString() == "typing")
                    {
                        chatPanel.Children.RemoveAt(i);
                    }
                }
            });
        }

        private async Task ShowTypingAndProcess(Func<Task> action)
        {
            AddTypingIndicator();
            await Task.Delay(800);
            RemoveTypingIndicator();
            await action();
        }

        private async void SendMessage_Click(object sender, RoutedEventArgs e)
        {
            await ProcessUserMessage();
        }

        private async void MessageBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                await ProcessUserMessage();
            }
        }

        private async Task ProcessUserMessage()
        {
            string message = messageBox.Text.Trim();
            if (string.IsNullOrWhiteSpace(message))
                return;

            AddUserMessage(message);
            messageBox.Clear();

            await ShowTypingAndProcess(() =>
            {
                ProcessResponse(message);
                return Task.CompletedTask;
            });
        }

        private void ProcessResponse(string message)
        {
            string lowerMsg = message.ToLower();

            // Get user name
            if (!hasUserName)
            {
                userName = message;
                userMemory["name"] = userName;
                hasUserName = true;
                statusText.Text = $"● Online - {userName}";
                AddBotMessage($"Nice to meet you, {userName}! 👋\n\nHow can I help you with cybersecurity today?");
                return;
            }

            // Check what is my name
            if (lowerMsg.Contains("what is my name"))
            {
                AddBotMessage($"Your name is {userName}! I remember you! 😊");
                return;
            }

            // Check for exit
            if (lowerMsg.Contains("bye") || lowerMsg.Contains("goodbye") || lowerMsg.Contains("exit"))
            {
                AddBotMessage($"Goodbye {userName}! Stay safe online! 👋");
                return;
            }

            // Check for help
            if (lowerMsg.Contains("help") || lowerMsg.Contains("what can you do"))
            {
                ShowHelpMenu();
                return;
            }

            // Check for follow-up
            if (lowerMsg.Contains("another") || lowerMsg.Contains("more") || lowerMsg.Contains("tell me more"))
            {
                if (!string.IsNullOrEmpty(currentTopic) && responses.ContainsKey(currentTopic))
                {
                    var tips = responses[currentTopic];
                    string tip = tips[random.Next(tips.Length)];
                    AddBotMessage($"📚 Another tip about {currentTopic}:\n\n{tip}");
                }
                else
                {
                    AddBotMessage("Please ask about a specific topic first!\n\nTry: 'Tell me about passwords'");
                }
                return;
            }

            // Check for sentiment
            if (lowerMsg.Contains("worried") || lowerMsg.Contains("scared") || lowerMsg.Contains("nervous"))
            {
                AddBotMessage("😟 I understand your concern. Cybersecurity can feel overwhelming, but I'm here to help you step by step!");
                return;
            }

            if (lowerMsg.Contains("frustrated") || lowerMsg.Contains("confused"))
            {
                AddBotMessage("😤 I hear your frustration. Let me make this simpler for you!");
                return;
            }

            if (lowerMsg.Contains("thank") || lowerMsg.Contains("thanks"))
            {
                AddBotMessage("🙏 You're very welcome! Stay safe online!");
                return;
            }

            // Check for keywords
            bool found = false;
            foreach (string topic in topics)
            {
                if (lowerMsg.Contains(topic))
                {
                    currentTopic = topic;
                    var tips = responses[topic];
                    string tip = tips[random.Next(tips.Length)];
                    string topicDisplay = topic == "2fa" ? "2FA" : topic.ToUpper();
                    AddBotMessage($"🔒 Here's what I know about {topicDisplay}:\n\n{tip}\n\nWould you like another tip?");
                    found = true;
                    break;
                }
            }

            // Default response
            if (!found)
            {
                string[] defaultResponses = {
                    "🤔 I'm not sure I understand.\n\nTry asking about: passwords, phishing, malware, scams, or privacy.",
                    "💭 I specialize in cybersecurity topics.\n\nType 'help' to see what I can do!",
                    "🔄 Try asking: 'Tell me about passwords' or 'What is phishing?'",
                    "📚 I can help with online safety! What would you like to know?"
                };
                AddBotMessage(defaultResponses[random.Next(defaultResponses.Length)]);
            }
        }

        private void ShowHelpMenu()
        {
            string help = "📖 I can help with these topics:\n\n" +
                         "🔐 Password Safety\n" +
                         "🎣 Phishing Detection\n" +
                         "🦠 Malware Protection\n" +
                         "🚨 Scam Detection\n" +
                         "🔒 Privacy Protection\n" +
                         "🔑 Two-Factor Authentication\n" +
                         "🌐 Safe Browsing\n\n" +
                         "💡 Just ask me about any topic!\n" +
                         "💡 Say 'another tip' for more info";

            AddBotMessage(help);
        }

        private void ShowQuickMenu_Click(object sender, RoutedEventArgs e)
        {
            string menu = "⚡ Quick Topics:\n\n" +
                         "• password\n" +
                         "• phishing\n" +
                         "• malware\n" +
                         "• scam\n" +
                         "• privacy\n" +
                         "• 2fa\n" +
                         "• safe browsing\n\n" +
                         "Type any topic to learn more!";

            AddBotMessage(menu);
        }

        private void ToggleVoice_Click(object sender, RoutedEventArgs e)
        {
            if (synthesizer == null)
            {
                AddBotMessage("⚠️ Voice is not available on this system.");
                return;
            }

            isVoiceEnabled = !isVoiceEnabled;
            btnVoice.Content = isVoiceEnabled ? "🔊" : "🔇";
            AddBotMessage(isVoiceEnabled ? "🔊 Voice responses enabled!" : "🔇 Voice responses disabled!");
        }

        private void ClearChat_Click(object sender, RoutedEventArgs e)
        {
            chatPanel.Children.Clear();
            AddBotMessage("Chat cleared! How can I help you?");
        }

        private void ScrollToBottom()
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                chatScrollViewer.ScrollToBottom();
            }));
        }

        private void SaveToHistory(string sender, string message)
        {
            try
            {
                string logDir = "ChatHistory";
                if (!Directory.Exists(logDir))
                    Directory.CreateDirectory(logDir);

                string logFile = Path.Combine(logDir, $"{DateTime.Now:yyyy-MM-dd}.txt");
                string logEntry = $"{DateTime.Now:HH:mm:ss} [{sender}] {message}";
                File.AppendAllText(logFile, logEntry + Environment.NewLine);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to save: {ex.Message}");
            }
        }
    }
}
namespace ActivityPulse.Application
{
    public static class Helpers
    {
        public static readonly Dictionary<string, string> KnownProcessNames = new()
        {
            { "explorer.exe", "File Explorer" },
            { "chrome.exe", "Google Chrome" },
            { "firefox.exe", "Mozilla Firefox" },
            { "notepad.exe", "Notepad" },
            { "wordpad.exe", "WordPad" },
            { "mspaint.exe", "Paint" },
            { "calc.exe", "Calculator" },
            { "cmd.exe", "Command Prompt" },
            { "powershell.exe", "Windows PowerShell" },
            { "msedge.exe", "Microsoft Edge" },
            { "devenv", "Visual Studio" },
            { "code.exe", "Visual Studio Code" },
            { "vlc.exe", "VLC Media Player" },
            { "spotify.exe", "Spotify" },
            { "skype.exe", "Skype" },
            { "teams.exe", "Microsoft Teams" },
            { "zoom.exe", "Zoom" },
            { "slack.exe", "Slack" },
            { "discord.exe", "Discord" },
            { "gitkraken.exe", "GitKraken" },
            { "postman.exe", "Post" }
        };

        public static readonly Dictionary<int, string> Quotes = new()
        {
            { 1, "\"Believe you can and you're halfway there.\" — Theodore Roosevelt" },
            { 2, "\"Success is not final, failure is not fatal: It is the courage to continue that counts.\" — Winston Churchill" },
            { 3, "\"Do what you can, with what you have, where you are.\" — Theodore Roosevelt" },
            { 4, "\"Don’t watch the clock; do what it does. Keep going.\" — Sam Levenson" },
            { 5, "\"The only limit to our realization of tomorrow is our doubts of today.\" — Franklin D. Roosevelt" },
            { 6, "\"Your time is limited, so don’t waste it living someone else’s life.\" — Steve Jobs" },
            { 7, "\"Great things are done by a series of small things brought together.\" — Vincent van Gogh" },
            { 8, "\"You miss 100% of the shots you don’t take.\" — Wayne Gretzky" },
            { 9, "\"It always seems impossible until it’s done.\" — Nelson Mandela" },
            { 10, "\"Push yourself, because no one else is going to do it for you.\" — Unknown" },
            { 11, "\"Life is not fair — get used to it.\" — Bill Gates" }
        };

        public static string GetDisplayName(string processName)
        {
            if (Helpers.KnownProcessNames.TryGetValue(processName, out var displayName))
            {
                return displayName;
            }
            return processName;
        }

        public static string GetRandomQuote()
        {
            var random = new Random();
            var index = random.Next(1, Quotes.Count + 1);
            return Quotes[index];
        }

        public static string FormatDisplayTime(double minutes)
        {
            if (minutes <= 0)
                return "0m";
            if (minutes < 1)
                return $"{minutes:F1}m";
            if (minutes < 60)
                return $"{(int)minutes}m";
            double hours = minutes / 60;
            return hours % 1 == 0
                ? $"{(int)hours}h"
                : $"{hours:F1}h";
        }
    }
}


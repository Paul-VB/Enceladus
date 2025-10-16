using Enceladus.Core.Utils;
using System.Xml.Linq;

namespace Enceladus.Core.Config
{
    public interface IConfigService
    {
        Config Config { get; }
        void Load();
    }
    public class ConfigService : IConfigService
    {
        private readonly string _configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config.xml");
        private readonly string _defaultConfigPath = "Enceladus.Core.Config.ConfigDefault.xml";

        private Config _config;
        public Config Config => _config;

        public ConfigService()
        {
            Load();
        }

        public void Load()
        {
            var defaultXml = LoadDefaultXml();

            //if no config file found, use defaults and write out a default file
            if (!File.Exists(_configPath))
            {
                _config = XmlHelper.Deserialize<Config>(defaultXml);
                XmlHelper.SerializeToFile(_config, _configPath);
                return;
            }

            try
            {
                var userXml = XDocument.Load(_configPath);
                var mergedXml = XmlHelper.Merge(defaultXml, userXml);

                _config = XmlHelper.Deserialize<Config>(mergedXml);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Warning: Failed to load config ({ex.Message}). Using defaults.");
                _config = XmlHelper.Deserialize<Config>(defaultXml);
            }
        }

        private XDocument LoadDefaultXml()
        {
            try
            {
                var assembly = typeof(ConfigService).Assembly;

                using (var stream = assembly.GetManifestResourceStream(_defaultConfigPath))
                {
                    if (stream == null)
                        throw new FileNotFoundException($"Embedded resource not found: {_defaultConfigPath}");

                    return XDocument.Load(stream);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"fatal: Failed to load default config ({ex.Message}).");
                throw;
            }
        }
    }
}

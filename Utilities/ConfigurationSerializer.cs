using System.IO;
using Newtonsoft.Json;

namespace Utilities
{
    public class ConfigurationSerializer<TConfig>
    {
        private readonly string _fileName;
        private readonly JsonSerializer _serializer;

        public ConfigurationSerializer(string fileName, TConfig configuration)
        {
            _fileName = fileName;
            _serializer = new JsonSerializer
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.Indented                
            };
            if (!File.Exists(_fileName))
            {
                Serialize(configuration);
            }
        }

                public ConfigurationSerializer(string fileName)
        {
            _fileName = fileName;
            _serializer = new JsonSerializer
            {
                NullValueHandling = NullValueHandling.Include,
                Formatting = Formatting.Indented                
            };
        }
        public void Serialize(TConfig configuration)
        {                        
            using (var streamWriter = new StreamWriter(_fileName))
            {
                _serializer.NullValueHandling = NullValueHandling.Ignore;
                _serializer.Serialize(streamWriter, configuration);
            }        
        }

        public TConfig Deserialize()
        {

            using (var streamReader = new StreamReader(_fileName))
            {
                return (TConfig)_serializer.Deserialize(streamReader, typeof(TConfig));
            } 
        }

    }
}

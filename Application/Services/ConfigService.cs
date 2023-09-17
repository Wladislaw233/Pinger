using System.Text;
using Models;
using Models.ProtocolConfigs;
using Newtonsoft.Json;
using Services.Interfaces;

namespace Services;

public class ConfigService : IConfigService
{
    private readonly string _configPath;
    private readonly string _configFullPath;

    public ConfigService(string configPath, string configFileName)
    {
        _configPath = configPath ??
                      throw new ArgumentException("The configuration file folder is incorrect.", nameof(configPath));

        _configFullPath = Path.Combine(configPath, configFileName ??
                                                   throw new ArgumentException("The file name is incorrect.",
                                                       nameof(configFileName)));
    }

    public Config GetConfig()
    {
        var directoryInfo = new DirectoryInfo(_configPath);

        if (!directoryInfo.Exists || !File.Exists(_configFullPath))
            return CreateDefaultConfigFile();

        using var fileStream = new FileStream(_configFullPath, FileMode.Open);

        var jsonTextBytesArray = new byte[fileStream.Length];

        // ReSharper disable once MustUseReturnValue
        fileStream.Read(jsonTextBytesArray, 0, jsonTextBytesArray.Length);

        var jsonText = Encoding.Default.GetString(jsonTextBytesArray);

        var config = JsonConvert.DeserializeObject<Config>(jsonText);

        return config ?? throw new InvalidCastException("Failed to read the configuration file.");
    }

    private Config CreateDefaultConfigFile()
    {
        var config = CreateDefaultConfigObject();

        var directoryInfo = new DirectoryInfo(_configPath);

        if (!directoryInfo.Exists)
            directoryInfo.Create();

        using var fileStream = new FileStream(_configFullPath, FileMode.OpenOrCreate);

        var jsonTextBytesArray = Encoding.Default.GetBytes(JsonConvert.SerializeObject(config));

        fileStream.Write(jsonTextBytesArray, 0, jsonTextBytesArray.Length);

        return config;
    }

    private static Config CreateDefaultConfigObject()
    {
        var httpConfigs = new List<HttpConfig>
        {
            new()
            {
                HostUrl = "https://www.google.com",
                StatusCode = 200,
                Period = 60000
            },
            new()
            {
                HostUrl = "https://www.yandex.ru",
                StatusCode = 200,
                Period = 10000
            }
        };

        var icmpConfigs = new List<IcmpConfig>
        {
            new()
            {
                HostUrl = "google.com",
                Period = 15000,
                Timeout = 10
            },
            new()
            {
                HostUrl = "yandex.ru",
                Period = 20000,
                Timeout = 10
            }
        };

        var tcpConfigs = new List<TcpConfig>
        {
            new()
            {
                HostUrl = "google.com",
                Period = 30000,
                Port = 80
            },
            new()
            {
                HostUrl = "yandex.ru",
                Period = 45000,
                Port = 80
            }
        };

        var config = new Config
        {
            HttpConfigs = httpConfigs,
            IcmpConfigs = icmpConfigs,
            TcpConfigs = tcpConfigs
        };

        return config;
    }
}
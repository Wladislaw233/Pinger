using System.Text;
using Models;
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
        if (directoryInfo.Exists || File.Exists(_configFullPath))
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
        var httpConfig = new HttpConfig
        {
            Url = "https://www.google.com",
            StatusCode = 200
        };

        var icmpConfig = new IcmpConfig
        {
            Host = "google.com",
            Timeout = 10
        };

        var tcpConfig = new TcpConfig
        {
            Host = "google.com",
            Port = 80,
            Timeout = 10
        };

        var config = new Config
        {
            Http = httpConfig,
            Icmp = icmpConfig,
            Tcp = tcpConfig
        };

        return config;
    }
}
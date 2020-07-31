using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Agile.Config.Client;
using Agile.Config.Protocol;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AgileConfigMVCSample
{
    public class Program
    {
        public static IConfigClient ConfigClient;

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, config) =>
            {
                //��ȡ��������
                var localconfig = new ConfigurationBuilder()
                                 .SetBasePath(Directory.GetCurrentDirectory())
                                 .AddJsonFile("appsettings.json").Build();
                //�ӱ����������ȡAgileConfig�������Ϣ
                var appId = localconfig["AgileConfig:appId"];
                var secret = localconfig["AgileConfig:secret"];
                var nodes = localconfig["AgileConfig:nodes"];
                //newһ��clientʵ��
                var configClient = new ConfigClient(appId, secret, nodes);
                //ʹ��AddAgileConfig����һ���µ�IConfigurationSource
                config.AddAgileConfig(configClient);
                //��һ����������clientʵ�����Ա������ط�����ֱ��ʹ��ʵ����������
                ConfigClient = configClient;
                //ע���������޸��¼�
                configClient.ConfigChanged += ConfigClient_ConfigChanged;
            })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        /// <summary>
        /// ���¼�����������Ŀ�����������޸ġ�ɾ����ʱ�򴥷�
        /// </summary>
        private static void ConfigClient_ConfigChanged(ConfigChangedArg obj)
        {
            Console.WriteLine($"action:{obj.Action} key:{obj.Key}");

            switch (obj.Action)
            {
                case ActionConst.Add:
                    break;
                case ActionConst.Update:
                    break;
                case ActionConst.Remove:
                    break;
                default:
                    break;
            }
        }
    }
}

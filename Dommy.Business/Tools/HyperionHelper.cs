namespace Dommy.Business.Tools
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net.Http;

    using Dommy.Business.Configs;

    using Newtonsoft.Json;

    using Ninject;

    public class HyperionHelper
    {
        private string uri;

        public HyperionHelper(string uri)
        {
            this.uri = uri;
        }

        public async void Effect(string effectName, int duration)
        {
            var client = new HttpClient();

            var c = JsonConvert.SerializeObject(
                new HyperionCommand()
            {
                Command = "effect",
                Effect = new HyperionEffect() { Name = effectName },
                Duration = duration,
                Priority = 100,
            },
                Formatting.None);

            var content = new StringContent(string.Format("{0}\n", c));

            try
            {
                await client.PostAsync(this.uri, content);
            }
            catch (Exception)
            {
                
            }
        }

        public class Config : IConfig
        {
            public void Create(IKernel kernel)
            {
                kernel.Bind<HyperionHelper>().ToSelf().WithConstructorArgument("uri", this.Uri);
            }

            public string Uri { get; set; }
        }
    }

    public class HyperionEffect
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class HyperionCommand
    {
        [JsonProperty("command")]
        public string Command { get; set; }

        [JsonProperty("effect")]
        public HyperionEffect Effect { get; set; }

        [JsonProperty("duration")]
        public int Duration { get; set; }

        [JsonProperty("priority")]
        public int Priority { get; set; }
    }
}
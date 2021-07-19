using Microsoft.Extensions.DependencyInjection;
using Netmentor.DiContainer;
using SocialGiveaway.Services.Twitter;
using SocialGiveaway.Services.YouTube;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialGiveaway.Services
{
    public class ServicesDI
    {

        public static readonly DiModule DiModule = BuildDependencyInjection();
        private static DiModule BuildDependencyInjection()
        {
            var module = new DiModule(typeof(ServicesDI).Assembly);
            return module
               .AddScoped<SelectTwitterWinner>()
               .AddScoped<SelectYouTubeWinner>();
        }

    }
}

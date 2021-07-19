using Microsoft.AspNetCore.Mvc;
using SocialGiveaway.Dto.YouTube;
using SocialGiveaway.Services.YouTube;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ROP.APIExtensions;

namespace SocialGiveaway.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Consumes("application/json")]
    public class YoutubeController : Controller
    {
        private readonly SelectYouTubeWinner _youTubeWinner;

        public YoutubeController(SelectYouTubeWinner twitterWinner)
        {
            _youTubeWinner = twitterWinner;
        }

        [HttpPost]
        public async Task<IActionResult> GetWinner(string videoId, List<YouTubeTicketDto> youtubeTickets)
        {
            return await _youTubeWinner
                .Execute(videoId, youtubeTickets)
                .ToActionResult();
        }
    }
}

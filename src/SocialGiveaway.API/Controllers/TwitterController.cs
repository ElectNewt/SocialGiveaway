using Microsoft.AspNetCore.Mvc;
using ROP.APIExtensions;
using SocialGiveaway.Services.Twitter;
using System.Collections.Generic;
using System.Threading.Tasks;
using SocialGiveaway.Dto.Twitter;

namespace SocialGiveAway.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Consumes("application/json")]
    public class TwitterController : Controller
    {
        private readonly SelectWinner _twitterWinner;

        public TwitterController(SelectWinner twitterWinner)
        {
            _twitterWinner = twitterWinner;
        }

        [HttpPost]
        public async Task<IActionResult> GetWinner(long tweetId, List<TweetTicketDto> tweetTickets)
        {
            return await _twitterWinner
                .Execute(tweetId, tweetTickets)
                .ToActionResult();
        }
    }
}

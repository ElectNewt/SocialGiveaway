using Microsoft.AspNetCore.Mvc;
using ROP.APIExtensions;
using SocialGiveaway.Services.Twitter;
using SocialGiveAway.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialGiveAway.API.Controllers
{
    public class TwitterController : Controller
    {
        private readonly SelectWinner _TwitterWinner;
        public async Task<IActionResult> GetWinner(int tweetId, List<TweetTicketDto> tweetTickets)
        {
            return await _TwitterWinner
                .Execute(tweetId, tweetTickets)
                .ToActionResult();

        }
    }
}

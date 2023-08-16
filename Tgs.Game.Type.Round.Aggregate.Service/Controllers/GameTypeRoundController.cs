using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Mvc;
using Tgs.Game.Round.Aggregate.Service;
using Tgs.MessageQueue;

namespace Tgs.Game.Type.Round.Aggregate.Service.Controllers;

[Route("api/[controller]")]
[ApiController]
public class GameTypeRoundController : ControllerBase
{
    private readonly IMessageSender _messageSender;

    public GameTypeRoundController(IMessageSender messageSender)
    {
        _messageSender = messageSender;
    }

    [HttpPost("{from:DateTime}/{to:DateTime}")]
    public async Task GenerateGameTypeRoundsAsync(DateTime from, DateTime to)
    {
        var request = new GameTypeRoundRequest
        {
            From = Timestamp.FromDateTime(from),
            To = Timestamp.FromDateTime(to)
        };

        await _messageSender.PublishAsync(
            "tgs.gametyperound.generate",
            request.ToByteArray(),
            HttpContext.RequestAborted).ConfigureAwait(false);
    }
}
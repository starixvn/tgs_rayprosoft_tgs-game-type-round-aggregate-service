using NSubstitute;
using Tgs.Game.Type.Round.Aggregate.Core;

namespace Tgs.Game.Type.Round.Aggregate.UnitTest;

public class GameTypeRoundTest
{
    [Test]
    public async Task Create_Game_Type_Round_When_Generated()
    {
        var fakeTgsRepository = Substitute.For<ITgsRepository>();

        var sut = new CreateGameTypeRoundAggregateHandler(
            fakeTgsRepository);

        fakeTgsRepository.ExecutionTimeIsExistsAsync(Arg.Any<AggregateType>(), Arg.Any<DateTime>(), Arg.Any<DateTime>())
            .Returns(true);

        var request = new CreateGameRoundAggregateRequest(DateTime.Parse("2023/08/01 00:00:00Z"), DateTime.Parse("2023/08/01 00:15:00Z"));

        await sut.Handle(request, default);

        await fakeTgsRepository.Received(0).GameTypeRoundsExistsAsync(Arg.Any<DateTime>(), Arg.Any<DateTime>());
        await fakeTgsRepository.Received(0).GenerateGameTypeRoundsAggregateAsync(Arg.Any<DateTime>(), Arg.Any<DateTime>());
        await fakeTgsRepository.Received(0).CreateExecutionTimeAsync(Arg.Any<AggregateType>(), Arg.Any<DateTime>(), Arg.Any<DateTime>());
    }

    [Test]
    public async Task Create_Game_Type_Round()
    {
        var fakeTgsRepository = Substitute.For<ITgsRepository>();

        var sut = new CreateGameTypeRoundAggregateHandler(
            fakeTgsRepository);

        fakeTgsRepository.ExecutionTimeIsExistsAsync(Arg.Any<AggregateType>(), Arg.Any<DateTime>(), Arg.Any<DateTime>())
            .Returns(false);

        fakeTgsRepository.GameTypeRoundsExistsAsync(Arg.Any<DateTime>(), Arg.Any<DateTime>())
            .Returns(false);

        var request = new CreateGameRoundAggregateRequest(DateTime.Parse("2023/08/01 00:00:00Z"), DateTime.Parse("2023/08/01 00:15:00Z"));

        await sut.Handle(request, default);

        await fakeTgsRepository.Received(1).GenerateGameTypeRoundsAggregateAsync(Arg.Any<DateTime>(), Arg.Any<DateTime>());
        await fakeTgsRepository.Received(1).CreateExecutionTimeAsync(Arg.Any<AggregateType>(), Arg.Any<DateTime>(), Arg.Any<DateTime>());
    }

    [Test]
    public async Task Create_Game_Type_Round_When_Data_Is_Existed()
    {
        var fakeTgsRepository = Substitute.For<ITgsRepository>();

        var sut = new CreateGameTypeRoundAggregateHandler(
            fakeTgsRepository);

        fakeTgsRepository.ExecutionTimeIsExistsAsync(Arg.Any<AggregateType>(), Arg.Any<DateTime>(), Arg.Any<DateTime>())
            .Returns(false);

        fakeTgsRepository.GameTypeRoundsExistsAsync(Arg.Any<DateTime>(), Arg.Any<DateTime>())
            .Returns(true);

        var request = new CreateGameRoundAggregateRequest(DateTime.Parse("2023/08/01 00:00:00Z"), DateTime.Parse("2023/08/01 00:15:00Z"));

        await sut.Handle(request, default);

        await fakeTgsRepository.Received(0).GenerateGameTypeRoundsAggregateAsync(Arg.Any<DateTime>(), Arg.Any<DateTime>());
        await fakeTgsRepository.Received(1).CreateExecutionTimeAsync(Arg.Any<AggregateType>(), Arg.Any<DateTime>(), Arg.Any<DateTime>());
    }
}
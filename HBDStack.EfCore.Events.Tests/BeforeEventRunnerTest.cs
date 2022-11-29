using System;
using System.Linq;
using System.Threading.Tasks;
using DDD4Tests.Domains;
using DDD4Tests.Domains.University;
using DDD4Tests.Events;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace HBDStack.EfCore.Events.Tests;

public class BeforeEventRunnerTest : IClassFixture<EventRunnerFixture>
{
    private readonly EventRunnerFixture _provider;

    public BeforeEventRunnerTest(EventRunnerFixture provider) => _provider = provider;

    [Fact]
    public async Task AddEntity_BeforeEvent_ShouldRaised()
    {
        BeforeAddedEventTestHandler.ReturnFailureResult = false;
        BeforeAddedEventTestHandler.AsyncCalled = false;
        //BeforeEventTestHandler.Called = false;

        var root = _provider.Context.Set<Root>().FirstOrDefault();

        root.AddEntity("Steven");
        await _provider.Context.SaveChangesAsync().ConfigureAwait(false);

        //BeforeEventTestHandler.Called.Should().BeTrue();
        BeforeAddedEventTestHandler.AsyncCalled.Should().BeTrue();
    }

    [Fact]
    public async Task AddEntity_BeforeEventAsync_ShouldRaised()
    {
        BeforeAddedEventTestHandler.ReturnFailureResult = false;

        var root = await _provider.Context.Set<Root>().FirstAsync().ConfigureAwait(false);

        root.AddEntity("Steven");

        await _provider.Context.SaveChangesAsync().ConfigureAwait(false);

        BeforeAddedEventTestHandler.Called.Should().BeFalse();
        BeforeAddedEventTestHandler.AsyncCalled.Should().BeTrue();
        BeforeUpdatedEventTestHandler.ProceedHandlers.Should().ContainSingle();
        BeforeUpdatedEventTestHandler.ProceedHandlers.Should().Contain($"Entity{root.Entities.FirstOrDefault()?.Id}");
    }

    [Fact]
    public async Task BeforeSave_WithFailedResult_ExceptionShouldRaised()
    {
        var root = _provider.Context.Set<Root>().First();

        root.AddEntity("Steven");

        BeforeAddedEventTestHandler.ReturnFailureResult = true;

        var a = () => _provider.Context.SaveChangesAsync();
        await a.Should().ThrowAsync<EventException>().ConfigureAwait(false);

        BeforeAddedEventTestHandler.ReturnFailureResult = false;
    }

    [Fact]
    public async Task AddEntity_BeforeEventAsync_Failed()
    {
        BeforeAddedEventTestHandler.ReturnFailureResult = false;

        var root = await _provider.Context.Set<Root>().FirstAsync().ConfigureAwait(false);

        root.AddEntity("Dragon");
        var entity = root.Entities.FirstOrDefault();
        entity?.DoLoop();

        var exception = await Assert.ThrowsAsync<OverflowException>(async () =>
            await _provider.Context.SaveChangesAsync().ConfigureAwait(false));
        exception.Message.Should().StartWith("Overflow maximum 5 loop check for run domain event handler.");
    }

    [Fact]
    public async Task AddCourse_CallOtherEvent_BeforeEventAsync_ShouldRaised()
    {
        var course = await _provider.Context.AddAsync(new Course("Become master of DDD"));
        await _provider.Context.SaveChangesAsync().ConfigureAwait(false);
        _provider.Context.ChangeTracker.Clear();

        await _provider.Context.AddAsync(new Student(new Guid("caa9c4e0-60d0-42e5-9130-642294e56acb"), "Dragon",
            course.Entity.Id));
        await _provider.Context.SaveChangesAsync().ConfigureAwait(false);

        BeforeStudentAddedEventHandler.ProceedHandlers.Should()
            .Contain($"{nameof(StudentAddedEvent)}caa9c4e0-60d0-42e5-9130-642294e56acb");
        BeforeCourseStartedEventHandler.ProceedHandlers.Should().Contain(nameof(CourseStartedEvent));
        BeforeCourseStatusChangedEventHandler.ProceedHandlers.Should().Contain(nameof(CourseStatusChangedEvent));
    }
}
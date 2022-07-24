using System.Threading.Tasks;
using FluentAssertions;
using HBDStack.EfCore.HookTests.Data;
using HBDStack.EfCore.Validation;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace HBDStack.EfCore.HookTests.Validations;

public class ValidationTests:IClassFixture<ValidationFixture>
{
    private readonly ValidationFixture _fixture;

    public ValidationTests(ValidationFixture fixture) => _fixture = fixture;

    [Fact]
    public async Task False_Validation_Tests()
    {
        var db = _fixture.Provider.GetRequiredService<HookContext>();
        
         db.Add(new Profile());
         
        var action = ()=> db.SaveChangesAsync();
       await action.Should().ThrowAsync<EntityValidationException>();
    }
    
    [Fact]
    public async Task Pass_Validation_Tests()
    {
        var db = _fixture.Provider.GetRequiredService<HookContext>();
        
        db.Add(new Profile{Name = "Steven"});
         
        var action = ()=> db.SaveChangesAsync();
        await action.Should().NotThrowAsync<EntityValidationException>();
    }
}
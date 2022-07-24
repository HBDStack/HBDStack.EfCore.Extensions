using System.ComponentModel.DataAnnotations;

namespace HBDStack.EfCore.Interceptors.Tests.Infra;

public class TestTruncateEntity
{
    [Key]
    public int Id { get; set; }
    
    [MaxLength(10)]
    public string Name { get; set; }
    
    [StringLength(10)]
    public string Title { get; set; }
}
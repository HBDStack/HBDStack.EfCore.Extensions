using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HBDStack.EfCore.Abstractions.Entities;

namespace DataLayer;

public class Address : Entity<int>
{
    #region Public Constructors

    public Address() : base(0)
    {
        OwnedEntity = new OwnedEntity();
    }

    #endregion Public Constructors

    #region Public Properties

    public OwnedEntity OwnedEntity { get; set; }
    [Required] [MaxLength(256)] public string Street { get; set; }

    public virtual User User { get; set; }
    [ForeignKey("Address_User")] public long UserId { get; set; }

    #endregion Public Properties
}
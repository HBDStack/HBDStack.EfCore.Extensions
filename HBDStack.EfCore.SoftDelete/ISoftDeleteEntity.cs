namespace HBDStack.EfCore.SoftDelete;

public interface ISoftDeleteEntity
{
    bool SoftDeleted { get; }

    void SetSoftDelete(bool deleted = true);
}
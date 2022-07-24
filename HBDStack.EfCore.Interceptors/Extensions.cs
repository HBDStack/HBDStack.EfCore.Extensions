using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace HBDStack.EfCore.Interceptors;

internal static class Extensions
{
    internal static Dictionary<IProperty, int> GetMaxLengthMetadata(this DbContext db)
    {
        var dic = new Dictionary<IProperty, int>();

        var entities = db.Model.GetEntityTypes();

        foreach (var property in entities.SelectMany(e => e.GetProperties()))
        {
            var annotation = property.GetAnnotations().FirstOrDefault(a => a.Name is "MaxLength" or "StringLength");
            if (annotation == null) continue;
            var maxLength = Convert.ToInt32(annotation.Value);
            if (maxLength > 0)
                dic[property] = maxLength;
        }

        return dic;
    }
}
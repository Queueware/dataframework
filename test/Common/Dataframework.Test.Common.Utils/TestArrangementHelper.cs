namespace Queueware.Dataframework.Test.Common.Utils;

public static class TestArrangementHelper
{
    public static IEnumerable<TEntity> GetRangeFrom<TEntity>(IEnumerable<TEntity> entities, bool isOne, bool isMany, bool isAll)
    {
        var entitiesList = entities.ToList();
        var maxIndex = 0;
        if (isOne)
        {
            maxIndex = 1;
        }
        else if (isMany)
        {
            maxIndex = entitiesList.Count / 2;
        }
        else if (isAll)
        {
            maxIndex = entitiesList.Count;
        }

        return entitiesList[..maxIndex];
    }
}
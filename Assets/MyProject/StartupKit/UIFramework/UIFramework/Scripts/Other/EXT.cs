using UnityEngine;

public static class EXT
{
    public static Transform GetChildWithName(this Transform targetTransform, string childName)
    {
        try
        {
            return targetTransform.Find(childName);
        }
        catch (System.Exception)
        {
            Debug.LogError("Child not found named " + childName);
            throw;
        }
    }

    public static string TrimId(this string m_id)
    {
        return m_id.Substring(0, 10);
    }
}
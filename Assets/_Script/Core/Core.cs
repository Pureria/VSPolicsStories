using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Core : MonoBehaviour
{
    public List<CoreComponent> CoreComponents { get; private set; } = new List<CoreComponent>();

    public void LogicUpdate()
    {
        foreach (CoreComponent component in CoreComponents)
            component.LogicUpdate();
    }

    public void AddCoreComponent(CoreComponent component)
    {
        if (!CoreComponents.Contains(component))
            CoreComponents.Add(component);
    }

    public T GetCoreComponent<T>() where T : CoreComponent
    {
        var comp = CoreComponents.OfType<T>().FirstOrDefault();

        if (comp == null)
            Debug.LogWarning($"{typeof(T)} Ç™ {transform.parent.name}Å@Ç…å©Ç¬Ç©ÇËÇ‹ÇπÇÒÅB");

        return comp;
    }

    public T GetCoreComponent<T>(ref T value) where T : CoreComponent
    {
        value = GetCoreComponent<T>();
        return value;
    }
}

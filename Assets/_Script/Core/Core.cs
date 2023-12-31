using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.Netcode;

public class Core : NetworkBehaviour
{
    public List<CoreComponent> CoreComponents { get; private set; } = new List<CoreComponent>();

    private void Start()
    {
        if (this.IsOwner)
            this.gameObject.name = "Core(Host)";
        else
            this.gameObject.name = "Core(Client)";
    }

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
            Debug.LogWarning($"{typeof(T)} が {transform.parent.name}　に見つかりません。");

        return comp;
    }

    public T GetCoreComponent<T>(ref T value) where T : CoreComponent
    {
        value = GetCoreComponent<T>();
        return value;
    }
}

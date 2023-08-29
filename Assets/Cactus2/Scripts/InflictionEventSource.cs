using System;
using UnityEngine;

public class InflictionEventSource : MonoBehaviour
{
    public event EventHandler<(IEntity issuer, float damageForHP, float damageForRP)> Inflicted;

    public void Inflict(IEntity issuer, float damageForHP, float damageForRP)
    {
        Inflicted?.Invoke(this, (issuer, damageForHP, damageForRP));
    }
}
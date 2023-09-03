#nullable enable

using System;
using System.Threading.Tasks;
using static Utils;
using static System.Math;

public class Animal : Entity, IAnimal
{
    float _recoveryProhDR = 0; // prohibition duration rest
    float _vitality;
    float _resilience;

    public override IVisitor? Visitor 
    { 
        get => base.Visitor; 
        set 
        {
            _visitor?.Remove(this);
            _visitor = value;
            _visitor?.Add(this);
        } 
    }
    public float RepairPromptness { get; } = 0.2f;
    public float ResilienceRecoveryDelay { get; } = 1;
    public float RepairA { get; } = 0.1f;
    public float Vitality { get => _vitality; set => _vitality = Clamp(value, 0, 1); }
    public float Resilience { get => _resilience; set => _resilience = Clamp(value, 0, 1); }

    public event AnimationTransitionEventHandler? TransitBodyAnimation;

    protected void OnTransitBodyAnimation(AnimationTransitionEventArgs e) => TransitBodyAnimation?.Invoke(this, e);

    protected override void Update(float deltaTime)
    {
        if (_resilience == 0) _recoveryProhDR = ResilienceRecoveryDelay;
        if (_recoveryProhDR <= 0)
            Resilience += deltaTime * RepairPromptness * (1 - Vitality + RepairA);

        base.Update(deltaTime);
    }

    public void InflictOnVitality(float strength)
    {
        Vitality -= strength;
    }
    public void InflictOnResilience(float damage)
    {
        Resilience -= damage;
    }
}

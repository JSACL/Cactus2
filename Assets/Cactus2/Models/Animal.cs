#nullable enable

using System.Threading.Tasks;
using static Utils;

public class Animal : Entity, IAnimal
{
    float _recoveryProhDR = 0; // prohibition duration rest
    float _vigor;
    float _repairPoint;

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
    public float RepairPointRecoveryDelay { get; } = 1;
    public float RepairA { get; } = 0.1f;
    public float Vigor { get => _vigor; set => _vigor = Confine(value, 0, 1); }
    public float RepairPoint { get => _repairPoint; set => _repairPoint = Confine(value, 0, 1); }

    public event AnimationTransitionEventHandler? TransitBodyAnimation;

    protected void OnTransitBodyAnimation(AnimationTransitionEventArgs e) => TransitBodyAnimation?.Invoke(this, e);

    protected override void Update(float deltaTime)
    {
        if (_repairPoint == 0) _recoveryProhDR = RepairPointRecoveryDelay;
        if (_recoveryProhDR <= 0)
            RepairPoint += deltaTime * RepairPromptness * (1 - Vigor + RepairA);

        base.Update(deltaTime);
    }
}

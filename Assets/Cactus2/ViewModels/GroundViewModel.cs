#nullable enable
using Cactus2;
using Nonno.Assets.Collections;

public class GroundViewModel : ViewModel<IGround>
{
    StabilizationEffect _stbCommand;
    DestabilizationEffect _dtbCommand;

    public EnterEffectComponent enc_body;
    public ExitEffectComponent exc_body;

    private void Start()
    {
        enc_body.Effect = _stbCommand;
        exc_body.Effect = _dtbCommand;
    }

    private void OnEnable()
    {
        _stbCommand = new() { Authority = Authority.Natural};
        _dtbCommand = new() { Authority = Authority.Natural};
    }
}
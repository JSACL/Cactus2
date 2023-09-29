#nullable enable
using UnityEngine;

public class GroundViewModel : ViewModel<IGround>
{
    readonly StabilizationCommand _stbCommand;
    readonly DestabilizationCommand _dtbCommand;

    public EnterCommandComponent enc_body;
    public ExitCommandComponent exc_body;

    private void Start()
    {
        enc_body.Command = _stbCommand;
        exc_body.Command = _dtbCommand;
    }

    private void OnEnable()
    {
        _stbCommand.Authority = Authority.Natural;
        _dtbCommand.Authority = Authority.Natural;
    }
}
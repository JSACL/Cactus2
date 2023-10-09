﻿#nullable enable
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cactus2;
public class SuperiorReferee : IReferee
{
    readonly List<IReferee> _inferiors;

    public IEnumerable<IReferee> Inferiors => _inferiors;

    public SuperiorReferee()
    {
        _inferiors = new();
    }

    public Judgement Judge(Authority offensiveSide, Authority defensiveSide)
    {
        var j = Judgement.None;
        foreach (var referee in _inferiors)
        {
            var j_ = referee.Judge(offensiveSide, defensiveSide);
            j = (j, j_) switch
            {
                (Judgement.Error, _) => Judgement.Error,
                (not Judgement.Invalid, Judgement.Valid) => Judgement.Valid,
                (not Judgement.Valid, Judgement.Invalid) => Judgement.Invalid,
                (_, Judgement.Error) => Judgement.Error,
                _ => Judgement.None,
            };
        }
        return j;
    }
    public async Task<Judgement> JudgeAsync(Authority offensiveSide, Authority defensiveSide)
    {
        var j = Judgement.None;
        var js = await Task.WhenAll(_inferiors.Select(x => x.JudgeAsync(offensiveSide, defensiveSide)));
        foreach (var j_ in js)
        {
            j = (j, j_) switch
            {
                (Judgement.Error, _) => Judgement.Error,
                (not Judgement.Invalid, Judgement.Valid) => Judgement.Valid,
                (not Judgement.Valid, Judgement.Invalid) => Judgement.Invalid,
                (_, Judgement.Error) => Judgement.Error,
                _ => Judgement.None,
            };
        }
        return j;
    }
    public Judgement Judge(IAuthorized offensiveSide, IAuthorized defensiveSide)
    {
        var j = Judgement.None;
        foreach (var referee in _inferiors)
        {
            var j_ = referee.Judge(offensiveSide, defensiveSide);
            j = (j, j_) switch
            {
                (Judgement.Error, _) => Judgement.Error,
                (not Judgement.Invalid, Judgement.Valid) => Judgement.Valid,
                (not Judgement.Valid, Judgement.Invalid) => Judgement.Invalid,
                (_, Judgement.Error) => Judgement.Error,
                _ => Judgement.None,
            };
        }
        return j;
    }
    public async Task<Judgement> JudgeAsync(IAuthorized offensiveSide, IAuthorized defensiveSide)
    {
        var j = Judgement.None;
        var js = await Task.WhenAll(_inferiors.Select(x => x.JudgeAsync(offensiveSide, defensiveSide)));
        foreach (var j_ in js)
        {
            j = (j, j_) switch
            {
                (Judgement.Error, _) => Judgement.Error,
                (not Judgement.Invalid, Judgement.Valid) => Judgement.Valid,
                (not Judgement.Valid, Judgement.Invalid) => Judgement.Invalid,
                (_, Judgement.Error) => Judgement.Error,
                _ => Judgement.None,
            };
        }
        return j;
    }

    public void Add(IReferee referee)
    {
        _inferiors.Add(referee);
    }

    public void Remove(IReferee referee)
    {
        _inferiors.Remove(referee);
    }

    public static SuperiorReferee Topmost { get; } = new();
}

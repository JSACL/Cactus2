using UnityEngine;

public static class Referee
{
    public static async void JudgeCollisionEnter(GameObject one, GameObject theother)
    {
        var hC_o = one.GetComponentSC<HarmfulObjectComponent>();
        var tC_t = theother.GetComponentSC<TargetComponent>();
        if (hC_o != null && tC_t != null)
        {
            var j = await IReferee.Current.JudgeAsync(hC_o.Participant, tC_t.ParticipantIndex);
            if (j == Judgement.Valid)
            {
                tC_t.InflictToHitPoint(hC_o.DamageForVitality);
                tC_t.InflictToRepairPoint(hC_o.DamageForResilience);
                hC_o.OnHit();
            }
        }
    }

    public static void JudgeTriggerStay(GameObject one, GameObject theother)
    {

    }

    public static ParticipantIndex GetTargetTag(ParticipantIndex tag)
    {
        foreach (var i in ParticipantIndex.Context.Indexes)
        {
            if (IReferee.Current.Judge(tag, i) == Judgement.Valid) return i;
        }
        return ParticipantIndex.Unknown;
    }
}
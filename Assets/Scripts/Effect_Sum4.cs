using System.Collections;
using UnityEngine;

public class Effect_Sum4 : Effect
{
    public override void DoEffect(CardInfo _info)
    {
        StartCoroutine(StealCoroutine());

    }

    IEnumerator StealCoroutine()
    {
        for (int i = 0; i < 4; i++)
        {
            CardDealer.instance.StealCard();
            yield return new WaitForSeconds(0.25f);
        }
    }
}
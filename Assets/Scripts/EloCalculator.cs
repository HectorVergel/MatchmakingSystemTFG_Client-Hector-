using UnityEngine;
using UnityEngine.Timeline;

public static class EloCalculator
{
    public static int m_InitialRanking = 1200;

    private static float GetExpectedScore(int myRating, int otherRating)
    {
        return 1.0f / (1 + 10 ^ ((myRating - otherRating) / 400));
    }

    public static int ComputeEloRating(int matchResult, int myRating, int otherRating)
    {
        float newRating = GameManager.instance.GetPlayer().ranking + GetKFactor(myRating) * (matchResult - GetExpectedScore(myRating, otherRating));
        return Mathf.RoundToInt(newRating);
    }

    private static int GetKFactor(int myRating)
    {
        int l_Kfactor = 0;
        switch (myRating)
        {
            case <2100:
                l_Kfactor = 32;
                break;
            case >2400:
                l_Kfactor = 24;
                break;
            default:
                l_Kfactor = 16;
                break;
        }

        return l_Kfactor;
    }
}
using UnityEngine;

[CreateAssetMenu(fileName = "CsoundScoreEvent", menuName = "Csound/ScoreEvent")]
public class CsoundScoreEventSO : ScriptableObject
{
    public string scoreCharacter, p1Instrument;
    public float p2Delay, p3Duration;
    public float[] pFields;

    public string ConcatenateScoreEventString()
    {
        string concatenatedPFields = string.Join(" ", pFields);
        string scoreEvent = scoreCharacter + " " + p1Instrument + " " + p2Delay + " " + p3Duration + " " + concatenatedPFields;
        return scoreEvent;
    }
}

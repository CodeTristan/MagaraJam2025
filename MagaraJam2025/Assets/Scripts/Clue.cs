using UnityEngine;

[System.Serializable]
public class Clue
{
    public ClueName clueName;
    public string description;

}

public enum ClueName
{
    None,
    [DisplayName("Peter Alibi")] Peter_Alibi,
    [DisplayName("Artist Titiz")] Artist_Titiz,
}
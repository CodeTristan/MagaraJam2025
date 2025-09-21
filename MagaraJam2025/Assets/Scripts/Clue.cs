using UnityEngine;

[System.Serializable]
public class Clue
{
    public ClueName clueName;
    [TextArea(3,10)]
    public string description;

}

public enum ClueName
{
    None,
    [DisplayName("Peter Alibi")] Peter_Alibi,
    [DisplayName("Artist Titiz")] Artist_Titiz,
    [DisplayName("Woman Alibi")] Woman_Alibi,
    [DisplayName("Woman Painting Liked")] Woman_Painting_Liked,
    [DisplayName("Framer Alibi")] Framer_Alibi,
    [DisplayName("Framer Heard Nothing")] Framer_Heard_Nothing,
}
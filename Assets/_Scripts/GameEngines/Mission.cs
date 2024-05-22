using UnityEngine;

public class Mission
{
    public string MissionTitle { get; set; }
    public string ShortDescription { get; set; }
    public string FullDescription { get; set; }
    public Sprite Image { get; set; }
    public Sprite Thumbnail { get; set; }
    public bool IsCompleted { get; set; } = false;

    public Mission(string missionTitle, string shortDescription, string fullDescription, Sprite image, Sprite thumbnail, bool isCompleted)
    {
        MissionTitle = missionTitle;
        ShortDescription = shortDescription;
        FullDescription = fullDescription;
        Image = image;
        Thumbnail = thumbnail;
        IsCompleted = isCompleted;
    }
}
using UnityEngine;

//儲存整段劇情的地方
[System.Serializable]
public class StorySequence
{
    public string storyID;//儲存劇情的ID
    public StoryStep[] steps;//這段劇情的所有步驟
}

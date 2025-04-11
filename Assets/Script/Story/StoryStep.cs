using UnityEngine;

//儲存每個步驟的地方
[System.Serializable]
public class StoryStep
{
    [TextArea(3, 10)]//在Inspector顯示的文字3個*10個
    public string dialogue;//對話內容
    public string speakerName;//說話者名字
    public string speakerID;//說話者id，在輸入劇情的時候會透過這個id認人
    public string photoID;//說話者的照片id，跟使用者一樣用id認人
    public bool keepBrightIfSameSpeaker = true;//如果跟上一句是同一個說話者就保持亮度
    public bool showPlayer = true;//是否顯示玩家
    public bool showEnemy = true;
}

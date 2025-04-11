using UnityEngine;
using UnityEngine.UI;

// 儲存角色圖片資料的地方
[System.Serializable]
public class PhotoData
{
    public string photoID;//照片的id，命名時不能用中文，其他隨意，可以用有意義的文字標註照片的表情
    public Sprite sprite;//儲存的圖片
}

// 儲存角色資料的地方
[System.Serializable]
public class CharacterData
{
    public string characterID;//角色的id，不能用中文
    public Image characterImage;//用來顯示初始的圖片，也是其他圖片連到的地方
    public Vector2 basePosition;//初始位置，需要設定，否則只是在scene拖拉一樣會跑掉
    public PhotoData[] photos;//照片的陣列
}

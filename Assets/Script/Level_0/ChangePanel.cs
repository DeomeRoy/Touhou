using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ChangePanel : MonoBehaviour{
    public RectTransform Target,Target2,Target3,Target4;
    public void MoveInPanel(){
        Target.DOAnchorPos(new Vector2(-533, 60), 1f);
        Target2.DOAnchorPos(new Vector2(-533, -100), 1f);
        Target3.DOAnchorPos(new Vector2(-533, -260), 1f);
        Target4.DOAnchorPos(new Vector2(-533, -420), 1f);
    }
    public void MoveOutPanel(){
        Target.DOAnchorPos(new Vector2(-1220, 60), 1f);
        Target2.DOAnchorPos(new Vector2(-1220, -100), 1f);
        Target3.DOAnchorPos(new Vector2(-1220, -260), 1f);
        Target4.DOAnchorPos(new Vector2(-1220, -420), 1f);
    }
}

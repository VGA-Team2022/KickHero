using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class StageManager 
{
    /// <summary>
    /// ステージ解放メソッド
    /// </summary>
    /// <param name="isCleared">SceneOperatorクラスの_isClearedStagesを想定</param>
    /// <param name="stage">6つのステージボタンを想定</param>
    public void OpenStage(bool[] isCleared, Button[] stage)
    {
        int count = 0;
        for (int i = 0; i < isCleared.Length; i++)
        {
            if (isCleared[i])
            {
                count++;
            }
        }
        for (int i = 1; i <= count; i++)
        {
            //stage[0]は一番最初の常にアクティブなステージを想定している。
            //なので、アクティブにしていくステージはstage[1]からにしている。
            if(i != 6)  //stage[6] は存在しないので制限をかける
            {
                stage[i].interactable = true;
            }
        }
    }
}

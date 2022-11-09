using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearJudgmentScript
{
    /// <summary>
    /// クリア時に呼ばれたいメソッド
    /// </summary>
    /// <param name="keepData">データ保管庫</param>
    /// <param name="num">シーン番号</param>
    public void CountStage(KeepData keepData, int num)
    {
        keepData._countClear = num;
        Debug.Log(keepData._countClear);
    }
}

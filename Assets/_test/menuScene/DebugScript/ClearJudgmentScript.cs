using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearJudgmentScript
{
    /// <summary>
    /// �N���A���ɌĂ΂ꂽ�����\�b�h
    /// </summary>
    /// <param name="keepData">�f�[�^�ۊǌ�</param>
    /// <param name="num">�V�[���ԍ�</param>
    public void CountStage(KeepData keepData, int num)
    {
        keepData._countClear = num;
        Debug.Log(keepData._countClear);
    }
}

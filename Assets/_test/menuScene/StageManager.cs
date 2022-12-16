using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageManager 
{
    /// <summary>
    /// �X�e�[�W������\�b�h
    /// </summary>
    /// <param name="isCleared">SceneOperator�N���X��_isClearedStages��z��</param>
    /// <param name="stage">6�̃X�e�[�W�{�^����z��</param>
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
        if (count < isCleared.Length)   //�G���[:OutOfIndex�@�ɂȂ�Ȃ����߂̐���
        {
            for (int i = 1; i <= count; i++)
            {
                //stage[0]�͈�ԍŏ��̏�ɃA�N�e�B�u�ȃX�e�[�W��z�肵�Ă���B
                //�Ȃ̂ŁA�A�N�e�B�u�ɂ��Ă����X�e�[�W��stage[1]����ɂ��Ă���B
                stage[i].interactable = true;
            }
        }
    }
}

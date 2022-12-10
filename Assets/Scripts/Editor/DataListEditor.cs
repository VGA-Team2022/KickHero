using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class DataListEditor : EditorWindow
{
    private Vector2 _dataScrollPosition;
    private Vector2 _parameterScrollPosition;

    private List<InGameData> _samples = new List<InGameData>();
    private InGameData _selectSample = null;

    [MenuItem("Editor/DataList")]
    private static void Create()
    {
        GetWindow<DataListEditor>("ステージデータ");
    }

    private void OnGUI()
    {
        using (new GUILayout.HorizontalScope())
        {
            UpdateLayoutData();
            UpdateLayoutParameter();
        }
    }

    private void Awake()
    {
        _samples.Clear();
        ImportAll();
    }

    private void UpdateLayoutData()
    {
        using (GUILayout.ScrollViewScope scroll = new GUILayout.ScrollViewScope(_dataScrollPosition, EditorStyles.helpBox, GUILayout.Width(150)))
        {
            _dataScrollPosition = scroll.scrollPosition;

            // メニュー追加
            GenericMenu menu = new GenericMenu();
            if (Event.current.type == EventType.ContextClick && Event.current.button == 1)
            {
                menu.AddItem(new GUIContent("AddSample"), false, () =>
                {
                    _samples.Add(CreateInstance<InGameData>());
                    _selectSample = _samples[_samples.Count - 1];
                    Export(_samples.Count - 1);
                });
            }

            // データリスト
            for (int i = 0; i < _samples.Count; i++)
            {
                GUI.backgroundColor = (_samples[i] == _selectSample ? Color.cyan : Color.white);
                if (GUILayout.Button($"Data:{i}"))
                {
                    _selectSample = _samples[i];

                    // メニュー追加
                    if (Event.current.button == 1)
                    {
                        int index = i;
                        menu.AddItem(new GUIContent("Remove"), false, () =>
                        {

                            _samples.Remove(_samples[index]);
                            File.Delete(ASSET_PATH + index + ".asset");
#if UNITY_EDITOR
                            UnityEditor.AssetDatabase.Refresh();
#endif
                            _selectSample = null;
                        });
                    }
                }
                GUI.backgroundColor = Color.white;
            }

            // メニュー表示
            if (menu.GetItemCount() > 0)
            {
                menu.ShowAsContext();
                Event.current.Use();
            }
        }
    }
    private void UpdateLayoutParameter()
    {
        using (GUILayout.ScrollViewScope scroll = new GUILayout.ScrollViewScope(_parameterScrollPosition, EditorStyles.helpBox))
        {
            _parameterScrollPosition = scroll.scrollPosition;

            if (_selectSample)
            {
                Editor.CreateEditor(_selectSample).DrawDefaultInspector();
            }
        }
        using (new GUILayout.HorizontalScope())
        {
            // 書き込みボタン
            if (GUILayout.Button("更新"))
            {
                int index = _samples.IndexOf(_selectSample);
                if (index != -1)
                {
                    Export(index);
                }
                else
                {
                    Debug.LogError("指定されたInGameDataが見つかりません。");
                }
            }
        }
    }

    const string ASSET_PATH = "Assets/Resources/InGameDatas/Data_";
    void Export(int index)
    {
        // 読み込み
        InGameData sample = AssetDatabase.LoadAssetAtPath<InGameData>(ASSET_PATH + index + ".asset");
        if (sample == null)
        {
            sample = ScriptableObject.CreateInstance<InGameData>();
        }

        // 新規の場合は作成
        if (!AssetDatabase.Contains(sample as UnityEngine.Object))
        {
            string directory = Path.GetDirectoryName(ASSET_PATH + index + ".asset");
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            // アセット作成
            AssetDatabase.CreateAsset(sample, ASSET_PATH + index + ".asset");
        }


        // コピー
        EditorUtility.CopySerialized(_samples[index], sample);

        // 直接編集できないようにする
        sample.hideFlags = HideFlags.NotEditable;
        // 更新通知
        EditorUtility.SetDirty(sample);
        // 保存
        AssetDatabase.SaveAssets();
        // エディタを最新の状態にする
        AssetDatabase.Refresh();
    }

    void ImportAll()
    {
        var count = Resources.LoadAll("InGameDatas").Length;
        for (int i = 0;i<count;i++)
        {
            Import(i);
        }
    }

    private void Import(int index)
    {
        if (_selectSample == null)
        {
            _selectSample = ScriptableObject.CreateInstance<InGameData>();
        }

        InGameData sample = AssetDatabase.LoadAssetAtPath<InGameData>(ASSET_PATH + index + ".asset");
        if (sample == null)
            return;
        _samples.Add(CreateInstance<InGameData>());
        EditorUtility.CopySerialized(sample, _samples[_samples.Count-1]);
    }

    private void OnDestroy()
    {
        for (int i = 0; i < _samples.Count; i++)
        {
            Export(i);
        }
    }
}

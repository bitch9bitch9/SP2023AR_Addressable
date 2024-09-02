using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 綁定成功UI
public class BindSuccessUI : MonoBehaviour
{
    // 引入firebaseManager
    [SerializeField]
    FirebaseManager firebaseManager;

    // Dropdown 用來得知綁定哪個特效
    [SerializeField]
    Dropdown dropdown;

    // List 用來儲存特效名稱
    [SerializeField]
    List<string> prefabs;

    // 選擇特效名，用於儲存模組名
    string modelName;

    private void Start() {
        // 開始時確保清空所有選項
        dropdown.options.Clear();

        // 讀取所有特效名稱加進dropdown
        foreach (var prefab in prefabs) {
            dropdown.options.Add(new Dropdown.OptionData() { text = prefab });
        }
    }

    // 當按下SAVE按鈕
    public void OnClickSaveB() {
        // 獲取索引值
        int index = dropdown.value;

        // 獲取3D模型名
        modelName = dropdown.options[index].text;

        // 儲存綁定訊息
        BindInfoSave();
    }

    // 隱藏此UI
    public void Hide() {
        gameObject.SetActive(false);
    }

    // 秀出此UI
    public void Show() {
        gameObject.SetActive(true);
    }

    // 儲存綁定訊息方法
    private void BindInfoSave() {
        // 儲存綁定訊息
        firebaseManager.UserSave(GameManager.bindCode, modelName);
    }
}

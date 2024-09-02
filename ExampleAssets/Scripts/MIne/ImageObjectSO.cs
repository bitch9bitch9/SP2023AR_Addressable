using UnityEngine;

// 目標物件組別
[CreateAssetMenu()]
public class ImageObjectSO : ScriptableObject {
    // 名稱
    public string objectName;
    // 圖片物件
    public GameObject imageTarget;
    // 認證碼
    public string bindCode;
}

using Firebase.Database;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

// 掃描物件控制
public class CharacterControl : MonoBehaviour {

    // QRCode物件
    public ImageObjectSO imageObject;

    // 此為parent，底下生成3Dmodel
    [SerializeField]
    Transform spawnTransform;

    // 用於被生成物件附值，用在LoseTarget刪除
    GameObject generatedObject;

    // 讀取資料庫3D模型名
    string modelName;

    private void Start() {
        if (GameManager.gameMode == "spawn") {
            // 協成函式呼叫載入3D模型名並生成
            StartCoroutine(LoadModelNameTask());
        }
    }

    // 當目標圖片被掃描
    public void OnGraphicBeScanned() {
        // 如果為綁定模式
        if (GameManager.gameMode == "bind") {

            //如果綁定碼等於QRCode物件之綁定碼
            if (GameManager.bindCode == imageObject.bindCode) {
                //    設為成功綁定
                BindUI.isBindSuccess = true;

                //    引入主畫面
                SceneManager.LoadScene("MainScene");
            }

            // 如果為顯示特效模式(可不用條件直接else，因為只有兩種模式)
        } else if (GameManager.gameMode == "spawn") {
            if (generatedObject != null) {
                generatedObject.SetActive(true);
            }
        }
    }

    // 當失去目標圖片
    public void OnLoseTarget() {
        if (generatedObject != null) {
            // 摧毀生成特效
            generatedObject.SetActive(false);
        }
    }

    // 生成特效方法(利用Addressable在根目錄拿取特效
    public void SpawnEffect() {
        Addressables.LoadAssetAsync<GameObject>("Assets/ExampleAssets/Hovl Studio/Magic circles Vol 3/Prefabs/" + modelName + ".prefab").Completed +=
                (task) => {
                    if (task.Status == AsyncOperationStatus.Succeeded) {
                        // 3D圖生成
                        generatedObject = Instantiate(task.Result, spawnTransform);
                        generatedObject.SetActive(false);
                    }
                };
    }

    // 載入模型名(成功則生成)
    IEnumerator LoadModelNameTask() {
        var task = FirebaseDatabase.DefaultInstance.RootReference.Child(imageObject.bindCode).Child("modelName").GetValueAsync();
        yield return new WaitUntil(() => task.IsCompleted);
        if (task.IsCompletedSuccessfully) {
            DataSnapshot snapshot = task.Result;
            if (snapshot.Value != null) {
                modelName = snapshot.Value.ToString();
                SpawnEffect();
            } else {
                modelName = "";
            }
        }
    }
}
using Firebase.Database;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

// ���y���󱱨�
public class CharacterControl : MonoBehaviour {

    // QRCode����
    public ImageObjectSO imageObject;

    // ����parent�A���U�ͦ�3Dmodel
    [SerializeField]
    Transform spawnTransform;

    // �Ω�Q�ͦ�������ȡA�ΦbLoseTarget�R��
    GameObject generatedObject;

    // Ū����Ʈw3D�ҫ��W
    string modelName;

    private void Start() {
        if (GameManager.gameMode == "spawn") {
            // �󦨨禡�I�s���J3D�ҫ��W�åͦ�
            StartCoroutine(LoadModelNameTask());
        }
    }

    // ��ؼйϤ��Q���y
    public void OnGraphicBeScanned() {
        // �p�G���j�w�Ҧ�
        if (GameManager.gameMode == "bind") {

            //�p�G�j�w�X����QRCode���󤧸j�w�X
            if (GameManager.bindCode == imageObject.bindCode) {
                //    �]�����\�j�w
                BindUI.isBindSuccess = true;

                //    �ޤJ�D�e��
                SceneManager.LoadScene("MainScene");
            }

            // �p�G����ܯS�ļҦ�(�i���α��󪽱�else�A�]���u����ؼҦ�)
        } else if (GameManager.gameMode == "spawn") {
            if (generatedObject != null) {
                generatedObject.SetActive(true);
            }
        }
    }

    // ���h�ؼйϤ�
    public void OnLoseTarget() {
        if (generatedObject != null) {
            // �R���ͦ��S��
            generatedObject.SetActive(false);
        }
    }

    // �ͦ��S�Ĥ�k(�Q��Addressable�b�ڥؿ������S��
    public void SpawnEffect() {
        Addressables.LoadAssetAsync<GameObject>("Assets/ExampleAssets/Hovl Studio/Magic circles Vol 3/Prefabs/" + modelName + ".prefab").Completed +=
                (task) => {
                    if (task.Status == AsyncOperationStatus.Succeeded) {
                        // 3D�ϥͦ�
                        generatedObject = Instantiate(task.Result, spawnTransform);
                        generatedObject.SetActive(false);
                    }
                };
    }

    // ���J�ҫ��W(���\�h�ͦ�)
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
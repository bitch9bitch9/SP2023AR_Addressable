using UnityEngine;
using UnityEngine.SceneManagement;

// �C���e���޲z
public class GameManager : MonoBehaviour {

    // �j�w�X
    public static string bindCode = "";

    // �ҰʼҦ��A�j�w����ܯS��
    public static string gameMode;


    // ���Uback��
    public void OnClickBack() {
        SceneManager.LoadScene("MainScene");
    }
}

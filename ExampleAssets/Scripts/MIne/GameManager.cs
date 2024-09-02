using UnityEngine;
using UnityEngine.SceneManagement;

// 遊戲畫面管理
public class GameManager : MonoBehaviour {

    // 綁定碼
    public static string bindCode = "";

    // 啟動模式，綁定或顯示特效
    public static string gameMode;


    // 按下back時
    public void OnClickBack() {
        SceneManager.LoadScene("MainScene");
    }
}

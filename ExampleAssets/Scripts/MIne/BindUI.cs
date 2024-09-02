using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// 綁定畫面
public class BindUI : MonoBehaviour {

    // 類別實例(亦可改用附掛方式)
    public static BindUI Instance { get; private set; }

    // 綁定碼輸入
    [SerializeField]
    InputField bindInput;

    // 綁定成功UI
    [SerializeField]
    BindSuccessUI bindSuccessUI;

    // 綁定是否成功
    public static bool isBindSuccess = false;

    private void Awake() {
        // 設定實例
        Instance = this;
    }


    private void Start() {
        // 如果綁定成功
        if (isBindSuccess) {
            // Show成功UI
            bindSuccessUI.Show();
            // Show此UI
            Show();
        } else
            // 隱藏此UI
            Hide();
    }


    // 去掃描QRCode以綁定
    public void GoSendQRCodeButton() {
        if (bindInput.text != "") {
            // 設定相機模式
            GameManager.gameMode = "bind";
            // 將綁定碼傳到GameManager裡以分辨綁定碼是否互相匹配
            GameManager.bindCode = bindInput.text;
            // 引入掃描畫面
            SceneManager.LoadScene("GameScene");
        }
    }

    // LogIn畫面按下綁定時
    public void Bind() {
        // 顯示此UI
        Show();
        // 如果綁定已成功
        if(isBindSuccess) {
            // show綁定成功UI
            bindSuccessUI.Show();
        } else {
            // 隱藏綁定成功UI
            bindSuccessUI.Hide();
        }
    }

    // 當按下取消綁定
    public void OnClickCancel() {
        // 隱藏此UI
        Hide();
    }

    // 隱藏UI
    public void Hide() {
        this.gameObject.SetActive(false);
    }

    // 秀出UI
    public void Show() { 
        this.gameObject.SetActive(true);
    }
}

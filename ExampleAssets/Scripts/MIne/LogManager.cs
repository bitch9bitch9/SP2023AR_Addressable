using Google;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// 登入/出控制
public class LogManager : MonoBehaviour {
    // firebase管理器
    [SerializeField]
    FirebaseManager firebaseManager;
    // Email輸入條
    [SerializeField]
    InputField inputEmail;
    // 密碼輸入條
    [SerializeField]
    InputField inputPassword;


    // 帳號訊息UI
    [SerializeField]
    GameObject infoUI;

    // 註冊/登入畫面
    [SerializeField]
    GameObject loginUI;

    // Email文字
    [SerializeField]
    Text textEmail;


    void Start() {
        // 當授權變動時聽取方法
        firebaseManager.auth.StateChanged += AuthStateChanged;
    }

    // 按下登入按鈕
    public void Login() {
        firebaseManager.Login(inputEmail.text, inputPassword.text);
    }

    // 按下登出按鈕
    public void Logout() {
        firebaseManager.Logout();
    }


    // 按下Google登入按鈕
    public void GoogleSignInClick() {
        if (GoogleSignIn.Configuration == null) {
            GoogleSignIn.Configuration = firebaseManager.configuration;
            GoogleSignIn.Configuration.UseGameSignIn = false;
        }
        GoogleSignIn.Configuration.RequestIdToken = true;
        GoogleSignIn.Configuration.RequestEmail = true;

        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(firebaseManager.OnGoogleAuthenticatedFinished);
    }


    // 授權變動時聽取方法
    void AuthStateChanged(object sender, System.EventArgs e) {
        if (firebaseManager.user == null) {
            textEmail.text = "";
            loginUI.SetActive(true);
            infoUI.SetActive(false);
        } else {
            textEmail.text = firebaseManager.user.Email;
            loginUI.SetActive(false);
            infoUI.SetActive(true);
        }
    }

    // 程式關閉
    private void OnDestroy() {
        firebaseManager.auth.StateChanged -= AuthStateChanged;
    }

    // 按下play
    public void Play() {
        // 設定相機模式
        GameManager.gameMode = "spawn";
        // 設定綁定成功為否，重新回到首頁時不會自動跳出BindUI
        BindUI.isBindSuccess = false;

        SceneManager.LoadScene("GameScene");
    }

    // 按下Bind
    public void ClickBind() {
        BindUI.Instance.Bind();
    }
}

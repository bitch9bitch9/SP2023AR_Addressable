using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
using Google;
using Firebase.Extensions;
using System;
using Firebase.Auth;

// Firebase管理器(有關googleSignIn內容因本人實力不足為抄取別人代碼，有錯誤還請幫忙修正
public class FirebaseManager : MonoBehaviour {
    // 設定Google網站API
    const string GoogleWebAPI = "935824609750-put3ncqvhj4r98fulhvb4mj4qis2tpsu.apps.googleusercontent.com";
    // 設定Google配置
    public GoogleSignInConfiguration configuration;

    // 設定授權
    public Firebase.Auth.FirebaseAuth auth;
    // 設定使用者
    public Firebase.Auth.FirebaseUser user;


    // 訊息顯示(需要時使用，引用在AddToInformation()
    [SerializeField]
    Text infoText;



    private void Awake() {
        // 配置Google設定
        configuration = new GoogleSignInConfiguration {
            // 客戶端ID = Google網站API
            WebClientId = GoogleWebAPI,
            // ID請求成功
            RequestIdToken = true,
        };
    }

    void Start() {
        // 授權設為Firebase預設實例
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        // 當授權變動時聽取方法
        auth.StateChanged += AuthStateChanged;
    }


    // 一般註冊方法
    public void Register(string email, string password) {
        // 創建使用者
        auth.CreateUserWithEmailAndPasswordAsync(email, password);
    }

    // 一般登入方法
    public void Login(string email, string password) {
        // 使用者登入
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
            
            // 如果登入失敗
            if (task.IsFaulted) {
                // 拿取錯誤變數
                FirebaseException exception = task.Exception.GetBaseException() as FirebaseException;
                // 如果錯誤訊息不為空
                if (exception != null) {
                    // 如果錯誤索引碼 == 未找到使用者的索引碼
                    if(exception.ErrorCode == (int)AuthError.UserNotFound) {
                        // 使用註冊方法
                        Register(email, password);
                    }
                }
                return;
            }
        });
    }

    // 登出方法
    public void Logout() {
        // 設綁定成功為否
        BindUI.isBindSuccess = false;

        // 判斷使用者是否為google帳號
        foreach (var userInfo in user.ProviderData) {
            if (userInfo.ProviderId == "google.com") {
                // 切斷google帳號預設連接
                GoogleDeconnect();
            }
        }

        // 登出
        auth.SignOut();
    }

    // 切斷google帳號預設連接方法
    public void GoogleDeconnect() {
        // 切斷google帳號預設連接
        GoogleSignIn.DefaultInstance.SignOut();
    }


    // 授權變動時聽取方法
    void AuthStateChanged(object sender, System.EventArgs e) {
        // 如果現在角色與使用者不同
        if (auth.CurrentUser != user) {
            // 設使用者為現任使用者
            user = auth.CurrentUser;
        }
    }


    // 關閉程式
    private void OnDestroy() {
        auth.StateChanged -= AuthStateChanged;
    }


    // 當Google認證完成時，我不確定她是怎麼執行的因為我是抄別人代碼的
    public void OnGoogleAuthenticatedFinished(Task<GoogleSignInUser> task) {
        // 登入失敗時
        if (task.IsFaulted) {
            //
            using (IEnumerator<Exception> enumerator = task.Exception.InnerExceptions.GetEnumerator()) {
                if (enumerator.MoveNext()) {
                    GoogleSignIn.SignInException error = (GoogleSignIn.SignInException)enumerator.Current;
                    AddToInformation("Got Error: " + error.Status + " " + error.Message);
                } else {
                    AddToInformation("Got Unexpected Exception?!?" + task.Exception);
                }
            }
        } else {
            Firebase.Auth.Credential credential = Firebase.Auth.GoogleAuthProvider.GetCredential(task.Result.IdToken, null);

            auth.SignInWithCredentialAsync(credential).ContinueWithOnMainThread(task => {
                if (task.Exception != null) {
                    if (task.Exception.InnerExceptions[0] is FirebaseException inner && (inner.ErrorCode != 0))
                        AddToInformation("\nError code = " + inner.ErrorCode + "Message = " + inner.Message);
                }
            });
        }
    }

    // 取得參數，用在存取、讀取資料庫
    public DatabaseReference GetUserReference() {
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        return reference;
    }

    // 增添Debug訊息(需要時自行啟用，目前用不到
    public void AddToInformation(string str) { infoText.text = str; }


    // 儲存綁定訊息
    public void UserSave(string license, string data) {
        // 如果使用者存在
        if (user != null) {
            // 儲存資料
            GetUserReference().Child(license).Child("modelName").SetValueAsync(data).ContinueWith(task => {
                if (task.IsCompletedSuccessfully) {
                    BindUI.Instance.Hide();
                }
            });
        }
    }
}

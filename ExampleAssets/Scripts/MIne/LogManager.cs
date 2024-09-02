using Google;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// �n�J/�X����
public class LogManager : MonoBehaviour {
    // firebase�޲z��
    [SerializeField]
    FirebaseManager firebaseManager;
    // Email��J��
    [SerializeField]
    InputField inputEmail;
    // �K�X��J��
    [SerializeField]
    InputField inputPassword;


    // �b���T��UI
    [SerializeField]
    GameObject infoUI;

    // ���U/�n�J�e��
    [SerializeField]
    GameObject loginUI;

    // Email��r
    [SerializeField]
    Text textEmail;


    void Start() {
        // ����v�ܰʮ�ť����k
        firebaseManager.auth.StateChanged += AuthStateChanged;
    }

    // ���U�n�J���s
    public void Login() {
        firebaseManager.Login(inputEmail.text, inputPassword.text);
    }

    // ���U�n�X���s
    public void Logout() {
        firebaseManager.Logout();
    }


    // ���UGoogle�n�J���s
    public void GoogleSignInClick() {
        if (GoogleSignIn.Configuration == null) {
            GoogleSignIn.Configuration = firebaseManager.configuration;
            GoogleSignIn.Configuration.UseGameSignIn = false;
        }
        GoogleSignIn.Configuration.RequestIdToken = true;
        GoogleSignIn.Configuration.RequestEmail = true;

        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(firebaseManager.OnGoogleAuthenticatedFinished);
    }


    // ���v�ܰʮ�ť����k
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

    // �{������
    private void OnDestroy() {
        firebaseManager.auth.StateChanged -= AuthStateChanged;
    }

    // ���Uplay
    public void Play() {
        // �]�w�۾��Ҧ�
        GameManager.gameMode = "spawn";
        // �]�w�j�w���\���_�A���s�^�쭺���ɤ��|�۰ʸ��XBindUI
        BindUI.isBindSuccess = false;

        SceneManager.LoadScene("GameScene");
    }

    // ���UBind
    public void ClickBind() {
        BindUI.Instance.Bind();
    }
}

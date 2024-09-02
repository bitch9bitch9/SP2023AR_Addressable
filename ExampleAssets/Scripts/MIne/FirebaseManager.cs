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

// Firebase�޲z��(����googleSignIn���e�]���H��O�������ۨ��O�H�N�X�A�����~�ٽ������ץ�
public class FirebaseManager : MonoBehaviour {
    // �]�wGoogle����API
    const string GoogleWebAPI = "935824609750-put3ncqvhj4r98fulhvb4mj4qis2tpsu.apps.googleusercontent.com";
    // �]�wGoogle�t�m
    public GoogleSignInConfiguration configuration;

    // �]�w���v
    public Firebase.Auth.FirebaseAuth auth;
    // �]�w�ϥΪ�
    public Firebase.Auth.FirebaseUser user;


    // �T�����(�ݭn�ɨϥΡA�ޥΦbAddToInformation()
    [SerializeField]
    Text infoText;



    private void Awake() {
        // �t�mGoogle�]�w
        configuration = new GoogleSignInConfiguration {
            // �Ȥ��ID = Google����API
            WebClientId = GoogleWebAPI,
            // ID�ШD���\
            RequestIdToken = true,
        };
    }

    void Start() {
        // ���v�]��Firebase�w�]���
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        // ����v�ܰʮ�ť����k
        auth.StateChanged += AuthStateChanged;
    }


    // �@����U��k
    public void Register(string email, string password) {
        // �ЫبϥΪ�
        auth.CreateUserWithEmailAndPasswordAsync(email, password);
    }

    // �@��n�J��k
    public void Login(string email, string password) {
        // �ϥΪ̵n�J
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
            
            // �p�G�n�J����
            if (task.IsFaulted) {
                // �������~�ܼ�
                FirebaseException exception = task.Exception.GetBaseException() as FirebaseException;
                // �p�G���~�T��������
                if (exception != null) {
                    // �p�G���~���޽X == �����ϥΪ̪����޽X
                    if(exception.ErrorCode == (int)AuthError.UserNotFound) {
                        // �ϥε��U��k
                        Register(email, password);
                    }
                }
                return;
            }
        });
    }

    // �n�X��k
    public void Logout() {
        // �]�j�w���\���_
        BindUI.isBindSuccess = false;

        // �P�_�ϥΪ̬O�_��google�b��
        foreach (var userInfo in user.ProviderData) {
            if (userInfo.ProviderId == "google.com") {
                // ���_google�b���w�]�s��
                GoogleDeconnect();
            }
        }

        // �n�X
        auth.SignOut();
    }

    // ���_google�b���w�]�s����k
    public void GoogleDeconnect() {
        // ���_google�b���w�]�s��
        GoogleSignIn.DefaultInstance.SignOut();
    }


    // ���v�ܰʮ�ť����k
    void AuthStateChanged(object sender, System.EventArgs e) {
        // �p�G�{�b����P�ϥΪ̤��P
        if (auth.CurrentUser != user) {
            // �]�ϥΪ̬��{���ϥΪ�
            user = auth.CurrentUser;
        }
    }


    // �����{��
    private void OnDestroy() {
        auth.StateChanged -= AuthStateChanged;
    }


    // ��Google�{�ҧ����ɡA�ڤ��T�w�o�O�����檺�]���ڬO�ۧO�H�N�X��
    public void OnGoogleAuthenticatedFinished(Task<GoogleSignInUser> task) {
        // �n�J���Ѯ�
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

    // ���o�ѼơA�Φb�s���BŪ����Ʈw
    public DatabaseReference GetUserReference() {
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
        return reference;
    }

    // �W�KDebug�T��(�ݭn�ɦۦ�ҥΡA�ثe�Τ���
    public void AddToInformation(string str) { infoText.text = str; }


    // �x�s�j�w�T��
    public void UserSave(string license, string data) {
        // �p�G�ϥΪ̦s�b
        if (user != null) {
            // �x�s���
            GetUserReference().Child(license).Child("modelName").SetValueAsync(data).ContinueWith(task => {
                if (task.IsCompletedSuccessfully) {
                    BindUI.Instance.Hide();
                }
            });
        }
    }
}

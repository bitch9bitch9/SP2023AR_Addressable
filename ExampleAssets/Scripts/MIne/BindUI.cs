using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// �j�w�e��
public class BindUI : MonoBehaviour {

    // ���O���(��i��Ϊ����覡)
    public static BindUI Instance { get; private set; }

    // �j�w�X��J
    [SerializeField]
    InputField bindInput;

    // �j�w���\UI
    [SerializeField]
    BindSuccessUI bindSuccessUI;

    // �j�w�O�_���\
    public static bool isBindSuccess = false;

    private void Awake() {
        // �]�w���
        Instance = this;
    }


    private void Start() {
        // �p�G�j�w���\
        if (isBindSuccess) {
            // Show���\UI
            bindSuccessUI.Show();
            // Show��UI
            Show();
        } else
            // ���æ�UI
            Hide();
    }


    // �h���yQRCode�H�j�w
    public void GoSendQRCodeButton() {
        if (bindInput.text != "") {
            // �]�w�۾��Ҧ�
            GameManager.gameMode = "bind";
            // �N�j�w�X�Ǩ�GameManager�̥H����j�w�X�O�_���ۤǰt
            GameManager.bindCode = bindInput.text;
            // �ޤJ���y�e��
            SceneManager.LoadScene("GameScene");
        }
    }

    // LogIn�e�����U�j�w��
    public void Bind() {
        // ��ܦ�UI
        Show();
        // �p�G�j�w�w���\
        if(isBindSuccess) {
            // show�j�w���\UI
            bindSuccessUI.Show();
        } else {
            // ���øj�w���\UI
            bindSuccessUI.Hide();
        }
    }

    // ����U�����j�w
    public void OnClickCancel() {
        // ���æ�UI
        Hide();
    }

    // ����UI
    public void Hide() {
        this.gameObject.SetActive(false);
    }

    // �q�XUI
    public void Show() { 
        this.gameObject.SetActive(true);
    }
}

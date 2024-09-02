using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// �j�w���\UI
public class BindSuccessUI : MonoBehaviour
{
    // �ޤJfirebaseManager
    [SerializeField]
    FirebaseManager firebaseManager;

    // Dropdown �Ψӱo���j�w���ӯS��
    [SerializeField]
    Dropdown dropdown;

    // List �Ψ��x�s�S�ĦW��
    [SerializeField]
    List<string> prefabs;

    // ��ܯS�ĦW�A�Ω��x�s�ҲզW
    string modelName;

    private void Start() {
        // �}�l�ɽT�O�M�ũҦ��ﶵ
        dropdown.options.Clear();

        // Ū���Ҧ��S�ĦW�٥[�idropdown
        foreach (var prefab in prefabs) {
            dropdown.options.Add(new Dropdown.OptionData() { text = prefab });
        }
    }

    // ����USAVE���s
    public void OnClickSaveB() {
        // ������ޭ�
        int index = dropdown.value;

        // ���3D�ҫ��W
        modelName = dropdown.options[index].text;

        // �x�s�j�w�T��
        BindInfoSave();
    }

    // ���æ�UI
    public void Hide() {
        gameObject.SetActive(false);
    }

    // �q�X��UI
    public void Show() {
        gameObject.SetActive(true);
    }

    // �x�s�j�w�T����k
    private void BindInfoSave() {
        // �x�s�j�w�T��
        firebaseManager.UserSave(GameManager.bindCode, modelName);
    }
}

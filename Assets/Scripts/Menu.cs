using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    private List<Button> guideButtons = new List<Button>();
    private List<TextMeshProUGUI> guideText = new List<TextMeshProUGUI>();

    [SerializeField] private GameObject Guide;

    private void Awake()
    {
        guideButtons = Guide.GetComponentsInChildren<Button>().ToList();
        guideText = Guide.GetComponentsInChildren<TextMeshProUGUI>().ToList();

        for (int i = 0; i < guideButtons.Count; i++)
        {
            int id = i;
            guideButtons[i].onClick.AddListener(()=>ClickBtn(id));
        }
    }
    public void ClickBtn(int id)
    {
        Debug.Log(id+"번째 버튼 클릭");
    }
}
 
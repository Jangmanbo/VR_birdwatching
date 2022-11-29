using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    private List<Button> guideButtons = new List<Button>();
    private List<Image> guideImage = new List<Image>();
    private List<TextMeshProUGUI> guideText = new List<TextMeshProUGUI>();

    [SerializeField] private GameObject Guide;
    [SerializeField] private List<Sprite> birdSprite;

    private void Awake()
    {
        guideButtons = Guide.GetComponentsInChildren<Button>().ToList();
        guideImage = Guide.GetComponentsInChildren<Image>().ToList();
        guideText = Guide.GetComponentsInChildren<TextMeshProUGUI>().ToList();

        // 버튼 리스너 설정
        for (int i = 0; i < guideButtons.Count; i++)
        {
            int id = i;
            guideButtons[i].onClick.AddListener(()=>ClickBtn(id));
        }

        SetGuide();
    }

    private void SetGuide()
    {
        for (int i = 0; i < guideText.Count; i++)
        {
            if (DataController.instance.data.picture[i]>0)
            {
                guideImage[i].sprite = birdSprite[i];
                guideText[i].text = BirdDataParse.GetBirdData(i).Name;
            }
        }
    }

    public void ClickBtn(int id)
    {
        Debug.Log(id+"번째 버튼 클릭");
    }
}
 
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

    private List<Sprite> sprites = new List<Sprite>();   // Detail 화면에서 보일 촬영했던 사진들
    private int idx;    // Detail 화면 사진 현재 idx

    [SerializeField] private GameObject Guide;
    [SerializeField] private GameObject Detail;
    [SerializeField] private Image Picture;
    [SerializeField] private List<Sprite> birdSprite;

    [SerializeField] private ScreenShot screenShot;

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
        for (int id = 0; id < guideText.Count; id++)
        {
            // 1번이라도 찍은 경우 사진/학명 표시
            if (DataController.instance.data.picture[id]>0)
            {
                guideImage[id].sprite = birdSprite[id];
                guideText[id].text = BirdDataParse.GetBirdData(id).Name;
            }
        }
    }

    // Detail화면 설정
    private void SetDetail(int id)
    {
        sprites = screenShot.ReadPictures(id);
        sprites.Insert(0, birdSprite[id]);

        idx = 0;
        SetPicture();
    }

    // 촬영했던 사진 설정
    private void SetPicture(int next = 0)
    {
        idx += next;
        if (idx >= sprites.Count) idx--;
        if (idx < 0) idx = 0;

        Picture.sprite = sprites[idx];
    }

    // ---------------버튼 클릭 리스너---------------
    // 도감에서 새 사진 클릭->Detail화면으로 이동
    public void ClickBtn(int id)
    {
        Debug.Log(id+"번째 버튼 클릭");

        // 열람된 종인 경우만 Detail 확인 가능
        if (DataController.instance.data.picture[id] > 0)
        {
            SetDetail(id);

            Guide.SetActive(false);
            Detail.SetActive(true);
        }
    }

    // 나가기
    public void ClickExitBtn()
    {
        Guide.SetActive(true);
        Detail.SetActive(false);
    }

    // 이전 사진 보기
    public void ClickLeftBtn()
    {
        SetPicture(-1);
    }

    // 다음 사진 보기
    public void ClickRightBtn()
    {
        SetPicture(1);
    }
}
 
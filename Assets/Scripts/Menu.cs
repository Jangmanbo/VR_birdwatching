using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RankInfo
{
    public string name;
    public int point;

    public RankInfo(string _name, int _point)
    {
        name = _name;
        point = _point;
    }
}

public class Menu : MonoBehaviour
{
    private List<Button> guideButtons = new List<Button>();
    private List<Image> guideImage = new List<Image>();
    private List<TextMeshProUGUI> guideText = new List<TextMeshProUGUI>();

    private List<Sprite> sprites = new List<Sprite>();   // Detail 화면에서 보일 촬영했던 사진들
    private int idx;    // Detail 화면 사진 현재 idx


    // UI
    [SerializeField] private GameObject Dictionary, Rank;   // 도감, 랭킹
    [SerializeField] private GameObject Guide, Detail;  // 도감 하위 화면

    [SerializeField] private List<Sprite> birdSprite;
    [SerializeField] private Image Picture;
    [SerializeField] private TextMeshProUGUI[] information;
    [SerializeField] private int[] standard;

    // 스크립트
    [SerializeField] private CFirebase firebase;
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
            guideButtons[i].onClick.AddListener(() => ClickBtn(id));
        }
    }

    private void OnEnable()
    {
        SetGuide();
        SetRank();
    }

    private void SetGuide()
    {
        for (int id = 0; id < guideText.Count; id++)
        {
            // 1번이라도 찍은 경우 사진/학명 표시
            if (DataController.instance.data.picture[id] > 0)
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
        SetInformation(id);
    }

    // 촬영했던 사진 설정
    private void SetPicture(int next = 0)
    {
        idx += next;
        if (idx >= sprites.Count) idx--;
        if (idx < 0) idx = 0;

        Picture.sprite = sprites[idx];
    }

    // 학명, 서식지, 개체수, 특징 등의 정보를 촬영 횟수에 따라 설정
    private void SetInformation(int id)
    {
        Bird bird = BirdDataParse.GetBirdData(id);
        if (DataController.instance.data.picture[id] >= standard[0]) information[0].text = "학명 : " + bird.Name;
        if (DataController.instance.data.picture[id] >= standard[1]) information[1].text = "서식지 : " + bird.Habitat;
        if (DataController.instance.data.picture[id] >= standard[2]) information[2].text = "개체수 : " + bird.Population;
        if (DataController.instance.data.picture[id] >= standard[3]) information[3].text = "특징 : " + bird.Feature;
    }

    private async void SetRank()
    {
        Task<List<RankInfo>> task = firebase.GetUserRankAsync();
        List<RankInfo> ranks = await task;

        foreach (RankInfo rank in ranks)
        {
            Debug.Log($"Ranking {rank.name} : {rank.point}");
        }
    }

    // ---------------버튼 클릭 리스너---------------
    // 도감 클릭
    public void ClickDictionary()
    {
        SetGuide();

        Dictionary.SetActive(true);
        Rank.SetActive(false);
    }


    // 랭킹 클릭
    public void ClickRank()
    {
        SetRank();

        Dictionary.SetActive(false);
        Rank.SetActive(true);
    }


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
 
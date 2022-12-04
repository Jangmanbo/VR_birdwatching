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

    private List<GameObject> rankItems = new List<GameObject>();


    // UI
    [SerializeField] private GameObject Dictionary, Rank;   // 도감, 랭킹

    // 도감 화면
    [SerializeField] private GameObject Guide, Detail;
    [SerializeField] private Image Picture;
    [SerializeField] private TextMeshProUGUI[] information;

    // 랭킹 화면
    [SerializeField] private GameObject rankPrefab;
    [SerializeField] private GameObject scrollContent;

    // 데이터
    [SerializeField] private List<Sprite> birdSprite;
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

    // 파이어베이스에서 랭킹 데이터 받아와서 스크롤뷰에 표시
    private async void SetRank()
    {
        Task<List<RankInfo>> task = firebase.GetRankingAsync();
        List<RankInfo> ranks = await task;

        int ranking = 1;    // 등수
        foreach (RankInfo rank in ranks)
        {
            GameObject item = Instantiate(rankPrefab);  // 오브젝트 복제
            item.transform.SetParent(scrollContent.transform);   // 스크롤뷰의 Contents 오브젝트의 자식으로 배치
            item.transform.localPosition = Vector3.zero;   // scale 조정
            item.transform.localScale = Vector3.one;   // scale 조정
            item.transform.localRotation = Quaternion.identity; // rotation 조정
            rankItems.Add(item);    // 추후 삭제를 위해 리스트에 저장

            // 등수, 이름, 포인트 설정
            List<TextMeshProUGUI> texts=item.GetComponentsInChildren<TextMeshProUGUI>().ToList();
            texts[0].text = ranking.ToString();
            texts[1].text = rank.name;
            texts[2].text = rank.point.ToString();

            ranking++;
            //Debug.Log($"Ranking {rank.name} : {rank.point}");
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
 
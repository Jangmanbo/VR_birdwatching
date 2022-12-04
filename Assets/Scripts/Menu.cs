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

    private List<Sprite> sprites = new List<Sprite>();   // Detail ȭ�鿡�� ���� �Կ��ߴ� ������
    private int idx;    // Detail ȭ�� ���� ���� idx

    private List<GameObject> rankItems = new List<GameObject>();


    // UI
    [SerializeField] private GameObject Dictionary, Rank;   // ����, ��ŷ

    // ���� ȭ��
    [SerializeField] private GameObject Guide, Detail;
    [SerializeField] private Image Picture;
    [SerializeField] private TextMeshProUGUI[] information;

    // ��ŷ ȭ��
    [SerializeField] private GameObject rankPrefab;
    [SerializeField] private GameObject scrollContent;

    // ������
    [SerializeField] private List<Sprite> birdSprite;
    [SerializeField] private int[] standard;

    // ��ũ��Ʈ
    [SerializeField] private CFirebase firebase;
    [SerializeField] private ScreenShot screenShot;

    private void Awake()
    {
        guideButtons = Guide.GetComponentsInChildren<Button>().ToList();
        guideImage = Guide.GetComponentsInChildren<Image>().ToList();
        guideText = Guide.GetComponentsInChildren<TextMeshProUGUI>().ToList();

        // ��ư ������ ����
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
            // 1���̶� ���� ��� ����/�и� ǥ��
            if (DataController.instance.data.picture[id] > 0)
            {
                guideImage[id].sprite = birdSprite[id];
                guideText[id].text = BirdDataParse.GetBirdData(id).Name;
            }
        }
    }

    // Detailȭ�� ����
    private void SetDetail(int id)
    {
        sprites = screenShot.ReadPictures(id);
        sprites.Insert(0, birdSprite[id]);

        idx = 0;
        SetPicture();
        SetInformation(id);
    }

    // �Կ��ߴ� ���� ����
    private void SetPicture(int next = 0)
    {
        idx += next;
        if (idx >= sprites.Count) idx--;
        if (idx < 0) idx = 0;

        Picture.sprite = sprites[idx];
    }

    // �и�, ������, ��ü��, Ư¡ ���� ������ �Կ� Ƚ���� ���� ����
    private void SetInformation(int id)
    {
        Bird bird = BirdDataParse.GetBirdData(id);
        if (DataController.instance.data.picture[id] >= standard[0]) information[0].text = "�и� : " + bird.Name;
        if (DataController.instance.data.picture[id] >= standard[1]) information[1].text = "������ : " + bird.Habitat;
        if (DataController.instance.data.picture[id] >= standard[2]) information[2].text = "��ü�� : " + bird.Population;
        if (DataController.instance.data.picture[id] >= standard[3]) information[3].text = "Ư¡ : " + bird.Feature;
    }

    // ���̾�̽����� ��ŷ ������ �޾ƿͼ� ��ũ�Ѻ信 ǥ��
    private async void SetRank()
    {
        Task<List<RankInfo>> task = firebase.GetRankingAsync();
        List<RankInfo> ranks = await task;

        int ranking = 1;    // ���
        foreach (RankInfo rank in ranks)
        {
            GameObject item = Instantiate(rankPrefab);  // ������Ʈ ����
            item.transform.SetParent(scrollContent.transform);   // ��ũ�Ѻ��� Contents ������Ʈ�� �ڽ����� ��ġ
            item.transform.localPosition = Vector3.zero;   // scale ����
            item.transform.localScale = Vector3.one;   // scale ����
            item.transform.localRotation = Quaternion.identity; // rotation ����
            rankItems.Add(item);    // ���� ������ ���� ����Ʈ�� ����

            // ���, �̸�, ����Ʈ ����
            List<TextMeshProUGUI> texts=item.GetComponentsInChildren<TextMeshProUGUI>().ToList();
            texts[0].text = ranking.ToString();
            texts[1].text = rank.name;
            texts[2].text = rank.point.ToString();

            ranking++;
            //Debug.Log($"Ranking {rank.name} : {rank.point}");
        }
    }

    // ---------------��ư Ŭ�� ������---------------
    // ���� Ŭ��
    public void ClickDictionary()
    {
        SetGuide();

        Dictionary.SetActive(true);
        Rank.SetActive(false);
    }


    // ��ŷ Ŭ��
    public void ClickRank()
    {
        SetRank();

        Dictionary.SetActive(false);
        Rank.SetActive(true);
    }


    // �������� �� ���� Ŭ��->Detailȭ������ �̵�
    public void ClickBtn(int id)
    {
        Debug.Log(id+"��° ��ư Ŭ��");

        // ������ ���� ��츸 Detail Ȯ�� ����
        if (DataController.instance.data.picture[id] > 0)
        {
            SetDetail(id);

            Guide.SetActive(false);
            Detail.SetActive(true);
        }
    }

    // ������
    public void ClickExitBtn()
    {
        Guide.SetActive(true);
        Detail.SetActive(false);
    }

    // ���� ���� ����
    public void ClickLeftBtn()
    {
        SetPicture(-1);
    }

    // ���� ���� ����
    public void ClickRightBtn()
    {
        SetPicture(1);
    }
}
 
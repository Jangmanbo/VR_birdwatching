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

    private List<Sprite> sprites = new List<Sprite>();   // Detail ȭ�鿡�� ���� �Կ��ߴ� ������
    private int idx;    // Detail ȭ�� ���� ���� idx

    [SerializeField] private GameObject Guide;
    [SerializeField] private List<Sprite> birdSprite;


    [SerializeField] private GameObject Detail;
    [SerializeField] private Image Picture;
    [SerializeField] private TextMeshProUGUI[] information;
    [SerializeField] private int[] standard;


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
            guideButtons[i].onClick.AddListener(()=>ClickBtn(id));
        }

        SetGuide();
    }

    private void SetGuide()
    {
        for (int id = 0; id < guideText.Count; id++)
        {
            // 1���̶� ���� ��� ����/�и� ǥ��
            if (DataController.instance.data.picture[id]>0)
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

    // ---------------��ư Ŭ�� ������---------------
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
 
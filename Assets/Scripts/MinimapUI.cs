using UnityEngine;
using UnityEngine.UI;

public class MinimapUI : MonoBehaviour
{
    [SerializeField] Transform left;
    [SerializeField] Transform right;
    [SerializeField] Transform top;
    [SerializeField] Transform bottom;

    [SerializeField] Image minimapImage;
    [SerializeField] Image minimapPlayerImage;

    [SerializeField] PlayerMove player;

    private void Start()
    {
        player = FindObjectOfType<PlayerMove>();
    }

    private void Update()
    {
        //맵의 크기를 파악
        Vector2 mapArea = new Vector2(Vector3.Distance(left.position, right.position), Vector3.Distance(bottom.position ,top.position));
        //맵기준 캐릭터의 위치
        Vector2 charPos = new Vector2(Vector3.Distance(left.position, new Vector3(player.transform.position.x, left.position.y, 0)), Vector3.Distance(bottom.position, new Vector3(bottom.position.x, player.transform.position.y, 0)));
        //맵길이에서 비례된 값
        Vector2 normalPos = new Vector2(charPos.x / mapArea.x , charPos.y / mapArea.y);

        //미니맵에서 캐릭터의 이미지 위치를 설정하는 부분  anchoredPosition == ?  sizeDelta == rectTransform의 크기
        minimapPlayerImage.rectTransform.anchoredPosition = new Vector2(minimapImage.rectTransform.sizeDelta.x * normalPos.x, minimapImage.rectTransform.sizeDelta.y * normalPos.y);
    }
}

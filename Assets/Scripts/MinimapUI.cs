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
        //���� ũ�⸦ �ľ�
        Vector2 mapArea = new Vector2(Vector3.Distance(left.position, right.position), Vector3.Distance(bottom.position ,top.position));
        //�ʱ��� ĳ������ ��ġ
        Vector2 charPos = new Vector2(Vector3.Distance(left.position, new Vector3(player.transform.position.x, left.position.y, 0)), Vector3.Distance(bottom.position, new Vector3(bottom.position.x, player.transform.position.y, 0)));
        //�ʱ��̿��� ��ʵ� ��
        Vector2 normalPos = new Vector2(charPos.x / mapArea.x , charPos.y / mapArea.y);

        //�̴ϸʿ��� ĳ������ �̹��� ��ġ�� �����ϴ� �κ�  anchoredPosition == ?  sizeDelta == rectTransform�� ũ��
        minimapPlayerImage.rectTransform.anchoredPosition = new Vector2(minimapImage.rectTransform.sizeDelta.x * normalPos.x, minimapImage.rectTransform.sizeDelta.y * normalPos.y);
    }
}

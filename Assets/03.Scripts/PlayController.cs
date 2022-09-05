using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayController: MonoBehaviour
{
    [SerializeField] InputField inputTargetBoxNo;
    [SerializeField] Button btnStart;

    [SerializeField] Transform transBoxMover;
    Transform transBoxInFrame;
    const int HALF_WIDTH = 1; // 한칸의 면적 절반

    [SerializeField] Transform transBox;

    private void Start()
    {
        btnStart.onClick.AddListener(OnClickStart);
        transBoxInFrame = transBoxMover.GetChild(1);
        Initialize();
    }

    private void Initialize()
    {
        transBoxMover.position = new Vector3(-1, 0, -1);
        transBoxInFrame.localPosition = new Vector3(0, 0, 0);
        transBox.position = new Vector3(0, 0.7f, 0); // 충돌체크 꼬이는거 방지. 살짝 위에서 박스 떨굼

        SystemManager.Instance.RefreshContainer();
    }

    private void OnClickStart()
    {
        Initialize();
        int targetBoxNo = 0;
        int.TryParse(inputTargetBoxNo.text, out targetBoxNo);

        if(targetBoxNo > 0)
        {
            Debug.Log("타겟 선정 : " + targetBoxNo);
            MoveBox(targetBoxNo);
            
        } else
        {
            Debug.Log("타겟은 1부터입니다");
        }
    }

    private void MoveBox(int targetBoxNo)
    {
        Container container = SystemManager.Instance.FindContainerById(targetBoxNo);
        if(container == null)
        {
            Debug.Log("존재하지 않는 컨테이너 번호입니다.");
            return;
        }
        Vector3 pos = container.GetPosition();

        int distance = (int)pos.z - (int)transBoxMover.position.z;
        StartCoroutine(CoMoveFowardBack(new Vector3(0, 0, distance + HALF_WIDTH), distance * 100, () => {
            if(DataController.Instance.data.frameY > 1) {
                int distance2 = (int)container.GetPosition().y - (int)transBoxInFrame.position.y;
                StartCoroutine(CoMoveUpDown(new Vector3(0, distance2, 0), distance2 * 50, () => {
                    PushBox(container);
                    return true;
                }));
            } else {
                PushBox(container);
            }
            return true;
        }));
    }

    private void PushBox(Container container)
    {
        container.OnSelected();
        int direction = (int)container.GetPosition().x - (int)transBox.position.x;
        StartCoroutine(CoMoveLeftRight(new Vector3(direction, 0, 0)));
    }
    

    IEnumerator CoMoveFowardBack(Vector3 vec, float delay, Func<bool> OnMoveEnd)
    {
        Vector3 dest = transBoxMover.position + vec;
        float elapsedTime = 0.0f;
        while (elapsedTime < delay)
        {
            elapsedTime += Time.deltaTime;
            Vector3 newPos = transBoxMover.position;
            if(dest.x != transBoxMover.position.x)
            {
                newPos.x = Mathf.Lerp(transBoxMover.position.x, dest.x, elapsedTime / delay);
            }
            else if (dest.z != transBoxMover.position.z)
            {
                newPos.z = Mathf.Lerp(transBoxMover.position.z, dest.z, elapsedTime / delay);
            }

            if(transBoxMover.position == newPos)
            {
                break;
            }
            transBoxMover.position = newPos;
            
            yield return null;
        }
        transBoxMover.position = dest;
        if(OnMoveEnd != null)
        {
            OnMoveEnd();
        }
        yield return null;
    }

    IEnumerator CoMoveUpDown(Vector3 vec, float delay, Func<bool> OnMoveEnd)
    {
        Vector3 dest = transBoxInFrame.localPosition + vec;
        float elapsedTime = 0.0f;
        while (elapsedTime < delay)
        {
            elapsedTime += Time.deltaTime;
            Vector3 newPos = transBoxInFrame.localPosition;
            if (dest.y != transBoxInFrame.localPosition.y)
            {
                newPos.y = Mathf.Lerp(transBoxInFrame.localPosition.y, dest.y, elapsedTime / delay);
            }
            if (transBoxInFrame.localPosition == newPos)
            {
                break;
            }
            transBoxInFrame.localPosition = newPos;

            yield return null;
        }
        transBoxInFrame.localPosition = dest;
        if (OnMoveEnd != null)
        {
            OnMoveEnd();
        }
        yield return null;
    }

    IEnumerator CoMoveLeftRight(Vector3 vec)
    {
        Vector3 dest = transBox.position + vec;
        float elapsedTime = 0.0f;
        float delay = 100.0f;
        while (elapsedTime < delay)
        {
            elapsedTime += Time.deltaTime;
            Vector3 newPos = transBox.position;
            newPos.x = Mathf.Lerp(transBox.position.x, dest.x, elapsedTime / delay);

            Debug.Log(newPos);
            if (transBox.position == newPos)
            {
                break;
            }
            transBox.position = newPos;

            yield return null;
        }
        transBox.position = dest;

        yield return null;
    }
}

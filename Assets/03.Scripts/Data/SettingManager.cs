using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingManager : MonoBehaviour
{
    [SerializeField]
    Transform objPanelSetting;
    InputField inputFrameX;
    InputField inputFrameY;
    InputField inputFrameZ;
    Button btnApply;

    [SerializeField]
    [Range(2, 2)]
    int frameX = 2;

    [SerializeField]
    [Range(1, 10)]
    int frameY = 1;

    [SerializeField]
    [Range(2, 10)]
    int frameZ = 3;

    enum eRow { ROW1, ROW2 };

    [SerializeField] Transform transMap;


    private void Start()
    {
        inputFrameX = objPanelSetting.GetChild((int)eRow.ROW1).GetChild(0).GetComponentInChildren<InputField>();
        inputFrameY = objPanelSetting.GetChild((int)eRow.ROW1).GetChild(1).GetComponentInChildren<InputField>();
        inputFrameZ = objPanelSetting.GetChild((int)eRow.ROW1).GetChild(2).GetComponentInChildren<InputField>();
        btnApply = objPanelSetting.GetChild((int)eRow.ROW1).GetChild(3).GetComponent<Button>();

        btnApply.onClick.AddListener(OnClickButtonApply);

        Load();
    }

    private void Load()
    {
        DataController.Instance.LoadData();
        Data data = DataController.Instance.data;
        inputFrameX.text = data.frameX.ToString();
        inputFrameY.text = data.frameY.ToString();
        inputFrameZ.text = data.frameZ.ToString();

        if(data.frameX >=2 && data.frameY >=1 && data.frameZ >= 1)
        {
            ResetContainerLocation(data.frameX, data.frameY, data.frameZ);
        }
    }

    private void OnClickButtonApply()
    {
        Data data = DataController.Instance.data;
        data.frameX = int.Parse(inputFrameX.text);
        data.frameY = int.Parse(inputFrameY.text);
        data.frameZ = int.Parse(inputFrameZ.text);

        DataController.Instance.SaveData();
        ResetContainerLocation(data.frameX, data.frameY, data.frameZ);
    }

    IEnumerator ResetContainerObject(int frameX, int frameY, int frameZ)
    {
        while (true)
        {
            Transform prevFrameY = transMap.Find("frameY");
            if (!prevFrameY)
            {
                break;
            }
            Destroy(prevFrameY.gameObject);

            yield return new WaitForFixedUpdate();
        }
        InitContainerLocation(frameX, frameY, frameZ);
    }
    
    private void ResetContainerLocation(int frameX, int frameY, int frameZ) {
        SystemManager.Instance.ResetContainerPool(transMap);
        StartCoroutine(ResetContainerObject(frameX, frameY, frameZ));
    }

    private void InitContainerLocation(int frameX, int frameY, int frameZ)
    {
        int id = 1;
        const int WIDTH = 2;
        for (int y = 0; y < frameY; y++)
        {
            GameObject frameYObj = new GameObject("frameY");
            for (int z = 0; z < frameZ; z++)
            {
                GameObject frameXObj = new GameObject("frameX");
                for (int x = 0; x < frameX; x++)
                {
                    Vector3 pos = new Vector3((WIDTH + 0.1f) * (x % 2 > 0 ? -1 : 1), y * WIDTH, z * WIDTH);
                    Container container = SystemManager.Instance.GetContainerObj();
                    container.SetId(id++);
                    container.SetPosition(pos);
                    GameObject obj = container.gameObject;
                    obj.transform.Rotate(0, x % 2 > 0 ? -90 : 90, 0);
                    obj.transform.SetParent(frameXObj.transform);
                    obj.transform.position = pos;
                }
                frameXObj.transform.SetParent(frameYObj.transform);

            }
            frameYObj.transform.Translate(0, 0, WIDTH);
            frameYObj.transform.SetParent(transMap);
        }
    }
}

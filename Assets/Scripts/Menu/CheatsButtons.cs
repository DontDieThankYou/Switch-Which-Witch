using UnityEngine;

public class CheatsButtons : MonoBehaviour
{
    [SerializeField] TMPro.TMP_Text text;

    // Update is called once per frame
    void Update()
    {
        text.text = string.Format("{0} sus, {1} para", PlayerController.instance.suspicion, VillageParanoia.instance.paranoia);
    }

    public void AddSuspicion()
    {
        PlayerController.instance.suspicion += 20.0f;
    }

    public void AddParanoia()
    {
        VillageParanoia.instance.paranoia += 20.0f;
    }
}

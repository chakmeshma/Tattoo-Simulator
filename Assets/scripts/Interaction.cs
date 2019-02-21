using UnityEngine;

public class Interaction : MonoBehaviour
{
    public GameObject decalSource;
    public ModelMovement modelMovement;
    public GameObject qualitySettingOptionsContainer;
    public GameObject highPolyModel;
    public GameObject lowPolyModel;


    private void Update()
    {
#if UNITY_STANDALONE || UNITY_WEBGL
        if (Input.GetKeyDown(KeyCode.LeftControl))
            tattooActivate(true);

        if (Input.GetKeyUp(KeyCode.LeftControl))
            tattooActivate(false);
#endif
    }

    #region Tattoo Activation
    public void onTattooButtonPressed()
    {
        tattooActivate(true);
    }

    public void onTattooButtonReleased()
    {
        tattooActivate(false);
    }

    protected void tattooActivate(bool active)
    {
        modelMovement.active = !active;
        decalSource.SetActive(active);
    }
    #endregion

    #region Quality Setting
    public void onQualitySettingButtonClicked()
    {
        if (qualitySettingOptionsContainer.activeSelf)
        {
            qualitySettingOptionsContainer.SetActive(false);
        }
        else
        {
            qualitySettingOptionsContainer.SetActive(true);
        }
    }

    public void onQualityLowSettingButton()
    {
        selectModel(false);
    }

    public void onQualityHighSettingButton()
    {
        selectModel(true);
    }

    protected void selectModel(bool highQuality)
    {
        if (highQuality)
        {
            highPolyModel.SetActive(true);
            lowPolyModel.SetActive(false);

            modelMovement.model = highPolyModel;
        }
        else
        {
            highPolyModel.SetActive(false);
            lowPolyModel.SetActive(true);

            modelMovement.model = lowPolyModel;
        }

        onQualitySettingButtonClicked();
    }
    #endregion
}

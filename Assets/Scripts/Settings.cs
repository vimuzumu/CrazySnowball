using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Settings : MonoBehaviour
{
    private const float BASE_MOVE_VELOCITY = 50f;
    private const float MAX_HORIZONTAL_VELOCITY = 20f;
    private const float SWIPE_SCREEN_RATIO = 0.25f;
    private const int STARTING_SIZE = 1;
    private const float EXP_GAIN_PER_SECOND = 400f;
    private const float EXP_LOSS_PER_HIT = 20f;
    private const float EXP_PER_SIZE_MULTIPLIER = 200f;
    private const float EATING_EXP_MULTIPLIER = 10f;
    private const float MAGNET_RADIUS = 5f;

    public static float baseMoveVelocity = BASE_MOVE_VELOCITY;
    public static float maxHorizontalVelocity = MAX_HORIZONTAL_VELOCITY;
    public static float swipeScreenRatio = SWIPE_SCREEN_RATIO;
    public static int startingSize = STARTING_SIZE;
    public static float expGainPerSecond = EXP_GAIN_PER_SECOND;
    public static float expLossPerHit = EXP_LOSS_PER_HIT;
    public static float expPerSizeMultiplier = EXP_PER_SIZE_MULTIPLIER;
    public static float eatingExpMultiplier = EATING_EXP_MULTIPLIER;
    public static float magnetRadius = MAGNET_RADIUS;

    [SerializeField]
    private TMP_InputField baseSpeedInput;
    [SerializeField]
    private TMP_InputField horizontalSpeedInput;
    [SerializeField]
    private Slider swipeSensitivitySlider;
    [SerializeField]
    private TextMeshProUGUI swipeSensitivityValueText;
    [SerializeField]
    private TMP_InputField startingSizeInput;
    [SerializeField]
    private TMP_InputField expPerSecondInput;
    [SerializeField]
    private TMP_InputField expLossPerHitInput;
    [SerializeField]
    private TMP_InputField nextSizeExpMultiplierInput;
    [SerializeField]
    private TMP_InputField eatingExpMultiplierInput;
    [SerializeField]
    private TMP_InputField magnetRadiusInput;

    void Start()
    {
        PopulateForm();
    }

    private void PopulateForm()
    {
        baseSpeedInput.text = baseMoveVelocity.ToString();
        horizontalSpeedInput.text = maxHorizontalVelocity.ToString();
        swipeSensitivitySlider.value = 1f - swipeScreenRatio;
        swipeSensitivityValueText.text = swipeSensitivitySlider.value.ToString();
        startingSizeInput.text = startingSize.ToString();
        expPerSecondInput.text = expGainPerSecond.ToString();
        expLossPerHitInput.text = expLossPerHit.ToString();
        nextSizeExpMultiplierInput.text = expPerSizeMultiplier.ToString();
        eatingExpMultiplierInput.text = eatingExpMultiplier.ToString();
        magnetRadiusInput.text = magnetRadius.ToString();
    }    

    public void SetDefaults()
    {
        baseMoveVelocity = BASE_MOVE_VELOCITY;
        maxHorizontalVelocity = MAX_HORIZONTAL_VELOCITY;
        swipeScreenRatio = SWIPE_SCREEN_RATIO;
        startingSize = STARTING_SIZE;
        expGainPerSecond = EXP_GAIN_PER_SECOND;
        expLossPerHit = EXP_LOSS_PER_HIT;
        expPerSizeMultiplier = EXP_PER_SIZE_MULTIPLIER;
        eatingExpMultiplier = EATING_EXP_MULTIPLIER;
        magnetRadius = MAGNET_RADIUS;
        PopulateForm();
    }

    public void OpenForm()
    {
        GameManager.GetGameManager().SetGameRunning(false);
        gameObject.SetActive(true);
    }

    public void ExitForm()
    {
        SceneManager.LoadScene(0);
    }

    public void SetBaseMoveSpeed()
    {
        Debug.Log("velocity: " + baseSpeedInput.text);
        baseMoveVelocity = float.Parse(baseSpeedInput.text);
    }

    public void SetMaxHorizontalVelocity()
    {
        maxHorizontalVelocity = float.Parse(horizontalSpeedInput.text);
    }

    public void SetSwipeScreenRatio()
    {
        swipeScreenRatio = 1f - swipeSensitivitySlider.value;
        swipeSensitivityValueText.text = swipeSensitivitySlider.value.ToString("n2");
    }

    public void SetStartingSize()
    {
        startingSize = int.Parse(startingSizeInput.text);
    }

    public void SetExpGainPerSecond()
    {
        expGainPerSecond = float.Parse(expPerSecondInput.text);
    }

    public void SetExpLossPerHit()
    {
        expLossPerHit = float.Parse(expLossPerHitInput.text);
    }

    public void SetExpPerSizeMultiplier()
    {
        expPerSizeMultiplier = float.Parse(nextSizeExpMultiplierInput.text);
    }

    public void SetEatingExpMultiplier()
    {
        eatingExpMultiplier = float.Parse(eatingExpMultiplierInput.text);
    }

    public void SetMagnetRadius()
    {
        magnetRadius = float.Parse(magnetRadiusInput.text);
    }
}

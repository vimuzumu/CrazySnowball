using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Settings : MonoBehaviour
{
    private const float DEFAULT_BASE_MOVE_VELOCITY = 50f;
    private const float DEFAULT_MAX_HORIZONTAL_VELOCITY = 20f;
    private const float DEFAULT_SWIPE_SCREEN_RATIO = 0.25f;
    private const int DEFAULT_STARTING_SIZE = 1;
    private const float DEFAULT_EXP_GAIN_PER_SECOND = 400f;
    private const float DEFAULT_EXP_LOSS_PER_HIT = 20f;
    private const float DEFAULT_EXP_PER_SIZE_MULTIPLIER = 200f;
    private const float DEFAULT_EATING_EXP_MULTIPLIER = 10f;
    private const float DEFAULT_MAGNET_RADIUS = 5f;
    private const float DEFAULT_MAGNET_DURATION = 5f;
    private const int DEFAULT_LEVEL_END_TAP_EXP_INDEX = 1;

    private static float[][] levelEndTapExpOptions = new float[][]
    {
        new float[] { 0.75f, 0.05f },   // High
        new float[] { 0.5f, 0.01f },    // Medium
        new float[] { 0.25f, 0.005f }   // Low
    };

    public static float baseMoveVelocity = DEFAULT_BASE_MOVE_VELOCITY;
    public static float maxHorizontalVelocity = DEFAULT_MAX_HORIZONTAL_VELOCITY;
    public static float swipeScreenRatio = DEFAULT_SWIPE_SCREEN_RATIO;
    public static int startingSize = DEFAULT_STARTING_SIZE;
    public static float expGainPerSecond = DEFAULT_EXP_GAIN_PER_SECOND;
    public static float expLossPerHit = DEFAULT_EXP_LOSS_PER_HIT;
    public static float expPerSizeMultiplier = DEFAULT_EXP_PER_SIZE_MULTIPLIER;
    public static float eatingExpMultiplier = DEFAULT_EATING_EXP_MULTIPLIER;
    public static float magnetRadius = DEFAULT_MAGNET_RADIUS;
    public static float magnetDuration = DEFAULT_MAGNET_DURATION;
    private static int LevelEndTapExpIndex = DEFAULT_LEVEL_END_TAP_EXP_INDEX;

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
    [SerializeField]
    private TMP_InputField magnetDurationInput;
    [SerializeField]
    private TMP_Dropdown levelEndExperiencePerTapDropdown;

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
        magnetDurationInput.text = magnetRadius.ToString();
        levelEndExperiencePerTapDropdown.SetValueWithoutNotify(LevelEndTapExpIndex);
    }    

    public void SetDefaults()
    {
        baseMoveVelocity = DEFAULT_BASE_MOVE_VELOCITY;
        maxHorizontalVelocity = DEFAULT_MAX_HORIZONTAL_VELOCITY;
        swipeScreenRatio = DEFAULT_SWIPE_SCREEN_RATIO;
        startingSize = DEFAULT_STARTING_SIZE;
        expGainPerSecond = DEFAULT_EXP_GAIN_PER_SECOND;
        expLossPerHit = DEFAULT_EXP_LOSS_PER_HIT;
        expPerSizeMultiplier = DEFAULT_EXP_PER_SIZE_MULTIPLIER;
        eatingExpMultiplier = DEFAULT_EATING_EXP_MULTIPLIER;
        magnetRadius = DEFAULT_MAGNET_RADIUS;
        magnetDuration = DEFAULT_MAGNET_DURATION;
        LevelEndTapExpIndex = DEFAULT_LEVEL_END_TAP_EXP_INDEX;
        PopulateForm();
    }

    public void OpenForm()
    {
        GameManager.GetGameManager().SetGameRunning(false);
        gameObject.SetActive(true);
    }

    public void ExitForm()
    {
        RestartLevel();
    }

    public static void RestartLevel()
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

    public void SetMagnetDuration()
    {
        magnetDuration = float.Parse(magnetDurationInput.text);
    }

    public void SetLevelEndTapExp()
    {
        LevelEndTapExpIndex = levelEndExperiencePerTapDropdown.value;
    }

    public static float[] GetLevelEndTapExp()
    {
        return levelEndTapExpOptions[LevelEndTapExpIndex];
    }
}

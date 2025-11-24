using UnityEngine;
using System;

public class ZenitsuStats : MonoBehaviour
{
    public event Action<int> OnLevelChanged;
    public event Action<float, float> OnExperienceChanged;

    [SerializeField] private int _currentLevel = 1;
    [SerializeField] private float _currentExperience = 0f;
    [SerializeField] private float _experienceToNextLevel = 100f;

    public int CurrentLevel
    {
        get { return _currentLevel; }
    }

    public float CurrentExperience
    {
        get { return _currentExperience; }
    }

    public float ExperienceToNextLevel
    {
        get { return _experienceToNextLevel; }
    }

    private void Start()
    {
        OnLevelChanged?.Invoke(_currentLevel);
        OnExperienceChanged?.Invoke(_currentExperience, _experienceToNextLevel);
    }

    public void AddExperience(float amount)
    {
        _currentExperience += amount;
        Debug.Log(amount + "경험치 획득(현재:" + _currentExperience + ")");
        OnExperienceChanged?.Invoke(_currentExperience, _experienceToNextLevel);
        while (_currentExperience >= _experienceToNextLevel)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        _currentLevel++;
        Debug.Log("레벨업! 젠이츠 레벨"+_currentLevel);
        _currentExperience -= _experienceToNextLevel;
        _experienceToNextLevel *= 1.1f;
        OnLevelChanged?.Invoke(_currentLevel);
        OnExperienceChanged?.Invoke(_currentExperience, _experienceToNextLevel);
    }

}

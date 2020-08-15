using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerUI : MonoBehaviour
{
    public Slider HPSlider;
    public Slider utilitySlider;
    public Slider ShiftSkillSlider;
    public Slider QSkillSlider;
    public Slider ESkillSlider;
    public Slider RSkillSlider;

    public void SetHealth(float health)
    {
        HPSlider.value = health;
    }

    public void SetUtility(float utility)
    {
        utilitySlider.value = utility;
    }

    public void UseShiftSkill(float cooldownDuration)
    {
        StartCoroutine(cooldownSliderDuration(cooldownDuration, ShiftSkillSlider));
    }
    public void UseQSkill(float cooldownDuration)
    {
        StartCoroutine(cooldownSliderDuration(cooldownDuration, QSkillSlider));
    }
    public void UseESkill(float cooldownDuration)
    {
        StartCoroutine(cooldownSliderDuration(cooldownDuration, ESkillSlider));
    }
    public void UseRSkill(float cooldownDuration)
    {
        StartCoroutine(cooldownSliderDuration(cooldownDuration, RSkillSlider));
    }

    IEnumerator cooldownSliderDuration(float duration, Slider s)
    {
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime/duration)
        {
            s.value = 1-t;
            yield return null;
        }
        s.value=0;
    }
}

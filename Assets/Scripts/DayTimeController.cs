﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DayTimeController : MonoBehaviour
{
    const float SecondsInDay = 86400f;
    public float time;
    [SerializeField] Text TimeDisplay;
    [SerializeField] float TimeScale;
    [SerializeField] float LightTransition = 0.0001f;
    public int hungerUpdaterCounter;
    public int healthUpdaterCounter;
    public int temperatureUpdateCounter;
    public int day;

    void Start()
    {
        day = 0;
        time = 25200f;
        hungerUpdaterCounter = 0;
        healthUpdaterCounter = 0;
        temperatureUpdateCounter = 0;
        TemperatureController.currentTemperature = 100;
    }

    void Update()
    {
        if (Time.timeScale == 0)
            return;

        // Cập nhật độ đói
        hungerUpdaterCounter += 1;
        if (hungerUpdaterCounter == 250)
        {
            HungerController.currentHunger -= 1;
            hungerUpdaterCounter = 0;
        }

        // Kiểm tra sức khỏe dựa trên độ đói và nhiệt độ
        if (HungerController.currentHunger < 10 || TemperatureController.currentTemperature < 10)
        {
            healthUpdaterCounter += 1;
            if (healthUpdaterCounter == 100)
            {
                // Giảm máu
                HealthController.currentHealth -= 1;
                healthUpdaterCounter = 0;
            }
        }

        // Kiểm soát thời gian và hiển thị
        time += Time.deltaTime * TimeScale;
        int hours = (int)(time / 3600f);
        TimeDisplay.text = hours.ToString("00") + ":00";

        // Điều chỉnh ánh sáng và nhiệt độ theo giờ
        UnityEngine.Rendering.Universal.Light2D light = transform.GetComponent<UnityEngine.Rendering.Universal.Light2D>();

        // Ánh sáng và nhiệt độ từ 4 giờ đến 20 giờ
        if (time > 25200f && time < 72000f)
        {
            light.intensity = 1f;
            TemperatureController.currentTemperature = 100;
        }

        // Ánh sáng và nhiệt độ từ 20 giờ đến 4 giờ
        if ((time > 72000f && time < 86400f) || (time > 0f && time < 18000f))
        {
            if (light.intensity > 0.3f)
            {
                light.intensity -= LightTransition;
            }
            temperatureUpdateCounter += 1;
            if (temperatureUpdateCounter > 50)
            {
                TemperatureController.currentTemperature -= 1;
                temperatureUpdateCounter = 0;
            }
        }

        // Ánh sáng tăng dần từ 4 giờ đến 7 giờ
        if (time > 18000f && time < 25200f)
        {
            if (light.intensity < 1f)
                light.intensity += LightTransition;

            temperatureUpdateCounter += 1;
            if (temperatureUpdateCounter > 50)
            {
                TemperatureController.currentTemperature -= 1;
                temperatureUpdateCounter = 0;
            }
        }

        // Kiểm tra nếu Player chết
        if (HealthController.currentHealth <= 0)
        {
            Application.LoadLevel(3); // Thay đổi sang màn hình game over
        }

        // Điều chỉnh ngày và các yếu tố khác
        if (time > SecondsInDay)
        {
            time = 0;
            day += 1;
            MoneyController.money += 200;
        }

       /* if (day == 9 && time > 25200f)
        {
            Application.LoadLevel(4);
        }*/
        // Kiểm tra nếu đã qua 2 ngày thì chuyển tới màn hình win game
        if (day >= 7)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Victory");
        }
    }

    // Phương thức để giảm máu khi NPC tấn công
    public void ApplyDamageToPlayer(float damage)
    {
        HealthController healthController = FindObjectOfType<HealthController>();
        if (healthController != null)
        {
            healthController.TakeDamage(damage);
        }
    }
}

using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Clase que controla el cronómetro en la escena.
/// </summary>
public class Timer : MonoBehaviour
{
    /// <summary>
    /// El momento en el que se ha iniciado el cronómetro.
    /// </summary>
    float startTime;

    /// <summary>
    /// El tiempo transcurrido desde que se inició el cronómetro.
    /// </summary>
    float timerControl;
    /// <summary>
    /// Los minutos.
    /// </summary>
    string mins;
    /// <summary>
    /// Los segundos.
    /// </summary>
    string secs;
    /// <summary>
    /// Los milisegundos.
    /// </summary>
    string millisecs;
    /// <summary>
    /// El texto que se mostrará en pantalla.
    /// </summary>
    string timerString;

    /// <summary>
    /// El panel que mostrará el texto del cronómetro.
    /// </summary>
    [SerializeField] Text timerText = null;

    private void Start()
    {
        startTime = Time.time;
    }

    private void Update()
    {
        timerControl = Time.time - startTime;
        mins = ((int)timerControl / 60).ToString ("00");
        secs = ((int)timerControl % 60).ToString ("00");
        millisecs = ((int)(timerControl * 100) % 100).ToString("00");

        timerString = string.Format("{00}:{01}:{02}", mins, secs, millisecs);

        timerText.text = timerString.ToString();
    }
}
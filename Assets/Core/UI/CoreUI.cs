﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if TMP
using TMPro;
#endif

namespace TDC.UI
{
    public abstract class CoreUI : MonoBehaviour
    {
        #region Core

        protected void SetString(GameObject objTxt, string text)
        {
#if TMP
            var txt = objTxt.GetComponent<TextMeshProUGUI>();
#else
            var txt = objTxt.GetComponent<Text>();
#endif

            if (txt && txt.text != text)
            {
                txt.text = text;
            }
            else
            {
                return;
            }
        }

        protected void SetTimeString(GameObject objTxt, int _seconds)
        {
#if TMP
            var txt = objTxt.GetComponent<TextMeshProUGUI>();
#else
            var txt = objTxt.GetComponent<Text>();
#endif

            if (txt)
            {
                int minute = _seconds / 60;
                float seconds = _seconds % 60;

                string txtMin = null;
                string txtSec = null;

                if (minute < 10)
                {
                    txtMin = "0" + minute;
                }
                else
                {
                    txtMin = minute.ToString();
                }

                if (seconds < 10)
                {
                    txtSec = "0" + seconds;
                }
                else
                {
                    txtSec = seconds.ToString();
                }


                string result = txtMin + ":" + txtSec;

                if(txt.text != result)
                {
                    txt.text = txtMin + ":" + txtSec;
                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }
        }

        protected void SetImage(Image Img, Sprite Sprt)
        {
            if (Img && Img.sprite != Sprt)
            {
                Img.sprite = Sprt;
            }
            else
            {
                return;
            }
        }

        protected void SetSlider(Slider _Slider, float Value)
        {
            if (_Slider && _Slider.value != Value)
            {
                _Slider.value = Value;
            }
            else
            {
                return;
            }
        }

        protected virtual void Sound()
        {
        }

#region Static

        public static void SetActive(GameObject Obj, bool State)
        {
            if (Obj && Obj.activeSelf != State)
            {
                Obj.SetActive(State);
            }
            else
            {
                return;
            }
        }

#endregion

#endregion
    }
}
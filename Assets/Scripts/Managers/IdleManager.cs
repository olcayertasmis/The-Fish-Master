using System;
using UnityEngine;
using Object = System.Object;

namespace Managers
{
    public class IdleManager : MonoBehaviour
    {
        public static IdleManager Instance;

        [HideInInspector] public int length;
        [HideInInspector] public int strength;
        [HideInInspector] public int offlineEarnings;
        [HideInInspector] public int lengthCost;
        [HideInInspector] public int strengthCost;
        [HideInInspector] public int offlineEarningsCost;
        [HideInInspector] public int wallet;
        [HideInInspector] public int totalGain;

        private int[] _costs = new int[]
        {
            120,
            151,
            197,
            250,
            324,
            414,
            537,
            687,
            892,
            1145,
            1484,
            1911,
            2479,
            3196,
            4148,
            5359,
            6954,
            9000,
            11687
        };

        private void Awake()
        {
            if (IdleManager.Instance) Destroy(gameObject);
            else IdleManager.Instance = this;

            length = -PlayerPrefs.GetInt("Length", 30);
            strength = PlayerPrefs.GetInt("Strength", 3);
            offlineEarnings = PlayerPrefs.GetInt("OfflineEarnings", 3);
            lengthCost = _costs[-length / 10 - 3];
            strengthCost = _costs[strength - 3];
            offlineEarningsCost = _costs[offlineEarnings - 3];
            wallet = PlayerPrefs.GetInt("Wallet", 0);
        }

        private void OnApplicationPause(bool paused)
        {
            if (paused)
            {
                DateTime now = DateTime.Now;
                PlayerPrefs.SetString("Date", string.Empty);
                Debug.Log(now.ToString());
            }
            else
            {
                string @string = PlayerPrefs.GetString("Date", string.Empty);
                if (@string != string.Empty)
                {
                    DateTime date = DateTime.Parse(@string);
                    totalGain = (int)((DateTime.Now - date).TotalMinutes * offlineEarnings + 1.0);
                    //ScreenManager Return
                }
            }
        }

        private void OnApplicationQuit()
        {
            OnApplicationPause(true);
        }

        public void BuyLength()
        {
            length -= 10;
            wallet -= lengthCost;
            lengthCost = _costs[-length / 10 - 3];
            PlayerPrefs.SetInt("Length", -length);
            PlayerPrefs.SetInt("Wallet", wallet);
            //Screen Manager MAIN
        }

        public void BuyStrength()
        {
            strength++;
            wallet -= strengthCost;
            strengthCost = _costs[strength - 3];
            PlayerPrefs.SetInt("Strength", strength);
            PlayerPrefs.SetInt("Wallet", wallet);
            //Screen Manager MAIN
        }

        public void BuyOfflineEarnings()
        {
            offlineEarnings++;
            wallet -= offlineEarningsCost;
            offlineEarningsCost = _costs[offlineEarnings - 3];
            PlayerPrefs.SetInt("OfflineEarnings", offlineEarnings);
            PlayerPrefs.SetInt("Wallet", wallet);
            //Screen Manager MAIN
        }

        public void CollectMoney()
        {
            wallet += totalGain;
            PlayerPrefs.SetInt("Wallet", wallet);
            //Back to main
        }

        public void CollectDoubleMoney()
        {
            wallet += totalGain * 2;
            PlayerPrefs.SetInt("Wallet", wallet);
            //Back to main
        }
    }
}
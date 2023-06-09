using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class ScreenManager : MonoBehaviour
    {
        public static ScreenManager Instance;

        private GameObject _currentScreen;

        public GameObject endScreen;
        public GameObject gameScreen;
        public GameObject mainScreen;
        public GameObject returnScreen;

        public Button lengthButton;
        public Button strengthButton;
        public Button offlineEarnsButton;

        public TextMeshProUGUI lengthCostText;
        public TextMeshProUGUI lengthValueText;
        public TextMeshProUGUI strengthCostText;
        public TextMeshProUGUI strengthValueText;
        public TextMeshProUGUI offlineEarnCostText;
        public TextMeshProUGUI offlineEarnValueText;
        public TextMeshProUGUI gameScreenMoneyText;
        public TextMeshProUGUI endScreenMoneyText;
        public TextMeshProUGUI returnScreenMoneyText;

        private int _gameCount;


        private void Awake()
        {
            if (ScreenManager.Instance) Destroy(base.gameObject);
            else ScreenManager.Instance = this;

            _currentScreen = mainScreen;
        }

        private void Start()
        {
            CheckIdles();
            UpdateTexts();
        }

        public void ChangeScreen(Screens screen)
        {
            _currentScreen.SetActive(false);
            switch (screen)
            {
                case Screens.MAIN:
                    _currentScreen = mainScreen;
                    UpdateTexts();
                    CheckIdles();
                    break;
                case Screens.GAME:
                    _currentScreen = gameScreen;
                    _gameCount++;
                    break;
                case Screens.END:
                    _currentScreen = endScreen;
                    SetEndScreenMoney();
                    break;
                case Screens.RETURN:
                    _currentScreen = returnScreen;
                    SetReturnScreenMoney();
                    break;
            }

            _currentScreen.SetActive(true);
        }

        public void SetEndScreenMoney()
        {
            endScreenMoneyText.text = "$ " + IdleManager.Instance.totalGain;
        }

        public void SetReturnScreenMoney()
        {
            returnScreenMoneyText.text = "$ " + IdleManager.Instance.totalGain + " gained while waiting!";
        }

        public void UpdateTexts()
        {
            gameScreenMoneyText.text = "$ " + IdleManager.Instance.wallet;
            lengthCostText.text = "$ " + IdleManager.Instance.lengthCost;
            lengthValueText.text = -IdleManager.Instance.length + "m";
            strengthCostText.text = "$ " + IdleManager.Instance.strengthCost;
            strengthValueText.text = IdleManager.Instance.strength + " fishes.";
            offlineEarnCostText.text = "$ " + IdleManager.Instance.offlineEarningsCost;
            offlineEarnValueText.text = "$ " + IdleManager.Instance.offlineEarnings + " /min.";
        }

        public void CheckIdles()
        {
            int lengthCost = IdleManager.Instance.lengthCost;
            int strengthCost = IdleManager.Instance.strengthCost;
            int offlineEarningsCost = IdleManager.Instance.offlineEarningsCost;
            int wallet = IdleManager.Instance.wallet;

            lengthButton.interactable = wallet >= lengthCost;
            
            if (wallet < strengthCost) strengthButton.interactable = false;
            else strengthButton.interactable = true;
            
            if (wallet < offlineEarningsCost) offlineEarnsButton.interactable = false;
            else offlineEarnsButton.interactable = true;
        }
    }
}
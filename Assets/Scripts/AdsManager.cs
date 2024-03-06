using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] string _iOSGameId = "5530313";
    [SerializeField] string _androidGameId = "5530312";
    [SerializeField] string _androidAdUnitId = "Rewarded_Android";
    [SerializeField] string _iOSAdUnitId = "Rewarded_iOS";
    string _adUnitId = null; 
    string _gameId = null;
    
    

    [SerializeField] bool _testMode = true;
    private static int countRestarts;
    
    void Awake()
    {   
      
#if UNITY_IOS
        _adUnitId = _iOSAdUnitId;
        _gameId = _iOSGameId;
#elif UNITY_ANDROID
        _adUnitId = _androidAdUnitId;
        _gameId = _androidGameId;
#endif
        Advertisement.Initialize(_gameId, _testMode, this);
        LoadAd();
    }

    

    private void Start() {
        // Advertisement.Initialize(_adUnitId, true);
       
        if (countRestarts != 0 && countRestarts % 3 == 0)
            ShowAd();

        countRestarts++;
    }
    
    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads Initialization Complete.");
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.LogError($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    }
   
 
    // подгрузка рекламы
    public void LoadAd()
    {
        // IMPORTANT! Only load content AFTER initialization (in this example, initialization is handled in a different script).
        Debug.Log("Loading Ad: " + _adUnitId);
        Advertisement.Load(_adUnitId, this);
    }
 
    // подгружена реклама
    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        Debug.Log("Ad Loaded: " + adUnitId);
        
    }
 
    // показать рекламы
    public void ShowAd()
    {
        Advertisement.Show(_adUnitId, this);
    }
 
    // реклама досмотрена и можно награждать ТУТ //////////////
    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        if (adUnitId.Equals(_adUnitId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            Debug.Log("Unity Ads Rewarded Ad Completed");
            
            // Проверяем, что реклама была досмотрена до конца
            if (adUnitId.Equals(_adUnitId) && showCompletionState == UnityAdsShowCompletionState.COMPLETED)
            {
                Debug.Log("Unity Ads Rewarded Ad Completed. Setting game score to +100.");
                SetGameScore(100);
            }
            
            
        }
    }
    
    void SetGameScore(int score)
    {
        PlayerPrefs.SetInt("score", score + 100);
        
        
    }
 
    // фэйл при загрузке рекламы
    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error loading Ad Unit {adUnitId}: {error.ToString()} - {message}");
    }
    // фэйл при показе рекламы
    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
    }
 
    public void OnUnityAdsShowStart(string adUnitId) { }
    public void OnUnityAdsShowClick(string adUnitId) { }
 
}


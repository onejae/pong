using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class AdsController : MonoBehaviour {
	public string androidUnitID;
	public string androidAppID;
	public string iosUnit;
	public string iosAppID;

	InterstitialAd interstitial;

	// Use this for initialization
	void Start () {
		LoadAd ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void Initialize()
	{
		string appId = string.Empty;

		#if UNITY_ANDROID
		appId = androidAppID;
		#elif UNITY_IPHONE
		appId = iosAppID;
		#endif

		// Initialize the Google Mobile Ads SDK.
		MobileAds.Initialize(appId);
	}

	public void LoadAd()
	{
		string adUnitId = string.Empty;

		#if UNITY_ANDROID
		adUnitId = androidUnitID;
		#elif UNITY_IOS
		adUnitId = iosUnitID;
		#endif

		interstitial = new InterstitialAd(adUnitId);

		// Create an empty ad request.
		AdRequest request = new AdRequest.Builder().Build();
		// Load the interstitial with the request.
		interstitial.LoadAd(request);
	}

	public void ShowAd()
	{
		if (interstitial.IsLoaded()) {
			interstitial.Show();
		}
	}
}

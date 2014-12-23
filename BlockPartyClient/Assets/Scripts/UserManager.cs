using Facebook.MiniJSON;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public static class UserManager
{
	static bool initialized;
	static bool loggedIn;
	public static string Name;
	public static string FacebookId;

	public static bool Initialized
	{
		get
		{
			return initialized;
		}
	}

	public static bool LoggedIn
	{
		get
		{
			return loggedIn;
		}
	}

	public static void Initialize()
	{
		FB.Init(OnInitComplete);
	}

	static void OnInitComplete()
	{
		initialized = true;
	}

	public static void Login()
	{
		FB.Login ("public_profile", OnLogin);
	}

	static void OnLogin(FBResult result)
	{
		loggedIn = true;

		FB.API ("/me", Facebook.HttpMethod.GET, OnGetMe);
	}

	static void OnGetMe(FBResult result)
	{
		var dictionary = Json.Deserialize(result.Text) as Dictionary<string, object>;
		Name = dictionary["name"] as string;
		FacebookId = dictionary["id"] as string;

		Debug.Log(Name + " (" + FacebookId + ")");
	}

	public static void SetProfilePicture(Texture2D texture)
	{

	}
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Extensions;
using Firebase.Auth;
using Firebase.Database;

[Serializable]
public class TeamSaveData
{
    public int round;
    public int health;
    public string teamKey;
    public string userID;
}

public class FireBaseManager : MonoBehaviour
{
    FirebaseAuth auth;

    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Exception != null)
                Debug.LogError(task.Exception);

            auth = FirebaseAuth.DefaultInstance;

            if (auth.CurrentUser == null)
                AnonymousSignIn();
        });
    }

    public void SaveTeam(int round, int health, string teamKey)
    {
        TeamSaveData teamData = new TeamSaveData();
        teamData.round = round;
        teamData.health = health;
        teamData.teamKey = teamKey;
        teamData.userID = FirebaseAuth.DefaultInstance.CurrentUser.UserId;
        string json = JsonUtility.ToJson(teamData);

        FirebaseDatabase db = FirebaseDatabase.DefaultInstance;
        db.RootReference.Child("Teams").Child("Round: " + round.ToString()).Child("Health: " + health.ToString()).Child(teamData.userID).SetRawJsonValueAsync(json).ContinueWithOnMainThread(task =>
        {
            if (task.Exception != null)
                Debug.LogWarning(task.Exception);
            else
                Debug.Log("DataTestWrite: Complete");

        });
    }

    private void AnonymousSignIn()
    {
        auth.SignInAnonymouslyAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Exception != null)
            {
                Debug.LogWarning(task.Exception);
            }
            else
            {
                FirebaseUser newUser = task.Result;
                Debug.LogFormat("User signed in successfully: {0} ({1})",
                    newUser.DisplayName, newUser.UserId);
            }
        });
    }

    public string LoadTeam(int round, int health)
    {
        string returnString = "";
        FirebaseDatabase db = FirebaseDatabase.DefaultInstance;
        db.RootReference.Child("Teams").Child("Round: " + round.ToString()).Child("Health: " + health.ToString()).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Exception != null)
            {
                Debug.LogError(task.Exception);
            }

            //here we get the result from our database.
            DataSnapshot snap = task.Result;
            IEnumerable<DataSnapshot> children = snap.Children;

            //And send the json data to a function that can update our game.

            foreach (var item in children)
            {
                if (auth.CurrentUser.UserId == JsonUtility.FromJson<TeamSaveData>(item.GetRawJsonValue()).userID)
                {
                    Debug.Log("<color=red>" + item.GetRawJsonValue() + "</color>");
                }
                else
                    Debug.Log(item.GetRawJsonValue());
            }

            Debug.Log("<color=red>_____________________________________________</color>");
            Debug.Log(snap.GetRawJsonValue());

            TeamSaveData teamData = JsonUtility.FromJson<TeamSaveData>(snap.GetRawJsonValue());
            returnString = teamData.teamKey;
        });

        return returnString;
    }

    // Update is called once per frame
    void Update()
    {

    }
}

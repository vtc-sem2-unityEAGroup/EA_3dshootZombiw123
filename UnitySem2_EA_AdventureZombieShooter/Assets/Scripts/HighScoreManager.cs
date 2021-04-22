using UnityEngine;
using System.Collections;
using System;
using System.Data;
using Mono.Data.Sqlite;
using System.Collections.Generic;
using UnityEngine.UI;

/// <summary>
/// This script handles all communication with the database
/// </summary>
public class HighScoreManager : MonoBehaviour {

    /// <summary>
    /// The connection string, this string tells the path to the database
    /// </summary>
    private string connectionString;

    /// <summary>
    /// This list contains all the highscores
    /// </summary>
    private List<HighScore> highScores = new List<HighScore>();

    /// <summary>
    /// This prefab is used when we need to create a new highscore
    /// </summary>
    public GameObject scorePrefab;

    /// <summary>
    /// This is the parent of all highscore objects
    /// </summary>
    public Transform scoreParent;

    /// <summary>
    /// Indicates how many scores we will show to the player
    /// </summary>
    public int topRanks;

    /// <summary>
    /// The amount of scores we will save in the database
    /// </summary>
    public int saveScores;

    /// <summary>
    /// The name input field
    /// </summary>
    public InputField enterName;
    public InputField tv_playerName;

    /// <summary>
    /// The dialog for entering the players name
    /// </summary>
    public GameObject nameDialog;

	// Use this for initialization

    public string playerName;
	void Start ()
    {
        //Sets the connectionstring as the default datapath inside the assetfolder
        connectionString = "URI=file:" + Application.dataPath + "/HighScoreDB.sqlite";
        //GetComponent<PlayerMovement>().enabled = true;
        //Creates the database if it doesn't exist
        CreateTable();

        //Deletes the extra scores
        DeleteExtraScore();
        
        //Shows the scores to the player
        ShowScores();
	}
	
	// Update is called once per frame
	void Update ()
   {
        if (Input.GetKeyDown(KeyCode.Escape)) //If we press escape then we want to show or hide the entername dialog
        {
            nameDialog.SetActive(!nameDialog.activeSelf);

        }
	}

    /// <summary>
    /// Creates a table if it doesn't exist
    /// </summary>
    private void CreateTable()
    {
        //Creates the connection
        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            //Opens the connection
            dbConnection.Open();

            //Creates a command so that we can execute it on the database
            using (IDbCommand dbCmd = dbConnection.CreateCommand()) 
            {
                //Create the query 
                string sqlQuery = String.Format("CREATE TABLE if not exists HighScores (PlayerID INTEGER PRIMARY KEY  AUTOINCREMENT  NOT NULL  UNIQUE , Name TEXT NOT NULL , Score INTEGER NOT NULL , Date DATETIME NOT NULL  DEFAULT CURRENT_DATE)");

                //Gives the sqlQuery to the command
                dbCmd.CommandText = sqlQuery;

                //Executes the commnad
                dbCmd.ExecuteScalar();

                //Closes the connections
                dbConnection.Close();
            }
        }
    }

    /// <summary>
    /// Is called when the player is pressing the OK Button
    /// </summary>
    public void EnterName()
    {

        if (enterName.text != string.Empty) //Makes sure that we have some text to enter
        {
            int score = UnityEngine.Random.Range(1, 500); //Generates a random score
            playerName = enterName.text;

            InsertScore(enterName.text, score); //Inserts the score in the database
            
            tv_playerName.text = enterName.text;
            enterName.text = string.Empty; //resets the textfield



            ShowScores(); //Gets the scores form the database

        }
    }

    /// <summary>
    /// Inserts  the score into the database
    /// </summary>
    /// <param name="name">The name of the player</param>
    /// <param name="newScore">The player's score</param>
    private void InsertScore(string name, int newScore)
    {
        GetScores(); //Gets the scores from the database

        int hsCount = highScores.Count; //Stores the amount of scores

        if (highScores.Count > 0) //If we have more than 0 highscores
        {
            HighScore lowestScore = highScores[highScores.Count - 1]; //Creates a reference to the lowest score

            //If the lowest score needs to be replaced
            if (lowestScore != null && saveScores > 0 && highScores.Count >= saveScores && newScore > lowestScore.Score)
            {
                DeleteScore(lowestScore.ID); //Deletes the lowest score

                hsCount--; //Reduces the amount of scores, so that we know if we should insert a new score
            }
        }
        if (hsCount < saveScores) //If there is room on the highscore list, then insert a new score
        {
            //Creates a database connection
            using (IDbConnection dbConnection = new SqliteConnection(connectionString)) 
            {
                //Opens the connection
                dbConnection.Open();

                //Creates a database comment
                using (IDbCommand dbCmd = dbConnection.CreateCommand())
                {
                    //Creates a query for inserting the new score
                    string sqlQuery = String.Format("INSERT INTO HighScores(Name,Score) VALUES(\"{0}\",\"{1}\")", name, newScore);
                    
                    dbCmd.CommandText = sqlQuery; //Gives the query to the commandtext
                    dbCmd.ExecuteScalar(); //Executes the query
                    dbConnection.Close();//Closes the connetcion


                }
            }
        }
    }

    /// <summary>
    /// Gets the scores from the database
    /// </summary>
    private void GetScores()
    {
        //Clears the highscore list so that we can get the new scores
        highScores.Clear();

        //Creates a database connection
        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            //Opens the connection
            dbConnection.Open();

            //Creates a database comment
            using (IDbCommand dbCmd = dbConnection.CreateCommand())
            {
                //Selects everything from the highscores
                string sqlQuery = "SELECT * FROM HighScores";

                //feeds the query to the command
                dbCmd.CommandText = sqlQuery;

                //Creates a reader and executes it so that we can load the highscores
                using (IDataReader reader = dbCmd.ExecuteReader())
                {
                    while (reader.Read()) //As long as we have something to read
                    {
                        //Adds  the new highscore to the highscore list
                        highScores.Add(new HighScore(reader.GetInt32(0), reader.GetInt32(2), reader.GetString(1), reader.GetDateTime(3)));
                    }

                    //Closes the connection
                    dbConnection.Close();
                    reader.Close();
                }
            }
        }

        highScores.Sort(); //Sorts the highscore from highest to lowest
    }

    /// <summary>
    /// Deletes a specific entry in the database
    /// </summary>
    /// <param name="id">The scores database id</param>
    private void DeleteScore(int id)
    {
        //Creates a database connection
        using (IDbConnection dbConnection = new SqliteConnection(connectionString))
        {
            dbConnection.Open(); //Opens the connection

            //Creates a database command
            using (IDbCommand dbCmd = dbConnection.CreateCommand())
            {
                //Creates a query
                string sqlQuery = String.Format("DELETE FROM HighScores WHERE PlayerID = \"{0}\"", id);

                //Feeds the query to the command
                dbCmd.CommandText = sqlQuery;

                //Executes the command
                dbCmd.ExecuteScalar();

                //Closes the connection
                dbConnection.Close();


            }
        }
    }

    /// <summary>
    /// Shows the scores to the player
    /// </summary>
    private void ShowScores()
    {
        GetScores(); //Gets the scores from the database

        //Runs through all the scores
        foreach (GameObject score in GameObject.FindGameObjectsWithTag("Score"))
        {
            //Destroyes all the old scores
            Destroy(score);
        }

        for (int i = 0; i < topRanks; i++) //This loops makes sure that we only show the top x sores
        {
            if (i <= highScores.Count - 1) //Makes sure that we don't get an index out of bounds exception
            {
                GameObject tmpObjec = Instantiate(scorePrefab); //Instantiates a new score

                HighScore tmpScore = highScores[i]; //Gets the current highscore

                //Sets the objects score
                tmpObjec.GetComponent<HighScoreScript>().SetScore(tmpScore.Name, tmpScore.Score.ToString(), "#" + (i + 1).ToString());

                tmpObjec.transform.SetParent(scoreParent); //Sets the score of the parent

                tmpObjec.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1); //Makes sure that the object has the correct scale
            }

        }
    }

    /// <summary>
    /// Deletes the extra scores, this is based on the saveScores variable
    /// </summary>
    private void DeleteExtraScore()
    {
        GetScores(); //Gets the current scores

        if (saveScores <= highScores.Count) //if the amount of scores to save is less than the amount of saves scores
        {
            int deleteCount = highScores.Count - saveScores; //Store the number of scores to delete

            highScores.Reverse(); //Reverses the order so that it is easier for us to delete the lowest scores

            using (IDbConnection dbConnection = new SqliteConnection(connectionString)) //Creates a connection
            {
                dbConnection.Open(); //Opens the connection

                using (IDbCommand dbCmd = dbConnection.CreateCommand()) //Creates a command
                {
                    for (int i = 0; i < deleteCount; i++) //Deletes the scores
                    {
                        //Creates the sqlQuery for deleting the highscore
                        string sqlQuery = String.Format("DELETE FROM HighScores WHERE PlayerID = \"{0}\"", highScores[i].ID);

                        //Feeds the query to the commandText
                        dbCmd.CommandText = sqlQuery;

                        dbCmd.ExecuteScalar(); //Executes the command
                    }

                    dbConnection.Close(); //Closes the connection


                }
            }
        }
    }
}

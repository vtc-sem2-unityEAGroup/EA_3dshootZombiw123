using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// This is the highscore class, it is used to handle all our highscores
/// </summary>
class HighScore : IComparable<HighScore>
{
    /// <summary>
    /// The score
    /// </summary>
    public int Score { get; set; }

    /// <summary>
    /// The name of the highscores owner
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// The date the highscore was made
    /// </summary>
    public DateTime Date { get; set; }

    /// <summary>
    /// The highscores database ID
    /// </summary>
    public int ID { get; set; }

    /// <summary>
    /// The Highscore's constructor
    /// </summary>
    /// <param name="id">The highscore's database id</param>
    /// <param name="score">The score</param>
    /// <param name="name">The name of the highscores owner</param>
    /// <param name="date">The date the highscore was created</param>
    public HighScore(int id, int score, string name, DateTime date)
    {
        this.Score = score;
        this.Name = name;
        this.ID = id;
        this.Date = date;
    }

    /// <summary>
    /// Compare to is used to sort the highscores in a list
    /// </summary>
    /// <param name="other">The score, that we are comparing this score to</param>
    public int CompareTo(HighScore other)
    {
        //first > second return -1
        //first < second return 1
        //first == second return 0

        if (other.Score < this.Score) //If the other score is less than this
        {
            return -1;
        }
        else if (other.Score > this.Score) //If the other score is larger than this
        {
            return 1;
        }
        else if (other.Date < this.Date) //If the scores are equal then we need to check the date
        {
            return -1;
        }
        else if (other.Date > this.Date)
        {
            return 1; 
        }

        //we will return 0 if the scores and dates are identical. 
        return 0;
    }
}

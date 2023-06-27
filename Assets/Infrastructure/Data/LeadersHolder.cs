using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.TestMono;

namespace Infrastructure.Data
{
    [Serializable]
    public class LeadersHolder
    {
        public List<ScoreHolder> Leaders;

        public LeadersHolder() => 
            Leaders = new List<ScoreHolder>();

        public void Add(ScoreHolder scoreHolder)
        {
            if (Leaders.Count > 0)
            {
                float min = Leaders.Min(x => x.Points);
                if (min < scoreHolder.Points && Leaders.Count > Constants.MaxLeaderBoardCount)
                    Leaders.Remove(Leaders.Where(x => x.Points == min).First());
            }

            Leaders.Add(scoreHolder);
        }

        public void Sort()
        {
            var count = Leaders.Count;
            for (int i = 0; i < count - 1; i++)
            for (int j = 0; j < count - i - 1; j++)
                if (Leaders[j].Points < Leaders[j + 1].Points)
                    (Leaders[j], Leaders[j + 1]) = (Leaders[j + 1], Leaders[j]);
        }
    }
}
using System.Data.Entity;

namespace StopWatch.Model
{

    public class ParticipantContainer : DbContext
    {
        public ParticipantContainer()
            : base("name=ParticipantContainer")
        {
        }
        public DbSet<Result> Results { get; set; }
    }   
}

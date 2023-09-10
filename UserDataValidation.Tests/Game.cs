using System.Runtime.ConstrainedExecution;
using UserDataValidation;

namespace UserDataValidation.Tests
{

    /// <summary>
    /// Test TInput class, based on video game data.
    /// </summary>
    public class GameInput
    {

        public string Name { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int CopiesSold { get; set; }

        public string? Director { get; set; }

        public List<string> Platforms { get; set; }

        public GameInput(string name, DateTime releaseDate, int copiesSold, string? director,
            List<string> platforms)
        {

            Name = name;
            ReleaseDate = releaseDate;
            CopiesSold = copiesSold;
            Director = director;
            Platforms = platforms;


        }


    }


    /// <summary>
    /// Test TSuccess class, based on video game data.
    /// </summary>
    public class ValidGame
    {
        public string Name { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int CopiesSold { get; set; }

        public string? Director { get; set; }

        public List<string> Platforms { get; set; }

        public ValidGame(string name, DateTime releaseDate, int copiesSold, string? director,
            List<string> platforms)
        {

            Name = name;
            ReleaseDate = releaseDate;
            CopiesSold = copiesSold;
            Director = director;
            Platforms = platforms;

        }

        // In Unit tests, this is used to save time,
        // since this output type is identical to the input type.
        public ValidGame(GameInput input)
        {
            Name = input.Name;
            ReleaseDate = input.ReleaseDate;
            CopiesSold = input.CopiesSold;
            Director = input.Director;
            Platforms = input.Platforms;

        }

    }


    /// <summary>
    /// Parent class for all types of validations
    /// </summary>
    public abstract class GameValidation { }

   

    // Name validations
    public abstract class NameValidation : GameValidation { }

    public class MustBeEntered_NameValidation : NameValidation { }

    public class MaxLength_NameValidation : NameValidation { public static int MAX_LENGTH = 60; }

    // Release Date validations
    public abstract class ReleaseDateValidation : GameValidation { }

    public class ValidYear_ReleaseDateValidation : ReleaseDateValidation { public static int MIN_YEAR = 1970; public static int MAX_YEAR = 2023; }

    // Copies sold validations
    public abstract class CopiesSoldValidation : GameValidation { }

    public class UnderZero_CopiesSoldValidation : CopiesSoldValidation { }

    // Director validations
    public abstract class DirectorValidation : GameValidation { }

    public abstract class StartsWithUpperCase_DirectorValidation : DirectorValidation { }

    // Platforms validations
    public abstract class PlatformsValidation : GameValidation { }
    public class MustHaveOnePlatform_PlatformsValidation : PlatformsValidation { }











}

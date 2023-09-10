using System.Runtime.InteropServices;

namespace UserDataValidation.Tests
{
    [TestClass]
    public class ValidationUnitTests
    {

        /// <summary>
        /// Runs a ruleset that should succeed at every rule and not change the result.
        /// </summary>
        [TestMethod]
        public void Validate_AllMet_InputOutputMatch()
        {

            // Defines ruleset
            Ruleset<ValidGame, GameValidation, GameInput> ruleset
                = new Ruleset<ValidGame, GameValidation, GameInput>();

            // Adds rule: name must be entered
            ruleset.Add(game =>
            {
               if (game.Name != null)
               {
                    return (new OKResult<ValidGame, GameValidation>(new ValidGame(game)), game);
               }
               else
               {
                    return (new ErrorResult<ValidGame, GameValidation>(new MustBeEntered_NameValidation()), null);
               }
            });

            // Adds rule: name must be less than max length
            ruleset.Add(game =>
            {
                if (game.Name.Length < MaxLength_NameValidation.MAX_LENGTH)
                {
                    return (new OKResult<ValidGame, GameValidation>(new ValidGame(game)), game);
                }
                else
                {
                    return (new ErrorResult<ValidGame, GameValidation>(new MaxLength_NameValidation()), null);
                }
            });

            // Adds rule: must have valid release date
            ruleset.Add(game =>
            {
                if (ValidYear_ReleaseDateValidation.MIN_YEAR < game.ReleaseDate.Year & 
                    game.ReleaseDate.Year < ValidYear_ReleaseDateValidation.MAX_YEAR)
                {
                    return (new OKResult<ValidGame, GameValidation>(new ValidGame(game)), game);
                }
                else
                {
                    return (new ErrorResult<ValidGame, GameValidation>(new ValidYear_ReleaseDateValidation()), null);
                }
            });

            // Adds rule: must have a positive number of sold copies
            ruleset.Add(game =>
            {
                if (game.CopiesSold > 0)
                {
                    return (new OKResult<ValidGame, GameValidation>(new ValidGame(game)), game);
                }
                else
                {
                    return (new ErrorResult<ValidGame, GameValidation>(new UnderZero_CopiesSoldValidation()), null);
                }
            });

            // Adds rule: must have at least one platform
            ruleset.Add(game =>
            {
                if (game.Platforms.Count > 0)
                {
                    return (new OKResult<ValidGame, GameValidation>(new ValidGame(game)), game);
                }
                else
                {
                    return (new ErrorResult<ValidGame, GameValidation>(new MustHaveOnePlatform_PlatformsValidation()), null);
                }
            });

            
            // Game to test
            GameInput testGame = new GameInput(
                "The Legend of Zelda: Breath of the Wild",
                new DateTime(2017, 3, 3),
                32350000,
                "Hidemaro Fujibayashi",
                new List<string>() { "Nintendo Switch" }
                );


            // Runs validation
            ValidationResult<ValidGame, GameValidation> testResult = ruleset.Validate(testGame);

            Assert.IsTrue(testResult is OKResult<ValidGame, GameValidation>);

            // Gets result
            OKResult<ValidGame, GameValidation> okResult = testResult as OKResult<ValidGame, GameValidation>;

            Assert.AreEqual(testGame.Name, okResult.Success.Name);
            Assert.AreEqual(testGame.ReleaseDate, okResult.Success.ReleaseDate);
            Assert.AreEqual(testGame.CopiesSold, okResult.Success.CopiesSold);
            Assert.AreEqual(testGame.Director, okResult.Success.Director);
            Assert.AreEqual(testGame.Platforms, okResult.Success.Platforms);

        }


        /// <summary>
        /// Runs a ruleset that should fail due to the name being too long.
        /// </summary>
        [TestMethod]
        public void Validate_NameTooLong_NameError()
        {

            // Defines ruleset
            Ruleset<ValidGame, GameValidation, GameInput> ruleset
                = new Ruleset<ValidGame, GameValidation, GameInput>();

            // Adds rule: name must be less than max length
            ruleset.Add(game =>
            {
                if (game.Name.Length < MaxLength_NameValidation.MAX_LENGTH)
                {
                    return (new OKResult<ValidGame, GameValidation>(new ValidGame(game)), game);
                }
                else
                {
                    return (new ErrorResult<ValidGame, GameValidation>(new MaxLength_NameValidation()), null);
                }
            });


            // Game to test (name is 60+ characters long)
            GameInput testGame = new GameInput(
                "0123456789012345678901234567890123456789012345678901234567890123456789",
                new DateTime(2017, 3, 3),
                32350000,
                "Hidemaro Fujibayashi",
                new List<string>() { "Nintendo Switch" }
                );

            // Runs validation
            ValidationResult<ValidGame, GameValidation> testResult = ruleset.Validate(testGame);

            Assert.IsTrue(testResult is ErrorResult<ValidGame, GameValidation>);

            // Gets result
            ErrorResult<ValidGame, GameValidation> errorResult = testResult as ErrorResult<ValidGame, GameValidation>;

            Assert.IsTrue(errorResult.Error is MaxLength_NameValidation);


        }


        /// <summary>
        /// Runs a ruleset that should fail
        /// due to the release date being before the oldest valid date.
        /// </summary>
        [TestMethod]
        public void Validate_YearTooFarPast_ReleaseDateError()
        {

            // Defines ruleset
            Ruleset<ValidGame, GameValidation, GameInput> ruleset
                = new Ruleset<ValidGame, GameValidation, GameInput>();

            // Adds rule: must have a valid release date
            ruleset.Add(game =>
            {
                if (ValidYear_ReleaseDateValidation.MIN_YEAR < game.ReleaseDate.Year &
                    game.ReleaseDate.Year < ValidYear_ReleaseDateValidation.MAX_YEAR)
                {
                    return (new OKResult<ValidGame, GameValidation>(new ValidGame(game)), game);
                }
                else
                {
                    return (new ErrorResult<ValidGame, GameValidation>(new ValidYear_ReleaseDateValidation()), null);
                }
            });

            // Game to test (game released 1/1/1900)
            GameInput testGame = new GameInput(
                "The Legend of Zelda: Breath of the Wild",
                new DateTime(1900, 1, 1),
                32350000,
                "Hidemaro Fujibayashi",
                new List<string>() { "Nintendo Switch" }
                );

            // Runs validation
            ValidationResult<ValidGame, GameValidation> testResult = ruleset.Validate(testGame);

            Assert.IsTrue(testResult is ErrorResult<ValidGame, GameValidation>);

            // Gets result
            ErrorResult<ValidGame, GameValidation> errorResult = testResult as ErrorResult<ValidGame, GameValidation>;

            Assert.IsTrue(errorResult.Error is ValidYear_ReleaseDateValidation);


        }


        /// <summary>
        /// Runs a ruleset that should succeed
        /// after converting the director name to its correct form.
        /// </summary>
        [TestMethod]
        public void Validate_NameStartsWithLowercase_OutputChangedName()
        {

            // Defines ruleset
            Ruleset<ValidGame, GameValidation, GameInput> ruleset
                = new Ruleset<ValidGame, GameValidation, GameInput>();

            // Adds rule: first letter of director name must be uppercase
            ruleset.Add(game =>
            {
                if (char.IsUpper(game.Director[0]))
                {
                    return (new OKResult<ValidGame, GameValidation>(new ValidGame(game)), game);
                }
                else
                {
                    // Converts name and returns OKResult
                    game.Director = $"{char.ToUpper(game.Director[0])}{game.Director[1..]}";
                    return (new OKResult<ValidGame, GameValidation>(new ValidGame(game)), game);
                }
            });

            // Game to test (lower case director first letter)
            GameInput testGame = new GameInput(
                "The Legend of Zelda: Breath of the Wild",
                new DateTime(1900, 1, 1),
                32350000,
                "hidemaro Fujibayashi",
                new List<string>() { "Nintendo Switch" }
                );

            // Runs validation
            ValidationResult<ValidGame, GameValidation> testResult = ruleset.Validate(testGame);

            Assert.IsTrue(testResult is OKResult<ValidGame, GameValidation>);

            // Gets result
            OKResult<ValidGame, GameValidation> okResult = testResult as OKResult<ValidGame, GameValidation>;

            Assert.AreEqual("Hidemaro Fujibayashi", okResult.Success.Director);


        }


        /// <summary>
        /// Runs a ruleset that should throw an expection
        /// due to the lack of any given rules.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NoSpecifiedRuleException))]
        public void Validate_NoRules_ThrowNoSpecifiedRuleException()
        {
            
            // Defines ruleset
            Ruleset<ValidGame, GameValidation, GameInput> ruleset
                = new Ruleset<ValidGame, GameValidation, GameInput>();

            // Game to test
            GameInput testGame = new GameInput(
                "The Legend of Zelda: Breath of the Wild",
                new DateTime(1900, 1, 1),
                32350000,
                "Hidemaro Fujibayashi",
                new List<string>() { "Nintendo Switch" }
                );

            // Runs validation with no rules
            ValidationResult<ValidGame, GameValidation> testResult = ruleset.Validate(testGame);

        }

    }


}
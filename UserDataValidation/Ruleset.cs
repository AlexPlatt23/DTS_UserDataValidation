using System.ComponentModel.DataAnnotations;

namespace UserDataValidation
{

    /// <summary>
    /// Contains the set of rules (validations) that can be used to validate an
    /// object of the TInput type, returning the resulting ValidationResult.
    /// </summary>
    public class Ruleset<TSuccess, TError, TInput>
    {

        /// <summary>
        /// Delegate for rules that can be added to the ruleset.
        /// </summary>
        public delegate (ValidationResult<TSuccess, TError>, TInput?) Rule(TInput input);



        private readonly List<Rule> _rules = new List<Rule>();



        /// <summary>
        /// Adds rules to the ruleset.
        /// </summary>
        public void Add(Rule rule)
        {
            _rules.Add(rule);
        }

        /// <summary>
        /// Validates the given item, generating a ValidationResult.
        /// </summary>
        /// <param name="item"> The input value to validate. </param>
        /// <returns> An OKResult with the converted output value,
        /// or an ErrorResult with the corresponding error. </returns>
        /// <exception cref="NoSpecifiedRuleException"></exception>
        public ValidationResult<TSuccess, TError> Validate(TInput item)
        {

            ValidationResult<TSuccess, TError> currentResult = null;
            TInput currentItem = item;

            // Loops through each rule
            foreach (var rule in _rules)
            {

                // Evaluates the current rule with the current state of the item
                (ValidationResult<TSuccess, TError>, TInput) evaluation = rule(currentItem);

                // Gets the ValidationResult
                currentResult = evaluation.Item1;

                // Returns it if it is an ErrorResult
                if (currentResult is ErrorResult<TSuccess, TError>)
                {
                    return currentResult;
                }

                // Otherwise updates the current item with the TInput result and loops
                currentItem = evaluation.Item2;


            }

            // If the for loop never started, throw error
            if (currentResult == null)
            {
                throw new NoSpecifiedRuleException("No rules have been specified in this ruleset.");
            }

            // Returns the final result
            return currentResult;
            



        }





    }

    

}

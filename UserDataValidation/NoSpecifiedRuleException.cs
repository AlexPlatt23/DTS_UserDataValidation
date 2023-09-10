namespace UserDataValidation
{
    /// <summary>
    /// Exception for when no rules have been specified for a Ruleset.
    /// </summary>
    public class NoSpecifiedRuleException : Exception
    {
        public NoSpecifiedRuleException() { }

        public NoSpecifiedRuleException(string message) : base(message) { }

        public NoSpecifiedRuleException(System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }


    }
    
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Blog.Domain.ValueObjects
{
    public sealed class Email
    { 
        private static readonly Regex regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$",
            RegexOptions.Compiled | RegexOptions.CultureInvariant);
        public string Value { get; private init; } 

        private Email(string value = "") => Value = value; 
        public static Email Create(string value)
        {
            if(string.IsNullOrWhiteSpace(value))
                throw new ArgumentNullException("Email is required!", nameof(value));

            var normalized = value.Trim().ToLowerInvariant();
            if(!regex.IsMatch(normalized))
                throw new ArgumentNullException("Email Format is Invalid.", nameof(value));

            return new Email(normalized);
        }
        public override bool Equals(object? obj) => 
            obj is Email other && Value.Equals(other.Value, StringComparison.InvariantCultureIgnoreCase);
        //public override int GetHashCode() => Value.GetHashCode(StringComparison.InvariantCultureIgnoreCase);
        //public override int GetHashCode() => Value.ToLowerInvariant().GetHashCode();
        public override int GetHashCode() => HashCode.Combine(Value.ToLowerInvariant());
        public override string ToString() => Value;
    }
}

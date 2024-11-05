namespace app.Domain.Agregate.ObjectValues
{
    public class Address
    {
        public string State { get; }
        public string Street { get; }
        public int Number { get; }
        public string Complement { get; }
        public string City { get; }

        public Address(string state, string street, int number, string complement, string city)
        {
            if (string.IsNullOrWhiteSpace(state) || state.Length != 2)
                throw new ArgumentException("State must be 2 characters.", nameof(state));

            if (string.IsNullOrWhiteSpace(street))
                throw new ArgumentException("Street cannot be empty.", nameof(street));

            if (number <= 0)
                throw new ArgumentException("Number must be greater than zero.", nameof(number));

            if (string.IsNullOrWhiteSpace(city))
                throw new ArgumentException("City cannot be empty.", nameof(city));

            State = state.ToUpper();
            Street = street;
            Number = number;
            Complement = complement;
            City = city;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(State, Street, Number, Complement, City);
        }

        public override string ToString()
        {
            return $"{Street}, {Number} {Complement}, {City} - {State}";
        }
    }

}

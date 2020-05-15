namespace PilkarzeMVVM.Model
{
    internal class Pilkarz
    {

        #region properties

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public double Age { get; set; }
        public double Weight { get; set; }
        #endregion


        #region constructors

        public Pilkarz()
        {
        }
        public Pilkarz(string firstName, string lastName, double age, double weight)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Age = age;
            this.Weight = weight;
        }
        #endregion

        #region methods

        public override string ToString()
        {
            return $"{FirstName} {LastName}, Wiek: {Age} lat, {Weight}kg";
        }

        public void Copy(Pilkarz player)
        {
            FirstName = player.FirstName;
            LastName = player.LastName;
            Age = player.Age;
            Weight = player.Weight;
        }
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            Pilkarz player = obj as Pilkarz;
            return (this.Age == player.Age && this.FirstName == player.FirstName && this.LastName == player.LastName
                && this.Weight == player.Weight);
        }
        #endregion
    }
}
namespace StudentApi
{
    public class RandomService
    {
        public RandomService()
        {
            RandomNumber = Random.Shared.Next(0, 100);
        }

        public int RandomNumber { get; }
    }
}

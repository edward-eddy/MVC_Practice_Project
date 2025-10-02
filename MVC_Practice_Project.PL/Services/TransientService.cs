namespace MVC_Practice_Project.PL.Services
{
    public class TransientService : ITransientService
    {
        public TransientService()
        {
            Guid = Guid.NewGuid();
        }
        public Guid Guid { get; set; }

        public string GetGuid()
        {
            return Guid.ToString();
        }
    }
}

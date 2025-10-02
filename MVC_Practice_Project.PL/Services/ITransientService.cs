namespace MVC_Practice_Project.PL.Services
{
    public interface ITransientService
    {
        public Guid Guid { get; set; }
        string GetGuid();
    }
}

namespace ParallelSeleniumUsingNUnit.PageModels
{
    public interface IGooglePageModel
    {
        IGooglePageModel Search(string search);
        bool PageContains(string content);
        void Close();
    }
}
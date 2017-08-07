namespace WScriptParser.Functions.Actions
{
    public interface IActionFunction : IFunction, IAction
    {
        string Token { get; set; }
    }
}

namespace cycle_analysis.Domain.User
{
    using cycle_analysis.Domain.User.Models;

    public interface ICurrentUserProvider
    {
        CurrentUserDetail Get();
    }
}

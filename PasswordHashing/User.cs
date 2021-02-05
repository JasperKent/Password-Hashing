namespace PasswordHashing
{
    public record User (string Username, string PasswordHash, int Salt)
    {
        public User() : this(null, null, 0) { }
    }
}

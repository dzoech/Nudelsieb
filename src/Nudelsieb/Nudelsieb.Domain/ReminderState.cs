namespace Nudelsieb.Domain
{
    public enum ReminderState
    {
        /// <summary>
        /// The reminder hasn't been sent to the user yet.
        /// </summary>
        Waiting,

        /// <summary>
        /// The reminder has been sent to the user but the user hasn't interacted with it.
        /// </summary>
        Active,

        /// <summary>
        /// The user has disabled the reminder.
        /// </summary>
        Disabled
    }
}

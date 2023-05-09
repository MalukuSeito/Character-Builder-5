using CB5e.Services;
using OGL;

namespace CB5e
{
    public static class Extensions
    {
        public static void Forget(this Task task)
        {
            task.ContinueWith(
                t => {
                    ConfigManager.LogError(t.Exception);
                },
                TaskContinuationOptions.OnlyOnFaulted);
        }

		public static string PlusMinus(this int value)
		{
            if (value < 0) return value.ToString();
            return $"+{value}";
		}

		public static string? PlusMinus(this int? value)
		{
            if (value == null) return null ;
			if (value < 0) return value.ToString();
			return $"+{value}";
		}

		public static bool HasAnyFlag(this ChangeType changeType, ChangeType other, ChangeType other2 = 0)
        {
            return (changeType & (other | other2)) != 0;
        }
	}
}

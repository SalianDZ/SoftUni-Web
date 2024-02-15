namespace Homies.Data
{
	public static class ValidationConstants
	{
		public static class EventValidations
		{
			public const int NameMinLength = 5;
			public const int NameMaxLength = 20;

			public const int DescriptionMinLength = 15;
			public const int DescriptionMaxLength = 150;

			public const string DateFormat = "yyyy-MM-dd H:mm";
		}

		public static class TypeValidations
		{
			public const int NameMinLength = 5;
			public const int NameMaxLength = 15;
		}
	}
}
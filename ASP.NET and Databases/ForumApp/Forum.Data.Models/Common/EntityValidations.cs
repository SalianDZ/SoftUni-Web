﻿namespace Forum.Data.Models.Common
{
    public static class EntityValidations
    {
        public static class Post
        {
            public const int TitleMaxLength = 50;
            public const int TitleMinLength = 30;
            public const int ContentMaxLength = 1500;
            public const int ContentMinLength = 30;
        }
    }
}

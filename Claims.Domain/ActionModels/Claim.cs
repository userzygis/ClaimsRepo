﻿namespace Claims.Domain.ActionModels
{
    public class Claim
    {
        public string? Id { get; set; }

        public string CoverId { get; set; }

        public DateTime Created { get; set; }

        public string Name { get; set; }

        public ClaimType Type { get; set; }

        public decimal DamageCost { get; set; }

    }

    public enum ClaimType
    {
        Collision = 0,
        Grounding = 1,
        BadWeather = 2,
        Fire = 3
    }
}

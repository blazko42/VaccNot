namespace VaccNot.Models
{
    public class VaccineSlot
    {
        public string Address { get; set; }
        public int? AvailableSlots { get; set; }
        public int? BoosterDays { get; set; } // pfizer -21; moderna - 28; astraZeneca - 56
        public int? BoosterId { get; set; } //pfizer -1; moderna - 2; astrazeneca- 3
        public string Code { get; set; }
        public string CountyId { get; set; }
        public string CountyName { get; set; }
        public int? Id { get; set; }
        public string LocalityId { get; set; }
        public string LocalityName { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public bool UsesWaitingList { get; set; }
        public int? WaitingListSize { get; set; }

    }

    public enum Booster : int
    {
        Pfizer = 1,
        Moderna = 2,
        AstraZeneca = 3
    }
}
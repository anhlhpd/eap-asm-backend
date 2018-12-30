using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Models
{
    public class Mark
    {
        private static readonly int MaxTheory = 100; // 100%.
        private static readonly int MaxPratice = 15;
        private static readonly int MaxAssignment = 10;
        private static readonly int PercentToPass = 40;

        public Mark() { }

        public Mark(MarkType type, int value)
        {
            this.MarkType = type;
            this.Value = value;
            this.GenerateStatus();
            this.CreatedAt = DateTime.Now;
            this.UpdatedAt = DateTime.Now;
        }

        public Mark(MarkType type, int value, string subjectId, string accountId)
        {
            this.SubjectId = subjectId;
            this.AccountId = accountId;
            this.MarkType = type;
            this.Value = value;
            this.GenerateStatus();
            this.CreatedAt = DateTime.Now;
            this.UpdatedAt = DateTime.Now;
        }

        private void GenerateStatus() {
            int max = 0;
            switch (this.MarkType) {
                case MarkType.Theory:
                    max = MaxTheory;
                    break;
                case MarkType.Practice:
                    max = MaxPratice;
                    break;
                case MarkType.Assignment:
                    max = MaxAssignment;
                    break;
            }
            double percent = (this.Value / max) * 100;
            this.Status = percent >= PercentToPass ? MarkStatus.Pass : MarkStatus.Fail;
        }

        [Key]
        [Required]
        public long Id { get; set; }
        [Required(ErrorMessage = "Please input account")]
        public string AccountId { get; set; }
        [Required(ErrorMessage = "Please input subject")]
        public string SubjectId { get; set; }
        [Required(ErrorMessage = "Please input mark value"),
            Range(0, 15, ErrorMessage = "Please input valid mark value")]
        public float Value { get; set; }
        [Required(ErrorMessage = "Please input mark type")]
        public MarkType MarkType { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public MarkStatus Status { get; set; }
        public Account Account { get; set; }
        public Subject Subject { get; set; }
    }

    public enum MarkStatus
    {
        Fail = 1,
        Pass = 0,
        Chosen = 2
    }

    public enum MarkType
    {
        Theory = 0,
        Practice = 1,
        Assignment = 2
    }
}

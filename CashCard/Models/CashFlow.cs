using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CashCard.Models
{
    public abstract class CashFlow
    {
        public int Id { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime Date { get; set; }
        public string Note { get; set; }
        public StateCashFlow State { get; protected set; }
        public string LogNote { get; set; }
        [ForeignKey("CutOffId")]
        public virtual CutOff CutOff { get; set; }
        public int CutOffId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
        public string UserId { get; set; }
        [ForeignKey("SuperVisorId")]
        public ApplicationUser SuperVisor { get; set; }
        public string SuperVisorId { get; set; }
        public int Total { get; protected set; }

        public abstract void SetTotal();

        public void SetToDraft()
        {
            if (State == StateCashFlow.Revision || State == StateCashFlow.Draft)
            {
                State = StateCashFlow.Draft;
            }
            else
            {
                throw new Exception("Operation not permited");
            }
        }
        public void SetToFinal()
        {
            if (State == StateCashFlow.Draft || State == StateCashFlow.Revision )
            {
                State = StateCashFlow.Final;
            }
            else
            {
                throw new Exception("Operation not permited");
            }
        }
        public void SetToApprove()
        {
            if (State == StateCashFlow.Final )
            {
                State = StateCashFlow.Approve;
            }
            else
            {
                throw new Exception("Operation not permited");
            }
        }
        public void SetToReject()
        {
            if (State == StateCashFlow.Final)
            {
                State = StateCashFlow.Reject;
            }
            else
            {
                throw new Exception("Operation not permited");
            }
        }

        public void SetToRevision()
        {
            if (State == StateCashFlow.Final)
            {
                State = StateCashFlow.Revision;
            }
            else
            {
                throw new Exception("Operation not permited");
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CashCard.Models
{
    public abstract class CashCard
    {
        private IList<ImageData> _imageDatas;

        protected CashCard()
        {
            Date = DateTime.Now.Date;
            _imageDatas = new List<ImageData>();
        }
        public int Id { get; set; }
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime Date { get; set; }
        public string Note { get; set; }
        public StateCashCard State { get; protected set; }
        public string LogNote { get; set; }
        [ForeignKey("CutOffId")]
        public virtual CutOff CutOff { get; set; }
        public int CutOffId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }
        public string UserId { get; set; }
        [ForeignKey("SuperVisorId")]
        public virtual ApplicationUser SuperVisor { get; set; }
        public string SuperVisorId { get; set; }
        [DisplayFormat(DataFormatString = "{0:N0}", ApplyFormatInEditMode = true)]
        public int Total { get; protected set; }

      
        public IList<ImageData> ImageDatas { get { return _imageDatas; } set { _imageDatas = value; } }
        public abstract void SetTotal();

        public void SetToDraft()
        {
            if (State == StateCashCard.Revision || State == StateCashCard.Draft)
            {
                State = StateCashCard.Draft;
            }
            else
            {
                throw new Exception("Operation not permited");
            }
        }
        public void SetToFinal()
        {
            if (State == StateCashCard.Draft || State == StateCashCard.Revision )
            {
                State = StateCashCard.Final;
            }
            else
            {
                throw new Exception("Operation not permited");
            }
        }
        public void SetToApprove()
        {
            if (State == StateCashCard.Final )
            {
                State = StateCashCard.Approve;
            }
            else
            {
                throw new Exception("Operation not permited");
            }
        }
        public void SetToReject()
        {
            if (State == StateCashCard.Final)
            {
                State = StateCashCard.Reject;
            }
            else
            {
                throw new Exception("Operation not permited");
            }
        }

        public void SetToRevision()
        {
            if (State == StateCashCard.Final)
            {
                State = StateCashCard.Revision;
            }
            else
            {
                throw new Exception("Operation not permited");
            }
        }
    }
    public class ImageData
    {
        public int Id { get; set; }
        public string Info { get; set; }
        public string FileName { get; set; }
        [ForeignKey("CashCardId")]
        public virtual CashCard CashCard { get; set; }
        public int CashCardId { get; set; }
       

    }
}
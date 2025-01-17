﻿using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.BaseImpl.EF;

namespace XAFSecurityBenchmark.Models.EFCore {

    public class Task : BaseObject, ITask {

        public virtual DateTime? DateCompleted { get; set; }
        public virtual String Subject { get; set; }

        [FieldSize(FieldSizeAttribute.Unlimited)]
        public virtual String Description { get; set; }

        public virtual DateTime? DueDate { get; set; }
        public virtual DateTime? StartDate { get; set; }

        [Browsable(false)]
        [NotMapped]
        public int Status_Int { get; set; }

        public virtual int PercentCompleted { get; set; }
        public virtual Party AssignedTo { get; set; }

        private TaskStatus status;

        [Column(nameof(Status_Int))]
        public virtual TaskStatus Status {
            get {
                return status;
            }
            set {
                status = value;
                if (isLoaded) {
                    if (value == TaskStatus.Completed) {
                        DateCompleted = DateTime.Now;
                    } else {
                        DateCompleted = null;
                    }
                }
            }
        }

        [Action(ImageName = "State_Task_Completed")]
        public void MarkCompleted() {
            Status = TaskStatus.Completed;
        }

        #region ITask

        DateTime ITask.DueDate { get { return DueDate.GetValueOrDefault(); } set { DueDate = value; } }
        DateTime ITask.StartDate { get { return StartDate.GetValueOrDefault(); } set { StartDate = value; } }
        TaskStatus ITask.Status { get { return Status; } set { Status = value; } }
        DateTime ITask.DateCompleted { get { return DateCompleted.GetValueOrDefault(); } }

        #endregion

        #region IXafEntityObject

        private bool isLoaded = false;
        public override void OnLoaded() {
            isLoaded = true;
        }

        #endregion
    }
}

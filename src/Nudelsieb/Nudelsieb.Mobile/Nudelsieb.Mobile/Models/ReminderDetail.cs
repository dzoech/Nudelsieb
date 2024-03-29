﻿using System.Runtime.Serialization;
using Xamarin.Forms.Internals;

namespace Nudelsieb.Mobile.Models
{
    /// <summary>
    /// Model for recent chat
    /// </summary>
    [Preserve(AllMembers = true)]
    [DataContract]
    public class ReminderDetail
    {
        private string imagePath;

        /// <summary>
        /// Gets or sets the profile image path.
        /// </summary>
        [DataMember(Name = "imagePath")]
        public string ImagePath
        {
            get
            {
                return App.ImageServerPath + this.imagePath;
            }

            set
            {
                this.imagePath = value;
            }
        }

        /// <summary>
        /// Gets or sets the profile name.
        /// </summary>
        [DataMember(Name = "senderName")]
        public string SenderName { get; set; }

        /// <summary>
        /// Gets or sets the message sent/received time.
        /// </summary>
        [DataMember(Name = "time")]
        public string Time { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        [DataMember(Name = "message")]
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the MessageType.
        /// </summary>
        [DataMember(Name = "messageType")]
        public string MessageType { get; set; }

        /// <summary>
        /// Gets or sets the message sent/received notification type.
        /// </summary>
        [DataMember(Name = "notificationType")]
        public string NotificationType { get; set; }

        /// <summary>
        /// Gets or sets the recipient's available status.
        /// </summary>
        [DataMember(Name = "availableStatus")]
        public string AvailableStatus { get; set; }
    }
}

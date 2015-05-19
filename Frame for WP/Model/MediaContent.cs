using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Frame_for_WP.Model
{
    /* Class representation of the media in the application */
    public class MediaContent
    {
        private bool fileType;
        public bool FileType
        {
            get { return fileType; }
        }

        private int rating;
        public int Rating
        {
            get { return rating; }
            set { rating = value; }
        }

        private int flagCount;
        public int FlagCount
        {
            get { return flagCount; }
        }

        private DateTime timestamp;
        public DateTime Timestamp
        {
            get { return timestamp; }
        }

        private WriteableBitmap bitmap;
        public WriteableBitmap Bitmap
        {
            get { return bitmap; }
        }

        private bool hasBeenVoted = false;
        public bool HasBeenVoted
        {
            get { return hasBeenVoted; }
            set { hasBeenVoted = value; }
        }

        private bool hasBeenFlagged = false;
        public bool HasBeenFlagged
        {
            get { return hasBeenFlagged; }
            set { hasBeenFlagged = value; }
        }

        private int databaseId;
        public int DatabaseId
        {
            get { return databaseId; }
        }

        public MediaContent(bool fileType, int dbId, DateTime timestamp, WriteableBitmap bitmap, int rating)
        {
            this.fileType = fileType;
            this.timestamp = timestamp;
            this.bitmap = bitmap;

            this.rating = rating;
            this.flagCount = 0;
            this.databaseId = dbId;
        }

        public MediaContent()
        {
            // TODO Auto-generated constructor stub
        }

        /* This section contains getters and/or setters for class properties */
        public void IncrementRating()
        {
            this.rating++;
        }

        public void DecrementRating()
        {
            this.rating--;
        }

        public void IncrementFlagCount()
        {
            this.flagCount++;
        }
    }

}

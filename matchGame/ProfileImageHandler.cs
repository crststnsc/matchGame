using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace matchGame
{
    internal class ProfileImageHandler
    {
        private List<string> profilePicturePaths = new List<string>();
        private int currentPictureIndex = -1;
        public BitmapImage imgProfilePicture;

        public ProfileImageHandler()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "pictures");
            LoadProfilePicturesFromFolder(path);
        }

        public void LoadProfilePicturesFromFolder(string folderPath)
        {

            // Get a list of all image files in the folder
            string[] imageFiles = Directory.GetFiles(folderPath, "*.jpg", SearchOption.AllDirectories);

            // Add each image file path to the profilePicturePaths list
            foreach (string imagePath in imageFiles)
            {
                profilePicturePaths.Add(imagePath);
            }
            currentPictureIndex = 0;

            LoadProfilePicture();
        }

        private void LoadProfilePicture()
        {
            // Check if there are any profile pictures
            if (profilePicturePaths.Count > 0)
            {
                // Load the currently selected profile picture
                imgProfilePicture = new BitmapImage(new Uri(profilePicturePaths[currentPictureIndex]));
            }
        }

        public void NextProfilePicture()
        {
            // Check if there are any profile pictures
            if (profilePicturePaths.Count > 0)
            {
                // Increment the current picture index
                currentPictureIndex++;

                // Check if the current picture index is greater than the number of profile pictures
                if (currentPictureIndex >= profilePicturePaths.Count)
                {
                    // Reset the current picture index
                    currentPictureIndex = 0;
                }

                LoadProfilePicture();
            }
        }
        
        public void PreviousProfilePicture()
        {
            // Check if there are any profile pictures
            if (profilePicturePaths.Count > 0)
            {
                // Decrement the current picture index
                currentPictureIndex--;

                // Check if the current picture index is less than 0
                if (currentPictureIndex < 0)
                {
                    // Set the current picture index to the last picture
                    currentPictureIndex = profilePicturePaths.Count - 1;
                }

                LoadProfilePicture();
            }
        }

        public List<string> GetImagePaths()
        {
            return profilePicturePaths;
        }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Frame_for_WP.Model
{
    public class JSONMessage
    {
        public static BitmapImage decodeBase64(string input)
        {
            byte[] decodedByte = Convert.FromBase64String(input);

            using(MemoryStream ms = new MemoryStream(decodedByte, 0, decodedByte.Length))
            {
                ms.Write(decodedByte, 0, decodedByte.Length);
                BitmapImage image = new BitmapImage();
                image.SetSource(ms);
                return image;
            }
        }

        /*private static int calculateInSampleSize(BitmapFactory.Options options, int reqWidth, int reqHeight) 
	    {
	        // Raw height and width of image
	        final int height = options.outHeight;
	        final int width = options.outWidth;
	        int inSampleSize = 1;
	
	        if (height > reqHeight || width > reqWidth) 
	        {
	
	            final int halfHeight = height / 2;
	            final int halfWidth = width / 2;
	
	            // Calculate the largest inSampleSize value that is a power of 2 and keeps both
	            // height and width larger than the requested height and width.
	            while ((halfHeight / inSampleSize) > reqHeight
	                    && (halfWidth / inSampleSize) > reqWidth) 
	            {
	                inSampleSize *= 2;
	            }
	        }

	        return inSampleSize;
	    }*/

        //FRONTEND
        public static string commentToJson(int id, string comment, string user)
        {
            JSONComment c = new JSONComment(1, id, comment, user);

            return JsonConvert.SerializeObject(c);
        }

        //FRONTEND
        public static string getCommentsFromDatabase(int postID)
        {
            JSONGetComments c = new JSONGetComments(1, 1, postID);

            return JsonConvert.SerializeObject(c);
        }

        private static string encodeToBase64(BitmapImage image, bool text)
        {
            WriteableBitmap wb = new WriteableBitmap(image);
            MemoryStream ms = new MemoryStream();

            if (!text)
                wb.SaveJpeg(ms, wb.PixelWidth, wb.PixelHeight, 0, 60);
            else
                wb.SaveJpeg(ms, wb.PixelWidth, wb.PixelHeight, 0, 100);

            byte[] bArray = ms.ToArray();
            return Convert.ToBase64String(bArray, 0, bArray.Length);
        }

        //create json media messages to be sent to client 
        public static string clientPictureToJson(BitmapImage pic, double lat, double lon, string user, string tags, bool text)
        {
            string pic_as_string = encodeToBase64(pic, text);
            JSONPicture c = new JSONPicture(1, pic_as_string, lat, lon, user, tags);

            return JsonConvert.SerializeObject(c);
        }

        public static string getPosts(int bottomId, string filter, double lat, double lon, int sort)
        {
            JSONGetPosts c = new JSONGetPosts(1, bottomId, lat, lon, filter, sort);

            return JsonConvert.SerializeObject(c);
        }

        /*public static JSONObject clientVideoToJson(Object vid, Double lat, Double lon, String user, String[] tags)
        {
            JSONObject jo = new JSONObject();

            try
            {
                jo.put("POST", 1);
                jo.put("Video", vid);
                jo.put("Lat", lat);
                jo.put("Lon", lon);
                jo.put("User", user);
                jo.put("Tags", tags);
            }
            catch (JSONException e)
            {
                e.printStackTrace();
            }

            return jo;
        }*/

        /*public static JSONObject clientComment(String text, String user, int id)
        {
            JSONObject jo = new JSONObject();

            try
            {
                jo.put("POST", 1);
                jo.put("Comment", text);
                jo.put("User", user);
                jo.put("ID", id);
            }
            catch (JSONException e)
            {
                e.printStackTrace();
            }

            return jo;
        }

        //getter methods
        public static String[] clientGetImage(JSONObject jo)
        {
            try
            {
                JSONArray jsonArray = (JSONArray)jo.get("Picture");
                List<String> list = new ArrayList<String>();
                for (int i = 0; i < jsonArray.length(); i++)
                {
                    list.add(jsonArray.getString(i));
                }
                String[] arrayString = list.toArray(new String[list.size()]);
                return arrayString;
            }
            catch (JSONException e)
            {
                e.printStackTrace();
            }

            return null;
        }

        public static Object getImage(JSONObject jo)
        {
            try
            {
                return jo.get("Picture");
            }
            catch (JSONException e)
            {
                e.printStackTrace();
            }

            return null;
        }
        public static String getUser(JSONObject jo)
        {
            try
            {
                return (String)jo.get("User");
            }
            catch (JSONException e)
            {
                e.printStackTrace();
            }

            return null;
        }
        public static String[] clientGetUser(JSONObject jo)
        {
            try
            {
                return (String[])jo.get("User");
            }
            catch (JSONException e)
            {
                e.printStackTrace();
            }

            return null;
        }
        public static Object getVideo(JSONObject jo)
        {
            try
            {
                return jo.get("Video");
            }
            catch (JSONException e)
            {
                e.printStackTrace();
            }

            return null;
        }
        public static Object[] clientGetVideo(JSONObject jo)
        {
            try
            {
                return (Object[])jo.get("Video");
            }
            catch (JSONException e)
            {
                e.printStackTrace();
            }

            return null;
        }
        public static int getID(JSONObject jo)
        {
            try
            {
                return jo.getInt("ID");
            }
            catch (JSONException e)
            {
                e.printStackTrace();
            }

            return 0;
        }
        public static Integer[] clientGetID(JSONObject jo)
        {
            try
            {
                JSONArray jsonArray = (JSONArray)jo.get("ID");
                List<Integer> list = new ArrayList<Integer>();
                for (int i = 0; i < jsonArray.length(); i++)
                {
                    list.add(jsonArray.getInt(i));
                }
                Integer[] arrayInt = list.toArray(new Integer[list.size()]);
                return arrayInt;
            }
            catch (JSONException e)
            {
                e.printStackTrace();
            }

            return null;
        }
        public static String getDate(JSONObject jo)
        {
            try
            {
                return (String)jo.get("Date");
            }
            catch (JSONException e)
            {
                e.printStackTrace();
            }

            return null;
        }
        public static String[] clientGetDate(JSONObject jo)
        {
            try
            {
                JSONArray jsonArray = (JSONArray)jo.get("Date");
                List<String> list = new ArrayList<String>();
                for (int i = 0; i < jsonArray.length(); i++)
                {
                    list.add(jsonArray.getString(i));
                }
                String[] arrayString = list.toArray(new String[list.size()]);
                return arrayString;
            }
            catch (JSONException e)
            {
                e.printStackTrace();
            }

            return null;
        }
        public static String[] getTags(JSONObject jo)
        {
            try
            {
                return (String[])jo.get("Tags");
            }
            catch (JSONException e)
            {
                e.printStackTrace();
            }

            return null;
        }

        public static String[][] clientGetTags(JSONObject jo)
        {
            try
            {
                return (String[][])jo.get("Tags");
            }
            catch (JSONException e)
            {
                e.printStackTrace();
            }

            return null;
        }

        public static Integer[] clientGetRating(JSONObject jo)
        {
            try
            {
                JSONArray jsonArray = (JSONArray)jo.get("Rating");
                List<Integer> list = new ArrayList<Integer>();
                for (int i = 0; i < jsonArray.length(); i++)
                {
                    list.add(jsonArray.getInt(i));
                }
                Integer[] arrayInt = list.toArray(new Integer[list.size()]);
                return arrayInt;
            }
            catch (JSONException e)
            {
                e.printStackTrace();
            }

            return null;
        }

        public static String getText(JSONObject jo)
        {
            try
            {
                return (String)jo.get("Text");
            }
            catch (JSONException e)
            {
                e.printStackTrace();
            }

            return null;
        }

        public static Double[] clientGetLon(JSONObject jo)
        {
            try
            {
                return (Double[])jo.get("Lon");
            }
            catch (JSONException e)
            {
                e.printStackTrace();
            }

            return null;
        }

        public static Double[] clientGetLat(JSONObject jo)
        {
            try
            {
                return (Double[])jo.get("Lat");
            }
            catch (JSONException e)
            {
                e.printStackTrace();
            }

            return null;
        }
        public static String getComment(JSONObject jo)
        {
            try
            {
                return (String)jo.get("Comment");
            }
            catch (JSONException e)
            {
                e.printStackTrace();
            }

            return null;
        }
        public static String[] getComments(JSONObject jo)
        {
            try
            {
                JSONArray jsonArray = (JSONArray)jo.get("Comment");
                List<String> list = new ArrayList<String>();
                for (int i = 0; i < jsonArray.length(); i++)
                {
                    list.add(jsonArray.getString(i));
                }
                String[] arrayString = list.toArray(new String[list.size()]);
                return arrayString;
            }
            catch (JSONException e)
            {
                e.printStackTrace();
            }

            return null;
        }
        public static int getVote(JSONObject jo)
        {
            try
            {
                return jo.getInt("Vote");
            }
            catch (JSONException e)
            {
                e.printStackTrace();
            }

            return 0;
        }
        public static int getPreviousVote(JSONObject jo)
        {
            try
            {
                return jo.getInt("Prev");
            }
            catch (JSONException e)
            {
                e.printStackTrace();
            }

            return 0;
        }

        //voting messages and flags
        public static JSONObject vote(String User, int id, int prev, int vote)
        {
            JSONObject jo = new JSONObject();

            try
            {
                jo.put("POST", 1);
                jo.put("Vote", vote);
                jo.put("ID", id);
                jo.put("Prev", prev);
                jo.put("User", User);
            }
            catch (JSONException e)
            {
                e.printStackTrace();
            }

            return jo;
        }

        public static JSONObject flag(String User, int id)
        {
            JSONObject jo = new JSONObject();

            try
            {
                jo.put("POST", 1);
                jo.put("Flag", "");
                jo.put("ID", id);
                jo.put("User", User);
            }
            catch (JSONException e)
            {
                e.printStackTrace();
            }

            return jo;
        }

        //requests

        public static int getSort(JSONObject jo)
        {
            try
            {
                return ((Integer)jo.get("Sort")).intValue();
            }
            catch (JSONException e)
            {
                e.printStackTrace();
            }
            return 0;
        }*/
    }

}

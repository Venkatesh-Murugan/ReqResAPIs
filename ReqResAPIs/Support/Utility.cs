namespace ReqResAPIs.Support;

public class Utility
 {
    public static string GenerateString()
    {
        Random res = new Random();

        string str = "abcdefghijklmnopqrstuvwxyz";
        int size = 6;

        string randomString = "";

        for (int i = 0; i < size; i++)
        {
            int x = res.Next(26);
            randomString = randomString + str[x];
        }
        return randomString;
    }
}
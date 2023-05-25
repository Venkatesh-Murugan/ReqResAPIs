namespace ReqResAPIs.Support;

public class Utility
 {
    public static string GenerateString(int size)
    {
        Random res = new Random();

        string str = "abcdefghijklmnopqrstuvwxyz";

        string randomString = "";

        for (int i = 0; i < size; i++)
        {
            int x = res.Next(26);
            randomString = randomString + str[x];
        }
        return randomString;
    }
}
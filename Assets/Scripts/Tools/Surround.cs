public class Surround
{
    public float[] FindAngles(float basicAngle, int numberOfAngles)
    {
        float angle = 360f / numberOfAngles;

        float[] directionsAngles = new float[numberOfAngles];
        for(int i = 0; i < numberOfAngles; i++)
        {
            directionsAngles[i] = basicAngle + angle * i;
        }

        return directionsAngles;
    }
}

public static double Calculate(string userInput)
{
    double[] result = userInput.Split().Select(double.Parse).ToArray();
    double nominal = result[0], percent = result[1], time = result[2];
    var a = nominal * Math.Pow(1 + ((percent / 100)/12), time); 
    return a; 	
}
  
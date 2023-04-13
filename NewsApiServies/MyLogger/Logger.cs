

 namespace Services.MyLogger
{
    public class Logger : Ilogger
    {
        public void log(string message)
        {
            using (StreamWriter streamWriter = new StreamWriter("log.txt",true))
            {
              
                streamWriter.WriteLine(message);
            }
           
        }

        public void log(string message, String messagetype)
        {
            log($"{messagetype} : {message}");
        }
    }
}

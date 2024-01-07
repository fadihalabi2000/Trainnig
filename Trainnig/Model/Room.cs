

namespace TrainnigApI.Model
{
    public class Room: BaseNormalEntity
    {
       
        public string? Name { get; set; }
        public int CenterId { get; set; }
      //  public Center? Center { get; set; }
        public int Capacity { get; set; }
      
    }

}

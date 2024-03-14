
using System.ComponentModel.DataAnnotations;

namespace TrainnigApI.Model
{
  public class BaseNormalEntity :IBaseNormalEntity
    {
        [Key]
        public int ID { get; set; }
    }
}

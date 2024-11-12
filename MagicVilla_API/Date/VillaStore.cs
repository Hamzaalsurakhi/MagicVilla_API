using MagicVilla_API.Models.DTO;

namespace MagicVilla_API.Date
{
    public class VillaStore
    {
        public static List<VillaDTO> viilaList = new List<VillaDTO>
        {
            new VillaDTO { Id=1, Name="Pool View",Sqft=100,Occupancy=4 },
            new VillaDTO { Id=2, Name="Beach View",Sqft=300,Occupancy=3 }
        };
    }
}

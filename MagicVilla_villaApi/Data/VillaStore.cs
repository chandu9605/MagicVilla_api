using MagicVilla_villaApi.Models.Dto;

namespace MagicVilla_villaApi.Data
{
    public static class VillaStore
    {
        public static List<VillaDto> villaList = new List<VillaDto> {
            new VillaDto {Id=1,Name="Pool View",SqFt=100,Occupancy=4 },
            new VillaDto { Id=2,Name="Beach View",SqFt=300,Occupancy=3}
            };
    }
}

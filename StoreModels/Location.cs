namespace StoreModels
{
    /// <summary>
    /// This class should contain all the fields and properties that define a store location.
    /// </summary>
    public class Location
    {
        public int? LocationID { get; set; }
        public string Address { get; set; }
        public string LocationName { get; set; }
        //TODO: add some property for the location inventory
    }
}
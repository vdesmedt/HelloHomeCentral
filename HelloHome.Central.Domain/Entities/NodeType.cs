namespace HelloHome.Central.Domain.Entities
{
    public enum NodeType 
    {
        HelloNergie = 1,    //Hal and Dry Sensor + SI7021
        HelloJulo = 2,      //Weather station 
        HelloRelay = 3,     //Control x relays and measure current through them + x push buttons + optional SI7021
        HelloSwitch = 4,    //4 push button + optional LCD
    }
}